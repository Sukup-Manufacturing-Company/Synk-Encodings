using Synk.Encodings.Crockford; 

namespace Synk.Encodings.Tests;

public class CrockfordUuidTests
{
    [Fact]
    public void Parse_Correctly_Parses_The_Input_String()
    {
        var uuid = CrockfordUuid.Parse("M7KA-0TM3-2ACM-X6NX-BRCY-712R-FM"); 
        Console.WriteLine(uuid.ToString()); 
    }
}
