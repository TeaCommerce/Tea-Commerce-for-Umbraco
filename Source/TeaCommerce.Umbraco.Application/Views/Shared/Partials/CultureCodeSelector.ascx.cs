using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TeaCommerce.Umbraco.Application.Views.Shared.Partials {
  public partial class CultureCodeSelector : UserControl {

    public string SelectedValue { get { return drpCultureCodes.SelectedValue; } set { drpCultureCodes.SelectedValue = value; } }
    public ListItemCollection Items { get { return drpCultureCodes.Items; } }
    public string CssClass { get; set; }
    public Unit Width { get; set; }
    public bool InsertEmptyItem { get; set; }
    public event EventHandler SelectedIndexChanged;

    private DropDownList drpCultureCodes;

    protected override void OnInit( EventArgs e ) {
      drpCultureCodes = new DropDownList();
      drpCultureCodes.AppendDataBoundItems = true;
      drpCultureCodes.DataTextField = "Name";
      drpCultureCodes.DataValueField = "Id";
      drpCultureCodes.CssClass = CssClass;
      drpCultureCodes.Width = Width == Unit.Empty ? Unit.Empty : Width;
      drpCultureCodes.SelectedIndexChanged += drpCultureCodes_SelectedIndexChanged;

      this.Controls.Add( drpCultureCodes );

      base.OnInit( e );
    }

    protected override void OnLoad( EventArgs e ) {
      drpCultureCodes.AutoPostBack = ( SelectedIndexChanged != null );

      base.OnLoad( e );
    }

    protected void drpCultureCodes_SelectedIndexChanged( object sender, EventArgs e ) {
      if ( SelectedIndexChanged != null )
        SelectedIndexChanged( this, e );
    }

    public void LoadCultureCodes() {
      drpCultureCodes.Items.Clear();
      if ( InsertEmptyItem )
        drpCultureCodes.Items.Add( new ListItem( "----", string.Empty ) );

      var cultures = from c in CultureInfo.GetCultures( CultureTypes.SpecificCultures )
                     where !string.IsNullOrEmpty( c.TextInfo.CultureName )
                     orderby c.EnglishName
                     select new {
                       Name = c.EnglishName + " - " + c.TextInfo.CultureName,
                       Id = c.TextInfo.CultureName
                     };

      drpCultureCodes.DataSource = cultures;
      drpCultureCodes.DataBind();
    }

  }
}