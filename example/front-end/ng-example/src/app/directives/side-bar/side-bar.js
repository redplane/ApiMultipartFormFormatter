module.exports = (ngModule) => {
    // Module template import.
    let ngModuleHtmlTemplate = require('./side-bar.html');

    // Directive declaration.
    ngModule.directive('sideBar', () => {
        return {
            template: ngModuleHtmlTemplate,
            restrict: 'E',
            scope: null,
            controller: ($scope, urlStatesConstant) => {

                //#region Properties

                // Constants reflection.
                $scope.urlStates = urlStatesConstant;
                //#endregion
            }
        }
    });
};