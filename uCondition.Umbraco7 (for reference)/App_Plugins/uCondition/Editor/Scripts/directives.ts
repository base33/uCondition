
angular.module("umbraco").directive("uconditionSwimlane", function () {
    return {
        scope: {
            model: "=",
            container: "=",
            isLast: "=",
            modalDialog: "=",
            isPreValue: "@",
            preview: "@",
            enableSwimlaneDelete: '='
        },
        restrict: 'E',
        replace: true,
        template: '<ng-include src="\'/app_plugins/uCondition/editor/uCondition.swimlane.html\'" />',
        link: function (scope, element, attrs, ctrl) {

        }
    };
});

angular.module("umbraco").directive("uconditionCondition", function () {
    return {
        scope: {
            model: "=",
            container: "=",
            isLast: "=",
            modalDialog: "=",
            isPreValue: "@",
            preview: "@"
        },
        restrict: 'E',
        replace: true,
        template: '<ng-include src="\'/app_plugins/uCondition/editor/uCondition.predicategroup.html\'" />',
        link: function (scope, element, attrs, ctrl) {

        }
    };
});

angular.module("umbraco").directive("uconditionContenteditable", function () {
    return {
        restrict: "A",
        require: "ngModel",
        link: function (scope, element, attrs, ngModel) {

            function read() {
                ngModel.$setViewValue(element.html());
            }

            ngModel.$render = function () {
                element.html(ngModel.$viewValue || "");
            };

            element.bind("blur keyup change", function () {
                scope.$apply(read);
            });
        }
    };
});


(function () {
    "use strict";

    // Vertilize Container
    angular.module("umbraco").directive('uconditionVertilizeContainer', [
        function () {
            return {
                restrict: 'EA',
                controller: [
                    '$scope', '$window',
                    function ($scope, $window) {
                        // Alias this
                        var _this = this;

                        // Array of children heights
                        _this.childrenHeights = [];

                        // API: Allocate child, return index for tracking.
                        _this.allocateMe = function () {
                            _this.childrenHeights.push(0);
                            return (_this.childrenHeights.length - 1);
                        };

                        // API: Update a child's height
                        _this.updateMyHeight = function (index, height) {
                            _this.childrenHeights[index] = height;
                        };

                        // API: Get tallest height
                        _this.getTallestHeight = function () {
                            var height = 0;
                            for (var i = 0; i < _this.childrenHeights.length; i = i + 1) {
                                height = Math.max(height, _this.childrenHeights[i]);
                            }
                            return height;
                        };

                        // Add window resize to digest cycle
                        angular.element($window).bind('resize', function () {
                            return $scope.$apply();
                        });
                    }
                ]
            };
        }
    ]);

    // Vertilize Item
    angular.module("umbraco").directive('uconditionVertilize', [
        function () {
            return {
                restrict: 'EA',
                require: '^uconditionVertilizeContainer',
                link: function (scope, element, attrs, parent) {
                    // My index allocation
                    var myIndex = parent.allocateMe();

                    // Get my real height by cloning so my height is not affected.
                    var getMyRealHeight = function () {
                        var clone = element.clone()
                            .removeAttr('uCondition-vertilize')
                            .css({
                                height: '',
                                width: element.outerWidth(),
                                position: 'fixed',
                                top: 0,
                                left: 0,
                                visibility: 'hidden'
                            });
                        element.after(clone);
                        var realHeight = clone.height();
                        clone['remove']();
                        return realHeight;
                    };

                    // Watch my height
                    scope.$watch(getMyRealHeight, function (myNewHeight) {
                        if (myNewHeight) {
                            parent.updateMyHeight(myIndex, myNewHeight);
                        }
                    });

                    // Watch for tallest height change
                    scope.$watch(parent.getTallestHeight, function (tallestHeight) {
                        if (tallestHeight) {
                            element.css('height', tallestHeight);
                        }
                    });
                }
            };
        }
    ]);

} ());