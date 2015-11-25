using System;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;
using Umbraco.Web.UI;

namespace TeaCommerce.Umbraco.Application.Views.Security {
  public partial class EditUserPermissions : UmbracoProtectedPage {

    private Permissions permissions = PermissionService.Instance.Get( HttpContext.Current.Request.QueryString[ "id" ] );
    private Permissions currentUserPermissions;

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      umbraco.BusinessLogic.User currentUser = umbraco.helper.GetCurrentUmbracoUser();

      #region Security check
      currentUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentUserPermissions == null || !currentUserPermissions.HasPermission( GeneralPermissionType.AccessSecurity ) ) {
        throw new SecurityException();
      } else {
        bool showUser = true;

        umbraco.BusinessLogic.User user = umbraco.BusinessLogic.User.GetUser( int.Parse( HttpContext.Current.Request.QueryString[ "id" ] ) );
        showUser = !user.IsRoot(); //Don't ever show admin user

        if ( showUser ) {
          bool showAllUsers = currentUser.IsRoot() || currentUser.Applications.Any( a => a.alias == "users" );
          showUser = showAllUsers || currentUser.Id == user.Id || ( permissions != null && currentUserPermissions.StoreSpecificPermissions.Any( p => p.Value.HasFlag( StoreSpecificPermissionType.AccessStore ) && permissions.HasPermission( StoreSpecificPermissionType.AccessStore, p.Key ) ) );
        }

        if ( !showUser ) {
          throw new SecurityException();
        }
      }
      #endregion
      
      AddTab( CommonTerms.Common, PnlCommon, SaveButton_Clicked );

      PPnlAccessSecurity.Text = StoreTerms.Security;
      ImgAccessSecurity.ImageUrl = WebUtils.GetWebResourceUrl( Constants.TreeIcons.Lock );
      PPnlAccessLicenses.Text = DeveloperTerms.Licenses;
      ImgAccessLicenses.ImageUrl = WebUtils.GetWebResourceUrl( Constants.TreeIcons.LicenseKey );
      PPnlCreateAndDeleteStore.Text = CommonTerms.CreateAndDeleteStore;
      ImgCreateAndDeleteStore.ImageUrl = WebUtils.GetWebResourceUrl( Constants.TreeIcons.Store );

