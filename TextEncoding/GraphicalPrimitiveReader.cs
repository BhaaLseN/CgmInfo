using System.Collections.Generic;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.GraphicalPrimitives;
using PointF = System.Drawing.PointF;

namespace CgmInfo.TextEncoding
{
    internal static class GraphicalPrimitiveReader
    {
        public static Polyline Polyline(MetafileReader reader)
        {
            var points = ReadPointList(reader);
            return new Polyline(points.ToArray());
        }

        public static Polyline IncrementalPolyline(MetafileReader reader)
        {
            var points = ReadIncrementalPointList(reader);
            return new Polyline(points.ToArray());
        }

        public static DisjointPolyline DisjointPolyline(MetafileReader reader)
        {
            var points = ReadPointList(reader);
            return new DisjointPolyline(points.ToArray());
        }

        public static DisjointPolyline IncrementalDisjointPolyline(MetafileReader reader)
        {
            var points = ReadIncrementalPointList(reader);
            return new DisjointPolyline(points.ToArray());
        }

        public static Polymarker Polymarker(MetafileReader reader)
        {
            var points = ReadPointList(reader);
            return new Polymarker(points.ToArray());
        }

        public static Polymarker IncrementalPolymarker(MetafileReader reader)
        {
            var points = ReadIncrementalPointList(reader);
            return new Polymarker(points.ToArray());
        }

        public static TextCommand Text(MetafileReader reader)
        {
            return new TextCommand(reader.ReadPoint(), ParseFinalFlag(reader.ReadEnum()), reader.ReadString());
        }

        public static RestrictedText RestrictedText(MetafileReader reader)
        {
            return new RestrictedText(reader.ReadVdc(), reader.ReadVdc(), reader.ReadPoint(), ParseFinalFlag(reader.ReadEnum()), reader.ReadString());
        }

        public static AppendText AppendText(MetafileReader reader)
        {
            return new AppendText(ParseFinalFlag(reader.ReadEnum()), reader.ReadString());
        }

        public static Polygon Polygon(MetafileReader reader)
        {
            var points = new List<PointF>();
            while (reader.HasMoreData(2))
            {
                points.Add(reader.ReadPoint());
            }
            return new Polygon(points.ToArray());
        }

        public static Polygon IncrementalPolygon(MetafileReader reader)
        {
            var points = new List<PointF>();
            var lastPoint = reader.ReadPoint();
            points.Add(lastPoint);
            while (reader.HasMoreData(2))
            {
                double deltaX = reader.ReadVdc();
                double deltaY = reader.ReadVdc();
                lastPoint = new PointF((float)(lastPoint.X + deltaX), (float)(lastPoint.Y + deltaY));
                points.Add(lastPoint);
            }
            return new Polygon(points.ToArray());
        }

        public static PolygonSet PolygonSet(MetafileReader reader)
        {
            var points = new List<PointF>();
            var flags = new List<EdgeOutFlags>();
            while (reader.HasMoreData(3))
            {
                points.Add(reader.ReadPoint());
                flags.Add(ParseEdgeOutFlags(reader.ReadEnum()));
            }
            return new PolygonSet(points.ToArray(), flags.ToArray());
        }

        public static PolygonSet IncrementalPolygonSet(MetafileReader reader)
        {
            var points = new List<PointF>();
            var flags = new List<EdgeOutFlags>();
            var lastPoint = reader.ReadPoint();
            points.Add(lastPoint);
            flags.Add(ParseEdgeOutFlags(reader.ReadEnum()));
            while (reader.HasMoreData(3))
            {
                double deltaX = reader.ReadVdc();
                double deltaY = reader.ReadVdc();
                lastPoint = new PointF((float)(lastPoint.X + deltaX), (float)(lastPoint.Y + deltaY));
                points.Add(lastPoint);
                flags.Add(ParseEdgeOutFlags(reader.ReadEnum()));
            }
            return new PolygonSet(points.ToArray(), flags.ToArray());
        }

        public static Rectangle Rectangle(MetafileReader reader)
        {
            return new Rectangle(reader.ReadPoint(), reader.ReadPoint());
        }

        public static Circle Circle(MetafileReader reader)
        {
            return new Circle(reader.ReadPoint(), reader.ReadVdc());
        }

        public static CircularArcCenter CircularArcCenter(MetafileReader reader)
        {
            return new CircularArcCenter(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadVdc());
        }

        public static Ellipse Ellipse(MetafileReader reader)
        {
            return new Ellipse(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint());
        }

        public static EllipticalArc EllipticalArc(MetafileReader reader)
        {
            return new EllipticalArc(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint());
        }

        private static FinalFlag ParseFinalFlag(string token)
        {
            // assume not final; unless its final
            if (token.ToUpperInvariant() == "FINAL")
                return FinalFlag.Final;
            return FinalFlag.NotFinal;
        }

        private static List<PointF> ReadPointList(MetafileReader reader)
        {
            var points = new List<PointF>();
            while (reader.HasMoreData(2))
            {
                points.Add(reader.ReadPoint());
            }

            return points;
        }
        private static List<PointF> ReadIncrementalPointList(MetafileReader reader)
        {
            var points = new List<PointF>();
            var lastPoint = reader.ReadPoint();
            points.Add(lastPoint);
            while (reader.HasMoreData(2))
            {
                double deltaX = reader.ReadVdc();
                double deltaY = reader.ReadVdc();
                lastPoint = new PointF((float)(lastPoint.X + deltaX), (float)(lastPoint.Y + deltaY));
                points.Add(lastPoint);
            }

            return points;
        }

        private static EdgeOutFlags ParseEdgeOutFlags(string token)
        {
            // assume invisible unless its any of the others
            token = token.ToUpperInvariant();
            if (token == "VIS")
                return EdgeOutFlags.Visible;
            else if (token == "CLOSEINVIS")
                return EdgeOutFlags.InvisibleClose;
            else if (token == "CLOSEVIS")
                return EdgeOutFlags.VisibleClose;
            return EdgeOutFlags.Invisible;
        }
    }
}
