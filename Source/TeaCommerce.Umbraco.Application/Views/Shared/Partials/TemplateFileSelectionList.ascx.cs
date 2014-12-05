using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeaCommerce.Api.Infrastructure.Templating;

namespace TeaCommerce.Umbraco.Application.Views.Shared.Partials {
  public partial class TemplateFileSelectionList : UserControl {

    public string SelectedValue { get { return chkListXSLTFiles.SelectedValue; } set { chkListXSLTFiles.SelectedValue = value; } }
    public ListItemCollection Items { get { return chkListXSLTFiles.Items; } }

    private CheckBoxList chkListXSLTFiles;

    protected override void OnInit( EventArgs e ) {
      chkListXSLTFiles = new CheckBoxList();
      chkListXSLTFiles.AppendDataBoundItems = true;

      this.Controls.Add( chkListXSLTFiles );

      base.OnInit( e );
    }

    public void LoadTemplateFiles() {
      chkListXSLTFiles.Items.Clear();
      chkListXSLTFiles.Items.AddRange( TemplatingService.Instance.GetTemplateFiles().Select( i => new ListItem( i ) ).ToArray() );
    }

  }
}