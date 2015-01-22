using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Xml.XPath;
using TeaCommerce.Api.Infrastructure.Installation;
using TeaCommerce.Api.Infrastructure.Logging;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Persistence;
using TeaCommerce.Api.Persistence.Installation;
using TeaCommerce.Api.Services;

namespace TeaCommerce.Umbraco.Configuration.Infrastructure.Installation {
  public class Installer : IInstaller {

    private readonly IDatabaseFactory _databaseFactory;
    private readonly PersistenceInstaller _persistenceInstaller;
    private readonly IStoreService _storeService;
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly IOrderService _orderService;

    public Installer( IDatabaseFactory databaseFactory, IStoreService storeService, IPaymentMethodService paymentMethodService, IOrderService orderService ) {
      _databaseFactory = databaseFactory;
      _storeService = storeService;
      _paymentMethodService = paymentMethodService;
      _orderService = orderService;
      _persistenceInstaller = new PersistenceInstaller( databaseFactory );
    }

    public void InstallOrUpdate() {
      _persistenceInstaller.InstallOrUpdate();

      Database database = _databaseFactory.Get();

      int currentVersion = database.ExecuteScalar<int>( "SELECT SpecialActionsVersion FROM TeaCommerce_Version" );
      int newVersion = 6;

      while ( currentVersion < newVersion ) {
        try {
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

          #region 3.0.1

          if ( currentVersion + 1 == 5 ) {
            #region Set default gift card settings for stores

            foreach ( Store store in _storeService.GetAll().Where( store => store.GiftCardSettings.Length == 0 && store.GiftCardSettings.DaysValid == 0 ) ) {
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
          LoggingService.Instance.Log( exp );
          break;
        }
      }

    }

    public void Uninstall() {
      _persistenceInstaller.Uninstall();
    }
  }
}