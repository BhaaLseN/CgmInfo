using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal
{
    public class APSStructureContext : NodeContext
    {
        public void AddAttributeNode(ApplicationStructureAttribute applicationStructureAttribute)
        {
            var allValues = applicationStructureAttribute.DataRecord.Elements.SelectMany(el => el.Values).ToArray();
            switch (applicationStructureAttribute.AttributeType.ToUpperInvariant())
            {
                #region WebCGM 2.0 Intelligent Content [WebCGM20-IC]
                // http://www.w3.org/TR/webcgm20/WebCGM20-IC.html

                case "CONTENT": // [WebCGM20-IC 3.2.2.8]
                case "INTERACTIVITY": // [WebCGM20-IC 3.2.2.10]
                case "LAYERNAME": // [WebCGM20-IC 3.2.2.4]
                case "LAYERDESC": // [WebCGM20-IC 3.2.2.5]
                case "NAME": // [WebCGM20-IC 3.2.2.7]
                case "SCREENTIP": // [WebCGM20-IC 3.2.2.6]
                case "VISIBILITY": // [WebCGM20-IC 3.2.2.9]
                    // simple attribute with one value; show just the value (unless it doesn't have one value for whatever reason)
                    if (allValues.Length != 1)
                        goto default;

                    AddNode("@{0} = '{1}'", applicationStructureAttribute.AttributeType, allValues[0]);
                    break;

                case "LINKURI": // [WebCGM20-IC 3.2.2.3]
                    // linkuri has exactly 3 attributes; if not, show the default stuff
                    if (allValues.Length != 3)
                        goto default;

                    // The data record is an SDR of one member, containing three strings (type SF, String Fixed).
                    // The first string is the link destination, a IRI,
                    // the second string (possibly null) is a Link Title parameter,
                    // and the third string (possibly null) is the Behavior parameter.
                    // Note that a null string is a zero-length string, and is not the same as an omitted parameter. The parameter must not be omitted.
                    AddNode("@{0} <a href=\"{1}\" target=\"{3}\">{2}</a>",
                        applicationStructureAttribute.AttributeType,
                        allValues[0], allValues[1], allValues[2]);
                    break;

                case "REGION": // [WebCGM20-IC 3.2.2.1]
                    if (allValues.Length < 2)
                        goto default;

                    WebCGM20Region(applicationStructureAttribute, allValues);
                    break;
                case "VIEWCONTEXT": // [WebCGM20-IC 3.2.2.2]
                    // viewcontext has exactly 4 attributes; the corner coordinates of a rectangle
                    if (allValues.Length != 4)
                        goto default;

                    // The data record is an SDR of 1 member of type VDC defining two corner points of a rectangle.
                    float startX = Convert.ToSingle(allValues[0]);
                    float startY = Convert.ToSingle(allValues[1]);
                    float endX = Convert.ToSingle(allValues[2]);
                    float endY = Convert.ToSingle(allValues[3]);
                    var start = new PointF(startX, startY);
                    var end = new PointF(endX, endY);

                    var regionNode = AddNode("@{0} Viewcontext ({1} by {2})", applicationStructureAttribute.AttributeType,
                        Math.Abs(endX - startX), Math.Abs(endY - startY));
                    regionNode.Nodes.AddRange(new[]
                    {
                        new SimpleNode(string.Format("Start: {0}", start)),
                        new SimpleNode(string.Format("End: {0}", end)),
                    });
                    break;

                #endregion

                default:
                    // in case theres just a single value; show it directly.
                    if (allValues.Length == 1)
                    {
                        AddNode("@{0} = '{1}'", applicationStructureAttribute.AttributeType, allValues[0]);
                    }
                    else
                    {
                        var attributeNode = AddNode("@{0} [{1} values]", applicationStructureAttribute.AttributeType, allValues.Length);
                        attributeNode.Nodes.AddRange(allValues.Select(value => new SimpleNode(Convert.ToString(value))));
                    }
                    break;
            }
        }

        private void WebCGM20Region(ApplicationStructureAttribute applicationStructureAttribute, object[] allValues)
        {
            // The data record is an SDR of one or more member pairs (i.e., 2*m members, m>=1).
            // Each member-pair defines a simple region: the first member is of data type Index, whose valid values are:
            //      1. rectangle
            //      2. ellipse
            //      3. polygon
            //      4. continuous polybezier
            //
            // The second member is type VDC and contains:
            //      for rectangle: 4 VDC defining two corner points;
            //      for ellipse: 6 VDC defining respectively the center, and two CDP endpoints;
            //      for polygon: 2n VDC defining polygon vertex points
            //      for polybezier: 2 * (3n + 1) VDC values, representing 3n + 1 points, defining n contiguous cubic bezier segments;
            //
            // For polygon and polybezier regions, closure is implicit (if the last given point does not match the first,
            // then the viewer closes the region with a straight line segment from the last to the first).
            float regionType = Convert.ToSingle(allValues[0]);
            switch ((int)regionType)
            {
                case 1: // Rectangle
                    if (allValues.Length == 5)
                    {
                        float startX = Convert.ToSingle(allValues[1]);
                        float startY = Convert.ToSingle(allValues[2]);
                        float endX = Convert.ToSingle(allValues[3]);
                        float endY = Convert.ToSingle(allValues[4]);
                        var start = new PointF(startX, startY);
                        var end = new PointF(endX, endY);

                        var regionNode = AddNode("@{0} Rectangle ({1} by {2})", applicationStructureAttribute.AttributeType,
                            Math.Abs(endX - startX), Math.Abs(endY - startY));
                        regionNode.Nodes.AddRange(new[]
                        {
                            new SimpleNode(string.Format("Start: {0}", start)),
                            new SimpleNode(string.Format("End: {0}", end)),
                        });
                    }
                    else
                    {
                        var regionNode = AddNode("@{0} Rectangle (malformed values)", applicationStructureAttribute.AttributeType);
                        regionNode.Nodes.AddRange(allValues.Skip(1).Select(o => new SimpleNode(Convert.ToString(o))));
                    }
                    break;
                case 2: // Ellipse
                    if (allValues.Length == 7)
                    {
                        float centerX = Convert.ToSingle(allValues[1]);
                        float centerY = Convert.ToSingle(allValues[2]);
                        float focus1X = Convert.ToSingle(allValues[3]);
                        float focus1Y = Convert.ToSingle(allValues[4]);
                        float focus2X = Convert.ToSingle(allValues[5]);
                        float focus2Y = Convert.ToSingle(allValues[6]);
                        var center = new PointF(centerX, centerY);
                        var focus1 = new PointF(focus1X, focus1Y);
                        var focus2 = new PointF(focus2X, focus2Y);

                        var regionNode = AddNode("@{0} Ellipse (at {1})", applicationStructureAttribute.AttributeType, center);
                        regionNode.Nodes.AddRange(new[]
                        {
                            new SimpleNode(string.Format("Center: {0}", center)),
                            new SimpleNode(string.Format("Focal Point 1: {0}", focus1)),
                            new SimpleNode(string.Format("Focal Point 2: {0}", focus2)),
                        });
                    }
                    else
                    {
                        var regionNode = AddNode("@{0} Ellipse (malformed values)", applicationStructureAttribute.AttributeType);
                        regionNode.Nodes.AddRange(allValues.Skip(1).Select(o => new SimpleNode(Convert.ToString(o))));
                    }
                    break;
                case 3: // Polygon
                    if (allValues.Length % 2 == 1)
                    {
                        var points = allValues.Skip(1)
                            .Select((o, i) => new { Index = i, Float = Convert.ToSingle(o) })
                            .GroupBy(a => a.Index / 2)
                            .Select(g => new PointF(g.First().Float, g.Skip(1).First().Float))
                            .ToArray();
                        var regionNode = AddNode("@{0} Polygon ({1} points)", applicationStructureAttribute.AttributeType, points.Length);
                        regionNode.Nodes.AddRange(points.Select(p => new SimpleNode(p.ToString())));
                    }
                    else
                    {
                        var regionNode = AddNode("@{0} Rectangle (malformed values)", applicationStructureAttribute.AttributeType);
                        regionNode.Nodes.AddRange(allValues.Skip(1).Select(o => new SimpleNode(Convert.ToString(o))));
                    }
                    break;
                case 4: // Continuous Polybezier
                    if (allValues.Length % 6 == 3)
                    {
                        var firstPoint = new PointF(Convert.ToSingle(allValues.Skip(1).First()), Convert.ToSingle(allValues.Skip(2).First()));
                        var pointGroups = allValues.Skip(3)
                            .Select((o, i) => new { Index = i, Float = Convert.ToSingle(o) })
                            .GroupBy(a => a.Index / 6)
                            .Select(g => new[]
                            {
                                new PointF(g.First().Float, g.Skip(1).First().Float),
                                new PointF(g.Skip(2).First().Float, g.Skip(3).First().Float),
                                new PointF(g.Skip(4).First().Float, g.Skip(5).First().Float),
                            })
                            .ToArray();
                        var points = new List<PointF[]>();
                        var lastPoint = firstPoint;
                        foreach (var pointGroup in pointGroups)
                        {
                            points.Add(new[]
                            {
                                lastPoint,
                                pointGroup[0],
                                pointGroup[1],
                                pointGroup[2],
                            });
                            lastPoint = pointGroup[2];
                        }
                        var regionNode = AddNode("@{0} Polybezier ({1} points)", applicationStructureAttribute.AttributeType, points.Count);
                        regionNode.Nodes.AddRange(points.Select(p => new SimpleNode(string.Format("{0}, {1}, {2}, {3}", p[0], p[1], p[2], p[3]))));
                    }
                    else
                    {
                        var regionNode = AddNode("@{0} Polybezier (malformed values)", applicationStructureAttribute.AttributeType);
                        regionNode.Nodes.AddRange(allValues.Skip(1).Select(o => new SimpleNode(Convert.ToString(o))));
                    }
                    break;
                default:
                    var fallbackNode = AddNode("@{0} Region Type {1} (unsupported)", applicationStructureAttribute.AttributeType, allValues[0]);
                    fallbackNode.Nodes.AddRange(allValues.Skip(1).Select(o => new SimpleNode(Convert.ToString(o))));
                    break;
            }
        }
    }
}
