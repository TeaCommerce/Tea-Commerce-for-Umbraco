angular.module('TeaCommerce').controller('PropertyRuleController', function ($scope) {
  $scope.editMode = false;

  if (!$scope.rule.settings) {
    $scope.rule.settings = { propertyAlias: "", isOrderProperty: true, compareMode: 0, value: "" };
  }

  $scope.save = function () {
    $scope.editMode = false;
    $scope.saveRule($scope.rule);
  };
});