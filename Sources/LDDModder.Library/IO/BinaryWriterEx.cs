using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace LDDModder.IO
{
    public class BinaryWriterEx : BinaryWriter
    {
        public static Endianness SystemEndian => BitConverter.IsLittleEndian ? Endianness.LittleEndian : Endianness.BigEndian;

        private Encoding _Encoding;
        public Endianness DefaultEndian { get; set; }
        public StringMarshalingMode DefaultStringMarshaling { get; set; } = StringMarshalingMode.FixedLength;

        public Encoding Encoding
        {
            get => _Encoding;
            set
            {
                //if (value != _Encoding)
                //    ChangeEncoding(value);
            }
        }

        public BinaryWriterEx(Stream output) : this(output, Encoding.UTF8, false)
        {
        }

        public BinaryWriterEx(Stream output, bool leaveOpen) : this(output, Encoding.UTF8, leaveOpen)
        {
        }

        public BinaryWriterEx(Stream output, Encoding encoding) : base(output, encoding)
        {
            _Encoding = encoding;
            DefaultEndian = SystemEndian;
        }

        public BinaryWriterEx(Stream output, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
        {
            _Encoding = encoding;
            DefaultEndian = SystemEndian;
        }

        #region Write by type

        public void WriteInt16(short value)
        {
            WriteInt16(value, DefaultEndian);
        }

        public void WriteInt16(short value, Endianness endianness)
        {
            if (endianness == SystemEndian)
                base.Write(value);
            var bytes = BitConverter.GetBytes(value);
            WriteBytes(Reverse(bytes));
        }

        public void WriteUInt16(ushort value)
        {
            WriteUInt16(value, DefaultEndian);
        }

        public void WriteUInt16(ushort value, Endianness endianness)
        {
            if (endianness == SystemEndian)
                base.Write(value);
            var bytes = BitConverter.GetBytes(value);
            WriteBytes(Reverse(bytes));
        }

        public void WriteInt32(int value)
        {
            WriteInt32(value, DefaultEndian);
        }

        public void WriteInt32(int value, Endianness endianness)
        {
            if (endianness == SystemEndian)
                base.Write(value);
            var bytes = BitConverter.GetBytes(value);
            WriteBytes(Reverse(bytes));
        }

        public void WriteUInt32(uint value)
        {
            WriteUInt32(value, DefaultEndian);
        }

        public void WriteUInt32(uint value, Endianness endianness)
        {
            if (endianness == SystemEndian)
                base.Write(value);
            var bytes = BitConverter.GetBytes(value);
            WriteBytes(Reverse(bytes));
        }

        public void WriteInt64(long value)
        {
            WriteInt64(value, DefaultEndian);
        }

        public void WriteInt64(long value, Endianness endianness)
        {
            if (endianness == SystemEndian)
                base.Write(value);
            var bytes = BitConverter.GetBytes(value);
            WriteBytes(Reverse(bytes));
        }

        public void WriteUInt64(ulong value)
        {
            WriteUInt64(value, DefaultEndian);
        }

        public void WriteUInt64(ulong value, Endianness endianness)
        {
            if (endianness == SystemEndian)
                base.Write(value);
            var bytes = BitConverter.GetBytes(value);
            WriteBytes(Reverse(bytes));
        }

        public void WriteSingle(float value)
        {
            WriteSingle(value, DefaultEndian);
        }

        public void WriteSingle(float value, Endianness endianness)
        {
            if (endianness == SystemEndian)
                base.Write(value);
            var bytes = BitConverter.GetBytes(value);
            WriteBytes(Reverse(bytes));
        }

        public void WriteDouble(double value)
        {
            WriteDouble(value, DefaultEndian);
        }

        public void WriteDouble(double value, Endianness endianness)
        {
            if (endianness == SystemEndian)
                base.Write(value);
            var bytes = BitConverter.GetBytes(value);
            WriteBytes(Reverse(bytes));
        }

        public void WriteDecimal(decimal value)
        {
            WriteDecimal(value, DefaultEndian);
        }

        public void WriteDecimal(decimal value, Endianness endianness)
        {
            if (endianness == SystemEndian)
                base.Write(value);
            var parts = decimal.GetBits(value);
            var bytes = new byte[16];
            bytes[0] = (byte)parts[0];
            bytes[1] = (byte)(parts[0] >> 8);
            bytes[2] = (byte)(parts[0] >> 16);
            bytes[3] = (byte)(parts[0] >> 24);
            bytes[4] = (byte)parts[1];
            bytes[5] = (byte)(parts[1] >> 8);
            bytes[6] = (byte)(parts[1] >> 16);
            bytes[7] = (byte)(parts[1] >> 24);
            bytes[8] = (byte)parts[2];
            bytes[9] = (byte)(parts[2] >> 8);
            bytes[10] = (byte)(parts[2] >> 16);
            bytes[11] = (byte)(parts[2] >> 24);
            bytes[12] = (byte)parts[3];
            bytes[13] = (byte)(parts[3] >> 8);
            bytes[14] = (byte)(parts[3] >> 16);
            bytes[15] = (byte)(parts[3] >> 24);
            WriteBytes(Reverse(bytes));
        }

        public void WriteString(string text, StringMarshalingMode stringType)
        {
            WriteString(text, Encoding, stringType);
        }

        public void WriteString(string text, Encoding encoding, StringMarshalingMode stringType)
        {
            byte[] textBytes = new byte[0];

            if (text != null)
            {
                textBytes = encoding.GetBytes(text);
                if (encoding == Encoding.Unicode && DefaultEndian != SystemEndian)
                {
                    for (int i = 0; i < textBytes.Length; i += 2)
                    {
                        byte tmp = textBytes[i];
                        textBytes[i] = textBytes[i + 1];
                        textBytes[i + 1] = tmp;
                    }
                }
            }

            if (stringType == StringMarshalingMode.NullTerminated)
            {
                var nullBytes = encoding.GetBytes(new char[] { '\0' });
                if (textBytes.Length > 0)
                    WriteBytes(textBytes);
                WriteBytes(nullBytes);
            }
            else
            {
                WriteInt32(text.Length);
                if (textBytes.Length > 0)
                    WriteBytes(textBytes);
            }
        }

        private void GenericWrite(object value, Endianness endianness)
        {
            var objType = value.GetType();
            if (objType == typeof(byte))
                Write((byte)value);
            else if (objType == typeof(bool))
                Write((bool)value);
            else if (objType == typeof(short))
                WriteInt16((short)value, endianness);
            else if (objType == typeof(ushort))
                WriteUInt16((ushort)value, endianness);
            else if (objType == typeof(int))
                WriteInt32((int)value, endianness);
            else if (objType == typeof(uint))
                WriteUInt32((uint)value, endianness);
            else if (objType == typeof(long))
                WriteInt64((long)value, endianness);
            else if (objType == typeof(ulong))
                WriteUInt64((ulong)value, endianness);
            else if (objType == typeof(float))
                WriteSingle((float)value, endianness);
            else if (objType == typeof(double))
                WriteDouble((double)value, endianness);
            else if (objType == typeof(decimal))
                WriteDecimal((decimal)value, endianness);
        }

        private void WriteBytes(byte[] buffer)
        {
            base.Write(buffer);
        }

        #endregion

        #region Write overrides

        public override void Write(short value)
        {
            WriteInt16(value);
        }

        public override void Write(ushort value)
        {
            WriteUInt16(value);
        }

        public override void Write(int value)
        {
            WriteInt32(value);
        }

        public override void Write(uint value)
        {
            WriteUInt32(value);
        }

        public override void Write(long value)
        {
            WriteInt64(value);
        }

        public override void Write(ulong value)
        {
            WriteUInt64(value);
        }

        public override void Write(float value)
        {
            WriteSingle(value);
        }

        public override void Write(double value)
        {
            WriteDouble(value);
        }

        #endregion

        #region Struct writing

        public void WriteStruct<T>(T item) where T : struct
        {
            WriteStruct(item, DefaultEndian);
        }

        public void WriteStruct<T>(T item, Endianness endianness) where T : struct
        {
            foreach (var fieldInfo in typeof(T).GetFields())
            {
                if (fieldInfo.FieldType == typeof(string))
                {
                    var marshalAttr = fieldInfo.GetCustomAttribute<MarshalAsAttribute>();
                    var charsetAttr = fieldInfo.GetCustomAttribute<EncodingAttribute>();
                    var stringMarshalAttr = fieldInfo.GetCustomAttribute<StringMarshalingAttribute>();

                    var stringMarshaling = DefaultStringMarshaling;
                    if (stringMarshalAttr != null)
                        stringMarshaling = stringMarshalAttr.MarshalingMode;

                    var encoding = Encoding;
                    if (charsetAttr?.CharSet == CharSet.Ansi)
                        encoding = Encoding.Default;
                    if (charsetAttr?.CharSet == CharSet.Unicode)
                        encoding = Encoding.Unicode;
                    var text = (string)fieldInfo.GetValue(item);

                    if (marshalAttr != null && marshalAttr.SizeConst > 0)
                    {
                        stringMarshaling = StringMarshalingMode.FixedLength;
                        if (text != null && text.Length > marshalAttr.SizeConst)
                            text = text.Substring(0, marshalAttr.SizeConst);
                        if (text == null || text.Length < marshalAttr.SizeConst)
                            text = text.PadRight(marshalAttr.SizeConst, (char)0);
                        Write(text.ToArray());
                    }
                    else
                        WriteString(text, encoding, stringMarshaling);
                }
                else if (fieldInfo.FieldType.IsValueType)
                    GenericWrite(fieldInfo.GetValue(item), endianness);
                else
                {

                }
            }
        }

        #endregion

        public static byte[] Reverse(byte[] b)
        {
            Array.Reverse(b);
            return b;
        }
    }
}
