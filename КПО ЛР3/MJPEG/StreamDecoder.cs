using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MJPEG
{
    public class StreamDecoder : IStreamDecoder
    {
        private string _uri;

        private HttpClient _client;

        private byte[] _lastFrame;

        private object _locker = new object();

        private bool _updateUri;

        public StreamDecoder()
        {
            _client = new HttpClient();
        }
        Task tsk;
        public void StartDecodingAsync(string uri)
        {
            if (_uri == null)
            {
                _uri = uri;

                tsk = Task.Factory.StartNew(DoWork, TaskCreationOptions.LongRunning);
                //tsk.
            }
            else
            {
                if (uri != _uri)
                {
                    _uri = uri;
                    _updateUri = true;
                }
            }
        }

        public byte[] GetLastFrame() => _lastFrame;

        public delegate void FrameHandler(object sender, FrameReceivedEventArgs e);

        public event FrameHandler OnFrameReceived;

        public void Pause()
		{
            isWorked = false;
		}

        public void Resume()
        {
            isWorked = true;
        }

        public void Stop()
		{
            isWorked = false;
            isCanceled = true;
        }

        bool isWorked = true;
        bool isCanceled = false;
        private async Task DoWork()
        {
            while (true)
            {
                try
                {
                    using (var stream = await _client.GetStreamAsync(_uri).ConfigureAwait(false))
                    {
                        while (isWorked)
                        {
                            if (isCanceled) break;

                            int contentLength = GetContentLength(stream);

                            if (contentLength == 0) break;

                            var content = GetContent(stream, contentLength);

                            if (content == null) break;

                            if (_updateUri)
                            {
                                _updateUri = false;
                                break;
                            }

                            lock (_locker)
                            {
                                _lastFrame = content;
                            }

                            OnFrameReceived?.Invoke(this, new FrameReceivedEventArgs()
                            {
                                Frame = content
                            });
                        }
                    }
                    if (isCanceled)
                        return; 
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                }
            }
        }

        internal byte[] SliceStream(Stream stream, byte[] beginPattern, byte[] endPattern)
        {
            int offset = 0;
            bool beginCathed = false;

            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int x = stream.ReadByte();

                    if (x == -1) return null;

                    if (!beginCathed)
                    {
                        if (x == beginPattern[offset])
                        {
                            offset++;

                            ms.WriteByte((byte)x);

                            if (offset == beginPattern.Length)
                            {
                                beginCathed = true;
                                offset = 0;
                            }
                        }
                        else
                        {
                            ms.SetLength(0);
                            offset = 0;
                        }
                    }
                    else
                    {
                        ms.WriteByte((byte)x);

                        if (x == endPattern[offset])
                        {
                            offset++;

                            if (offset == endPattern.Length)
                            {
                                return ms.ToArray();
                            }
                        }
                        else
                        {
                            offset = 0;
                        }
                    }
                }
            }
        }

        internal int GetContentLength(Stream stream)
        {
            int res = 0;

            // Get header which starts with "--my" and ends with "\r\n\r\n"
            var header = SliceStream(stream, new byte[] { 0x2D, 0x2D, 0x6D, 0x79 }, new byte[] { 0x0D, 0x0A, 0x0D, 0x0A });

            if (header == null || header.Length < 7)
            {
                return 0;
            }

            for (int i = header.Length - 5; i >= 0; i--)
            {
                if (header[i] == 0x20)
                {
                    string s = Encoding.UTF8.GetString(header, i, header.Length - i - 4);

                    if (!int.TryParse(s, out res))
                    {
                        return 0;
                    }

                    break;
                }
            }

            return res;
        }

        internal byte[] GetContent(Stream stream, int contentLength)
        {
            int bytesProcessed = 0;

            byte[] res = new byte[contentLength];

            while (bytesProcessed != contentLength)
            {
                int bytesReceived = stream.Read(res, bytesProcessed, contentLength - bytesProcessed);

                if (bytesReceived == 0) return null;

                bytesProcessed += bytesReceived;
            }

            return res;
        }

    }
}
