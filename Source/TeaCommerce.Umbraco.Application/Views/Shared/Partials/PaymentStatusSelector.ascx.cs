using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Models;
using TeaCommerce.Umbraco.Application.Resources;

namespace TeaCommerce.Umbraco.Application.Views.Shared.Partials {
  public partial class PaymentStatusSelector : UserControl {

    public string SelectedValue { get { return drpPaymentStatuses.SelectedValue; } set { drpPaymentStatuses.SelectedValue = value; } }
    public ListItemCollection Items { get { return drpPaymentStatuses.Items; } }
    public bool InsertEmptyItem { get; set; }
    public event EventHandler SelectedIndexChanged;

    private DropDownList drpPaymentStatuses;

    protected override void OnInit( EventArgs e ) {
      base.OnInit( e );
      
      drpPaymentStatuses = new DropDownList();
      drpPaymentStatuses.CssClass = "guiInputText guiInputStandardSize";
      drpPaymentStatuses.SelectedIndexChanged += drpPaymentStatuses_SelectedIndexChanged;

      this.Controls.Add( drpPaymentStatuses );
    }

    protected override void OnLoad( EventArgs e ) {
      base.OnLoad( e );

      drpPaymentStatuses.AutoPostBack = ( SelectedIndexChanged != null );
    }

    protected void drpPaymentStatuses_SelectedIndexChanged( object sender, EventArgs e ) {
      if ( SelectedIndexChanged != null )
        SelectedIndexChanged( this, e );
    }

    public void LoadPaymentStatuses() {
      drpPaymentStatuses.Items.Clear();
      if ( InsertEmptyItem )
        drpPaymentStatuses.Items.Add( new ListItem( "----", string.Empty ) );

      drpPaymentStatuses.Items.Add( new ListItem( CommonTerms.Initialized, PaymentState.Initialized.ToString() ) );
      drpPaymentStatuses.Items.Add( new ListItem( CommonTerms.Authorized, PaymentState.Authorized.ToString() ) );
      drpPaymentStatuses.Items.Add( new ListItem( CommonTerms.Cancelled, PaymentState.Cancelled.ToString() ) );
      drpPaymentStatuses.Items.Add( new ListItem( CommonTerms.Captured, PaymentState.Captured.ToString() ) );
      drpPaymentStatuses.Items.Add( new ListItem( CommonTerms.Refunded, PaymentState.Refunded.ToString() ) );
    }

  }
}