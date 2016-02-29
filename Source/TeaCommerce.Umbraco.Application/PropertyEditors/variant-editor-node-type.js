angular.module("umbraco").controller("TeaCommerce.PropertyEditors.variantEditorNodeType", [
'$scope',
'$rootScope',
'$timeout',
'$routeParams',
'contentResource',
'entityResource',
'dialogService',
    function ($scope, $rootScope, $timeout, $routeParams, contentResource, entityResource, dialogService) {

      if (!$scope.model.value) {
        $scope.model.value = {};
      }

      var config = {
        multiPicker: false,
        callback: function (selectedNode) {
          if (selectedNode) {
            $scope.model.value.contentId = selectedNode.id;
            $scope.model.value.selectedNode = selectedNode;
            $scope.model.value.showChosenContent = true;
            console.log(selectedNode);
          } else {
            $scope.clearContent();
          }
        }
      };


      $scope.openContentPicker = function () {
        var ds = dialogService.contentPicker(config);
      }

      //Clear chosen content
      $scope.clearContent = function () {
        $scope.model.value.contentId = "";
        $scope.model.value.selectedNode = "";
        $scope.model.value.showChosenContent = false;
      };


    }]
);