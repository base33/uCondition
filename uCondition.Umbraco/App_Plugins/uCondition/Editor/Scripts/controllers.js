var uCondition;
(function (uCondition) {
    var Editor;
    (function (Editor) {
        var Controllers;
        (function (Controllers) {
            var uConditionController = (function () {
                function uConditionController($scope, predicateSyncService) {
                    this.$scope = $scope;
                    if (this.$scope.model.value == null || this.$scope.model.value == "") {
                        $scope.model.value = new uCondition.Editor.Models.uConditionModel();
                    }
                    predicateSyncService.SyncPredicateGroups($scope.model.value.PredicateGroups);
                    //for (var i = 0; i < $scope.model.value.PredicateGroups.length; i++) {
                    //    predicateSyncService.SyncActions($scope.model.value.PredicateGroups[i].Actions);
                    //}
                    this.Model = this.$scope.model.value;
                    this.ModalDialog = new uCondition.Editor.Models.ModalDialog();
                    var defaultConfig = new uCondition.Editor.Models.DataTypeConfig();
                    this.Config = $scope.model.config;
                    this.Config = angular.extend({}, defaultConfig, this.Config);
                    uCondition.Editor.Models.DataTypeConfig.CleanUp(this.Config);
                    var that = this;
                    $scope.$on("formSubmitting", function () {
                        $scope.model.value = that.Model;
                    });
                }
                uConditionController.prototype.AddSwimlane = function (conditions) {
                    conditions.push(new uCondition.Editor.Models.Swimlane());
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
                    this.ModalDialog.title = "Add Predicate";
                    this.ModalDialog.subtitle = "Choose the predicate you would like to add";
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
                    this.ModalDialog.title = "Edit Predicate";
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
                    if (!confirm("Are you sure you want to delete this expression?"))
                        return;
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
                                    if (valueType != Editor.Models.EditablePropertyDisplayMode.Standard) {
                                        if (valueType == Editor.Models.EditablePropertyDisplayMode.Standard) {
                                            $scope.model.dialogData.Values[k].DisplayValue = "";
                                        }
                                        if (valueType == Editor.Models.EditablePropertyDisplayMode.Hidden) {
                                            $scope.model.dialogData.Values[k].DisplayValue = "Set";
                                        }
                                        else if (valueType == Editor.Models.EditablePropertyDisplayMode.IsBoolean) {
                                            $scope.model.dialogData.Values[k].DisplayValue = that.Fields[i].PropertyEditorModel.value == "1" ? "True" : "False";
                                        }
                                        else if (valueType == Editor.Models.EditablePropertyDisplayMode.IsPreValue) {
                                            var fieldWrapper = that.GetFieldWrapperByAlias(that.Fields[i].PropertyEditorModel.alias, that.Fields);
                                            //TODO: here, the value is an index - need to get the value from prevalues list
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
                            .then((function (field, value, index) {
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

                    uConditionApiService.GetPredicates()
                        .then(function (predicateConfigs) {
                            var groups = {};
                            groups["Special"] = [new uCondition.Editor.Models.PredicateGroup()];
                            for (var i = 0; i < predicateConfigs.length; i++) {
                                if (typeof groups[predicateConfigs[i].Category] == "undefined")
                                    groups[predicateConfigs[i].Category] = [];
                                var predicate = new uCondition.Editor.Models.Predicate();
                                predicate.Config = predicateConfigs[i];
                                groups[predicateConfigs[i].Category].push(predicate);
                            }
                            that.$scope.groups = [];
                            for (var group in groups) {
                                var predicateGroup = new uCondition.Editor.Models.Dialogs.ConditionGroup();
                                predicateGroup.Name = group;
                                for (var i = 0; i < groups[group].length; i++) {
                                    var predicateOption = new uCondition.Editor.Models.Dialogs.ConditionOption();
                                    predicateOption.Condition = groups[group][i];
                                    predicateGroup.Conditions.push(predicateOption);
                                }
                                that.$scope.groups.push(predicateGroup);
                            }
                            that.$scope.groups.sort(function (a, b) {
                                if (a.Name == "Special")
                                    return -2;
                                if (a.Name < b.Name)
                                    return -1;
                                if (a.Name > b.Name)
                                    return 1;
                                return 0;
                            });
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

                    uConditionApiService.GetActions()
                        .then(function (predicateConfigs) {
                            var groups = {};
                            for (var i = 0; i < predicateConfigs.length; i++) {
                                if (typeof groups[predicateConfigs[i].Category] == "undefined")
                                    groups[predicateConfigs[i].Category] = [];
                                var predicate = new uCondition.Editor.Models.Action();
                                predicate.Config = predicateConfigs[i];
                                groups[predicateConfigs[i].Category].push(predicate);
                            }
                            that.$scope.groups = [];
                            for (var group in groups) {
                                var predicateGroup = new uCondition.Editor.Models.Dialogs.ConditionGroup();
                                predicateGroup.Name = group;
                                for (var i = 0; i < groups[group].length; i++) {
                                    var predicateOption = new uCondition.Editor.Models.Dialogs.ConditionOption();
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
        })(Controllers = Editor.Controllers || (Editor.Controllers = {}));
    })(Editor = uCondition.Editor || (uCondition.Editor = {}));
})(uCondition || (uCondition = {}));
//# sourceMappingURL=controllers.js.map