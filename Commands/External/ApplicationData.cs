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

        public int Identifier { get; private set; }
        public string DataRecord { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptExternalApplicationData(this, parameter);
        }
    }
}
