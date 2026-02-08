using System.Collections.Generic;
using CgmInfo.Commands;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.GraphicalPrimitives;
using CgmInfo.Utilities;

namespace CgmInfo.Writers
{
    // [ISO/IEC 8632-3 8.6]
    internal static class GraphicalPrimitiveWriter
    {
        public static void Polyline(MetafileWriter writer, Command command)
        {
            var polyline = (Polyline)command;
            // P1-Pn: (point) n (X,Y) polyline vertices
            WritePointList(writer, polyline.Points);
        }

        public static void DisjointPolyline(MetafileWriter writer, Command command)
        {
            var disjointPolyline = (DisjointPolyline)command;
            // P1-Pn: (point) n (X,Y) line segment endpoints
            WritePointList(writer, disjointPolyline.Points);
        }

        public static void Polymarker(MetafileWriter writer, Command command)
        {
            var polymarker = (Polymarker)command;
            // P1-Pn: (point) n (X,Y) marker positions
            WritePointList(writer, polymarker.Points);
        }

        public static void Text(MetafileWriter writer, Command command)
        {
            var text = (TextCommand)command;
            // P1: (point) text position
            writer.WritePoint(text.Position);
            // P2: (enumerated) final / not - final flag: valid values are
            //      0 not final
            //      1 final
            writer.WriteEnum(text.Final);
            // P3: (string) text string
            writer.WriteString(text.Text);
        }

        public static void RestrictedText(MetafileWriter writer, Command command)
        {
            var restrictedText = (RestrictedText)command;
            // P1: (vdc) delta width
            writer.WriteVdc(restrictedText.DeltaWidth);
            // P2: (vdc) delta height
            writer.WriteVdc(restrictedText.DeltaHeight);
            // P3: (point) text position
            writer.WritePoint(restrictedText.Position);
            // P4: (enumerated) final / not - final flag: valid values are
            //      0 not final
            //      1 final
            writer.WriteEnum(restrictedText.Final);
            // P5: (string) text string
            writer.WriteString(restrictedText.Text);
        }

        public static void AppendText(MetafileWriter writer, Command command)
        {
            var appendText = (AppendText)command;
            // P1: (enumerated) final / not - final flag: valid values are
            //      0 not final
            //      1 final
            writer.WriteEnum(appendText.Final);
            // P2: (string) text string
            writer.WriteString(appendText.Text);
        }

        public static void Polygon(MetafileWriter writer, Command command)
        {
            var polygon = (Polygon)command;
            // P1-Pn: (point) n (X,Y) polygon vertices
            WritePointList(writer, polygon.Points);
        }

        public static void PolygonSet(MetafileWriter writer, Command command)
        {
            var polygonSet = (PolygonSet)command;
            for (int i = 0; i < polygonSet.Points.Length; i++)
            {
                // P(i): (point) (X,Y) polygon vertex
                writer.WritePoint(polygonSet.Points[i]);
                // P(i+1): (enumerated) edge out flag, indicating closures and edge visibility: valid values are
                //      0 invisible
                //      1 visible
                //      2 close, invisible
                //      3 close, visible
                writer.WriteEnum(polygonSet.Flags[i]);
            }
        }

        public static void CellArray(MetafileWriter writer, Command command)
        {
            var cellArray = (CellArray)command;
            // P1: (point) corner point P
            writer.WritePoint(cellArray.CornerPointP);
            // P2: (point) corner point Q
            writer.WritePoint(cellArray.CornerPointQ);
            // P3: (point) corner point R
            writer.WritePoint(cellArray.CornerPointR);
            // P4: (integer) nx
            writer.WriteInteger(cellArray.NX);
            // P5: (integer) ny
            writer.WriteInteger(cellArray.NY);
            // P6: (integer) local colour precision: valid values are 0, 1, 2, 4, 8, 16, 24, and 32. If the value is zero (the
            //      'default colour precision indicator' value), the COLOUR (INDEX) PRECISION for the picture indicates the
            //      precision with which the colour list is encoded. If the value is non-zero, the precision with which the colour
            //      data is encoded is given by the value.
            // TODO: do we want to store the original local color precision?
            writer.WriteInteger(0);
            // P7: (enumerated) cell representation mode: valid values are
            //      0 run length list mode
            //      1 packed list mode
            // TODO: support RLE?
            writer.WriteEnum(CellRepresentationMode.PackedList);
            // P8: (colour list) array of cell colour values.
            //      If the COLOUR SELECTION MODE is 'direct', the values will be direct colour values. If the COLOUR
            //      SELECTION MODE is 'indexed', the values will be indexes into the COLOUR TABLE.
            //      If the cell representation mode is 'packed list', the colour values are represented by rows of values, each
            //      row starting on a word boundary. If the cell representation mode is 'run length', the colour list values are
            //      represented by rows broken into runs of constant colour; each row starts on a word boundary. Each list
            //      item consists of a cell count (integer) followed by a colour value. With the exception of the first run of a
            //      row, the integer count of each run immediately follows the colour specifier of the preceding run with no
            //      intervening padding.
            int cellCounter = 0;
            foreach (var color in cellArray.Colors)
            {
                writer.WriteColor(color);
                if (cellCounter++ > cellArray.NX)
                {
                    // chunks are split into rows; each row is word-aligned
                    // word-align the next row if necessary
                    writer.EnsureWordAligned();
                    cellCounter = 0;
                }
            }
        }

