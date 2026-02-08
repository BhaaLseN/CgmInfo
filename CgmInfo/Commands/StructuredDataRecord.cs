using System.Collections.Generic;

namespace CgmInfo.Commands
{
    // structured data records are self-defining structures [ISO/IEC 8632-3 7, Table 1, Note 17]
    // each record contains a single member and is comprised of [ISO/IEC 8632-3 8.3, 21 FONT PROPERTIES, P3]
    //      data type indicator
    //      data element count
    //      data element(s)
    // see also [ISO/IEC 8632-1 Annex C, C.2.2]
    public class StructuredDataRecord
    {
        public StructuredDataRecord(IEnumerable<StructuredDataElement> elements)
        {
            Elements = elements;
        }

        public IEnumerable<StructuredDataElement> Elements { get; }
    }
}
