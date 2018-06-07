module.exports = (ngModule) => {
    // Module template import.
    let ngModuleHtmlTemplate = require('./navigation-bar.html');

    // Directive declaration.
    ngModule.directive('navigationBar', () => {
        return {
            template: ngModuleHtmlTemplate,
            restrict: 'E',
            scope: null,
            controller: ($scope) => {
            }
        }
    });
};