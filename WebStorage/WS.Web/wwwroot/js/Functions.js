function GetParentId() {
    var parent = sessionStorage.getItem("parentId");
    document.querySelector('input[name=parentId]').value = parent;
    if (parent != 0) {
        $('#backParentId').show();
    }
    return parent;
}
function ShowFileOptions(doc) {
    var tr = document.getElementById(doc)
    $('.filerow').removeClass('selected'); // "Unselect" all the rows
    $('#'+doc).addClass('selected'); // Select the one clicked

    $.ajax({
        type: "Post",
        url: '/Document/FileOptions',
        data: {
            id: doc
        },
        success: function (data, textStatus, jqXHR) {
            $('#file-options').html(data);
        }
    });
}

function ShowDeletedFileOptions(doc) {
    $.ajax({
        type: "Post",
        url: '/Document/DeletedFileOptions',
        data: {
            id: doc
        },
        success: function (data, textStatus, jqXHR) {
            $('#deleted-file-options').html(data);
        }
    });
}

function DoubleClickAction(isFile, id) {
    if (isFile === 0) {
        $('#backParentId').show();
        $.ajax({
            type: "Post",
            url: '/Document/ReturnDocumentList',
            data: {
                parentId: id
            },
            success: function (data, textStatus, jqXHR) {
                $('#dropzone-drop-area').html(data);
            }
        });
    }
    else {
        ViewFile(id);
    }
}
function TurnOnDeletionMode() {
    sessionStorage.setItem("mode", "del");
}
function GoBack() {
    var parentId = GetParentId();
    $.ajax({
        type: "Post",
        url: '/Document/ReturnParent',
        data: {
            id: parentId
        },
        success: function (data, textStatus, jqXHR) {
            $('#dropzone-drop-area').html(data);
        }
    });
}



function GetLinks(docId) {

    $.ajax({
        method: 'GET',
        url: '/Share/GetPublicAccessLink',
        data: {
            documentId: docId,
        },
        success: function (data) {
            if (data.link == "") {
                $('#publicLink').hide();
                $('#closePAccess').hide();
            }
            else {
                $('#openPAccess').hide();
                if (data.isEditable) {
                    $('#sel1').get(0).selectedIndex = 1;

                }
                $('#openPAccess').hide();
                $('#pl').val(data.link);
                $('#pl').select();
            }
        }
    })
    GetAllUser(docId);
}

function CopyToClipboard() {
    $("#pl").select();
    document.execCommand("copy");
}

function sendInvite(docId) {
    var email = $("#privateEmail").val();
    $("#privateEmail").val("");
    var isEdit = $('#sel3').val();
    $.ajax({
        method: 'GET',
        url: '/Share/AddAccessForUser',
        data: {
            documentId: docId,
            guestEmail: email,
            isEditable: isEdit
        },
        success: function (data) {
            console.log(data);
            $('#accessLevelHeader').show();
            $('#closeLAccess').show();

            var selectOptions = isEdit == "true" ? '<option value="false">Просматривать</option><option value="true" selected>Редактировать</option>' :
                '<option value="false">Просматривать</option><option value="true">Редактировать</option>';
            $('.EmailsList').append('<li class="list-group-item">'
                + email +
                '<a href="#"  onclick="deleteUser(' +
                docId + ', \'' + email + '\', this);" ' +
                'class="delete-link pull-right"><i class="glyphicon glyphicon-remove"></i></a>' +
                '<select class="pull-right sel2" onchange="updateAccessForUser(' + docId + ", \'" + email + '\', this);">' +
                selectOptions +
                '</select></li>');
        }
    })
}
function updateAccessForUser(docId, email, tagIsEdit) {
    var isEdit = $(tagIsEdit).val();
    $.ajax({
        method: 'GET',
        url: '/Share/AddAccessForUser',
        data: {
            documentId: docId,
            guestEmail: email,
            isEditable: isEdit
        },
        success: function (data) {
            console.log(data);
        }
    })
}

function GetAllUser(docId) {
    $.ajax({
        method: 'GET',
        url: '/Share/GetAllUsersForSharedDocument',
        data: {
            documentId: docId,
        },
        success: function (data) {
            $('.EmailsList').children().remove();
            for (var i = 0; i < data.length; i++) {
                var selectOptions = data[i].isEditable == true ? '<option value="false">Просматривать</option><option value="true" selected>Редактировать</option>' :
                    '<option value="false">Просматривать</option><option value="true">Редактировать</option>';
                $('.EmailsList').append('<li class="list-group-item">'
                    + data[i].guestEmail +
                    '<a href="#"  onclick="deleteUser(' +
                    docId + ', \'' + data[i].guestEmail + '\', this);" ' +
                    'class="delete-link pull-right"><i class="glyphicon glyphicon-remove deleteIco"></i></a>' +
                    '<select class="pull-right sel2" onchange="updateAccessForUser(' + docId + ", \'" + data[i].guestEmail + '\', this);">' +
                    selectOptions +
                    '</select></li>');
            }
            if ($('.EmailsList').children().length == 0) {
                $('#accessLevelHeader').hide();
                $('#closeLAccess').hide();
            }
        }
    })
}
function openPublicAccess(docId) {

    $.ajax({
        method: 'GET',
        url: '/Share/OpenPublicAccess',
        data: {
            documentId: docId,
            isEditable: 'false'
        },
        success: function (data) {
            $('#publicLink').show();
            $('#sel1').get(0).selectedIndex = 0;
            $('#pl').val(data);
            $('#pl').select();
            $('#openPAccess').hide();
            $('#closePAccess').show();
        }
    })
}

