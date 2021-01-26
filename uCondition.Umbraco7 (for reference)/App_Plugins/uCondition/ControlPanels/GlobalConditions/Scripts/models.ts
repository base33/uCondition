
module uCondition.GlobalConditions.Models {
    export class Condition {
        public Id: number;
        public Guid: string;
        public Name: string;
        public Condition: uCondition.Editor.Models.uConditionModel;
        public Created: string;
        public LastUpdated: string;

        public constructor() {
            this.Id = 0;
            this.Guid = this.generateGuid();
            this.Name = "";
            this.Condition = new uCondition.Editor.Models.uConditionModel();
            this.Created = "";
            this.LastUpdated = "";
        }

        private generateGuid() : string {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                    .toString(16)
                    .substring(1);
            }
            return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
                s4() + '-' + s4() + s4() + s4();
        }
    }

    export class ConditionDisplay {
        public Condition: Condition = new Condition();
        public Editing: boolean = false;
    }
}