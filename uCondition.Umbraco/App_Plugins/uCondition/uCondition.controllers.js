var uCondition;
(function (uCondition) {
    var Controllers;
    (function (Controllers) {
        var uConditionController = (function () {
            function uConditionController($scope, predicateSyncService) {
                this.$scope = $scope;
                if (this.$scope.model.value == null || this.$scope.model.value == "") {
                    $scope.model.value = new uCondition.Models.uConditionModel();
                }
                predicateSyncService.SyncPredicateGroups($scope.model.value.PredicateGroups);
                //for (var i = 0; i < $scope.model.value.PredicateGroups.length; i++) {
                //    predicateSyncService.SyncActions($scope.model.value.PredicateGroups[i].Actions);
                //}
                this.Model = this.$scope.model.value;
                this.ModalDialog = new uCondition.Models.ModalDialog();
                var defaultConfig = new uCondition.Models.DataTypeConfig();
                this.Config = $scope.model.config;
                this.Config = angular.extend({}, defaultConfig, this.Config);
                uCondition.Models.DataTypeConfig.CleanUp(this.Config);
                var that = this;
                $scope.$on("formSubmitting", function () {
                    $scope.model.value = that.Model;
                });
            }
            uConditionController.prototype.AddSwimlane = function (conditions) {

                console.log('NOW?!');

                conditions.push(new uCondition.Models.Swimlane());
            };
            return uConditionController;
        }());
        Controllers.uConditionController = uConditionController;
        var PredicateGroupController = (function () {
            function PredicateGroupController($scope, $timeout, editorService) {
                this.$scope = $scope;
                this.$timeout = $timeout;
                this.dialogService = editorService;
                this.ModalDialog = $scope.modalDialog;
            }
            PredicateGroupController.prototype.GetConditionFieldSummary = function (condition) {
                var summary = "";
                for (var i = 0; i < condition.Values.length; i++) {
                    summary += "<b>" + condition.Values[i].DisplayName + "</b>" + ": "
                        + (condition.Values[i].Value == null || condition.Values[i].Value == "null" ? "<span style='color:red'>" : "")
                        + (condition.Values[i].DisplayValue.length > 0 ? condition.Values[i].DisplayValue : condition.Values[i].Value)
                        + (condition.Values[i].Value == null || condition.Values[i].Value == "null" ? "</span>" : "")
                        + "<br/>";
                }
                summary = summary.substr(0, summary.length - 5);
                return summary;
            };
            PredicateGroupController.prototype.AddPredicate = function (conditions) {
                var _this = this;
                this.ModalDialog.title = "Add Condition";
                this.ModalDialog.subtitle = "Choose the condition you would like to add";
                this.ModalDialog.view = "/App_Plugins/uCondition/editor/dialogs/add/addcondition.html";
                this.ModalDialog.show = true;
                this.ModalDialog.submitButtonLabel = "Add Condition";
                this.ModalDialog.closeButtonLabel = "Cancel";
                this.ModalDialog.value = {};
                this.ModalDialog.submit = function (model) {
                    for (var i = 0; i < model.value.length; i++) {
                        conditions.push(model.value[i]);
                    }
                    _this.ModalDialog.show = false;
                    _this.EditManyConditions(model.value);
                };
            };
            PredicateGroupController.prototype.EditPredicate = function (condition, callback) {
                var _this = this;
                if (callback === void 0) { callback = null; }
                this.ModalDialog.title = "Edit Condition";
                this.ModalDialog.subtitle = condition.Config.Name;
                this.ModalDialog.view = "/app_plugins/ucondition/editor/dialogs/edit/ucondition.editcondition.html";
                this.ModalDialog.dialogData = condition;
                this.ModalDialog.submitButtonLabel = "Accept Changes";
                this.ModalDialog.closeButtonLabel = "Cancel";
                this.ModalDialog.value = {};
                this.ModalDialog.submit = function (model) {
                    _this.ModalDialog.show = false;
                    condition.NeedsConfiguring = false;
                    if (callback != null)
                        callback();
                };
                this.ModalDialog.show = true;
            };
            /**
             * loop through and show edit dialog for each condition that needs configuring
             * @param conditions conditions to edit
             */
            PredicateGroupController.prototype.EditManyConditions = function (conditions) {
                //shallow clone the array to make sure it doesn't change while running through it
                var conditionsToProcess = conditions.splice(0);
                var index = 0;
                var that = this;
                loopThroughConditionsForEditing();
                function loopThroughConditionsForEditing() {
                    that.$timeout(function () {
                        index++;
                        if (conditionsToProcess.length < index)
                            return;
                        if (conditionsToProcess[index - 1].Type == "Predicate" && conditionsToProcess[index - 1].NeedsConfiguring) {
                            that.EditPredicate(conditionsToProcess[index - 1], loopThroughConditionsForEditing);
                        }
                        else {
                            loopThroughConditionsForEditing();
                        }
                    }, 200);
                }
            };
            PredicateGroupController.prototype.RemoveCondition = function (container, conditionToRemove) {
                var index = -1;
                for (var i = 0; i < container.Conditions.length; i++) {
                    if (container.Conditions[i] == conditionToRemove) {
                        index = i;
                        break;
                    }
                }
                if (index != -1)
                    container.Conditions.splice(index, 1);
            };
            PredicateGroupController.prototype.RemoveSwimlane = function (container, predicateToRemove) {
                var index = -1;
                for (var i = 0; i < container.PredicateGroups.length; i++) {
                    if (container.PredicateGroups[i] == predicateToRemove) {
                        index = i;
                        break;
                    }
                }
                if (index != -1)
                    container.PredicateGroups.splice(index, 1);
            };
            return PredicateGroupController;
        }());
        Controllers.PredicateGroupController = PredicateGroupController;
        var EditConditionDialog = (function () {
            function EditConditionDialog($scope, uConditionApiService) {
                this.$scope = $scope;
                this.uConditionApiService = uConditionApiService;
                this.Fields = this.GetFields($scope.model.dialogData);
                var that = this;
                $scope.$on("formSubmitted", function () {
                    for (var i = 0; i < that.Fields.length; i++) {
                        for (var k = 0; k < $scope.model.dialogData.Values.length; k++) {
                            if (that.Fields[i].PropertyEditorModel.alias == $scope.model.dialogData.Values[k].Alias) {
                                $scope.model.dialogData.Values[k].Value = that.Fields[i].PropertyEditorModel.value;
                                //resolve display value
                                var valueType = that.GetConfigPropertyByAlias(that.Fields[i].PropertyEditorModel.alias, $scope.model.dialogData.Config.Fields).ValueType;
                                if (valueType != uCondition.Models.EditablePropertyDisplayMode.Standard) {
                                    if (valueType == uCondition.Models.EditablePropertyDisplayMode.Standard) {
                                        $scope.model.dialogData.Values[k].DisplayValue = "";
                                    }
                                    if (valueType == uCondition.Models.EditablePropertyDisplayMode.Hidden) {
                                        $scope.model.dialogData.Values[k].DisplayValue = "Set";
                                    }
                                    else if (valueType == uCondition.Models.EditablePropertyDisplayMode.IsBoolean) {
                                        $scope.model.dialogData.Values[k].DisplayValue = that.Fields[i].PropertyEditorModel.value == "1" ? "True" : "False";
                                    }
                                    else if (valueType == uCondition.Models.EditablePropertyDisplayMode.IsPreValue) {
                                        var fieldWrapper = that.GetFieldWrapperByAlias(that.Fields[i].PropertyEditorModel.alias, that.Fields);
                                        //if its a multivalue list of prevalues
                                        if (fieldWrapper.PropertyEditorModel.config.items) {
                                            $scope.model.dialogData.Values[k].DisplayValue = "";
                                            if (Array.isArray(that.Fields[i].PropertyEditorModel.value)) {
                                                for (var j = 0; j < that.Fields[i].PropertyEditorModel.value.length; j++) {
                                                    $scope.model.dialogData.Values[k].DisplayValue += that.GetMultiValueNameById(that.Fields[i].PropertyEditorModel.value[j], fieldWrapper.PropertyEditorModel.config.items) + ", ";
                                                }
                                                $scope.model.dialogData.Values[k].DisplayValue = $scope.model.dialogData.Values[k].DisplayValue.substr(0, $scope.model.dialogData.Values[k].DisplayValue.length - 2);
                                            }
                                            else {
                                                //its a value
                                                $scope.model.dialogData.Values[k].DisplayValue += that.GetMultiValueNameById(that.Fields[i].PropertyEditorModel.value, fieldWrapper.PropertyEditorModel.config.items);
                                            }
                                        }
                                        else {
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ;
                });
            }
            EditConditionDialog.prototype.GetFields = function (condition) {
                var fields = new Array();
                //for each field in the editable fields for this condition
                for (var i = 0; i < condition.Config.Fields.length; i++) {
                    var currentField = condition.Config.Fields[i];
                    //get the field value from the condition values
                    var fieldValue = this.GetFieldValueByAlias(currentField.Alias, condition.Values);
                    //if it doesn't exists, create it
                    if (fieldValue == null) {
                        fieldValue = {
                            Alias: currentField.Alias,
                            Value: null,
                            DisplayName: currentField.Name,
                            DisplayValue: ""
                        };
                        condition.Values.push(fieldValue);
                    }
                    this.uConditionApiService.GetPropertyEditorByName(currentField.Control)
                        .success((function (field, value, index) {
                        return function (dataType) {
                            //add the field wrapper, which holds the property editor model and the name for display
                            var fieldWrapper = {
                                Name: field.Name,
                                Alias: field.Alias,
                                PropertyEditorModel: {},
                                SortOrder: index,
                            };
                            if (dataType == "null") {
                                fieldWrapper.PropertyEditorModel.view = field.Control;
                                fieldWrapper.PropertyEditorModel.alias = field.Alias;
                                fieldWrapper.PropertyEditorModel.value = value.Value;
                                fieldWrapper.PropertyEditorModel.preview = false;
                                fields.push(fieldWrapper);
                            }
                            else {
                                fieldWrapper.PropertyEditorModel.view = dataType.view;
                                fieldWrapper.PropertyEditorModel.alias = field.Alias;
                                fieldWrapper.PropertyEditorModel.value = value.Value;
                                fieldWrapper.PropertyEditorModel.preview = false;
                                fieldWrapper.PropertyEditorModel.config = dataType.prevalues;
                                fields.push(fieldWrapper);
                            }
                        };
                    })(currentField, fieldValue, i));
                }
                return fields;
            };
            EditConditionDialog.prototype.GetFieldValueByAlias = function (alias, values) {
                for (var i = 0; i < values.length; i++) {
                    if (values[i].Alias == alias)
                        return values[i];
                }
                return null;
            };
            EditConditionDialog.prototype.GetConfigPropertyByAlias = function (alias, values) {
                for (var i = 0; i < values.length; i++) {
                    if (values[i].Alias == alias)
                        return values[i];
                }
                return null;
            };
            EditConditionDialog.prototype.GetFieldWrapperByAlias = function (alias, fieldWrappers) {
                for (var i = 0; i < fieldWrappers.length; i++) {
                    if (fieldWrappers[i].Alias == alias)
                        return fieldWrappers[i];
                }
                return null;
            };
            EditConditionDialog.prototype.GetMultiValueNameById = function (id, items) {
                for (var i = 0; i < items.length; i++) {
                    if (items[i].id == id)
                        return items[i].value;
                }
                return null;
            };
            return EditConditionDialog;
        }());
        Controllers.EditConditionDialog = EditConditionDialog;
        var AddConditionDialog = (function () {
            function AddConditionDialog($scope, uConditionApiService) {
                this.$scope = $scope;
                this.uConditionApiService = uConditionApiService;
                var that = this;
                $scope.filterGroup = "All";
                uConditionApiService.GetPredicates().then(function (predicateConfigs) {
                    var groups = {};
                    groups["Special"] = [new uCondition.Models.PredicateGroup()];
                    for (var i = 0; i < predicateConfigs.length; i++) {
                        if (typeof groups[predicateConfigs[i].Category] == "undefined")
                            groups[predicateConfigs[i].Category] = [];
                        var predicate = new uCondition.Models.Predicate();
                        predicate.Config = predicateConfigs[i];
                        groups[predicateConfigs[i].Category].push(predicate);
                    }
                    that.$scope.groups = [];
                    for (var group in groups) {
                        var predicateGroup = new uCondition.Models.Dialogs.ConditionGroup();
                        predicateGroup.Name = group;
                        for (var i = 0; i < groups[group].length; i++) {
                            var predicateOption = new uCondition.Models.Dialogs.ConditionOption();
                            predicateOption.Condition = groups[group][i];
                            predicateGroup.Conditions.push(predicateOption);
                        }
                        that.$scope.groups.push(predicateGroup);
                    }
                });
                $scope.FilterGroups = function (groupName) {
                    for (var i = 0; i < $scope.groups.length; i++) {
                        $scope.groups[i].Show = $scope.groups[i].Name == groupName || groupName == "All";
                    }
                };
                $scope.FilterAllByText = function (text) {
                    text = text.toLowerCase();
                    for (var i = 0; i < $scope.groups.length; i++) {
                        var showCount = 0;
                        for (var k = 0; k < $scope.groups[i].Conditions.length; k++) {
                            $scope.groups[i].Conditions[k].Show = $scope.groups[i].Conditions[k].Condition.Config && $scope.groups[i].Conditions[k].Condition.Config.Name.toLowerCase().indexOf(text) >= 0 || text.length == 0;
                            if ($scope.groups[i].Conditions[k].Show)
                                showCount++;
                        }
                        if ($scope.filterGroup == "All" || ($scope.groups[i].Name == $scope.filterGroup && $scope.groups[i].Name != $scope.filterGroup))
                            $scope.groups[i].Show = showCount > 0;
                    }
                };
                this.$scope.$on("formSubmitting", function () {
                    $scope.model.value = [];
                    for (var i = 0; i < $scope.groups.length; i++) {
                        for (var k = 0; k < $scope.groups[i].Conditions.length; k++) {
                            if ($scope.groups[i].Conditions[k].Selected) {
                                if ($scope.groups[i].Conditions[k].Condition.Type == "Predicate")
                                    $scope.groups[i].Conditions[k].Condition.NeedsConfiguring = $scope.groups[i].Conditions[k].Condition.Config.Fields.length > 0;
                                $scope.model.value.push($scope.groups[i].Conditions[k].Condition);
                            }
                        }
                    }
                });
            }
            return AddConditionDialog;
        }());
        Controllers.AddConditionDialog = AddConditionDialog;
        var AddActionDialog = (function () {
            function AddActionDialog($scope, uConditionApiService) {
                this.$scope = $scope;
                this.uConditionApiService = uConditionApiService;
                var that = this;
                $scope.filterGroup = "All";
                uConditionApiService.GetActions().then(function (predicateConfigs) {
                    var groups = {};
                    for (var i = 0; i < predicateConfigs.length; i++) {
                        if (typeof groups[predicateConfigs[i].Category] == "undefined")
                            groups[predicateConfigs[i].Category] = [];
                        var predicate = new uCondition.Models.Action();
                        predicate.Config = predicateConfigs[i];
                        groups[predicateConfigs[i].Category].push(predicate);
                    }
                    that.$scope.groups = [];
                    for (var group in groups) {
                        var predicateGroup = new uCondition.Models.Dialogs.ConditionGroup();
                        predicateGroup.Name = group;
                        for (var i = 0; i < groups[group].length; i++) {
                            var predicateOption = new uCondition.Models.Dialogs.ConditionOption();
                            predicateOption.Condition = groups[group][i];
                            predicateGroup.Conditions.push(predicateOption);
                        }
                        that.$scope.groups.push(predicateGroup);
                    }
                });
                $scope.FilterGroups = function (groupName) {
                    for (var i = 0; i < $scope.groups.length; i++) {
                        $scope.groups[i].Show = $scope.groups[i].Name == groupName || groupName == "All";
                    }
                };
                $scope.FilterAllByText = function (text) {
                    text = text.toLowerCase();
                    for (var i = 0; i < $scope.groups.length; i++) {
                        var showCount = 0;
                        for (var k = 0; k < $scope.groups[i].Conditions.length; k++) {
                            $scope.groups[i].Conditions[k].Show = $scope.groups[i].Conditions[k].Condition.Config && $scope.groups[i].Conditions[k].Condition.Config.Name.toLowerCase().indexOf(text) >= 0 || text.length == 0;
                            if ($scope.groups[i].Conditions[k].Show)
                                showCount++;
                        }
                        if ($scope.filterGroup == "All" || ($scope.groups[i].Name == $scope.filterGroup && $scope.groups[i].Name != $scope.filterGroup))
                            $scope.groups[i].Show = showCount > 0;
                    }
                };
                this.$scope.$on("formSubmitting", function () {
                    $scope.model.value = [];
                    for (var i = 0; i < $scope.groups.length; i++) {
                        for (var k = 0; k < $scope.groups[i].Conditions.length; k++) {
                            if ($scope.groups[i].Conditions[k].Selected) {
                                $scope.model.value.push($scope.groups[i].Conditions[k].Condition);
                            }
                        }
                    }
                });
            }
            return AddActionDialog;
        }());
        Controllers.AddActionDialog = AddActionDialog;
    })(Controllers = uCondition.Controllers || (uCondition.Controllers = {}));
})(uCondition || (uCondition = {}));
var uCondition;
(function (uCondition) {
    var Services;
    (function (Services) {
        var uConditionApiService = (function () {
            function uConditionApiService($http) {
                this.$http = $http;
            }
            uConditionApiService.prototype.GetPredicates = function () {
                return this.$http.get("/umbraco/backoffice/Api/uConditionApi/GetPredicates");
            };
            uConditionApiService.prototype.GetActions = function () {
                return this.$http.get("/umbraco/backoffice/Api/uConditionApi/GetActions");
            };
            uConditionApiService.prototype.GetPropertyEditorByName = function (propertyEditorName) {
                return this.$http.get("/umbraco/backoffice/Api/uConditionApi/GetPropertyEditorByName?datatypeName=" + propertyEditorName);
            };
            return uConditionApiService;
        }());
        Services.uConditionApiService = uConditionApiService;
        var PredicateSyncService = (function () {
            function PredicateSyncService(uConditionApiService) {
                this.uConditionApiService = uConditionApiService;
            }
            PredicateSyncService.prototype.SyncPredicateGroups = function (predicateGroups) {
                var that = this;
                this.uConditionApiService.GetPredicates().then(function (predicateConfigs) {
                    for (var i = 0; i < predicateGroups.length; i++) {
                        that.SyncPredicateGroup(predicateGroups[i], predicateConfigs);
                    }
                });
            };
            //public SyncActions(actions: Array<Models.Action>) {
            //    var that = this;
            //    this.uConditionApiService.GetActions().then(function (predicateConfigs) {
            //        for (var i = 0; i < actions.length; i++) {
            //            that.SyncPredicate(<any>actions[i], that.FindPredicateConfigByAlias(actions[i].Config.Alias, predicateConfigs));
            //        }
            //    });
            //}
            PredicateSyncService.prototype.SyncPredicateGroup = function (predicateGroup, predicateConfigs) {
                for (var i = 0; i < predicateGroup.Conditions.length; i++) {
                    var currentPredicate = predicateGroup.Conditions[i];
                    if (currentPredicate.Type == "PredicateGroup") {
                        this.SyncPredicateGroup(currentPredicate, predicateConfigs);
                    }
                    else {
                        var predicateConfig = this.FindPredicateConfigByAlias(currentPredicate.Config.Alias, predicateConfigs);
                        this.SyncPredicate(currentPredicate, predicateConfig);
                    }
                }
            };
            PredicateSyncService.prototype.SyncPredicate = function (predicate, predicateConfig) {
                predicate.Config = predicateConfig;
            };
            PredicateSyncService.prototype.FindPredicateConfigByAlias = function (alias, predicateConfigs) {
                for (var i = 0; i < predicateConfigs.length; i++) {
                    if (predicateConfigs[i].Alias == alias)
                        return predicateConfigs[i];
                }
                return null;
            };
            return PredicateSyncService;
        }());
        Services.PredicateSyncService = PredicateSyncService;
    })(Services = uCondition.Services || (uCondition.Services = {}));
})(uCondition || (uCondition = {}));
angular.module("umbraco").controller("uCondition", ["$scope", "PredicateSyncService", uCondition.Controllers.uConditionController]);
angular.module("umbraco").controller("uCondition.PredicateGroup", ["$scope", "$timeout", "editorService", uCondition.Controllers.PredicateGroupController]);
angular.module("umbraco").controller("uCondition.Dialogs.AddCondition", ["$scope", "uConditionApiService", uCondition.Controllers.AddConditionDialog]);
angular.module("umbraco").controller("uCondition.Dialogs.AddAction", ["$scope", "uConditionApiService", uCondition.Controllers.AddActionDialog]);
angular.module("umbraco").controller("uCondition.Editors.GenericEditor", ["$scope", "uConditionApiService", uCondition.Controllers.EditConditionDialog]);
angular.module("umbraco").service("uConditionApiService", ["$http", uCondition.Services.uConditionApiService]);
angular.module("umbraco").service("PredicateSyncService", ["uConditionApiService", uCondition.Services.PredicateSyncService]);
angular.module('umbraco').directive('uCondition.Editors.PropertyEditor', function () {
    return {
        template: 'Name: {{customer.name}} Street: {{customer.street}}'
    };
});
angular.module("umbraco").directive("uconditionSwimlane", function () {
    return {
        scope: {
            model: "=",
            container: "=",
            isLast: "=",
            modalDialog: "=",
            isPreValue: "@",
            preview: "@",
            actionsEnabled: '='
        },
        restrict: 'E',
        replace: true,
        template: '<ng-include src="\'/app_plugins/uCondition/uCondition.swimlane.html\'" />',
        link: function (scope, element, attrs, ctrl) {
        }
    };
});
angular.module("umbraco").directive("uconditionCondition", function () {
    return {
        scope: {
            model: "=",
            container: "=",
            isLast: "=",
            modalDialog: "=",
            isPreValue: "@",
            preview: "@"
        },
        restrict: 'E',
        replace: true,
        template: '<ng-include src="\'/app_plugins/uCondition/uCondition.predicategroup.html\'" />',
        link: function (scope, element, attrs, ctrl) {
        }
    };
});
angular.module("umbraco").directive("contenteditable", function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, element, attrs, ngModel) {
            function read() {
                ngModel.$setViewValue(element.html());
            }
            ngModel.$render = function () {
                element.html(ngModel.$viewValue || "");
            };
            element.bind("blur keyup change", function () {
                scope.$apply(read);
            });
        }
    };
});
(function () {
    "use strict";
    // Vertilize Container
    angular.module("umbraco").directive('vertilizeContainer', [
        function () {
            return {
                restrict: 'EA',
                controller: [
                    '$scope', '$window',
                    function ($scope, $window) {
                        // Alias this
                        var _this = this;
                        // Array of children heights
                        _this.childrenHeights = [];
                        // API: Allocate child, return index for tracking.
                        _this.allocateMe = function () {
                            _this.childrenHeights.push(0);
                            return (_this.childrenHeights.length - 1);
                        };
                        // API: Update a child's height
                        _this.updateMyHeight = function (index, height) {
                            _this.childrenHeights[index] = height;
                        };
                        // API: Get tallest height
                        _this.getTallestHeight = function () {
                            var height = 0;
                            for (var i = 0; i < _this.childrenHeights.length; i = i + 1) {
                                height = Math.max(height, _this.childrenHeights[i]);
                            }
                            return height;
                        };
                        // Add window resize to digest cycle
                        angular.element($window).bind('resize', function () {
                            return $scope.$apply();
                        });
                    }
                ]
            };
        }
    ]);
    // Vertilize Item
    angular.module("umbraco").directive('vertilize', [
        function () {
            return {
                restrict: 'EA',
                require: '^vertilizeContainer',
                link: function (scope, element, attrs, parent) {
                    // My index allocation
                    var myIndex = parent.allocateMe();
                    // Get my real height by cloning so my height is not affected.
                    var getMyRealHeight = function () {
                        var clone = element.clone()
                            .removeAttr('vertilize')
                            .css({
                            height: '',
                            width: element.outerWidth(),
                            position: 'fixed',
                            top: 0,
                            left: 0,
                            visibility: 'hidden'
                        });
                        element.after(clone);
                        var realHeight = clone.height();
                        clone['remove']();
                        return realHeight;
                    };
                    // Watch my height
                    scope.$watch(getMyRealHeight, function (myNewHeight) {
                        if (myNewHeight) {
                            parent.updateMyHeight(myIndex, myNewHeight);
                        }
                    });
                    // Watch for tallest height change
                    scope.$watch(parent.getTallestHeight, function (tallestHeight) {
                        if (tallestHeight) {
                            element.css('height', tallestHeight);
                        }
                    });
                }
            };
        }
    ]);
}());
//angular.module("umbraco").directive("verticalCenterize", function () {
//    return {
//        restrict: 'A',
//        link: function (scope, element, attrs, parent) {
//            var elHeight = element.innerHeight();
//            element.css({
//                "top": "50%",
//                "margin-top": elHeight / 2 * -1
//            });
//            scope.$watch(function () { return parent.innerHeight(); }, function (parentHeight) {
//            });
//        }
//    };
//}); 
