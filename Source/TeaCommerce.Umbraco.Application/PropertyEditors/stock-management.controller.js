angular.module("umbraco").controller("TeaCommerce.StockManagementController", function ($scope, $http, $routeParams) {
  $scope.ui = {
    creating: false
  };
  
  var variantId = $scope.model.variantId ? '_' + $scope.model.variantId : '';

  if ($routeParams.create) {
    $scope.ui.creating = true;
  } else {
    $http.get('backoffice/teacommerce/products/getstock/?productIdentifier=' + $routeParams.id + variantId).success(function (data) {
      $scope.stock = data;
    });
  }

  if (!$routeParams.create) {
    $scope.$on("formSubmitting", function () {
      var data = {
        //TODO: Get sku field name from store Eller få sku'en via model ligesom varianten
        sku: jQuery('#sku').val(),
        value: $scope.stock ? $scope.stock.Value : null
      };
      $http.post('backoffice/teacommerce/products/poststock?productIdentifier=' + $routeParams.id + variantId, data);
    });
  }
});