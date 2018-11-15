function GetParentId() {
    var parent = sessionStorage.getItem("parentId");
    document.querySelector('input[name=parentId]').value = parent;
    return parent;
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
function SetParentId(id) {
    document.querySelector('input[name=parentId]').value = id;
    if (id == 0) {
        $('#backParentId').hide();
    }
    sessionStorage.setItem("parentId",id)
}
function ContextResult(action, id) {
    if (action = "delete") Url.Action("Delete", "Document", id)
}