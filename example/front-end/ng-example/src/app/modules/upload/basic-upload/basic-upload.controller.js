module.exports = (ngModule) => {
    ngModule.controller('basicUploadController', (FileUploader,
                                                  $upload, $ngConfirm,
                                                  $compile, $interpolate,
                                                  $scope) => {

        //#region Properties

        $scope.fileUploader = new FileUploader({});

        $scope.oModel = {
            author: {
                fullName: null
            },
            attachment: null
        };

        //#endregion

        //#region Methods

        /*
        * Called when upload is clicked.
        * */
        $scope.clickUpload = () => {
            $upload
                .basicUpload($scope.oModel.author.fullName, $scope.oModel.attachment)
                .then((basicUploadResult) => {

                    let messages = basicUploadResult.messages;
                    let htmlMessage = '';
                    htmlMessage += '<ul>';
                    for (let message of messages)
                        htmlMessage += `<li>${message}</li>`;
                    htmlMessage += '</ul>';

                    $ngConfirm({
                        title: 'Api response',
                        content: htmlMessage,
                        scope: $scope,
                        buttons: {
                            // long hand button definition
                            ok: {
                                text: "ok!",
                                btnClass: 'btn-primary',
                                keys: ['enter'], // will trigger when enter is pressed
                                action: (scope) => {
                                    return true;
                                }
                            }
                        },
                    });
                })
        };

        //#endregion

        //#region Events

        /*
        * Called when file is added.
        * */
        $scope.fileUploader.onAfterAddingFile = (fileItem) => {
            $scope.oModel.attachment = fileItem._file;
        };

        //#endregion
    });
};