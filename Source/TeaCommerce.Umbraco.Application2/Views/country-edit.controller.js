angular.module("umbraco").controller("TeaCommerce.Country.EditController", function ($scope, $routeParams, $http, notificationsService) {

	$scope.contentTabs = { tabs: [{ id: 1, label: "Common" }] };
	var storeId = "",
		countryId = "";
	var isStoreId = true;
	  
	var ids = $routeParams.id.split('-');
	storeId = ids[0];
	countryId = ids[1];
	
	$http.get('backoffice/teacommerce2/Country/Get?storeId='+storeId+'&countryId='+countryId).success(function (countryDtm) {

    $scope.country = countryDtm;
	
	});
	
	$http.get('backoffice/teacommerce2/Country/GetCurrencies?storeId='+storeId).success(function (currenciesDtm) {

    $scope.currencies = currenciesDtm;
	
	});
	
	$http.get('backoffice/teacommerce2/Country/GetShippingMethods?storeId='+storeId).success(function (shippingMethodsDtm) {

    $scope.shippingMethods = shippingMethodsDtm;
	
	});
	
	$http.get('backoffice/teacommerce2/Country/GetPaymentMethods?storeId='+storeId).success(function (paymentMethodsDtm) {

    $scope.paymentMethods = paymentMethodsDtm;
	
	});
	
	$scope.save = function() {
		console.log('SAVE');
	$http.post('backoffice/teacommerce2/Country/Save', $scope.country).success(function (data) {
		notificationsService.success("Saved!", "Your changes were saved!");
		$scope.contentForm.$dirty=false;
	  }).error(function(e){

	  });
	}

});

