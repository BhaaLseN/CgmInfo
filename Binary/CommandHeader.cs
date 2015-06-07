namespace CgmInfo.Binary
{
#if DEBUG
    [System.Diagnostics.DebuggerDisplay("Class={ElementClass}, Id={ElementId}")]
#endif
    public class CommandHeader
    {
        public CommandHeader(int elementClass, int elementId, int parameterListLength)
        {
            ElementClass = elementClass;
            ElementId = elementId;
            ParameterListLength = parameterListLength;
        }
        public int ElementClass { get; private set; }
        public int ElementId { get; private set; }
        public int ParameterListLength { get; private set; }
    }
}
