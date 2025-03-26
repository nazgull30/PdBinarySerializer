namespace ByteFormatter;

using System;
using System.Collections.Generic;
using System.Text;

public sealed class ByteWriter
{
    private readonly ByteBuffer _buffer;

    public ByteWriter()
    {
        _buffer = new ByteBuffer();
    }

    public ByteWriter(byte[] buffer)
    {
        _buffer = new ByteBuffer(buffer);
    }

    private ByteWriter(ByteBuffer buffer)
    {
        _buffer = buffer;
    }

    public long Position => _buffer.Position;

    public void Reset() => _buffer.Reset();

    public byte[] ToArray() => _buffer.AsArraySegment();

    public void Write(char value) => _buffer.WriteByte((byte)value);

    public void Write(byte value) => _buffer.WriteByte(value);

    public void Write(byte? value)
    {
        if (value.HasValue)
        {
            Write(true);
            Write(value.Value);
        }
        else
            Write(false);
    }

    public void Write(sbyte value) => _buffer.WriteByte((byte)value);

    public void Write(sbyte? value)
    {
        if (value.HasValue)
        {
            Write(true);
            Write(value.Value);
        }
        else
            Write(false);
    }

    public void Write(short value)
        => _buffer.WriteByte2(
            (byte)((uint)value & byte.MaxValue),
            (byte)((value >> 8) & byte.MaxValue)
        );

    public void Write(short? value)
    {
        if (value.HasValue)
        {
            Write(true);
            Write(value.Value);
        }
        else
            Write(false);
    }

    public void Write(ushort value)
        => _buffer.WriteByte2(
            (byte)(value & (uint)byte.MaxValue),
            (byte)((value >> 8) & byte.MaxValue)
        );

    public void Write(ushort? value)
    {
        if (value.HasValue)
        {
            Write(true);
            Write(value.Value);
        }
        else
            Write(false);
    }

    public void Write(int value)
        => _buffer.WriteByte4(
            (byte)(value & byte.MaxValue),
            (byte)((value >> 8) & byte.MaxValue),
            (byte)((value >> 16) & byte.MaxValue),
            (byte)((value >> 24) & byte.MaxValue)
        );

    public void Write(int? value)
    {
        if (value.HasValue)
        {
            Write(true);
            Write(value.Value);
        }
        else
            Write(false);
    }

    public void Write(int[] array)
    {
        Write(array.Length);
        for (var i = 0; i < array.Length; i++)
        {
            Write(array[i]);
        }
    }

    public void Write(int?[] array)
    {
        Write(array.Length);
        for (var i = 0; i < array.Length; i++)
        {
            Write(array[i]);
        }
    }

    public void Write(uint value)
        => _buffer.WriteByte4(
            (byte)(value & byte.MaxValue),
            (byte)((value >> 8) & byte.MaxValue),
            (byte)((value >> 16) & byte.MaxValue),
            (byte)((value >> 24) & byte.MaxValue)
        );

    public void Write(uint? value)
    {
        if (value.HasValue)
        {
            Write(true);
            Write(value.Value);
        }
        else
            Write(false);
    }

    public void Write(long value)
        => _buffer.WriteByte8(
            (byte)((ulong)value & byte.MaxValue),
            (byte)((ulong)(value >> 8) & byte.MaxValue),
            (byte)((ulong)(value >> 16) & byte.MaxValue),
            (byte)((ulong)(value >> 24) & byte.MaxValue),
            (byte)((ulong)(value >> 32) & byte.MaxValue),
            (byte)((ulong)(value >> 40) & byte.MaxValue),
            (byte)((ulong)(value >> 48) & byte.MaxValue),
            (byte)((ulong)(value >> 56) & byte.MaxValue)
        );

    public void Write(long? value)
    {
        if (value.HasValue)
        {
            Write(true);
            Write(value.Value);
        }
        else
            Write(false);
    }

    public void Write(ulong value)
        => _buffer.WriteByte8(
            (byte)(value & byte.MaxValue),
            (byte)((value >> 8) & byte.MaxValue),
            (byte)((value >> 16) & byte.MaxValue),
            (byte)((value >> 24) & byte.MaxValue),
            (byte)((value >> 32) & byte.MaxValue),
            (byte)((value >> 40) & byte.MaxValue),
            (byte)((value >> 48) & byte.MaxValue),
            (byte)((value >> 56) & byte.MaxValue)
        );

    public void Write(ulong? value)
    {
        if (value.HasValue)
        {
            Write(true);
            Write(value.Value);
        }
        else
            Write(false);
    }

    public void Write(float value)
    {
        var intVal = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
        Write(intVal);
    }

    public void Write(float? value)
    {
        if (value.HasValue)
        {
            Write(true);
            Write(value.Value);
        }
        else
            Write(false);
    }

    public void Write(float[] array)
    {
        Write(array.Length);
        for (var i = 0; i < array.Length; i++)
        {
            Write(array[i]);
        }
    }

    public void Write(float?[] array)
    {
        Write(array.Length);
        for (var i = 0; i < array.Length; i++)
        {
            Write(array[i]);
        }
    }

