angular.module("umbraco").directive("globalConditionEditor", function () {
    return {
        scope: {
            model: "=",
            accept: "&",
            cancel: "&"
        },
        restrict: 'E',
        replace: true,
        template: '<ng-include src="\'/app_plugins/uCondition/controlpanels/globalconditions/globalconditions.editor.html\'" />',
        link: function (scope, element, attrs, ctrl) {

        }
    };
});