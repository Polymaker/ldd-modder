using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace LDDModder.IO
{
    public class StreamPortion : Stream
    {
        private Stream sourceStream;
        private long sourceOffset;
        private long portionLength;
        private bool disposeSource;

        public override bool CanRead
        {
            get { return sourceStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return sourceStream.CanSeek; }
        }

        public override bool CanTimeout
        {
            get { return sourceStream.CanTimeout; }
        }

        public override bool CanWrite
        {
            get { return sourceStream.CanWrite; }
        }

        public override long Length
        {
            get { return portionLength; }
        }

        public override int ReadTimeout
        {
            get
            {
                return sourceStream.ReadTimeout;
            }
            set
            {
                sourceStream.ReadTimeout = value;
            }
        }

        public override long Position
        {
            get
            {
                EnsureInPortion();
                return sourceStream.Position - sourceOffset;
            }
            set
            {
                sourceStream.Position = sourceOffset + Math.Min(value, portionLength);
            }
        }

        public StreamPortion(Stream source, long offset, long length)
        {
            sourceStream = source;
            sourceOffset = offset;
            portionLength = length;
            disposeSource = false;
            sourceStream.Seek(offset, SeekOrigin.Begin);
        }

        public StreamPortion(Stream source, long offset, long length, bool disposeSource)
        {
            sourceStream = source;
            sourceOffset = offset;
            portionLength = length;
            this.disposeSource = disposeSource;
            sourceStream.Seek(offset, SeekOrigin.Begin);
        }

        public override void Close()
        {
            //sourceStream.Close();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            EnsureInPortion();
            long remainingSize = Length - Position;
            if (remainingSize <= 0)
                return 0;
            return sourceStream.Read(buffer, offset, (int)Math.Min(count, remainingSize));
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            EnsureInPortion();
            if (origin == SeekOrigin.Begin)
                sourceStream.Seek(sourceOffset + offset, SeekOrigin.Begin);
            else if (origin == SeekOrigin.End)
                sourceStream.Seek(sourceOffset + portionLength - offset, SeekOrigin.Begin);
            else
                sourceStream.Seek(offset, SeekOrigin.Current);
            return Position;
        }

        private void EnsureInPortion()
        {
            long currentPos = sourceStream.Position - sourceOffset;
            if (currentPos < 0)//operations on source stream
                Position = 0;//reposition
            else if (currentPos > portionLength)//operations on source stream
                Position = portionLength;//reposition
        }

        public override void Flush()
        {
            sourceStream.Flush();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            EnsureInPortion();
            sourceStream.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposeSource)
                sourceStream.Dispose();
        }
    }
}
