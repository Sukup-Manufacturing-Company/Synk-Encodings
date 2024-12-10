using System.Text;
using Synk.Encodings.Crockford.Helpers; 

namespace Synk.Encodings.Crockford;
public struct CrockfordUuid
{
    private const string Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
    // private static readonly string _checksumChars = "*~$=U"; 
    private const string CrockfordChars = "0123456789ABCDEFGHJKMNPQRSTVWXYZ"; 
    private readonly string _str; 
    public readonly int Length => _str.Length; 
    public static readonly CrockfordUuid Empty; 
    private CrockfordUuid(Guid guid)    
    {
        _str = EncodeSegmented(guid.ToByteArray(), '-', 4); 
    }
    public override readonly string ToString()
    {
        return _str; 
    }
    public static CrockfordUuid FromGuid(Guid guid)
    {
        return new CrockfordUuid(guid); 
    }
    private static ReadOnlySpan<char> Encode(byte[] input)
    {
        ArgumentNullException.ThrowIfNull(input); 

        StringBuilder sb = new();
        for (int offset = 0; offset < input.Length;)
        {
            byte a, b, c, d, e, f, g, h;
            int numCharsToOutput = GetNextGroup(input, ref offset, out a, out b, out c, out d, out e, out f, out g, out h);

            sb.AppendWhen(numCharsToOutput >= 1, CrockfordChars[a]); 
            sb.AppendWhen(numCharsToOutput >= 2, CrockfordChars[b]);
            sb.AppendWhen(numCharsToOutput >= 3, CrockfordChars[c]);
            sb.AppendWhen(numCharsToOutput >= 4, CrockfordChars[d]);
            sb.AppendWhen(numCharsToOutput >= 5, CrockfordChars[e]); 
            sb.AppendWhen(numCharsToOutput >= 6, CrockfordChars[f]);
            sb.AppendWhen(numCharsToOutput >= 7, CrockfordChars[g]);  
            sb.AppendWhen(numCharsToOutput >= 8, CrockfordChars[h]);
        }

        return sb.ToString().TrimStart('0'); 
    }
    // Create formatted string -> FXAG-K3WP-QRV4-H2TV-DPTB-SDJJ-40
    private static string EncodeSegmented(byte[] input, char separater, int segmentLength)
    {
        ArgumentNullException.ThrowIfNull(input); 

        ReadOnlySpan<char> encoded = Encode(input); 
        StringBuilder sb = new(); 

        for (int i = 0; i < encoded.Length;)
        {
            int sliceLength = i + segmentLength > encoded.Length ? encoded.Length - i : segmentLength; 
            sb.Append(encoded.Slice(i, sliceLength)); 
            i += sliceLength; 

            if (i < encoded.Length)
            {
                sb.Append(separater); 
            }
        }

        return sb.ToString(); 
    }
    
    public static CrockfordUuid Parse(string input)
    {
        ArgumentNullException.ThrowIfNull(input); 
        
        input = input.Replace("-", string.Empty).ToUpperInvariant(); 
        if (input.Length is 0)
        {
            return Empty; 
        }

        var output = new byte[input.Length * 5 / 8];
        var bitIndex = 0;
        var inputIndex = 0;
        var outputBits = 0;
        var outputIndex = 0;
        while (outputIndex < output.Length)
        {
            var byteIndex = Base32Chars.IndexOf(input[inputIndex]);
            if (byteIndex < 0)
            {
                throw new FormatException();
            }

            var bits = Math.Min(5 - bitIndex, 8 - outputBits);
            output[outputIndex] <<= bits;
            output[outputIndex] |= (byte)(byteIndex >> (5 - (bitIndex + bits)));

            bitIndex += bits;
            if (bitIndex >= 5)
            {
                inputIndex++;
                bitIndex = 0;
            }

            outputBits += bits;
            if (outputBits >= 8)
            {
                outputIndex++;
                outputBits = 0;
            }
        }

        throw new NotImplementedException(); 
    }
    public static byte[] Decode(string input)
    {
        ArgumentNullException.ThrowIfNull(input); 

        input = input.TrimEnd('=').ToUpperInvariant();
        if (input.Length == 0)
        {
            return new byte[0];
        }

        var output = new byte[input.Length * 5 / 8];
        var bitIndex = 0;
        var inputIndex = 0;
        var outputBits = 0;
        var outputIndex = 0;
        while (outputIndex < output.Length)
        {
            var byteIndex = Base32Chars.IndexOf(input[inputIndex]);
            if (byteIndex < 0)
            {
                throw new FormatException();
            }

            var bits = Math.Min(5 - bitIndex, 8 - outputBits);
            output[outputIndex] <<= bits;
            output[outputIndex] |= (byte)(byteIndex >> (5 - (bitIndex + bits)));

            bitIndex += bits;
            if (bitIndex >= 5)
            {
                inputIndex++;
                bitIndex = 0;
            }

            outputBits += bits;
            if (outputBits >= 8)
            {
                outputIndex++;
                outputBits = 0;
            }
        }
        return output;
    }

    // returns the number of bytes that were output
    private static int GetNextGroup(byte[] input, ref int offset, out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h)
    {
        uint b1, b2, b3, b4, b5;
        int retVal = (input.Length - offset) switch 
        {
            1 => 2, 
            2 => 4, 
            3 => 5,
            4 => 7,
            _ => 8 
        }; 

        b1 = (offset < input.Length) ? input[offset++] : 0U;
        b2 = (offset < input.Length) ? input[offset++] : 0U;
        b3 = (offset < input.Length) ? input[offset++] : 0U;
        b4 = (offset < input.Length) ? input[offset++] : 0U;
        b5 = (offset < input.Length) ? input[offset++] : 0U;

        a = (byte)(b1 >> 3);
        b = (byte)(((b1 & 0x07) << 2) | (b2 >> 6));
        c = (byte)((b2 >> 1) & 0x1f);
        d = (byte)(((b2 & 0x01) << 4) | (b3 >> 4));
        e = (byte)(((b3 & 0x0f) << 1) | (b4 >> 7));
        f = (byte)((b4 >> 2) & 0x1f);
        g = (byte)(((b4 & 0x3) << 3) | (b5 >> 5));
        h = (byte)(b5 & 0x1f);

        return retVal;
    }
}