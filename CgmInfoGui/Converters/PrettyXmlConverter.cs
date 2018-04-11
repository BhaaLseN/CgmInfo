using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;

namespace CgmInfoGui.Converters
{
    // adapted from http://xmlflowdocument.codeplex.com/
    // https://chriscavanagh.wordpress.com/2008/11/02/rendering-xml-as-a-flowdocument/
    class PrettyXmlConverter : DependencyObject, IValueConverter
    {
        #region Dependency properties

        public static DependencyProperty BracketStyleProperty = RegisterStyle("BracketStyle", MakeStyle(Colors.Blue));
        public static DependencyProperty ElementNameStyleProperty = RegisterStyle("ElementNameStyle", MakeStyle(Colors.DarkRed));
        public static DependencyProperty SpaceStyleProperty = RegisterStyle("SpaceStyle", null);
        public static DependencyProperty AttributeNameStyleProperty = RegisterStyle("AttributeNameStyle", MakeStyle(Colors.Red));
        public static DependencyProperty NamespaceValueStyleProperty = RegisterStyle("NamespaceValueStyle", MakeStyle(Colors.Red));
        public static DependencyProperty AssignmentStyleProperty = RegisterStyle("AssignmentStyle", MakeStyle(Colors.DarkBlue));
        public static DependencyProperty QuoteStyleProperty = RegisterStyle("QuoteStyle", MakeStyle(Colors.DarkBlue));
        public static DependencyProperty AttributeValueStyleProperty = RegisterStyle("AttributeValueStyle", MakeStyle(Colors.Black));
        public static DependencyProperty ElementValueStyleProperty = RegisterStyle("ElementValueStyle", MakeStyle(Colors.Black));
        public static DependencyProperty CommentStyleProperty = RegisterStyle("CommentStyle", MakeStyle(Colors.Green));
        public static DependencyProperty NotificationStyleProperty = RegisterStyle("NotificationStyle", MakeStyle(Colors.DarkGreen, FontStyles.Italic));
        public static DependencyProperty ChildIndentProperty = DependencyProperty.Register("ChildIndent", typeof(double), typeof(PrettyXmlConverter), new PropertyMetadata(25d));
        public static DependencyProperty FontFamilyProperty = FlowDocument.FontFamilyProperty.AddOwner(typeof(PrettyXmlConverter), new FrameworkPropertyMetadata(new FontFamily("Consolas")));
        public static DependencyProperty FontSizeProperty = FlowDocument.FontSizeProperty.AddOwner(typeof(PrettyXmlConverter), new FrameworkPropertyMetadata(14d));
        public static DependencyProperty PagePaddingProperty = FlowDocument.PagePaddingProperty.AddOwner(typeof(PrettyXmlConverter), new FrameworkPropertyMetadata(new Thickness(5)));

        #endregion

        #region Properties

        public Style BracketStyle { get { return (Style)GetValue(BracketStyleProperty); } set { SetValue(BracketStyleProperty, value); } }
        public Style ElementNameStyle { get { return (Style)GetValue(ElementNameStyleProperty); } set { SetValue(ElementNameStyleProperty, value); } }
        public Style SpaceStyle { get { return (Style)GetValue(SpaceStyleProperty); } set { SetValue(SpaceStyleProperty, value); } }
        public Style AttributeNameStyle { get { return (Style)GetValue(AttributeNameStyleProperty); } set { SetValue(AttributeNameStyleProperty, value); } }
        public Style NamespaceValueStyle { get { return (Style)GetValue(NamespaceValueStyleProperty); } set { SetValue(NamespaceValueStyleProperty, value); } }
        public Style AssignmentStyle { get { return (Style)GetValue(AssignmentStyleProperty); } set { SetValue(AssignmentStyleProperty, value); } }
        public Style QuoteStyle { get { return (Style)GetValue(QuoteStyleProperty); } set { SetValue(QuoteStyleProperty, value); } }
        public Style AttributeValueStyle { get { return (Style)GetValue(AttributeValueStyleProperty); } set { SetValue(AttributeValueStyleProperty, value); } }
        public Style ElementValueStyle { get { return (Style)GetValue(ElementValueStyleProperty); } set { SetValue(ElementValueStyleProperty, value); } }
        public Style CommentStyle { get { return (Style)GetValue(CommentStyleProperty); } set { SetValue(CommentStyleProperty, value); } }
        public Style NotificationStyle { get { return (Style)GetValue(NotificationStyleProperty); } set { SetValue(NotificationStyleProperty, value); } }
        public double ChildIndent { get { return (double)GetValue(ChildIndentProperty); } set { SetValue(ChildIndentProperty, value); } }
        public FontFamily FontFamily { get { return (FontFamily)GetValue(FontFamilyProperty); } set { SetValue(FontFamilyProperty, value); } }
        public double FontSize { get { return (double)GetValue(FontSizeProperty); } set { SetValue(FontSizeProperty, value); } }
        public Thickness PagePadding { get { return (Thickness)GetValue(PagePaddingProperty); } set { SetValue(PagePaddingProperty, value); } }

