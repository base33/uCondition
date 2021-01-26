var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};

var uConditionPublicAccess;
(function (uConditionPublicAccess) {
    var Controllers;
    (function (Controllers) {
        var PublicAccessController = (function () {
            function PublicAccessController($scope, publicAccessService, editorService, treeService, navigationService, notificationsService, $routeParams, contentResource, mediaResource, appState) {
                this.$scope = $scope;
                this.publicAccessService = publicAccessService;
                this.editorService = editorService;
                this.EditorModel = null;
                this.$scope.editModel = null;
                this.$scope.metaData = null;
                this.$routeParams = $routeParams;
                this.TreeService = treeService;
                this.notificationsService = notificationsService;
                this.navigationService = navigationService;
                this.AppState = appState;
                this.loading = true;
                var that = this;
                $scope.currentNode = null;

                // The hacky way of retrieving the action and node data
                // https://our.umbraco.com/forum/extending-umbraco-and-using-the-api/96324-menuitemadditionaldata
                var menuActions = this.AppState.getMenuState('menuActions');
                var menuTitle = this.AppState.getMenuState('dialogTitle');
                var metaData = menuActions.find(function (element) {
                    if (element.name === menuTitle) {
                        return element.name
                    }
                }).metaData;

                $scope.metaData = metaData;

                var resource = $routeParams.section == "content" ? contentResource : mediaResource;
                resource.getById(metaData.nodeId)
                    .then(data => {
                        $scope.currentNode = data;
                        navigationService.syncTree({ tree: $routeParams.section, path: data.path, forceReload: true });
                    })
                    .then(() => {
                        that.publicAccessService.GetCondition(metaData.nodeId)
                            .then(response => {
                                that.EditorModel = response.data == null
                                    ? new Models.ProtectedPageModel()
                                    : response.data;

                                that.$scope.editModel = {
                                    view: "/app_plugins/ucondition/editor/ucondition.html",
                                    value: that.EditorModel.Condition,
                                    preview: false,
                                    config: {
                                        EnableAlternativeConditions: false
                                    }
                                };
                            })
                            .then(() => {
                                if (that.EditorModel.FalseActionNodeId) {
                                    contentResource.getById(that.EditorModel.FalseActionNodeId)
                                        .then(response => {
                                            // Workaround for the variant or culture name for the node
                                            if (response.name == null && response.variants[0].name != null) {
                                                response.name = response.variants[0].name;
                                            }

                                            that.EditorModel.FalseActionNode = response;
                                            that.loading = false;
                                        });
                                }
                                else {
                                    that.loading = false;
                                }
                            });
                    });
            };

            PublicAccessController.prototype.AddFalseCondition = function () {
                this.EditorModel.Conditions.push(new Models.ProtectedPageCondition());
            };

            PublicAccessController.prototype.PickFalseActionNodeId = function () {
                var _this = this;
                _this.navigationService.allowHideDialog(false);
                _this.editorService.contentPicker({
                    multiPicker: false,
                    submit: function (model) {
                        _this.EditorModel.FalseActionNode = model.selection[0];
                        _this.EditorModel.FalseActionNodeId = model.selection[0].id;
                        _this.editorService.close();
                        _this.navigationService.allowHideDialog(true);
                    },
                    close: function () {
                        _this.editorService.close();
                    }
                });
            };

            PublicAccessController.prototype.Move = function (array, index, newIndex) {
                if (index >= array.length) {
                    var k = newIndex - array.length;
                    while ((k--) + 1) {
                        array.push(undefined);
                    }
                }
                array.splice(newIndex, 0, array.splice(index, 1)[0]);
            };

            PublicAccessController.prototype.RemoveProtection = function () {
                var _this = this;
                if (confirm("Are you sure you wish to remove protection for this page?")) {
                    this.publicAccessService.RemoveCondition(_this.$scope.metaData.nodeId)
                        .then(() => {
                            _this.notificationsService.success("Protection removed");
                            _this.navigationService.allowHideDialog(true);
                            _this.navigationService.hideDialog();
                            _this.navigationService.syncTree({ tree: _this.$routeParams.section, path: _this.$scope.currentNode.path, forceReload: true });
                        });
                }
            };

            PublicAccessController.prototype.SaveChanges = function () {
                var _this = this;
                this.publicAccessService.SaveCondition(_this.$scope.metaData.nodeId, _this.EditorModel)
                    .then(() => {
                        _this.notificationsService.success("Protection saved");
                        _this.navigationService.allowHideDialog(true);
                        _this.navigationService.hideDialog();
                        _this.navigationService.syncTree({ tree: _this.$routeParams.section, path: _this.$scope.currentNode.path, forceReload: true });
                    });
            };

            PublicAccessController.prototype.Close = function () {
                var _this = this;
                _this.navigationService.allowHideDialog(true);
                _this.navigationService.hideDialog();
            };

            return PublicAccessController;
        }());

        Controllers.PublicAccessController = PublicAccessController;
    })(Controllers = uConditionPublicAccess.Controllers || (uConditionPublicAccess.Controllers = {}));

    var Models;
    (function (Models) {
        var ProtectedPageCondition = (function () {
            function ProtectedPageCondition() {
                this.Condition = new uCondition.Editor.Models.uConditionModel();
                this.FalseActionNodeId = 0;
            }
            return ProtectedPageCondition;
        }());

        Models.ProtectedPageCondition = ProtectedPageCondition;

        var ProtectedPageModel = (function (_super) {
            __extends(ProtectedPageModel, _super);
            function ProtectedPageModel() {
                _super.apply(this, arguments);
                this.Conditions = new Array();
            }
            return ProtectedPageModel;
        }(ProtectedPageCondition));

        Models.ProtectedPageModel = ProtectedPageModel;
    })(Models = uConditionPublicAccess.Models || (uConditionPublicAccess.Models = {}));

    var Services;
    (function (Services) {
        var uConditionPublicAccessService = (function () {
            function uConditionPublicAccessService($http) {
                this.$http = $http;
            }

            uConditionPublicAccessService.prototype.GetCondition = function (id) {
                return this.$http.get("/Umbraco/BackOffice/Api/uConditionPublicAccessApi/Get?id=" + id);
            };

            uConditionPublicAccessService.prototype.SaveCondition = function (id, conditionModel) {
                return this.$http.post("/Umbraco/BackOffice/Api/uConditionPublicAccessApi/Save?id=" + id, conditionModel);
            };

            uConditionPublicAccessService.prototype.RemoveCondition = function (id) {
                return this.$http.delete("/Umbraco/BackOffice/Api/uConditionPublicAccessApi/Delete?id=" + id);
            };

            return uConditionPublicAccessService;
        }());

        Services.uConditionPublicAccessService = uConditionPublicAccessService;
    })(Services = uConditionPublicAccess.Services || (uConditionPublicAccess.Services = {}));
})(uConditionPublicAccess || (uConditionPublicAccess = {}));

