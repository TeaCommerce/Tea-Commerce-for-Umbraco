angular.module('TeaCommerce', []).controller("CampaignController", function ($scope, $http) {
  
  $http.get('/umbraco/backoffice/teacommerce/campaigns/getmanifestsforrules/').success(function (data) {
    $scope.ruleManifests = {};
    for (var i = 0; i < data.length; i++) {
      var ruleManifest = data[i];
      $scope.ruleManifests[ruleManifest.alias] = ruleManifest;
    }

    $http.get('/umbraco/backoffice/teacommerce/campaigns/getmanifestsforawards/').success(function (data1) {
      $scope.awardManifests = {};
      for (var i = 0; i < data1.length; i++) {
        var awardManifest = data1[i];
        $scope.awardManifests[awardManifest.alias] = awardManifest;
      }
      
      $http.get('/umbraco/backoffice/teacommerce/campaigns/get/?storeId=' + getParameterByName('storeId') + '&campaignId=' + getParameterByName('id')).success(function (data2) {
        $scope.campaign = data2;
      });
    });
  });
  
  $scope.addRuleGroup = function () {
    $http.post('/umbraco/backoffice/teacommerce/campaigns/addrulegroup', {
      storeId: $scope.campaign.storeId,
      campaignId: $scope.campaign.id
    }).success(function (data) {
      $scope.campaign.ruleGroups.push(data);
    });
  };

  $scope.addRule = function (ruleGroup) {
    $http.post('/umbraco/backoffice/teacommerce/campaigns/addrule', {
      storeId: $scope.campaign.storeId,
      campaignId: $scope.campaign.id,
      ruleGroupId: ruleGroup.id,
      ruleAlias: ruleGroup.selectedRuleManifest.alias
    }).success(function (data) {
      ruleGroup.rules.push(data);
    });
  };

  $scope.saveRule = function (rule) {
    $http.post('/umbraco/backoffice/teacommerce/campaigns/saverule', {
      storeId: $scope.campaign.storeId,
      campaignId: $scope.campaign.id,
      ruleId: rule.id,
      settings: JSON.stringify(rule.settings)
    });
  };
  
  $scope.deleteRule = function (rule) {
    $http.post('/umbraco/backoffice/teacommerce/campaigns/removerule', {
      storeId: $scope.campaign.storeId,
      campaignId: $scope.campaign.id,
      ruleId: rule.id
    }).success(function () {
      for (var i = 0; i < $scope.campaign.ruleGroups.length; i++) {
        var ruleGroup = $scope.campaign.ruleGroups[i];
        var foundAtIndex = -1;
        
        for (var j = 0; j < ruleGroup.rules.length; j++) {
          var rule2 = ruleGroup.rules[j];
          if (rule2.id == rule.id) {
            foundAtIndex = j;
            break;
          }
        }
        
        if (foundAtIndex > -1) {
          ruleGroup.rules.splice(foundAtIndex, 1);
          if (ruleGroup.rules.length == 0) {
            $scope.campaign.ruleGroups.splice(i, 1);
          }
          break;
        }
      }
    });
  };
  
  $scope.addAward = function () {
    $http.post('/umbraco/backoffice/teacommerce/campaigns/addaward', {
      storeId: $scope.campaign.storeId,
      campaignId: $scope.campaign.id,
      awardAlias: $scope.selectedAwardManifest.alias
    }).success(function (data) {
      $scope.campaign.awards.push(data);
    });
  };

  $scope.saveAward = function (award) {
    $http.post('/umbraco/backoffice/teacommerce/campaigns/saveaward', {
      storeId: $scope.campaign.storeId,
      campaignId: $scope.campaign.id,
      awardId: award.id,
      settings: JSON.stringify(award.settings)
    });
  };

  $scope.deleteAward = function (award) {
    $http.post('/umbraco/backoffice/teacommerce/campaigns/removeaward', {
      storeId: $scope.campaign.storeId,
      campaignId: $scope.campaign.id,
      awardId: award.id
    }).success(function () {
      for (var i = 0; i < $scope.campaign.awards.length; i++) {
        var award2 = $scope.campaign.awards[i];
        if (award2.id == award.id) {
          $scope.campaign.awards.splice(i, 1);
          break;
        }
      }
    });
  };

});

function getParameterByName(name) {
  name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
  var regex = new RegExp("[\\?&]" + name + "=([^&#]*)", 'i'),
      results = regex.exec(location.search);
  return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}