        #endregion

        #region Static helpers

        private static DependencyProperty RegisterStyle(string name, Style defaultStyle)
        {
            return DependencyProperty.Register(
                name,
                typeof(Style),
                typeof(PrettyXmlConverter),
                new PropertyMetadata(defaultStyle));
        }

        private static Style MakeStyle(Color color)
        {
            return MakeStyle(color, null);
        }

        private static Style MakeStyle(Color color, FontStyle? fontStyle)
        {
            var style = new Style();
            style.Setters.Add(new Setter(Run.ForegroundProperty, new SolidColorBrush(color)));
            if (fontStyle.HasValue)
                style.Setters.Add(new Setter(Run.FontStyleProperty, fontStyle.Value));
            return style;
        }

        private static string MakeName(XName name, XElement contextElement)
        {
            if (name.Namespace == null)
                return name.LocalName;

            string prefix = contextElement.GetPrefixOfNamespace(name.Namespace);
            // no prefix: probably the default namespace
            if (string.IsNullOrEmpty(prefix))
                return name.LocalName;

            return string.Format("{0}:{1}", prefix, name.LocalName);
        }

        private static bool IsNamespaceDeclaration(XAttribute a)
        {
            if (string.Equals(a.Name.LocalName, "xmlns", StringComparison.InvariantCultureIgnoreCase))
                return true;
            if (a.Name.Namespace == XNamespace.Xmlns)
                return true;

            return false;
        }

        #endregion

        public FlowDocument Render(XDocument document)
        {
            var doc = new FlowDocument
            {
                FontFamily = this.FontFamily,
                FontSize = this.FontSize,
                PagePadding = this.PagePadding
            };

            // declaration is not an XNode (or XObject), so we can only take care of it here
            if (document.Declaration != null)
                doc.Blocks.Add(RenderLine(new[] { Bracket(document.Declaration.ToString()) }, 0, 0));

            foreach (var block in document.Nodes().SelectMany(n => RenderNode(n, 0)))
            {
                doc.Blocks.Add(block);
            }

            return doc;
        }
        public FlowDocument Render(XElement element)
        {
            var doc = new FlowDocument
            {
                FontFamily = this.FontFamily,
                FontSize = this.FontSize,
                PagePadding = this.PagePadding
            };

            foreach (var b in RenderElement(element, 0))
            {
                doc.Blocks.Add(b);
            }

            return doc;
        }

        #region Block helpers

        public IEnumerable<Block> RenderElement(XElement element, double indent)
        {
            var childIndent = ChildIndent;
            var hanging = childIndent / 2;

            // render start tag, attributes, and short closing-tag if no content
            yield return RenderLine(RenderElement(element), indent, hanging);

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
                yield return RenderLine(RenderEndElement(element), indent, hanging);
            }
        }

