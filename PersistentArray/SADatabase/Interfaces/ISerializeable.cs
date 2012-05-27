namespace Interfaces
{
    public interface ISerializeable
    {
        byte[] Serialize();

        //static T Deserialize(byte[] data);
    }
}
