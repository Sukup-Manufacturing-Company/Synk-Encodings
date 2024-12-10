using Synk.Encodings.Crockford; 

namespace Synk.Encodings.Tests;

public class CrockfordUuidTests
{
    [Fact]
    public void Parse_Correctly_Parses_The_Input_String()
    {
        string expected = "M7KA-0TM3-2ACM-X6NX-BRCY-712R-FM"; 
        var uuid = CrockfordUuid.Parse(expected); 
        string actual = uuid.ToString(); 

        Console.WriteLine(actual);
        Console.WriteLine(uuid.ToString('-', 6)); 
        Assert.Equal(expected, actual); 
    }
}
