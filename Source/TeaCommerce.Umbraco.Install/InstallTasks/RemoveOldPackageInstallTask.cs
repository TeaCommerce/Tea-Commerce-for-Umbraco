using System.Collections.Generic;
using System.Linq;
using umbraco.cms.businesslogic.packager;

namespace TeaCommerce.Umbraco.Install.InstallTasks {
  public class RemoveOldPackageInstallTask : IInstallTask {

    public RemoveOldPackageInstallTask() {
    }

    public void Install() {
      List<InstalledPackage> teaCommercePackages = InstalledPackage.GetAllInstalledPackages().Where( ip => ip.Data.Name.Equals( "Tea Commerce" ) || ip.Data.Name.Equals( "teacommerce" ) ).ToList();

      if ( teaCommercePackages.Count > 1 ) {
        teaCommercePackages.RemoveAt( teaCommercePackages.Count - 1 );
        foreach ( InstalledPackage package in teaCommercePackages )
          package.Delete();
      }
    }

    public void Uninstall() {

    }
  }
}
