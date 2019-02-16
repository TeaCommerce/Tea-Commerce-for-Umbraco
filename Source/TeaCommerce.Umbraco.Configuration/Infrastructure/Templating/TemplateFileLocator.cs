using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using TeaCommerce.Api.Infrastructure.Templating;
using Umbraco.Core.IO;

namespace TeaCommerce.Umbraco.Configuration.Infrastructure.Templating {
  public class TemplateFileLocator : ITemplateFileLocator {

    public IEnumerable<string> GetTemplateFiles() {
      List<string> templateFiles = new List<string>();

      //Razor files
      string basePath = HostingEnvironment.MapPath( "~" );
      templateFiles.AddRange( GetTemplateFilesFromDir( basePath, HostingEnvironment.MapPath( SystemDirectories.MacroScripts ), "*.cshtml" ) );
      templateFiles.AddRange( GetTemplateFilesFromDir( basePath, HostingEnvironment.MapPath( SystemDirectories.MacroScripts ), "*.vbhtml" ) );
      //Add partial views
      templateFiles.AddRange( GetTemplateFilesFromDir( basePath, HostingEnvironment.MapPath( SystemDirectories.MvcViews ) + "\\Partials", "*.cshtml"));

      return templateFiles;
    }

    private IEnumerable<string> GetTemplateFilesFromDir( string orgPath, string path, string searchPattern ) {
      List<string> templateFiles = new List<string>();

      DirectoryInfo dirInfo = new DirectoryInfo( path );
      if ( dirInfo.Exists ) {
        foreach ( FileInfo file in dirInfo.GetFiles( searchPattern ) )
          templateFiles.Add( ( path.Replace( orgPath, string.Empty ).Trim( Path.DirectorySeparatorChar ) + Path.DirectorySeparatorChar + file.Name ).Trim( Path.DirectorySeparatorChar ) );

        //Populate subdirectories
        foreach ( DirectoryInfo dir in dirInfo.GetDirectories() )
          templateFiles.AddRange( GetTemplateFilesFromDir( orgPath, path + Path.DirectorySeparatorChar + dir.Name, searchPattern ) );
      }

      return templateFiles;
    }

    public string TranslateTemplateFileLocation( string templateFile ) {
      if ( !string.IsNullOrEmpty( templateFile ) ) {
        if ( templateFile.StartsWith( new string( Path.DirectorySeparatorChar, 1 ) ) || templateFile.StartsWith( "/" ) ) {
          templateFile = "~" + templateFile;
        } else if ( !templateFile.StartsWith( "~" ) ) {
          templateFile = "~" + Path.DirectorySeparatorChar + templateFile;
        }
        templateFile = templateFile.Replace( "/", new string( Path.DirectorySeparatorChar, 1 ) );

        if ( templateFile.EndsWith( ".xslt" ) && !templateFile.StartsWith( SystemDirectories.Xslt.Replace( "/", new string( Path.DirectorySeparatorChar, 1 ) ) ) ) {
          templateFile = templateFile.Replace( "~", SystemDirectories.Xslt );
        }
        else if (templateFile.StartsWith("~\\Partials"))
        {
           templateFile = templateFile.Replace("~\\", SystemDirectories.MvcViews + "/");
                }
        else if ( ( templateFile.EndsWith( ".cshtml" ) || templateFile.EndsWith( ".vbhtml" ) ) &&
                    !templateFile.StartsWith( SystemDirectories.MvcViews.Replace("/", new string(Path.DirectorySeparatorChar, 1))) &&
                    !templateFile.StartsWith( SystemDirectories.MacroScripts.Replace( "/", new string( Path.DirectorySeparatorChar, 1 ) ) ) ) {
          templateFile = templateFile.Replace( "~", SystemDirectories.MacroScripts );
        }
        templateFile = templateFile.Replace( "/", new string( Path.DirectorySeparatorChar, 1 ) );
      }

      return templateFile;
    }

  }
}
