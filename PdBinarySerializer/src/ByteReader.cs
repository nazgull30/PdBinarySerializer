namespace ByteFormatter;

using System;
using System.Collections.Generic;
using System.Text;

public sealed class ByteReader : ICloneable
{
    private readonly ByteBuffer _buffer;

    public ByteReader()
    {
        _buffer = new ByteBuffer();
    }

    public ByteReader(byte[] buffer)
    {
        _buffer = new ByteBuffer(buffer);
    }

    private ByteReader(ByteBuffer buffer)
    {
        _buffer = buffer;
    }

    public long Position => _buffer.Position;

    public int Length => _buffer.Length;

    public void ReInit(byte[] bytes) => _buffer.ReInit(bytes);

    public void Reset() => _buffer.Reset();

    public void SeekZero() => _buffer.SeekZero();

    public void Skip(uint length) => _buffer.Skip(length);

    public void Replace(byte[] buffer) => _buffer.Replace(buffer);

    public byte ReadByte() => _buffer.ReadByte();

    public void SkipByte() => _buffer.Skip(1U);

    public byte? ReadByteNullable() => ReadBoolean() ? ReadByte() : null as byte?;

    public void SkipByteNullable()
    {
        if (ReadBoolean())
            _buffer.Skip(1U);
    }

    public sbyte ReadSByte() => (sbyte)_buffer.ReadByte();

    public void SkipSByte() => _buffer.Skip(1U);

    public sbyte? ReadSByteNullable() => ReadBoolean() ? ReadSByte() : null as sbyte?;

    public void SkipSByteNullable()
    {
        if (ReadBoolean())
            _buffer.Skip(1U);
    }

    public short ReadInt16()
        => (short)(ushort)((ushort)(0U | _buffer.ReadByte()) |
                           (uint)(ushort)((uint)_buffer.ReadByte() << 8));

    public void SkipInt16() => _buffer.Skip(2U);

    public short? ReadInt16Nullable() => ReadBoolean() ? ReadInt16() : null as short?;

    public void SkipInt16Nullable()
    {
        if (ReadBoolean())
            _buffer.Skip(2U);
    }

    public ushort ReadUInt16()
        => (ushort)((ushort)(0U | _buffer.ReadByte()) |
                    (uint)(ushort)((uint)_buffer.ReadByte() << 8));

    public void SkipUInt16() => _buffer.Skip(2U);

    public ushort? ReadUInt16Nullable() => ReadBoolean() ? ReadUInt16() : null as ushort?;

    public void SkipUInt16Nullable()
    {
        if (ReadBoolean())
            _buffer.Skip(2U);
    }

    public int ReadInt32()
        => (int)(0U | _buffer.ReadByte() | ((uint)_buffer.ReadByte() << 8) |
                 ((uint)_buffer.ReadByte() << 16) | ((uint)_buffer.ReadByte() << 24));

    public void SkipInt32() => _buffer.Skip(4U);

    public uint ReadUInt32()
        => 0U | _buffer.ReadByte() | ((uint)_buffer.ReadByte() << 8) |
           ((uint)_buffer.ReadByte() << 16) | ((uint)_buffer.ReadByte() << 24);

    public void SkipUInt32() => _buffer.Skip(4U);

    public uint? ReadUInt32Nullable() => ReadBoolean() ? ReadUInt32() : null as uint?;

    public void SkipUInt32Nullable()
    {
        if (ReadBoolean())
            _buffer.Skip(4U);
    }

    public long ReadInt64()
        => (long)(0UL | _buffer.ReadByte() | ((ulong)_buffer.ReadByte() << 8) |
                  ((ulong)_buffer.ReadByte() << 16) | ((ulong)_buffer.ReadByte() << 24) |
                  ((ulong)_buffer.ReadByte() << 32) | ((ulong)_buffer.ReadByte() << 40) |
                  ((ulong)_buffer.ReadByte() << 48) | ((ulong)_buffer.ReadByte() << 56));

    public void SkipInt64() => _buffer.Skip(8U);

    public long? ReadInt64Nullable()
        => ReadBoolean() ? ReadInt64() : null as long?;

    public void SkipInt64Nullable()
    {
        if (ReadBoolean())
            _buffer.Skip(8U);
    }

    public ulong ReadUInt64()
        => 0UL | _buffer.ReadByte() | ((ulong)_buffer.ReadByte() << 8) |
           ((ulong)_buffer.ReadByte() << 16) | ((ulong)_buffer.ReadByte() << 24) |
           ((ulong)_buffer.ReadByte() << 32) | ((ulong)_buffer.ReadByte() << 40) |
           ((ulong)_buffer.ReadByte() << 48) | ((ulong)_buffer.ReadByte() << 56);


    public void SkipUInt64() => _buffer.Skip(8U);

