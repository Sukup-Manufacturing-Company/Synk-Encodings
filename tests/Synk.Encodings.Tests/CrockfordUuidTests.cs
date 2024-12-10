using Synk.Encodings.Crockford;
using Xunit.Abstractions;

namespace Synk.Encodings.Tests;

public class CrockfordUuidTests
{
    private readonly ITestOutputHelper _output; 
    public CrockfordUuidTests(ITestOutputHelper output)
    {
        _output = output; 
    }

    [Fact]
    public void Parse_Correctly_Parses_The_Input_String()
    {
        string expected = "M7KA-0TM3-2ACM-X6NX-BRCY-712R-FM"; 
        var uuid = CrockfordUuid.Parse(expected); 
        string actual = uuid.ToString(); 

        _output.WriteLine(actual); 
        _output.WriteLine(uuid.ToString('-', 6)); 

        Assert.Equal(expected, actual); 
    }
}
