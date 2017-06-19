angular.module("umbraco").controller("TeaCommerce.PaymentMethod.EditController", function ($scope, $routeParams, $http, notificationsService, dialogService, entityResource) {

	$scope.contentTabs = { tabs: [{ id: 1, label: "Common" }, { id: 2, label: "Available in these countries" }] };
	var storeId = "",
		paymentMethodId = "";
	var isStoreId = true;
	  
	var ids = $routeParams.id.split('-');
	storeId = ids[0];
	paymentMethodId = ids[1];
	$scope.settings = {};
	$scope.settings.toggleStatus = true;
	
	$http.get('backoffice/teacommerce2/PaymentMethod/Get?storeId='+storeId+'&paymentMethodId='+paymentMethodId).success(function (paymentMethodDtm) {

    $scope.paymentMethod = paymentMethodDtm;
	$scope.optionToggled();
	
	if ($scope.paymentMethod.imageIdentifier != null){
		entityResource.getById($scope.paymentMethod.imageIdentifier, "Media").then(function (item) {
			$scope.node = item;
		});
	}
	
	});
	
	$http.get('backoffice/teacommerce2/PaymentMethod/GetVatGroups?storeId='+storeId).success(function (vatGroupsDtm) {

    $scope.vatGroups = vatGroupsDtm;
	$scope.optionToggled();
	
	});
	
	$http.get('backoffice/teacommerce2/PaymentMethod/GetCurrencies?storeId='+storeId).success(function (currenciesDtm) {

    $scope.currencies = currenciesDtm;
	$scope.optionToggled();
	
	});
	
	$http.get('backoffice/teacommerce2/PaymentMethod/GetCountries?storeId='+storeId).success(function (countriesDtm) {

    $scope.countries = countriesDtm;
	$scope.optionToggled();
	
	});
	
	$http.get('backoffice/teacommerce2/PaymentMethod/GetCountryRegions?storeId='+storeId).success(function (countryRegionsDtm) {

    $scope.countryRegions = countryRegionsDtm;
	$scope.optionToggled();
	
	});
	
	$scope.openMediaPicker = function() {
		dialogService.mediaPicker({ callback: populatePicture,  onlyImages: true});
	}
	
	function populatePicture(item) {
		if(item != null && item != '' && item.contentTypeAlias === "Image"){
			$scope.node = item;
			$scope.paymentMethod.imageIdentifier = item.id;
			$scope.thumbnail = item.thumbnail;
			$scope.width = item.originalWidth;
			$scope.height = item.originalHeight;
		}
	}
	
	$scope.removePicture = function() {
		$scope.node = undefined;
		$scope.paymentMethod.imageIdentifier = "";
	}
	
	$scope.toggleSelection = function toggleSelection(countryId) {
    var idx = $scope.paymentMethod.allowedInFollowingCountries.indexOf(countryId);

    // Is currently selected
    if (idx > -1) {
      $scope.paymentMethod.allowedInFollowingCountries.splice(idx, 1);
    }

    // Is newly selected
    else {
      $scope.paymentMethod.allowedInFollowingCountries.push(countryId);
    }
  }
	
	$scope.toggleSelectionAll = function toggleSelectionAll() {
		if(!$scope.settings.toggleStatus) {
			angular.forEach($scope.countries, function (country) {
				var idx = $scope.paymentMethod.allowedInFollowingCountries.indexOf(country.id);

				if (idx > -1) {
					$scope.paymentMethod.allowedInFollowingCountries.splice(idx, 1);
				}
				
			});
		} else if ($scope.settings.toggleStatus) {
			angular.forEach($scope.countries, function (country) {
				var idx = $scope.paymentMethod.allowedInFollowingCountries.indexOf(country.id);

				if (idx == -1) {
				  $scope.currency.allowedInFollowingCountries.push(country.id);
				}
				
			});
		}
		
	}
	
	$scope.optionToggled = function() {
		if($scope.paymentMethod && $scope.countries){
		$scope.isAllSelected = $scope.countries.every( function(country) { 
			var idx = $scope.paymentMethod.allowedInFollowingCountries.indexOf(country.id); 
			if(idx > -1) {
				return true; 
			}
			else {
				return false;
			}
		} );
		$scope.settings.toggleStatus = $scope.isAllSelected;
		}
	}
	
	$scope.save = function() {
		console.log('SAVE');
	$http.post('backoffice/teacommerce2/PaymentMethod/Save', $scope.paymentMethod).success(function (data) {
		notificationsService.success("Saved!", "Your changes were saved!");
		$scope.contentForm.$dirty=false;
	  }).error(function(e){

	  });
	}
});