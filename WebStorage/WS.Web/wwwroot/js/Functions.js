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