

window.uploadImg = {
    fotoCarregar: function () {
        let photo = document.getElementById('imgCliente');
        let file = document.getElementById('fImage');


        file.click();
    }
}

window.focos = {
    focusById: function (idElemento) {
        var element = document.getElementById(idElemento);
        if (element) {
            element.focus();
            element.style.borderColor = "red";
        }
    }
}

