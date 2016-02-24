using System;
using TeaCommerce.Api.Models;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.Utils;
using umbraco.interfaces;

namespace TeaCommerce.Umbraco.Application.Trees.Actions {
  public class CreateAllCountriesAction : ACreateAllAction {
    public override Type SortType { get { return typeof( Country ); } }
    public override string JsFunctionName { get { return "createAllCountries()"; } }
    public override char Letter { get { return 'ý'; } }
    public override string Title { get { return CommonTerms.CreateAllCountries; } }
  }

  public abstract class ACreateAllAction : IAction {
    public abstract Type SortType { get; }
    public abstract string Title { get; }

    public string Alias { get { return "teaCommerceCreateAll"; } }
    public bool CanBePermissionAssigned { get { return false; } }
    public string Icon { get { return "add"; } }
    public abstract string JsFunctionName { get; }
    public string JsSource {
      get {
        return "function " + JsFunctionName + " { UmbClientMgr.openModalWindow('" + WebUtils.GetPageUrl( Constants.Pages.CreateAll ) + "?type=" + SortType.FullName + "&nodeId=' + UmbClientMgr.mainTree().getActionNode().nodeId, '" + Title + "', true, 600, 425); }";
      }
    }
    public abstract char Letter { get; }
    public bool ShowInNotifier { get { return true; } }
  }
}