        public static void Rectangle(MetafileWriter writer, Command command)
        {
            var rectangle = (Rectangle)command;
            // P1: (point) first corner
            writer.WritePoint(rectangle.FirstCorner);
            // P2: (point) second corner
            writer.WritePoint(rectangle.SecondCorner);
        }

        public static void Circle(MetafileWriter writer, Command command)
        {
            var circle = (Circle)command;
            // P1: (point) centre of circle
            writer.WritePoint(circle.Center);
            // P2: (vdc) radius of circle
            writer.WriteVdc(circle.Radius);
        }

        public static void CircularArc3Point(MetafileWriter writer, Command command)
        {
            var circularArc3Point = (CircularArc3Point)command;
            // P1: (point) starting point
            writer.WritePoint(circularArc3Point.Start);
            // P2: (point) intermediate point
            writer.WritePoint(circularArc3Point.Intermediate);
            // P3: (point) ending point
            writer.WritePoint(circularArc3Point.End);
        }

        public static void CircularArc3PointClose(MetafileWriter writer, Command command)
        {
            var circularArc3PointClose = (CircularArc3PointClose)command;
            // P1: (point) starting point
            writer.WritePoint(circularArc3PointClose.Start);
            // P2: (point) intermediate point
            writer.WritePoint(circularArc3PointClose.Intermediate);
            // P3: (point) ending point
            writer.WritePoint(circularArc3PointClose.End);
            // P4: (enumerated) type of arc closure: valid values are
            //      0 pie closure
            //      1 chord closure
            writer.WriteEnum(circularArc3PointClose.Closure);
        }

        public static void CircularArcCenter(MetafileWriter writer, Command command)
        {
            var circularArcCenter = (CircularArcCenter)command;
            // P1: (point) centre of circle
            writer.WritePoint(circularArcCenter.Center);
            // P2: (vdc) delta X for start vector
            writer.WriteVdc(circularArcCenter.Start.X);
            // P3: (vdc) delta Y for start vector
            writer.WriteVdc(circularArcCenter.Start.Y);
            // P4: (vdc) delta X for end vector
            writer.WriteVdc(circularArcCenter.End.X);
            // P5: (vdc) delta Y for end vector
            writer.WriteVdc(circularArcCenter.End.Y);
            // P6: (vdc) radius of circle
            writer.WriteVdc(circularArcCenter.Radius);
        }

        public static void CircularArcCenterClose(MetafileWriter writer, Command command)
        {
            var circularArcCenterClose = (CircularArcCenterClose)command;
            // P1: (point) centre of circle
            writer.WritePoint(circularArcCenterClose.Center);
            // P2: (vdc) delta X for start vector
            writer.WriteVdc(circularArcCenterClose.Start.X);
            // P3: (vdc) delta Y for start vector
            writer.WriteVdc(circularArcCenterClose.Start.Y);
            // P4: (vdc) delta X for end vector
            writer.WriteVdc(circularArcCenterClose.End.X);
            // P5: (vdc) delta Y for end vector
            writer.WriteVdc(circularArcCenterClose.End.Y);
            // P6: (vdc) radius of circle
            // P7: (enumerated) type of arc closure: valid values are
            writer.WriteVdc(circularArcCenterClose.Radius);
            //      0 pie closure
            //      1 chord closure
            writer.WriteEnum(circularArcCenterClose.Closure);
        }

        public static void Ellipse(MetafileWriter writer, Command command)
        {
            var ellipse = (Ellipse)command;
            // P1: (point) centre of ellipse
            writer.WritePoint(ellipse.Center);
            // P2: (point) endpoint of first conjugate diameter
            writer.WritePoint(ellipse.FirstConjugateDiameter);
            // P3: (point) endpoint of second conjugate diameter
            writer.WritePoint(ellipse.SecondConjugateDiameter);
        }

