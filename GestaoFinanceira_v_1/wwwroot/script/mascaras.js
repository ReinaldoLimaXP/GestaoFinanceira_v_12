
window.masksHoraServico = {
    mascaraHoraSer: function (elementID) {
        var phoneMaskHota = IMask(
            document.getElementById("tempoEstimado"), {
            mask: '00:00',
        });
    }
}



window.masksCPF = {
    mascaraCPF: function (elementID) {
        var phoneMasktel = IMask(
            document.getElementById("cpfCliente"), {
            mask: '000.000.000-00',
        });
    }
}


window.masksTel = {
    mascaratel: function (elementID) {
        var phoneMasktel = IMask(
        document.getElementById("celularTel"), {
            mask: '(00) 90000-0000',
        });
    }
}

window.masksTelCasa = {
    mascaratelCasa: function (elementID) {
        var phoneMasktel = IMask(
            document.getElementById("telCasa"), {
            mask: '(00) 90000-0000',
        });
    }
}

window.masksTelTra= {
    mascaratelTra: function (elementID) {
        var phoneMasktel = IMask(
            document.getElementById("celTrabalho"), {
            mask: '(00) 90000-0000',
        });
    }
}

window.masksCep = {
    mascaraCep: function (elementID) {
        var phoneMaskcep = IMask(
            document.getElementById("cep"), {
            mask: '00000-000',
        });
    }
}


window.masksCNPJ = {
    mascaraCNPJ: function (elementID) {
        var phoneMaskCNPJ = IMask(
            document.getElementById("cnpjCliente"), {
                mask: '00.000.000/0000-00',
        });
    }
}

window.masksPlaca = {
    mascaraPlaca: function (elementID) {
        var phoneMaskCNPJ = IMask(
            document.getElementById("placa"), {
                mask: 'aaa-0*00',
        });
    }
}

window.masksAnofab = {
    mascaraAnofab: function (elementID) {
        var phoneMaskCNPJ = IMask(
            document.getElementById("fabicacao"), {
            mask: '0000/0000',
        });
    }
}


window.maskMoeda = {
    mascaraMoeda: function () {
        
    }
}
