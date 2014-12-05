using System.IO;
using System.Reflection;
using System.Web;
using System.Xml;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;
using System.Web.Hosting;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class TeaMoveFile : IPackageAction {

    private string targetFilePath;
    private string sourceFilePath;
    private bool overwriteFile = true;
    private bool embeddedResource;

    public void Initialize( XmlNode xmlData ) {
      embeddedResource = xmlData.Attributes[ "embeddedResource" ] != null && bool.Parse( xmlData.Attributes[ "embeddedResource" ].Value );

      targetFilePath = HostingEnvironment.MapPath( xmlData.Attributes[ "targetFile" ].Value );
      sourceFilePath = !embeddedResource ? HostingEnvironment.MapPath( xmlData.Attributes[ "sourceFile" ].Value ) : xmlData.Attributes[ "sourceFile" ].Value;
      overwriteFile = xmlData.Attributes[ "overwriteFile" ] == null || !( xmlData.Attributes[ "overwriteFile" ].Value.Equals( "false" ) || xmlData.Attributes[ "overwriteFile" ].Value.Equals( "0" ) );

    }

    #region IPackageAction Members

    public string Alias() {
      return "TeaMoveFile";
    }

    public bool Execute( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );

      if ( overwriteFile && File.Exists( targetFilePath ) )
        File.Delete( targetFilePath );

      if ( overwriteFile || !File.Exists( targetFilePath ) ) {
        if ( !embeddedResource ) {
          File.Move( sourceFilePath, targetFilePath );
          File.Delete( sourceFilePath );
        } else {
          Directory.CreateDirectory( Path.GetDirectoryName( targetFilePath ) );
          using ( Stream input = Assembly.GetExecutingAssembly().GetManifestResourceStream( sourceFilePath ) ) {
            using ( FileStream output = new FileStream( targetFilePath, FileMode.OpenOrCreate ) ) {
              input.CopyTo( output );
            }
          }
        }
      }

      return true;
    }

    public XmlNode SampleXml() {
      return helper.parseStringToXmlNode( string.Format( @"<Action runat=""install"" alias=""{0}"" sourceFile=""~/test.xml"" targetFile=""~/test2.xml"" overwriteFile=""false"" embeddedResource=""false""  />", this.Alias() ) );
    }

    public bool Undo( string packageName, XmlNode xmlData ) {
      Initialize( xmlData );
      File.Delete( targetFilePath );

      return true;
    }

    #endregion
  }
}