using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("YALLA.NET")]
[assembly: AssemblyDescription("Yet Another Logging Library Abstraction for .NET")]
[assembly: AssemblyVersion("1.0.*")]
[assembly: CLSCompliant(true)]

#if !(PORTABLE && NET40)
[assembly: ComVisible(true)]
#endif
