using System;
using System.Security;
using TeaCommerce.Api.Infrastructure.Licensing;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;

namespace TeaCommerce.Umbraco.Application.Views.Developer {
  public partial class LicenseCheck : UmbracoProtectedPage {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( GeneralPermissionType.AccessLicenses ) ) {
        throw new SecurityException();
      }
      #endregion

      AddTab( CommonTerms.Common, PnlLicense );

      LitHeading.Text = DeveloperTerms.ValidDomains;
      LitWantToBuyALicense.Text = DeveloperTerms.WantToBuyALicense + " ?";
      LitLink.Text = DeveloperTerms.GoToTheTeaCommerceWebsite;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );
      
      if ( !IsPostBack ) {
        LvLicensesTeaCommerce.DataSource = LicenseService.Instance.GetLicenses();
        LvLicensesTeaCommerce.DataBind();
      }
    }


  }
}