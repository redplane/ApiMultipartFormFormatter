module.exports = function (ngModule) {
    require('./ui.service')(ngModule);
    require('./upload.service')(ngModule);
};