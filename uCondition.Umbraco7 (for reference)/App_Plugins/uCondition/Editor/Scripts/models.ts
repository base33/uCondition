
module uCondition.Editor.Models {
    export class uConditionModel {
        public PredicateGroups: Array<Swimlane>;

        public constructor() {
            this.PredicateGroups = [new Swimlane()];
        }
    }

    export class DataTypeConfig {
        public EnableActions: boolean = false;
        public EnableAlternativeConditions: boolean = true;

        public static CleanUp(config: DataTypeConfig): void {
            config.EnableActions = config.EnableActions == true || (<string><any>config.EnableActions) == "1";
            config.EnableAlternativeConditions = config.EnableAlternativeConditions == true || (<string><any>config.EnableAlternativeConditions) == "1";
        }
    }

    export abstract class Condition {
        public Type: string;
        public RightOperationAnd: boolean;

        public constructor(type: string) {
            this.Type = type;
            this.RightOperationAnd = true;
        }
    }

    export class PredicateGroup extends Condition {
        public Conditions: Array<Condition>;

        public constructor(type: string = "PredicateGroup") {
            super(type);
            this.Conditions = new Array<Condition>();
        }
    }

    export class Predicate extends Condition {
        public Config: IConfig;
        public Invert: boolean = false;
        public Values: Array<IEditablePropertyValue> = new Array<IEditablePropertyValue>();
        public NeedsConfiguring: boolean = true;
        public Missing: boolean = false;

        public constructor(type: string = "Predicate") {
            super(type);
        }
    }

    export class Action {
        public Config: IConfig;
        public Values: Array<IEditablePropertyValue> = new Array<IEditablePropertyValue>();
    }

    export class Swimlane extends PredicateGroup {
        public Description: string;
        public Actions: Array<Action> = new Array<Action>();

        public constructor() {
            super("Swimlane");
            this.Description = "";
        }
    }

    export interface IConfig {
        Name: string;
        Alias: string;
        Icon: string;
        Category: string;
        Fields: Array<IEditableProperty>;
    }

    export interface IEditableProperty {
        Name: string;
        Alias: string;
        Control: string;
        ValueType: string;
    }

    export class EditablePropertyDisplayMode {
        public static Standard: string = "Standard";
        public static IsBoolean: string = "IsBoolean";
        public static IsPreValue: string = "IsPreValue";
        public static Hidden: string = "Hidden";
    }

    export interface IEditablePropertyValue {
        Alias: string;
        Value: any;
        DisplayName: any;
        DisplayValue: any;
    }

    export class ModalDialog {
        public title: string = "";
        public subtitle: string = "";
        public view: string = "";
        public show: boolean = false;
        public filter: boolean = false;
        public event: () => void = () => { };
        public submit: (model: any) => void = (model) => { };
        public submitButtonLabel: string = "";
        public closeButtonLabel: string = "";
        public dialogData: any = {};
        public value = {};
    }
}

module uCondition.Editor.Models.Dialogs {
    export class ConditionGroup {
        public Name: string = "";
        public Show: boolean = true;
        public Conditions: Array<ConditionOption> = new Array<ConditionOption>();
    }

    export class ConditionOption {
        public Condition: Condition = null;
        public Selected: boolean = false;
        public Show: boolean = true;
    }
}