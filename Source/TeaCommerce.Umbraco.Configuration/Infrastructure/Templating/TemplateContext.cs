using umbraco.cms.businesslogic.macro;
using umbraco.interfaces;
using umbraco.MacroEngines;

namespace TeaCommerce.Umbraco.Configuration.Infrastructure.Templating {
  public abstract class TemplateContext<T> : BaseContext<T> {

    public new T Model { get { return CurrentModel; } }

    public void SetMembers( T model ) {
      CurrentModel = model;
      ParameterDictionary = new UmbracoParameterDictionary( null );
#pragma warning disable 612,618
      CultureDictionary = new UmbracoCultureDictionary();
#pragma warning restore 612,618
    }

    public override void SetMembers( MacroModel macro, INode node ) {

    }

  }
}