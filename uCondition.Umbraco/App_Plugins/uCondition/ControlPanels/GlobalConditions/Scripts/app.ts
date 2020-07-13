declare var $: any;
declare var angular: any;

angular.module("umbraco").controller("uCondition.GlobalConditions", ["$scope", "globalConditionsApi", "notificationsService", uCondition.GlobalConditions.Controllers.GlobalConditionsCtrl]);
angular.module("umbraco").controller("uCondition.GlobalConditions.Editor", ["$scope", "globalConditionsApi", uCondition.GlobalConditions.Controllers.GlobalConditionEditorCtrl]);
angular.module("umbraco").service("globalConditionsApi", ["$http", uCondition.GlobalConditions.Services.ApiService]);