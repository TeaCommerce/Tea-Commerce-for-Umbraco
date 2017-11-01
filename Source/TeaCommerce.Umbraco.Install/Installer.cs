using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using TeaCommerce.Api.Infrastructure.Installation;
using TeaCommerce.Api.Infrastructure.Logging;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Persistence;
using TeaCommerce.Api.Persistence.Installation;
using TeaCommerce.Api.Services;
using TeaCommerce.Umbraco.Install.InstallTasks;

namespace TeaCommerce.Umbraco.Install {
  public class Installer : IInstaller {

    private readonly IDatabaseFactory _databaseFactory;
    private readonly PersistenceInstaller _persistenceInstaller;
    private readonly IStoreService _storeService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly IOrderService _orderService;

    private readonly IList<IInstallTask> _installTasks;

    public Installer( IDatabaseFactory databaseFactory, IStoreService storeService, IPaymentMethodService paymentMethodService, IOrderService orderService ) {
      _databaseFactory = databaseFactory;
      _storeService = storeService;
      _paymentMethodService = paymentMethodService;
      _orderService = orderService;
      _persistenceInstaller = new PersistenceInstaller( databaseFactory );

      _installTasks = new List<IInstallTask>();

      //Sections
      _installTasks.Add( new SectionInstallTask( "Tea Commerce", "teacommerce", "icon-shopping-basket-alt-2" ) );

      //Trees
      _installTasks.Add( new ApplicationTreeInstallTask( "tea-commerce-store-tree", "Stores", 0, "TeaCommerce.Umbraco.Application.Trees.StoreTree,TeaCommerce.Umbraco.Application" ) );
      _installTasks.Add( new ApplicationTreeInstallTask( "tea-commerce-security-tree", "Security", 1, "TeaCommerce.Umbraco.Application.Trees.SecurityTree,TeaCommerce.Umbraco.Application" ) );
      _installTasks.Add( new ApplicationTreeInstallTask( "tea-commerce-licenses-tree", "Licenses", 2, "TeaCommerce.Umbraco.Application.Trees.LicenseTree,TeaCommerce.Umbraco.Application" ) );
      _installTasks.Add( new ApplicationTreeInstallTask( "tea-commerce-need-help-tree", "Need help?", 3, "TeaCommerce.Umbraco.Application.Trees.NeedHelpTree,TeaCommerce.Umbraco.Application" ) );

      //Grant permissions
      _installTasks.Add( new GrantPermissionsInstallTask() );

      //Misc files
      _installTasks.Add( new MoveFileInstallTask( "~/macroScripts/tea-commerce/email-template-confirmation.cshtml.default", "~/macroScripts/tea-commerce/email-template-confirmation.cshtml" ) { OverwriteFile = false } );
      _installTasks.Add( new MoveFileInstallTask( "~/macroScripts/tea-commerce/email-template-payment-inconsistency.cshtml.default", "~/macroScripts/tea-commerce/email-template-payment-inconsistency.cshtml" ) { OverwriteFile = false } );
      _installTasks.Add( new MoveFileInstallTask( "~/macroScripts/tea-commerce/edit-order.cshtml.default", "~/macroScripts/tea-commerce/edit-order.cshtml" ) { OverwriteFile = false } );

      //Data type definitions
      _installTasks.Add( new DataTypeDefinitionInstallTask( "Tea Commerce: Store picker", "TeaCommerce.StorePicker" ) );
      _installTasks.Add( new DataTypeDefinitionInstallTask( "Tea Commerce: VAT group picker", "TeaCommerce.VatGroupPicker" ) );
      _installTasks.Add( new DataTypeDefinitionInstallTask( "Tea Commerce: Stock management", "TeaCommerce.StockManagement" ) );
      _installTasks.Add( new DataTypeDefinitionInstallTask( "Tea Commerce: Variant Editor", "TeaCommerce.VariantEditor" ) );

    }

