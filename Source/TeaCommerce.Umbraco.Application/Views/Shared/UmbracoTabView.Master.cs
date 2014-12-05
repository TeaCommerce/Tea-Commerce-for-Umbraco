using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.uicontrols;

namespace TeaCommerce.Umbraco.Application.Views.Shared {
  public partial class UmbracoTabView : MasterPage {

    public TabView CurrentTabView { get { return TabViewControl; } }
    
  }
}