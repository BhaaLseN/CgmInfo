using System.Collections.Generic;
using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.GraphicalPrimitives;

namespace CgmInfo.BinaryEncoding
{
    internal static class GraphicalPrimitiveReader
    {
        public static Polyline Polyline(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1-Pn: (point) n (X,Y) polyline vertices [ISO/IEC 8632-3 8.6]
            var points = new List<PointF>();
            // TODO: point is 2 VDCs, but that may range from 8 bits each until up to 64 bits for a single coordinate
            //       this should probably check for 2x VDC size instead of simply 2x minimum-possible VDC size
            while (reader.HasMoreData(2))
            {
                points.Add(reader.ReadPoint());
            }
            return new Polyline(points.ToArray());
        }

        public static TextCommand Text(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) text position [ISO/IEC 8632-3 8.6]
            // P2: (enumerated) final / not - final flag: valid values are
            //      0 not final
            //      1 final
            // P3: (string) text string 
            return new TextCommand(reader.ReadVdc(), reader.ReadVdc(), reader.ReadEnum<FinalFlag>(), reader.ReadString());
        }

        public static RestrictedText RestrictedText(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (vdc) delta width [ISO/IEC 8632-3 8.6]
            // P2: (vdc) delta height
            // P3: (point) text position
            // P4: (enumerated) final / not - final flag: valid values are
            //      0 not final
            //      1 final
            // P5: (string) text string 
            return new RestrictedText(reader.ReadVdc(), reader.ReadVdc(), reader.ReadVdc(), reader.ReadVdc(), reader.ReadEnum<FinalFlag>(), reader.ReadString());
        }

        public static AppendText AppendText(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) final / not - final flag: valid values are
            //      0 not final
            //      1 final
            // P2: (string) text string [ISO/IEC 8632-3 8.6]
            return new AppendText(reader.ReadEnum<FinalFlag>(), reader.ReadString());
        }
    }
}
