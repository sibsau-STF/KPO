using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;

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

	struct ImageFile
		{
		public Bitmap Image;
		public string Name;
		public TimeSpan ProceedTime;

		public ImageFile (Bitmap img, string name)
			{
			Image = img;
			Name = name;
			ProceedTime = new TimeSpan(0);
			}
		}

	struct DBRow
		{
		public string Name;
		public string Path;
		public DateTime Saved;
		public int ProceedTime;

		public DBRow (string name, string path, TimeSpan time)
			{
			Name = name;
			Path = path;
			Saved = DateTime.Now;
			ProceedTime = (int)time.TotalMilliseconds;
			}
		}

	class VideoRecorder
		{
		public event Action ProceedingStart;
		public event Action ProceedingEnd;
		public event Action SavingEnd;
		public event Action<DBRow> InsertDB;

		public bool isRecording { get; protected set; } = false;
		public bool isStopped { get; protected set; } = true;
		public bool isProceeding { get; protected set; } = false;
		public bool isBusy { get; protected set; } = false;

		ConcurrentQueue<ImageFile> sourceFiles;
		ConcurrentQueue<ImageFile> filteredFiles;
		ConcurrentQueue<ImageFile> dbRequests = new ConcurrentQueue<ImageFile>();
		Random rand = new Random();


		VideoFilter Filter;
		ImageResolution Resolution;
		string savePath;
		VideoReceiver Reciever;

		int AvailableWorkers;
		List<Task> Workers;
		DateTime RecordTime;

		public VideoRecorder (VideoReceiver reciever)
			{
			Reciever = reciever;
			}

		public void Record (VideoFilter filter, ImageResolution size, int workersCount, string savePath)
			{
			Filter = filter;
			Resolution = size;
			AvailableWorkers = workersCount;
			this.savePath = savePath;

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
			isStopped = true;
			isRecording = false;
			planWork();
			}

		private Task AcceptDBRequests (Control actor)
			{
			ImageFile file = new ImageFile();
			string path;
			isBusy = false;
			while ( true )
				{
				isBusy = !dbRequests.IsEmpty || isProceeding;
				// TODO: при переключении isBudy надо вызвать событие и сохранять savePath
				if ( isBusy )
					{
					while ( !dbRequests.IsEmpty )
						{
						while ( dbRequests.TryDequeue(out file) )
							Thread.Sleep(rand.Next(10, 80));
						var newRow = new DBRow(file.Name, savePath, file.ProceedTime);
						InsertDB?.Invoke(newRow);
						// INSERT here
						}
					}
				else
					{
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
					Workers.Add(Task.Factory.StartNew(applyFilter));
					freeWorkers--;
					}

				// resize images
				if ( !filteredFiles.IsEmpty && freeWorkers > 0 )
					{
					Workers.Add(Task.Factory.StartNew(applyResizing));
					freeWorkers--;
					}
				}
			Workers.Clear();
			}

		private void saveFrame (Bitmap img)
			{
			if ( isRecording )
				sourceFiles.Enqueue(new ImageFile(img, RecordTime.ToString() + "_" + sourceFiles.Count));
			}
		}
	}
