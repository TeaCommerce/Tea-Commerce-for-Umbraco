using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using TeaCommerce.Api.Common;
using System.Web.Hosting;

namespace TeaCommerce.Umbraco.Install.InstallTasks {
  public abstract class AMergeXmlFileInstallTask : IInstallTask {

    private bool fileMoved;

    private string targetFilePath;
    private string TargetFilePath {
      get { return targetFilePath; }
      set {
        targetFilePath = value;
        targetFile = null;
        if ( !File.Exists( targetFilePath ) ) {
          if ( CreateIfTargetFileNotExists ) {
            if ( !EmbeddedResource ) {
              File.Move( HostingEnvironment.MapPath( SourceFilePath ), targetFilePath );
            } else {
              using ( Stream input = Assembly.GetExecutingAssembly().GetManifestResourceStream( sourceFilePath ) ) {
                Directory.CreateDirectory( Path.GetDirectoryName( targetFilePath ) );
                using ( FileStream output = new FileStream( targetFilePath, FileMode.OpenOrCreate ) ) {
                  input.CopyTo( output );
                }
              }
            }
            fileMoved = true;
          } else
            targetFilePath = null;
        }
      }
    }

    private string sourceFilePath;
    private string SourceFilePath {
      get {
        return sourceFilePath;
      }
      set {
        sourceFilePath = value;
        sourceFile = null;
      }
    }

    public bool DeleteTargetFileOnUndo { get; set; }
    public bool CreateIfTargetFileNotExists { get; set; }
    public bool OverwriteValues { get; set; }
    public bool EmbeddedResource { get; set; }

    private XDocument targetFile;
    protected XDocument TargetFile {
      get {
        if ( targetFile == null && !string.IsNullOrEmpty( targetFilePath ) )
          targetFile = XDocument.Load( targetFilePath );
        return targetFile;
      }
    }
    private XDocument sourceFile;
    protected XDocument SourceFile {
      get {
        if ( sourceFile == null ) {
          if ( EmbeddedResource )
            sourceFile = XDocument.Load( Assembly.GetExecutingAssembly().GetManifestResourceStream( SourceFilePath ) );
          else
            sourceFile = XDocument.Load( HostingEnvironment.MapPath( SourceFilePath ) );
        }
        return sourceFile;
      }
    }

    public AMergeXmlFileInstallTask( ) {
      CreateIfTargetFileNotExists = true;
    }

    protected void SetFiles( string sourceFile, string targetFile ) {
      SourceFilePath = sourceFile;
      TargetFilePath = HostingEnvironment.MapPath( targetFile );
    }

    private void MergeElement( XElement sourceElement, XElement targetElement ) {
      foreach ( XAttribute sourceAttribute in sourceElement.Attributes() ) {
        XAttribute targetAttribute = targetElement.Attributes().FirstOrDefault( i => i.Name.Equals( sourceAttribute.Name ) );
        if ( targetAttribute == null || OverwriteValues )
          targetElement.SetAttributeValue( sourceAttribute.Name, sourceAttribute.Value );
      }

      foreach ( XElement tempSourceElement in sourceElement.Elements() ) {
        XElement tempTargetElement = CheckElementExists( tempSourceElement, targetElement );
        if ( !tempTargetElement.HasElements && OverwriteValues && !string.IsNullOrEmpty( tempTargetElement.Value ) )
          tempTargetElement.Value = tempSourceElement.Value;

        MergeElement( tempSourceElement, tempTargetElement );
      }
    }

    private XElement CheckElementExists( XElement sourceElement, XElement parentElement ) {
      XElement tempTargetElement = FindElement( parentElement, sourceElement );
      if ( tempTargetElement == null ) {
        tempTargetElement = new XElement( sourceElement.Name );
        if ( !sourceElement.HasElements )
          tempTargetElement.Value = sourceElement.Value;
        parentElement.Add( tempTargetElement );
      }
      return tempTargetElement;
    }

    protected virtual XElement FindElement( XElement parentElement, XElement sourceElement ) {
      return parentElement.Elements().FirstOrDefault( i => i.Name.Equals( sourceElement.Name ) && ( i.Attribute( "alias" ) != null ? i.Attribute( "alias" ).Value.Equals( sourceElement.Attribute( "alias" ).Value ) : true ) );
    }

    protected virtual void CleanFile() { }

    public void Install() {
      if ( TargetFile != null ) {
        XElement root = TargetFile.Root;
        if ( root == null ) {
          root = new XElement( SourceFile.Root.Name );
          TargetFile.Add( root );
        }
        if ( !fileMoved ) {
          MergeElement( SourceFile.Root, root );

          //Save file
          TargetFile.Save( TargetFilePath, SaveOptions.None );

          if ( !EmbeddedResource )
            File.Delete( HostingEnvironment.MapPath( SourceFilePath ) );
        }
      }
    }

    public void Uninstall() {
      if ( TargetFile != null ) {
        if ( !DeleteTargetFileOnUndo ) {
          CleanFile();
          TargetFile.Save( TargetFilePath, SaveOptions.None );
        } else
          File.Delete( TargetFilePath );
      }
    }
  }
}