using System.Collections.Generic;
using System.Linq;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.GraphicalPrimitives;
using CgmInfo.Utilities;
using PointF = System.Drawing.PointF;

namespace CgmInfo.BinaryEncoding
{
    // [ISO/IEC 8632-3 8.6]
    internal static class GraphicalPrimitiveReader
    {
        public static Polyline Polyline(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1-Pn: (point) n (X,Y) polyline vertices
            var points = ReadPointList(reader);
            return new Polyline(points.ToArray());
        }

        public static DisjointPolyline DisjointPolyline(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1-Pn: (point) n (X,Y) line segment endpoints
            var points = ReadPointList(reader);
            return new DisjointPolyline(points.ToArray());
        }

        public static Polymarker Polymarker(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1-Pn: (point) n (X,Y) marker positions
            var points = ReadPointList(reader);
            return new Polymarker(points.ToArray());
        }

        public static TextCommand Text(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) text position
            // P2: (enumerated) final / not - final flag: valid values are
            //      0 not final
            //      1 final
            // P3: (string) text string 
            return new TextCommand(reader.ReadPoint(), reader.ReadEnum<FinalFlag>(), reader.ReadString());
        }

        public static RestrictedText RestrictedText(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (vdc) delta width
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
            // P2: (string) text string
            return new AppendText(reader.ReadEnum<FinalFlag>(), reader.ReadString());
        }

        public static Polygon Polygon(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1-Pn: (point) n (X,Y) polygon vertices
            var points = new List<PointF>();
            // TODO: point is 2 VDCs, but that may range from 8 bits each until up to 64 bits for a single coordinate
            //       this should probably check for 2x VDC size instead of simply 2x minimum-possible VDC size
            while (reader.HasMoreData(2))
            {
                points.Add(reader.ReadPoint());
            }
            return new Polygon(points.ToArray());
        }

        public static PolygonSet PolygonSet(MetafileReader reader, CommandHeader commandHeader)
        {
            // P(i): (point) (X,Y) polygon vertex
            // P(i+1): (enumerated) edge out flag, indicating closures and edge visibility: valid values are
            //      0 invisible
            //      1 visible
            //      2 close, invisible
            //      3 close, visible
            var points = new List<PointF>();
            var flags = new List<EdgeOutFlags>();
            // TODO: point is 2 VDCs, but that may range from 8 bits each until up to 64 bits for a single coordinate
            //       this should probably check for 2x VDC size instead of simply 2x minimum-possible VDC size
            while (reader.HasMoreData(3))
            {
                points.Add(reader.ReadPoint());
                flags.Add(reader.ReadEnum<EdgeOutFlags>());
            }
            return new PolygonSet(points.ToArray(), flags.ToArray());
        }

        public static CellArray CellArray(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) corner point P
            // P2: (point) corner point Q
            // P3: (point) corner point R
            // P4: (integer) nx
            // P5: (integer) ny
            // P6: (integer) local colour precision: valid values are 0, 1, 2, 4, 8, 16, 24, and 32. If the value is zero (the
            //      'default colour precision indicator' value), the COLOUR (INDEX) PRECISION for the picture indicates the
            //      precision with which the colour list is encoded. If the value is non-zero, the precision with which the colour
            //      data is encoded is given by the value.
            // P7: (enumerated) cell representation mode: valid values are
            //      0 run length list mode
            //      1 packed list mode
            // P8: (colour list) array of cell colour values.
            //      If the COLOUR SELECTION MODE is 'direct', the values will be direct colour values. If the COLOUR
            //      SELECTION MODE is 'indexed', the values will be indexes into the COLOUR TABLE.
            //      If the cell representation mode is 'packed list', the colour values are represented by rows of values, each
            //      row starting on a word boundary. If the cell representation mode is 'run length', the colour list values are
            //      represented by rows broken into runs of constant colour; each row starts on a word boundary. Each list
            //      item consists of a cell count (integer) followed by a colour value. With the exception of the first run of a
            //      row, the integer count of each run immediately follows the colour specifier of the preceding run with no
            //      intervening padding.
            var p = reader.ReadPoint();
            var q = reader.ReadPoint();
            var r = reader.ReadPoint();

            int nx = reader.ReadInteger();
            int ny = reader.ReadInteger();

