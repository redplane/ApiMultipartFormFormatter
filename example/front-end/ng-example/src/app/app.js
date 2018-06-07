'use strict';

// Css imports.
require('../../node_modules/bootstrap/dist/css/bootstrap.css');
require('../../node_modules/bootstrap/dist/css/bootstrap-theme.css');

require('../../node_modules/angular-toastr/dist/angular-toastr.css');

// Font awesome.
require('../../node_modules/font-awesome/css/font-awesome.css');
require('../../node_modules/angular-block-ui/dist/angular-block-ui.css');

// Moment picker.
require('../../node_modules/angular-moment-picker/dist/angular-moment-picker.css');
require('../../node_modules/angular-confirm1/css/angular-confirm.css');

// Import app style.
require('./app.scss');

// Import jquery lib.
require('jquery');
require('bluebird');
require('bootstrap');
require('moment');
require('@uirouter/angularjs');

// Angular plugins declaration.
let angular = require('angular');
require('@uirouter/angularjs');
require('angular-block-ui');
require('angular-toastr');
require('angular-translate');
require('angular-translate-loader-static-files');
require('angular-moment');
require('angular-moment-picker');
require('ng-data-annotation');
require('angular-file-upload');
require('angular-confirm1');

// Module declaration.
let ngModule = angular.module('ngApp', [
    'ui.router', 'blockUI', 'toastr', 'pascalprecht.translate',
    'angularMoment', 'moment-picker', 'ngDataAnnotations', 'angularFileUpload', 'cp.ngConfirm']);

ngModule.config(($urlRouterProvider, $translateProvider, $httpProvider) => {

    // API interceptor
    $httpProvider.interceptors.push('apiInterceptor');

    // Url router config.
    $urlRouterProvider.otherwise('/basic-upload');

    // Translation config.
    $translateProvider.useStaticFilesLoader({
        prefix: './assets/dictionary/',
        suffix: '.json'
    });

    // en-US is default selection.
    $translateProvider.use('en-US');

});

// Constants import.
require('./constants/index')(ngModule);

// Factories import.
require('./factories/index')(ngModule);

// Services import.
require('./services/index')(ngModule);

// Directive requirements.
require('./directives/index')(ngModule);

// Module requirements.
require('./modules/index')(ngModule);