module.exports = (ngModule) => {
    require('./basic-upload/basic-upload.controller')(ngModule);
    require('./list-upload/list-upload.controller')(ngModule);
    require('./master-layout/master-layout.controller')(ngModule);
    require('./nested-info-upload/nested-info-upload.controller')(ngModule);

    require('./upload.route')(ngModule);
};