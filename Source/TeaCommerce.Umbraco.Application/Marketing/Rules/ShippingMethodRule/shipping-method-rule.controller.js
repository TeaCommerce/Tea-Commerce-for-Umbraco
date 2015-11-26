angular.module('TeaCommerce').controller('ShippingMethodRuleController', function ($scope, $http) {
  $scope.editMode = false;

  if (!$scope.rule.settings) {
    $scope.rule.settings = { shippingMethodId: null };
  }

  $http.get('/umbraco/backoffice/teacommerce/shippingmethods/getall/?storeId=' + $scope.campaign.storeId).success(function (shippingMethods) {

    if ($scope.rule.settings.shippingMethodId) {
      var found = false;
      for (var i = 0; i < shippingMethods.length; i++) {
        if (shippingMethods[i].id == $scope.rule.settings.shippingMethodId) {
          found = true;
          break;
        }
      }

      if (!found) {
        $http.get('/umbraco/backoffice/teacommerce/shippingmethods/get/?storeId=' + $scope.campaign.storeId + '&shippingMethodId=' + $scope.rule.settings.shippingMethodId).success(function (shippingMethod) {
          shippingMethod.name = '*' + shippingMethod.name;
          shippingMethods.push(shippingMethod);

          $scope.shippingMethods = shippingMethods;
        });
      } else {
        $scope.shippingMethods = shippingMethods;
      }
    } else {
      $scope.shippingMethods = shippingMethods;
    }
  });

  $scope.save = function() {
    $scope.editMode = false;
    $scope.saveRule($scope.rule);
  };
});