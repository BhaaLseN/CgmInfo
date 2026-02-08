using CgmInfo.Traversal;

namespace CgmInfo.Commands.External
{
    [TextToken("APPLDATA")]
    public class ApplicationData : Command
    {
        public ApplicationData(int identifier, string dataRecord)
            : base(7, 2)
        {
            Identifier = identifier;
            DataRecord = dataRecord;
        }

        public int Identifier { get; }
        public string DataRecord { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptExternalApplicationData(this, parameter);
        }
    }
}
