angular.module("umbraco").controller("TeaCommerce.StorePickerController", function ($scope, $http) {

  $http.get('backoffice/teacommerce/stores/getall').success(function (data) {
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
    $scope.stores = data;

    if ($scope.model.value && !entityFound) {
      $http.get('backoffice/teacommerce/stores/get?storeId=' + $scope.model.value).success(function (data) {
        data.name = '* ' + data.name;
        $scope.stores.push(data);
      });
    }
    
  });
});