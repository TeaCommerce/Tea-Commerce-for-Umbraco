angular.module('umbraco').controller('TeaCommerce.VariantEditor.Controller', [

'$scope',
'$rootScope',
'$timeout',
'$routeParams',
'contentResource',
'editorState',
'assetsService',
'entityResource',
function ($scope, $rootScope, $timeout, $routeParams, contentResource, editorState, assetsService, entityResource) {
  $scope.ui = {
    creating: false
  };

  $scope.settings = {
    xPath: $scope.model.config.xpathOrNode.query,
    contentId: $scope.model.config.xpathOrNode.contentId,
    showXPath: $scope.model.config.xpathOrNode.showXPath,
    docTypeAlias: $scope.model.config.variantDocumentType,
    hideLabel: $scope.model.config.hideLabel == 1,
    forceEditorToChooseAllVariantGroups: $scope.model.config.forceEditorToChooseAllVariantGroups == 1,
    extraListInformation: splitString($scope.model.config.extraListInformation, ','),
    variantGroupDocumentTypes: splitString($scope.model.config.variantGroupDocumentTypes, ','),
    variantGroupNodeName: splitString($scope.model.config.variantGroupNodeName, ','),

  };

  //Set defaults
  if (!$scope.model.value) {
    $scope.model.value = {};
  }
  if (!$scope.model.value.variants || $scope.model.value.variants.length === 0) {
    $scope.model.value.variants = [];
    $scope.variantGroupsOpen = true;
  }
  $scope.variants = [];
  $scope.model.hideLabel = $scope.settings.hideLabel;
  $scope.variantGroupsClosedCookieName = 'teaCommerceVariantGroupsDisabledDefault' + editorState.current.contentTypeAlias;

  if (!$scope.model.value.variantGroupsOpen) {
    $scope.model.value.variantGroupsOpen = Cookies.getJSON($scope.variantGroupsClosedCookieName);
    if (!$scope.model.value.variantGroupsOpen) {
      $scope.model.value.variantGroupsOpen = {};
    }
  }


  if ($routeParams.create) {
    $scope.ui.creating = true;

  } else {

  }


  if (!$routeParams.create) {
    $scope.$on('formSubmitting', function () {

    });
  }

  $scope.addVariantOptions = function () {
    var checkedGroups = $scope.getCheckedVariantGroups(),
        indices = [];

    for (checkedGroupKey in checkedGroups) {
      indices.push(checkedGroupKey);
    }

    var combinations = findCombinations({
      mapping: checkedGroups.slice(0),
      indices: indices
    });

    //Use combinations
    for (var i = 0; i < combinations.length; i++) {
      var combination = combinations[i],
          variant = findVariantByCombination(combination);

      if (!variant) {
        //Create new and add to data
        loadVariant({
          id: guid(),
          docTypeAlias: $scope.settings.docTypeAlias,
          properties: {},
          combination: combination
        });
      }
    }
    setVariantColumns();
    $scope.variantGroupsOpen = false;
    $scope.setCheckedStatusOnAllVariantGroups(false);
  };

  $scope.combinationChanged = function (e) {
    //variant, variantGroup
    var oldCombination = {},
        variantGroupItem = null;

    //Copy old combination
    oldCombination.id = e.variant.combinationDictionary[e.variantGroup.id].id;
    oldCombination.name = e.variant.combinationDictionary[e.variantGroup.id].id;
    oldCombination.groupId = e.variant.combinationDictionary[e.variantGroup.id].id;
    oldCombination.groupName = e.variant.combinationDictionary[e.variantGroup.id].id;

    for (var i = 0; i < e.variantGroup.items.length; i++) {
      var item = e.variantGroup.items[i];
      if (item.id === oldCombination.id) {
        variantGroupItem = item;
        break;
      }
    }
    if (variantGroupItem) {


      var combination = { name: variantGroupItem.name, id: variantGroupItem.id, groupName: e.variantGroup.name, groupId: e.variantGroup.id },
          combinationExists = false,
          combinationIndex = -1;
      e.variant.combinationDictionary[e.variantGroup.id] = combination;
      for (var i = 0; i < e.variant.combination.length; i++) {
        var variantGroupItem = e.variant.combination[i];
        if (variantGroupItem && variantGroupItem.groupId === combination.groupId) {
          oldCombination = e.variant.combination[i];
          e.variant.combination[i] = combination;
          combinationIndex = i;

          combinationExists = true;
          break;
        }
      }
      if (!combinationExists) {
        e.variant.combination.push(combination);
        combinationIndex = e.variant.combination.length - 1;
      }

      var matchingVariant = findVariantByCombination(e.variant.combination, e.variant.id);
      e.variant.combinationExists = matchingVariant;

      e.variant.combinationName = '';
      for (var i = 0; i < e.variant.combination.length; i++) {
        var variantGroupItem = e.variant.combination[i];
        if (e.variant.combinationName) {
          e.variant.combinationName += ' | ';
        }
        e.variant.combinationName += variantGroupItem.groupName + ': ' + variantGroupItem.name;
      }


    }
    setVariantColumns();
  };

  $scope.setOrder = function (variantColumnId) {
    if (!$scope.orderByColumn) {
      $scope.orderByColumn = {
        column: variantColumnId,
        reverse: true
      };
    } else {
      $scope.orderByColumn.column = variantColumnId;
      $scope.orderByColumn.reverse = $scope.orderByColumn.column === variantColumnId ? !$scope.orderByColumn.reverse : false;
    }

  };

  $scope.getOrderByValue = function (variant) {
    return variant.combinationDictionary[$scope.orderByColumn.column] ? variant.combinationDictionary[$scope.orderByColumn.column].name : '';
  };

  $scope.getCheckedVariantGroups = function () {
    var checkedGroups = [];
    //Find all checked variant groups and items
    if ($scope.variantGroups) {
      for (var i = 0; i < $scope.variantGroups.length; i++) {
        var variantGroup = $scope.variantGroups[i];
        for (var y = 0; y < variantGroup.items.length; y++) {
          var item = variantGroup.items[y];
          if (item.checked) {
            if (!checkedGroups[variantGroup.id]) {
              checkedGroups[variantGroup.id] = [];
            }
            checkedGroups[variantGroup.id].push({ name: item.name, id: item.id, groupName: variantGroup.name, groupId: variantGroup.id });
          }
        }
      }
    }
    return checkedGroups;
  };

  $scope.setCheckedStatusOnAllVariantGroups = function (checked) {
    if ($scope.variantGroups) {
      for (var i = 0; i < $scope.variantGroups.length; i++) {
        var variantGroup = $scope.variantGroups[i];
        for (var y = 0; y < variantGroup.items.length; y++) {
          variantGroup.items[y].checked = checked;
        }
      }
    }
  };

  $scope.isVariantGroupsChecked = function () {
    var checkedGroups = $scope.getCheckedVariantGroups();
    var validated = checkedGroups && checkedGroups.length > 0;
    if (validated && $scope.settings.forceEditorToChooseAllVariantGroups) {

      for (var i = 0; i < $scope.variantGroups.length; i++) {
        var variantGroup = $scope.variantGroups[i];
        var found = false;
        for (var key in checkedGroups) {
          found = key == variantGroup.id;
          if (found) {
            break;
          }
        }
        if (!found) {
          validated = false;
          break;
        }
      }
    }

    return validated;
  };

  $scope.setVariantGroupClosedState = function (variantGroup) {

    $scope.model.value.variantGroupsOpen[variantGroup.id] = !$scope.model.value.variantGroupsOpen[variantGroup.id];

    var expireDate = new Date();
    expireDate.setFullYear(expireDate.getFullYear() + 6);
    Cookies.set($scope.variantGroupsClosedCookieName, $scope.model.value.variantGroupsOpen, { expires: expireDate });
  };

  $scope.openOrCloseAllVariants = function () {
    var allAreClosed = true;
    for (var i = 0; i < $scope.variants.length; i++) {
      var variant = $scope.variants[i];
      if (variant.open) {
        allAreClosed = false;
        break;
      }
    }

    for (var i = 0; i < $scope.variants.length; i++) {
      $scope.variants[i].open = allAreClosed;
    }
    return false;
  };

  $scope.openOrCloseAllVariantGroups = function (force) {
    var allAreClosed = force === undefined || force === 'open';
    if (!force) {
      allAreClosed = $scope.checkIfAllVariantGroupsAreClosed();
    }

    for (var i = 0; i < $scope.variantGroups.length; i++) {
      $scope.model.value.variantGroupsOpen[$scope.variantGroups[i].id] = allAreClosed;
    }

    var expireDate = new Date();
    expireDate.setFullYear(expireDate.getFullYear() + 6);
    Cookies.set($scope.variantGroupsClosedCookieName, $scope.model.value.variantGroupsOpen, { expires: expireDate });
    return false;
  };

  $scope.checkIfAllVariantGroupsAreClosed = function () {
    var allAreClosed = true;
    if ($scope.variantGroups) {
      for (var i = 0; i < $scope.variantGroups.length; i++) {
        var variantGroup = $scope.variantGroups[i];
        if ($scope.model.value.variantGroupsOpen[variantGroup.id]) {
          allAreClosed = false;
          break;
        }
      }
    }

    return allAreClosed;
  };

  $scope.checkIfAllVariantAreClosed = function () {
    var allAreClosed = true;
    if ($scope.variantGroups) {
      for (var i = 0; i < $scope.variants.length; i++) {
        var variant = $scope.variants[i];
        if (variant.open) {
          allAreClosed = false;
          break;
        }
      }
    }

    return allAreClosed;
  };

  $scope.selectOrDeselectAllVariantTypes = function () {

    var allAreAreUnChecked = $scope.checkIfAllVariantTypesAreUnchecked();

    for (var i = 0; i < $scope.variantGroups.length; i++) {
      var variantGroup = $scope.variantGroups[i];
      for (var y = 0; y < variantGroup.items.length; y++) {
        variantGroup.items[y].checked = allAreAreUnChecked;
      }
    }

    $scope.openOrCloseAllVariantGroups('open');
    return false;
  };

  $scope.checkIfAllVariantTypesAreUnchecked = function () {

    var allAreAreUnChecked = true;
    if ($scope.variantGroups) {
      for (var i = 0; i < $scope.variantGroups.length; i++) {
        var variantGroup = $scope.variantGroups[i];
        for (var y = 0; y < variantGroup.items.length; y++) {
          var item = variantGroup.items[y];
          if (item.checked) {
            allAreAreUnChecked = false;
            break;
          }
        }
        if (allAreAreUnChecked) {
          break;
        }
      }
    }

    return allAreAreUnChecked;
  };

  $scope.deleteVariant = function (variant) {

    var index = -1;
    for (var i = 0; i < $scope.variants.length; i++) {
      if ($scope.variants[i].id === variant.id) {
        index = i;
        break;
      }
    }
    if (index > -1) {
      removeFromArray($scope.variants, index);
    }
  };

  $scope.deleteAllVariants = function () {

    if (confirm('Are you sure you want to delete all variants?')) {
      $scope.variants = [];
    }
  };

  $scope.validateVariants = function () {
    var variantGroups = [],
        isValid = { holesInVariants: false, duplicatesFound: false };

    for (var i = 0; i < $scope.variants.length; i++) {
      var variant = $scope.variants[i];
      variant.validation = {};
      if (variantGroups.length === 0) {
        for (var y = 0; y < $scope.variantColumns.length; y++) {
          var variantColumn = $scope.variantColumns[y];
          if (!variantColumn.extra) {
            variantGroups.push(variantColumn.id);
          }
        }
      }
      var combinationsCount = 0;
      for (var key in variant.combinationDictionary) {
        if (variant.combinationDictionary[key].id > 0) {
          combinationsCount++;
        }
      }
      variant.validation.holesInVariants = variantGroups.length !== combinationsCount;
      isValid.holesInVariants = isValid.holesInVariants || variant.validation.holesInVariants;
      if (!isValid.holesInVariants) {
        var isFound = false;
        for (var y = 0; y < variantGroups.lenght; y++) {
          for (var groupId in variant.combinationDictionary) {
            if (variant.combinationDictionary[groupId].id > 0) {
              if (variantGroups[i] === groupId) {
                isFound = true;
                break;
              }
            }
          }
          if (!isFound) {
            isValid.holesInVariants = true;
            variant.validation.holesInVariants = true;
            break;
          }
        }
      }

      var matchingVariant = findVariantByCombination(variant.combination, variant.id);
      variant.validation.duplicatesFound = !!matchingVariant;
      isValid.duplicatesFound = isValid.duplicatesFound || variant.validation.duplicatesFound;
    }

    $scope.hasError = !isValid.holesInVariants && !isValid.duplicatesFound;

    return isValid;
  };


  function findCombinations(o) {
    var current = [],
        combinations = [];

    function step() {
      if (current.length === o.indices.length) {
        combinations.push(current.slice(0));
        return;
      }

      o.mapping[o.indices[current.length]].forEach(function (x) {
        current.push(x);
        step();
        current.pop();
      });
    }

    step(); // start recursion
    return combinations;
  }

  function findVariantByCombination(combination, excludeVariantId) {
    var rtnVariant = null,
        numberOfVariantTypes = 0;
    for (var i = 0; i < combination.length; i++) {
      if (combination[i].id) {
        numberOfVariantTypes++;
      }
    }

    for (var y = 0; y < $scope.variants.length; y++) {
      var variant = $scope.variants[y],
          numberOfVariantTypesTmp = 0,
          combinationFound = true;

      for (var i = 0; i < variant.combination.length; i++) {
        if (variant.combination[i].id) {
          numberOfVariantTypesTmp++;
        }
      }

      if (variant.id === excludeVariantId) {
        continue;
      }

      if (numberOfVariantTypes === numberOfVariantTypesTmp) {
        for (var i = 0; i < combination.length; i++) {
          var variantGroupItem = combination[i],
            variantGroupItemFound = false;

          for (var x = 0; x < variant.combination.length; x++) {
            var tmpCombination = variant.combination[x];

            if (tmpCombination.id === variantGroupItem.id) {
              variantGroupItemFound = true;
              break;
            }
          }

          if (!variantGroupItemFound) {
            combinationFound = false;
            break;
          }

        }
      } else {
        combinationFound = false;
      }

      if (combinationFound) {
        rtnVariant = variant;
        break;
      }

    }

    return rtnVariant;
  }

  function loadVariants() {

    for (var i = 0; i < $scope.model.value.variants.length; i++) {
      var modelVariant = $scope.model.value.variants[i];

      loadVariant(modelVariant);

    };

  }

  function loadVariant(modelVariant) {

    //Create UI variant
    var stockProp = null,
        skuProp = null,
        variant = {
          properties: [],
          id: modelVariant.id,
          combination: modelVariant.combination
        };

    for (var p = 0; p < $scope.doctypeProperties.length; p++) {
      var prop = $scope.doctypeProperties[p],
        //Copy the property settings
          variantProp = {
            'label': prop.label,
            'description': prop.description,
            'view': prop.view,
            'config': prop.config,
            'hideLabel': prop.hideLabel,
            'validation': prop.validation,
            'id': prop.id,
            'value': prop.value,
            'alias': prop.alias,
            'editor': prop.editor
          };

      if (modelVariant.properties[prop.alias]) {
        //Merge current value
        variantProp.value = modelVariant.properties[prop.alias];
      }
      //Add property to UI variant
      variant.properties.push(variantProp);
      
      if (prop.editor === 'TeaCommerce.StockManagement') {
        variantProp.variantId = variant.id;
        stockProp = variantProp;
      }

      if (prop.alias === 'sku') {
        skuProp = variantProp;
      }
    }

    if (skuProp && stockProp) {
      stockProp.skuProp = skuProp;
    }

    variant.combinationDictionary = {};
    variant.combinationName = '';
    for (var i = 0; i < variant.combination.length; i++) {
      var variantGroupItem = variant.combination[i];
      if (variant.combinationName) {
        variant.combinationName += ' | ';
      }
      variant.combinationDictionary[variantGroupItem.groupId] = variantGroupItem;
      variant.combinationName += variantGroupItem.groupName + ': ' + variantGroupItem.name;
    }

    for (var i = 0; i < $scope.variantGroups.length; i++) {

      var variantGroup = $scope.variantGroups[i],
          found = false;

      for (combinationVariantGroupId in variant.combinationDictionary) {
        if (combinationVariantGroupId == variantGroup.id) {
          found = true;
          break;
        }
      }
      if (!found) {
        variant.combinationDictionary[variantGroup.id] = { id: 0 };
      } 
    }

    //Push to UI array
    $scope.variants.push(variant);

  };

  if (!$routeParams.create) {
    $scope.$on("formSubmitting", function () {
      saveVariantsToValue();
    });
  }

  function saveVariantsToValue() {
    var modelVariants = [];
    for (var i = 0; i < $scope.variants.length; i++) {
      var variant = $scope.variants[i],
          modelVariant = { id: variant.id, documentTypeAlias: variant.documentTypeAlias, validation: variant.validation, properties: {}, combination: variant.combination };

      for (var p = 0; p < variant.properties.length; p++) {
        var prop = variant.properties[p];
        modelVariant.properties[prop.alias] = prop.value;
      }

      modelVariants.push(modelVariant);

    };
    $scope.model.value.variants = modelVariants;

  };

  function setVariantColumns() {
    $scope.variantColumns = [];

    for (var i = 0; i < $scope.variantGroups.length; i++) {
      var variantGroup = $scope.variantGroups[i],
          found = false;

      for (var y = 0; y < $scope.variants.length; y++) {
        var variant = $scope.variants[y];

        if (variant.combinationDictionary[variantGroup.id] && variant.combinationDictionary[variantGroup.id].id) {
          found = true;
          break;
        }
      }
      if (found) {
        $scope.variantColumns.push(variantGroup);
      }
    }

    $scope.variantColumns.sort(function (a, b) {
      return a.name < b.name ? -1 : 1;
    });


    for (var i = 0; i < $scope.extraPropertiesForColumn.length; i++) {
      var prop = $scope.extraPropertiesForColumn[i];
      $scope.variantColumns.push({ name: prop.label, id: prop.alias, extra: true });
    }

    for (var i = 0; i < $scope.variants.length; i++) {
      var variant = $scope.variants[i];
      variant.columns = {};
      variant.documentTypeAlias = $scope.settings.docTypeAlias;

      for (var y = 0; y < $scope.variantColumns.length; y++) {
        var column = $scope.variantColumns[y];
        if (column.extra) {
          //Extra property
          for (var x = 0; x < variant.properties.length; x++) {
            var prop = variant.properties[x];
            if (prop.alias === column.id) {
              variant.columns[column.id] = prop;
              break;
            }
          }
        } else {
          //Variant group
          if (variant.combinationDictionary[column.id] && variant.combinationDictionary[column.id].id) {
            variant.columns[column.id] = variant.combinationDictionary[column.id].name;
          } else {
            variant.columns[column.id] = '';
          }
        }
      }

    }


  }

  function readyScaffolding() {

    if ($scope.settings.showXPath && $scope.settings.xPath) {

      entityResource.getByQuery($scope.settings.xPath, $routeParams.id, "Document").then(function (ent) {
        getChildren(ent);
      });

    } else if (!$scope.settings.showXPath && $scope.settings.contentId) {
      entityResource.getById($scope.settings.contentId, "Document").then(function (ent) {
        getChildren(ent);
      });
    }


  };

  function readyVariantScaffolding() {
    $scope.doctypeProperties = [];
    $scope.extraPropertiesForColumn = [];
    if ($scope.settings.docTypeAlias) {
      contentResource.getScaffold(-20, $scope.settings.docTypeAlias).then(function (data) {
        // Remove the last tab
        $scope.doctype = data.tabs.pop();

        for (var t = 0; t < data.tabs.length; t++) {
          var tab = data.tabs[t];
          for (var p = 0; p < tab.properties.length; p++) {
            var prop = tab.properties[p];
            $scope.doctypeProperties.push(prop);
            for (var i = 0; i < $scope.settings.extraListInformation.length; i++) {
              if ($scope.settings.extraListInformation[i] === prop.alias) {
                $scope.extraPropertiesForColumn.push(prop);
                $scope.extraPropertiesForColumn.sort(function (a, b) {
                  return a.name < b.name ? -1 : 1;
                });
                break;
              }
            }
          }
        }
        loadVariants();

        setVariantColumns();
      });
    } else {
      loadVariants();

      setVariantColumns();
    }
  }

  function checkDocumentTypeFromFilter(content) {
    var rtn = $scope.settings.variantGroupDocumentTypes.length === 0 || ($scope.settings.variantGroupDocumentTypes.length === 1 && $scope.settings.variantGroupDocumentTypes[0] === '');

    for (var i = 0; i < $scope.settings.variantGroupDocumentTypes.length; i++) {
      if ($scope.settings.variantGroupDocumentTypes[i] === content.contentTypeAlias) {
        rtn = true;
        break;
      }
    }

    return rtn;
  }

  function checkNodeNameFromFilter(content) {
    var rtn = $scope.settings.variantGroupNodeName.length === 0 || ($scope.settings.variantGroupNodeName.length === 1 && $scope.settings.variantGroupNodeName[0] === '');

    for (var i = 0; i < $scope.settings.variantGroupNodeName.length; i++) {
      if ($scope.settings.variantGroupNodeName[i] === content.name) {
        rtn = true;
        break;
      }
    }

    return rtn;
  }

  function getChildren(ent) {
    contentResource.getChildren(ent.id).then(function (data) {
      $scope.variantGroups = [];

      var variantGroups = jQuery(data.items),
          endCount = variantGroups.length,
          answersReciewed = 0;
      variantGroups.each(function (i) {
        var variantGroup = this;
        variantGroup.label = variantGroup.name;
        if (checkDocumentTypeFromFilter(variantGroup) && checkNodeNameFromFilter(variantGroup)) {
          contentResource.getChildren(variantGroup.id, $scope.options).then(function (data) {
            variantGroup.items = data.items;
            var savedOpenOptions = $scope.model.value.variantGroupsOpen[variantGroup.id];
            variantGroup.open = savedOpenOptions === undefined ? $scope.variantGroupsOpen : savedOpenOptions;
            $scope.variantGroups.push(variantGroup);
            answersReciewed++;
            if (answersReciewed === endCount) {
              readyVariantScaffolding();
            }
          });
        }

      });
    });


  }

  $scope.setOrder(0);
  readyScaffolding();

  function guid() {
    function s4() {
      return Math.floor((1 + Math.random()) * 0x10000)
        .toString(16)
        .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
  }

  // Array Remove - By John Resig (MIT Licensed)
  function removeFromArray(arr, from, to) {
    var rest = arr.slice((to || from) + 1 || arr.length);
    arr.length = from < 0 ? arr.length + from : from;
    return arr.push.apply(arr, rest);
  };

  // Array Remove - By John Resig (MIT Licensed)
  function splitString(str, delimiter) {
    if (!str) {
      return [];
    }
    var arr = str.split(',');
    for (var i = 0; i < arr.length; i++) {
      arr[i] = arr[i].trim();
    }

    return arr;
  };

}]);
