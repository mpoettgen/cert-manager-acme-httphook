using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyProduct("cert-manager-acme-httphook")]
[assembly: AssemblyCompany("Michael Pöttgen")]
[assembly: AssemblyCopyright("Copyright 2021 Michael Pöttgen")]
[assembly: AssemblyTrademark("")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