    public ulong? ReadUInt64Nullable()
        => ReadBoolean() ? ReadUInt64() : null as ulong?;

    public void SkipUInt64Nullable()
    {
        if (ReadBoolean())
            _buffer.Skip(8U);
    }

    public decimal ReadDecimal()
        => new decimal(new int[4]
        {
            ReadInt32(),
            ReadInt32(),
            ReadInt32(),
            ReadInt32()
        });

    public void SkipDecimal() => _buffer.Skip(16U);

    public float ReadSingle()
    {
        var intVal = ReadInt32();
        return BitConverter.ToSingle(BitConverter.GetBytes(intVal), 0);
    }

    public void SkipSingle() => _buffer.Skip(4U);

    public float? ReadSingleNullable()
    {
        var hasValue = ReadBoolean();
        return hasValue ? ReadSingle() : null as float?;
    }

    public void SkipSingleNullable()
    {
        if (ReadBoolean())
            _buffer.Skip(4U);
    }

    public double ReadDouble()
    {
        var longVal = ReadInt64();
        return BitConverter.Int64BitsToDouble(longVal);
    }

    public void SkipDouble() => _buffer.Skip(8U);

    public char ReadChar() => (char)_buffer.ReadByte();

    public void SkipChar() => _buffer.Skip(1U);

    public bool ReadBoolean() => _buffer.ReadByte() == 1;

    public void SkipBoolean() => _buffer.Skip(1U);

    public byte[] ReadBytes(int count)
    {
        if (count < 0)
            throw new IndexOutOfRangeException("NetworkReader ReadBytes " + count);
        var buffer = new byte[count];
        _buffer.ReadBytes(buffer, (uint)count);
        return buffer;
    }

    public byte[] ReadBytesAndSize()
    {
        var num = ReadInt32();
        return num == 0 ? Array.Empty<byte>() : ReadBytes(num);
    }

    public void SkipBytesAndSize()
    {
        var length = ReadInt32();
        _buffer.Skip((uint)length);
    }

    public string ReadString()
    {
        var bytes = ReadBytesAndSize();
        return Encoding.UTF8.GetString(bytes);
    }

    public void SkipString() => SkipBytesAndSize();

    public int? ReadInt32Nullable()
    {
        var hasValue = ReadBoolean();
        return hasValue ? ReadInt32() : null as int?;
    }

    public void SkipInt32Nullable()
    {
        if (ReadBoolean())
            _buffer.Skip(4U);
    }

    public int[] ReadInt32Array()
    {
        var lenght = ReadInt32();
        var array = new int[lenght];
        for (var i = 0; i < lenght; i++)
        {
            array[i] = ReadInt32();
        }

        return array;
    }

    public void SkipInt32Array()
    {
        var length = ReadInt32();
        _buffer.Skip((uint)length * 4U);
    }

    public int?[] ReadInt32NullableArray()
    {
        var lenght = ReadInt32();
        var array = new int?[lenght];
        for (var i = 0; i < lenght; i++)
        {
            array[i] = ReadInt32Nullable();
        }

        return array;
    }

    public void SkipInt32NullableArray()
    {
        var lenght = ReadInt32();
        for (var i = 0; i < lenght; i++)
        {
            SkipInt32Nullable();
        }
    }

    public float[] ReadSingleArray()
    {
        var lenght = ReadInt32();
        var array = new float[lenght];
        for (var i = 0; i < lenght; i++)
        {
            array[i] = ReadSingle();
        }

        return array;
    }

    public void SkipSingleArray()
    {
        var length = ReadInt32();
        _buffer.Skip((uint)length * 4U);
    }

    public float?[] ReadSingleNullableArray()
    {
        var lenght = ReadInt32();
        var array = new float?[lenght];
        for (var i = 0; i < lenght; i++)
        {
            array[i] = ReadSingleNullable();
        }

        return array;
    }

    public void SkipSingleNullableArray()
    {
        var lenght = ReadInt32();
        for (var i = 0; i < lenght; i++)
        {
            SkipSingleNullable();
        }
    }

    public List<T> ReadList<T>()
        where T : IByteConvertable, new()
    {
        var count = ReadInt32();
        var list = new List<T>(count);
        for (var i = 0; i < count; i++)
        {
            var convertable = new T();
            convertable.FromByte(this);
            list.Add(convertable);
        }

        return list;
    }

    public T[] ReadArray<T>()
        where T : IByteConvertable, new()
    {
        var count = ReadInt32();
        var list = new T[count];
        for (var i = 0; i < count; i++)
        {
            var convertable = new T();
            convertable.FromByte(this);
            list[i] = convertable;
        }

        return list;
    }

#if UNITY_2017_2_OR_NEWER
		public Vector2 ReadVector2() => new Vector2(ReadSingle(), ReadSingle());

		public void SkipVector2() => _buffer.Skip(8U);

