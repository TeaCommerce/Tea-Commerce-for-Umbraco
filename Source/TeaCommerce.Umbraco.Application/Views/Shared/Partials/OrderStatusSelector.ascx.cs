using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Services;

namespace TeaCommerce.Umbraco.Application.Views.Shared.Partials {
  public partial class OrderStatusSelector : UserControl {

    public string SelectedValue { get { return drpOrderStatuses.SelectedValue; } set { drpOrderStatuses.SelectedValue = value; } }
    public string CssClass { get { return drpOrderStatuses.CssClass; } set { drpOrderStatuses.CssClass = value; } }
    public ListItemCollection Items { get { return drpOrderStatuses.Items; } }
    public bool InsertEmptyItem { get; set; }
    public event EventHandler SelectedIndexChanged;

    private DropDownList drpOrderStatuses;

    protected override void OnInit( EventArgs e ) {
      drpOrderStatuses = new DropDownList();
      drpOrderStatuses.AppendDataBoundItems = true;
      drpOrderStatuses.DataTextField = "Name";
      drpOrderStatuses.DataValueField = "Id";
      drpOrderStatuses.CssClass = "guiInputText guiInputStandardSize";
      drpOrderStatuses.SelectedIndexChanged += drpOrderStatuses_SelectedIndexChanged;

      Controls.Add( drpOrderStatuses );

      base.OnInit( e );
    }

    protected override void OnLoad( EventArgs e ) {
      drpOrderStatuses.AutoPostBack = ( SelectedIndexChanged != null );

      base.OnLoad( e );
    }

    protected void drpOrderStatuses_SelectedIndexChanged( object sender, EventArgs e ) {
      if ( SelectedIndexChanged != null )
        SelectedIndexChanged( this, e );
    }

    public void LoadOrderStatuses( long storeId ) {
      drpOrderStatuses.Items.Clear();
      if ( InsertEmptyItem )
        drpOrderStatuses.Items.Add( new ListItem( "----", string.Empty ) );

      drpOrderStatuses.DataSource = OrderStatusService.Instance.GetAll( storeId );
      drpOrderStatuses.DataBind();
    }

  }
}