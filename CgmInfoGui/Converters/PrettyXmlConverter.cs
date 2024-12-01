using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Avalonia.Data.Converters;
using AvaloniaEdit.Document;

namespace CgmInfoGui.Converters;

// adapted from http://xmlflowdocument.codeplex.com/
// https://chriscavanagh.wordpress.com/2008/11/02/rendering-xml-as-a-flowdocument/
public sealed class PrettyXmlConverter : IValueConverter
{
    private const int ChildIndent = 4;
    private static string MakeName(XName name, XElement contextElement)
    {
        if (name.Namespace == null)
            return name.LocalName;

        string? prefix = contextElement.GetPrefixOfNamespace(name.Namespace);
        // no prefix: probably the default namespace
        if (string.IsNullOrEmpty(prefix))
            return name.LocalName;

        return string.Format("{0}:{1}", prefix, name.LocalName);
    }

    public TextDocument Render(XDocument document)
    {
        var lines = new List<string>();
        // declaration is not an XNode (or XObject), so we can only take care of it here
        if (document.Declaration != null)
            lines.Add(RenderLine(indent: 0, document.Declaration.ToString()));

        lines.AddRange(document.Nodes().SelectMany(n => RenderNode(n, indent: 0)));

        return new(string.Join(Environment.NewLine, lines));
    }
    public TextDocument Render(XElement element)
    {
        var lines = new List<string>(RenderElement(element, 0));
        return new(string.Join(Environment.NewLine, lines));
    }

    private static IEnumerable<string> RenderElement(XElement element, int indent)
    {
        var childIndent = ChildIndent;

        // render start tag, attributes, and short closing-tag if no content
        yield return RenderLine(indent, RenderElement(element));

        bool hasContent = HasContent(element);
        if (hasContent)
        {
            // render content
            foreach (var node in element.Nodes())
            {
                foreach (var block in RenderNode(node, indent + childIndent))
                    yield return block;
            }

            // render end tag, since it isn't a short closing-tag
            yield return RenderLine(indent, RenderEndElement(element));
        }
    }

    private static IEnumerable<string> RenderNode(XNode node, int indent)
    {
        // render a child element, if it is one
        if (node is XElement xmlElement)
            return RenderElement(xmlElement, indent);

        // try to render a list of handled node subclasses; or fall back to a generic result
        // those are assumed to be inline and simply become one line.
        IEnumerable<string> ret;
        if (node is XText xmlText)
            ret = RenderText(xmlText);
        else if (node is XComment xmlComment)
            ret = [RenderComment(xmlComment)];
        else if (node is XDocumentType xmlDoctype)
            ret = [xmlDoctype.ToString()];
        else
            ret = [RenderUnsupported(node)];

        return [RenderLine(indent, ret)];
    }

    private static string RenderLine(int indent, params IEnumerable<string> inlines)
        => new string(' ', indent) + string.Join("", inlines);

    private static IEnumerable<string> RenderElement(XElement element)
    {
        yield return "<";
        yield return MakeName(element.Name, element);

        foreach (var a in element.Attributes())
        {
            yield return " ";
            yield return MakeName(a.Name, element);
            yield return "=";
            yield return "\"";
            yield return a.Value;
            yield return "\"";
        }

        bool hasContent = HasContent(element);

        yield return hasContent ? ">" : "/>";
    }

    private static string RenderEndElement(XElement element)
        => "</" + MakeName(element.Name, element) + ">";

    private static IEnumerable<string> RenderText(XText text)
        => text.Value.Split('\n', StringSplitOptions.TrimEntries);

    private static string RenderComment(XComment comment)
        => "<!--" + comment.Value + "-->";

    private static string RenderUnsupported(XNode node)
        => node.ToString();

    private static bool HasContent(XElement element)
        => element.Nodes().Any();

    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        if (value is XDocument doc)
            return Render(doc);

        if (value is XmlNode node)
        {
            using var reader = new XmlNodeReader(node);
            reader.MoveToContent();
            value = XElement.Load(reader, LoadOptions.PreserveWhitespace);
        }

        return (value is XElement element && (parameter as string)?.ToLower() != "text")
            ? Render(element)
            : value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}
