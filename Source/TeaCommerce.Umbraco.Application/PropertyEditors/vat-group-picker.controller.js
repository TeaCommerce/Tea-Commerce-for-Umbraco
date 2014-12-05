angular.module("umbraco").controller("TeaCommerce.VatGroupController", function ($scope, $http, $routeParams) {

  $http.get('backoffice/teacommerce/vatgroups/getall?pageId=' + $routeParams.id).success(function (data) {
    var entityFound = false;
    for (var i = 0; i < data.length; i++) {
      //Must convert id to string because Umbraco saves and parse it back as a string
      var entity = data[i];
      entity.id = '' + entity.id;

      if (entity.id == $scope.model.value) {
        entityFound = true;
        break;
      }
    }
    $scope.vatGroups = data;
    
    if ($scope.model.value && !entityFound) {
      $http.get('backoffice/teacommerce/vatgroups/get?pageId=' + $routeParams.id + '&vatGroupId=' + $scope.model.value).success(function (data) {
        data.name = '* ' + data.name;
        $scope.vatGroups.push(data);
      });
    }

  });
});