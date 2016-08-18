using Autofac;
using System;
using System.Collections.Generic;
using TeaCommerce.Api.Dependency;
using TeaCommerce.Api.Marketing.Models;
using TeaCommerce.Api.Models;
using TeaCommerce.Api.Web;
using TeaCommerce.Umbraco.Configuration.InformationExtractors;
using TeaCommerce.Umbraco.Configuration.Services;
using TeaCommerce.Umbraco.Configuration.Variant;
using TeaCommerce.Umbraco.Configuration.Variant.Product;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace TeaCommerce.Umbraco.Web {

  /// <summary>
  /// Static Razor library to receive data from Tea Commerce and interact with the customers shopping experience
  /// </summary>
  public static class TC {

    #region Store

    /// <summary>
    /// Gets a specific store.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <returns>A store object.</returns>
    public static Store GetStore( long storeId ) {
      return TeaCommerceHelper.GetStore( storeId );
    }

    #endregion

    #region Order

    /// <summary>
    /// Gets the customers current order.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="autoCreate">If <c>true</c> - creates a new order if no current order is present in the session.</param>
    /// <returns>An order object.</returns>
    public static Order GetCurrentOrder( long storeId, bool autoCreate = true ) {
      return TeaCommerceHelper.GetCurrentOrder( storeId, autoCreate );
    }

    /// <summary>
    /// Sets the customers current order.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="orderId">Unique id of the order.</param>
    /// <returns>The new current order object.</returns>
    public static Order SetCurrentOrder( long storeId, Guid orderId ) {
      return TeaCommerceHelper.SetCurrentOrder( storeId, orderId );
    }

    /// <summary>
    /// Removes the customers current order from session. The order will not be deleted.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <returns>The removed order object.</returns>
    public static Order RemoveCurrentOrder( long storeId ) {
      return TeaCommerceHelper.RemoveCurrentOrder( storeId );
    }

    /// <summary>
    /// Gets the customers current finalized order.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <returns>An order object.</returns>
    public static Order GetCurrentFinalizedOrder( long storeId ) {
      return TeaCommerceHelper.GetCurrentFinalizedOrder( storeId );
    }

    /// <summary>
    /// Gets a specific order.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="orderId">Unique id of the order.</param>
    /// <returns>An order object.</returns>
    public static Order GetOrder( long storeId, Guid orderId ) {
      return TeaCommerceHelper.GetOrder( storeId, orderId );
    }

    /// <summary>
    /// Gets a collection of orders.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="orderIds">A collection of unique order id's.</param>
    /// <returns>A collection of orders.</returns>
    public static IEnumerable<Order> GetOrders( long storeId, IEnumerable<Guid> orderIds ) {
      return TeaCommerceHelper.GetOrders( storeId, orderIds );
    }

    /// <summary>
    /// Gets finalized orders for a specific customer.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="customerId">Id of the customer.</param>
    /// <returns>A collection of orders.</returns>
    public static IEnumerable<Order> GetFinalizedOrdersForCustomer( long storeId, string customerId ) {
      return TeaCommerceHelper.GetFinalizedOrdersForCustomer( storeId, customerId );
    }

    #endregion

    #region Order lines

    /// <summary>
    /// Adds or updates a single order line on the current order. If the customer does not have a current order a new one will be created.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="productIdentifier">A unique identifier of the product. E.g. the node id from Umbraco.</param>
    /// <param name="quantity">The quantity of the order line. Default behavior will add the quantity to the existing order line's quantity. This can be changed using the <paramref name="overwriteQuantity"/> parameter.</param>
    /// <param name="properties">A dictionary containing the property aliases and their values.</param>
    /// <param name="overwriteQuantity">If <c>true</c> - the <paramref name="quantity"/> parameter will overwrite the current quantity of the order line.</param>
    /// <param name="bundleIdentifier">Use to be able to create product bundles. This identifier is used when adding sub order lines to this order line.</param>
    /// <param name="parentBundleIdentifier">The <paramref name="bundleIdentifier" /> of the order line you want to add this product to.</param>
    /// <returns>The order line just created or updated.</returns>
    public static OrderLine AddOrUpdateOrderLine( long storeId, string productIdentifier, decimal? quantity = null, IDictionary<string, string> properties = null, bool overwriteQuantity = false, string bundleIdentifier = null, string parentBundleIdentifier = null ) {
      return TeaCommerceHelper.AddOrUpdateOrderLine( storeId, productIdentifier, quantity, properties, overwriteQuantity, bundleIdentifier, parentBundleIdentifier );
    }

    /// <summary>
    /// Updates a single order line on the current order. If the customer does not have a current order a new one will be created.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="orderLineId">The id of a specific order line.</param>
    /// <param name="quantity">The quantity of the order line. Default behavior will add the quantity to the existing order line's quantity. This can be changed using the <paramref name="overwriteQuantity"/> parameter.</param>
    /// <param name="properties">A dictionary containing the property aliases and their values.</param>
    /// <param name="overwriteQuantity">If <c>true</c> - the <paramref name="quantity"/> parameter will overwrite the current quantity of the order line.</param>
    /// <returns>The order line updated.</returns>
    public static OrderLine UpdateOrderLine( long storeId, long orderLineId, decimal? quantity = null, IDictionary<string, string> properties = null, bool overwriteQuantity = false ) {
      return TeaCommerceHelper.UpdateOrderLine( storeId, orderLineId, quantity, properties, overwriteQuantity );
    }

    /// <summary>
    /// Removes a single order line from the customer's current order.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="orderLineId">The id of a specific order line.</param>
    /// <returns>The removed order line.</returns>
    public static OrderLine RemoveOrderLine( long storeId, long orderLineId ) {
      return TeaCommerceHelper.RemoveOrderLine( storeId, orderLineId );
    }

    /// <summary>
    /// Removes all order lines from the customer's current order.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <returns>A collection of all removed order lines.</returns>
    public static IEnumerable<OrderLine> RemoveAllOrderLines( long storeId ) {
      return TeaCommerceHelper.RemoveAllOrderLines( storeId );
    }

    #endregion

    #region Order properties

    /// <summary>
    /// Adds or updates a single custom order property on the current order. If the customer does not have a current order a new one will be created.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="key">The key/alias of the property.</param>
    /// <param name="value">Value of the property. Can be any number of characters.</param>
    /// <returns>The custom order property just created or updated.</returns>
    public static CustomProperty AddOrUpdateOrderProperty( long storeId, string key, string value ) {
      return TeaCommerceHelper.AddOrUpdateOrderProperty( storeId, key, value );
    }

    /// <summary>
    /// Adds or updates a collection of custom order properties on the current order. If the customer does not have a current order a new one will be created.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="properties">A dictionary where keys will be the new property's alias and the value will be the value of the new property.</param>
    /// <returns>A collection of the added or updated custom order properties.</returns>
    public static IEnumerable<CustomProperty> AddOrUpdateOrderProperties( long storeId, IDictionary<string, string> properties ) {
      return TeaCommerceHelper.AddOrUpdateOrderProperties( storeId, properties );
    }

    #endregion

    #region Vat groups

    public static VatGroup GetVatGroup( long storeId, long vatGroupId ) {
      return TeaCommerceHelper.GetVatGroup( storeId, vatGroupId );
    }

    public static IEnumerable<VatGroup> GetVatGroups( long storeId ) {
      return TeaCommerceHelper.GetVatGroups( storeId );
    }

    public static VatGroup GetCurrentVatGroup( long storeId ) {
      return TeaCommerceHelper.GetCurrentVatGroup( storeId );
    }

    public static VatGroup SetCurrentVatGroup( long storeId, long vatGroupId ) {
      return TeaCommerceHelper.SetCurrentVatGroup( storeId, vatGroupId );
    }

    #endregion

    #region Currencies

    /// <summary>
    /// Gets a currency from a specfic store.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="currencyId">The currency id.</param>
    /// <returns>A currency object.</returns>
    public static Currency GetCurrency( long storeId, long currencyId ) {
      return TeaCommerceHelper.GetCurrency( storeId, currencyId );
    }

    /// <summary>
    /// Gets the currencies for a specific store that are allowed in the current payment country region. Will fallback to current payment country.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="onlyAllowed">If <c>true</c> - only currencies allowed in the current session state will be returned.</param>
    /// <returns>A collection of currency objects.</returns>
    public static IEnumerable<Currency> GetCurrencies( long storeId, bool onlyAllowed = true ) {
      return TeaCommerceHelper.GetCurrencies( storeId, onlyAllowed );
    }

    /// <summary>
    /// Gets the current currency of the customer's session. If the customer has a current order the currency of that order is returned.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <returns>The current currency.</returns>
    public static Currency GetCurrentCurrency( long storeId ) {
      return TeaCommerceHelper.GetCurrentCurrency( storeId );
    }

    /// <summary>
    /// Changes the current currency of the customer's session. The currency is also changed for the customer's current order if present. If you try to set the current currency to a currency that is not allowed in the current payment country, nothing will happen.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="currencyId">Id of the currency to change to.</param>
    /// <returns>The current currency.</returns>
    public static Currency SetCurrentCurrency( long storeId, long currencyId ) {
      return TeaCommerceHelper.SetCurrentCurrency( storeId, currencyId );
    }

    #endregion

    #region Countries

    /// <summary>
    /// Gets a country from a specfic store.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="countryId">The country id.</param>
    /// <returns>A country object.</returns>
    public static Country GetCountry( long storeId, long countryId ) {
      return TeaCommerceHelper.GetCountry( storeId, countryId );
    }

    /// <summary>
    /// Gets the countries for a specific store.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <returns>A collection of country objects.</returns>
    public static IEnumerable<Country> GetCountries( long storeId ) {
      return TeaCommerceHelper.GetCountries( storeId );
    }

    /// <summary>
    /// Gets the current payment country of the customer's session. If the customer has a current order the payment country of that order is returned.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <returns>The current payment country object.</returns>
    public static Country GetCurrentPaymentCountry( long storeId ) {
      return TeaCommerceHelper.GetCurrentPaymentCountry( storeId );
    }

    /// <summary>
    /// Gets the current shipping country of the customer's session. If the customer has a current order the shipping country of that order is returned.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <returns>The current shipping country object.</returns>
    public static Country GetCurrentShippingCountry( long storeId ) {
      return TeaCommerceHelper.GetCurrentShippingCountry( storeId );
    }

    /// <summary>
    /// Changes the current payment country of the customer's session. The payment country is also changed for the customer's current order if present. If the country is changed and the current currency isn't allowed in this country - then the currency will change to the default currency of the new payment country. It is the same with the current shipping method and payment method.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="countryId">Id of the country to change to.</param>
    /// <returns>The new current payment country object.</returns>
    public static Country SetCurrentPaymentCountry( long storeId, long countryId ) {
      return TeaCommerceHelper.SetCurrentPaymentCountry( storeId, countryId );
    }

    /// <summary>
    /// Changes the current shipping country of the customer's session. The shipping country is also changed for the customer's current order if present. If the country is changed and the current shipping method isn't allowed in this country - then the shipping method will change to the default shipping method of the new shipping country. If no current shipping country is specified it will fallback to use the payment country.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="countryId">Id of the country to change to.</param>
    /// <returns>The new current shipping country object.</returns>
    public static Country SetCurrentShippingCountry( long storeId, long? countryId ) {
      return TeaCommerceHelper.SetCurrentShippingCountry( storeId, countryId );
    }

    #endregion

    #region Country regions

    /// <summary>
    /// Gets a country region from a specfic store.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="countryRegionId">The country region id.</param>
    /// <returns>A country region object.</returns>
    public static CountryRegion GetCountryRegion( long storeId, long countryRegionId ) {
      return TeaCommerceHelper.GetCountryRegion( storeId, countryRegionId );
    }

    /// <summary>
    /// Gets the country regions for a specific store. Specify a <paramref name="countryId"/> to only get country regions for this country.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="countryId">Id of the country.</param>
    /// <returns>A collection of country region objects.</returns>
    public static IEnumerable<CountryRegion> GetCountryRegions( long storeId, long? countryId = null ) {
      return TeaCommerceHelper.GetCountryRegions( storeId, countryId );
    }

    /// <summary>
    /// Gets the current payment country region of the customer's session. If the customer has a current order the payment country region of that order is returned.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <returns>The current payment country region object.</returns>
    public static CountryRegion GetCurrentPaymentCountryRegion( long storeId ) {
      return TeaCommerceHelper.GetCurrentPaymentCountryRegion( storeId );
    }

    /// <summary>
    /// Gets the current shipping country region of the customer's session. If the customer has a current order the shipping country region of that order is returned.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <returns>The current shipping country region object.</returns>
    public static CountryRegion GetCurrentShippingCountryRegion( long storeId ) {
      return TeaCommerceHelper.GetCurrentShippingCountryRegion( storeId );
    }

    /// <summary>
    /// Changes the current payment country region of the customer's session. The payment country region is also changed for the customer's current order if present. If the country region is changed and the current payment and method isn't allowed in this country region - then the payment method will change to the default payment method of the new payment country region. If no current payment country region is specified it will fallback to use the payment country.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="countryRegionId">Id of the country region to change to.</param>
    /// <returns>The new current payment country region object.</returns>
    public static CountryRegion SetCurrentPaymentCountryRegion( long storeId, long? countryRegionId ) {
      return TeaCommerceHelper.SetCurrentPaymentCountryRegion( storeId, countryRegionId );
    }

    /// <summary>
    /// Changes the current shipping country region of the customer's session. The shipping country region is also changed for the customer's current order if present. If the country region is changed and the current shipping method isn't allowed in this country region - then the shipping method will change to the default shipping method of the new shipping country region. If no current shipping country region is specified it will fallback to use the shipping country and then payment country.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="countryRegionId">Id of the country region to change to.</param>
    /// <returns>The new current shipping country region object.</returns>
    public static CountryRegion SetCurrentShippingCountryRegion( long storeId, long? countryRegionId ) {
      return TeaCommerceHelper.SetCurrentShippingCountryRegion( storeId, countryRegionId );
    }

    #endregion

    #region Shipping methods

    /// <summary>
    /// Gets a shipping method from a specfic store.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="shippingMethodId">Id of the shipping method.</param>
    /// <returns>A shipping method object.</returns>
    public static ShippingMethod GetShippingMethod( long storeId, long shippingMethodId ) {
      return TeaCommerceHelper.GetShippingMethod( storeId, shippingMethodId );
    }

    /// <summary>
    /// Gets a collection of all shipping methods that are allowed in the current shipping country and shipping country region. Fallback to payment country and payment country region.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="onlyAllowed">If <c>true</c> - only shipping methods allowed in the current session state will be returned.</param>
    /// <returns>A collection of shipping method objects.</returns>
    public static IEnumerable<ShippingMethod> GetShippingMethods( long storeId, bool onlyAllowed = true ) {
      return TeaCommerceHelper.GetShippingMethods( storeId, onlyAllowed );
    }

    /// <summary>
    /// Gets the current shipping method of the customer's session. If the customer has a current order the shipping method of that order is returned.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <returns>The current shipping method object.</returns>
    public static ShippingMethod GetCurrentShippingMethod( long storeId ) {
      return TeaCommerceHelper.GetCurrentShippingMethod( storeId );
    }

    /// <summary>
    /// Changes the current shipping method of the customer's session. The shipping method is also changed for the customer's current order if present.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="shippingMethodId">Id of the shipping method.</param>
    /// <returns>The new current shipping method object.</returns>
    public static ShippingMethod SetCurrentShippingMethod( long storeId, long? shippingMethodId ) {
      return TeaCommerceHelper.SetCurrentShippingMethod( storeId, shippingMethodId );
    }

    /// <summary>
    /// Get the price for a specific shipping method using the current currency.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="shippingMethodId">Id of the shipping method.</param>
    /// <returns>A price object.</returns>
    public static Price GetPriceForShippingMethod( long storeId, long shippingMethodId ) {
      return TeaCommerceHelper.GetPriceForShippingMethod( storeId, shippingMethodId );
    }

    #endregion

    #region Payment methods

    /// <summary>
    /// Gets a payment method from a specfic store.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="paymentMethodId">Id of the payment method.</param>
    /// <returns>A payment method object.</returns>
    public static PaymentMethod GetPaymentMethod( long storeId, long paymentMethodId ) {
      return TeaCommerceHelper.GetPaymentMethod( storeId, paymentMethodId );
    }

    /// <summary>
    /// Gets a collection of all payment methods that are allowed in the current payment country and payment country region.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="onlyAllowed">If <c>true</c> - only payment methods allowed in the current session state will be returned.</param>
    /// <returns>A collection of payment methods objects.</returns>
    public static IEnumerable<PaymentMethod> GetPaymentMethods( long storeId, bool onlyAllowed = true ) {
      return TeaCommerceHelper.GetPaymentMethods( storeId, onlyAllowed );
    }

    /// <summary>
    /// Gets the current payment method of the customer's session. If the customer has a current order the payment method of that order is returned.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <returns>The current payment method object.</returns>
    public static PaymentMethod GetCurrentPaymentMethod( long storeId ) {
      return TeaCommerceHelper.GetCurrentPaymentMethod( storeId );
    }

    /// <summary>
    /// Changes the current payment method of the customer's session. The payment method is also changed for the customer's current order if present.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="paymentMethodId">Id of the payment method.</param>
    /// <returns>The new current payment method object.</returns>
    public static PaymentMethod SetCurrentPaymentMethod( long storeId, long? paymentMethodId ) {
      return TeaCommerceHelper.SetCurrentPaymentMethod( storeId, paymentMethodId );
    }

    /// <summary>
    /// Get the price for a specific payment method using the current currency.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="paymentMethodId">Id of the payment method.</param>
    /// <returns>A price object.</returns>
    public static Price GetPriceForPaymentMethod( long storeId, long paymentMethodId ) {
      return TeaCommerceHelper.GetPriceForPaymentMethod( storeId, paymentMethodId );
    }

    #endregion

    #region Payment provider

    /// <summary>
    /// Generates and returns the full html for the form to post when going to payment. Will use the current payment method's payment provider.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="submitInput">A html button/input that should be outputted with the form. Will make it possible to use whatever button needed in the setup.</param>
    /// <returns>A html form.</returns>
    public static string GeneratePaymentForm( long storeId, string submitInput ) {
      return TeaCommerceHelper.GeneratePaymentForm( storeId, submitInput );
    }

    #endregion

    #region Discount codes

    /// <summary>
    /// Adds a discount code to the customer's current order.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="code">The discount code.</param>
    /// <returns>The added discount code.</returns>
    public static AppliedDiscountCode AddDiscountCode( long storeId, string code ) {
      return TeaCommerceHelper.AddDiscountCode( storeId, code );
    }

    /// <summary>
    /// Remove a discount code from the customer's current order.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="code">The discount code.</param>
    /// <returns>The removed discount code.</returns>
    public static AppliedDiscountCode RemoveDiscountCode( long storeId, string code ) {
      return TeaCommerceHelper.RemoveDiscountCode( storeId, code );
    }

    #endregion

    #region Gift cards

    /// <summary>
    /// Adds a gift card to the customer's current order.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="code">The gift card code.</param>
    /// <returns>The added gift card.</returns>
    public static AppliedGiftCard AddGiftCard( long storeId, string code ) {
      return TeaCommerceHelper.AddGiftCard( storeId, code );
    }

    /// <summary>
    /// Remove a gift card from the customer's current order.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="code">The gift card code.</param>
    /// <returns>The removed gift card.</returns>
    public static AppliedGiftCard RemoveGiftCard( long storeId, string code ) {
      return TeaCommerceHelper.RemoveGiftCard( storeId, code );
    }

    #endregion

    #region Prices

    /// <summary>
    /// Format a decimal to a price using the current currency.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="price">The price to format excl. VAT.</param>
    /// <returns>A price object.</returns>
    public static Price FormatPrice( long storeId, decimal price, long? currencyId ) {
      return TeaCommerceHelper.FormatPrice( storeId, price, currencyId );
    }

    /// <summary>
    /// Gets a price for a specific product in the current currency.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="productIdentifier">A unique identifier of the product. E.g. the node id from Umbraco.</param>
    /// <returns>A price object.</returns>
    public static Price GetPrice( long storeId, string productIdentifier ) {
      return TeaCommerceHelper.GetPrice( storeId, productIdentifier );
    }

    public static Price GetPrice<T1>( long storeId, T1 product ) {
      return TeaCommerceHelper.GetPrice<T1, string>( storeId, product, null );
    }

    public static Price GetPrice<T1, T2>( long storeId, T1 product, T2 variant ) {
      return TeaCommerceHelper.GetPrice( storeId, product, variant );
    }

    #endregion

    #region Stock

    /// <summary>
    /// Gets the current stock for a specific product.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="productIdentifier">A unique identifier of the product. E.g. the node id from Umbraco.</param>
    /// <returns>The stock of the product. Will be null if no stock have been provided for this product.</returns>
    public static decimal? GetStock( long storeId, string productIdentifier ) {
      return TeaCommerceHelper.GetStock( storeId, productIdentifier );
    }

    public static decimal? GetStock<T1, T2>( long storeId, T1 product, T2 variant ) {
      return TeaCommerceHelper.GetStock( storeId, product, variant );
    }

    #endregion

    #region Product information

    /// <summary>
    /// Returns the value of a property on the product. Will traverse the content tree recursively to find the value. Will also use the master relation property of the product to search master products. NOTE: If you have a DynamicNode model use that instead of the string productIdentifier, which is slightly slower.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="productIdentifier">A unique identifier of the product. E.g. the node id from Umbraco.</param>
    /// <param name="propertyAlias">Alias of the property to find.</param>
    /// <param name="func">A function to filter the result.</param>
    /// <returns>The text value of the property.</returns>
    public static T GetPropertyValue<T>( long storeId, string productIdentifier, string propertyAlias, Func<IPublishedContent, bool> func = null ) {
      ProductIdentifier productIdentifierObj = new ProductIdentifier( productIdentifier );
      UmbracoHelper umbracoHelper = new UmbracoHelper( UmbracoContext.Current );

      return PublishedContentProductInformationExtractor.Instance.GetPropertyValue<T>( umbracoHelper.TypedContent( productIdentifierObj.NodeId ), propertyAlias, productIdentifierObj.VariantId, func );
    }

    /// <summary>
    /// Returns the value of a property on the product. Will traverse the content tree recursively to find the value. Will also use the master relation property of the product to search master products.
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="model">The product as a IPublishedContent.</param>
    /// <param name="propertyAlias">Alias of the property to find.</param>
    /// <param name="variantId">The id of a specific product variant</param>
    /// <param name="func">A function to filter the result.</param>
    /// <returns>The text value of the property.</returns>
    public static T GetPropertyValue<T>( long storeId, IPublishedContent model, string propertyAlias, string variantId = null, Func<IPublishedContent, bool> func = null ) {
      return PublishedContentProductInformationExtractor.Instance.GetPropertyValue<T>( model, propertyAlias, variantId, func );
    }

    #endregion

    #region Variants

    /// <summary>
    /// Get a variant from a specific product content. The variants will be fetched from the property field using the "Tea Commerce: Variant editor"
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="model">The product as a IPublishedContent.</param>
    /// <param name="variantId">The id of a specific product variant</param>
    /// <param name="onlyValid">Fetch only the valid variants. A valid variant have one of each variant type and is not a duplicate.</param>
    /// <returns></returns>
    public static VariantPublishedContent GetVariant<T>( long storeId, T model, string variantId, bool onlyValid = true ) {
      IVariantService<T> variantService = DependencyContainer.Instance.Resolve<IVariantService<T>>();
      return variantService.GetVariant( storeId, model, variantId, onlyValid );
    }

    /// <summary>
    /// Get the variants from a specific product content. The variants will be fetched from the property field using the "Tea Commerce: Variant editor"
    /// </summary>
    /// <param name="storeId">Id of the store.</param>
    /// <param name="model">The product as a IPublishedContent.</param>
    /// <param name="onlyValid">Fetch only the valid variants. A valid variant have one of each variant type and is not a duplicate.</param>
    /// <returns></returns>
    public static IEnumerable<VariantPublishedContent> GetVariants<T>( long storeId, T model, bool onlyValid = true ) {
      IVariantService<T> variantService = DependencyContainer.Instance.Resolve<IVariantService<T>>();
      return variantService.GetVariants( storeId, model, onlyValid );

    }

    /// <summary>
    /// Gets the attribute groups and attributes present in a collection of variants.
    /// </summary>
    /// <param name="variants">A collection of variants.</param>
    /// <returns></returns>
    public static IEnumerable<VariantGroup> GetVariantGroups<T>( IEnumerable<VariantPublishedContent> variants ) {
      IVariantService<T> variantService = DependencyContainer.Instance.Resolve<IVariantService<T>>();
      return variantService.GetVariantGroups( variants );
    }

    /// <summary>
    /// Will get variant information from several products. This json will mostly be used to create variant selection drop downs in the frontend
    /// </summary>
    /// <param name="variants"></param>
    /// <returns>A json blob with a dictionary of products and their variants</returns>
    public static string GetVariantJson<T>( long storeId, IEnumerable<T> productContents, bool onlyValid ) {
      IVariantService<T> variantService = DependencyContainer.Instance.Resolve<IVariantService<T>>();
      return variantService.GetVariantJson( storeId, productContents, onlyValid );
    }

    /// <summary>
    /// Will get variant information from a single product. This json will mostly be used to create variant selection drop downs in the frontend
    /// </summary>
    /// <param name="variants"></param>
    /// <returns>A json blob with a dictionary of products and their variants. This dictionary will only contain a single product</returns>
    public static string GetVariantJson<T>( long storeId, T productContents, bool onlyValid ) {
      IVariantService<T> variantService = DependencyContainer.Instance.Resolve<IVariantService<T>>();
      return variantService.GetVariantJson( storeId, new List<T> { productContents }, onlyValid );
    }

    #endregion
  }
}
