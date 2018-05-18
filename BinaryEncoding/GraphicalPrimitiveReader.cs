using System;
using System.Collections.Generic;
using System.Linq;
using CgmInfo.Commands;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.GraphicalPrimitives;
using CgmInfo.Utilities;

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
            var points = new List<MetafilePoint>();
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
            var points = new List<MetafilePoint>();
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

        public static CircularArcCenterReversed CircularArcCenterReversed(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) centre of circle
            // P2: (vdc) delta X for start vector
            // P3: (vdc) delta Y for start vector
            // P4: (vdc) delta X for end vector
            // P5: (vdc) delta Y for end vector
            // P6: (vdc) radius of circle
            // NOTE: Text Encoding allows start/end vectors to be encoded as point (mostly for syntax),
            //       but it actually makes sense to store them as such - so we rely on ReadPoint to read 2x VDC for us.
            return new CircularArcCenterReversed(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadVdc());
        }

        public static ConnectingEdge ConnectingEdge(MetafileReader reader, CommandHeader commandHeader)
        {
            // CONNECTING EDGE: has no parameters
            return new ConnectingEdge();
        }

        public static HyperbolicArc HyperbolicArc(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) centre point
            // P2: (point) transverse radius end point
            // P3: (point) conjugate radius end point
            // P4: (vdc) start vector x component
            // P5: (vdc) start vector y component
            // P6: (vdc) end vector x component
            // P7: (vdc) end vector y component
            // NOTE: Text Encoding does not permit start/end vectors to be encoded as point here (vs. ARCBOUNDS definitions),
            //       but it actually makes sense to store them as such - we can still rely on ReadPoint to read 2x VDC for us.
            return new HyperbolicArc(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint());
        }

        public static ParabolicArc ParabolicArc(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) tangent intersection point
            // P2: (point) start point
            // P3: (point) end point
            return new ParabolicArc(reader.ReadPoint(), reader.ReadPoint(), reader.ReadPoint());
        }

        public static NonUniformBSpline NonUniformBSpline(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) spline order (=m)
            // P2: (integer) number of control points (=n)
            // P(3)-P(2+n): (points) array of control points
            // P(3+n)-P(2+2n+m): (real) list of knots, of length n+m.
            // P(3+2n+m): (real) parameter start value
            // P(4+2n+m): (real) parameter end value
            int splineOrder = reader.ReadInteger();
            int numberOfControlPoints = reader.ReadInteger();
            var controlPoints = new List<MetafilePoint>();
            for (int i = 0; i < numberOfControlPoints; i++)
                controlPoints.Add(reader.ReadPoint());
            var knots = new List<double>();
            for (int i = 0; i < splineOrder + numberOfControlPoints; i++)
                knots.Add(reader.ReadReal());
            double start = reader.ReadReal();
            double end = reader.ReadReal();
            return new NonUniformBSpline(splineOrder, controlPoints.ToArray(), knots.ToArray(), start, end);
        }

        public static NonUniformRationalBSpline NonUniformRationalBSpline(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) spline order (=m)
            // P2: (integer) number of control points (=n)
            // P(3)-P(2+n): (points) array of control points
            // P(3+n)-P(2+2n+m): (real) list of knots, of length n+m.
            // P(3+2n+m): (real) parameter start value
            // P(4+2n+m): (real) parameter end value
            // P(5+2n+m)-P(4+3n+m): (real) list of weights, of length n.
            int splineOrder = reader.ReadInteger();
            int numberOfControlPoints = reader.ReadInteger();
            var controlPoints = new List<MetafilePoint>();
            for (int i = 0; i < numberOfControlPoints; i++)
                controlPoints.Add(reader.ReadPoint());
            var knots = new List<double>();
            for (int i = 0; i < splineOrder + numberOfControlPoints; i++)
                knots.Add(reader.ReadReal());
            double start = reader.ReadReal();
            double end = reader.ReadReal();
            var weights = new List<double>();
            for (int i = 0; i < numberOfControlPoints; i++)
                weights.Add(reader.ReadReal());
            return new NonUniformRationalBSpline(splineOrder, controlPoints.ToArray(), knots.ToArray(), start, end, weights.ToArray());
        }

        public static Polybezier Polybezier(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) continuity indicator: valid values are
            //      1: discontinuous
            //      2: continuous
            //      >2 reserved for registered values
            // P2-Pn: (point) list of point sequences: each sequence defines a single bezier curve and contains 4 or 3 points
            //      according to the continuity indicator values 1 or 2, respectively (if the indicator is 2, the first curve, and
            //      only the first, is defined by 4 points).
            int continuityIndicator = reader.ReadIndex();
            var pointSequences = ReadPointList(reader);
            return new Polybezier(continuityIndicator, pointSequences.ToArray());
        }

        public static BitonalTile BitonalTile(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) compression type: valid values are
            //      0: null background
            //      1: null foreground
            //      2: T6
            //      3: T4 1-dimensional
            //      4: T4 2-dimensional
            //      5: bitmap (uncompressed)
            //      6: run length
            //      >6 reserved for registered values
            // P2: (integer) row padding indicator: valid values are non-negative integers.
            // P3: (colour) cell background colour
            // P4: (colour) cell foreground colour
            // P5: (structured data record) method-specific parameters, valid values are
            //      [null_SDR], for compression types 1-5,
            //      [(integer: i_I), (integer: 1), (integer: run-count precision)], for type=6,
            //      as defined in the Register, for type>6.
            //      Note 1 See NOTE 17, Table 1, for additional SDR formatting requirements.
            // P6 (bitstream) compressed cell colour specifiers
            int compressionType = reader.ReadIndex();
            int rowPaddingIndicator = reader.ReadInteger();
            var cellBackgroundColor = reader.ReadColor();
            var cellForegroundColor = reader.ReadColor();
            var parameters = ReadTileSDR(compressionType, reader);
            byte[] compressedCells = reader.ReadBitstream();
            return new BitonalTile(compressionType, rowPaddingIndicator, cellBackgroundColor, cellForegroundColor, parameters, compressedCells);
        }

        public static Tile Tile(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) compression type: valid values are
            //      0: null background
            //      1: null foreground
            //      2: T6
            //      3: T4 1-dimensional
            //      4: T4 2-dimensional
            //      5: bitmap (uncompressed)
            //      6: run length
            //      >6 reserved for registered values
            // P2: (integer) row padding indicator: valid values are non-negative integers.
            // P3: (integer) cell colour precision: valid values are as for the local colour precision of CELL ARRAY for
            //      compression types 0 - 5, or any value specified in the Register for compression type>6.
            // P4: (structured data record) method-specific parameters, valid values are
            //      [null_SDR], for compression types 1-5,
            //      [(integer: i_I), (integer: 1), (integer: run-count precision)], for type=6,
            //      as defined in the Register, for type>6.
            //      Note 2 See NOTE 17, Table 1, for additional SDR formatting requirements.
            // P5 (bitstream) compressed cell colour specifiers
            int compressionType = reader.ReadIndex();
            int rowPaddingIndicator = reader.ReadInteger();
            int cellColorPrecision = reader.ReadInteger();
            var parameters = ReadTileSDR(compressionType, reader);
            byte[] compressedCells = reader.ReadBitstream();
            return new Tile(compressionType, rowPaddingIndicator, cellColorPrecision, parameters, compressedCells);
        }

        private static StructuredDataRecord ReadTileSDR(int compressionType, MetafileReader reader)
        {
            switch (compressionType)
            {
                case 0: // null background
                case 1: // null foreground
                case 2: // T6
                case 3: // T4 1-dimensional
                case 4: // T4 2-dimensional
                case 5: // bitmap (uncompressed)
                    // [null_SDR], for compression types 1-5,
                    return new StructuredDataRecord(new StructuredDataElement[0]);

                case 6: // run length
                    return reader.ReadStructuredDataRecord();

                case 7: // baseline JPEG (ISO/IEC 9973)
                        // 1) JPEG COLOUR MODEL - member type, Index (IX); number of items, 1; valid values:
                        //      0 - JPEG COLOUR MODEL is the same as COLOUR MODEL of the metafile
                        //      1 - RGB
                        //      2 - CIELAB
                        //      3 - CIELUV
                        //      4 - CMYK
                        //      5 - RGB-related
                        //      Values less than 0 and greater than 5 are invalid.
                        // 2) JPEG COLOUR SUBMODEL (Applicable only when JPEG COLOUR MODEL is 5) -
                        //      member type, Index (IX); number of items, 1; valid values:
                        //      0 - YCbCr
                        //      1 - YCrCb
                        //      2 - YUV
                        //      3 - YIQ
                        //      4 - YES
                        //      5 - ADT
                        //      Values less than 0 and greater than 5 are invalid.
                        // The value of the JPEG COLOUR SUBMODEL is ignored by interpreters
                        // when the value of the JPEG COLOUR MODEL is not 5 (RGB-related)
                case 8: // LZW (ISO/IEC 9973)
                    return reader.ReadStructuredDataRecord();

                case 9: // PNG (ISO/IEC 9973)
                        // PNG is a little special: the SDR contains bitstreams, one per chunk;
                        // but bitstreams themselves have no length indication. override the SDR reader to be PNG-aware.
                    return reader.ReadStructuredDataRecord(new PNGSDRReader());

                default: // >6 reserved for registered values, >9 for values known in ISO/IEC 9973 at the time of writing
                    // TODO: as defined in the Register, for type>9.
                    return null;
            }
        }

        private static List<MetafilePoint> ReadPointList(MetafileReader reader)
        {
            var points = new List<MetafilePoint>();
            // TODO: point is 2 VDCs, but that may range from 8 bits each until up to 64 bits for a single coordinate
            //       this should probably check for 2x VDC size instead of simply 2x minimum-possible VDC size
            while (reader.HasMoreData(2))
            {
                points.Add(reader.ReadPoint());
            }

            return points;
        }

        private sealed class PNGSDRReader : StructuredDataRecordReader
        {
            protected override byte[] ReadBitStream(MetafileReader reader)
            {
                // the PNG SDR holds multiple entries of type BitStream, but the BitStream itself has no real length indication.
                // PNG uses the chunk length of the PNG chunk to do this.
                // PNG chunks look like this:
                //      DWORD DataLength
                //      DWORD Type
                //      BYTE Data[]
                //      DWORD Crc
                // Data is DataLength, and the full record is DataLength + 3 * sizeof(DWORD)
                byte[] dataLength = new byte[4];
                for (int i = 0; i < dataLength.Length; i++)
                    dataLength[i] = reader.ReadByte();

                // length is always stored in big-endian
                int length;
                if (BitConverter.IsLittleEndian)
                    length = BitConverter.ToInt32(dataLength.Reverse().ToArray(), 0);
                else
                    length = BitConverter.ToInt32(dataLength, 0);

                byte[] type = new byte[4];
                for (int i = 0; i < dataLength.Length; i++)
                    type[i] = reader.ReadByte();

                byte[] data = new byte[length];
                for (int i = 0; i < data.Length; i++)
                    data[i] = reader.ReadByte();

                byte[] crc = new byte[4];
                for (int i = 0; i < dataLength.Length; i++)
                    crc[i] = reader.ReadByte();

                // bitstream is a series of unsigned integer at fixed 16-bit precision [ISO/IEC 8632-3 7, Table 1, BS / Note 15]
                // 16 bits per entry is chosen for portability reasons and need not be filled completely; the remainder is set to 0.
                // we'll have to advance the stream to be aligned at a 16-bit boundary before we leave.
                if (length % 2 != 0)
                {
                    byte shouldBeZero = reader.ReadByte();
                    System.Diagnostics.Debug.Assert(shouldBeZero == 0);
                }

                byte[] result = new byte[dataLength.Length + type.Length + data.Length + crc.Length];
                Array.Copy(dataLength, 0, result, 0, dataLength.Length);
                Array.Copy(type, 0, result, dataLength.Length, type.Length);
                Array.Copy(data, 0, result, dataLength.Length + type.Length, data.Length);
                Array.Copy(crc, 0, result, dataLength.Length + type.Length + data.Length, crc.Length);

                return result;
            }
        }
    }
}
