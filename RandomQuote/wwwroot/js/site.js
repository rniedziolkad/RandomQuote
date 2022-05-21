// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function editUsername() {
    document.getElementById("btnEditUsername").hidden = true;
    document.getElementById("submitUsername").hidden = false;
    document.getElementById("UserName").disabled = false;
    /*let token = document.getElementsByName("__RequestVerificationToken")[0].value;
    $.ajax({
        url: "/Account/EditUsername",
        headers: { "RequestVerificationToken": token },
        success: function(response){
            document.getElementById("usernameContent").innerHTML = response;
        }
    });*/
}

function acceptFormUsername(){
    alert("Sending POST!")
}

function editEmail(){
    let editBtns = document.getElementsByClassName("editBtn");
    for(const editBtn of editBtns){
        editBtn.hidden = true;
        editBtn.disabled = true;
    }
    let submit = document.getElementById("submitEditEmail");
    submit.hidden = false;
    submit.disabled = false;
    let cancel = document.getElementById("cancelEditEmail");
    cancel.hidden = false;
    cancel.disabled = false;
    document.getElementById("Email").disabled = false;
}

function cancelEmail(){
    let submit = document.getElementById("submitEditEmail");
    submit.hidden = true;
    submit.disabled = true;
    let cancel = document.getElementById("cancelEditEmail");
    cancel.hidden = true;
    cancel.disabled = true;
    let editBtns = document.getElementsByClassName("editBtn");
    for(const editBtn of editBtns){
        editBtn.hidden = false;
        editBtn.disabled = false;
    }
    document.getElementById("Email").disabled = true;
}
function editUserInfo(){
    let editBtns = document.getElementsByClassName("editBtn");
    for(const editBtn of editBtns){
        editBtn.hidden = true;
        editBtn.disabled = true;
    }
    let inputs = document.getElementsByClassName("iUserInfo");
    for(const input of inputs){
        input.disabled = false;
    }
    let submit = document.getElementById("submitEditUserInfo");
    submit.hidden = false;
    submit.disabled = false;
    let cancel = document.getElementById("cancelEditUserInfo");
    cancel.hidden = false;
    cancel.disabled = false;
}

function cancelEditInfo(){
    let editBtns = document.getElementsByClassName("editBtn");
    for(const editBtn of editBtns){
        editBtn.hidden = false;
        editBtn.disabled = false;
    }
    let inputs = document.getElementsByClassName("iUserInfo");
    for(const input of inputs){
        input.disabled = true;
    }
    let submit = document.getElementById("submitEditUserInfo");
    submit.hidden = true;
    submit.disabled = true;
    let cancel = document.getElementById("cancelEditUserInfo");
    cancel.hidden = true;
    cancel.disabled = true;
}