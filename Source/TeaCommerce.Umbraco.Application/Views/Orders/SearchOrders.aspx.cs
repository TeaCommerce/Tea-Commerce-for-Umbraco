using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Infrastructure.Licensing;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Api.Web.PaymentProviders;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;

namespace TeaCommerce.Umbraco.Application.Views.Orders {
  public partial class SearchOrders : UmbracoProtectedPage {

    //Must be protected to give the UI file access
    protected long StoreId { get { return long.Parse( HttpContext.Current.Request.QueryString["storeId"] ); } }

    private long? _currentPage;
    private long CurrentPage {
      get {
        if ( _currentPage == null )
          _currentPage = (long)ViewState["CurrentPage"];

        return _currentPage.Value;
      }
      set {
        if ( value == 1 || ( value > 1 && value <= MaxPages ) )
          ViewState["CurrentPage"] = _currentPage = value;
      }
    }

    private long? _maxPages;
    private long MaxPages {
      get {
        if ( _maxPages == null )
          _maxPages = (long)ViewState["MaxPages"];

        return _maxPages.Value;
      }
      set {
        ViewState["MaxPages"] = _maxPages = value;
      }
    }

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessStore, StoreId ) ) {
        throw new SecurityException();
      }
      #endregion

      AddTab( CommonTerms.Common, PnlSearch );

      LitTrialMode.Text = CommonTerms.TrialMode;
      HypTrialMode.Text = CommonTerms.TrialModeBuy;
      PnSearchCriteria.Text = CommonTerms.SearchCriteria;
      PPnlOrderNumber.Text = CommonTerms.OrderNumer;
      PPnlFirstName.Text = CommonTerms.FirstName;
      PPnlLastName.Text = CommonTerms.LastName;
      PPnlPaymentStatus.Text = CommonTerms.PaymentState;
      PPnlOrderStage.Text = CommonTerms.OrderStage;
      PPnlPageSize.Text = CommonTerms.OrdersPerPage;
      PPnlStartDate.Text = CommonTerms.StartDate;
      PPnlEndDate.Text = CommonTerms.EndDate;
      BtnSearch.Text = CommonTerms.Search;
      BtnReset.Text = CommonTerms.Reset;
      PnSearchResult.Text = CommonTerms.SearchResult;

      PnTools.Text = CommonTerms.Tools;
      PPnlDeleteOrders.Text = BtnDeleteOrders.Text = CommonTerms.DeleteOrders;
      ChkRevertFinalize.Text = CommonTerms.RevertFinalize;
      PPnlCapturePayments.Text = BtnCapturePayments.Text = CommonTerms.CapturePayment;
      PPnlChangeOrderStatus.Text = BtnChangeOrderStatus.Text = CommonTerms.ChangeOrderStatus;
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        PnlLicenseCheck.Visible = !LicenseService.Instance.ValidateLicenseFeatures( Feature.Basic );

        LoadOrderStages();
        PaymentStatusSelectorControl.LoadPaymentStatuses();
        OrderStatusSelectorControl.LoadOrderStatuses( StoreId );
        OrderStatusSelectorControl.CssClass = "guiInputText guiInputSmallSize";
        OrderStatusSelectorControl.Items.Insert( 0, new ListItem( "----", "" ) );

        #region Load search filters in session

        string orderNumber = (string)Session["Search_OrderNumber"];
        string firstName = (string)Session["Search_FirstName"];
        string lastName = (string)Session["Search_LastName"];
        PaymentState? paymentStatus = (PaymentState?)Session["Search_PaymentStatus"];
        bool? orderStage = (bool?)Session["Search_OrderStage"];
        long? pageSize = (long?)Session["Search_PageSize"];
        DateTime? startDate = (DateTime?)Session["Search_StartDate"];
        DateTime? endDate = (DateTime?)Session["Search_EndDate"];
        string orderStatusAlias = OrderStatusService.Instance.Get( StoreId, long.Parse( HttpContext.Current.Request.QueryString["orderStatusId"] ) ).Alias;
        orderStatusAlias = orderStatusAlias.First().ToString().ToUpper() + orderStatusAlias.Substring( 1 );

        TxtOrderNumber.Text = orderNumber;
        TxtFirstName.Text = firstName;
        TxtLastName.Text = lastName;
        PaymentStatusSelectorControl.Items.TrySelectByValue( paymentStatus );
        DrpOrderStages.Items.TrySelectByValue( orderStage );
        DrpPageSize.Items.TrySelectByValue( pageSize );
        DPStart.DateTime = startDate ?? DateTime.MinValue;
        TxtOrderStatus.Text = orderStatusAlias;

        if ( endDate != null ) {
          DPEnd.DateTime = endDate.Value.AddDays( -1 );
        }

        #endregion

        CurrentPage = 1;
        Search();
      }
    }

    protected void BtnChangeOrderStatus_Click( object sender, EventArgs e ) {
      long orderStatusId = long.Parse( OrderStatusSelectorControl.SelectedValue );
      foreach ( Order order in OrderService.Instance.Get( StoreId, GetSelectedOrderIds() ) ) {
        order.OrderStatusId = orderStatusId;
        order.Save();
      }
      Search();
    }

    protected void BtnDeleteOrders_Click( object sender, EventArgs e ) {
      foreach ( Order order in OrderService.Instance.Get( StoreId, GetSelectedOrderIds() ) ) {
        order.Delete( ChkRevertFinalize.Checked );
      }
      Search();
    }

    protected void BtnCapturePayments_Click( object sender, EventArgs e ) {
      foreach ( Order order in OrderService.Instance.Get( StoreId, GetSelectedOrderIds() ) ) {
        if ( order.PaymentInformation.PaymentMethodId != null ) {
          PaymentMethodService.Instance.Get( order.StoreId, order.PaymentInformation.PaymentMethodId.Value ).CapturePayment( order );
        }
      }
      Search();
    }

    protected void BtnSearch_Click( object sender, EventArgs e ) {
      CurrentPage = 1;
      Search();
    }

    protected void BtnReset_Click( object sender, EventArgs e ) {
      TxtOrderNumber.Text = string.Empty;
      TxtFirstName.Text = string.Empty;
      TxtLastName.Text = string.Empty;
      PaymentStatusSelectorControl.SelectedValue = string.Empty;
      DrpOrderStages.SelectedValue = string.Empty;
      DrpPageSize.SelectedValue = "25";
      DPStart.DateTime = DateTime.MinValue;
      DPEnd.DateTime = DateTime.MinValue;

      CurrentPage = 1;
      Search();
    }

    private void Search() {
      string orderNumber = TxtOrderNumber.Text;
      string firstName = TxtFirstName.Text;
      string lastName = TxtLastName.Text;
      long orderStatusId = long.Parse( HttpContext.Current.Request.QueryString["orderStatusId"] );
      PaymentState? paymentState = PaymentStatusSelectorControl.SelectedValue.TryParse<PaymentState>();
      bool? orderStage = DrpOrderStages.SelectedValue.TryParse<bool>();
      long pageSize = long.Parse( DrpPageSize.SelectedValue );
      DateTime? startDate = DPStart.DateTime != DateTime.MinValue ? (DateTime?)DPStart.DateTime : null;
      DateTime? endDate = DPEnd.DateTime != DateTime.MinValue ? (DateTime?)DPEnd.DateTime.AddDays( 1d ) : null;

      #region Save search filters in session

      Session["Search_OrderNumber"] = orderNumber;
      Session["Search_FirstName"] = firstName;
      Session["Search_LastName"] = lastName;
      Session["Search_PaymentStatus"] = paymentState;
      Session["Search_OrderStage"] = orderStage;
      Session["Search_PageSize"] = pageSize;
      Session["Search_StartDate"] = startDate;
      Session["Search_EndDate"] = endDate;

      #endregion

      Tuple<IEnumerable<Order>, long> pagedOrders = OrderService.Instance.Search( StoreId, orderStatusId, orderNumber, firstName, lastName, paymentState, startDate, endDate, orderStage, CurrentPage, pageSize );

      MaxPages = pagedOrders.Item2;

      #region Paging

      LitMaxPages.Text = MaxPages.ToString( CultureInfo.CurrentUICulture );
      LitCurrentPage.Text = CurrentPage.ToString( CultureInfo.CurrentUICulture );
      LBtnFirstPage.Enabled = LBtnPreviousPage.Enabled = CurrentPage != 1;
      LBtnNextPage.Enabled = LBtnLastPage.Enabled = CurrentPage != MaxPages;
      LBtnFirstPage.Enabled = CurrentPage != 1;
      PnlPager.Visible = MaxPages > 1;

      #endregion

      LvOrders.DataSource = pagedOrders.Item1.OrderByDescending( o => o.DateFinalized ?? o.DateCreated );
      LvOrders.DataBind();
      PnSearchResult.Visible = LvOrders.Items.Count > 0;
    }

    protected void LBtnNextPage_Click( object sender, EventArgs e ) {
      CurrentPage++;
      Search();
    }

    protected void LBtnPreviousPage_Click( object sender, EventArgs e ) {
      CurrentPage--;
      Search();
    }

    protected void LBtnFirstPage_Click( object sender, EventArgs e ) {
      CurrentPage = 1;
      Search();
    }

    protected void LBtnLastPage_Click( object sender, EventArgs e ) {
      CurrentPage = MaxPages;
      Search();
    }

    private void LoadOrderStages() {
      DrpOrderStages.Items.Clear();
      DrpOrderStages.Items.Add( new ListItem( CommonTerms.Order, bool.TrueString ) );
      DrpOrderStages.Items.Add( new ListItem( CommonTerms.Cart, bool.FalseString ) );
      DrpOrderStages.Items.Add( new ListItem( CommonTerms.All, string.Empty ) );
    }

    private IEnumerable<Guid> GetSelectedOrderIds() {
      return ( from item in LvOrders.Items
               where item.FindControl<CheckBox>( "ChkSelect" ).Checked
               select Guid.Parse( item.FindControl<HiddenField>( "HdnId" ).Value ) ).ToList();
    }
  }
}