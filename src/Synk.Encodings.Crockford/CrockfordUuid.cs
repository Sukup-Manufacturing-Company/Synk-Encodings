using System.Text;

namespace Synk.Encodings.Crockford;
public readonly struct CrockfordUuid
{
    private static readonly char[] CrockfordChars = [.. "0123456789ABCDEFGHJKMNPQRSTVWXYZ"]; 
    private readonly char[] _encodedChars = new char[26];
    public readonly int Length => _encodedChars.Length; 
    public static readonly CrockfordUuid Empty; 
    private CrockfordUuid(byte[] bytes)    
    {
        EncodeCharacters(bytes, ref _encodedChars); 
    }
    public override readonly string ToString()
    {
        return GetFormattedString(_encodedChars, separater: '-', segmentLength: 4); 
    }
    public string ToString(char separater, int segmentLength)
    {
        return GetFormattedString(_encodedChars, separater, segmentLength); 
    }
    public static CrockfordUuid FromGuid(Guid guid) => new(guid.ToByteArray()); 
    private static void EncodeCharacters(byte[] source, ref char[] dest)
    {
        ArgumentNullException.ThrowIfNull(source); 

        // write the contents of the input array into the 
        // array of Crockford's Base32 encoded characters
        WriteEncodedCharacters(source, dest); 

        // remove any leading zeros 
        int leadingZeroCount = 0; 
        foreach (var num in dest)
        {
            if (num is not '0')
            {
                break; 
            }

            leadingZeroCount++; 
        }
        dest = dest.AsSpan()[leadingZeroCount..].ToArray(); 
    }
    // Create formatted string -> FXAG-K3WP-QRV4-H2TV-DPTB-SDJJ-40
    private static string GetFormattedString(ReadOnlySpan<char> input, char separater, int segmentLength)
    {
        StringBuilder sb = new(); 
        for (int i = 0; i < input.Length;)
        {
            int sliceLength = i + segmentLength > input.Length ? input.Length - i : segmentLength; 
            sb.Append(input.Slice(i, sliceLength)); 
            i += sliceLength; 

            if (i < input.Length)
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

        byte[]? output = new byte[input.Length * 5 / 8];
        int bitIndex = 0;
        int inputIndex = 0;
        int outputBits = 0;
        int outputIndex = 0;
        while (outputIndex < output.Length)
        {
            int byteIndex = Array.IndexOf(CrockfordChars, input[inputIndex]);
            if (byteIndex < 0)
            {
                throw new FormatException();
            }

            int bits = Math.Min(5 - bitIndex, 8 - outputBits);
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

        return new CrockfordUuid(output); 
    }
    private static int WriteEncodedCharacters(byte[] source, char[] dest)
    {
        int destIndex = 0; 
        for (int srcIndex = 0; srcIndex < source.Length;)
        {
            int bytesToWrite = (source.Length - srcIndex) switch 
            {
                1 => 2, 
                2 => 4, 
                3 => 5,
                4 => 7,
                _ => 8 
            }; 

            uint b1, b2, b3, b4, b5;
            b1 = (srcIndex < source.Length) ? source[srcIndex++] : 0U;
            b2 = (srcIndex < source.Length) ? source[srcIndex++] : 0U;
            b3 = (srcIndex < source.Length) ? source[srcIndex++] : 0U;
            b4 = (srcIndex < source.Length) ? source[srcIndex++] : 0U;
            b5 = (srcIndex < source.Length) ? source[srcIndex++] : 0U;

            byte a, b, c, d, e, f, g, h; 
            a = (byte)(b1 >> 3); 
            b = (byte)(((b1 & 0x07) << 2) | (b2 >> 6)); 
            c = (byte)((b2 >> 1) & 0x1f); 
            d = (byte)(((b2 & 0x01) << 4) | (b3 >> 4)); 
            e = (byte)(((b3 & 0x0f) << 1) | (b4 >> 7)); 
            f = (byte)((b4 >> 2) & 0x1f);
            g = (byte)(((b4 & 0x3) << 3) | (b5 >> 5));
            h = (byte)(b5 & 0x1f);

            if (bytesToWrite-- is > 0) dest[destIndex++] = CrockfordChars[a]; 
            if (bytesToWrite-- is > 0) dest[destIndex++] = CrockfordChars[b]; 
            if (bytesToWrite-- is > 0) dest[destIndex++] = CrockfordChars[c]; 
            if (bytesToWrite-- is > 0) dest[destIndex++] = CrockfordChars[d]; 
            if (bytesToWrite-- is > 0) dest[destIndex++] = CrockfordChars[e]; 
            if (bytesToWrite-- is > 0) dest[destIndex++] = CrockfordChars[f]; 
            if (bytesToWrite-- is > 0) dest[destIndex++] = CrockfordChars[g]; 
            if (bytesToWrite-- is > 0) dest[destIndex++] = CrockfordChars[h]; 
        }

        int bytesWritten = destIndex + 1; 
        return bytesWritten;
    }
}