		public Vector2? ReadVector2Nullable()
		{
			var hasValue = ReadBoolean();
			return hasValue ? new Vector2(ReadSingle(), ReadSingle()) : null as Vector2?;
		}

		public void SkipVector2Nullable()
		{
			if (ReadBoolean())
				_buffer.Skip(8U);
		}

		public Vector2Int ReadVector2Int() => new Vector2Int(ReadInt32(), ReadInt32());

		public void SkipVector2Int() => _buffer.Skip(8U);

		public Vector2Int? ReadVector2IntNullable()
		{
			var hasValue = ReadBoolean();
			return hasValue ? new Vector2Int(ReadInt32(), ReadInt32()) : null as Vector2Int?;
		}

		public void SkipVector2IntNullable()
		{
			if (ReadBoolean())
				_buffer.Skip(8U);
		}

		public Vector2Int[] ReadVector2IntArray()
		{
			var length = ReadInt32();
			var array = new Vector2Int[length];
			for (var i = 0; i < length; i++)
			{
				array[i] = ReadVector2Int();
			}

			return array;
		}

		public void SkipVector2IntArray()
		{
			var length = ReadInt32();
			_buffer.Skip((uint)length * 8U);
		}

		public Vector3 ReadVector3() => new Vector3(ReadSingle(), ReadSingle(), ReadSingle());

		public void SkipVector3() => _buffer.Skip(12U);

		public Vector3? ReadVector3Nullable() => ReadBoolean()
			? new Vector3(ReadSingle(), ReadSingle(), ReadSingle())
			: null as Vector3?;

		public void SkipVector3Nullable()
		{
			if (ReadBoolean())
				SkipVector3();
		}

		public Vector4 ReadVector4() => new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());

		public void SkipVector4() => _buffer.Skip(16U);

		public Color ReadColor() => new Color(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());

		public void SkipColor() => _buffer.Skip(16U);

		public Color32 ReadColor32() => new Color32(ReadByte(), ReadByte(), ReadByte(), ReadByte());

		public void SkipColor32() => _buffer.Skip(4U);

		public Quaternion ReadQuaternion() => new Quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());

		public void SkipQuaternion() => _buffer.Skip(16U);

		public Quaternion? ReadQuaternionNullable() => ReadBoolean()
			? new Quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle())
			: null as Quaternion?;

		public void SkipQuaternionNullable()
		{
			if (ReadBoolean())
				_buffer.Skip(16U);
		}

		public Rect ReadRect() => new Rect(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());

		public void SkipRect() => _buffer.Skip(16U);

		public RectInt ReadRectInt() => new RectInt(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32());

		public void SkipRectInt() => _buffer.Skip(16U);

		public RectInt? ReadRectIntNullable()
		{
			var hasValue = ReadBoolean();
			return hasValue
				? new RectInt(ReadInt32(), ReadInt32(), ReadInt32(), ReadInt32())
				: null as RectInt?;
		}

		public void SkipRectIntNullable()
		{
			if (ReadBoolean())
				_buffer.Skip(16U);
		}

		public Plane ReadPlane() => new Plane(ReadVector3(), ReadSingle());

		public void SkipPlane() => _buffer.Skip(16U);

		public Bounds ReadBounds() => new Bounds(ReadVector3(), ReadVector3());

		public void SkipBounds() => Skip(24U);

		public Bounds? ReadBoundsNullable()
			=> ReadBoolean() ? new Bounds(ReadVector3(), ReadVector3()) : null as Bounds?;

		public void SkipBoundsNullable()
		{
			if (ReadBoolean())
				Skip(24U);
		}

		public Ray ReadRay() => new Ray(ReadVector3(), ReadVector3());

		public void SkipRay() => _buffer.Skip(24U);

		public Matrix4x4 ReadMatrix4x4() =>
			new Matrix4x4
			{
				m00 = ReadSingle(),
				m01 = ReadSingle(),
				m02 = ReadSingle(),
				m03 = ReadSingle(),
				m10 = ReadSingle(),
				m11 = ReadSingle(),
				m12 = ReadSingle(),
				m13 = ReadSingle(),
				m20 = ReadSingle(),
				m21 = ReadSingle(),
				m22 = ReadSingle(),
				m23 = ReadSingle(),
				m30 = ReadSingle(),
				m31 = ReadSingle(),
				m32 = ReadSingle(),
				m33 = ReadSingle()
			};

		public void SkipMatrix4x4() => _buffer.Skip(64U);
#endif

    public override string ToString() => _buffer.ToString();

    public T Read<T>() where T : IByteConvertable, new()
    {
        var convertable = new T();
        convertable.FromByte(this);
        return convertable;
    }

    object ICloneable.Clone() => Clone();

    public ByteReader Clone() => new ByteReader(_buffer.Clone());
}
