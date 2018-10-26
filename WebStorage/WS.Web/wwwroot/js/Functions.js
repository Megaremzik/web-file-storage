function StoreUserToSession() {
    sessionStorage.setItem("user", document.getElementById("email").value)
}