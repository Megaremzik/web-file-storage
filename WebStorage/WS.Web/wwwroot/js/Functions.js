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
    //if (tr.hasClass("selected") == false) {
    //    var selected = document.getElementsByClassName("selected");
    //    selected.removeClass("selected");
    //    tr.addClass("selected");
    //}

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
function updateAccessForUser(docId, email, tag) {
    var isEdit = $(tag).val();
    $.ajax({
        method: 'GET',
        url: '/Share/AddAccessForUser',
        data: {
            documentId: docId,
            guestEmail: email,
            isEditable: isEdit
        },
        success: function (data) {
            console.log("test");
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
    sessionStorage.setItem("parentId",id)
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
        sessionStorage.setItem("copy","")
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
function CheckIfItIsABlankSpace(id) {
    if (id == "filetable") return true;
    return false;
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
    $('.filerow').trigger("contextmenu");
}
$(function () {
    // make button open the menu
    $('#menuButton').on('click', function (e) {
        e.preventDefault();
        //$('.filerow').contextMenu();
        $('.filerow').trigger("contextmenu");
        // or $('.context-menu-one').contextMenu({x: 100, y: 100});
    });
})
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