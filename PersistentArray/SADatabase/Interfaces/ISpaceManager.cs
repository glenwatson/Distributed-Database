namespace Interfaces
{
    public interface ISpaceManager
    {
        /// <summary>
        /// Gets the next space available.
        /// Does not reserve the space.
        /// </summary>
        /// <returns>A token representing the space</returns>
        int AllocateBlock();

        void FreeBlock(int blockIndex);
    }
}
