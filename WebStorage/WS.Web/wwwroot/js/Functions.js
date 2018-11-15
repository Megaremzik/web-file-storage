function StoreUserToSession() {
    sessionStorage.setItem("user", document.getElementById("email").value)
}
function ShowFileOptions(doc) {
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
        //Просмотр файла
    }
}
function TurnOnDeletionMode() {
    sessionStorage.setItem("mode", "del");
}
function GoBack(parentId) {
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

function openPublicAccess(docId, isEdit) {

    $.ajax({
        method: 'GET',
        url: '/Share/OpenPublicAccess',
        data: {
            documentId: docId,
            isEditable: isEdit
        }, 
        success: function (data) {
            $('#publicLink').show();
            $('#pl').val(data);
            $('#pl').select();
        }
    })
}

function GetLinks(docId) {
    $.ajax({
        method: 'GET',
        url: '/Share/GetPublicAccessLink',
        data: {
            documentId: docId,
        },
        success: function (data) {
            if (data == "") {
                $('#publicLink').hide();
            }
            else {
                $('#publicLink').show();
                $('#pl').val(data);
                $('#pl').select();
            }
        }
    })
  //  GetAllUser(docId);
}

function CopyToClipboard(docId) {
    $("#pl").select();
    document.execCommand("copy");
}

function sendInvite(docId) {
    var email = $("#privateEmail").val();
    $.ajax({
        method: 'GET',
        url: '/Share/AddAccessForUser',
        data: {
            documentId: docId,
            guestEmail: email,
            isEditable: 'false'
        },
        success: function (data) {
            $('#privatel').val(data);
            $('#privatel').select();
        }
    })
}
function GetAllUser(docId) {
    var email = $("#privateEmail").val();
    $.ajax({
        method: 'GET',
        url: '/Share/GetAllUsersForSharedDocument',
        data: {
            documentId: docId,
        },
        success: function (data) {
            console.log(data[users])
        }
    })
}
