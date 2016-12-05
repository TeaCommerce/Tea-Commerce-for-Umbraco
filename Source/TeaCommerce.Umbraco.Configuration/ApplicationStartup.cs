using System;
using System.Linq;
using System.Reflection;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Configuration.Services;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Logging;
using Umbraco.Core.Models.Membership;
using Umbraco.Core.Services;

namespace TeaCommerce.Umbraco.Configuration {
  public class ApplicationStartup : ApplicationEventHandler {

    protected override void ApplicationStarted( UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext ) {
      try {
        DependencyContainer.Configure( Assembly.Load( "TeaCommerce.Umbraco.Configuration" ) );
      } catch ( Exception exp ) {
        LogHelper.Error<ApplicationStartup>( "Error loading Autofac modules", exp );
      }

      Domain.New += Domain_New;
      Domain.AfterSave += Domain_AfterSave;
      Domain.AfterDelete += Domain_AfterDelete;

      UserService.SavedUser += UserService_SavedUser;
    }

    void UserService_SavedUser( IUserService sender, SaveEventArgs<IUser> e ) {
      //Add all permissions to user if they have access to the Tea Commerce section, but no permissions at all in Tea Commerce
      foreach ( IUser user in e.SavedEntities ) {
        if ( user.AllowedSections.Contains( "teacommerce" ) ) {

          //Chekc if user has no permissions in Tea Commerce
          Permissions permissions = PermissionService.Instance.Get( user.Id.ToInvariantString() );
          if ( permissions != null && !permissions.IsUserSuperAdmin ) {
            bool createPermissions = permissions.GeneralPermissions.Equals( GeneralPermissionType.None );

            if ( createPermissions ) {
              foreach ( Store store in StoreService.Instance.GetAll() ) {
                if ( permissions.StoreSpecificPermissions.ContainsKey( store.Id ) && !permissions.StoreSpecificPermissions[ store.Id ].Equals( StoreSpecificPermissionType.None ) ) {
                  createPermissions = false;
                  break;
                }
              }
            }

            if ( createPermissions ) {

              //Give all general permissions
              foreach ( GeneralPermissionType permissionType in Enum.GetValues( typeof( GeneralPermissionType ) ).Cast<GeneralPermissionType>() ) {
                permissions.GeneralPermissions |= permissionType;
              }

              //Give all store specific permissions to all stores
              foreach ( Store store in StoreService.Instance.GetAll() ) {
                StoreSpecificPermissionType storePermissions = StoreSpecificPermissionType.None;

                foreach ( StoreSpecificPermissionType permissionType in Enum.GetValues( typeof( StoreSpecificPermissionType ) ).Cast<StoreSpecificPermissionType>() ) {
                  storePermissions |= permissionType;
                }
                permissions.StoreSpecificPermissions.Add( store.Id, storePermissions );
              }

              permissions.Save();
            }

          }
        }
      }
    }

    void Domain_New( Domain sender, NewEventArgs e ) {
      Compatibility.Domain.InvalidateCache();
    }

    void Domain_AfterSave( Domain sender, SaveEventArgs e ) {
      Compatibility.Domain.InvalidateCache();
    }

    void Domain_AfterDelete( Domain sender, umbraco.cms.businesslogic.DeleteEventArgs e ) {
      Compatibility.Domain.InvalidateCache();
    }
  }
}