angular.module('TeaCommerce').controller('PaymentMethodRuleController', function ($scope, $http) {
  $scope.editMode = false;

  if (!$scope.rule.settings) {
    $scope.rule.settings = { paymentMethodId: null };
  }

  $http.get('/umbraco/backoffice/teacommerce/paymentmethods/getall/?storeId=' + $scope.campaign.storeId).success(function (paymentMethods) {

    if ($scope.rule.settings.paymentMethodId) {
      var found = false;
      for (var i = 0; i < paymentMethods.length; i++) {
        if (paymentMethods[i].id == $scope.rule.settings.paymentMethodId) {
          found = true;
          break;
        }
      }

      if (!found) {
        $http.get('/umbraco/backoffice/teacommerce/paymentmethods/get/?storeId=' + $scope.campaign.storeId + '&paymentMethodId=' + $scope.rule.settings.paymentMethodId).success(function (paymentMethod) {
          paymentMethod.name = '*' + paymentMethod.name;
          paymentMethods.push(paymentMethod);

          $scope.paymentMethods = paymentMethods;
        });
      } else {
        $scope.paymentMethods = paymentMethods;
      }
    } else {
      $scope.paymentMethods = paymentMethods;
    }
  });

  $scope.save = function() {
    $scope.editMode = false;
    $scope.saveRule($scope.rule);
  };
});