angular.module("umbraco").controller("TeaCommerce.Currency.EditController", function ($scope, $routeParams, $http, notificationsService) {
	
	$scope.contentTabs = { tabs: [{ id: 1, label: "Common" }, { id: 2, label: "Available in these countries" }] };
	var storeId = "",
		currencyId = "";
	var isStoreId = true;
	  
	var ids = $routeParams.id.split('-');
	storeId = ids[0];
	currencyId = ids[1];
	$scope.settings = {};
	$scope.settings.toggleStatus = true;
	
	$http.get('backoffice/teacommerce2/Currency/Get?storeId='+storeId+'&currencyId='+currencyId).success(function (currencyDtm) {

		$scope.currency = currencyDtm;
		$scope.optionToggled();
		if($scope.currency.symbol != null && $scope.currency.symbol != ""){
			$scope.settings.chkSpecificSymbol = true;
		} else {
			$scope.settings.chkSpecificSymbol = false;
		}
		
		
	});
	
	$http.get('backoffice/teacommerce2/Currency/GetCountries?storeId='+storeId).success(function (countriesDtm) {
		
		$scope.countries = countriesDtm;
		$scope.optionToggled();
	
	});
	
	$http.get('backoffice/teacommerce2/Currency/GetCultures?storeId='+storeId).success(function (culturesDtm) {
		
		$scope.cultures = culturesDtm;
		$scope.optionToggled();
	
	});
	
	$scope.toggleSelection = function toggleSelection(countryId) {
    var idx = $scope.currency.allowedInFollowingCountries.indexOf(countryId);

    // Is currently selected
    if (idx > -1) {
      $scope.currency.allowedInFollowingCountries.splice(idx, 1);
    }

    // Is newly selected
    else {
      $scope.currency.allowedInFollowingCountries.push(countryId);
    }
  }
	
	$scope.toggleSelectionAll = function toggleSelectionAll() {
		if(!$scope.settings.toggleStatus) {
			angular.forEach($scope.countries, function (country) {
				if(country.defaultCurrencyId != $scope.currency.id){
					var idx = $scope.currency.allowedInFollowingCountries.indexOf(country.id);

					if (idx > -1) {
					  $scope.currency.allowedInFollowingCountries.splice(idx, 1);
					}
				}
				
			});
		} else if ($scope.settings.toggleStatus) {
			angular.forEach($scope.countries, function (country) {
				var idx = $scope.currency.allowedInFollowingCountries.indexOf(country.id);

				if (idx == -1) {
				  $scope.currency.allowedInFollowingCountries.push(country.id);
				}
				
			});
		}
		
	}
	
	$scope.optionToggled = function() {
		if($scope.currency && $scope.countries){
		$scope.isAllSelected = $scope.countries.every( function(country) { 
			var idx = $scope.currency.allowedInFollowingCountries.indexOf(country.id); 
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
		if(!$scope.settings.chkSpecificSymbol){
			$scope.currency.symbol = "";
			$scope.currency.symbolPlacement = "left";
		}
		$http.post('backoffice/teacommerce2/Currency/Save', $scope.currency).success(function (data) {
			notificationsService.success("Saved!", "Your changes were saved!");
			$scope.contentForm.$dirty=false;
		  }).error(function(e){
			  
		  });
	}

});