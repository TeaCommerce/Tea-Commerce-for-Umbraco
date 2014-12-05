angular.module('TeaCommerce').controller('ProductRuleController', function ($scope) {
  $scope.editMode = false;

  if (!$scope.rule.settings) {
    $scope.rule.settings = { nodeId: null };
  }

  findContentName($scope.rule.settings.nodeId, function (name, breadcrump) {
    $scope.contentName = name;
    $scope.contentBreadcrump = breadcrump;
    $scope.$apply();
  });
  
  $scope.openContentPicker = function () {
    openContentPicker(function (nodeId, name, breadcrump) {
      $scope.rule.settings.nodeId = nodeId;
      $scope.contentName = name;
      $scope.contentBreadcrump = breadcrump;
      $scope.$apply();
    });
  };

  $scope.deleteValue = function() {
    $scope.rule.settings.nodeId = null;
    $scope.contentName = $scope.contentBreadcrump = '';
  };

  $scope.save = function() {
    $scope.editMode = false;
    $scope.saveRule($scope.rule);
  };
});