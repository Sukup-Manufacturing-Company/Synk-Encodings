# Synk-Encodings

## Synk.Encodings.Crockford 
- [Synk.Encodings.Crockford](./src/Synk.Encodings.Crockford)

### Installing

First you need to add our Nuget github package registry as a source to your local Nuget config. 
```bash
$ dotnet nuget add source https://nuget.pkg.github.com/Sukup-Manufacturing-Company/index.json \
-n github \
-u <your github username> \
-p <your github personal access token> \
--store-password-in-clear-text
```

You can verify that the source has been added by: 
```bash
$ dotnet nuget list source
Registered Sources:
  1.  nuget.org [Enabled]
      https://api.nuget.org/v3/index.json
  2.  github [Enabled]
      https://nuget.pkg.github.com/Sukup-Manufacturing-Company/index.json
  3.  Microsoft Visual Studio Offline Packages [Enabled]
      C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\
```

Now you can install the package, like any other .NET package hosted on nuget.org.
```bash
dotnet add package Synk.Encodings.Crockford --version 0.1.0
```

### Usage 

```c#
using Synk.Encodings.Crockford;

var uuid1 = CrockfordUuid.Parse("M7KA-0TM3-2ACM-X6NX-BRCY-712R-FM");
var uuid2 = CrockfordUuid.FromGuid(Guid.NewGuid()); 
```
