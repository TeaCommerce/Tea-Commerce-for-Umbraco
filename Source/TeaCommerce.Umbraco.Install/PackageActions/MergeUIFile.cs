using System.Xml.Linq;
using System.Xml.XPath;

namespace TeaCommerce.Umbraco.Install.PackageActions {
  public class MergeUIFile : AMergeXmlFile {

    public override string Alias() {
      return "MergeUIFile";
    }

    protected override void CleanFile() {
      TargetFile.XPathSelectElements( "//nodeType [@teaCommerce]" ).Remove();
    }

  }
}