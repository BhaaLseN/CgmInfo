using System.Linq;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Traversal;

namespace CgmInfoGui.Traversal;

public class HotspotBuilderVisitor : CommandVisitor<HotspotContext>
{
    // consider grobject, para and subpara as hotspot (ie. interactive regions/objects that can be "picked") [WebCGM20-Concepts 2.3.3]
    private static readonly string[] HotspotAPSTypes = { "GROBJECT", "PARA", "SUBPARA" };
    private static bool IsHotspot(BeginApplicationStructure beginApplicationStructure)
    {
        if (beginApplicationStructure == null)
            return false;
        return HotspotAPSTypes.Contains(beginApplicationStructure.Type.ToUpperInvariant());
    }

    public override void AcceptDelimiterBeginApplicationStructure(BeginApplicationStructure beginApplicationStructure, HotspotContext parameter)
    {
        parameter.BeginAPS(beginApplicationStructure);

        if (IsHotspot(beginApplicationStructure))
            parameter.BeginHotspot(beginApplicationStructure.Identifier);
    }
    public override void AcceptDelimiterBeginApplicationStructureBody(BeginApplicationStructureBody beginApplicationStructureBody, HotspotContext parameter)
    {
        parameter.SealHotspot();
    }
    public override void AcceptDelimiterEndApplicationStructure(EndApplicationStructure endApplicationStructure, HotspotContext parameter)
    {
        var beginAps = parameter.EndAPS();
        if (IsHotspot(beginAps))
            parameter.EndHotspot();
    }
    public override void AcceptApplicationStructureDescriptorAttribute(ApplicationStructureAttribute applicationStructureAttribute, HotspotContext parameter)
    {
        parameter.AddHotspotAttribute(applicationStructureAttribute);
    }
}
