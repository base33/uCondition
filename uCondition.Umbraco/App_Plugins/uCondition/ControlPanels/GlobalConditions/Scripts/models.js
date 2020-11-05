var uCondition;
(function (uCondition) {
    var GlobalConditions;
    (function (GlobalConditions) {
        var Models;
        (function (Models) {
            var Condition = (function () {
                function Condition() {
                    this.Id = 0;
                    this.Guid = this.generateGuid();
                    this.Name = "";
                    this.Condition = new uCondition.Editor.Models.uConditionModel();
                    this.Created = "";
                    this.LastUpdated = "";
                }
                Condition.prototype.generateGuid = function () {
                    function s4() {
                        return Math.floor((1 + Math.random()) * 0x10000)
                            .toString(16)
                            .substring(1);
                    }
                    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
                        s4() + '-' + s4() + s4() + s4();
                };
                return Condition;
            }());
            Models.Condition = Condition;
            var ConditionDisplay = (function () {
                function ConditionDisplay() {
                    this.Condition = new Condition();
                    this.Editing = false;
                }
                return ConditionDisplay;
            }());
            Models.ConditionDisplay = ConditionDisplay;
        })(Models = GlobalConditions.Models || (GlobalConditions.Models = {}));
    })(GlobalConditions = uCondition.GlobalConditions || (uCondition.GlobalConditions = {}));
})(uCondition || (uCondition = {}));
//# sourceMappingURL=models.js.map