            int localColorPrecision = reader.ReadInteger();
            if (localColorPrecision == 0)
            {
                if (reader.Descriptor.ColorSelectionMode == ColorModeType.Direct)
                    localColorPrecision = reader.Descriptor.ColorPrecision;
                else
                    localColorPrecision = reader.Descriptor.ColorIndexPrecision;
            }
            // might be either 1/2/4 or 8/16/32 here; but we want byte-sizes in ReadColor
            if (localColorPrecision >= 8)
                localColorPrecision /= 8;

            var cellRepresentationMode = reader.ReadEnum<CellRepresentationMode>();

            int totalCount = nx * ny;
            var colors = new List<MetafileColor>();
            while (colors.Count < totalCount)
            {
                // chunks are split into rows; each row is word-aligned
                // word-align the next read if necessary
                if (reader.Position % 2 == 1 && reader.HasMoreData())
                    reader.ReadByte();

                int rowCount = nx;
                while (rowCount > 0)
                {
                    if (cellRepresentationMode == CellRepresentationMode.RunLengthList)
                    {
                        int cellCount = reader.ReadInteger();
                        rowCount -= cellCount;
                        var cellColor = reader.ReadColor(localColorPrecision);
                        colors.AddRange(Enumerable.Range(0, cellCount).Select(i => cellColor));
                    }
                    else
                    {
                        rowCount--;
                        colors.Add(reader.ReadColor(localColorPrecision));
                    }
                }
            }
            return new CellArray(p, q, r, nx, ny, colors.ToArray());
        }

        public static Rectangle Rectangle(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) first corner
            // P2: (point) second corner
            return new Rectangle(reader.ReadPoint(), reader.ReadPoint());
        }

        public static Circle Circle(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) centre of circle
            // P2: (vdc) radius of circle
            return new Circle(reader.ReadPoint(), reader.ReadVdc());
        }

        public static CircularArc3Point CircularArc3Point(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) starting point
            // P2: (point) intermediate point
            // P3: (point) ending point
            return new CircularArc3Point(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint());
        }

        public static CircularArc3PointClose CircularArc3PointClose(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) starting point
            // P2: (point) intermediate point
            // P3: (point) ending point
            // P4: (enumerated) type of arc closure: valid values are
            //      0 pie closure
            //      1 chord closure
            return new CircularArc3PointClose(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadEnum<ArcClosureType>());
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

        public static CircularArcCenterClose CircularArcCenterClose(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) centre of circle
            // P2: (vdc) delta X for start vector
            // P3: (vdc) delta Y for start vector
            // P4: (vdc) delta X for end vector
            // P5: (vdc) delta Y for end vector
            // P6: (vdc) radius of circle
            // P7: (enumerated) type of arc closure: valid values are
            //      0 pie closure
            //      1 chord closure
            // NOTE: Text Encoding allows start/end vectors to be encoded as point (mostly for syntax),
            //       but it actually makes sense to store them as such - so we rely on ReadPoint to read 2x VDC for us.
            return new CircularArcCenterClose(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadVdc(), reader.ReadEnum<ArcClosureType>());
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

        public static EllipticalArcClose EllipticalArcClose(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) centre of ellipse
            // P2: (point) endpoint for first conjugate diameter
            // P3: (point) endpoint for second conjugate diameter
            // P4: (vdc) delta X for start vector
            // P5: (vdc) delta Y for start vector
            // P6: (vdc) delta X for end vector
            // P7: (vdc) delta Y for end vector
            // P8: (enumerated) type of arc closure: valid values are
            //      0 pie closure
            //      1 chord closure
            // NOTE: Text Encoding allows start/end vectors to be encoded as point (mostly for syntax),
            //       but it actually makes sense to store them as such - so we rely on ReadPoint to read 2x VDC for us.
            return new EllipticalArcClose(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadEnum<ArcClosureType>());
        }

        private static List<PointF> ReadPointList(MetafileReader reader)
        {
            var points = new List<PointF>();
            // TODO: point is 2 VDCs, but that may range from 8 bits each until up to 64 bits for a single coordinate
            //       this should probably check for 2x VDC size instead of simply 2x minimum-possible VDC size
            while (reader.HasMoreData(2))
            {
                points.Add(reader.ReadPoint());
            }

            return points;
        }
    }
}
