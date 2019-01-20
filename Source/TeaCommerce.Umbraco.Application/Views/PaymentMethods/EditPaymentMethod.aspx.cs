using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Common;
using TeaCommerce.Api.Infrastructure.Security;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Services;
using TeaCommerce.Api.Web.PaymentProviders;
using TeaCommerce.Umbraco.Application.Resources;
using TeaCommerce.Umbraco.Application.UI;
using TeaCommerce.Umbraco.Application.Utils;
using umbraco.cms.businesslogic.language;
using umbraco.uicontrols;
using Umbraco.Web.UI;

namespace TeaCommerce.Umbraco.Application.Views.PaymentMethods {
  public partial class EditPaymentMethod : UmbracoProtectedPage {

    private PaymentMethod paymentMethod = PaymentMethodService.Instance.Get( long.Parse( HttpContext.Current.Request.QueryString[ "storeId" ] ), long.Parse( HttpContext.Current.Request.QueryString[ "id" ] ) );
    private IEnumerable<Language> umbracoLanguages = Language.GetAllAsList();

    private IEnumerable<CountryRegion> allCountryRegions;
    private IEnumerable<Currency> allCurrencies;
    long currentCountryId;
    long? currentCountryRegionId;

    private IPaymentProvider selectedPaymentProviderInstance;
    private IPaymentProvider SelectedPaymentProviderInstance {
      get {
        if ( selectedPaymentProviderInstance == null ) {
          selectedPaymentProviderInstance = PaymentProviderService.Instance.Get( DrpPaymentProviders.SelectedValue );
        }
        return selectedPaymentProviderInstance;
      }
    }

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );

      #region Security check
      Permissions currentLoggedInUserPermissions = PermissionService.Instance.GetCurrentLoggedInUserPermissions();
      if ( currentLoggedInUserPermissions == null || !currentLoggedInUserPermissions.HasPermission( StoreSpecificPermissionType.AccessSettings, paymentMethod.StoreId ) ) {
        throw new SecurityException();
      }
      #endregion

      AddTab( CommonTerms.Common, PnlCommon, SaveButton_Clicked );
      AddTab( CommonTerms.AvailableInTheseCountries, PnCurrencies, SaveButton_Clicked );
      AddTab( PaymentProviderTerms.PaymentProvider, PnlPaymentProvider, SaveButton_Clicked );

      PPnlName.Text = CommonTerms.Name;
      PPnlDictionaryItemName.Text = CommonTerms.Alias;
      PPnlSku.Text = CommonTerms.Sku;
      PPnlVatGroup.Text = CommonTerms.VatGroup;
      PPnlImage.Text = CommonTerms.Image;
      PnDefaultCurrencies.Text = CommonTerms.DefaultPrices;

      ChkSelectAll.Text = CommonTerms.SelectAll;

      LitPaymentProvider.Text = PaymentProviderTerms.PaymentProvider;
      BtnOverwriteSettings.OnClientClick = "return confirm('" + PaymentProviderTerms.LoadDefaultSettingsConfirm + "');";
      BtnOverwriteSettings.Text = PaymentProviderTerms.LoadDefaultSettings;
      HypDocumentation.Text = PaymentProviderTerms.LinkToDocumentation;

      PnPaymentProviderAPICalls.Text = PaymentProviderTerms.AllowedApiCalls;
      ChkAllowsGetStatus.Text = PaymentProviderTerms.GetStatus;
      ChkAllowsCapturePayment.Text = PaymentProviderTerms.CapturePayment;
      ChkAllowsRefundPayment.Text = PaymentProviderTerms.RefundPayment;
      ChkAllowsCancelPayment.Text = PaymentProviderTerms.CancelPayment;

      PlcCommonSettings.Controls.Add( AddSettingList( null ) );

