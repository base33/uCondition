var uCondition;
(function (uCondition) {
    var GlobalConditions;
    (function (GlobalConditions) {
        var Controllers;
        (function (Controllers) {
            var GlobalConditionsCtrl = (function () {
                function GlobalConditionsCtrl($scope, globalConditionsApi, notificationsService) {
                    this.CreateMode = false;
                    this.CreateModel = null;
                    this.EditIndex = -1;
                    this.GlobalConditionsApi = globalConditionsApi;
                    this.NotificationsService = notificationsService;
                    this.Conditions = new Array();
                    this.LoadAllConditions();
                }
                GlobalConditionsCtrl.prototype.LoadAllConditions = function () {
                    this.Conditions = new Array();
                    var that = this;
                    this.GlobalConditionsApi.GetAllGlobalConditions().success(function (conditions) {
                        for (var i = 0; i < conditions.length; i++) {
                            var conditionDisplay = new uCondition.GlobalConditions.Models.ConditionDisplay();
                            conditionDisplay.Condition = conditions[i];
                            that.Conditions.push(conditionDisplay);
                        }
                    });
                };
                GlobalConditionsCtrl.prototype.CreateCondition = function () {
                    this.CreateModel = new uCondition.GlobalConditions.Models.ConditionDisplay();
                    this.CreateMode = true;
                };
                GlobalConditionsCtrl.prototype.CancelCreateCondition = function (condition) {
                    this.CreateMode = false;
                    this.CreateModel = null;
                    this.EditIndex = -1;
                    condition.Editing = false;
                };
                GlobalConditionsCtrl.prototype.EditCondition = function (condition, index) {
                    if (this.EditIndex >= 0)
                        return;
                    condition.Editing = true;
                    this.EditIndex = index;
                };
                GlobalConditionsCtrl.prototype.SaveCondition = function (conditionDisplay) {
                    var _this = this;
                    var createMode = conditionDisplay.Condition.Id == 0;
                    this.GlobalConditionsApi.SaveGlobalCondition(conditionDisplay.Condition).success(function (persistedCondition) {
                        if (createMode)
                            _this.Conditions.push(conditionDisplay);
                        conditionDisplay.Condition = persistedCondition;
                        if (createMode)
                            _this.NotificationsService.success("Global Condition created!");
                        else
                            _this.NotificationsService.success("Global Condition updated!");
                    });
                    if (createMode) {
                        this.CreateModel = null;
                        this.CreateMode = false;
                    }
                    else {
                        conditionDisplay.Editing = false;
                        this.EditIndex = -1;
                    }
                };
                GlobalConditionsCtrl.prototype.DeleteCondition = function (conditionDisplay) {
                    if (this.EditIndex >= 0)
                        return;
                    if (confirm("Are you sure you want to delete '" + conditionDisplay.Condition.Name + "'?")) {
                        this.GlobalConditionsApi.DeleteGlobalCondition(conditionDisplay.Condition.Id);
                        var index = this.Conditions.indexOf(conditionDisplay);
                        this.Conditions.splice(index, 1);
                    }
                };
                return GlobalConditionsCtrl;
            }());
            Controllers.GlobalConditionsCtrl = GlobalConditionsCtrl;
            var GlobalConditionEditorCtrl = (function () {
                function GlobalConditionEditorCtrl($scope, GlobalConditionsApi) {
                    this.$scope = $scope;
                    this.GlobalConditionsApi = GlobalConditionsApi;
                    $scope.editValue = angular.copy($scope.model);
                    $scope.editModel =
                        {
                            view: "/app_plugins/ucondition/editor/ucondition.html",
                            alias: "test",
                            value: $scope.editValue.Condition,
                            preview: false,
                            config: {
                                EnableAlternativeConditions: false
                            }
                        };
                }
                GlobalConditionEditorCtrl.prototype.Accept = function () {
                    this.$scope.model.Condition = this.$scope.editModel.value;
                    this.$scope.model.Name = this.$scope.editValue.Name;
                    this.$scope.accept();
                };
                return GlobalConditionEditorCtrl;
            }());
            Controllers.GlobalConditionEditorCtrl = GlobalConditionEditorCtrl;
        })(Controllers = GlobalConditions.Controllers || (GlobalConditions.Controllers = {}));
    })(GlobalConditions = uCondition.GlobalConditions || (uCondition.GlobalConditions = {}));
})(uCondition || (uCondition = {}));
//# sourceMappingURL=controllers.js.map