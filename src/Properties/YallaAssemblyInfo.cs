using System.Resources;
using System.Reflection;

[assembly: AssemblyCompany("Dmitry Shechtman")]
[assembly: AssemblyProduct("YALLA.NET")]
[assembly: AssemblyCopyright("Copyright © Dmitry Shechtman 2015")]
[assembly: NeutralResourcesLanguage("en")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
