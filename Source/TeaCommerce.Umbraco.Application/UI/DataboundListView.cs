using System.Web.UI.WebControls;

namespace TeaCommerce.Umbraco.Application.UI {
  public class DataboundListView : ListView {

    private bool _isLayoutTemplateDataBound;

    protected override void CreateLayoutTemplate() {
      base.CreateLayoutTemplate();

      // Only need to databind the layout template if it contains some controls and it has not already been databound (fixes an issue which caused any controls within the layout template to be databound an infinate number of times)
      if ( Controls.Count == 1 && !_isLayoutTemplateDataBound ) {
        // Databind the layout template
        Controls[ 0 ].DataBind();


        // Save the result so we don't databind again
        _isLayoutTemplateDataBound = true;
      }
    }

  }
}