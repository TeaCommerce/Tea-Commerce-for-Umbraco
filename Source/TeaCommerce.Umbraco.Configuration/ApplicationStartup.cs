using System;
using System.Reflection;
using TeaCommerce.Api.Dependency;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using Umbraco.Core;
using Umbraco.Core.Logging;

namespace TeaCommerce.Umbraco.Configuration {
  public class ApplicationStartup : ApplicationEventHandler {

    protected override void ApplicationStarted( UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext ) {
      try {
        DependencyContainer.Configure( Assembly.Load( "TeaCommerce.Umbraco.Configuration" ) );
      } catch ( Exception exp ) {
        LogHelper.Error( GetType(), "Error loading Autofac modules", exp );
      }

      Domain.New += Domain_New;
      Domain.AfterSave += Domain_AfterSave;
      Domain.AfterDelete += Domain_AfterDelete;
    }

    void Domain_New( Domain sender, NewEventArgs e ) {
      Compatibility.Domain.InvalidateCache();
    }

    void Domain_AfterSave( Domain sender, SaveEventArgs e ) {
      Compatibility.Domain.InvalidateCache();
    }

    void Domain_AfterDelete( Domain sender, DeleteEventArgs e ) {
      Compatibility.Domain.InvalidateCache();
    }
  }
}