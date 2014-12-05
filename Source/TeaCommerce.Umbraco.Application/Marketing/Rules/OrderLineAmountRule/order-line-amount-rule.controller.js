angular.module('TeaCommerce').controller('OrderLineAmountRuleController', function ($scope) {
  $scope.editMode = false;

  if (!$scope.rule.settings) {
    $scope.rule.settings = { type: 0, compareMode: 0, amounts: {}, accumulate: false };
  }

  $scope.currencies = TC.getCurrencies({ storeId: $scope.campaign.storeId, onlyAllowed: false });

  for (var i = 0; i < $scope.currencies.length; i++) {
    var currency = $scope.currencies[i];
    for (var key in $scope.rule.settings.amounts) {
      if (key == currency.id) {
        currency.amount = $scope.rule.settings.amounts[key];
        break;
      }
    }
  }

  $scope.save = function () {
    $scope.editMode = false;
    for (var i = 0; i < $scope.currencies.length; i++) {
      var currency = $scope.currencies[i];
      $scope.rule.settings.amounts[currency.id] = currency.amount;
    }
    $scope.saveRule($scope.rule);
  };
});