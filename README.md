# Synk-Encodings

## Synk.Encodings.Crockford 
- [Synk.Encodings.Crockford](./src/Synk.Encodings.Crockford)

### Installing

```powershell
dotnet add package Synk.Encodings.Crockford
```

### Usage 

```c#
using Synk.Encodings.Crockford;

var uuid1 = CrockfordUuid.Parse("M7KA-0TM3-2ACM-X6NX-BRCY-712R-FM");
var uuid2 = CrockfordUuid.FromGuid(Guid.NewGuid()); 
```
