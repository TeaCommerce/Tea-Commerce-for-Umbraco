using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Infrastructure.Templating;

namespace TeaCommerce.Umbraco.Application.Views.Shared.Partials {
  public partial class TemplateFileSelector : UserControl {

    public string SelectedValue { get { return drpTemplateFiles.SelectedValue; } set { drpTemplateFiles.SelectedValue = value; } }
    public ListItemCollection Items { get { return drpTemplateFiles.Items; } }
    public bool InsertEmptyItem { get; set; }

    private DropDownList drpTemplateFiles;

    protected override void OnInit( EventArgs e ) {
      drpTemplateFiles = new DropDownList();
      drpTemplateFiles.AppendDataBoundItems = true;
      drpTemplateFiles.CssClass = "guiInputText guiInputStandardSize";

      this.Controls.Add( drpTemplateFiles );

      base.OnInit( e );
    }

    public void LoadTemplateFiles() {
      drpTemplateFiles.Items.Clear();
      if ( InsertEmptyItem )
        drpTemplateFiles.Items.Add( new ListItem( "----", string.Empty ) );
      drpTemplateFiles.Items.AddRange( TemplatingService.Instance.GetTemplateFiles().Select( i => new ListItem( i ) ).ToArray() );
    }

  }
}