namespace ByteFormatter;

public interface IByteConvertable
{
    public void ToByte(ByteWriter writer);

    public void FromByte(ByteReader reader);
}