      PnStoreSpecificPermissions.Text = CommonTerms.Stores;
      PPnlStoreSpecificPermissions.Text = CommonTerms.StoreSpecificPermissions;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {

        PPnlAccessSecurity.Visible = currentUserPermissions.HasPermission( GeneralPermissionType.AccessSecurity );
        PPnlAccessLicenses.Visible = currentUserPermissions.HasPermission( GeneralPermissionType.AccessLicenses );
        PPnlCreateAndDeleteStore.Visible = currentUserPermissions.HasPermission( GeneralPermissionType.CreateAndDeleteStore );

        if ( permissions != null ) {
          ChkAccessSecurity.Checked = permissions.HasPermission( GeneralPermissionType.AccessSecurity );
          ChkAccessLicenses.Checked = permissions.HasPermission( GeneralPermissionType.AccessLicenses );
          ChkCreateAndDeleteStore.Checked = permissions.HasPermission( GeneralPermissionType.CreateAndDeleteStore );
        }

        //Loop stores and only show the ones the current logged in user has access to
        LvStores.DataSource = StoreService.Instance.GetAll().Where( s => currentUserPermissions.HasPermission( StoreSpecificPermissionType.AccessStore, s.Id ) );
        LvStores.DataBind();
        PnStoreSpecificPermissions.Visible = LvStores.Items.Any();
      }
    }

    protected void LvStores_ItemDataBound( object sender, ListViewItemEventArgs e ) {
      Store store = (Store)( e.Item as ListViewDataItem ).DataItem;

      e.Item.FindControl<Image>( "ImgAccessStore" ).ImageUrl = WebUtils.GetWebResourceUrl( Constants.TreeIcons.Store );
      CheckBox chkAccessStore = e.Item.FindControl<CheckBox>( "ChkAccessStore" );
      chkAccessStore.Text = store.Name;
      chkAccessStore.Checked = permissions != null && permissions.HasPermission( StoreSpecificPermissionType.AccessStore, store.Id );

      e.Item.FindControl<Image>( "ImgMarketing" ).ImageUrl = WebUtils.GetWebResourceUrl( Constants.TreeIcons.Target );
      CheckBox chkMarketing = e.Item.FindControl<CheckBox>( "ChkMarketing" );
      chkMarketing.Text = CommonTerms.Marketing;
      chkMarketing.Checked = permissions != null && permissions.HasPermission( StoreSpecificPermissionType.AccessMarketing, store.Id );
      e.Item.FindControl<Panel>( "PnlMarketing" ).Visible = currentUserPermissions.HasPermission( StoreSpecificPermissionType.AccessMarketing, store.Id );

      e.Item.FindControl<Image>( "ImgAccessSettings" ).ImageUrl = WebUtils.GetWebResourceUrl( Constants.TreeIcons.Toolbox );
      CheckBox chkAccessSettings = e.Item.FindControl<CheckBox>( "ChkAccessSettings" );
      chkAccessSettings.Text = CommonTerms.Settings;
      chkAccessSettings.Checked = permissions != null && permissions.HasPermission( StoreSpecificPermissionType.AccessSettings, store.Id );
      e.Item.FindControl<Panel>( "PnlAccessSettings" ).Visible = currentUserPermissions.HasPermission( StoreSpecificPermissionType.AccessSettings, store.Id );
    }

    protected void SaveButton_Clicked( object sender, EventArgs e ) {
      if ( Page.IsValid ) {
        if ( permissions == null ) {
          permissions = new Permissions( HttpContext.Current.Request.QueryString[ "id" ], false );
        }

        if ( ChkAccessSecurity.Checked ) {
          permissions.GeneralPermissions |= GeneralPermissionType.AccessSecurity;
        } else {
          if ( permissions.GeneralPermissions.HasFlag( GeneralPermissionType.AccessSecurity ) ) {
            permissions.GeneralPermissions ^= GeneralPermissionType.AccessSecurity;
          }
        }
        if ( ChkAccessLicenses.Checked ) {
          permissions.GeneralPermissions |= GeneralPermissionType.AccessLicenses;
        } else {
          if ( permissions.GeneralPermissions.HasFlag( GeneralPermissionType.AccessLicenses ) ) {
            permissions.GeneralPermissions ^= GeneralPermissionType.AccessLicenses;
          }
        }
        if ( ChkCreateAndDeleteStore.Checked ) {
          permissions.GeneralPermissions |= GeneralPermissionType.CreateAndDeleteStore;
        } else {
          if ( permissions.GeneralPermissions.HasFlag( GeneralPermissionType.CreateAndDeleteStore ) ) {
            permissions.GeneralPermissions ^= GeneralPermissionType.CreateAndDeleteStore;
          }
        }

        foreach ( ListViewDataItem item in LvStores.Items ) {
          long storeId = long.Parse( item.FindControl<HiddenField>( "HdfId" ).Value );

          if ( !permissions.StoreSpecificPermissions.ContainsKey( storeId ) ) {
            permissions.StoreSpecificPermissions.Add( storeId, StoreSpecificPermissionType.None );
          }

          if ( item.FindControl<CheckBox>( "ChkAccessStore" ).Checked ) {
            permissions.StoreSpecificPermissions[ storeId ] |= StoreSpecificPermissionType.AccessStore;
          } else {
            if ( permissions.StoreSpecificPermissions[ storeId ].HasFlag( StoreSpecificPermissionType.AccessStore ) ) {
              permissions.StoreSpecificPermissions[ storeId ] ^= StoreSpecificPermissionType.AccessStore;
            }
          }

          if ( item.FindControl<CheckBox>( "ChkMarketing" ).Checked ) {
            permissions.StoreSpecificPermissions[ storeId ] |= StoreSpecificPermissionType.AccessMarketing;
          } else {
            if ( permissions.StoreSpecificPermissions[ storeId ].HasFlag( StoreSpecificPermissionType.AccessMarketing ) ) {
              permissions.StoreSpecificPermissions[ storeId ] ^= StoreSpecificPermissionType.AccessMarketing;
            }
          }

          if ( item.FindControl<CheckBox>( "ChkAccessSettings" ).Checked ) {
            permissions.StoreSpecificPermissions[ storeId ] |= StoreSpecificPermissionType.AccessSettings;
          } else {
            if ( permissions.StoreSpecificPermissions[ storeId ].HasFlag( StoreSpecificPermissionType.AccessSettings ) ) {
              permissions.StoreSpecificPermissions[ storeId ] ^= StoreSpecificPermissionType.AccessSettings;
            }
          }

          if ( permissions.StoreSpecificPermissions[ storeId ] == StoreSpecificPermissionType.None ) {
            permissions.StoreSpecificPermissions.Remove( storeId );
          }
        }

        permissions.Save();

        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, CommonTerms.UserPermissionsSaved, string.Empty );
      }
    }

  }
}