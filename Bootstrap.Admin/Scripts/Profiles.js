﻿$(function () {
    var $headerIcon = $('#headerIcon');
    var preIcon = $headerIcon.attr('src');
    $('#fileIcon').fileinput({
        uploadUrl: Profiles.url,
        language: 'zh',
        maxFileSize: 5000,
        allowedFileExtensions: ['jpg', 'png', 'bmp', 'gif', 'jpeg'],
        initialPreview: [
            preIcon
        ],
        initialPreviewConfig: [
            { caption: "现在头像", size: $('#fileIcon').attr('data-init'), showZoom: false },
        ],
        initialPreviewAsData: true,
        overwriteInitial: true,
        dropZoneTitle: "请选择头像"
    }).on('fileuploaded', function (event, data, previewId, index) {
        var url = data.response;
        if (!!url) $headerIcon.attr('src', url);
    });

    $('#infoDataForm').autoValidate({
        displayName: {
            required: true,
            maxlength: 50
        }
    }, {
            button: ['btnSaveDisplayName']
        });
    $('#passwordDataForm').autoValidate({
        currentPassword: {
            required: true,
            maxlength: 50
        },
        newPassword: {
            required: true,
            maxlength: 50
        },
        confirmPassword: {
            required: true,
            equalTo: "#newPassword",
            maxlength: 50
        }
    }, {
            button: ['btnSavePassword']
        });

    var bsa = new BootstrapAdmin({
        url: Profiles.url,
        bootstrapTable: null,
        dataEntity: new DataEntity({
            map: {
                Password: "currentPassword",
                NewPassword: "newPassword",
                DisplayName: "displayName",
                UserName: "userName",
                Css: "css"
            }
        }),
        click: {
            assign: [{
                id: 'btnSavePassword',
                click: function (row, data) {
                    if ($(this).attr('data-valid') == "true") {
                        data.UserStatus = 2;
                        $.bc({ url: User.url, method: "PUT", data: data, title: "更改密码" });
                    }
                }
            }, {
                id: 'btnSaveDisplayName',
                click: function (row, data) {
                    if ($(this).attr('data-valid') == "true") {
                        data.UserStatus = 1;
                        $.bc({
                            url: User.url, method: "PUT", data: data, title: "修改用户显示名称",
                            callback: function (result) {
                                if (result) {
                                    $('#userDisplayName').text(data.DisplayName);
                                }
                            }
                        });
                    }
                }
            }, {
                id: 'btnSaveCss',
                click: function (row, data) {
                    data.UserStatus = 3;
                    $.bc({ url: User.url, method: "PUT", data: data, title: "保存样式" });
                }
            }]
        }
    });

    $('button[data-admin="False"]').removeAttr('disabled');
    $('#kvFileinputModal').appendTo('body');
});