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
    //$('#file-options').load('@Url.Action("FileOptions", "Document",new { id = doc})');
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
        //$('#dropzone-drop-area').load('@Url.Action("ReturnDocumentList", "Document")');
       
    }
    else {
        //Просмотр файла
    }
}