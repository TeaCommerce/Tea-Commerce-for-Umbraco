using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using TeaCommerce.Api.Common;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using System.Web.Hosting;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public abstract class AMergeXmlFile : IPackageAction {

    private bool fileMoved;

    private string targetFilePath;
    private string TargetFilePath {
      get { return targetFilePath; }
      set {
        targetFilePath = value;
        targetFile = null;
        if ( !File.Exists( targetFilePath ) ) {
          if ( createIfTargetFileNotExists ) {
            if ( !embeddedResource ) {
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

    protected bool deleteTargetFileOnUndo;
    protected bool createIfTargetFileNotExists = true;
    protected bool overwriteValues;
    protected bool embeddedResource;

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
          if ( embeddedResource )
            sourceFile = XDocument.Load( Assembly.GetExecutingAssembly().GetManifestResourceStream( SourceFilePath ) );
          else
            sourceFile = XDocument.Load( HostingEnvironment.MapPath( SourceFilePath ) );
        }
        return sourceFile;
      }
    }

    public void Initialize( XmlNode xmlData ) {
      embeddedResource = xmlData.Attributes[ "embeddedResource" ] != null && ( xmlData.Attributes[ "embeddedResource" ].Value.TryParse<bool>() ?? false );
      deleteTargetFileOnUndo = xmlData.Attributes[ "deleteTargetFileOnUndo" ] != null && ( xmlData.Attributes[ "deleteTargetFileOnUndo" ].Value.TryParse<bool>() ?? false );
      overwriteValues = xmlData.Attributes[ "overwriteValues" ] != null && ( xmlData.Attributes[ "overwriteValues" ].Value.TryParse<bool>() ?? false );
      if ( xmlData.Attributes[ "createIfTargetFileNotExists" ] != null ) {
        createIfTargetFileNotExists = ( xmlData.Attributes[ "createIfTargetFileNotExists" ].Value.TryParse<bool>() ?? true );
      }
      SourceFilePath = xmlData.Attributes[ "sourceFile" ].Value;
      TargetFilePath = HostingEnvironment.MapPath( xmlData.Attributes[ "targetFile" ].Value );
    }

    #region IPackageAction Members

    public abstract string Alias();

    public bool Execute( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );

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

          if ( !embeddedResource )
            File.Delete( HostingEnvironment.MapPath( SourceFilePath ) );
        }
      }

      return true;
    }

    private void MergeElement( XElement sourceElement, XElement targetElement ) {
      foreach ( XAttribute sourceAttribute in sourceElement.Attributes() ) {
        XAttribute targetAttribute = targetElement.Attributes().FirstOrDefault( i => i.Name.Equals( sourceAttribute.Name ) );
        if ( targetAttribute == null || overwriteValues )
          targetElement.SetAttributeValue( sourceAttribute.Name, sourceAttribute.Value );
      }

      foreach ( XElement tempSourceElement in sourceElement.Elements() ) {
        XElement tempTargetElement = CheckElementExists( tempSourceElement, targetElement );
        if ( !tempTargetElement.HasElements && overwriteValues && !string.IsNullOrEmpty( tempTargetElement.Value ) )
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

    public XmlNode SampleXml() {
      return helper.parseStringToXmlNode( string.Format( @"<Action runat=""install"" alias=""{0}"" sourceFile=""~/test2.xml"" targetFile=""~/test.xml"" overwriteValues=""false"" deleteTargetFileOnUndo=""false"" />", this.Alias() ) );
    }

    public bool Undo( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );

      if ( TargetFile != null ) {
        if ( !deleteTargetFileOnUndo ) {
          CleanFile();
          TargetFile.Save( TargetFilePath, SaveOptions.None );
        } else
          File.Delete( TargetFilePath );
      }

      return true;
    }

    #endregion

    protected virtual XElement FindElement( XElement parentElement, XElement sourceElement ) {
      return parentElement.Elements().FirstOrDefault( i => i.Name.Equals( sourceElement.Name ) && ( i.Attribute( "alias" ) != null ? i.Attribute( "alias" ).Value.Equals( sourceElement.Attribute( "alias" ).Value ) : true ) );
    }

    protected virtual void CleanFile() { }

  }
}