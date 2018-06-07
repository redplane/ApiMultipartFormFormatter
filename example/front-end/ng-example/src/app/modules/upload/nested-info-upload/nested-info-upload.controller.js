module.exports = (ngModule) => {
    ngModule.controller('nestedInfoUploadController', (FileUploader,
                                                       $upload, $ngConfirm,
                                                       $compile, $interpolate,
                                                       $scope) => {

        //#region Properties

        $scope.parentUploader = new FileUploader();
        $scope.childUploader = new FileUploader();

        $scope.oModel = {
            attachment: null,
            profile:{
                name: null,
                attachment: null
            }
        };

        $scope.bIsEmptyAttachment = true;

        //#endregion

        //#region Methods

        /*
        * Called when upload is clicked.
        * */
        $scope.clickUpload = () => {
            $upload
                .nestedInfoUpload($scope.oModel.attachment, $scope.oModel.profile.name, $scope.oModel.profile.attachment)
                .then((listUploadResult) => {

                    let messages = listUploadResult.messages;
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
        $scope.parentUploader.onAfterAddingFile = (fileItem) => {
            $scope.oModel.attachment = fileItem._file;
        };

        /*
        * Called when file is added on profile.
        * */
        $scope.childUploader.onAfterAddingFile = (fileItem) => {
            $scope.oModel.profile.attachment = fileItem._file;
        };

        $scope.$watch('oModel.attachment', (value) => {
            if (value == null || !(value instanceof File)){
                $scope.bIsEmptyAttachment = true;
                return;
            }

            $scope.bIsEmptyAttachment = false;
        });

        //#endregion
    })
};