using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CgmInfo.Parameters;

namespace CgmInfoCmd
{
    public class PrintMetafileDescriptorParameterVisitor : IMetafileDescriptorParameterVisitor<int>
    {
        #region IMetafileDescriptorParameterVisisor<object> Members

        public void VisitInteger(IntegerMetafileDescriptorParameter integerParameter, int parameter)
        {
            Indent indent = parameter;
            if (integerParameter.Type == MetafileDescriptorType.VdcType)
            {
                if (integerParameter.ValueType == 0)
                    Console.WriteLine(indent + "{0}: Integer", integerParameter.Type);
                else if (integerParameter.ValueType == 1)
                    Console.WriteLine(indent + "{0}: Real", integerParameter.Type);
                else
                    Console.WriteLine(indent + "{0}: Unknown ({1})", integerParameter.Type, integerParameter.ValueType);
            }
            else
            {
                Console.WriteLine(indent + "{0}: {1}", integerParameter.Type, integerParameter.ValueType);
            }
        }

        public void VisitString(StringMetafileDescriptorParameter stringParameter, int parameter)
        {
            Indent indent = parameter;
            Console.WriteLine(indent + "{0}: {1}", stringParameter.Type, stringParameter.ValueType);
        }

        public void VisitRealPrecision(RealPrecisionMetafileDescriptorParameter realPrecisionParameter, int parameter)
        {
            Indent indent = parameter;
            if ((realPrecisionParameter.RealFormat == 0 && realPrecisionParameter.RealExponent == 9 && realPrecisionParameter.RealFraction == 23)
             || (realPrecisionParameter.RealFormat == 0 && realPrecisionParameter.RealExponent == 12 && realPrecisionParameter.RealFraction == 52)
             || (realPrecisionParameter.RealFormat == 1 && realPrecisionParameter.RealExponent == 16 && realPrecisionParameter.RealFraction == 16)
             || (realPrecisionParameter.RealFormat == 1 && realPrecisionParameter.RealExponent == 32 && realPrecisionParameter.RealFraction == 32))
                Console.WriteLine(indent + "{0}: {1}-bit {2} point",
                    realPrecisionParameter.Type,
                    realPrecisionParameter.RealExponent + realPrecisionParameter.RealFraction,
                    realPrecisionParameter.RealFormat == 1 ? "fixed" : "floating");
            else
                Console.WriteLine(indent + "{0}: illegal precision combination: {1}-bit ({2}+{3}) {4} point",
                    realPrecisionParameter.Type,
                    realPrecisionParameter.RealExponent + realPrecisionParameter.RealFraction,
                    realPrecisionParameter.RealExponent, realPrecisionParameter.RealFraction,
                    realPrecisionParameter.RealFormat == 1 ? "fixed" :
                    realPrecisionParameter.RealFormat == 0 ? "floating" : realPrecisionParameter.RealFormat.ToString());
        }

        public void VisitUnsupported(UnsupportedMetafileDescriptorParameter unsupportedParameter, int parameter)
        {
            Indent indent = parameter;
            Console.WriteLine(indent + "Unsupported type {0}", unsupportedParameter.Type);
        }

        #endregion

        private sealed class Indent
        {
            private readonly int _depth;
            private Indent(int depth)
            {
                _depth = depth;
            }
            public static implicit operator Indent(int depth)
            {
                return new Indent(depth);
            }
            public static implicit operator string(Indent indent)
            {
                if (indent == null || indent._depth <= 0)
                    return "";
                return new string('\t', indent._depth);
            }
        }
    }
}
