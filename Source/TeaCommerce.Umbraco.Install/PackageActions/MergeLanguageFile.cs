using System.Xml.Linq;
using System.Xml.XPath;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class MergeLanguageFile : AMergeXmlFile {

    public override string Alias() {
      return "MergeLanguageFile";
    }

    protected override void CleanFile() {
      TargetFile.XPathSelectElements( "./language/area [@alias='teaCommerce']" ).Remove();
      TargetFile.XPathSelectElements( "./language/area [@alias='actions']/key [contains(@alias,'teaCommerce')]" ).Remove();
      TargetFile.XPathSelectElements( "./language/area [@alias='sections']/key [@alias='teacommerce']" ).Remove();
    }

  }
}