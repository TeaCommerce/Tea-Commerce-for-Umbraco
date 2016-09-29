using System;
using System.Reflection;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Infrastructure.Installation;
using TeaCommerce.Umbraco.Configuration.Services;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;

namespace TeaCommerce.Umbraco.Configuration {
  public class ApplicationStartup : ApplicationEventHandler {

    protected override void ApplicationStarted( UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext ) {
      try {
        DependencyContainer.Configure( 
          Assembly.Load( "TeaCommerce.Umbraco.Configuration" ),
          Assembly.Load( "TeaCommerce.Umbraco.Install" ) );
      } catch ( Exception exp ) {
        LogHelper.Error( GetType(), "Error loading Autofac modules", exp );
      }

      //Run install/update on each application start up to support UaaS and Nuget
      InstallationService.Instance.InstallOrUpdate();

      Domain.New += Domain_New;
      Domain.AfterSave += Domain_AfterSave;
      Domain.AfterDelete += Domain_AfterDelete;

      ContentService.Published += ContentService_Published;
    }

    private void ContentService_Published( global::Umbraco.Core.Publishing.IPublishingStrategy sender, global::Umbraco.Core.Events.PublishEventArgs<global::Umbraco.Core.Models.IContent> e ) {
      CacheService.Instance.InvalidateApplicationCache();
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