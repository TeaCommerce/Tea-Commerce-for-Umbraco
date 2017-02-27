using System.Xml.Linq;
using System.Xml.XPath;

namespace TeaCommerce.Umbraco.Install.InstallTasks {
  public class UIFileInstallTask : AMergeXmlFileInstallTask {

    public UIFileInstallTask( string sourceFile, string targetFile ) : base() {
      OverwriteValues = true;
      EmbeddedResource = true;
      SetFiles( sourceFile, targetFile );
    }

    protected override void CleanFile() {
      TargetFile.XPathSelectElements( "//nodeType [@teaCommerce]" ).Remove();
    }

  }
}
