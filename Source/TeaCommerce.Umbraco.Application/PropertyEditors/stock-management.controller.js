angular.module("umbraco").controller("TeaCommerce.StockManagementController", function ($scope, $http, $routeParams) {
  $scope.ui = {
    creating: false
  };

  var variantId = $scope.model.variantId ? '_' + $scope.model.variantId : '';



  if ($routeParams.create) {
    $scope.ui.creating = true;
  } else {
    if (!$scope.model.isVariantProp || ($scope.model.skuProp && $scope.model.skuProp.value)) {
      $http.get('backoffice/teacommerce/products/getstock/?productIdentifier=' + $routeParams.id + variantId).success(function (data) {
        $scope.stock = data;
      });
    }
  }

  if (!$routeParams.create) {
    $scope.$on("formSubmitting", function () {     

      if (!$scope.model.isVariantProp || ($scope.model.skuProp && $scope.model.skuProp.value)) {
        //Find the products "sku" field
        var productSkuField = null;
        jQuery('.umb-pane #sku').each(function () {
          skuField = jQuery(this);
          if (!skuField.closest('.teaCommerceVariantEditor')[0]) {
            productSkuField = skuField;
          }
        });

        var data = {
          sku: $scope.model.isVariantProp ? $scope.model.skuProp.value : (!!productSkuField && productSkuField[0] ? productSkuField.val() : ''),
          value: $scope.stock ? $scope.stock.Value : null
        };
        $http.post('backoffice/teacommerce/products/poststock?productIdentifier=' + $routeParams.id + variantId, data);
      }
    });
  }
});