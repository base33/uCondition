var uCondition;
(function (uCondition) {
    var GlobalConditions;
    (function (GlobalConditions) {
        var Services;
        (function (Services) {
            var ApiService = (function () {
                function ApiService($http) {
                    this.$http = $http;
                }
                ApiService.prototype.GetAllGlobalConditions = function () {
                    return this.$http.get("/umbraco/backoffice/Api/uConditionApi/GetAllGlobalConditions");
                };
                ApiService.prototype.GetGlobalConditionData = function (guid) {
                    return this.$http.get("/umbraco/backoffice/Api/uConditionApi/GetGlobalConditionData?guid=" + guid);
                };
                ApiService.prototype.SaveGlobalCondition = function (globalCondition) {
                    return this.$http.post("/umbraco/backoffice/Api/uConditionApi/SaveGlobalCondition", angular.toJson(globalCondition));
                };
                ApiService.prototype.DeleteGlobalCondition = function (id) {
                    return this.$http.delete("/umbraco/backoffice/Api/uConditionApi/DeleteGlobalCondition?id=" + id);
                };
                return ApiService;
            }());
            Services.ApiService = ApiService;
        })(Services = GlobalConditions.Services || (GlobalConditions.Services = {}));
    })(GlobalConditions = uCondition.GlobalConditions || (uCondition.GlobalConditions = {}));
})(uCondition || (uCondition = {}));
//# sourceMappingURL=services.js.map