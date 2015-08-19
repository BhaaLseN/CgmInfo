using System.Xml;
using System.Xml.Linq;

namespace CgmInfoGui.Properties
{
    internal sealed partial class Settings
    {
        public XDocument DockLayout
        {
            get
            {
                if (string.IsNullOrEmpty(DockLayoutString))
                    return null;
                try
                {
                    return XDocument.Parse(DockLayoutString);
                }
                catch (XmlException)
                {
                    return null;
                }
            }
            set
            {
                if (value != null && value.Root != null)
                    DockLayoutString = value.ToString(SaveOptions.DisableFormatting);
                else
                    DockLayoutString = null;
            }
        }
    }
}
