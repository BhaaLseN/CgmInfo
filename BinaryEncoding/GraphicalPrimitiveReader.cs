using System.Collections.Generic;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.GraphicalPrimitives;
using PointF = System.Drawing.PointF;

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
            return new TextCommand(reader.ReadPoint(), reader.ReadEnum<FinalFlag>(), reader.ReadString());
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
            return new RestrictedText(reader.ReadVdc(), reader.ReadVdc(), reader.ReadPoint(), reader.ReadEnum<FinalFlag>(), reader.ReadString());
        }

        public static AppendText AppendText(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) final / not - final flag: valid values are
            //      0 not final
            //      1 final
            // P2: (string) text string [ISO/IEC 8632-3 8.6]
            return new AppendText(reader.ReadEnum<FinalFlag>(), reader.ReadString());
        }

        public static Polygon Polygon(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1-Pn: (point) n (X,Y) polygon vertices [ISO/IEC 8632-3 8.6]
            var points = new List<PointF>();
            // TODO: point is 2 VDCs, but that may range from 8 bits each until up to 64 bits for a single coordinate
            //       this should probably check for 2x VDC size instead of simply 2x minimum-possible VDC size
            while (reader.HasMoreData(2))
            {
                points.Add(reader.ReadPoint());
            }
            return new Polygon(points.ToArray());
        }

        public static Rectangle Rectangle(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) first corner [ISO/IEC 8632-3 8.6]
            // P2: (point) second corner
            return new Rectangle(reader.ReadPoint(), reader.ReadPoint());
        }

        public static Circle Circle(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) centre of circle [ISO/IEC 8632-3 8.6]
            // P2: (vdc) radius of circle
            return new Circle(reader.ReadPoint(), reader.ReadVdc());
        }

        public static CircularArcCenter CircularArcCenter(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) centre of circle
            // P2: (vdc) delta X for start vector
            // P3: (vdc) delta Y for start vector
            // P4: (vdc) delta X for end vector
            // P5: (vdc) delta Y for end vector
            // P6: (vdc) radius of circle
            // NOTE: Text Encoding allows start/end vectors to be encoded as point (mostly for syntax),
            //       but it actually makes sense to store them as such - so we rely on ReadPoint to read 2x VDC for us.
            return new CircularArcCenter(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadVdc());
        }

        public static Ellipse Ellipse(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) centre of ellipse
            // P2: (point) endpoint of first conjugate diameter
            // P3: (point) endpoint of second conjugate diameter
            return new Ellipse(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint());
        }

        public static EllipticalArc EllipticalArc(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) centre of ellipse
            // P2: (point) endpoint for first conjugate diameter
            // P3: (point) endpoint for second conjugate diameter
            // P4: (vdc) delta X for start vector
            // P5: (vdc) delta Y for start vector
            // P6: (vdc) delta X for end vector
            // P7: (vdc) delta Y for end vector
            // NOTE: Text Encoding allows start/end vectors to be encoded as point (mostly for syntax),
            //       but it actually makes sense to store them as such - so we rely on ReadPoint to read 2x VDC for us.
            return new EllipticalArc(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint());
        }
    }
}
