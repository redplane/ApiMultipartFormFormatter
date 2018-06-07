module.exports = (ngModule) => {
    ngModule.factory('apiInterceptor',
        ($injector,
         $q) => {

            return {
                /*
                * Callback which is fired when request is made.
                * */
                request: (x) => {
                    return x;
                },

                /*
                * Callback which is fired when request is made failingly.
                * */
                requestError: (config) => {
                    return config;
                },

                /*
                * Callback which is fired when response is sent back from back-end.
                * */
                response: (x) => {
                    // Stop blockUI.
                    //blockUI.stop();

                    return x;
                },

                /*
                * Callback which is fired when response is failed.
                * */
                responseError: (x) => {
                    // Response is invalid.
                    if (!x)
                        return $q.reject(x);

                    let url = x.config.url;
                    if (!url || url.indexOf('/api/') === -1)
                        return $q.reject(x);

                    // Find state.
                    let state = $injector.get('$state');

                    // Find toastr notification from injector.
                    let toastr = $injector.get('toastr');

                    // Find translate service using injector.
                    let translate = $injector.get('$translate');

                    let szMessage = '';
                    switch (x.status) {
                        case 400:
                            szMessage = 'Bad request';
                            break;
                        case 401:
                            szMessage = 'Your credential is invalid.';
                            break;
                        case 403:
                            if (x.data)
                                szMessage = translate.instant(x.data.message);
                            break;
                        case 500:
                            szMessage = 'Internal server error';
                            break;
                        default:
                            szMessage = 'Unknown error';
                            break;
                    }

                    if (toastr)
                        toastr.error(szMessage, 'Error');
                    else
                        console.log(szMessage);
                    return $q.reject(x);
                }
            }
        });
};