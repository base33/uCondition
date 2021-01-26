angular.module("umbraco").controller("uCondition", ["$scope", "PredicateSyncService", uCondition.Editor.Controllers.uConditionController]);
angular.module("umbraco").controller("uCondition.PredicateGroup", ["$scope", "$timeout", "dialogService", uCondition.Editor.Controllers.PredicateGroupController]);
angular.module("umbraco").controller("uCondition.Dialogs.AddCondition", ["$scope", "uConditionApiService", uCondition.Editor.Controllers.AddConditionDialog]);
angular.module("umbraco").controller("uCondition.Dialogs.AddAction", ["$scope", "uConditionApiService", uCondition.Editor.Controllers.AddActionDialog]);
angular.module("umbraco").controller("uCondition.Editors.GenericEditor", ["$scope", "uConditionApiService", uCondition.Editor.Controllers.EditConditionDialog]);
angular.module("umbraco").service("uConditionApiService", ["$http", uCondition.Editor.Services.uConditionApiService]);
angular.module("umbraco").service("PredicateSyncService", ["uConditionApiService", uCondition.Editor.Services.PredicateSyncService]);
//# sourceMappingURL=app.js.map