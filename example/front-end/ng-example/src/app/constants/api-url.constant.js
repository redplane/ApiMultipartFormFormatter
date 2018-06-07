module.exports = (ngModule) => {
    ngModule.constant('apiUrlConstant', {
        apiBaseUrl: 'http://localhost:51506',

        upload:{
            basicUpload: 'api/upload/basic-upload',
            listUpload: 'api/upload/attachments-list-upload',
            nestedInfoUpload: 'api/upload/nested-info-upload'
        }
    })
};