angular.module("umbraco").controller("TeaCommerce.OrderStatus.EditController", function ($scope, $routeParams, $http, notificationsService, /*contentEditingHelper, entityResource, contentResource, editorState*/) {
  //alert($routeParams.id);
  
  //$scope.routeParams = $routeParams;
  //$scope.content = entityResource.getById($routeParams.id, "Document");
  //alert($scope.content);
  //$scope.ancestors = [$routeParams.section, $routeParams.tree, $routeParams.method, $routeParams.id];
  $scope.contentTabs = { tabs: [{ id: 1, label: "Content" }, { id: 2, label: "Tab 2" }] };
  var storeId = "",
      orderStatusId = "";
  var isStoreId = true;
	  
  var ids = $routeParams.id.split('-');
  storeId = ids[0];
  orderStatusId = ids[1];
	  
  $http.get('backoffice/teacommerce2/OrderStatus/Get?storeId='+storeId+'&orderStatusId='+orderStatusId).success(function (orderStatusDtm) {

    $scope.orderStatus = orderStatusDtm;
	
  });
	
	$scope.save = function() {
		console.log('SAVE');
	$http.post('backoffice/teacommerce2/OrderStatus/Save', $scope.orderStatus).success(function (data) {
		notificationsService.success("Saved!", "Your changes were saved!");
		$scope.contentForm.$dirty=false;
	  }).error(function(e){
		  
	  });
	}
	/*if (content.parentId && content.parentId != -1) {
                entityResource.getAncestors(content.id, "document")
               .then(function (anc) {
                   $scope.ancestors = anc;
               });
            }
	
	/*$scope.editorState = editorState;
	editorState.set($scope.content);
	
	if(editorState.current == null){
		alert("editorState is null!");
	} else if(editorState.current != null){
		alert("editorState is not null!");
	}

        /*var buttons = contentEditingHelper.configureContentEditorButtons({
            create: $routeParams.create,
            content: content,
            methods: {
                saveAndPublish: $scope.saveAndPublish,
                sendToPublish: $scope.sendToPublish,
                save: $scope.save,
                unPublish: $scope.unPublish
            }
        });
        /*$scope.defaultButton = buttons.defaultButton;
        $scope.subButtons = buttons.subButtons;

        editorState.set($scope.content);

        //We fetch all ancestors of the node to generate the footer breadcrumb navigation
        if (!$routeParams.create) {
            if (content.parentId && content.parentId != -1) {
                entityResource.getAncestors(content.id, "document")
               .then(function (anc) {
                   $scope.ancestors = anc;
               });
            }
        }*/

});

