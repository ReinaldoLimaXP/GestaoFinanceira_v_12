

window.modal = {
    showModal: function (elementID) {
        if (elementID != null && elementID != undefined) {
            let modalElement = document.getElementById(elementID);
            modalElement.style.display = "block";
            return "OK";
        } else {
            return "erro";
        }
    }
}

window.modalFechar = {
    fecharModal: function (elementID) {
        if (elementID != null && elementID != undefined) {
            let modalElement = document.getElementById(elementID);
            modalElement.style.display = "none";
            return "OK";
        } else {
            return "erro";
        }
    }
}


