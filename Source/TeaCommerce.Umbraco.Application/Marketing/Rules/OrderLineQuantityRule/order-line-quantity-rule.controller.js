angular.module('TeaCommerce').controller('OrderLineQuantityRuleController', function ($scope) {
  $scope.editMode = false;

  if (!$scope.rule.settings) {
    $scope.rule.settings = { compareMode: 0, quantity: "", accumulate: false };
  }

  $scope.save = function () {
    $scope.editMode = false;
    $scope.saveRule($scope.rule);
  };
});