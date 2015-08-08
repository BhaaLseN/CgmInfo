namespace CgmInfo.Commands
{
    public sealed class UnsupportedCommand : Command
    {
        public UnsupportedCommand(int elementClass, int elementId)
            : base(elementClass, elementId)
        {
        }
    }
}