        private IEnumerable<Block> RenderNode(XNode node, double indent)
        {
            // render a child element, if it is one
            if (node is XElement xmlElement)
                return RenderElement(xmlElement, indent);

            // try to render a list of handled node subclasses; or fall back to a generic result
            // those are assumed to be inline and simply become one line.
            IEnumerable<Inline> ret;
            if (node is XText xmlText)
                ret = RenderText(xmlText);
            else if (node is XComment xmlComment)
                ret = RenderComment(xmlComment);
            else if (node is XDocumentType xmlDoctype)
                ret = new[] { Bracket(xmlDoctype.ToString()) };
            else
                ret = RenderUnsupported(node);

            return new[] { RenderLine(ret, indent, ChildIndent / 2) };
        }

        public Paragraph RenderLine(IEnumerable<Inline> inlines, double indent, double hanging)
        {
            var paragraph = new Paragraph
            {
                TextAlignment = TextAlignment.Left,
                IsHyphenationEnabled = false,
                TextIndent = -hanging,
                Margin = new Thickness(indent + hanging, 0, 0, 0)
            };

            paragraph.Inlines.AddRange(inlines);

            return paragraph;
        }

        #endregion

        #region Inline helpers

        public IEnumerable<Inline> RenderElement(XElement element)
        {
            yield return Bracket("<");
            yield return ElementName(MakeName(element.Name, element));

            foreach (var a in element.Attributes())
            {
                bool isNamespaceDeclaration = IsNamespaceDeclaration(a);
                yield return Space();
                yield return AttributeName(MakeName(a.Name, a.Parent));
                yield return Assignment();
                yield return Quote();
                yield return isNamespaceDeclaration ? NamespaceValue(a.Value) : AttributeValue(a.Value);
                yield return Quote();
            }

            bool hasContent = HasContent(element);

            yield return Bracket(hasContent ? ">" : "/>");
        }

        public IEnumerable<Inline> RenderEndElement(XElement element)
        {
            yield return Bracket("</");
            yield return ElementName(MakeName(element.Name, element));
            yield return Bracket(">");
        }

        private IEnumerable<Inline> RenderText(XText text)
        {
            bool first = true;
            foreach (var line in text.Value.Split('\n').Select(l => l.Trim()))
            {
                if (!first)
                    yield return new LineBreak();
                else
                    first = false;

                yield return ElementValue(line.Trim());
            }
        }

        private IEnumerable<Inline> RenderComment(XComment comment)
        {
            yield return Comment("<!--");
            yield return Comment(comment.Value);
            yield return Comment("-->");
        }

        private IEnumerable<Inline> RenderUnsupported(XNode node)
        {
            yield return new Run(node.ToString()) { Style = NotificationStyle };
        }

        #endregion

        #region Inline style helpers

        private Inline Bracket(string text)
        {
            return new Run(text) { Style = BracketStyle };
        }

        private Inline ElementName(string text)
        {
            return new Run(text) { Style = ElementNameStyle };
        }

        private Inline Space()
        {
            return new Run(" ") { Style = SpaceStyle };
        }

        private Inline AttributeName(string text)
        {
            return new Run(text) { Style = AttributeNameStyle };
        }

        private Inline NamespaceValue(string text)
        {
            return new Run(text) { Style = NamespaceValueStyle };
        }

        private Inline Assignment()
        {
            return new Run("=") { Style = AssignmentStyle };
        }

        private Inline Quote()
        {
            return new Run("\"") { Style = QuoteStyle };
        }

        private Inline AttributeValue(string text)
        {
            return new Run(text) { Style = AttributeValueStyle };
        }

        private Inline ElementValue(string text)
        {
            return new Run(text) { Style = ElementValueStyle };
        }

        private Inline Comment(string text)
        {
            return new Run(text) { Style = CommentStyle };
        }

        #endregion

        #region XML helpers

        private bool HasContent(XElement element)
        {
            return element.Nodes().Any();
        }

        #endregion

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is XDocument doc)
                return Render(doc);

            if (value is XmlNode node)
            {
                using (var reader = new XmlNodeReader(node))
                {
                    reader.MoveToContent();
                    value = XElement.Load(reader, LoadOptions.PreserveWhitespace);
                }
            }


            return (value is XElement element && (parameter as string)?.ToLower() != "text")
                ? Render(element)
                : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
