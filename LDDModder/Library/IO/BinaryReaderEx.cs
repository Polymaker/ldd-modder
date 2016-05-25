using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace LDDModder.IO
{
    public class BinaryReaderEx : BinaryReader
    {
        // Fields...
        private Endians _DefaultEndian;

        public Endians DefaultEndian
        {
            get
            {
                return _DefaultEndian;
            }
            set
            {
                _DefaultEndian = value;
            }
        }

        public BinaryReaderEx(Stream input)
            : base(input)
        {
            _DefaultEndian = BitConverter.IsLittleEndian ? Endians.LittleEndian : Endians.BigEndian;
        }

        public BinaryReaderEx(BinaryReader reader)
            : base(reader.BaseStream)
        {
            _DefaultEndian = BitConverter.IsLittleEndian ? Endians.LittleEndian : Endians.BigEndian;
        }

        public BinaryReaderEx(Stream input, Encoding encoding)
            : base(input, encoding)
        {
            _DefaultEndian = BitConverter.IsLittleEndian ? Endians.LittleEndian : Endians.BigEndian;
        }

        public BinaryReaderEx(Stream input, Endians endianness)
            : base(input)
        {
            _DefaultEndian = endianness;
        }

        public BinaryReaderEx(BinaryReader reader, Endians endianness)
            : base(reader.BaseStream)
        {
            _DefaultEndian = endianness;
        }

        public BinaryReaderEx(Stream input, Encoding encoding, Endians endianness)
            : base(input, encoding)
        {
            _DefaultEndian = endianness;
        }

        public override int ReadInt32()
        {
            return ReadInt32(DefaultEndian);
        }

        public override long ReadInt64()
        {
            return ReadInt64(DefaultEndian);
        }

        public override uint ReadUInt32()
        {
            return ReadUInt32(DefaultEndian);
        }

        public override short ReadInt16()
        {
            return ReadInt16(DefaultEndian);
        }

        public override ushort ReadUInt16()
        {
            return ReadUInt16(DefaultEndian);
        }

        public uint ReadUInt32(Endians edian)
        {
            if (BitConverter.IsLittleEndian && edian == Endians.LittleEndian)
                return base.ReadUInt32();
            return BitConverter.ToUInt32(Reverse(ReadBytesRequired(sizeof(UInt32))), 0);
        }

        public int ReadInt32(Endians edian)
        {
            if (BitConverter.IsLittleEndian && edian == Endians.LittleEndian)
                return base.ReadInt32();
            return BitConverter.ToInt32(Reverse(ReadBytesRequired(sizeof(Int32))), 0);
        }

        public long ReadInt64(Endians edian)
        {
            if (BitConverter.IsLittleEndian && edian == Endians.LittleEndian)
                return base.ReadInt64();
            return BitConverter.ToInt64(Reverse(ReadBytesRequired(sizeof(Int64))), 0);
        }

        public ushort ReadUInt16(Endians edian)
        {
            if (BitConverter.IsLittleEndian && edian == Endians.LittleEndian)
                return base.ReadUInt16();
            return BitConverter.ToUInt16(Reverse(ReadBytesRequired(sizeof(UInt16))), 0);
        }

        public short ReadInt16(Endians edian)
        {
            if (BitConverter.IsLittleEndian && edian == Endians.LittleEndian)
                return base.ReadInt16();
            return BitConverter.ToInt16(Reverse(ReadBytesRequired(sizeof(Int16))), 0);
        }

        public byte[] ReadBytesRequired(int byteCount)
        {
            var result = ReadBytes(BitConverter.IsLittleEndian ? Endians.LittleEndian : Endians.BigEndian, byteCount);

            if (result.Length != byteCount)
                throw new EndOfStreamException(string.Format("{0} bytes required from stream, but only {1} returned.", byteCount, result.Length));

            return result;
        }

        public override byte[] ReadBytes(int count)
        {
            return ReadBytes(DefaultEndian, count);
        }

        public byte[] ReadBytes(Endians edian, int count)
        {
            if (BitConverter.IsLittleEndian && edian == Endians.LittleEndian)
                return base.ReadBytes(count);

            return Reverse(base.ReadBytes(count));
        }

        public static byte[] Reverse(byte[] b)
        {
            Array.Reverse(b);
            return b;
        }
    }

    public enum Endians
    {
        LittleEndian,
        BigEndian
    }
}
