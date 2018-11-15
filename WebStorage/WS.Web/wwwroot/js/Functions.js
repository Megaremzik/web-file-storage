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
        //Просмотр файла
    }
}
function TurnOnDeletionMode() {
    sessionStorage.setItem("mode", "del");
}
function GoBack() {
    var parentId = sessionStorage.getItem("parentId");    
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
function SetParentId(id) {
    if (id == 0) {
        $('#backParentId').hide();
    }
    sessionStorage.setItem("parentId",id)
}
