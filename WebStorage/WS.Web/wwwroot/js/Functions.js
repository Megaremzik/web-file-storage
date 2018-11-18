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
                '<select class="pull-right sel2" onchange="updateAccessForUser(' + docId + ", \'" + email + '\', this);">'  +
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
            if ($('.EmailsList').children().length==0) {
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