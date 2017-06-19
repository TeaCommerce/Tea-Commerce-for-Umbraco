angular.module("umbraco").controller("TeaCommerce.ShippingMethod.EditController", function ($scope, $routeParams, $http, notificationsService, dialogService, entityResource) {

	$scope.contentTabs = { tabs: [{ id: 1, label: "Common" }, { id: 2, label: "Available in these countries" }] };
	var storeId = "",
		shippingMethodId = "";
	var isStoreId = true;
	  
	var ids = $routeParams.id.split('-');
	storeId = ids[0];
	shippingMethodId = ids[1];
	
	$http.get('backoffice/teacommerce2/ShippingMethod/Get?storeId='+storeId+'&shippingMethodId='+shippingMethodId).success(function (shippingMethodDtm) {

    $scope.shippingMethod = shippingMethodDtm;
	
	if ($scope.shippingMethod.imageIdentifier != null){
		entityResource.getById($scope.shippingMethod.imageIdentifier, "Media").then(function (item) {
			$scope.node = item;
		});
	}
	
	});
	
	$http.get('backoffice/teacommerce2/PaymentMethod/GetVatGroups?storeId='+storeId).success(function (vatGroupsDtm) {

    $scope.vatGroups = vatGroupsDtm;
	
	});
	
	$scope.openMediaPicker = function() {
		dialogService.mediaPicker({ callback: populatePicture,  onlyImages: true});
	}
	
	function populatePicture(item) {
		if(item != null && item != '' && item.contentTypeAlias === "Image"){
			$scope.node = item;
			$scope.shippingMethod.imageIdentifier = item.id;
			$scope.thumbnail = item.thumbnail;
			$scope.width = item.originalWidth;
			$scope.height = item.originalHeight;
		}
	}
	
	$scope.removePicture = function() {
		$scope.node = undefined;
		$scope.shippingMethod.imageIdentifier = "";
	}
	
	$scope.save = function() {
		console.log('SAVE');
	$http.post('backoffice/teacommerce2/ShippingMethod/Save', $scope.shippingMethod).success(function (data) {
		notificationsService.success("Saved!", "Your changes were saved!");
		$scope.contentForm.$dirty=false;
	  }).error(function(e){

	  });
	}

});