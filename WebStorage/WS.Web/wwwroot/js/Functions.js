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
}