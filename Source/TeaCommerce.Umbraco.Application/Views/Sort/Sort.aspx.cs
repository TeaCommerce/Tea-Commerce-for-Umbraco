using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Marketing.Models;
using TeaCommerce.Api.Marketing.Services;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using Umbraco.Web.UI.Pages;

namespace TeaCommerce.Umbraco.Application.Views.Sort {
  public partial class Sort : UmbracoProtectedPage {

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      string type = Request.QueryString[ "type" ];

      if ( typeof( Store ).FullName == type ) {
        if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( GeneralPermissionType.CreateAndDeleteStore ) ) {
          throw new SecurityException();
        }
      } else {
        //Store specific sorts
        long storeId = long.Parse( Request.QueryString[ "nodeId" ].Split( '_' )[ 1 ] );

        if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessSettings, storeId ) ) {
          throw new SecurityException();
        }
      }
      #endregion

      LitSortHelp.Text = CommonTerms.SortHelp;
      BtnSubmit.Text = CommonTerms.Save;
      LitCancel.Text = CommonTerms.Cancel;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        LvSortables.DataSource = GetSortables();
        LvSortables.DataBind();
        BtnSubmit.DataBind();
      }
    }

    private IEnumerable<ISortable> GetSortables() {
      string type = Request.QueryString[ "type" ];

      if ( typeof( Store ).FullName == type ) {
        return StoreService.Instance.GetAll();
      }
      //Store specific sorts
      long storeId = long.Parse( Request.QueryString[ "nodeId" ].Split( '_' )[ 1 ] );

      if ( typeof( Campaign ).FullName == type ) {
        return CampaignService.Instance.GetAll( storeId );
      }
      if ( typeof( OrderStatus ).FullName == type ) {
        return OrderStatusService.Instance.GetAll( storeId );
      }
      if ( typeof( ShippingMethod ).FullName == type ) {
        return ShippingMethodService.Instance.GetAll( storeId );
      }
      if ( typeof( PaymentMethod ).FullName == type ) {
        return PaymentMethodService.Instance.GetAll( storeId );
      }
      if ( typeof( Country ).FullName == type ) {
        return CountryService.Instance.GetAll( storeId );
      }
      if ( typeof( CountryRegion ).FullName == type ) {
        long countryId = long.Parse( Request.QueryString[ "nodeId" ].Split( '_' )[ 2 ] );
        return CountryRegionService.Instance.GetAll( storeId, countryId );
      }
      if ( typeof( Currency ).FullName == type ) {
        return CurrencyService.Instance.GetAll( storeId );
      }
      if ( typeof( EmailTemplate ).FullName == type ) {
        return EmailTemplateService.Instance.GetAll( storeId );
      }
      if ( typeof( VatGroup ).FullName == type )
        return VatGroupService.Instance.GetAll( storeId );

      throw new ArgumentException( type + " is not supported for sorting" );
    }

    protected void LvSortables_LayoutCreated( object sender, EventArgs e ) {
      ListView LvSortables = sender as ListView;
      LvSortables.FindControl<Literal>( "LtNameHeader" ).Text = CommonTerms.Name;
      LvSortables.FindControl<Literal>( "LtNameSort" ).Text = CommonTerms.Sort;
    }

    protected void BtnSubmit_Click( object sender, EventArgs e ) {
      List<ISortable> sortables = GetSortables().ToList();
      foreach ( ListViewDataItem item in LvSortables.Items ) {
        int sort = int.Parse( ( item.FindControl( "HdnSort" ) as HiddenField ).Value );
        long id = long.Parse( ( item.FindControl( "HdnId" ) as HiddenField ).Value );
        ISortable sortable = sortables.First( s => s.Id.Equals( id ) );
        sortable.Sort = sort;
        Save( sortable );
      }
      new ClientTools( Page ).ChildNodeCreated().CloseModalWindow();
    }

    private void Save( ISortable entity ) {
      if ( entity is Store ) {
        ( (Store)entity ).Save();
      } else if ( entity is Campaign ) {
        ( (Campaign)entity ).Save();
      } else if ( entity is OrderStatus ) {
        ( (OrderStatus)entity ).Save();
      } else if ( entity is ShippingMethod ) {
        ( (ShippingMethod)entity ).Save();
      } else if ( entity is PaymentMethod ) {
        ( (PaymentMethod)entity ).Save();
      } else if ( entity is Country ) {
        ( (Country)entity ).Save();
      } else if ( entity is CountryRegion ) {
        ( (CountryRegion)entity ).Save();
      } else if ( entity is Currency ) {
        ( (Currency)entity ).Save();
      } else if ( entity is EmailTemplate ) {
        ( (EmailTemplate)entity ).Save();
      } else if ( entity is VatGroup ) {
        ( (VatGroup)entity ).Save();
      }
    }

  }
}