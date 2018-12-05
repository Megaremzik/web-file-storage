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



function GetLinks() {
    var docId = document.getElementById("SharingId").value
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
                $('#openPAccess').show();
                
            }
            else {
                $('#openPAccess').hide();
                if (data.isEditable) {
                    $('#sel1').get(0).selectedIndex = 1;

                }
                $('#closePAccess').show();
                $('#publicLink').show();
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

function sendInvite() {
    var docId = document.getElementById("SharingId").value
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

            var selectOptions = isEdit == "true" ? '<option value="false">View</option><option value="true" selected>Edit</option>' :
                '<option value="false">View</option><option value="true">Edit</option>';
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
function updateAccessForUser(docIdd, email, tagIsEdit) {
    var docId = document.getElementById("SharingId").value
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

function GetAllUser() {
    var docId = document.getElementById("SharingId").value
    $.ajax({
        method: 'GET',
        url: '/Share/GetAllUsersForSharedDocument',
        data: {
            documentId: docId,
        },
        success: function (data) {
            $('.EmailsList').children().remove();
            for (var i = 0; i < data.length; i++) {
                var selectOptions = data[i].isEditable == true ? '<option value="false">View</option><option value="true" selected>Edit</option>' :
                    '<option value="false">View</option><option value="true">Edit</option>';
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
function openPublicAccess() {
    var docId = document.getElementById("SharingId").value
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

function changeAccessLevel() {
    var isEdit = $('#sel1').val();
    var docId = document.getElementById("SharingId").value
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

function closePublicAccess() {
    var docId = document.getElementById("SharingId").value
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

function deleteUser(docIds, user, tag) {
    var docId = document.getElementById("SharingId").value
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

function closeLimitedAccess() {
    var docId = document.getElementById("SharingId").value
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
        $("#shareModal #SharingId").val(id);
        GetLinks(id);
        $('#shareModal').modal('show');
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
    var isFolder = row.children[0].innerHTML.includes("fa-folder");

    if (!isFolder) {
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
window.currentProcessingItem = 0;
window.ProcessingItems = 0;
function HideResults() {
    $(".result").hide();
}
function StoreUserToSession() {
    sessionStorage.setItem("user", document.getElementById("email").value)
}
//$(".nav-left_pan").click(function () {
//    sessionStorage.setItem("navNumber",$(this).value)
//});
function SetNumber(number) {
    sessionStorage.setItem("navNumber", number)
}
function AdddropdownClickEvents() {
    [].forEach.call($('.dropdownload'), function (el) {
        el.addEventListener('click', function (e) {
            var id = el.parentElement.parentElement.parentElement.parentElement.parentElement.id;
            ContextResult('download', id);
        })
    });
    [].forEach.call($('.dropshare'), function (el) {
        el.addEventListener('click', function (e) {
            var id = el.parentElement.parentElement.parentElement.parentElement.parentElement.id;
            ContextResult('share', id);
        })
    });
    [].forEach.call($('.dropview'), function (el) {
        el.addEventListener('click', function (e) {
            var id = el.parentElement.parentElement.parentElement.parentElement.parentElement.id;
            ContextResult('view', id);
        })
    });
    [].forEach.call($('.droprename'), function (el) {
        el.addEventListener('click', function (e) {
            var id = el.parentElement.parentElement.parentElement.parentElement.parentElement.id;
            ContextResult('rename', id);
        })
    });
    [].forEach.call($('.dropcut'), function (el) {
        el.addEventListener('click', function (e) {
            var id = el.parentElement.parentElement.parentElement.parentElement.parentElement.id;
            ContextResult('cut', id);
        })
    });
    [].forEach.call($('.droppaste'), function (el) {
        el.addEventListener('click', function (e) {
            var id = el.parentElement.parentElement.parentElement.parentElement.parentElement.id;
            ContextResult('paste', id);
        })
    });
    [].forEach.call($('.dropcopy'), function (el) {
        el.addEventListener('click', function (e) {
            var id = el.parentElement.parentElement.parentElement.parentElement.parentElement.id;
            ContextResult('copy', id);
        })
    });
    [].forEach.call($('.dropdelete'), function (el) {
        el.addEventListener('click', function (e) {
            var id = el.parentElement.parentElement.parentElement.parentElement.parentElement.id;
            ContextResult('delete', id);
        })
    });
}
//$('.dropdownload').addEventlistener("click",function () {
//    var id = $(this).parent().parent().parent().parent().id;
//    ContextResult('download', id);
//})