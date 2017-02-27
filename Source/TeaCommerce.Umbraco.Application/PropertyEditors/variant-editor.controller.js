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
    xPath: $scope.model.config.xpathOrNode ? $scope.model.config.xpathOrNode.query : '',//Xpath to variant group parent
    contentId: $scope.model.config.xpathOrNode ? $scope.model.config.xpathOrNode.contentId : '',//Content id of variant group parent
    showXPath: $scope.model.config.xpathOrNode ? $scope.model.config.xpathOrNode.showXPath : '',//Use xpath instead op content id
    docTypeAlias: $scope.model.config.variantDocumentType,//Doc type alias for the variant properties
    hideLabel: $scope.model.config.hideLabel == 1,//Hide Umbraco label
    forceEditorToChooseAllVariantGroups: $scope.model.config.forceEditorToChooseAllVariantGroups == 1,//Validation rule will force editor to choose from all variant groups
    extraListInformation: splitString($scope.model.config.extraListInformation, ','),//Extra property aliases to display in variant table
    variantGroupDocumentTypes: splitString($scope.model.config.variantGroupDocumentTypes, ','),//Specific variant group doc types to use in this particular editor
    variantGroupNodeName: splitString($scope.model.config.variantGroupNodeName, ','),//Specific variant group node names to use in this particular editor
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

  //Set which variant groups should be open if it has not been saved on this product node
  if (!$scope.model.value.variantGroupsOpen) {
    //Get settings from cookie
    $scope.model.value.variantGroupsOpen = Cookies.getJSON($scope.variantGroupsClosedCookieName);
    if (!$scope.model.value.variantGroupsOpen) {
      //None will be opened
      $scope.model.value.variantGroupsOpen = {};
    }
  }


  if ($routeParams.create) {
    $scope.ui.creating = true;
  }

  //Will create and add variant option from the UI selection made by the editor
  //The already existing combinations wil not be created againg
  $scope.addVariantOptions = function () {
    //Get all checked variant groups as a dictionary
    var checkedGroups = $scope.getCheckedVariantGroups(),
        indices = [];

    //Add all group id's to indices list for later use
    for (var checkedGroupKey in checkedGroups) {
      indices.push(checkedGroupKey);
    }

    //Find all variant type combinations from the checked groups and their indices
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

    //Run though all variants and update the information needed to create the columns of the variants table
    setVariantColumns();
    //Close all variant groups
    $scope.variantGroupsOpen = false;
    //Uncheck all variant types
    $scope.setCheckedStatusOnAllVariantGroups(false);
  };

  //Event hook when combinations have changed on a variant
  $scope.combinationChanged = function (e) {
    //variant, variantGroup
    var oldCombination = {},
        variantGroupItem = null;

    //Copy values to old combination
    oldCombination.id = e.variant.combinationDictionary[e.variantGroup.id].id;
    oldCombination.name = e.variant.combinationDictionary[e.variantGroup.id].id;
    oldCombination.groupId = e.variant.combinationDictionary[e.variantGroup.id].id;
    oldCombination.groupName = e.variant.combinationDictionary[e.variantGroup.id].id;

    //Run through all items on the variant group and find the new item
    for (var i = 0; i < e.variantGroup.items.length; i++) {
      var item = e.variantGroup.items[i];
      if (item.id === oldCombination.id) {
        variantGroupItem = item;
        break;
      }
    }

    //If an item was found start doing stuff
    if (variantGroupItem) {
      //Create the new combination
      var combination = { name: variantGroupItem.name, id: variantGroupItem.id, groupName: e.variantGroup.name, groupId: e.variantGroup.id },
          combinationExists = false,
          combinationIndex = -1;
      //Add the combination to the variant
      e.variant.combinationDictionary[e.variantGroup.id] = combination;

      //Run through existing combinations and update if on is found with the variant group id
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
        //The combination did not so we add it to the variant
        e.variant.combination.push(combination);
        combinationIndex = e.variant.combination.length - 1;
      }

      //See if the combination already exists on another variant
      var matchingVariant = findVariantByCombination(e.variant.combination, e.variant.id);
      e.variant.combinationExists = !!matchingVariant;

      e.variant.combinationName = '';
      for (var i = 0; i < e.variant.combination.length; i++) {
        var variantGroupItem = e.variant.combination[i];
        if (e.variant.combinationName) {
          e.variant.combinationName += ' | ';
        }
        e.variant.combinationName += variantGroupItem.groupName + ': ' + variantGroupItem.name;
      }
      
    }

    //Run though all variants and update the information needed to create the columns of the variants table
    setVariantColumns();
  };

  //Will order variant table by the specified column
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

  //Get the order by value for a given variant depending on the current ordering settings
  $scope.getOrderByValue = function (variant) {
    return variant.combinationDictionary[$scope.orderByColumn.column] ? variant.combinationDictionary[$scope.orderByColumn.column].name : '';
  };

  //Gets all checked variant groups as a dictionary
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

  //Will check or uncheck all variant types
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

  //Check if all variant types are currently checked
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

  //Open or close a specific variant group and save the state in a cookie
  $scope.setVariantGroupClosedState = function (variantGroup) {

    $scope.model.value.variantGroupsOpen[variantGroup.id] = !$scope.model.value.variantGroupsOpen[variantGroup.id];

    var expireDate = new Date();
    expireDate.setFullYear(expireDate.getFullYear() + 6);
    Cookies.set($scope.variantGroupsClosedCookieName, $scope.model.value.variantGroupsOpen, { expires: expireDate });
  };

  //Will open all variants in table if they are all currently closed.
  //If some or all are open they will be closed
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

  //Will open all variants groups in table if they are all currently closed.
  //If some or all are open they will be closed
  //The state will be saved in a cookie
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

  //Check if all variant groups types are currently closed
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

  //Check if all variant types are currently closed
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

  //Will check all variants types if they are all currently unchecked.
  //If some or all are checked they will be unchecked
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

  //Check if all variant types are currently unchecked
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

  //Will delete a variant without warning
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

  //Will delete all variants after a confirm box have been displayed
  $scope.deleteAllVariants = function () {

    if (confirm('Are you sure you want to delete all variants?')) {
      $scope.variants = [];
    }
  };

  //Will run validation on all variants
  //Each variant must have no unselected variant types comparing to all other variants
  //There must be no duplicates to a specific variant
  $scope.validateVariants = function () {
    var variantGroups = [],
        isValid = { holesInVariants: false, duplicatesFound: false };

    //Loop though all variants
    for (var i = 0; i < $scope.variants.length; i++) {
      var variant = $scope.variants[i];

      //Reset variant validation status
      variant.validation = {};

      //First variant will trigger a creation of a base set of variant groups that must be filled out
      if (variantGroups.length === 0) {
        for (var y = 0; y < $scope.variantColumns.length; y++) {
          var variantColumn = $scope.variantColumns[y];
          if (!variantColumn.extra) {
            variantGroups.push(variantColumn.id);
          }
        }
      }

      //Count the combination making up this variant
      var combinationsCount = 0;
      for (var key in variant.combinationDictionary) {
        //If the combination have an variant type id it's regarded as selected on this variant
        if (variant.combinationDictionary[key].id > 0) {
          combinationsCount++;
        }
      }

      //Check number of combinations. If there's too few on this variant it does not validate.
      variant.validation.holesInVariants = variantGroups.length !== combinationsCount;
      isValid.holesInVariants = isValid.holesInVariants || variant.validation.holesInVariants;

      //Chek for duplicate variants
      var matchingVariant = findVariantByCombination(variant.combination, variant.id);
      variant.validation.duplicatesFound = !!matchingVariant;
      isValid.duplicatesFound = isValid.duplicatesFound || variant.validation.duplicatesFound;
    }

    $scope.hasError = !isValid.holesInVariants && !isValid.duplicatesFound;

    return isValid;
  };

  //Will find all combinations in a given set of data
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

  //Will find a single variant by it's combination of variant types
  //If there are more than one variant matching the combination only the first one will be returned
  function findVariantByCombination(combination, excludeVariantId) {
    var rtnVariant = null,
        numberOfVariantTypes = 0;
    //Check the number of combinations
    for (var i = 0; i < combination.length; i++) {
      if (combination[i].id) {
        numberOfVariantTypes++;
      }
    }

    //Loop through all variants
    for (var y = 0; y < $scope.variants.length; y++) {
      var variant = $scope.variants[y],
          numberOfVariantTypesTmp = 0,
          combinationFound = true;

      //If we found a variant that should be excluded from the set we continue to the next variant
      if (variant.id === excludeVariantId) {
        continue;
      }

      //Find the variants number of combinations
      for (var i = 0; i < variant.combination.length; i++) {
        if (variant.combination[i].id) {
          numberOfVariantTypesTmp++;
        }
      }
      
      //If the number of combinations match we will need to check that it's the right combinations
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
            //Not this variant
            combinationFound = false;
            break;
          }

        }
      } else {
        //Not this variant
        combinationFound = false;
      }

      if (combinationFound) {
        //We found it. Let's break and get on with our lives
        rtnVariant = variant;
        break;
      }

    }

    return rtnVariant;
  }

  //Load all variants from the saved data
  function loadVariants() {

    for (var i = 0; i < $scope.model.value.variants.length; i++) {
      var modelVariant = $scope.model.value.variants[i];

      loadVariant(modelVariant);

    };

  }

  //Load a single variant
  //Properties and property meta data will be added using the variant document type
  //Combinations will be found and enriched with extra data
  function loadVariant(modelVariant) {

    //Create UI variant
    var stockProp = null,
        skuProp = null,
        variant = {
          properties: [],
          id: modelVariant.id,
          combination: modelVariant.combination
        };

    //Run through the properties on the document type and add each of them to the variant
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
      
      //The stock property is special
      if (prop.editor === 'TeaCommerce.StockManagement') {
        //We add extra information the stock property editor will need
        variantProp.variantId = variant.id;
        stockProp = variantProp;
        variantProp.isVariantProp = true;
      }

      //The sku property is also special
      if (prop.alias === 'sku') {
        skuProp = variantProp;
      }
    }

    if (skuProp && stockProp) {
      //Add the sku to the stock property
      //If the variant have a custom sku it can also have a custom stock
      stockProp.skuProp = skuProp;
    }

    variant.combinationDictionary = {};
    variant.combinationName = '';
    //Run through combinations for this variant and collect extra data
    for (var i = 0; i < variant.combination.length; i++) {
      var variantGroupItem = variant.combination[i];
      if (variant.combinationName) {
        variant.combinationName += ' | ';
      }
      variant.combinationDictionary[variantGroupItem.groupId] = variantGroupItem;
      variant.combinationName += variantGroupItem.groupName + ': ' + variantGroupItem.name;
    }

    //Make sure there are no holes in the variant combination dictionary
    //Holes would make the angular code toss around nullpointer exceptions
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
    //On the form submitting event we need to prepare the variant data for save
    $scope.$on("formSubmitting", function () {
      saveVariantsToValue();
    });
  }

  //This will prepare variant data for save and move them to model.value
  //We save a much more simple version of the data than we use to display the angular UI
  function saveVariantsToValue() {
    var entityVariants = [];
    //Run through all variants
    for (var i = 0; i < $scope.variants.length; i++) {
      var variant = $scope.variants[i],
          //Create an entity of the variant data
          entityVariant = { id: variant.id, documentTypeAlias: variant.documentTypeAlias, validation: variant.validation, properties: {}, combination: variant.combination };

      //Run through variant properties and add them to the entity
      for (var p = 0; p < variant.properties.length; p++) {
        var prop = variant.properties[p];
        entityVariant.properties[prop.alias] = prop.value;
      }

      //Push the entity to the result set
      entityVariants.push(entityVariant);

    };
    //Add result to model value. It is now ready to save
    $scope.model.value.variants = entityVariants;

  };

  //Will run though all variants and update the information needed to create the columns of the variants table
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

    //Order columns by name
    $scope.variantColumns.sort(function (a, b) {
      return a.name < b.name ? -1 : 1;
    });

    //Add columns for extra variant properties
    for (var i = 0; i < $scope.extraPropertiesForColumn.length; i++) {
      var prop = $scope.extraPropertiesForColumn[i];
      $scope.variantColumns.push({ name: prop.label, id: prop.alias, extra: true });
    }

    //Loop through variants and add column information on each of them
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

  //Ready the templates for all variant groups, variant types and variants
  //The templates come from the Umbraco document types
  function readyScaffolding() {

    //Either use the xpath or the content id from the settings to find the root of the variant groups
    if ($scope.settings.showXPath && $scope.settings.xPath) {
      //Find the variant group root using xpath
      entityResource.getByQuery($scope.settings.xPath, $routeParams.id, "Document").then(function (ent) {
        //Get all variant groups
        getChildren(ent);
      });

    } else if (!$scope.settings.showXPath && $scope.settings.contentId) {
      //Find the variant group root using the content id
      entityResource.getById($scope.settings.contentId, "Document").then(function (ent) {
        //Get all variant groups
        getChildren(ent);
      });
    }


  };

  //Will fetch the document type for the variants and start loading the variants
  function readyVariantScaffolding() {
    $scope.doctypeProperties = [];
    $scope.extraPropertiesForColumn = [];
    if ($scope.settings.docTypeAlias) {
      //There's a document type from the settings to load
      contentResource.getScaffold(-20, $scope.settings.docTypeAlias).then(function (data) {
        // Remove the last tab
        $scope.doctype = data.tabs.pop();

        //Run through all tabs and get all properties for the variants
        for (var t = 0; t < data.tabs.length; t++) {
          var tab = data.tabs[t];
          for (var p = 0; p < tab.properties.length; p++) {
            var prop = tab.properties[p];
            $scope.doctypeProperties.push(prop);
            //Check if this property should be added to the list of extra columns in the variants table
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
        //Load all variants
        loadVariants();

        //Run though all variants and update the information needed to create the columns of the variants table
        setVariantColumns();
      });
    } else {
      //Load all variants
      loadVariants();

      //Run though all variants and update the information needed to create the columns of the variants table
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

  //Get all variant groups
  function getChildren(ent) {
    //Using the umbraco content resource we fetch all variant groups
    contentResource.getChildren(ent.id).then(function (data) {
      $scope.variantGroups = [];

      var variantGroups = jQuery(data.items),
          legalVariantGroups = [],
          answersReciewed = 0;

      //Check how many variant groups that is legal
      variantGroups.each(function (i) {
        var variantGroup = this;
        //Run the variant group through the filters from the settings
        if (checkDocumentTypeFromFilter(variantGroup) && checkNodeNameFromFilter(variantGroup)) {
          legalVariantGroups.push(variantGroup);
        }
      });
      var endCount = legalVariantGroups.length;

      //Run through each variant groups
      variantGroups.each(function (i) {
        var variantGroup = this;
        variantGroup.label = variantGroup.name;
        
          //Get all variant types for the variant group
          contentResource.getChildren(variantGroup.id, $scope.options).then(function (data) {
            //The items is the variant types
            variantGroup.items = data.items;
            //Set the open state of the variant group
            var savedOpenOptions = $scope.model.value.variantGroupsOpen[variantGroup.id];
            variantGroup.open = savedOpenOptions === undefined ? $scope.variantGroupsOpen : savedOpenOptions;
            //Add to the set of variant groups
            $scope.variantGroups.push(variantGroup);
            answersReciewed++;

            if (answersReciewed === endCount) {
              //After the last answer from the server have been recieved var ready the variant scaffolding
              readyVariantScaffolding();
            }
          });
        

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
