var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var uCondition;
(function (uCondition) {
    var Models;
    (function (Models) {
        var uConditionModel = (function () {
            function uConditionModel() {
                this.PredicateGroups = new Array();
            }
            return uConditionModel;
        }());
        Models.uConditionModel = uConditionModel;
        var DataTypeConfig = (function () {
            function DataTypeConfig() {
                this.EnableActions = false;
            }
            DataTypeConfig.CleanUp = function (config) {
                config.EnableActions = config.EnableActions == true || config.EnableActions == "1";
            };
            return DataTypeConfig;
        }());
        Models.DataTypeConfig = DataTypeConfig;
        var Condition = (function () {
            function Condition(type) {
                this.Type = type;
                this.RightOperationAnd = true;
            }
            return Condition;
        }());
        Models.Condition = Condition;
        var PredicateGroup = (function (_super) {
            __extends(PredicateGroup, _super);
            function PredicateGroup(type) {
                if (type === void 0) { type = "PredicateGroup"; }
                _super.call(this, type);
                this.Description = "";
                this.Conditions = new Array();
            }
            return PredicateGroup;
        }(Condition));
        Models.PredicateGroup = PredicateGroup;
        var Predicate = (function (_super) {
            __extends(Predicate, _super);
            function Predicate(type) {
                if (type === void 0) { type = "Predicate"; }
                _super.call(this, type);
                this.Invert = false;
                this.Values = new Array();
                this.NeedsConfiguring = true;
            }
            return Predicate;
        }(Condition));
        Models.Predicate = Predicate;
        var Action = (function () {
            function Action() {
                this.Values = new Array();
            }
            return Action;
        }());
        Models.Action = Action;
        var Swimlane = (function (_super) {
            __extends(Swimlane, _super);
            function Swimlane() {
                _super.call(this, "Swimlane");
                this.Actions = new Array();
            }
            return Swimlane;
        }(PredicateGroup));
        Models.Swimlane = Swimlane;
        var EditablePropertyDisplayMode = (function () {
            function EditablePropertyDisplayMode() {
            }
            EditablePropertyDisplayMode.Standard = "Standard";
            EditablePropertyDisplayMode.IsBoolean = "IsBoolean";
            EditablePropertyDisplayMode.IsPreValue = "IsPreValue";
            EditablePropertyDisplayMode.Hidden = "Hidden";
            return EditablePropertyDisplayMode;
        }());
        Models.EditablePropertyDisplayMode = EditablePropertyDisplayMode;
        var ModalDialog = (function () {
            function ModalDialog() {
                this.title = "";
                this.subtitle = "";
                this.view = "";
                this.show = false;
                this.filter = false;
                this.event = function () { };
                this.submit = function (model) { };
                this.submitButtonLabel = "";
                this.closeButtonLabel = "";
                this.dialogData = {};
                this.value = {};
            }
            return ModalDialog;
        }());
        Models.ModalDialog = ModalDialog;
    })(Models = uCondition.Models || (uCondition.Models = {}));
})(uCondition || (uCondition = {}));
var uCondition;
(function (uCondition) {
    var Models;
    (function (Models) {
        var Dialogs;
        (function (Dialogs) {
            var ConditionGroup = (function () {
                function ConditionGroup() {
                    this.Name = "";
                    this.Show = true;
                    this.Conditions = new Array();
                }
                return ConditionGroup;
            }());
            Dialogs.ConditionGroup = ConditionGroup;
            var ConditionOption = (function () {
                function ConditionOption() {
                    this.Condition = null;
                    this.Selected = false;
                    this.Show = true;
                }
                return ConditionOption;
            }());
            Dialogs.ConditionOption = ConditionOption;
        })(Dialogs = Models.Dialogs || (Models.Dialogs = {}));
    })(Models = uCondition.Models || (uCondition.Models = {}));
})(uCondition || (uCondition = {}));
