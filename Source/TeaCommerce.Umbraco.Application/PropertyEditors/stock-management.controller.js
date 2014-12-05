angular.module("umbraco").controller("TeaCommerce.StockManagementController", function ($scope, $http, $routeParams) {
  $scope.ui = {
    creating: false
  };
  
  if ($routeParams.create) {
    $scope.ui.creating = true;
    //TODO: kan vi løse så umbraco sender os det nye id i formsubmitting?
  } else {
    $http.get('backoffice/teacommerce/products/getstock/?pageId=' + $routeParams.id).success(function (data) {
      $scope.stock = data;
    });
  }

  if (!$routeParams.create) {
    $scope.$on("formSubmitting", function () {
      var data = {
        sku: jQuery('#sku').val(),
        value: $scope.stock.Value
      };
      $http.post('backoffice/teacommerce/products/poststock?pageId=' + $routeParams.id, data);
    });
  }
});