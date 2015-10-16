angular.module('TeaCommerce').controller('DiscountCodeRuleController', function ($scope, $http) {
  $scope.editMode = false;
  $scope.numberOfDiscountCodes = 0;
  $scope.numberOfUnusedDiscountCodes = 0;

  $scope.resetManual = function () {
    $scope.manualSettings = { codes: '', maxUses: 1 };
  };

  $scope.resetGenerate = function () {
    $scope.generateSettings = { numberToGenerate: 1, maxUses: 1, length: 6, prefix: '', postfix: '' };
  };

  $scope.getNumber = function (number) {
    if (number == '') {
      number = 0;
    }
    return new Array(parseInt(number));
  };

  $scope.view = 'manual';
  $http.get('/umbraco/backoffice/teacommerce/discountcodes/getcount/?storeId=' + $scope.campaign.storeId + '&ruleId=' + $scope.rule.id).success(function (data) {
    $scope.numberOfDiscountCodes = data.total;
    $scope.numberOfUnusedDiscountCodes = data.unused;
  });

  $scope.resetManual();
  $scope.resetGenerate();

  $scope.manual = function () {
    $http.post('/umbraco/backoffice/teacommerce/discountcodes/add', {
      storeId: $scope.campaign.storeId,
      ruleId: $scope.rule.id,
      maxUses: $scope.manualSettings.maxUses,
      codes: $scope.manualSettings.codes
    }).success(function (data) {
      $scope.numberOfDiscountCodes += data.DiscountCodes.length;
      $scope.numberOfUnusedDiscountCodes += data.DiscountCodes.length;

      var discountCodesAlreadyExistsStringList = [];
      data.DiscountCodesAlreadyExists.forEach(function (data) {
        discountCodesAlreadyExistsStringList.push(data['Code'] + '\n');
      });

      if (data.DiscountCodesAlreadyExists.length > 0) {
        alert("Already exists:\n" + discountCodesAlreadyExistsStringList);
      }

    });

    $scope.resetManual();
    $scope.editMode = false;
  };

  $scope.generate = function () {
    $http.post('/umbraco/backoffice/teacommerce/discountcodes/generate', {
      storeId: $scope.campaign.storeId,
      ruleId: $scope.rule.id,
      numberToGenerate: $scope.generateSettings.numberToGenerate,
      maxUses: $scope.generateSettings.maxUses,
      length: $scope.generateSettings.length,
      prefix: $scope.generateSettings.prefix,
      postfix: $scope.generateSettings.postfix
    }).success(function (data) {
      $scope.numberOfDiscountCodes += data.length;
      $scope.numberOfUnusedDiscountCodes += data.length;
    });

    $scope.resetGenerate();
    $scope.editMode = false;
  };

  $scope.download = function () {
    window.open('/umbraco/backoffice/teacommerce/discountcodes/getdownload/?storeId=' + $scope.campaign.storeId + '&campaignId=' + $scope.campaign.id + '&ruleId=' + $scope.rule.id, '_blank', '');
  };
});