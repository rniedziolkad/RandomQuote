function clearValidationErrors(){
    for(let validationField of document.querySelectorAll('[data-valmsg-for]')){
        validationField.innerHTML = "";
        validationField.classList.add("field-validation-valid");
        validationField.classList.remove("field-validation-error");
    }
}

function editEmail(){
    document.getElementById("btnEditEmail").hidden = true;
    let editBtns = document.getElementsByClassName("editBtn");
    for(const editBtn of editBtns){
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

function exitEmail(){
    document.getElementById("btnEditEmail").hidden = false;
    let submit = document.getElementById("submitEditEmail");
    submit.hidden = true;
    submit.disabled = true;
    let cancel = document.getElementById("cancelEditEmail");
    cancel.hidden = true;
    cancel.disabled = true;
    let editBtns = document.getElementsByClassName("editBtn");
    for(const editBtn of editBtns){
        editBtn.disabled = false;
    }
    document.getElementById("Email").disabled = true;
    clearValidationErrors();
}
function submitEmail(){
    let data = $("#formEmail").serialize();
    $.ajax({
        type: "POST",
        url: '/Account/EditEmail',
        data: data,
        dataType: "json",
        success: function (response) {
            if(response["succeeded"]){
                location.reload();
            }
            else{
                let errors = response.errors;
                for(let key in errors){
                    let errorSummary = "";
                    for(let innerError of errors[key]){
                        errorSummary+=innerError["errorMessage"]+"\n";
                    }
                    if(key==="email"){
                        let valFields = document.querySelectorAll('[data-valmsg-for="Email"]')
                        for(let valField of valFields){
                            valField.classList.remove("field-validation-valid");
                            valField.classList.add("field-validation-error");
                            valField.innerHTML = errorSummary;
                        }
                    }
                    else{
                        console.error(key+"\n"+errorSummary)
                    }
                    
                }
                
            }
        },
        error: function(xhr){
            alert(xhr.statusText+":" + xhr.status);
            location.reload();
        }
    });
}
function editUserInfo(){
    document.getElementById("btnEditUserInfo").hidden = true;
    let editBtns = document.getElementsByClassName("editBtn");
    for(const editBtn of editBtns){
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

function exitUserInfo(){
    document.getElementById("btnEditUserInfo").hidden = false;
    let editBtns = document.getElementsByClassName("editBtn");
    for(const editBtn of editBtns){
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
    clearValidationErrors();
}

function submitUserInfo(){
    let data = $("#formUserInfo").serialize();
    $.ajax({
        type: "POST",
        url: '/Account/EditUserInfo',
        data: data,
        dataType: "json",
        success: function (response) {
            if(response["succeeded"]){
                location.reload();
            }
            else{
                let errors = response.errors;
                for(let key in errors){
                    let errorSummary = "";
                    for(let innerError of errors[key]){
                        errorSummary+=innerError["errorMessage"]+"\n";
                    }
                    if(key==="Sex"){
                        let valFields = document.querySelectorAll('[data-valmsg-for="Sex"]')
                        for(let valField of valFields){
                            valField.classList.remove("field-validation-valid");
                            valField.classList.add("field-validation-error");
                            valField.innerHTML = errorSummary;
                        }
                    }
                    if(key==="FirstName"){
                        let valFields = document.querySelectorAll('[data-valmsg-for="FirstName"]')
                        for(let valField of valFields){
                            valField.classList.remove("field-validation-valid");
                            valField.classList.add("field-validation-error");
                            valField.innerHTML = errorSummary;
                        }
                    }
                    if(key==="LastName"){
                        let valFields = document.querySelectorAll('[data-valmsg-for="LastName"]')
                        for(let valField of valFields){
                            valField.classList.remove("field-validation-valid");
                            valField.classList.add("field-validation-error");
                            valField.innerHTML = errorSummary;
                        }
                    }
                    if(key==="Description"){
                        let valFields = document.querySelectorAll('[data-valmsg-for="Description"]')
                        for(let valField of valFields){
                            valField.classList.remove("field-validation-valid");
                            valField.classList.add("field-validation-error");
                            valField.innerHTML = errorSummary;
                        }
                    }
                    else{
                        console.error(key+"\n"+errorSummary)
                    }

                }

            }
        },
        error: function(xhr){
            alert(xhr.statusText+":" + xhr.status);
            location.reload();
        }
    });
}
function showNotification(message){
    let notification = document.getElementById("notificationMessage");
    if(notification!=null){
        notification.innerHTML = message;
        notification.addEventListener("click", closeNotification, true);
        notification.addEventListener("mouseenter", function(){
            notification.style.color = "#555";
        }, true);
        notification.addEventListener("mouseleave", function(){
            notification.style.color = "#000";
        }, true);
        setTimeout(closeNotification, 5000);
    }
}
function closeNotification(){
    let notification = document.getElementById("notificationMessage");
    if(notification!=null){
        notification.remove();
    }
}

function editPassword(){
    window.location.replace("/Account/ChangePassword");
}
function exitEditPassword(){
    window.location.replace("/Account");
}

function deleteQuote(){
    let data = $("#QuoteId").serialize();
    $.ajax({
        type: "DELETE",
        url: "/ManageQuotes/Delete",
        headers: {"RequestVerificationToken" : $('input[name="__RequestVerificationToken"]').val()},
        data: data,
        dataType: "json",
        success: function (response) {
            if(response["succeeded"]){
                location.href = "/ManageQuotes";
            }
            else{
                alert("Error deleting");
            }
        },
        error: function (xhr){
            alert(xhr.statusText+":" + xhr.status);
            location.reload();
        }
    });
}