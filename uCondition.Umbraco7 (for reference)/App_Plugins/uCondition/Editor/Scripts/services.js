var uCondition;
(function (uCondition) {
    var Editor;
    (function (Editor) {
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
                    this.uConditionApiService.GetPredicates().success(function (predicateConfigs) {
                        for (var i = 0; i < predicateGroups.length; i++) {
                            that.SyncPredicateGroup(predicateGroups[i], predicateConfigs);
                        }
                    });
                };
                PredicateSyncService.prototype.SyncPredicateGroup = function (predicateGroup, predicateConfigs) {
                    for (var i = 0; i < predicateGroup.Conditions.length; i++) {
                        var currentPredicate = predicateGroup.Conditions[i];
                        if (currentPredicate.Type == "PredicateGroup") {
                            this.SyncPredicateGroup(currentPredicate, predicateConfigs);
                        }
                        else {
                            var predicateConfig = this.FindPredicateConfigByAlias(currentPredicate.Config.Alias, predicateConfigs);
                            if (predicateConfig != null) {
                                this.SyncPredicate(currentPredicate, predicateConfig);
                                currentPredicate.Missing = false;
                            }
                            else {
                                currentPredicate.Missing = true;
                            }
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
        })(Services = Editor.Services || (Editor.Services = {}));
    })(Editor = uCondition.Editor || (uCondition.Editor = {}));
})(uCondition || (uCondition = {}));
//# sourceMappingURL=services.js.map