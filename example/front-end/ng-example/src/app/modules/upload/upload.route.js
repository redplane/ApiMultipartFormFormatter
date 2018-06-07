module.exports = (ngModule) => {

    const ngHtmlMasterLayout = require('./master-layout/master-layout.html');
    const ngHtmlBasicUpload = require('./basic-upload/basic-upload.html');
    const ngHtmlListUpload = require('./list-upload/list-upload.html');
    const ngHtmlNestedInfoUpload = require('./nested-info-upload/nested-info-upload.html');

    ngModule.config(($stateProvider) => {

        $stateProvider.state('upload', {
            url: null,
            controller: 'uploadMasterLayoutController',
            template: ngHtmlMasterLayout,
            redirectTo: '/basic-upload'
        });

        $stateProvider.state('basic-upload', {
            url: '/basic-upload',
            controller: 'basicUploadController',
            template: ngHtmlBasicUpload,
            parent: 'upload'
        });

        $stateProvider.state('list-upload', {
            url: '/list-upload',
            controller: 'listUploadController',
            template: ngHtmlListUpload,
            parent: 'upload'
        });

        $stateProvider.state('nested-info-upload', {
            url: '/nest-info-upload',
            controller: 'nestedInfoUploadController',
            template: ngHtmlNestedInfoUpload,
            parent: 'upload'
        });

    });
};