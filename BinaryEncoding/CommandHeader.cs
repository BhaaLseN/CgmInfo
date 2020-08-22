namespace CgmInfo.BinaryEncoding
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
        public int ElementClass { get; }
        public int ElementId { get; }
        public int ParameterListLength { get; }
    }
}
