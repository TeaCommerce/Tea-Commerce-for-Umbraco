angular.module('TeaCommerce').controller('OrderLineAmountAwardController', function ($scope) {
  $scope.editMode = false;

  if (!$scope.award.settings) {
    $scope.award.settings = { type: 0, nodeId: null, usePercentage: 'true', percentage: '', amounts: {} };
  }
  
  findContentName($scope.award.settings.nodeId, function (name, breadcrump) {
    $scope.contentName = name;
    $scope.contentBreadcrump = breadcrump;
    $scope.$apply();
  });

  $scope.openContentPicker = function () {
    openContentPicker(function (nodeId, name, breadcrump) {
      $scope.award.settings.nodeId = nodeId;
      $scope.contentName = name;
      $scope.contentBreadcrump = breadcrump;
      $scope.$apply();
    });
  };

  $scope.deleteValue = function () {
    $scope.award.settings.nodeId = null;
    $scope.contentName = $scope.contentBreadcrump = '';
  };

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