function changeAccessLevel(docId) {
    var isEdit = $('#sel1').val();
    $.ajax({
        method: 'GET',
        url: '/Share/OpenPublicAccess',
        data: {
            documentId: docId,
            isEditable: isEdit
        },
        success: function (data) {
            if (isEdit == 'true') {
                $('#sel1').get(0).selectedIndex = 1;
            }
            $('#pl').val(data);
            $('#pl').select();
        }
    })
}

function closePublicAccess(docId) {

    $.ajax({
        method: 'GET',
        url: '/Share/ClosePublicAccess',
        data: {
            documentId: docId,
        },
        success: function (data) {
            $('#accessLevelHeader').hide();
            $('#publicLink').hide();
            $('#pl').val("");
            $('#closePAccess').hide();
            $('#openPAccess').show();
        }
    })
}

function deleteUser(docId, user, tag) {
    $.ajax({
        method: 'GET',
        url: '/Share/DeleteAccessForUser',
        data: {
            documentId: docId,
            guestEmail: user
        },
        success: function (data) {
            $(tag).parent().remove();
            if ($('.EmailsList').children().length == 0) {
                $('#accessLevelHeader').hide();
                $('#closeLAccess').hide();
            }
        }
    })
}

function closeLimitedAccess(docId) {
    $.ajax({
        method: 'GET',
        url: '/Share/CloseLimitedAccess',
        data: {
            documentId: docId,
        },
        success: function (data) {
            $('.EmailsList').children().remove();
            $('#accessLevelHeader').hide();
            $('#closeLAccess').hide();
        }
    })
}
function SetParentId(id) {
    document.querySelector('input[name=parentId]').value = id;
    if (id == 0) {
        $('#backParentId').hide();
    }
    sessionStorage.setItem("parentId", id)
}
function ContextResult(action, id) {
    if (action == "delete") {
        Url.Action("Delete", "Document", id);
    }
    else if (action == "copy") {
        sessionStorage.setItem("copy", id);
        sessionStorage.setItem("type", "copy");
    }
    else if (action == "cut") {
        sessionStorage.setItem("copy", id);
        sessionStorage.setItem("type", "cut");
    }
    else if (action == "paste") {
        var parent = sessionStorage.getItem("parentId");
        var type = sessionStorage.getItem("type");
        var item = sessionStorage.getItem("copy");
        sessionStorage.setItem("copy", "")
        $.ajax({
            type: "Post",
            url: '/Document/Paste',
            data: {
                id: item,
                parentId: parent,
                type: type
            },
            success: function (data, textStatus, jqXHR) {
                $('#dropzone-drop-area').html(data);
            }
        });
    }
    else if (action == "rename") {
        var row = document.getElementById(id);
        var cell = row.children[0].textContent
        $("#renameModal #ModelId").val(id);
        $("#renameModal #ModelName").val(cell);
        $('#renameModal').modal('show');
    }
    else if (action == "download") {
        Download(id);
    }
    else if (action == "share") {
        $('#myModal').modal('show');
    }
    else if (action == "view") {
        ViewFile(id);
    }
}
function Rename() {
    var id = document.getElementById("ModelId").value;
    var name = document.getElementById("ModelName").value;
    $.ajax({
            type: "Post",
            url: '/Document/Rename',
            data: {
                id: id,
                name: name
            },
        success: function (data, textStatus, jqXHR) {
                $('#renameModal').modal('hide');
                $('#dropzone-drop-area').html(data);
            }
        });
};
function ViewFile(id) {
    var row = document.getElementById(id);
    var name = row.children[0].textContent;
    var isfile = row.children[3].textContent;
    if (isfile == "True") {
        var extention = name.split('.');
        if (extention[extention.length - 1] == "txt" || extention[extention.length - 1] == "mp3" || extention[extention.length - 1] == "png" || extention[extention.length - 1] == "pdf" || extention[extention.length - 1] == "mp4" || extention[extention.length - 1] == "js" || extention[extention.length - 1] == "bmp" || extention[extention.length - 1] == "jpg") {
            window.open('/Document/ViewFile/?id=' + id, "_blank");
        }
        else {
            $("#viewErrorModal #DownloadFileId").val(id);
            $("#viewErrorModal").modal("show");     
        }
    }
}
function GetClientReport() {
    window.open('/Document/ViewFile', "_blank");
};
function CheckIfItIsABlankSpace(id) {
    if (id == "filetable") return true;
    return false;
}

