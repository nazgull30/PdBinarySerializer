namespace ByteFormatter;

using System;

public sealed class ByteBuffer : ICloneable
{
    private const int INITIAL_SIZE = 64;
    private const float GROWTH_FACTOR = 1.5f;

    private byte[] _buffer;

    public ByteBuffer() : this(new byte[INITIAL_SIZE])
    {
    }

    public ByteBuffer(byte[] buffer, long position = 0)
    {
        _buffer = buffer;
        _position = position;
    }

    private long _position;
    public long Position => _position;

    public int Length => _buffer.Length;

    public void ReInit(byte[] buffer)
    {
        _buffer = buffer;
        _position = 0;
    }

    public void Reset()
    {
        Array.Clear(_buffer, 0, _buffer.Length);
        _position = 0;
    }

    public byte ReadByte()
    {
        if (_position >= _buffer.Length)
            throw new IndexOutOfRangeException("NetworkReader:ReadByte out of range:" + ToString());
        return _buffer[_position++];
    }

    public void ReadBytes(byte[] buffer, uint count)
    {
        if (_position + count > _buffer.Length)
            throw new IndexOutOfRangeException("NetworkReader:ReadBytes out of range: (" + count + ") " +
                                               ToString());
        for (int index = 0; (uint)index < count; ++index)
            buffer[index] = _buffer[_position + index];
        _position += count;
    }

    public byte[] AsArraySegment()
    {
        var bytes = new byte[_position];
        Array.Copy(_buffer, bytes, _position);
        return bytes;
    }

    public void WriteByte(byte value)
    {
        WriteCheckForSpace(1);
        _buffer[_position] = value;
        ++_position;
    }

    public void WriteByte2(byte value0, byte value1)
    {
        WriteCheckForSpace(2);
        _buffer[_position] = value0;
        _buffer[_position + 1U] = value1;
        _position += 2U;
    }

    public void WriteByte4(byte value0, byte value1, byte value2, byte value3)
    {
        WriteCheckForSpace(4);
        _buffer[_position] = value0;
        _buffer[_position + 1U] = value1;
        _buffer[_position + 2U] = value2;
        _buffer[_position + 3U] = value3;
        _position += 4U;
    }

    public void WriteByte8(
        byte value0,
        byte value1,
        byte value2,
        byte value3,
        byte value4,
        byte value5,
        byte value6,
        byte value7
    )
    {
        WriteCheckForSpace(8);
        _buffer[_position] = value0;
        _buffer[_position + 1U] = value1;
        _buffer[_position + 2U] = value2;
        _buffer[_position + 3U] = value3;
        _buffer[_position + 4U] = value4;
        _buffer[_position + 5U] = value5;
        _buffer[_position + 6U] = value6;
        _buffer[_position + 7U] = value7;
        _position += 8U;
    }

    public void WriteBytesAtOffset(byte[] buffer, int targetOffset, int count)
    {
        var num = count + (uint)targetOffset;
        WriteCheckForSpace((int)num);
        if (targetOffset == 0 && count == buffer.Length)
            buffer.CopyTo(_buffer, (int)_position);
        else
            for (var index = 0; index < (int)count; ++index)
                _buffer[targetOffset + index] = buffer[index];

        if (num <= _position)
            return;
        _position = num;
    }

    public void WriteBytes(byte[] buffer, int count)
    {
        WriteCheckForSpace(count);
        if (count == buffer.Length)
            buffer.CopyTo(_buffer, (int)_position);
        else
            for (var index = 0; index < count; ++index)
                _buffer[_position + index] = buffer[index];

        _position += count;
    }

    private void WriteCheckForSpace(int count)
    {
        if (_position + count < _buffer.Length)
            return;

        var length = (int)Math.Ceiling(_buffer.Length * GROWTH_FACTOR);
        while (_position + count >= length)
        {
            length = (int)Math.Ceiling(length * GROWTH_FACTOR);
        }

        var numArray = new byte[length];
        _buffer.CopyTo(numArray, 0);
        _buffer = numArray;
    }

    public void SeekZero() => _position = 0U;

    public void Skip(uint length) => _position += length;

    public void Replace(byte[] buffer)
    {
        _buffer = buffer;
        _position = 0U;
    }

    public override string ToString() => $"NetBuf sz:{_buffer.Length} pos:{_position}";

    object ICloneable.Clone() => Clone();

    public ByteBuffer Clone() => new ByteBuffer(_buffer, _position);

    public ByteBuffer Copy()
    {
        var bytes = new byte[_buffer.Length];
        _buffer.CopyTo(bytes, 0);
        return new ByteBuffer(bytes, _position);
    }
}