    public void Write(double value)
    {
        var longVal = BitConverter.DoubleToInt64Bits(value);
        Write(longVal);
    }

    public void Write(decimal value)
    {
        var bits = decimal.GetBits(value);
        Write(bits[0]);
        Write(bits[1]);
        Write(bits[2]);
        Write(bits[3]);
    }

    public void Write(bool value) => _buffer.WriteByte(value ? (byte)1 : (byte)0);

    public void Write(byte[] buffer)
        => _buffer.WriteBytes(buffer, buffer.Length);

    public void Write(byte[] buffer, int count)
        => _buffer.WriteBytes(buffer, count);

    public void Write(byte[] buffer, int offset, int count)
        => _buffer.WriteBytesAtOffset(buffer, offset, count);

    public void WriteBytesAndSize(byte[] buffer, int count)
    {
        if (buffer == null || count == 0)
        {
            Write(0);
        }
        else
        {
            Write(count);
            _buffer.WriteBytes(buffer, count);
        }
    }

    public void Write(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        WriteBytesAndSize(bytes, bytes.Length);
    }

    public void Write<T>(IList<T> list)
        where T : IByteConvertable
    {
        Write(list.Count);
        for (var i = 0; i < list.Count; i++)
        {
            var convertable = list[i];
            convertable.ToByte(this);
        }
    }

#if UNITY_2017_2_OR_NEWER
		public void Write(Vector2 value)
		{
			Write(value.x);
			Write(value.y);
		}

		public void Write(Vector2? value)
		{
			if (value.HasValue)
			{
				Write(true);
				var v = value.Value;
				Write(v.x);
				Write(v.y);
			}
			else
				Write(false);
		}

		public void Write(Vector2Int value)
		{
			Write(value.x);
			Write(value.y);
		}

		public void Write(Vector2Int? value)
		{
			if (value.HasValue)
			{
				Write(true);
				var v = value.Value;
				Write(v.x);
				Write(v.y);
			}
			else
				Write(false);
		}

		public void Write(Vector2Int[] array)
		{
			Write(array.Length);
			for (var i = 0; i < array.Length; i++)
			{
				Write(array[i]);
			}
		}

		public void Write(Vector3 value)
		{
			Write(value.x);
			Write(value.y);
			Write(value.z);
		}

		public void Write(Vector3? value)
		{
			if (value.HasValue)
			{
				Write(true);
				Write(value.Value);
			}
			else
				Write(false);
		}

		public void Write(Vector4 value)
		{
			Write(value.x);
			Write(value.y);
			Write(value.z);
			Write(value.w);
		}

		public void Write(Vector4? value)
		{
			if (value.HasValue)
			{
				Write(true);
				Write(value.Value);
			}
			else
				Write(false);
		}

		public void Write(Color value)
		{
			Write(value.r);
			Write(value.g);
			Write(value.b);
			Write(value.a);
		}

		public void Write(Color32 value)
		{
			Write(value.r);
			Write(value.g);
			Write(value.b);
			Write(value.a);
		}

		public void Write(Quaternion value)
		{
			Write(value.x);
			Write(value.y);
			Write(value.z);
			Write(value.w);
		}

		public void Write(Quaternion? value)
		{
			if (value.HasValue)
			{
				Write(true);
				Write(value.Value);
			}
			else
				Write(false);
		}

		public void Write(Rect value)
		{
			Write(value.xMin);
			Write(value.yMin);
			Write(value.width);
			Write(value.height);
		}

		public void Write(RectInt value)
		{
			Write(value.xMin);
			Write(value.yMin);
			Write(value.width);
			Write(value.height);
		}

		public void Write(RectInt? value)
		{
			if (value.HasValue)
			{
				var r = value.Value;
				Write(true);
				Write(r.xMin);
				Write(r.yMin);
				Write(r.width);
				Write(r.height);
			}
			else
				Write(false);
		}

		public void Write(Plane value)
		{
			Write(value.normal);
			Write(value.distance);
		}

		public void Write(Bounds value)
		{
			Write(value.center);
			Write(value.size);
		}

		public void Write(Bounds? value)
		{
			if (value.HasValue)
			{
				var bounds = value.Value;
				Write(true);
				Write(bounds.center);
				Write(bounds.size);
			}
			else
				Write(false);
		}

		public void Write(Ray value)
		{
			Write(value.direction);
			Write(value.origin);
		}

		public void Write(Matrix4x4 value)
		{
			Write(value.m00);
			Write(value.m01);
			Write(value.m02);
			Write(value.m03);
			Write(value.m10);
			Write(value.m11);
			Write(value.m12);
			Write(value.m13);
			Write(value.m20);
			Write(value.m21);
			Write(value.m22);
			Write(value.m23);
			Write(value.m30);
			Write(value.m31);
			Write(value.m32);
			Write(value.m33);
		}
#endif

    public void Write(IByteConvertable convertable) => convertable.ToByte(this);

    public void SeekZero() => _buffer.SeekZero();

    public void Skip(uint length) => _buffer.Skip(length);

    public ByteWriter Copy() => new ByteWriter(_buffer.Copy());
}