      foreach ( Language language in umbracoLanguages )
        AddTab( language.CultureAlias, AddSettingList( language.id ), SaveButton_Clicked );
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      if ( !IsPostBack ) {
        LoadPaymentProviders();

        DrpVatGroups.DataSource = VatGroupService.Instance.GetAll( paymentMethod.StoreId );
        DrpVatGroups.DataBind();

        TxtName.Text = paymentMethod.Name;
        TxtDictionaryItemName.Text = paymentMethod.Alias;
        TxtSku.Text = paymentMethod.Sku;
        if ( paymentMethod.VatGroupId != null ) {
          DrpVatGroups.SelectedValue = paymentMethod.VatGroupId.Value.ToString();
        }
        if ( !string.IsNullOrEmpty( paymentMethod.ImageIdentifier ) ) {
          CPImage.Value = paymentMethod.ImageIdentifier;
        }

        foreach ( ListItem item in DrpPaymentProviders.Items ) {
          item.Selected = item.Value == paymentMethod.PaymentProviderAlias;
        }

        if ( paymentMethod.Settings.Count == 0 )
          OverwriteSettings();

        LoadPaymentMethodSettings();
        LoadPaymentProviderAPIAllows( false );

        LoadCurrenciesAndCountries();
      }
    }

    private void LoadCurrenciesAndCountries() {
      allCountryRegions = CountryRegionService.Instance.GetAll( paymentMethod.StoreId );
      allCurrencies = CurrencyService.Instance.GetAll( paymentMethod.StoreId );

      LvDefaultCurrencies.DataSource = allCurrencies;
      LvDefaultCurrencies.DataBind();

      LvCountries.DataSource = CountryService.Instance.GetAll( paymentMethod.StoreId );
      LvCountries.DataBind();
    }

    protected void LvDefaultCurrencies_ItemDataBound( object sender, ListViewItemEventArgs e ) {
      Currency currency = (Currency)( e.Item as ListViewDataItem ).DataItem;

      ServicePrice servicePrice = paymentMethod.OriginalPrices.Get( currency.Id );
      if ( servicePrice != null ) {
        e.Item.FindControl<HiddenField>( "HdfPriceId" ).Value = servicePrice.Id.ToString();
        e.Item.FindControl<TextBox>( "TxtPrice" ).Text = servicePrice.Value.ToString( "0.####" );
      }
    }

    protected void LvCountries_ItemDataBound( object sender, ListViewItemEventArgs e ) {
      Country country = (Country)( e.Item as ListViewDataItem ).DataItem;
      currentCountryId = country.Id;
      currentCountryRegionId = null;

      e.Item.FindControl<CheckBox>( "ChkIsSupported" ).Checked = paymentMethod.AllowedInFollowingCountries.Contains( country.Id );
      e.Item.FindControl<HyperLink>( "HypCustomPrices" ).Text = CommonTerms.CustomPrices;

      ListView LvCurrencies = (ListView)e.Item.FindControl( "LvCurrencies" );
      LvCurrencies.DataSource = allCurrencies.Where( c => c.AllowedInFollowingCountries.Contains( country.Id ) );
      LvCurrencies.DataBind();

      ListView lvCountryRegions = (ListView)e.Item.FindControl( "LvCountryRegions" );
      lvCountryRegions.DataSource = allCountryRegions.Where( cr => cr.CountryId == country.Id );
      lvCountryRegions.DataBind();
    }

    protected void LvCountryRegions_ItemDataBound( object sender, ListViewItemEventArgs e ) {
      CountryRegion countryRegion = (CountryRegion)( e.Item as ListViewDataItem ).DataItem;
      currentCountryRegionId = countryRegion.Id;

      e.Item.FindControl<CheckBox>( "ChkIsSupported" ).Checked = paymentMethod.AllowedInFollowingCountryRegions.Contains( countryRegion.Id );
      e.Item.FindControl<HyperLink>( "HypCustomPrices" ).Text = CommonTerms.CustomPrices;

      ListView LvCurrencies = (ListView)e.Item.FindControl( "LvCurrencies" );
      LvCurrencies.DataSource = allCurrencies.Where( c => c.AllowedInFollowingCountries.Contains( countryRegion.CountryId ) );
      LvCurrencies.DataBind();
    }

    protected void LvCurrencies_LayoutCreated( object sender, EventArgs e ) {
      ListView lvCurrencies = sender as ListView;
      lvCurrencies.FindControl<Literal>( "LitCurrency" ).Text = CommonTerms.Currency;
      lvCurrencies.FindControl<Literal>( "LitShippingFee" ).Text = CommonTerms.Price;
    }

    protected void LvCurrencies_ItemDataBound( object sender, ListViewItemEventArgs e ) {
      Currency currency = (Currency)( e.Item as ListViewDataItem ).DataItem;

      //Write Currency name
      e.Item.FindControl<Label>( "LblCurrencyName" ).Text = currency.Name;

      ServicePrice servicePrice = paymentMethod.OriginalPrices.Get( currency.Id, currentCountryId, currentCountryRegionId );
      if ( servicePrice != null ) {
        e.Item.FindControl<HiddenField>( "HdfPriceId" ).Value = servicePrice.Id.ToString();
        e.Item.FindControl<TextBox>( "TxtPrice" ).Text = servicePrice.Value.ToString( "0.####" );
      }
    }

    protected void SaveButton_Clicked( object sender, EventArgs e ) {
      if ( Page.IsValid ) {

        paymentMethod.Name = TxtName.Text;
        paymentMethod.Alias = TxtDictionaryItemName.Text;
        paymentMethod.Sku = TxtSku.Text;
        paymentMethod.VatGroupId = DrpVatGroups.SelectedValue.TryParse<long>();
        paymentMethod.ImageIdentifier = CPImage.Value;

        paymentMethod.AllowedInFollowingCountries = new List<long>();
        paymentMethod.AllowedInFollowingCountryRegions = new List<long>();

        //Add default prices
        LoopPrices( LvDefaultCurrencies );

        //Run through countries
        foreach ( ListViewDataItem item in LvCountries.Items ) {
          long countryId = long.Parse( item.FindControl<HiddenField>( "HdfId" ).Value );

          //Check to see if the country has been selected
          if ( item.FindControl<CheckBox>( "ChkIsSupported" ).Checked ) {
            paymentMethod.AllowedInFollowingCountries.Add( countryId );
          }

          LoopPrices( item.FindControl<ListView>( "LvCurrencies" ), countryId );

          //Run through country regions
          ListView lvCountryRegions = item.FindControl<ListView>( "LvCountryRegions" );
          foreach ( ListViewDataItem item2 in lvCountryRegions.Items ) {
            long countryRegionId = long.Parse( item2.FindControl<HiddenField>( "HdfId" ).Value );

            //Check to see if the country has been selected
            if ( item2.FindControl<CheckBox>( "ChkIsSupported" ).Checked ) {
              paymentMethod.AllowedInFollowingCountryRegions.Add( countryRegionId );
            }

            LoopPrices( item2.FindControl<ListView>( "LvCurrencies" ), countryId, countryRegionId );
          }
        }

        paymentMethod.PaymentProviderAlias = DrpPaymentProviders.SelectedValue;
        paymentMethod.AllowsRetrievalOfPaymentStatus = ChkAllowsGetStatus.Checked;
        paymentMethod.AllowsCapturingOfPayment = ChkAllowsCapturePayment.Checked;
        paymentMethod.AllowsRefundOfPayment = ChkAllowsRefundPayment.Checked;
        paymentMethod.AllowsCancellationOfPayment = ChkAllowsCancelPayment.Checked;

        //Reset settings
        var tempSettings = GetTempSettings( null ).ToList();
        ViewState[ "PaymentMethodSettings" ] = null;

        foreach ( Language language in umbracoLanguages ) {
          tempSettings.AddRange( GetTempSettings( language.id ).ToList() );
          ViewState[ "PaymentMethodSettings" + language.id ] = null;
        }

        paymentMethod.Settings = tempSettings.Select( tempSetting => new PaymentMethodSetting( tempSetting.Key, tempSetting.Value, tempSetting.UmbracoLanguageId ) { Id = tempSetting.Id } ).ToList();

        //Save the paymentmethod
        paymentMethod.Save();

        LoadCurrenciesAndCountries();

        //Reload settings
        LoadPaymentMethodSettings();

        //Show confirming speech bubble in umbraco
        ClientTools.ShowSpeechBubble( SpeechBubbleIcon.Save, CommonTerms.PaymentMethodSaved, string.Empty );
      }
    }

    private void LoopPrices( ListView lvCurrencies, long? countryId = null, long? countryRegionId = null ) {
      foreach ( ListViewDataItem item in lvCurrencies.Items ) {
        long currencyId = long.Parse( ( item.FindControl<HiddenField>( "HdfId" ) ).Value );
        long? servicePriceId = item.FindControl<HiddenField>( "HdfPriceId" ).Value.TryParse<long>();
        decimal? price = ( item.FindControl<TextBox>( "TxtPrice" ) ).Text.ParseToDecimal();

        if ( servicePriceId != null ) {
          if ( price != null ) {
            ServicePrice servicePrice = paymentMethod.OriginalPrices.SingleOrDefault( p => p.Id == servicePriceId.Value );
            servicePrice.Value = price.Value;
          } else {
            paymentMethod.OriginalPrices.RemoveAll( p => p.Id == servicePriceId.Value );
          }
        } else {
          if ( price != null ) {
            paymentMethod.OriginalPrices.Add( countryId == null ? new ServicePrice( currencyId, price.Value ) : new ServicePrice( currencyId, price.Value, countryId.Value, countryRegionId ) );
          }
        }

      }
    }

    private List<PaymentMethodTempSetting> GetTempSettings( int? languageId ) {
      string strLanguageId = languageId != null ? languageId.Value.ToString() : string.Empty;
      List<PaymentMethodTempSetting> settings = new List<PaymentMethodTempSetting>();

      ListView lvSettings = CurrentTabView.FindControl<ListView>( "LvSettings" + strLanguageId );
      foreach ( ListViewDataItem dataItem in lvSettings.Items ) {
        HiddenField hdfId = dataItem.FindControl<HiddenField>( "HdfId" );
        TextBox txtKey = dataItem.FindControl<TextBox>( "TxtKey" );
        TextBox txtValue = dataItem.FindControl<TextBox>( "TxtValue" );

        settings.Add( new PaymentMethodTempSetting() { Id = long.Parse( hdfId.Value ), UmbracoLanguageId = languageId, Key = txtKey.Text, Value = txtValue.Text } );
      }

      return settings;
    }

    protected void DrpPaymentProviders_SelectedIndexChanged( object sender, EventArgs e ) {
      LoadPaymentProviderAPIAllows( true );
    }

    private void LoadPaymentProviderAPIAllows( bool setTrue ) {
      IPaymentProvider paymentProvider = PaymentProviderService.Instance.Get( DrpPaymentProviders.SelectedValue );

      if ( paymentProvider != null ) {
        HypDocumentation.Visible = !string.IsNullOrEmpty( paymentProvider.DocumentationLink );
        HypDocumentation.NavigateUrl = paymentProvider.DocumentationLink;

        ChkAllowsGetStatus.Checked = paymentProvider.SupportsRetrievalOfPaymentStatus ? ( setTrue ? setTrue : paymentMethod.AllowsRetrievalOfPaymentStatus ) : false;
        ChkAllowsGetStatus.Visible = paymentProvider.SupportsRetrievalOfPaymentStatus;

        ChkAllowsCapturePayment.Checked = paymentProvider.SupportsCapturingOfPayment ? ( setTrue ? setTrue : paymentMethod.AllowsCapturingOfPayment ) : false;
        ChkAllowsCapturePayment.Visible = paymentProvider.SupportsCapturingOfPayment;

        ChkAllowsRefundPayment.Checked = paymentProvider.SupportsRefundOfPayment ? ( setTrue ? setTrue : paymentMethod.AllowsRefundOfPayment ) : false;
        ChkAllowsRefundPayment.Visible = paymentProvider.SupportsRefundOfPayment;

        ChkAllowsCancelPayment.Checked = paymentProvider.SupportsCancellationOfPayment ? ( setTrue ? setTrue : paymentMethod.AllowsCancellationOfPayment ) : false;
        ChkAllowsCancelPayment.Visible = paymentProvider.SupportsCancellationOfPayment;

        PnPaymentProviderAPICalls.Visible = paymentProvider.SupportsRetrievalOfPaymentStatus || paymentProvider.SupportsCapturingOfPayment || paymentProvider.SupportsRefundOfPayment || paymentProvider.SupportsCancellationOfPayment;
      }
    }

    #region Payment method settings

    #region Settings list

    private Pane AddSettingList( int? languageId ) {
      string strLanguageId = languageId != null ? languageId.Value.ToString() : string.Empty;

      Pane pane = new Pane();
      if ( languageId == null ) {
        pane.Text = CommonTerms.DefaultSettings;
      }

      ListView lvSettings = new ListView();
      lvSettings.ID = "LvSettings" + strLanguageId;
      lvSettings.ItemDataBound += lvSettings_ItemDataBound;
      lvSettings.ItemEditing += lvSettings_ItemEditing;
      lvSettings.ItemDeleting += lvSettings_ItemDeleting;
      lvSettings.LayoutTemplate = new LvSettingsLayoutTemplate();
      lvSettings.ItemTemplate = new LvSettingsItemTemplate();
      lvSettings.ItemPlaceholderID = "itemPlaceHolder";
      pane.Controls.Add( lvSettings );

      Panel pnlAddSetting = new Panel();
      pnlAddSetting.Style[ "clear" ] = "both;";
      pnlAddSetting.Style[ "float" ] = "left;";
      pane.Controls.Add( pnlAddSetting );

      Button btnAddSetting = new Button();
      btnAddSetting.Click += btnAddSetting_Click;
      btnAddSetting.Text = PaymentProviderTerms.AddSetting;
      btnAddSetting.CommandArgument = strLanguageId;
      pnlAddSetting.Controls.Add( btnAddSetting );

      return pane;
    }

    protected void lvSettings_ItemDataBound( object sender, ListViewItemEventArgs e ) {
      PaymentMethodTempSetting setting = (PaymentMethodTempSetting)( e.Item as ListViewDataItem ).DataItem;

      HiddenField hdfId = e.Item.FindControl<HiddenField>( "HdfId" );
      Label lblKey = e.Item.FindControl<Label>( "LblKey" );
      TextBox txtKey = e.Item.FindControl<TextBox>( "TxtKey" );
      Label lblValue = e.Item.FindControl<Label>( "LblValue" );
      TextBox txtValue = e.Item.FindControl<TextBox>( "TxtValue" );

      hdfId.Value = setting.Id.ToString();
      lblKey.Text = SelectedPaymentProviderInstance.GetLocalizedSettingsKey( setting.Key, CommonTerms.Culture );
      txtKey.Text = setting.Key;
      if ( string.IsNullOrEmpty( setting.Value ) || setting.Value.Length <= 35 ) {
        lblValue.Text = setting.Value;
      } else {
        lblValue.Text = setting.Value.Substring( 0, Math.Min( 35, setting.Value.Length ) ) + "...";
      }
      txtValue.Text = setting.Value;

      lblKey.Visible = lblValue.Visible = !string.IsNullOrEmpty( setting.Key ) || !string.IsNullOrEmpty( setting.Value );
      txtKey.Visible = txtValue.Visible = string.IsNullOrEmpty( setting.Key ) && string.IsNullOrEmpty( setting.Value );

      HiddenField hdfLanguageId = ( sender as ListView ).FindControl<HiddenField>( "HdfLanguageId" );
      hdfLanguageId.Value = setting.UmbracoLanguageId != null ? setting.UmbracoLanguageId.Value.ToString() : string.Empty;
    }

    protected void lvSettings_ItemEditing( object sender, ListViewEditEventArgs e ) {
      ListViewItem item = ( sender as ListView ).Items.Single( i => i.DataItemIndex == e.NewEditIndex );

      Label lblKey = item.FindControl<Label>( "LblKey" );
      TextBox txtKey = item.FindControl<TextBox>( "TxtKey" );
      Label lblValue = item.FindControl<Label>( "LblValue" );
      TextBox txtValue = item.FindControl<TextBox>( "TxtValue" );

      lblKey.Text = SelectedPaymentProviderInstance.GetLocalizedSettingsKey( txtKey.Text, CommonTerms.Culture );
      lblValue.Text = txtValue.Text;

      txtKey.Visible = txtValue.Visible = !txtKey.Visible;
      lblKey.Visible = lblValue.Visible = !lblKey.Visible;
    }

    protected void lvSettings_ItemDeleting( object sender, ListViewDeleteEventArgs e ) {
      ListView lvSettings = sender as ListView;
      HiddenField hdfLanguageId = lvSettings.FindControl<HiddenField>( "HdfLanguageId" );
      int? languageId = hdfLanguageId.Value.TryParse<int>();
      string strLaguageId = languageId != null && languageId.Value > 0 ? languageId.Value.ToString() : string.Empty;

      //Save all temp entries so user dont lose data
      SaveTempSettings();

      List<PaymentMethodTempSetting> paymentMethodSettings = ( ViewState[ "PaymentMethodSettings" + strLaguageId ] as List<PaymentMethodTempSetting> );
      paymentMethodSettings.RemoveAt( e.ItemIndex );

      lvSettings.DataSource = paymentMethodSettings;
      lvSettings.DataBind();
    }

    protected void btnAddSetting_Click( object sender, EventArgs e ) {
      string strLanguageId = ( sender as Button ).CommandArgument;

      int? languageId = strLanguageId.TryParse<int>();
      ListView lvSettings = CurrentTabView.FindControl<ListView>( "LvSettings" + strLanguageId );

      //Save all temp entries so user dont lose data
      SaveTempSettings();

      List<PaymentMethodTempSetting> paymentMethodSettings = ( ViewState[ ( "PaymentMethodSettings" + strLanguageId ) ] as List<PaymentMethodTempSetting> );
      paymentMethodSettings.Add( new PaymentMethodTempSetting() { UmbracoLanguageId = languageId } );

      lvSettings.DataSource = paymentMethodSettings;
      lvSettings.DataBind();
    }

    private void SaveTempSettings() {
      ViewState[ "PaymentMethodSettings" ] = GetTempSettings( null );

      foreach ( Language language in umbracoLanguages ) {
        ViewState[ "PaymentMethodSettings" + language.id ] = GetTempSettings( language.id );
      }
    }

    #endregion

    protected void BtnOverwriteSettings_Click( object sender, EventArgs e ) {
      OverwriteSettings();
      LoadPaymentMethodSettings();
    }

    private void OverwriteSettings() {
      foreach ( Language language in umbracoLanguages )
        ViewState[ "PaymentMethodSettings" + language.id ] = new List<PaymentMethodTempSetting>();
      ViewState[ "PaymentMethodSettings" ] = SelectedPaymentProviderInstance.DefaultSettings.Select( p => new PaymentMethodTempSetting() { Key = p.Key, Value = p.Value } ).ToList();
    }

    private void LoadPaymentMethodSettings() {
      List<PaymentMethodTempSetting> paymentMethodTempSettings = ViewState[ "PaymentMethodSettings" ] as List<PaymentMethodTempSetting>;
      if ( paymentMethodTempSettings == null ) {
        paymentMethodTempSettings = paymentMethod.Settings.Where( s => s.LanguageId == null ).Select( s => new PaymentMethodTempSetting() { Id = s.Id, Key = s.Key, Value = s.Value, UmbracoLanguageId = s.LanguageId } ).ToList();
        ViewState[ "PaymentMethodSettings" ] = paymentMethodTempSettings;
      }

      ListView lvCommonSettings = CurrentTabView.FindControl<ListView>( "LvSettings" );
      lvCommonSettings.DataSource = paymentMethodTempSettings;
      lvCommonSettings.DataBind();

      foreach ( Language language in umbracoLanguages ) {
        List<PaymentMethodTempSetting> paymentMethodLanguageTempSettings = ViewState[ "PaymentMethodSettings" + language.id ] as List<PaymentMethodTempSetting>;
        if ( paymentMethodLanguageTempSettings == null ) {
          paymentMethodLanguageTempSettings = paymentMethod.Settings.Where( s => s.LanguageId == language.id ).Select( s => new PaymentMethodTempSetting() { Id = s.Id, Key = s.Key, Value = s.Value, UmbracoLanguageId = s.LanguageId } ).ToList();
          ViewState[ "PaymentMethodSettings" + language.id ] = paymentMethodLanguageTempSettings;
        }

        ListView lvSettings = CurrentTabView.FindControl<ListView>( "LvSettings" + language.id );
        lvSettings.DataSource = paymentMethodLanguageTempSettings;
        lvSettings.DataBind();
      }
    }

    private void LoadPaymentProviders() {
      DrpPaymentProviders.Items.Clear();

      foreach ( var paymentProviderAlias in PaymentProviderService.Instance.GetAllPaymentProviderAliases() ) {
        DrpPaymentProviders.Items.Add( new ListItem( paymentProviderAlias, paymentProviderAlias ) );
      }
    }

    #endregion

    [Serializable]
    public class PaymentMethodTempSetting {
      public long Id { get; set; }
      public long? UmbracoLanguageId { get; set; }
      public string Key { get; set; }
      public string Value { get; set; }
    }

    #region Templates

    private class LvSettingsItemTemplate : ITemplate {
      #region ITemplate Members

      public void InstantiateIn( Control container ) {
        Panel pnlWrap = new Panel();
        pnlWrap.CssClass = "umb-el-wrap";

        HiddenField hdfId = new HiddenField();
        hdfId.ID = "HdfId";
        pnlWrap.Controls.Add( hdfId );

        //Panel pnlPropertyItemheader = new Panel();
        ////pnlPropertyItemheader.CssClass = "propertyItemheader";
        //pnlWrap.Controls.Add( pnlPropertyItemheader );

        Label lblKey = new Label();
        lblKey.ID = "LblKey";
        lblKey.CssClass = "control-label";
        lblKey.Attributes.CssStyle.Add("word-break", "break-word");
        lblKey.AssociatedControlID = "TxtValue";
        pnlWrap.Controls.Add( lblKey );

        TextBox txtKey = new TextBox();
        txtKey.ID = "TxtKey";
        txtKey.CssClass = "control-label";
        pnlWrap.Controls.Add( txtKey );

        Panel pnlPropertyItemContent = new Panel();
        pnlPropertyItemContent.CssClass = "controls controls-row";
        pnlWrap.Controls.Add( pnlPropertyItemContent );

        Label lblValue = new Label();
        lblValue.ID = "LblValue";
        pnlPropertyItemContent.Controls.Add( lblValue );

        TextBox txtValue = new TextBox();
        txtValue.ID = "TxtValue";
        txtValue.CssClass = "guiInputText guiInputStandardSize";
        pnlPropertyItemContent.Controls.Add( txtValue );

        ImageButton btnDelete = new ImageButton();
        btnDelete.ID = "BtnDelete";
        btnDelete.CommandName = "Delete";
        btnDelete.ToolTip = CommonTerms.Delete;
        btnDelete.ImageUrl = WebUtils.GetWebResourceUrl( Constants.EditorIcons.Delete );
        btnDelete.CssClass = "button";
        btnDelete.Style[ "float" ] = "right;";
        pnlPropertyItemContent.Controls.Add( btnDelete );

        ImageButton btnEdit = new ImageButton();
        btnEdit.ID = "BtnEdit";
        btnEdit.CommandName = "Edit";
        btnEdit.ToolTip = CommonTerms.Edit;
        btnEdit.ImageUrl = WebUtils.GetWebResourceUrl( Constants.EditorIcons.Edit );
        btnEdit.CssClass = "button";
        btnEdit.Style[ "float" ] = "right;";
        pnlPropertyItemContent.Controls.Add( btnEdit );


        container.Controls.Add( pnlWrap );
      }

      #endregion
    }

    private class LvSettingsLayoutTemplate : ITemplate {
      #region ITemplate Members

      public void InstantiateIn( Control container ) {
        HiddenField hdfLanguageId = new HiddenField();
        hdfLanguageId.ID = "HdfLanguageId";
        container.Controls.Add( hdfLanguageId );

        PropertyPanel ppnlSettingHeader = new PropertyPanel();
        ppnlSettingHeader.ID = "PPNLSettingHeader";
        ppnlSettingHeader.Text = PaymentProviderTerms.Key;
        container.Controls.Add( ppnlSettingHeader );

        Label lblValueHeader = new Label();
        lblValueHeader.ID = "LblValueHeader";
        lblValueHeader.Text = PaymentProviderTerms.Value;
        lblValueHeader.Font.Bold = true;
        ppnlSettingHeader.Controls.Add( lblValueHeader );

        PlaceHolder itemPlaceHolder = new PlaceHolder() { ID = "itemPlaceHolder" };
        container.Controls.Add( itemPlaceHolder );
      }

      #endregion
    }

    #endregion

  }
}