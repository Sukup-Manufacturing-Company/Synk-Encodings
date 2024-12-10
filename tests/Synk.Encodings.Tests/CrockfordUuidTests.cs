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

        _output.WriteLine($"----------- Test Method: {nameof(Parse_Correctly_Parses_The_Input_String)} ------"); 
        _output.WriteLine(actual); 
        _output.WriteLine(uuid.ToString('-', 6)); 

        Assert.Equal(expected, actual); 
    }
    
    [Fact]
    public void FromGuid_Is_Successfull()
    {
        var guid = Guid.NewGuid(); 
        var uuid = CrockfordUuid.FromGuid(guid); 

        _output.WriteLine($"----------- Test Method: {nameof(FromGuid_Is_Successfull)} ------"); 
        _output.WriteLine(guid.ToString()); 
        _output.WriteLine(uuid.ToString()); 
    }

    [Fact]
    public void FromGuid_Throws_ArgumentExeception()
    {
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000000");
        void action() => CrockfordUuid.FromGuid(guid);

        Assert.Throws<ArgumentException>(action); 
    }

    [Fact]
    public void ToGuid_Is_Successfull()
    {
        var expected = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"); 
        var uuid = CrockfordUuid.FromGuid(expected); 
        var actual = uuid.ToGuid(); 

        _output.WriteLine($"----------- Test Method: {nameof(ToGuid_Is_Successfull)} ------"); 
        _output.WriteLine(expected.ToString()); 
        _output.WriteLine(actual.ToString()); 

        Assert.Equal(expected, actual); 
    }

}
