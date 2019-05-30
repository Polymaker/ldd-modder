using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.IO
{
    public class StreamPortion : Stream
    {
        private readonly Stream sourceStream;
        private readonly long sourceOffset;
        private readonly long portionLength;
        private readonly bool keepOpen;
        private bool forceReadOnly;
        private long localPosition;

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
            get { return sourceStream.CanWrite && !forceReadOnly; }
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
            get => localPosition;
            set
            {
                if (value < 0 || value > Length)
                    throw new IndexOutOfRangeException();
                localPosition = value;
            }
        }


        public StreamPortion(Stream sourceStream, long sourceOffset, long portionLength, bool keepOpen)
        {
            this.sourceStream = sourceStream;
            this.sourceOffset = sourceOffset;
            this.portionLength = portionLength;
            this.keepOpen = keepOpen;
        }

        public StreamPortion(Stream sourceStream, long sourceOffset, long portionLength)
            : this(sourceStream, sourceOffset, portionLength, true)
        {
            
        }

        public StreamPortion(Stream sourceStream, long sourceOffset, long portionLength, bool keepOpen, bool forceReadOnly) : this(sourceStream, sourceOffset, portionLength, keepOpen)
        {
            this.forceReadOnly = forceReadOnly;
        }

        protected override void Dispose(bool disposing)
        {
            if (!keepOpen)
                sourceStream.Dispose();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            long oldPos = sourceStream.Position;
            try
            {
                sourceStream.Position = sourceOffset + localPosition;
                if (count > Length - localPosition)
                    count = (int)(Length - localPosition);
                int byteRead = sourceStream.Read(buffer, offset, count);
                localPosition += byteRead;
                return byteRead;
            }
            finally
            {
                sourceStream.Position = oldPos;
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
                localPosition = offset;
            else if (origin == SeekOrigin.End)
                localPosition = Length + offset;
            else
                localPosition += offset;

            if (localPosition < 0)
                localPosition = 0;
            if (localPosition > Length)
                localPosition = Length;
            
            return localPosition;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!CanWrite)
                throw new InvalidOperationException();

            long oldPos = sourceStream.Position;
            try
            {
                sourceStream.Position = sourceOffset + localPosition;
                if (localPosition + count > Length)
                    count = (int)(Length - localPosition);
                sourceStream.Write(buffer, offset, count);
                localPosition = sourceStream.Position - sourceOffset;
            }
            finally
            {
                sourceStream.Position = oldPos;
            }
        }

        public override void Flush()
        {
            sourceStream.Flush();
        }
    }
}
