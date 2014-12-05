using System;
using TeaCommerce.Api.Marketing.Models;
using TeaCommerce.Api.Models;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.Utils;
using umbraco.interfaces;

namespace TeaCommerce.Umbraco.Application.Trees.Actions {

  //NOTE: Actions can't be generic or have constructors with parameters

  public class SortStoresAction : ASortAction {
    public override Type SortType { get { return typeof( Store ); } }
    public override string JsFunctionName { get { return "sortStores()"; } }
    public override char Letter { get { return 'Ý'; } }
  }

  public class SortCampaignsAction : ASortAction {
    public override Type SortType { get { return typeof( Campaign ); } }
    public override string JsFunctionName { get { return "sortCampaigns()"; } }
    public override char Letter { get { return 'Ĵ'; } }
  }

  public class SortOrderStatusesAction : ASortAction {
    public override Type SortType { get { return typeof( OrderStatus ); } }
    public override string JsFunctionName { get { return "sortOrderStatuses()"; } }
    public override char Letter { get { return 'Ò'; } }
  }

  public class SortShippingMethodsAction : ASortAction {
    public override Type SortType { get { return typeof( ShippingMethod ); } }
    public override string JsFunctionName { get { return "sortShippingMethods()"; } }
    public override char Letter { get { return 'Ô'; } }
  }

  public class SortPaymentMethodsAction : ASortAction {
    public override Type SortType { get { return typeof( PaymentMethod ); } }
    public override string JsFunctionName { get { return "sortPaymentMethods()"; } }
    public override char Letter { get { return 'Ó'; } }
  }

  public class SortCountriesAction : ASortAction {
    public override Type SortType { get { return typeof( Country ); } }
    public override string JsFunctionName { get { return "sortCountries()"; } }
    public override char Letter { get { return 'Û'; } }
  }

  public class SortCountryRegionsAction : ASortAction {
    public override Type SortType { get { return typeof( CountryRegion ); } }
    public override string JsFunctionName { get { return "sortCountryRegions()"; } }
    public override char Letter { get { return 'Ï'; } }
  }

  public class SortCurrenciesAction : ASortAction {
    public override Type SortType { get { return typeof( Currency ); } }
    public override string JsFunctionName { get { return "sortCurrencies()"; } }
    public override char Letter { get { return 'Ñ'; } }
  }

  public class SortVatGroupsAction : ASortAction {
    public override Type SortType { get { return typeof( VatGroup ); } }
    public override string JsFunctionName { get { return "sortVATGroups()"; } }
    public override char Letter { get { return 'Ð'; } }
  }

  public class SortEmailTemplatesAction : ASortAction {
    public override Type SortType { get { return typeof( EmailTemplate ); } }
    public override string JsFunctionName { get { return "sortEmailTemplates()"; } }
    public override char Letter { get { return 'Õ'; } }
  }

  public abstract class ASortAction : IAction {

    public abstract Type SortType { get; }

    public string Alias { get { return "teaCommerceSort"; } }
    public bool CanBePermissionAssigned { get { return false; } }
    public string Icon { get { return "navigation-vertical"; } }
    public abstract string JsFunctionName { get; }
    public string JsSource {
      get {
        return "function " + JsFunctionName + " { UmbClientMgr.openModalWindow('" + WebUtils.GetPageUrl( Constants.Pages.Sort ) + "?type=" + SortType.FullName + "&nodeId=' + UmbClientMgr.mainTree().getActionNode().nodeId, '" + CommonTerms.Sort + "', true, 600, 425); }";
      }
    }
    public abstract char Letter { get; }
    public bool ShowInNotifier { get { return true; } }

  }

}