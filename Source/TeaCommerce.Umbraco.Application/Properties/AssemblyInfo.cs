using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web.UI;
using TeaCommerce.Umbraco.Application;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle( "TeaCommerce.Umbraco.Application" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "Tea Commerce" )]
[assembly: AssemblyProduct( "Tea Commerce for Umbraco" )]
[assembly: AssemblyCopyright( "Copyright © 2010 Tea Commerce" )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible( false )]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid( "ec5ec8dc-3733-4991-b246-45d8e1ebd1f0" )]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion( "3.0.0.0" )]
[assembly: AssemblyFileVersion( "3.2.1.0" )]

//Embedded Resources

[assembly: WebResource( Constants.EditorIcons.Edit, Constants.MimeTypes.GIF )]
[assembly: WebResource( Constants.EditorIcons.Delete, Constants.MimeTypes.GIF )]
[assembly: WebResource(Constants.EditorIcons.Calendar, Constants.MimeTypes.GIF)]

[assembly: WebResource( Constants.MiscIcons.Exclamation, Constants.MimeTypes.PNG )]

[assembly: WebResource( Constants.Scripts.Default, Constants.MimeTypes.JavaScript )]
