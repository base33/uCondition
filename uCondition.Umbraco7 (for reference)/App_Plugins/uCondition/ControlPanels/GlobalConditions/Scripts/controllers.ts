
module uCondition.GlobalConditions.Controllers {
    export class GlobalConditionsCtrl {
        public GlobalConditionsApi: uCondition.GlobalConditions.Services.ApiService;
        public NotificationsService: any;
        public Conditions: Array<uCondition.GlobalConditions.Models.ConditionDisplay>;

        public CreateMode: boolean = false;
        public CreateModel: uCondition.GlobalConditions.Models.ConditionDisplay = null;

        public EditIndex: number = -1;

        public constructor($scope: any, globalConditionsApi: uCondition.GlobalConditions.Services.ApiService, notificationsService: any) {
            this.GlobalConditionsApi = globalConditionsApi;
            this.NotificationsService = notificationsService;
            this.Conditions = new Array<uCondition.GlobalConditions.Models.ConditionDisplay>();

            this.LoadAllConditions();
        }

        public LoadAllConditions() {
            this.Conditions = new Array<uCondition.GlobalConditions.Models.ConditionDisplay>();
            var that = this;
            this.GlobalConditionsApi.GetAllGlobalConditions().success(
                (conditions: Array<uCondition.GlobalConditions.Models.Condition>) => {
                    for (var i = 0; i < conditions.length; i++) {
                        var conditionDisplay = new uCondition.GlobalConditions.Models.ConditionDisplay();
                        conditionDisplay.Condition = conditions[i];
                        that.Conditions.push(conditionDisplay);
                    }
                });
        }

        public CreateCondition() {
            this.CreateModel = new uCondition.GlobalConditions.Models.ConditionDisplay();
            this.CreateMode = true;
        }

        public CancelCreateCondition(condition: uCondition.GlobalConditions.Models.ConditionDisplay) {
            this.CreateMode = false;
            this.CreateModel = null;
            this.EditIndex = -1;
            condition.Editing = false;
        }

        public EditCondition(condition: uCondition.GlobalConditions.Models.ConditionDisplay, index: number) {
            if (this.EditIndex >= 0)
                return;

            condition.Editing = true;
            this.EditIndex = index;
        }

        public SaveCondition(conditionDisplay: uCondition.GlobalConditions.Models.ConditionDisplay) {
            var createMode = conditionDisplay.Condition.Id == 0;

            this.GlobalConditionsApi.SaveGlobalCondition(conditionDisplay.Condition).success(
                (persistedCondition) => {
                    if (createMode)
                        this.Conditions.push(conditionDisplay);

                    conditionDisplay.Condition = persistedCondition;

                    if (createMode)
                        this.NotificationsService.success("Global Condition created!");
                    else
                        this.NotificationsService.success("Global Condition updated!");
                });

            if (createMode) {
                this.CreateModel = null;
                this.CreateMode = false;
            }
            else {
                conditionDisplay.Editing = false;
                this.EditIndex = -1;
            }
        }

        public DeleteCondition(conditionDisplay: uCondition.GlobalConditions.Models.ConditionDisplay) {
            if (this.EditIndex >= 0)
                return;

            if (confirm("Are you sure you want to delete '" + conditionDisplay.Condition.Name + "'?")) {
                this.GlobalConditionsApi.DeleteGlobalCondition(conditionDisplay.Condition.Id);

                var index = this.Conditions.indexOf(conditionDisplay)
                this.Conditions.splice(index, 1);
            }
        }
    }

    export class GlobalConditionEditorCtrl {
        public constructor(public $scope: any, public GlobalConditionsApi: uCondition.GlobalConditions.Services.ApiService) {
            $scope.editValue = angular.copy($scope.model);
            $scope.editModel =
                {
                    view: "/app_plugins/ucondition/editor/ucondition.html",
                    alias: "test",
                    value: $scope.editValue.Condition,
                    preview: false,
                    config: <uCondition.Editor.Models.DataTypeConfig>{
                        EnableAlternativeConditions: false
                    }
                };
        }

        public Accept() {
            this.$scope.model.Condition = this.$scope.editModel.value;
            this.$scope.model.Name = this.$scope.editValue.Name;
            this.$scope.accept();
        }
    }
}