    public void InstallOrUpdate() {
      _persistenceInstaller.InstallOrUpdate();

      Database database = _databaseFactory.Get();

      int currentVersion = database.ExecuteScalar<int>( "SELECT SpecialActionsVersion FROM TeaCommerce_Version" );
      int newVersion = 5;

      while ( currentVersion < newVersion ) {
        try {

          #region Initial install

          if ( currentVersion == 0 ) {
            foreach ( IInstallTask installTask in _installTasks ) {
              installTask.Install();
            }
          }

          #endregion

          #region 2.1.0

          if ( currentVersion + 1 == 1 ) {
            #region Auto set current cart and order number for all stores
            foreach ( Store store in _storeService.GetAll() ) {
              store.CurrentCartNumber = database.ExecuteScalar<long>( "SELECT COUNT(Id) FROM TeaCommerce_Order WHERE StoreId=@0", store.Id );
              store.CurrentOrderNumber = database.ExecuteScalar<long>( "SELECT COUNT(Id) FROM TeaCommerce_Order WHERE StoreId=@0 AND DateFinalized IS NOT NULL", store.Id );
              store.Save();
            }
            #endregion
          }

          #endregion

          #region 2.1.1

          if ( currentVersion + 1 == 2 ) {
            #region Correct wrong order properties for sage pay orders
            foreach ( Store store in _storeService.GetAll() ) {
              List<long> sagePayPaymentMethodIds = _paymentMethodService.GetAll( store.Id ).Where( p => p.PaymentProviderAlias == "SagePay" ).Select( p => p.Id ).ToList();

              if ( sagePayPaymentMethodIds.Any() ) {
                IEnumerable<Order> orders = _orderService.Get( store.Id, database.Fetch<Guid>( "SELECT Id FROM TeaCommerce_Order WHERE StoreId=@0 AND PaymentMethodId IN (" + string.Join( ",", sagePayPaymentMethodIds ) + ")", store.Id ) );

                foreach ( Order order in orders ) {
                  CustomProperty vendorTxCode = order.Properties.Get( "VendorTxCode" );
                  CustomProperty txAuthNo = order.Properties.Get( "TxAuthNo" );

                  if ( vendorTxCode == null && txAuthNo == null ) continue;

                  if ( vendorTxCode != null ) {
                    vendorTxCode.Alias = "vendorTxCode";
                  }

                  if ( txAuthNo != null ) {
                    txAuthNo.Alias = "txAuthNo";
                  }

                  order.Save();
                }
              }
            }
            #endregion
          }

          #endregion

          #region 2.2

          if ( currentVersion + 1 == 3 ) {
            //Had to remove the deletion of TeaCommerce.PaymentProviders.dll and TeaCommerce.PaymentProviders.XmlSerializers.dll
          }

          #endregion

          #region 2.3.2

          if ( currentVersion + 1 == 3 ) {
            #region Remove order xml cache because properties was wrongly serialized
            string teaCommerceAppDataPath = HostingEnvironment.MapPath( "~/App_Data/tea-commerce" );
            if ( teaCommerceAppDataPath != null ) {
              DirectoryInfo teaCommerceAppDataFolder = new DirectoryInfo( teaCommerceAppDataPath );
              if ( teaCommerceAppDataFolder.Exists ) {
                foreach ( FileInfo orderXmlCache in teaCommerceAppDataFolder.GetFiles( "finalized-orders-xml-cache*" ) ) {
                  orderXmlCache.Delete();
                }
              }
            }
            #endregion
          }

          #endregion

          #region 3.0.0

          if ( currentVersion + 1 == 4 ) {
            #region Remove old javascript API file
            string javaScriptApiFile = HostingEnvironment.MapPath( "~/scripts/tea-commerce.min.js" );
            if ( javaScriptApiFile != null && File.Exists( javaScriptApiFile ) ) {
              File.Delete( javaScriptApiFile );
            }
            #endregion

            #region Remove old Tea Commerce folder in Umbraco plugins
            string oldTeaCommercePluginPath = HostingEnvironment.MapPath( "~/umbraco/plugins/tea-commerce" );
            if ( oldTeaCommercePluginPath != null && Directory.Exists( oldTeaCommercePluginPath ) ) {
              Directory.Delete( oldTeaCommercePluginPath, true );
            }
            oldTeaCommercePluginPath = HostingEnvironment.MapPath( "~/App_Plugins/tea-commerce" );
            if ( oldTeaCommercePluginPath != null && Directory.Exists( oldTeaCommercePluginPath ) ) {
              Directory.Delete( oldTeaCommercePluginPath, true );
            }
            #endregion

            #region Create default gift card settings

            foreach ( Store store in _storeService.GetAll() ) {
              store.GiftCardSettings.Length = 10;
              store.GiftCardSettings.DaysValid = 1095;
              store.Save();
            }

            #endregion
          }

          #endregion

          currentVersion++;
          database.Execute( "UPDATE TeaCommerce_Version SET SpecialActionsVersion=@0", currentVersion );
        } catch ( Exception exp ) {
          LoggingService.Instance.Error<Installer>( "Tea Commerce installation failed", exp );
          break;
        }
      }

      //Language files
      new LanguageFileInstallTask( "TeaCommerce.Umbraco.Install.Content.Resources.da.xml", "~/umbraco/config/lang/da.xml" ).Install();
      new LanguageFileInstallTask( "TeaCommerce.Umbraco.Install.Content.Resources.en.xml", "~/umbraco/config/lang/en.xml" ).Install();
      new LanguageFileInstallTask( "TeaCommerce.Umbraco.Install.Content.Resources.en_us.xml", "~/umbraco/config/lang/en_us.xml" ).Install();
      new LanguageFileInstallTask( "TeaCommerce.Umbraco.Install.Content.Resources.se.xml", "~/umbraco/config/lang/se.xml" ).Install();
      new UIFileInstallTask( "TeaCommerce.Umbraco.Install.Content.XML.UI.xml", "~/umbraco/config/create/UI.xml" ).Install();

      //Fix package file
      new RemoveOldPackageInstallTask().Install();

    }

    public void Uninstall() {
      foreach ( IInstallTask installTask in _installTasks ) {
        installTask.Uninstall();
      }

      _persistenceInstaller.Uninstall();
    }
  }
}