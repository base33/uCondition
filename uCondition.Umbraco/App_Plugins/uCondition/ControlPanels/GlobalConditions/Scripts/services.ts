
module uCondition.GlobalConditions.Services {
    export class ApiService {
        public constructor(public $http: any) {

        }

        public GetAllGlobalConditions() {
            return this.$http.get("/umbraco/backoffice/Api/uConditionApi/GetAllGlobalConditions");
        }

        public GetGlobalConditionData(guid: string) {
            return this.$http.get("/umbraco/backoffice/Api/uConditionApi/GetGlobalConditionData?guid=" + guid);
        }

        public SaveGlobalCondition(globalCondition) {
            return this.$http.post("/umbraco/backoffice/Api/uConditionApi/SaveGlobalCondition", angular.toJson(globalCondition));
        }

        public DeleteGlobalCondition(id: number) {
            return this.$http.delete("/umbraco/backoffice/Api/uConditionApi/DeleteGlobalCondition?id=" + id);
        }
    }
}