angular.module("umbraco").controller("TeaCommerce.VatGroup.EditController", function ($scope, $routeParams, $http, notificationsService) {

	$scope.contentTabs = { tabs: [{ id: 1, label: "Common" }] };
	var storeId = "",
		vatGroupId = "";
	var isStoreId = true;
	  
	var ids = $routeParams.id.split('-');
	storeId = ids[0];
	vatGroupId = ids[1];
	
	$http.get('backoffice/teacommerce2/VatGroup/Get?storeId='+storeId+'&vatGroupId='+vatGroupId).success(function (vatGroupDtm) {

    $scope.vatGroup = vatGroupDtm;
	
	});
	
	$scope.save = function() {
		console.log('SAVE');
	$http.post('backoffice/teacommerce2/VatGroup/Save', $scope.vatGroup).success(function (data) {
		notificationsService.success("Saved!", "Your changes were saved!");
		$scope.contentForm.$dirty=false;
	  }).error(function(e){

	  });
	}

});