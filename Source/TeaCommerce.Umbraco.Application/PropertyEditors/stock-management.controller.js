angular.module("umbraco").controller("TeaCommerce.StockManagementController", function ($scope, $http, $routeParams) {
  $scope.ui = {
    creating: false
  };
  
  var variantId = $scope.model.variantId ? '_' + $scope.model.variantId : '';



  if ($routeParams.create) {
    $scope.ui.creating = true;
  } else {
    if ($scope.model.skuProp !== '') {
      $http.get('backoffice/teacommerce/products/getstock/?productIdentifier=' + $routeParams.id + variantId).success(function (data) {
        $scope.stock = data;
      });
    }
  }

  if (!$routeParams.create) {
    $scope.$on("formSubmitting", function () {
      var productSkuField = null;
      jQuery('.umb-pane #sku').each(function () {
        skuField = jQuery(this);
        if (!skuField.closest('.teaCommerceVariantEditor')[0]) {
          productSkuField = skuField;
        }
      });

      if ($scope.model.skuProp !== '') {
        //teaCommerceVariantEditor

        var data = {
          sku: $scope.model.skuProp !== undefined ? $scope.model.skuProp : (!productSkuField && productSkuField[0] ? productSkuField.val() : ''),
          value: $scope.stock ? $scope.stock.Value : null
        };
        $http.post('backoffice/teacommerce/products/poststock?productIdentifier=' + $routeParams.id + variantId, data);
      }
    });
  }
});