angular.module("umbraco.directives").directive("uconditionEditor", [function uConditionEditor($scope) {
    return {
        restrict: 'E',
        replace: false,
        scope: {
            value: '='
        },
        template: "<ng-form val-form-manager><umb-property-editor ng-if='value' model='editModel'></umb-property-editor></ng-form>",
        link: function (scope, element, attrs) {
            scope.editModel = {
                view: "/app_plugins/ucondition/editor/ucondition.html",
                alias: "test",
                value: scope.value,
                preview: false,
                config: {
                    EnableAlternativeConditions: false
                }
            };
            setTimeout(function () {
                scope.$watch(function () { return scope.value; }, function () { return scope.editModel.value = scope.value; });
            }, 200);
            setTimeout(function () {
                scope.$watch(function () { return scope.editModel.value; }, function () { return scope.value = scope.editModel.value; });
            }, 200);
        }
    };
}]);
angular.module("umbraco").controller("uCondition.PublicAccess", ["$scope", "uCondition.PublicAccessService", "editorService", "treeService", "navigationService", "notificationsService", "$routeParams", "contentResource", "mediaResource", "appState", uConditionPublicAccess.Controllers.PublicAccessController]);
angular.module("umbraco").service("uCondition.PublicAccessService", ["$http", uConditionPublicAccess.Services.uConditionPublicAccessService]);