
$(document).ready(function () {
    $('#divAdd').hide();
    $('#Alert').hide();
    
    //$('#Projeto').removeAttr('checked');
    //cccreate
    //$('#divAdd').hide();
    //$('#Alert').hide();
    $('#chkEdicao').click(function () {
        if (this.checked) {
            $('#divAdd').show();
            
        }
        else {
            $('#divAdd').hide();
        }
    });
    $('#Prospect').click(function () {
        if (this.checked == true) {
            
            prospect = true;
        }
        if (this.checked == false) {
            
            prospect = false;
        }
    });
    $('#Projeto').click(function() {
        if (this.checked == true) {
            $("#Classe").prop('disabled', false);
            $("#Item").prop('disabled', false);
            console.log("Projeto");
            projeto = true;
        }
        if (this.checked == false) {
            $("#Classe").val("");
            $("#Item").val("");
            $("#Classe").prop('disabled', true);
            $("#Item").attr('disabled', true);

            console.log("Nao é projeto");
            projeto = false;
        }
    });
    
    setTimeout(function () { Mascaras() }, 3000);
});

function Mascaras() {
    var novoCCusto = $("#CCusto");
    novoCCusto.mask('999999999', { placeholder: '000000000', reverse: true });
    var novoCCitem = $("#Item");
    novoCCitem.mask('A999999', { placeholder: '00000000', reverse: true });
}
var visivel_add = false;
function btnAddCCusto() {
    if (visivel_add == false) {
        $('#divAdd').show();
        visivel_add = true;
    }
    else if (visivel_add == true) {
        $('#divAdd').hide();
        visivel_add = false;
    }
}
/*-----------------------------------------------------------------------------------------*/
/* ADICIONAR CC EMPRESA DE CENTRO DE CUSTO*/
var projeto = false;
var prospect = false;
function EmpCCustoModel() {
    var valido = false;
    var empresaid = $('#EmpresaId').val();
    var cc = $('#CCusto').val();
    var ccdesc = $('#CCustoDesc').val();
    //var projeto = $('#Projeto').val();
    var item = $('#Item').val();
    var classe = $('#UnidadeClasse').val();
    var ativo = $('#Ativo').val();

    var token = $('input[name=__RequestVerificationToken]').val();
    console.log(model);

    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO
    valido = true;
    

    if ((cc.length <= 8) == true) {
        valido = false;
        $('#Alert').show();
        $('#Alert').append('<span class="error">*C.Custo - Dê o codigo do C.C</span><br/>');
    }

    if (classe == "" || classe.empty || classe == null) {
        classe == 0;
        console.log("classe em branco");
    }

    if (empresaid == "" || empresaid.empty || empresaid == null) {
        empresaid == 0;
        console.log("Todos");
    }

    if (/^\d+(?:\.\d+)?$/.test(cc) == false) {
        valido = false;
        $('#Alert').show();
        $('#Alert').append('<span class="error">*C.Custo - Remova as letras</span><br/>');
    }
    if ((ccdesc.length < 3) == true) {
        valido = false;
        $('#Alert').show();
        $('#Alert').append('<span class="error">*C.Custo Desc - Faça uma descriçao mais elaborada</span><br/>');
    }

    var model = { "EmpresaId": empresaid, "Projeto": projeto, "CCusto": cc, "CCustoDesc": ccdesc, "Item": item, "Classe": classe, "Ativo": ativo,"Prospect": prospect }
    console.log(model);
    if (valido != false) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/EmpCCusto/CreateModel",
            traditional: true,
            data: model,
            success: function (data) {
                alert(data.Message);
                //$('#Alert').remove();
                $('#Alert').hide();
                $('#chkEdicao').removeAttr('checked');
                $('#divAdd').hide();
                $(document).ready(function () {
                    document.getElementById('CCusto').value = "";
                    document.getElementById('CCustoDesc').value = "";
                    $('#Ativo').removeAttr('checked');
                    window.history.go(0);
                });
            }
        });
    } else {
        alert("Verifique os campos obrigatórios *");
    }
}

function ProjetoInsere() {
    //var proj = $("#Projeto").prop("checked", true);
    var projeto = $("#Projeto").val();
    if (projeto == true) {
        $("#Classe").prop('disabled', false);
        $("#Item").prop('disabled', false);
        console.log("Projeto");
    }
    if (projeto== false) {
        $("#Classe").val("");
        $("#Item").val("");
        $("#Classe").prop('disabled', true);
        $("#Item").attr('disabled', true);
        $("#Prospect").prop('disabled', true);
        console.log("Nao é projeto");
    }
}