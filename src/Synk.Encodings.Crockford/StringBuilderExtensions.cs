using System.Text;

namespace Synk.Encodings.Crockford.Helpers;
internal static class StringBuilderExtensions 
{
    public static StringBuilder AppendWhen(this StringBuilder sb, bool condition, char value) => 
        condition ? sb.Append(value) : sb; 
   
}