        public static void EllipticalArc(MetafileWriter writer, Command command)
        {
            var ellipticalArc = (EllipticalArc)command;
            // P1: (point) centre of ellipse
            writer.WritePoint(ellipticalArc.Center);
            // P2: (point) endpoint for first conjugate diameter
            writer.WritePoint(ellipticalArc.FirstConjugateDiameter);
            // P3: (point) endpoint for second conjugate diameter
            writer.WritePoint(ellipticalArc.SecondConjugateDiameter);
            // P4: (vdc) delta X for start vector
            writer.WriteVdc(ellipticalArc.Start.X);
            // P5: (vdc) delta Y for start vector
            writer.WriteVdc(ellipticalArc.Start.Y);
            // P6: (vdc) delta X for end vector
            writer.WriteVdc(ellipticalArc.End.X);
            // P7: (vdc) delta Y for end vector
            writer.WriteVdc(ellipticalArc.End.Y);
        }

        public static void EllipticalArcClose(MetafileWriter writer, Command command)
        {
            var ellipticalArcClose = (EllipticalArcClose)command;
            // P1: (point) centre of ellipse
            writer.WritePoint(ellipticalArcClose.Center);
            // P2: (point) endpoint for first conjugate diameter
            writer.WritePoint(ellipticalArcClose.FirstConjugateDiameter);
            // P3: (point) endpoint for second conjugate diameter
            writer.WritePoint(ellipticalArcClose.SecondConjugateDiameter);
            // P4: (vdc) delta X for start vector
            writer.WriteVdc(ellipticalArcClose.Start.X);
            // P5: (vdc) delta Y for start vector
            writer.WriteVdc(ellipticalArcClose.Start.Y);
            // P6: (vdc) delta X for end vector
            writer.WriteVdc(ellipticalArcClose.End.X);
            // P7: (vdc) delta Y for end vector
            writer.WriteVdc(ellipticalArcClose.End.Y);
            // P8: (enumerated) type of arc closure: valid values are
            //      0 pie closure
            //      1 chord closure
            writer.WriteEnum(ellipticalArcClose.Closure);
        }

        public static void CircularArcCenterReversed(MetafileWriter writer, Command command)
        {
            var circularArcCenterReversed = (CircularArcCenterReversed)command;
            // P1: (point) centre of circle
            writer.WritePoint(circularArcCenterReversed.Center);
            // P2: (vdc) delta X for start vector
            writer.WriteVdc(circularArcCenterReversed.Start.X);
            // P3: (vdc) delta Y for start vector
            writer.WriteVdc(circularArcCenterReversed.Start.Y);
            // P4: (vdc) delta X for end vector
            writer.WriteVdc(circularArcCenterReversed.End.X);
            // P5: (vdc) delta Y for end vector
            writer.WriteVdc(circularArcCenterReversed.End.Y);
            // P6: (vdc) radius of circle
            writer.WriteVdc(circularArcCenterReversed.Radius);
        }

        public static void ConnectingEdge(MetafileWriter writer, Command command)
        {
            // CONNECTING EDGE: has no parameters
        }

        public static void HyperbolicArc(MetafileWriter writer, Command command)
        {
            var hyperbolicArc = (HyperbolicArc)command;
            // P1: (point) centre point
            writer.WritePoint(hyperbolicArc.Center);
            // P2: (point) transverse radius end point
            writer.WritePoint(hyperbolicArc.TraverseRadiusEndPoint);
            // P3: (point) conjugate radius end point
            writer.WritePoint(hyperbolicArc.ConjugateRadiusEndPoint);
            // P4: (vdc) start vector x component
            writer.WriteVdc(hyperbolicArc.Start.X);
            // P5: (vdc) start vector y component
            writer.WriteVdc(hyperbolicArc.Start.Y);
            // P6: (vdc) end vector x component
            writer.WriteVdc(hyperbolicArc.End.X);
            // P7: (vdc) end vector y component
            writer.WriteVdc(hyperbolicArc.End.Y);
        }

        public static void ParabolicArc(MetafileWriter writer, Command command)
        {
            var parabolicArc = (ParabolicArc)command;
            // P1: (point) tangent intersection point
            writer.WritePoint(parabolicArc.TangentIntersectionPoint);
            // P2: (point) start point
            writer.WritePoint(parabolicArc.Start);
            // P3: (point) end point
            writer.WritePoint(parabolicArc.End);
        }

        public static void NonUniformBSpline(MetafileWriter writer, Command command)
        {
            var nonUniformBSpline = (NonUniformBSpline)command;
            // P1: (integer) spline order (=m)
            writer.WriteInteger(nonUniformBSpline.SplineOrder);
            // P2: (integer) number of control points (=n)
            writer.WriteInteger(nonUniformBSpline.ControlPoints.Length);
            // P(3)-P(2+n): (points) array of control points
            WritePointList(writer, nonUniformBSpline.ControlPoints);
            // P(3+n)-P(2+2n+m): (real) list of knots, of length n+m.
            foreach (double knot in nonUniformBSpline.Knots)
                writer.WriteReal(knot);
            // P(3+2n+m): (real) parameter start value
            writer.WriteReal(nonUniformBSpline.Start);
            // P(4+2n+m): (real) parameter end value
            writer.WriteReal(nonUniformBSpline.End);
        }

