using System.IO;
using System.Threading;

namespace Persistence
{
    public class SlowRAF : RandomAccessFile
    {
        private const int Latency = 1500;

        public SlowRAF(FileStream stream) : base(stream)
        {
        }

        public new void WriteInt(int i)
        {
            Sleep();
            base.WriteInt(i);
        }

        public new void WriteByte(byte b)
        {
            Sleep();
            base.WriteByte(b);
        }

        public new void WriteByteArray(byte[] buffer)
        {
            Sleep();
            base.WriteByteArray(buffer);
        }

        public new int ReadInt()
        {
            Sleep();
            return base.ReadInt();
        }

        public new byte ReadByte()
        {
            Sleep();
            return base.ReadByte();
        }

        public new void ReadByteArray(byte[] buffer)
        {
            Sleep();
            base.ReadByteArray(buffer);
        }

        private void Sleep()
        {
            Thread.Sleep(Latency);
        }

        public new void Flush()
        {
            Sleep();
            base.Flush();
        }
    }
}
