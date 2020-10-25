using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace КПО_ЛР3
	{

	struct ImageResolution
		{
		public int Width { get; private set; }
		public int Height { get; private set; }

		public ImageResolution (int width, int height)
			{
			Width = width;
			Height = height;
			}

		public override string ToString ()
			{
			return $"{Width} x {Height}";
			}
		}

	class VideoRecorder
		{
		public event Action RecordingStart;
		public event Action RecordingEnd;
		public event Action ProceedingStart;
		public event Action ProceedingEnd;
		public event Action SavingStart;
		public event Action SavingEnd;

		public event Action<DBRow> InsertDB;

		public bool isRecording { get; protected set; } = false;
		public bool isStopped { get; protected set; } = true;
		public bool isProceeding { get; protected set; } = false;
		public bool isBusy { get; protected set; } = false;
		bool haveWork => !dbRequests.IsEmpty || isProceeding;

		public List<DBRow> Logs;

		ConcurrentQueue<ImageFile> sourceFiles;
		ConcurrentQueue<ImageFile> filteredFiles;
		ConcurrentQueue<ImageFile> dbRequests = new ConcurrentQueue<ImageFile>();
		Random rand = new Random();

		ImagesDB DB;
		VideoFilter Filter;
		ImageResolution Resolution;
		string savePath;
		VideoReceiver Reciever;

		int AvailableWorkers;
		List<Task> Workers;
		DateTime RecordTime;
		Task background;
		CancellationTokenSource cancelDBSource;

		public VideoRecorder (VideoReceiver reciever)
			{
			Reciever = reciever;
			DB = new ImagesDB();
			Logs = DB.Select();
			}

		public void StartDB (Control control)
			{
			cancelDBSource = new CancellationTokenSource();
			background = Task.Factory.StartNew(delegate (object obj)
				{ AcceptDBRequests((Control)obj); }, control, cancelDBSource.Token);
			}

		public void StopDB ()
			{
			cancelDBSource.Cancel();
			}

		public void Record (VideoFilter filter, ImageResolution size, int workersCount, string savePath)
			{
			Filter = filter;
			Resolution = size;
			AvailableWorkers = workersCount;
			this.savePath = savePath;
			RecordingStart?.Invoke();

			if ( !Directory.Exists(this.savePath) )
				Directory.CreateDirectory(this.savePath);

			Resume();
			RecordTime = DateTime.Now;
			sourceFiles = new ConcurrentQueue<ImageFile>();
			filteredFiles = new ConcurrentQueue<ImageFile>();
			Reciever.ImageRecieved += saveFrame;
			}

		public void Resume ()
			{
			isRecording = true;
			isStopped = false;
			}

		public void Pause ()
			{
			isRecording = false;
			}

		public void Stop ()
			{
			Reciever.ImageRecieved -= saveFrame;
			RecordingEnd?.Invoke();
			isStopped = true;
			isRecording = false;
			planWork();
			}

		private void AcceptDBRequests (Control actor)
			{
			ImageFile file = new ImageFile();

			isBusy = false;

			while ( true )
				{
				if ( !isBusy && haveWork )
					{
					if ( actor.InvokeRequired )
						actor.Invoke(new Action(() =>
							SavingStart?.Invoke()));
					isBusy = true;
					}

				if ( haveWork )
					{
					while ( haveWork )
						{
						 while ( !dbRequests.IsEmpty && !dbRequests.TryDequeue(out file) )
							Thread.Sleep(rand.Next(10, 80));

						if ( file.Image == null )
							break;

						var newRow = new DBRow(file.Name, savePath, file.ProceedTime);
						DB.Insert(newRow);
						file.Save(savePath);

						if ( actor.InvokeRequired )
							actor.Invoke(new Action(() =>
								InsertDB?.Invoke(newRow)));
						}
					}
				else
					{
					// если выполнял работу и новых файлов не ожидается
					if ( isBusy && !isProceeding )
						{
						isBusy = false;
						if ( actor.InvokeRequired )
							actor.Invoke(new Action(() =>
								SavingEnd?.Invoke()));
						}
					Thread.Sleep(2000);
					continue;
					}
				}
			}

		private Bitmap ResizeImage (Bitmap img, ImageResolution res)
			{
			int width = res.Width;
			int height = res.Height;
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(img.HorizontalResolution, img.VerticalResolution);

			using ( var graphics = Graphics.FromImage(destImage) )
				{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				if ( img.Width > width || img.Height > height )
					graphics.InterpolationMode = InterpolationMode.Bicubic;
				else
					graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using ( var wrapMode = new ImageAttributes() )
					{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(img, destRect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, wrapMode);
					}
				}
			return destImage;
			}

		private void applyFilter ()
			{
			ImageFile file = new ImageFile();
			var now = DateTime.Now;
			while ( !sourceFiles.IsEmpty && !sourceFiles.TryDequeue(out file) )
				Thread.Sleep(rand.Next(0, 100));

			var result = Filter.Filter(file.Image);
			var then = DateTime.Now;
			file.Image = result;
			file.ProceedTime += then - now;
			filteredFiles.Enqueue(file);
			}

		private void applyResizing ()
			{
			ImageFile file = new ImageFile();
			var now = DateTime.Now;
			while ( !filteredFiles.IsEmpty && !filteredFiles.TryDequeue(out file) )
				Thread.Sleep(rand.Next(0, 100));

			var result = ResizeImage(file.Image, Resolution);
			var then = DateTime.Now;
			file.ProceedTime += then - now;
			file.Image = result;

			dbRequests.Enqueue(file);
			}

		private async Task planWork ()
			{
			ProceedingStart?.Invoke();
			isProceeding = true;
			int freeWorkers = AvailableWorkers;

			TaskFactory factory = new TaskFactory();
			Workers = new List<Task>();

			// filter images
			while ( !sourceFiles.IsEmpty || !filteredFiles.IsEmpty || Workers.Count > 0 )
				{
				var activeWorkers = Workers.FindAll(tsk => !tsk.IsCompleted);
				if ( freeWorkers != AvailableWorkers )
					{
					// wait for free worker
					await Task.WhenAny(Workers);
					activeWorkers = Workers.FindAll(tsk => !tsk.IsCompleted);
					}
				freeWorkers += Workers.Count - activeWorkers.Count;
				Workers = activeWorkers;

				// filter images
				if ( !sourceFiles.IsEmpty && freeWorkers > 0 )
					{
					Workers.Add(factory.StartNew(applyFilter));
					freeWorkers--;
					}

				// resize images
				if ( !filteredFiles.IsEmpty && freeWorkers > 0 )
					{
					Workers.Add(factory.StartNew(applyResizing));
					freeWorkers--;
					}
				}
			isProceeding = false;
			ProceedingEnd?.Invoke();
			Workers.Clear();
			}

		private void saveFrame (Bitmap img)
			{
			if ( isRecording )
				sourceFiles.Enqueue(new ImageFile(img, RecordTime.ToString() + "_" + sourceFiles.Count));
			}
		}
	}