        public static void NonUniformRationalBSpline(MetafileWriter writer, Command command)
        {
            var nonUniformRationalBSpline = (NonUniformRationalBSpline)command;
            // P1: (integer) spline order (=m)
            writer.WriteInteger(nonUniformRationalBSpline.SplineOrder);
            // P2: (integer) number of control points (=n)
            writer.WriteInteger(nonUniformRationalBSpline.ControlPoints.Length);
            // P(3)-P(2+n): (points) array of control points
            WritePointList(writer, nonUniformRationalBSpline.ControlPoints);
            // P(3+n)-P(2+2n+m): (real) list of knots, of length n+m.
            foreach (double knot in nonUniformRationalBSpline.Knots)
                writer.WriteReal(knot);
            // P(3+2n+m): (real) parameter start value
            writer.WriteReal(nonUniformRationalBSpline.Start);
            // P(4+2n+m): (real) parameter end value
            writer.WriteReal(nonUniformRationalBSpline.End);
            // P(5+2n+m)-P(4+3n+m): (real) list of weights, of length n.
            foreach (double weight in nonUniformRationalBSpline.Weights)
                writer.WriteReal(weight);
        }

        public static void Polybezier(MetafileWriter writer, Command command)
        {
            var polybezier = (Polybezier)command;
            // P1: (index) continuity indicator: valid values are
            //      1: discontinuous
            //      2: continuous
            //      >2 reserved for registered values
            writer.WriteIndex(polybezier.ContinuityIndicator);
            // P2-Pn: (point) list of point sequences: each sequence defines a single bezier curve and contains 4 or 3 points
            //      according to the continuity indicator values 1 or 2, respectively (if the indicator is 2, the first curve, and
            //      only the first, is defined by 4 points).
            WritePointList(writer, polybezier.PointSequences);
        }

        public static void BitonalTile(MetafileWriter writer, Command command)
        {
            var bitonalTile = (BitonalTile)command;
            // P1: (index) compression type: valid values are
            //      0: null background
            //      1: null foreground
            //      2: T6
            //      3: T4 1-dimensional
            //      4: T4 2-dimensional
            //      5: bitmap (uncompressed)
            //      6: run length
            //      >6 reserved for registered values
            writer.WriteIndex(bitonalTile.CompressionType);
            // P2: (integer) row padding indicator: valid values are non-negative integers.
            writer.WriteInteger(bitonalTile.RowPaddingIndicator);
            // P3: (colour) cell background colour
            writer.WriteColor(bitonalTile.CellBackgroundColor);
            // P4: (colour) cell foreground colour
            writer.WriteColor(bitonalTile.CellForegroundColor);
            // P5: (structured data record) method-specific parameters, valid values are
            //      [null_SDR], for compression types 1-5,
            //      [(integer: i_I), (integer: 1), (integer: run-count precision)], for type=6,
            //      as defined in the Register, for type>6.
            //      Note 1 See NOTE 17, Table 1, for additional SDR formatting requirements.
            writer.WriteStructuredDataRecord(bitonalTile.Parameters);
            // P6 (bitstream) compressed cell colour specifiers
            writer.WriteBitStream(bitonalTile.CompressedCells);
        }

        public static void Tile(MetafileWriter writer, Command command)
        {
            var tile = (Tile)command;
            // P1: (index) compression type: valid values are
            //      0: null background
            //      1: null foreground
            //      2: T6
            //      3: T4 1-dimensional
            //      4: T4 2-dimensional
            //      5: bitmap (uncompressed)
            //      6: run length
            //      >6 reserved for registered values
            writer.WriteIndex(tile.CompressionType);
            // P2: (integer) row padding indicator: valid values are non-negative integers.
            writer.WriteInteger(tile.RowPaddingIndicator);
            // P3: (integer) cell colour precision: valid values are as for the local colour precision of CELL ARRAY for
            //      compression types 0 - 5, or any value specified in the Register for compression type>6.
            writer.WriteInteger(tile.CellColorPrecision);
            // P4: (structured data record) method-specific parameters, valid values are
            //      [null_SDR], for compression types 1-5,
            //      [(integer: i_I), (integer: 1), (integer: run-count precision)], for type=6,
            //      as defined in the Register, for type>6.
            //      Note 2 See NOTE 17, Table 1, for additional SDR formatting requirements.
            writer.WriteStructuredDataRecord(tile.Parameters);
            // P5 (bitstream) compressed cell colour specifiers
            writer.WriteBitStream(tile.CompressedCells);
        }

        private static void WritePointList(MetafileWriter writer, IEnumerable<MetafilePoint> points)
        {
            foreach (var point in points)
                writer.WritePoint(point);
        }
    }
}
