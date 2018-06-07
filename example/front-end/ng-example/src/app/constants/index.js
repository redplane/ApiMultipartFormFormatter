module.exports = function (ngModule) {
    /*
    * Constants declaration.
    * */
    require('./app-settings.constant')(ngModule);
    require('./api-url.constant')(ngModule);
};