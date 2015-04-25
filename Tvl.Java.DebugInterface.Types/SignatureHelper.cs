namespace Tvl.Java.DebugInterface.Types
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class SignatureHelper
    {
        private const string ArgFormat = @"(?:\[*(?:[ZBCDFIJSV]|L[a-zA-Z0-9_$/]+;))";
        private static readonly Regex SignatureFormat =
            new Regex(
                @"^\((?<ARG>" + ArgFormat + @")*\)(?<RET>" + ArgFormat + @")$",
                RegexOptions.Compiled);

        public static void ParseMethodSignature(string signature, out List<string> argumentTypeSignatures, out string returnTypeSignature)
        {
            argumentTypeSignatures = new List<string>();

            Match match = SignatureFormat.Match(signature);
            foreach (var arg in match.Groups["ARG"].Captures.Cast<Capture>())
                argumentTypeSignatures.Add(arg.Value);

            returnTypeSignature = match.Groups["RET"].Value;
        }

        public static string DecodeTypeName(string signature)
        {
            Contract.Requires<ArgumentNullException>(signature != null, "signature");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(signature));

            switch (signature[0])
            {
            case 'Z':
                return "boolean";
            case 'B':
                return "byte";
            case 'C':
                return "char";
            case 'D':
                return "double";
            case 'F':
                return "float";
            case 'I':
                return "int";
            case 'J':
                return "long";
            case 'S':
                return "short";
            case 'V':
                return "void";
            case '[':
                return DecodeTypeName(signature.Substring(1)) + "[]";
            case 'L':
                return signature.Substring(1, signature.Length - 2).Replace('/', '.');
            default:
                throw new FormatException();
            }
        }
    }
}