function SendEmail(email) {
    var model = {
        'Email': email
    };
    $.ajax({
        type: "POST",
        url: "/Account/ForgotPassword",
        data: JSON.stringify(model),
        contentType: 'application/json',
        success: function () {
            $("#forgotModal").modal("hide");
        }
    })
}

function ConfirmForgot() {
    $(window).load(function () {
        $('#forgotModal').modal('show');
    });
}

function ConfirmReset() {
    $(window).load(function () {
        $('#resetModal').modal('show');
    });
}

function ConfirmDelete(name, isFile) {
    if (isFile === 1) {
        $(".modal-title").text("Удалить файл?")
    }
    else {
        $(".modal-title").text("Удалить папку?")
    }
    $("#deleteMessege").text("Действительно удалить " + name + " из Foxbox?");
    $("#deleteModal").modal("show");
}

function FinalConfirmDelete(name, isFile) {
    if (isFile === 1) {
        $(".modal-title").text("Delete file permanently?")
    }
    else {
        $(".modal-title").text("Delete folder permanently?")
    }
    $("#deleteMessege").text("File " + name + " will be permanently deleted from Foxbox and you will not be able to restore it.");
    $("#finalDeleteModal").modal("show");
}

function FinalDeleteDoc() {
    var id = $("#hiddenTaskId").val();
    $.ajax({
        type: "POST",
        url: "/Document/FinalDelete",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#finalDeleteModal").modal("hide");
                $("#" + id).remove();
            }
            else {
                $("#finalDeleteModal").modal("hide");
            }
        }
    })
}

function DeleteDoc() {
    var id = $("#hiddenTaskId").val();
    $.ajax({
        type: "POST",
        url: "/Document/Delete",
        data: { id: id },
        success: function (result) {
            if (result) {
                $("#deleteModal").modal("hide");
                $("#" + id).remove();
            }
            else {
                $("#deleteModal").modal("hide");
            }
        }
    })
}
function ChooseFiles() {
    $('.dropzone').trigger('click');
}
function TriggerMenu() {
    //$('.filerow').trigger("contextmenu");
    $('.filerow').contextMenu({ x: event.pageX, y: event.pageY });
}
function CreateFolder() {
    var id = GetParentId();
    $("#createModal #CreateParentId").val(id);
    $("#createModal").modal("show");
}
function SendCreateFolderRequest() {
    var id = document.getElementById("CreateParentId").value;
    var name = document.getElementById("CreateName").value;
    $.ajax({
        type: "Post",
        url: '/Document/CreateFolder',
        data: {
            parentId: id,
            name: name
        },
        success: function (data, textStatus, jqXHR) {
            $('#createModal').modal('hide');
            $('#dropzone-drop-area').html(data);
        }
    });
}
function Download(id) {
    $.ajax({
        type: "GET",
        url: '/Download/Get',
        data: {
            documentId: id
        },
        success: function (data) {
            window.location = '/Download/Get?documentId='+id;
        }
    }); 
}
function ShowSnack() {
    var x = document.getElementById("snackbar");
    x.className = "show";
}
function SearchTop() {
    $(".result").show();
    var searchPattern = $('#pattern').val()
    $.ajax({
        url: "/search/FindTop",
        data: {
            pattern: searchPattern,
            count: 4
        },
        success: function (data) {
            $(".result").html("");
            if (!data.length && searchPattern) {
                $(".result").html("<div class='search-error'>Ничего не найдено<div>");
            }
            for (var i = 0; i < data.length; i++) {
                var icon;
                if (data[i].isFile) {
                    icon = '<i class="glyphicon glyphicon-file pull-left file-icon"></i>'
                }
                else {
                    icon = '<i class="glyphicon glyphicon-folder-open pull-left folder-icon"></i>'
                }
                var index = data[i].name.toLowerCase().indexOf(searchPattern.toLowerCase());
                var ptrn = data[i].name.slice(index, index + searchPattern.length);
                var name = data[i].name.slice(0, index) + `<b>${ptrn}</b>` + data[i].name.slice(index + ptrn.length);
                var path = data[i].path;

                $(".result").append(
                    `<div class="search-item" onmousedown="return location.href = '/Search/GetDocument?documentId=${data[i].id}'">${icon}<div>
                    <p class="file-name">${name}</p><p class="folder-path">В папке: ${path}</p></div></div>`);
            }
        }
    });
}

function HideResults() {
    $(".result").hide();
}
function StoreUserToSession() {
    sessionStorage.setItem("user", document.getElementById("email").value)
}
window.currentProcessingItem = 0;
window.ProcessingItems = 0;