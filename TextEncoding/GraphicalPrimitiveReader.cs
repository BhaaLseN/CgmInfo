using System.Collections.Generic;
using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.GraphicalPrimitives;

namespace CgmInfo.TextEncoding
{
    internal static class GraphicalPrimitiveReader
    {
        public static Polyline Polyline(MetafileReader reader)
        {
            var points = new List<PointF>();
            do
            {
                points.Add(reader.ReadPoint());
            } while (!reader.AtEndOfElement);
            return new Polyline(points.ToArray());
        }

        public static Polyline IncrementalPolyline(MetafileReader reader)
        {
            var points = new List<PointF>();
            var lastPoint = reader.ReadPoint();
            points.Add(lastPoint);
            do
            {
                double deltaX = reader.ReadVdc();
                if (reader.AtEndOfElement)
                    break;
                double deltaY = reader.ReadVdc();
                lastPoint = new PointF((float)(lastPoint.X + deltaX), (float)(lastPoint.Y + deltaY));
                points.Add(lastPoint);
            } while (!reader.AtEndOfElement);
            return new Polyline(points.ToArray());
        }

        public static TextCommand Text(MetafileReader reader)
        {
            return new TextCommand(reader.ReadVdc(), reader.ReadVdc(), ParseFinalFlag(reader.ReadEnum()), reader.ReadString());
        }

        public static RestrictedText RestrictedText(MetafileReader reader)
        {
            return new RestrictedText(reader.ReadVdc(), reader.ReadVdc(), reader.ReadVdc(), reader.ReadVdc(), ParseFinalFlag(reader.ReadEnum()), reader.ReadString());
        }

        public static AppendText AppendText(MetafileReader reader)
        {
            return new AppendText(ParseFinalFlag(reader.ReadEnum()), reader.ReadString());
        }

        private static FinalFlag ParseFinalFlag(string token)
        {
            // assume not final; unless its final
            if (token.ToUpperInvariant() == "FINAL")
                return FinalFlag.Final;
            return FinalFlag.NotFinal;
        }
    }
}
