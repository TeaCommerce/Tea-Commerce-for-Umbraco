angular.module('TeaCommerce').controller('OrderAmountAwardController', function ($scope) {
  $scope.editMode = false;

  if (!$scope.award.settings) {
    $scope.award.settings = { type: 0, usePercentage: 'true', percentage: '', amounts: {} };
  }

  $scope.currencies = TC.getCurrencies({ storeId: $scope.campaign.storeId, onlyAllowed: false });

  for (var i = 0; i < $scope.currencies.length; i++) {
    var currency = $scope.currencies[i];
    for (var key in $scope.award.settings.amounts) {
      if (key == currency.id) {
        currency.amount = $scope.award.settings.amounts[key];
        break;
      }
    }
  }

  $scope.save = function () {
    $scope.editMode = false;
    for (var i = 0; i < $scope.currencies.length; i++) {
      var currency = $scope.currencies[i];
      $scope.award.settings.amounts[currency.id] = currency.amount;
    }
    $scope.saveAward($scope.award);
  };
});