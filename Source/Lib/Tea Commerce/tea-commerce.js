/***************************************************
- TEACOMMERCE CLASS
***************************************************/
if (typeof TC === 'undefined') { var TC = {}; }

(function () {

  /***************************************************
  - ORDER
  ***************************************************/

  TC.getCurrentOrder = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCurrentOrder',
    formData = {};
    formData[method] = 'autoCreate';
    formData.autoCreate = settings.autoCreate;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.removeCurrentOrder = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'RemoveCurrentOrder',
    formData = {};
    formData[method] = '';

    return tcs.postToServer(method, formData, settings);
  };

  TC.getCurrentFinalizedOrder = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCurrentFinalizedOrder',
    formData = {};
    formData[method] = '';

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getOrder = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetOrder',
        formData = {};
    formData[method] = 'orderId';
    formData.orderId = settings.orderId;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getOrders = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetOrders',
        formData = {},
        i = 0;
    formData[method] = 'orderIds';
    formData.orderIds = '';

    for (i = 0; i < settings.orderIds.length; i++) {
      var orderId = settings.orderIds[i];
      if (formData.orderIds.length > 0) {
        formData.orderIds += ',';
      }
      formData.orderIds += orderId;
    }

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };
  
  TC.getFinalizedOrdersForCustomer = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetFinalizedOrdersForCustomer',
        formData = {};
    formData[method] = 'customerId';
    formData.customerId = settings.customerId;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };


  /***************************************************
  - ORDER LINES
  ***************************************************/

  TC.addOrUpdateOrderLine = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'AddOrUpdateOrderLine',
        formData = {};
    formData[method] = 'productIdentifier, orderLineId, quantity, properties, overwriteQuantity, bundleIdentifier, parentBundleIdentifier';
    formData.productIdentifier = settings.productIdentifier;
    formData.orderLineId = settings.orderLineId;
    formData.quantity = settings.quantity;
    formData.properties = '';
    formData.overwriteQuantity = settings.overwriteQuantity;
    formData.bundleIdentifier = settings.bundleIdentifier;
    formData.parentBundleIdentifier = settings.parentBundleIdentifier;

    for (key in settings.properties) {
      if (formData.properties.length > 0) {
        formData.properties += ',';
      }
      formData.properties += key;
      formData[key] = settings.properties[key];
    }

    return tcs.postToServer(method, formData, settings);
  };

  TC.removeOrderLine = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'RemoveOrderLine',
        formData = {};
    formData[method] = 'orderLineId';
    formData.orderLineId = settings.orderLineId;

    return tcs.postToServer(method, formData, settings);
  };

  TC.removeAllOrderLines = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'RemoveAllOrderLines',
        formData = {};
    formData[method] = '';

    return tcs.postToServer(method, formData, settings);
  };

  /***************************************************
  - ORDER PROPERTIES
  ***************************************************/

  TC.addOrUpdateOrderProperties = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'AddOrUpdateOrderProperties',
        formData = {};
    formData[method] = 'properties';
    formData.properties = '';

    for (key in settings.properties) {
      if (formData.properties.length > 0) {
        formData.properties += ',';
      }
      formData.properties += key;
      formData[key] = settings.properties[key];
    }

    return tcs.postToServer(method, formData, settings);
  };

  /***************************************************
  - CURRENCIES
  ***************************************************/

  TC.getCurrency = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCurrency',
        formData = {};
    formData[method] = 'currencyId';
    formData.currencyId = settings.currencyId;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getCurrencies = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCurrencies',
        formData = {};
    formData[method] = 'onlyAllowed';
    formData.onlyAllowed = settings.onlyAllowed;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getCurrentCurrency = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCurrentCurrency',
        formData = {};
    formData[method] = '';

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.setCurrentCurrency = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'SetCurrentCurrency',
        formData = {};
    formData[method] = 'currencyId';
    formData.currencyId = settings.currencyId;

    return tcs.postToServer(method, formData, settings);
  };

  /***************************************************
  - COUNTRIES
  ***************************************************/

  TC.getCountry = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCountry',
        formData = {};
    formData[method] = 'countryId';
    formData.countryId = settings.countryId;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getCountries = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCountries',
        formData = {};
    formData[method] = '';

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getCurrentPaymentCountry = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCurrentPaymentCountry',
        formData = {};
    formData[method] = '';

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getCurrentShippingCountry = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCurrentShippingCountry',
        formData = {};
    formData[method] = '';

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.setCurrentPaymentCountry = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'SetCurrentPaymentCountry',
        formData = {};
    formData[method] = 'countryId';
    formData.countryId = settings.countryId;

    return tcs.postToServer(method, formData, settings);
  };

  TC.setCurrentShippingCountry = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'SetCurrentShippingCountry',
        formData = {};
    formData[method] = 'countryId';
    formData.countryId = settings.countryId;

    return tcs.postToServer(method, formData, settings);
  };

  /***************************************************
  - COUNTRY REGIONS
  ***************************************************/

  TC.getCountryRegion = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCountryRegion',
        formData = {};
    formData[method] = 'countryRegionId';
    formData.countryRegionId = settings.countryRegionId;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getCountryRegions = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCountryRegions',
        formData = {};
    formData[method] = 'countryId';
    formData.countryId = settings.countryId;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getCurrentPaymentCountryRegion = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCurrentPaymentCountryRegion',
        formData = {};
    formData[method] = '';

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getCurrentShippingCountryRegion = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCurrentShippingCountryRegion',
        formData = {};
    formData[method] = '';

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.setCurrentPaymentCountryRegion = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'SetCurrentPaymentCountryRegion',
        formData = {};
    formData[method] = 'countryRegionId';
    formData.countryRegionId = settings.countryRegionId;

    return tcs.postToServer(method, formData, settings);
  };

  TC.setCurrentShippingCountryRegion = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'SetCurrentShippingCountryRegion',
        formData = {};
    formData[method] = 'countryRegionId';
    formData.countryRegionId = settings.countryRegionId;

    return tcs.postToServer(method, formData, settings);
  };

  /***************************************************
  - SHIPPING METHODS
  ***************************************************/

  TC.getShippingMethod = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetShippingMethod',
        formData = {};
    formData[method] = 'shippingMethodId';
    formData.shippingMethodId = settings.shippingMethodId;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getShippingMethods = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetShippingMethods',
        formData = {};
    formData[method] = 'onlyAllowed';
    formData.onlyAllowed = settings.onlyAllowed;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getCurrentShippingMethod = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCurrentShippingMethod',
        formData = {};
    formData[method] = '';

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.setCurrentShippingMethod = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'SetCurrentShippingMethod',
        formData = {};
    formData[method] = 'shippingMethodId';
    formData.shippingMethodId = settings.shippingMethodId;

    return tcs.postToServer(method, formData, settings);
  };

  TC.getPriceForShippingMethod = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetPriceForShippingMethod',
        formData = {};
    formData[method] = 'shippingMethodId';
    formData.shippingMethodId = settings.shippingMethodId;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  /***************************************************
  - PAYMENT METHODS
  ***************************************************/

  TC.getPaymentMethod = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetPaymentMethod',
        formData = {};
    formData[method] = 'paymentMethodId';
    formData.paymentMethodId = settings.paymentMethodId;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getPaymentMethods = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetPaymentMethods',
        formData = {};
    formData[method] = 'onlyAllowed';
    formData.onlyAllowed = settings.onlyAllowed;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getCurrentPaymentMethod = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetCurrentPaymentMethod',
        formData = {};
    formData[method] = '';

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.setCurrentPaymentMethod = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'SetCurrentPaymentMethod',
        formData = {};
    formData[method] = 'paymentMethodId';
    formData.paymentMethodId = settings.paymentMethodId;

    return tcs.postToServer(method, formData, settings);
  };

  TC.getPriceForPaymentMethod = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetPriceForPaymentMethod',
        formData = {};
    formData[method] = 'paymentMethodId';
    formData.paymentMethodId = settings.paymentMethodId;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  /***************************************************
  - PAYMENT PROVIDER
  ***************************************************/

  TC.generatePaymentForm = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GeneratePaymentForm',
        formData = {};
    formData[method] = '';

    settings.async = false;
    var rtnData = tcs.postToServer(method, formData, settings);

    var form = jQuery(rtnData.data.form);
    jQuery('body').append(form);
    if (rtnData.submitJavascriptFunction) {
      eval(rtnData.data.submitJavascriptFunction);
    } else {
      form[0].submit();
    }
  };

  /***************************************************
  - DISCOUNT CODES
  ***************************************************/

  TC.addDiscountCode = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'AddDiscountCode',
        formData = {};
    formData[method] = '';

    formData[method] = 'code';
    formData.code = settings.code;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.removeDiscountCode = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'RemoveDiscountCode',
        formData = {};
    formData[method] = '';

    formData[method] = 'code';
    formData.code = settings.code;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  /***************************************************
  - GIFT CARDS
  ***************************************************/

  TC.addGiftCard = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'AddGiftCard',
        formData = {};
    formData[method] = '';

    formData[method] = 'code';
    formData.code = settings.code;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.removeGiftCard = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'RemoveGiftCard',
        formData = {};
    formData[method] = '';

    formData[method] = 'code';
    formData.code = settings.code;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  /***************************************************
  - PRICES
  ***************************************************/

  TC.formatPrice = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'FormatPrice',
        formData = {};
    formData[method] = '';

    formData[method] = 'price';
    formData.price = settings.price;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  TC.getPrice = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetPrice',
        formData = {};
    formData[method] = '';

    formData[method] = 'productIdentifier';
    formData.productIdentifier = settings.productIdentifier;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  /***************************************************
  - STOCK
  ***************************************************/

  TC.getStock = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'GetStock',
        formData = {};
    formData[method] = '';

    formData[method] = 'productIdentifier';
    formData.productIdentifier = settings.productIdentifier;

    if (typeof settings.async === 'undefined') {
      settings.async = false;
    }
    return tcs.postToServer(method, formData, settings);
  };

  /***************************************************
  - RENDER TEMPLATE FILE
  ***************************************************/
  TC.renderTemplateFile = function (settings) {
    settings = tcs.fixSettings(settings);
    var method = 'RenderTemplateFile',
        formData = {};
    formData[method] = 'templateFile, pageId, cultureName';
    formData.templateFile = settings.templateFile;
    formData.pageId = settings.pageId;
    formData.cultureName = settings.cultureName;

    if (typeof settings.dataType === 'undefined') {
      settings.dataType = 'text';
    }
    return tcs.postToServer(method, formData, settings);
  };

  /***************************************************
  - EVENT HANDLERS
  ***************************************************/

  TC.bind = function (event, fn) {
    tcs.bind(event, fn);
  };
  
  /***************************************************
  - POST FORM USING AJAX
  ***************************************************/
  TC.postForm = function (form) {
    tcs.postForm(form);
  };

  /***************************************************
  - PRIVATE SERVICE OBJECT 
  ***************************************************/
  var TCService = function () {
    var TCService = this;
    TCService.eventSubscribers = [];

    TCService.postToServer = function (method, formData, settings) {
      formData = formData || {};
      formData.isJavaScript = 'true';

      var rtnData = null,
          async = settings.async,
          dataType = settings.dataType,
          storeId = settings.storeId || _storeId,
          adminOrderId = settings.adminOrderId || '';

      formData.storeId = storeId;
      formData.adminOrderId = adminOrderId;

      if (typeof async === 'undefined') {
        async = true;
      }
      if (typeof dataType === 'undefined') {
        dataType = 'json';
      }
      
      if (async) {
        TCService.fireBeforeEvent(method, formData);
        TCService.fireBeforeEvent('CartUpdated', formData);
      }
      
      jQuery.ajax({
        type: 'POST',
        url: '[formPostUrl]',
        data: formData,
        async: async,
        success: function (json, success, response) { rtnData = TCService.success(json, success, response, null, settings); },
        error: function (json) { TCService.error(json, settings); },
        cache: false,
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        dataType: dataType
      });
      return rtnData;
    };


    TCService.success = function (json, success, response, jQForm, settings) {
      if (json && json.order) {
        TCService.fixDates(json.order);
      }
      TCService.fixDates(json);
      if (settings && settings.successfn) {
        settings.successfn(json, jQForm);
      }
      if (!settings || (json && json.methodsCalled)) {
        var i;
        for (i = 0; i < json.methodsCalled.length; i++) {
          var data = json.methodsCalled[i];
          TCService.fireAfterEvent(data.eventName, data.data, json, jQForm);
        }
      }

      if (json && json.methodsCalled) {
        TCService.fireAfterEvent('CartUpdated', json, jQForm);
      }
      return json;
    };

    TCService.error = function (json, settings) {
      if (json.responseText.indexOf('isn\'t licensed') > -1) {
        alert(json.responseText);
      }
      if (settings && settings.errorfn) {
        settings.errorfn(json);
      }
      TCService.fireEvent('cartUpdateError', json);
    };

    TCService.fixDates = function (jsonOrder) {
      /// <summary>
      /// Fixes the three dates on an order Json object. (CreatedDate, ModifiedDate and OrderDate)
      /// </summary>
      /// <param name="jsonOrder">The Json order</param>
      if (jsonOrder) {
        if (jsonOrder.dateCreated) {
          jsonOrder.dateCreated = TCService.parseJsonDate(jsonOrder.dateCreated);
        }
        if (jsonOrder.dateModified) {
          jsonOrder.dateModified = TCService.parseJsonDate(jsonOrder.dateModified);
        }
        if (jsonOrder.dateOrder) {
          jsonOrder.dateOrder = TCService.parseJsonDate(jsonOrder.dateOrder);
        }
      }
    };

    TCService.parseJsonDate = function (date) {
      /// <summary>
      /// Parses a Json date to a Date object
      /// </summary>
      /// <param name='date'>The date string to parse</param>
      /// <returns>A Date object</returns>
      if (date && date.indexOf('/Date(') === -1) {
        return date;
      }
      return new Date(parseInt(date.substr(6), 10));
    };

    TCService.fixSettings = function (settings) {
      var defaultSettings = {};
      return jQuery.extend({}, defaultSettings, settings);
    };

    /*
    ** EVENT HANDLING
    */
    TCService.bind = function (event, fn) {

      if (!TCService.eventSubscribers[event]) {
        TCService.eventSubscribers[event] = [];
      }
      TCService.eventSubscribers[event].push(fn);

    };

    TCService.fireBeforeEvent = function (event, arg1, arg2) {
      TCService.fireEvent('before' + event, arg1, arg2);
    };

    TCService.fireAfterEvent = function (event, arg1, arg2, arg3) {
      TCService.fireEvent('after' + event, arg1, arg2, arg3);
    };

    TCService.fireEvent = function (event, arg1, arg2, arg3) {
      if (TCService.eventSubscribers[event]) {
        var i;
        for (i = 0; i < TCService.eventSubscribers[event].length; i++) {
          TCService.eventSubscribers[event][i](arg1, arg2, arg3);
        }
      }
    };

    /*************************************************************
    - AJAX FORM POSTING
    *************************************************************/
    TCService.allMethods = [
      'GetCurrentOrder',
      'RemoveCurrentOrder',
      'GetCurrentFinalizedOrder',
      'AddOrUpdateOrderLine',
      'RemoveAllOrderLines',
      'RemoveOrderLine',
      'AddOrUpdateOrderProperties',
      'GetCurrency',
      'GetCurrentCurrency',
      'GetCurrencies',
      'SetCurrentCurrency',
      'GetCountry',
      'GetCountries',
      'GetCurrentPaymentCountry',
      'GetCurrentShippingCountry',
      'SetCurrentPaymentCountry',
      'SetCurrentShippingCountry',
      'GetCountryRegion',
      'GetCountryRegions',
      'GetCurrentPaymentCountryRegion',
      'GetCurrentShippingCountryRegion',
      'SetCurrentPaymentCountryRegion',
      'SetCurrentShippingCountryRegion',
      'GetShippingMethods',
      'GetShippingMethod',
      'GetCurrentShippingMethod',
      'SetCurrentShippingMethod',
      'GetPaymentMethods',
      'GetPaymentMethod',
      'GetCurrentPaymentMethod',
      'SetCurrentPaymentMethod',
      'GeneratePaymentForm',
      'FormatPrice',
      'RenderTemplateFile'
    ];
    
    TCService.postForm = function (form) {
      // submit the form 
      // prepare Options Object 
      var settings = {
        success: function (json, success, response, jQForm) { TCService.success(json, success, response, jQForm, null); },
        error: function (json) { TCService.error(json, null); },
        beforeSubmit: function (formData, jQForm, settings) {
          var calledMethods = [],
              i = 0;
          for (i = 0; i < formData.length; i++) {
            var item = formData[i],
                name = item.name,
                methodIndex = TCService.allMethods.contains(name);

            //Make sure the filed is a valid method
            if (methodIndex > -1) {
              //Make sure that the method has not yet been called
              if (calledMethods.contains(name) < 0) {
                TCService.fireBeforeEvent(name, formData, jQForm);
                calledMethods.push(name);
              }
            }
          }

          TCService.fireBeforeEvent('CartUpdated', formData, jQForm);
        },
        dataType: 'json',
        type: 'POST',
        data: { isJavaScript: true }
      };

      jQuery(form).ajaxSubmit(settings);
    };

    jQuery(function () {
      jQuery('body').on('submit', '.ajaxForm', function () {
        TCService.postForm(this);
        // return false to prevent normal browser submit and page navigation 
        return false;
      });
    });

  };

  var tcs = new TCService();
})();




/*************************************************************
- UTILS
*************************************************************/
//Returns true if the array contains the object
Array.prototype.contains = function (obj) {
  var i = this.length;
  while (i--) {
    if (this[i] === obj) {
      return i;
    }
  }
  return -1;
};