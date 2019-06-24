using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using System.Web.WebPages;
using System.Xml.XPath;
using System.Xml.Xsl;
using TeaCommerce.Api.Infrastructure.Templating;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Serialization;
using TeaCommerce.Umbraco.Configuration.Services;
using Umbraco.Core.IO;
using umbraco;
using umbraco.cms.businesslogic.language;
using umbraco.cms.businesslogic.macro;
using umbraco.MacroEngines;
using umbraco.NodeFactory;
using Umbraco.Web;

namespace TeaCommerce.Umbraco.Configuration.Infrastructure.Templating {
  public class TemplateRenderer : ITemplateRenderer {

    private readonly ITemplateFileLocator _templateFileLocator;

    public TemplateRenderer( ITemplateFileLocator templateFileLocator ) {
      _templateFileLocator = templateFileLocator;
    }

    public string RenderTemplateFile<T>( string templateFile, T model, long? languageId ) {
      return RenderTemplateFile( templateFile, null, model, languageId );
    }

    public string RenderTemplateFile( string templateFile, object pageId ) {
      return RenderTemplateFile<object>( templateFile, int.Parse( pageId.ToString() ), null, null );
    }

    private string RenderTemplateFile<T>( string templateFile, int? pageId, T model, long? languageId ) {
      string rtnStr = "";

      CultureInfo currentUiCulture = (CultureInfo)Thread.CurrentThread.CurrentUICulture.Clone();

      //If not language id is specified - try and get it from the current domain from Umbraco
      if ( languageId == null && pageId != null ) {
        languageId = LanguageService.Instance.GetLanguageIdByNodePath( new Node( pageId.Value ).Path );
      }

      string cultureName = null;

      //Try to change culture for this rendering process
      if ( languageId != null ) {
        Language language = Language.GetAllAsList().SingleOrDefault( l => l.id == languageId.Value );
        if ( language != null ) {
          cultureName = language.CultureAlias;
        }
      }

      if ( !string.IsNullOrEmpty( cultureName ) ) {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo( cultureName );
      }

      //Ensure that we have a path that points to the root of the website
      templateFile = _templateFileLocator.TranslateTemplateFileLocation( templateFile );

      if ( !string.IsNullOrEmpty( templateFile ) && File.Exists( HostingEnvironment.MapPath( templateFile ) ) && ( templateFile.EndsWith( ".cshtml" ) || templateFile.EndsWith( ".vbhtml" ) ) ) {
        if (templateFile.Replace("\\", "/").StartsWith(SystemDirectories.MvcViews + "/Partials"))
        {
           if(model != null)
           {
              rtnStr = ViewRenderer.RenderPartialView(templateFile, model);
           }
           else
           {
              //If we don't have a model passed in, we pass the current page as model, as this is what happens normally in Umbraco
              var content = UmbracoContext.Current.ContentCache.GetById(int.Parse(pageId.ToString()));
              rtnStr = ViewRenderer.RenderPartialView(templateFile, content);
           }
         }
         else
         {
             rtnStr = RenderRazorFile(templateFile, pageId, model);
         }
      }

      Thread.CurrentThread.CurrentUICulture = currentUiCulture;

      return rtnStr;
    }

    private string RenderRazorFile<T>( string templateFile, int? pageId, T model ) {
      string rtnStr;

      WebPageBase razorWebPage = WebPageBase.CreateInstanceFromVirtualPath( templateFile );
      razorWebPage.Context = new HttpContextWrapper( HttpContext.Current );

      if ( razorWebPage is IMacroContext ) {
        if ( razorWebPage is TemplateContext<T> && model != null ) {
          ( (TemplateContext<T>)razorWebPage ).SetMembers( model );
        } else if ( pageId != null ) {
          ( (IMacroContext)razorWebPage ).SetMembers( new MacroModel(), new Node( pageId.Value ) );
        }
      }

      using ( StringWriter output = new StringWriter() ) {
        razorWebPage.ExecutePageHierarchy( new WebPageContext( razorWebPage.Context, razorWebPage, null ), output );
        rtnStr = output.ToString();
      }

      return rtnStr;
    }

  }
}
