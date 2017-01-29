using System.Reflection;
using System.Runtime.CompilerServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Easy.Web.Core")]
[assembly: AssemblyProduct("Easy.Web")]
[assembly: AssemblyDescription("A fast, lean and opinionated web framework based on the Microsoft ASP.NET Core.")]
[assembly: AssemblyCopyright("Copyright ©  2017")]
[assembly: AssemblyTrademark("Nima Ara")]

[assembly: InternalsVisibleTo("Easy.Web.Serialization.XML")]
[assembly: InternalsVisibleTo("Easy.Web.Serialization.JSONNet")]
[assembly: InternalsVisibleTo("Easy.Web.Serialization.NetJSON")]
[assembly: InternalsVisibleTo("Easy.Web.Serialization.Jil")]
[assembly: InternalsVisibleTo("Easy.Web.Serialization.ProtobufNet")]
[assembly: InternalsVisibleTo("Easy.Web.Tests.Unit")]
[assembly: InternalsVisibleTo("Easy.Web.Tests.Integration")]