module.exports = (ngModule) => {
    ngModule.controller('listUploadController', (FileUploader,
                                                 $upload, $ngConfirm,
                                                 $compile, $interpolate,
                                                 $scope) => {

        //#region Properties

        $scope.fileUploader = new FileUploader({});

        $scope.oModel = {
            author: {
                fullName: null
            },
            attachments: []

        };

        $scope.bIsEmptyAttachment = true;

        //#endregion

        //#region Methods

        /*
        * Called when upload is clicked.
        * */
        $scope.clickUpload = () => {
            $upload
                .listUpload($scope.oModel.author.fullName, $scope.oModel.attachments)
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

        /*
        * Called when delete pending file button is clicked.
        * */
        $scope.deletePendingFile = (attachment) => {

            let attachments = $scope.oModel.attachments;
            if (!attachments || attachments.length < 1)
                return;

            let iIndex = attachments.indexOf(attachment);
            if (iIndex === -1)
                return;

            attachments.splice(iIndex, 1);
        };

        //#endregion

        //#region Events

        /*
        * Called when file is added.
        * */
        $scope.fileUploader.onAfterAddingFile = (fileItem) => {
            let attachments = $scope.oModel.attachments;
            attachments.push(fileItem._file);
            $scope.oModel.attachments = attachments;
        };

        //#endregion

        //#region Watcher


        $scope.$watch(() => {
            return JSON.stringify($scope.oModel.attachments);
        }, (value) => {
            let attachments = $scope.oModel.attachments;
            if (attachments == null || !(attachments instanceof Array) || attachments.length < 1) {
                $scope.bIsEmptyAttachment = true;
                return;
            }

            $scope.bIsEmptyAttachment = false;
        })

        //#endregion
    });
};