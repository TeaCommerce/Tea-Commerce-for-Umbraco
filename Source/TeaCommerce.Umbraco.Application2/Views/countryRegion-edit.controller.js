angular.module("umbraco").controller("TeaCommerce.CountryRegion.EditController", function ($scope, $routeParams, $http, notificationsService) {

	$scope.contentTabs = { tabs: [{ id: 1, label: "Common" }] };
	var storeId = "",
		countryRegionId = "";
	var isStoreId = true;
	  
	var ids = $routeParams.id.split('-');
	storeId = ids[0];
	countryRegionId = ids[1];
	
	$http.get('backoffice/teacommerce2/CountryRegion/Get?storeId='+storeId+'&countryRegionId='+countryRegionId).success(function (countryRegionDtm) {

    $scope.countryRegion = countryRegionDtm;
	
	});

	$http.get('backoffice/teacommerce2/Country/GetShippingMethods?storeId='+storeId).success(function (shippingMethodsDtm) {

    $scope.shippingMethods = shippingMethodsDtm;
	
	});
	
	$http.get('backoffice/teacommerce2/Country/GetPaymentMethods?storeId='+storeId).success(function (paymentMethodsDtm) {

    $scope.paymentMethods = paymentMethodsDtm;
	
	});
	
	$scope.save = function() {
		console.log('SAVE');
	$http.post('backoffice/teacommerce2/CountryRegion/Save', $scope.countryRegion).success(function (data) {
		notificationsService.success("Saved!", "Your changes were saved!");
		$scope.contentForm.$dirty=false;
	  }).error(function(e){

	  });
	}

});