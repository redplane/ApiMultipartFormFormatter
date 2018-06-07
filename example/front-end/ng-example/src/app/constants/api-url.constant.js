module.exports = (ngModule) => {
    ngModule.constant('apiUrlConstant', {
        apiBaseUrl: API_ENDPOINT,

        upload:{
            basicUpload: 'api/upload/basic-upload',
            listUpload: 'api/upload/attachments-list-upload',
            nestedInfoUpload: 'api/upload/nested-info-upload'
        }
    })
};