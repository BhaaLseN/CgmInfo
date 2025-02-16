using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Traversal;

namespace CgmInfoGui.Traversal;

public class XCFDocumentBuilderVisitor : CommandVisitor<XCFDocumentContext>
{
    public override void AcceptApplicationStructureDescriptorAttribute(ApplicationStructureAttribute applicationStructureAttribute, XCFDocumentContext parameter)
    {
        parameter.AddAPSAttribute(applicationStructureAttribute);
    }

    public override void AcceptDelimiterBeginApplicationStructure(BeginApplicationStructure beginApplicationStructure, XCFDocumentContext parameter)
    {
        parameter.AddAPSElement(beginApplicationStructure);
    }

    public override void AcceptDelimiterBeginApplicationStructureBody(BeginApplicationStructureBody beginApplicationStructureBody, XCFDocumentContext parameter)
    {
        parameter.RemoveEmptyAPSElement();
    }
}
