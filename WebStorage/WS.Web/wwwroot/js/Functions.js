function StoreUserToSession() {
    sessionStorage.setItem("user", document.getElementById("email").value)
}
function ShowFileOptions(doc) {
    $('#file-options').load('@Url.Action("FileOptions", "Document",doc)');
}