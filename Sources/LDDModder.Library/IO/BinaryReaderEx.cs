using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LDDModder.IO
{
    public class BinaryReaderEx : BinaryReader
    {
        public static Endianness SystemEndian => BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;

        private Encoding _Encoding;
        public Endianness DefaultEndian { get; set; }

        public Encoding Encoding
        {
            get => _Encoding;
            set
            {
                if (value != _Encoding)
                    ChangeEncoding(value);
            }
        }

        public BinaryReaderEx(Stream input) : base(input)
        {
            DefaultEndian = SystemEndian;
            _Encoding = Encoding.UTF8;
        }

        public BinaryReaderEx(Stream input, Encoding encoding) : base(input, encoding)
        {
            DefaultEndian = SystemEndian;
            _Encoding = encoding;
        }

        public BinaryReaderEx(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
        {
            DefaultEndian = SystemEndian;
            _Encoding = encoding;
        }

        #region ReadBytes

        public override byte[] ReadBytes(int count)
        {
            return ReadBytes(count, DefaultEndian);
        }

        public byte[] ReadBytes(int count, Endianness endianness)
        {
            if (endianness == SystemEndian)
                return base.ReadBytes(count);
            
            return Reverse(base.ReadBytes(count));
        }

        #endregion

        #region Read Int/Uint 16/32/64

        public override short ReadInt16() => ReadInt16(DefaultEndian);

        public override ushort ReadUInt16() => ReadUInt16(DefaultEndian);

        public override int ReadInt32() => ReadInt32(DefaultEndian);

        public override uint ReadUInt32() => ReadUInt32(DefaultEndian);

        public override long ReadInt64() => ReadInt64(DefaultEndian);

        public override ulong ReadUInt64() => ReadUInt64(DefaultEndian);

        public override float ReadSingle() => ReadSingle(DefaultEndian);

        public override double ReadDouble() => ReadDouble(DefaultEndian);

        public short ReadInt16(Endianness endianness)
        {
            if (endianness == SystemEndian)
                return base.ReadInt16();

            return BitConverter.ToInt16(GetBytes<short>(endianness), 0);
        }

        public int ReadInt32(Endianness endianness)
        {
            if (endianness == SystemEndian)
                return base.ReadInt32();

            return BitConverter.ToInt32(GetBytes<int>(endianness), 0);
        }

        public long ReadInt64(Endianness endianness)
        {
            if (endianness == SystemEndian)
                return base.ReadInt64();

            return BitConverter.ToInt64(GetBytes<long>(endianness), 0);
        }

        public ushort ReadUInt16(Endianness endianness)
        {
            if (endianness == SystemEndian)
                return base.ReadUInt16();

            return BitConverter.ToUInt16(GetBytes<ushort>(endianness), 0);
        }

        public uint ReadUInt32(Endianness endianness)
        {
            if (endianness == SystemEndian)
                return base.ReadUInt32();

            return BitConverter.ToUInt32(GetBytes<uint>(endianness), 0);
        }

        public ulong ReadUInt64(Endianness endianness)
        {
            if (endianness == SystemEndian)
                return base.ReadUInt64();
            return BitConverter.ToUInt64(GetBytes<ulong>(endianness), 0);
        }

        public float ReadSingle(Endianness endianness)
        {
            if (endianness == SystemEndian)
                return base.ReadSingle();
            return BitConverter.ToSingle(GetBytes<float>(endianness), 0);
        }

        public double ReadDouble(Endianness endianness)
        {
            if (endianness == SystemEndian)
                return base.ReadDouble();
            return BitConverter.ToDouble(GetBytes<double>(endianness), 0);
        }

        #endregion

        public string ReadString(int length)
        {
            return new string(ReadChars(length));
        }

        public string ReadString(int length, Encoding encoding)
        {
            var currentEncoding = Encoding;

            try
            {
                ChangeEncoding(encoding);
                return new string(ReadChars(length));
            }
            catch
            {
                throw;
            }
            finally
            {
                ChangeEncoding(currentEncoding);
            }
        }

        public override char ReadChar() => ReadChar(DefaultEndian);

        public char ReadChar(Endianness endianness)
        {
            if (endianness == SystemEndian || Encoding != Encoding.Unicode)
                return base.ReadChar();

            //char result = base.ReadChar();

            return (char)ReadInt16(endianness);
        }

        public string ReadNullTerminatedString()
        {
            string str = "";
            char ch;
            while ((ch = ReadChar()) != 0)
                str += ch;

            return str;
        }

        public string ReadNullTerminatedString(Encoding encoding)
        {
            var currentEncoding = Encoding;
            
            try
            {
                ChangeEncoding(encoding);
                return ReadNullTerminatedString();
            }
            catch
            {
                throw;
            }
            finally
            {
                ChangeEncoding(currentEncoding);
            }
        }

        private void ChangeEncoding(Encoding encoding)
        {
            if (encoding != Encoding)
            {
                var f1 = typeof(BinaryReader).GetField("m_decoder", BindingFlags.NonPublic | BindingFlags.Instance);
                var f2 = typeof(BinaryReader).GetField("m_maxCharsSize", BindingFlags.NonPublic | BindingFlags.Instance);
                var f3 = typeof(BinaryReader).GetField("m_buffer", BindingFlags.NonPublic | BindingFlags.Instance);
                var f4 = typeof(BinaryReader).GetField("m_2BytesPerChar", BindingFlags.NonPublic | BindingFlags.Instance);

                f1.SetValue(this, encoding.GetDecoder());
                f2.SetValue(this, encoding.GetMaxCharCount(128));
                int num = encoding.GetMaxByteCount(1);
                if (num < 16)
                    num = 16;
                f3.SetValue(this, new byte[num]);
                f4.SetValue(this, (encoding is UnicodeEncoding));

                _Encoding = encoding;
            }
        }

        private byte[] GetBytes<T>(Endianness endianness) where T : struct
        {
            return ReadBytes(Marshal.SizeOf<T>(), endianness);
        }

        public static byte[] Reverse(byte[] b)
        {
            Array.Reverse(b);
            return b;
        }

        public T ReadStruct<T>() where T : struct
        {
            return ReadStruct<T>(DefaultEndian);
        }

        public T ReadStruct<T>(Endianness endianness) where T : struct
        {
            object structObj = default(T);

            foreach (var fieldInfo in typeof(T).GetFields())
            {
                object convertedValue = null;
                if (fieldInfo.FieldType == typeof(byte))
                    convertedValue = ReadByte();
                else if (fieldInfo.FieldType == typeof(short))
                    convertedValue = ReadInt16(endianness);
                else if (fieldInfo.FieldType == typeof(ushort))
                    convertedValue = ReadUInt16(endianness);
                else if (fieldInfo.FieldType == typeof(int))
                    convertedValue = ReadInt32(endianness);
                else if (fieldInfo.FieldType == typeof(uint))
                    convertedValue = ReadUInt32(endianness);
                else if (fieldInfo.FieldType == typeof(long))
                    convertedValue = ReadInt64(endianness);
                else if (fieldInfo.FieldType == typeof(ulong))
                    convertedValue = ReadUInt64(endianness);
                else if (fieldInfo.FieldType == typeof(float))
                    convertedValue = ReadSingle(endianness);
                else if (fieldInfo.FieldType == typeof(double))
                    convertedValue = ReadDouble(endianness);
                else if (fieldInfo.FieldType == typeof(string))
                {
                    var marshalAttr = fieldInfo.GetCustomAttribute<MarshalAsAttribute>();
                    var charsetAttr = fieldInfo.GetCustomAttribute<EncodingAttribute>();
                    Encoding encoding = Encoding;

                    if (charsetAttr?.CharSet == CharSet.Ansi)
                        encoding = Encoding.Default;
                    if (charsetAttr?.CharSet == CharSet.Unicode)
                        encoding = Encoding.Unicode;

                    if (marshalAttr != null)
                        convertedValue = ReadString(marshalAttr.SizeConst, encoding);
                    else
                        convertedValue = ReadNullTerminatedString(encoding);
                }
                else if (fieldInfo.FieldType == typeof(byte[]))
                {
                    var marshalAttr = fieldInfo.GetCustomAttribute<MarshalAsAttribute>();
                    if (marshalAttr != null)
                        convertedValue = base.ReadBytes(marshalAttr.SizeConst);//if we want a byte array, we surely want it in the same order as they are in the file
                    else
                        throw new InvalidOperationException("A MarshalAsAttribute is required with SizeConst defined");
                }
                else if (fieldInfo.FieldType.IsValueType)
                {
                    int structSize = Marshal.SizeOf(fieldInfo.FieldType);
                    var bytes = ReadBytes(structSize, endianness);
                    var tmpPtr = Marshal.AllocHGlobal(structSize);
                    Marshal.Copy(bytes, 0, tmpPtr, structSize);
                    convertedValue = Marshal.PtrToStructure(tmpPtr, fieldInfo.FieldType);
                    Marshal.FreeHGlobal(tmpPtr);
                }
                else
                    continue;

                fieldInfo.SetValue(structObj, convertedValue);
            }

            return (T)structObj;
        }
    }
}
