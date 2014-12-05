using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;

namespace TeaCommerce.Umbraco.Application.Views.Stores {
  public partial class CreateStore : UmbracoUserControl {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      LitName.Text = CommonTerms.Name;
      BtnCreate.Text = CommonTerms.Create;
      LitOr.Text = CommonTerms.Or;
      LitCancel.Text = CommonTerms.Cancel;
    }

    protected void BtnCreate_Click( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        Store store = new Store( TxtName.Text );
        store.Save();

        //Give permissions to the current logged in user
        Permissions permissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
        if ( permissions != null && !permissions.IsUserSuperAdmin ) {
          StoreSpecificPermissionType storePermissions = StoreSpecificPermissionType.None;

          foreach ( StoreSpecificPermissionType permissionType in Enum.GetValues( typeof( StoreSpecificPermissionType ) ).Cast<StoreSpecificPermissionType>() ) {
            storePermissions |= permissionType;
          }
          permissions.StoreSpecificPermissions.Add( store.Id, storePermissions );
          permissions.Save();
        }
        
        const string editOrderFilePath = @"macroScripts\tea-commerce\edit-order.cshtml";
        if ( File.Exists( HostingEnvironment.MapPath( "~" + Path.DirectorySeparatorChar + editOrderFilePath ) ) ) {
          store.UISettings.EditOrderUiFile = editOrderFilePath;
          store.Save();
        }

        if ( store.GeneralSettings.ConfirmationEmailTemplateId != null ) {
          const string emailTemplateFile = @"macroScripts\tea-commerce\email-template-confirmation.cshtml";

          if ( File.Exists( HostingEnvironment.MapPath( "~" + Path.DirectorySeparatorChar + emailTemplateFile ) ) ) {
            EmailTemplate emailTemplate = EmailTemplateService.Instance.Get( store.Id, store.GeneralSettings.ConfirmationEmailTemplateId.Value );
            EmailTemplateSettings defaultSettings = emailTemplate.Settings.SingleOrDefault( s => s.LanguageId == null );
            if ( defaultSettings != null ) {
              defaultSettings.TemplateFile = emailTemplateFile;
            }
            emailTemplate.Save();
          }
        }

        if ( store.GeneralSettings.PaymentInconsistencyEmailTemplateId != null ) {
          const string emailTemplateFile = @"macroScripts\tea-commerce\email-template-payment-inconsistency.cshtml";

          if ( File.Exists( HostingEnvironment.MapPath( "~" + Path.DirectorySeparatorChar + emailTemplateFile ) ) ) {
            EmailTemplate emailTemplate = EmailTemplateService.Instance.Get( store.Id, store.GeneralSettings.PaymentInconsistencyEmailTemplateId.Value );
            EmailTemplateSettings defaultSettings = emailTemplate.Settings.SingleOrDefault( s => s.LanguageId == null );
            if ( defaultSettings != null ) {
              defaultSettings.TemplateFile = emailTemplateFile;
            }
            emailTemplate.Save();
          }
        }

        Redirect( WebUtils.GetPageUrl( Constants.Pages.EditStore ) + "?id=" + store.Id );
      }

    }

  }
}