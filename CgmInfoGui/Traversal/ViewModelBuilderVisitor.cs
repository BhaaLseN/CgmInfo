using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CgmInfo.Commands;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfo.Traversal;
using CgmInfoGui.ViewModels.Nodes;
using CgmInfo.Commands.Enums;
namespace CgmInfoGui.Traversal
{
    public class MetafileContext
    {
        public MetafileViewModel Metafile { get; set; }
    }
    public class ViewModelBuilderVisitor : ICommandVisitor<MetafileContext>
    {
        public void AcceptDelimiterBeginMetafile(BeginMetafile beginMetafile, MetafileContext parameter)
        {
            parameter.Metafile = new MetafileViewModel(beginMetafile.Name);
        }

        private static NodeBase AddMetafileNode(MetafileContext context, string format, params object[] args)
        {
            if (context.Metafile == null)
                throw new InvalidOperationException("Got a Metafile Descriptor element without a Metafile");
            var newNode = new SimpleNode(string.Format(format, args));
            context.Metafile.Descriptor.Nodes.Add(newNode);
            return newNode;
        }
        public void AcceptMetafileDescriptorColorIndexPrecision(ColorIndexPrecision colorIndexPrecision, MetafileContext parameter)
        {
            AddMetafileNode(parameter, "COLOUR INDEX PRECISION: {0} bit", colorIndexPrecision.Precision);
        }

        public void AcceptMetafileDescriptorColorModel(ColorModelCommand colorModel, MetafileContext parameter)
        {
            AddMetafileNode(parameter, "COLOUR MODEL: {0}", colorModel.ColorModel);
        }

        public void AcceptMetafileDescriptorColorPrecision(ColorPrecision colorPrecision, MetafileContext parameter)
        {
            AddMetafileNode(parameter, "COLOUR PRECISION: {0} bit", colorPrecision.Precision);
        }

        public void AcceptMetafileDescriptorColorValueExtent(ColorValueExtent colorValueExtent, MetafileContext parameter)
        {
            var extentNode = AddMetafileNode(parameter, "COLOUR VALUE EXTENT: Color Space {0}", colorValueExtent.ColorSpace);
            if (colorValueExtent.ColorSpace == ColorSpace.CIE)
            {
                extentNode.Nodes.AddRange(new[]
                {
                    new SimpleNode(string.Format("First Component: {0}", colorValueExtent.FirstComponent)),
                    new SimpleNode(string.Format("Second Component: {0}", colorValueExtent.SecondComponent)),
                    new SimpleNode(string.Format("Third Component: {0}", colorValueExtent.ThirdComponent)),
                });
            }
            else if (colorValueExtent.ColorSpace != ColorSpace.Unknown) // RGB or CMYK
            {
                extentNode.Nodes.AddRange(new[]
                {
                    new SimpleNode(string.Format("Minimum: {0}", colorValueExtent.Minimum)),
                    new SimpleNode(string.Format("Maximum: {0}", colorValueExtent.Maximum)),
                });
            }
        }

        public void AcceptMetafileDescriptorIndexPrecision(IndexPrecision indexPrecision, MetafileContext parameter)
        {
            AddMetafileNode(parameter, "INDEX PRECISION: {0} bit", indexPrecision.Precision);
        }

        public void AcceptMetafileDescriptorIntegerPrecision(IntegerPrecision integerPrecision, MetafileContext parameter)
        {
            AddMetafileNode(parameter, "INTEGER PRECISION: {0} bit", integerPrecision.Precision);
        }

        public void AcceptMetafileDescriptorMaximumColorIndex(MaximumColorIndex maximumColorIndex, MetafileContext parameter)
        {
            AddMetafileNode(parameter, "MAXIMUM COLOUR INDEX: {0}", maximumColorIndex.Index);
        }

        public void AcceptMetafileDescriptorMetafileDescription(MetafileDescription metafileDescription, MetafileContext parameter)
        {
            parameter.Metafile.Descriptor.Nodes.Add(new MetafileDescriptionViewModel(metafileDescription.Description));
        }

        public void AcceptMetafileDescriptorMetafileVersion(MetafileVersion metafileVersion, MetafileContext parameter)
        {
            AddMetafileNode(parameter, "METAFILE VERSION: {0}", metafileVersion.Version);
        }

        public void AcceptMetafileDescriptorNamePrecision(NamePrecision namePrecision, MetafileContext parameter)
        {
            AddMetafileNode(parameter, "NAME PRECISION: {0} bit", namePrecision.Precision);
        }

        public void AcceptMetafileDescriptorRealPrecision(RealPrecision realPrecision, MetafileContext parameter)
        {
            var realNode = AddMetafileNode(parameter, "REAL PRECISION: {0}", realPrecision.RepresentationForm);
            realNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Exponent Width: {0} bit", realPrecision.ExponentWidth)),
                new SimpleNode(string.Format("Fraction Width: {0} bit", realPrecision.FractionWidth)),
            });
        }

        public void AcceptMetafileDescriptorVdcType(VdcType vdcType, MetafileContext parameter)
        {
            AddMetafileNode(parameter, "VDC TYPE: {0}", vdcType.Specification);
        }

        public void AcceptUnsupportedCommand(UnsupportedCommand unsupportedCommand, MetafileContext parameter)
        {
            // do nothing
        }
    }
}
