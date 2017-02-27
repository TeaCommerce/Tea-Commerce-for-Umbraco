using System.Xml.Linq;
using System.Xml.XPath;

namespace TeaCommerce.Umbraco.Install.InstallTasks {
  public class LanguageFileInstallTask : AMergeXmlFileInstallTask {

    public LanguageFileInstallTask( string sourceFile, string targetFile ) : base() {
      CreateIfTargetFileNotExists = false;
      OverwriteValues = true;
      EmbeddedResource = true;
      SetFiles( sourceFile, targetFile );
    }

    protected override void CleanFile() {
      TargetFile.XPathSelectElements( "./language/area [@alias='teaCommerce']" ).Remove();
      TargetFile.XPathSelectElements( "./language/area [@alias='actions']/key [contains(@alias,'teaCommerce')]" ).Remove();
      TargetFile.XPathSelectElements( "./language/area [@alias='sections']/key [@alias='teacommerce']" ).Remove();
    }

  }
}
