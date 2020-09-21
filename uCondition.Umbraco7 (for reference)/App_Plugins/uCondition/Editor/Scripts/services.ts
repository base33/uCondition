
module uCondition.Editor.Services {
    export class uConditionApiService {

        public constructor(public $http: any) {

        }

        public GetPredicates(): any {
            return this.$http.get("/umbraco/backoffice/Api/uConditionApi/GetPredicates");
        }

        public GetActions(): any {
            return this.$http.get("/umbraco/backoffice/Api/uConditionApi/GetActions");
        }

        public GetPropertyEditorByName(propertyEditorName: string): any {
            return this.$http.get("/umbraco/backoffice/Api/uConditionApi/GetPropertyEditorByName?datatypeName=" + propertyEditorName);
        }
    }

    export class PredicateSyncService {
        public constructor(public uConditionApiService: uConditionApiService) {

        }

        public SyncPredicateGroups(predicateGroups: Array<Editor.Models.PredicateGroup>) {
            var that = this;
            this.uConditionApiService.GetPredicates().success(function (predicateConfigs) {
                for (var i = 0; i < predicateGroups.length; i++) {
                    that.SyncPredicateGroup(predicateGroups[i], predicateConfigs);
                }
            });
        }

        private SyncPredicateGroup(predicateGroup: Editor.Models.PredicateGroup, predicateConfigs: Array<Editor.Models.IConfig>) {
            for (var i = 0; i < predicateGroup.Conditions.length; i++) {
                var currentPredicate = predicateGroup.Conditions[i];

                if (currentPredicate.Type == "PredicateGroup") {
                    this.SyncPredicateGroup(currentPredicate as Editor.Models.PredicateGroup, predicateConfigs);
                }
                else {
                    var predicateConfig = this.FindPredicateConfigByAlias((<Editor.Models.Predicate>currentPredicate).Config.Alias, predicateConfigs);

                    if (predicateConfig != null) {
                        this.SyncPredicate(currentPredicate as Editor.Models.Predicate, predicateConfig);
                        (<Editor.Models.Predicate>currentPredicate).Missing = false;
                    }
                    else {
                        (<Editor.Models.Predicate>currentPredicate).Missing = true;
                    }
                }
            }
        }

        private SyncPredicate(predicate: Editor.Models.Predicate, predicateConfig: Editor.Models.IConfig) {
            predicate.Config = predicateConfig;
        }

        private FindPredicateConfigByAlias(alias: string, predicateConfigs: Array<Editor.Models.IConfig>): Editor.Models.IConfig {
            for (var i = 0; i < predicateConfigs.length; i++) {
                if (predicateConfigs[i].Alias == alias)
                    return predicateConfigs[i];
            }
            return null;
        }
    }
}