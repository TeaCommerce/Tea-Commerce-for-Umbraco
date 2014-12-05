using System.Web.UI;

namespace TeaCommerce.Umbraco.Application.Utils {
  public class WebUtils {

    private static Page cachedPage;
    private static Page CachedPage {
      get {
        if ( cachedPage == null )
          cachedPage = new Page();
        return cachedPage;
      }
    }

    public static string GetWebResourceUrl( string resource ) {
      return CachedPage.ClientScript.GetWebResourceUrl( typeof( Constants ), resource );
    }

    public static string GetPageUrl( string url, bool umbracoContext = true ) {
      //For some reason '/' is not respected in the umbraco menu. This is a hack to ensure everything works for now. 
      if ( umbracoContext ) {
        return "../App_Plugins/TeaCommerce" + url;
      }

      return "../.." + url;
    }

  }
}