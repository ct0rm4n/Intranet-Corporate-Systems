/*-----------------------------------------------------------------------------------------------------//
 Criação: 16/04/2018
 
 funçoes:
    Agrupamentos Listbox: additem, Remove Item, Adicionar Todos, Remover Todos.
    Grava empresa: 
*/

function Mascaras() {
    var $seuFornecedor = $("#Fornecedor");
    $seuFornecedor.mask('A999999', { placeholder: '000000', reverse: false, autocomplete: 'A000000' });
}

$(document).ready(function () {
    //Debug;
    //formulários de cadastros
    $.ajaxSetup({ cache: false });
    
    //cadastrar empresa
    
    $(".btnCadEmp").click(function () {

        var id = $(this).data("value");

        $("#MeuModal").load("/Empresa/Create/", function () {
            $('#myModal').modal("show");
        });
    });
    //editar empresa
    $(".btnEdtEmp").click(function () {

        var id = $('#AlterarEmp').val();

        $("#MeuModal").load("/Empresa/Edit/"+id, function () {
            $('#myModal').modal("show");
        });
    });
    //cadastrar unidade
    $(".btnCadUnid").click(function () {

        var id = $(this).data("value");

        $("#MeuModal").load("/Unidades/Create/", function () {
            $('#myModal').modal("show");
        });
    });
    //cadastrar depto
    $(".btnCadDepto").click(function () {

        var id = $(this).data("value");

        $("#MeuModal").load("/Setor/Create/", function () {
            $('#myModal').modal("show");
        });
    });
    //controle de cc por empresa
    $(".btnContCC").click(function () {

        var id = $(this).data("value");

        window.location.replace('/Administrador/ControleEmp');
    });

    //$('#chkEdicao').removeAttr('checked');

    $('#Alert').hide();
    $("#SolicCCAdd").hide();
    $("#SolicEmpAdd").hide();

    var visivel_emp = false;
    $("#btnSolicEmpAdd").click(function() {
        if (visivel_emp == true) {
            visivel_emp = false;
            $("#SolicEmpAdd").hide();
            
        } else {
            visivel_emp = true
            $("#SolicEmpAdd").show();
            Mascaras();
            setTimeout(function () { Mascaras() }, 3000);
        }

    });
    //botao cancelar do formulario solicita empresa
    $("#btnSolicEmpAddCancel").click(function () {
        visivel_emp = false;
        $("#SolicEmpAdd").hide();
    });
    $('#Fornecedor').focusout(function(){
        console.log("foii");
        var valor = $('#Fornecedor').val();
        if (valor.length == 3) {
            var valor = '000' + valor;
            $("#Fornecedor").val(valor);

        }
        if (valor.length == 2) {
            var valor = '0000' + valor;
            $("#Fornecedor").val(valor);

        }
        if (valor.length == 1) {
            var valor = '00000' + valor;
            $("#Fornecedor").val(valor);

        }
    });

    var visivel_cc;
    $("#btnSolicCCAdd").click(function() {
        if (visivel_cc == true) {
            visivel_cc = false;
            $("#SolicCCAdd").hide();
        } else {
            visivel_cc = true
            $("#SolicCCAdd").show();
            var userid = $("#Id").val();
            console.log(userid);
            $("#UserId").val(userid);
        }

    });
    //botao cancelar do formulario solicita cc
    $("#btnSolicCCAddCancel").click(function () {
        visivel_emp = false;
        $("#SolicCCAdd").hide();
    });
    $("#Alert").hide();
    $('#AlertC').hide();
    
});

/*-----------------------------------LIST BOX FUNCOES-------------------------------------------------*/
/*Botao primeira etapa*/
function AddItem() {
    $("#SetorId option:selected").appendTo("#SetorEmp");
    $("#SetorEmp option").attr("selected", false);
}
function RemoveItem() {
    $("#SetorEmp option:selected").appendTo("#SetorId");
    $("#SetorId option").attr("selected", false);
}
function AddAll() {
    $("#SetorEmp option").appendTo("#SetorId");
    $("#SetorId option").attr("selected", false);
}
function RemoveAll() {
    $("#SetorId option").appendTo("#SetorEmp");
    $("#SetorEmp option").attr("selected", false);
}
/*----------------------------------------------------------------------------------------------------- */
//abrirr terceira etapa do cadastro de emp 
function Proximo() {
    var razaosocial = document.getElementById('RazaoSocial').value;
    console.log("Razao social: "+razaosocial);
    $("#MeuModal").load("/EmpCCusto/Create")
    document.getElementById('Proximo').disabled = true;   
}

function Finaliza() {
    $(document).ready(function () {
        $.ajaxSetup({ cache: false });
        $('#myModal').modal('hide');
        //window.location.replace('/Administrador/Index');
    });
}
//abrir controle de cc da empresa
function ControleCC() {
    $(document).ready(function () {
        var empID = document.getElementById('EmpresaId').value;
        console.log(empID);
        window.location.replace('/Administrador/ControleEmp/');
    });
}
//Adiconar empresadeptounidade
function CriarEmpDptoCC() {
    $(document).ready(function () {
        var empID = document.getElementById('EmpresaId').value;
        $("#MeuModal").load("/EmpresaDeptoUnids/Create/"+empID, function () {
            $('#myModal').modal("show");
        });
    });
}
/*-----------------------------------------------------------------------------------------------------
    FUNCAO DE INSERÇÂO DA Empresa
-------------------------PRIMEIRA ETAPA-----------------------------------------------*/
function GravarEmpresa() {
    //VALIDAÇÃO
    //debugger
    //var data = $('#cadempresa').serialize();
    var razaosocial = $('#RazaoSocial').val();
    var complemento = $('#Complemento').val();
    var codsiga = $('#CodSiga').val();
    var ativo = $('#Ativo').val();
    var deptos = $('#EmpDepto').val();
    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO
    var model = {
        "RazaoSocial": razaosocial, "Complemento": complemento, "CodSiga": codsiga, "Ativo": ativo
    }
    console.log(model);
    var valido = true;
    if (razaosocial == "" || razaosocial == null || razaosocial.length < 5) {
        //$('.Alert').after('<span class="error">*Dê um nome a empresa</span>');
        
        $('#Alert').append('<span class="error">*Razão Social - Dê um nome a empresa</span><br/>');
        valido = false;
        console.log("RazaoSocial");
    }

    if (codsiga == "" || codsiga == null || codsiga.length < 1) {
        $('#Alert').append('<span class="error">*COD-Siga - Consulte o codigo com seu administrador</span><br/>');
        valido = false;
        console.log("Siga");
    }
    if (ativo == false) {
        $('#Alert').append('<span class="error">*Ativo - Não pode inserir empresa inativa</span><br/>');
        valido = false;
        console.log("Ativa");
    }

    if (valido==true) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/Empresa/CreateEmp/",
            traditional: true,
            data: model,
            success: function (data) {
                alert(data.Message);
                console.log("Sucesso");
                //Gravar();
                EmpresaDepto(data.EmpresaId);
            }
        });
    }
    else {
        $('#Alert').show();
        alert("Verifique os campos obrigatórios, razao social e os departamentos.");
        $('#Alert').show();
    }
}
function AlterarEmpresa() {
    //VALIDAÇÃO
    //debugger
    var empresaid = $('#EmpresaId').val();
    var razaosocial = $('#RazaoSocial').val();
    var complemento = $('#Complemento').val();
    var codsiga = $('#CodSiga').val();
    var ativo = $('#Ativo').val();
    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO
    var valido = true;
    if (razaosocial.length < 5) {
        $('#RazaoSocial').after('<span class="error">*Dê um nome a empresa</span><br/>');
        valido = false;
    }


    if (codsiga.length < 1) {
        $('#CodSiga').after('<span class="error">*Consulte o codigo com seu administrador</span><br/>');
        valido = false;
    }
    var model = {
        "EmpresaId": empresaid, "RazaoSocial": razaosocial ,"Complemento": complemento, "CodSiga": codsiga, "Ativo": ativo
    }
    console.log(model);

    if (valido == true) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/Empresa/Edit/",
            traditional: true,
            data: model,
            success: function (data) {
                alert(data.Message);
                console.log("Sucesso");
                EmpresaDepto(empresaid);
            }
        });
    }
    else {
        alert("Verifique os campos obrigatórios, razao social e os departamentos.")
    }
}
/* ------------------------SEGUNDA ETAPA---------------------------------*/
function EmpresaDepto(empresaid) {
    var listarray = new Array();
    var listarrayIds = new Array();
    var select = document.getElementById('SetorEmp');
    for (var i = 0; i < select.options.length; i++) {
        console.log(select.options[i]);
        listarray.push(select.options[i].text);
        listarrayIds.push(select.options[i].value);
    }
    var valido = true;
    
    var model = { "listarray": listarrayIds, "EmpresaId": empresaid }
    console.log(model);

    if (valido == true) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/SetorEmp/CreateSetorEmp",
            traditional: true,
            data: model,
            success: function (data) {
                alert(data.Message);
                console.log("Salvo!" + listarray);
                //document.getElementById('Proximo').disabled = false;
                //document.getElementById('Finaliza').disabled = true;
                document.getElementById('Salvar').disabled = true;
                window.location.replace('/Administrador/Index');
            }
        });
    } else {
        alert("Verifique os campos obrigatórios, descrição e lista de agrupamentos.");
    }
}

function CadastrarSolictEmpController() {
    var valido = false;
    var userid = $('#Id').val();
    var emp = $('#EmpresaId').val();
    var fornecedor = $('#Fornecedor').val();
    var projeto = $('#Projeto').val();
    console.log(projeto);

    var token = $('input[name=__RequestVerificationToken]').val();
    
    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO
    valido = true;

    if ($('#Projeto').checked) {
        console.log("Projeto");
        projeto = true;
    } else {
        console.log("Nao e projeto");
        projeto = false;
    }
    if ((emp.length > 0 || emp != "") == false) {
        valido = false;
        $('#Alert').show();
        $('#Alert').append('<span class="error">*Empresa - Informe a empresa</span></br>');
        console.log(dv.length);
    }
   
    if ((fornecedor.length == 6 || fornecedor != "" || /^\d+(?:\.\d+)?$/.test(fornecedor) ||fornecedor.empty) == false) {
        valido = false;
        $('#Alert').show();
        $('#Alert').append('<span class="error">*Fornecedor - Informe o fornecedor do usuário, fonecedor é por empresa e possui 6 digitos</span></br>');
        console.log(fornecedor.length);
    }
    else if
    (fornecedor.length < 6) {
        valido = false;
        $('#Alert').show();
        $('#Alert').append('<span class="error">*Fornecedor - Informe o fornecedor do usuário, fonecedor é por empresa e possui 6 digitos</span></br>');
        console.log(fornecedor.length);
    }
    if ((userid.length != 0) == false) {
        valido = false;
        console.log(userid);
    }
    var model = {
        "UserId": userid, "EmpresaId": emp, "Fornecedor": fornecedor, "Projeto": projeto
    }
    console.log(model);

    if (valido != false) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/SolicitaEmpresas/Create/" + model,
            traditional: true,
            data: model,
            success: function (data) {
                if (data.Status == true) {
                    alert(data.Message);
                    visivel = false;
                    $("#SolicEmpAdd").hide();
                    $("#Alert").hide();
                    window.location.replace('/UsersAdmin/Edit/' + userid);
                }
            }
        });
    } else {
        alert("Verifique os campos obrigatórios *");
    }
}
function CadastrarSolictCCController() {
    var valido = false;
    var userid = $('#UserId').val();
    var empcc = $('#EmpCCustoId').val();
    var solicita = $('#SolicitaEmpresaId').val();
    var unidade = $('#UnidadeId').val();

    var token = $('input[name=__RequestVerificationToken]').val();

    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO
    valido = true;

    console.log(solicita);
    if ((unidade == 0 && unidade == "") == true) {
        valido = false;
        $('#AlertC').show();
        $('#AlertC').append('<span class="error">*Unidade - Informe a unidade para esse projeto</span></br>');
    }
    if ((empcc == 0 && empcc == "") == true) {
        valido = false;
        $('#AlertC').show();
        $('#AlertC').append('<span class="error">*Centro de Custo- Informe o campo</span></br>');
    }

    if ((empcc == 0 && empcc == "") == true) {
        valido = false;
        $('#AlertC').show();
        $('#AlertC').append('<span class="error">*Centro de Custo- Informe o campo</span></br>');
    }

    if ((userid.length != 0) == false) {
        valido = false;
        console.log(userid);
    }
    var model = { "UserId": userid, "EmpCCustoId": empcc, "SolicitaEmpresaId":solicita, "UnidadeId": unidade }

    console.log(model);

    if (valido != false) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/SolicitaCusto/Create/",
            traditional: true,
            data: model,
            success: function (data) {
                alert(data.Message);
                visivel = false;
                $("#SolicCCAdd").hide();
                $("#Alert").hide();
                window.location.replace('/UsersAdmin/Edit/'+data.Id);
            }
        });
    } else {
        alert("Verifique os campos obrigatórios *");
    }
}
//dropdownlist dinamico!!!!!PORRAA nois é os bicho mesmo rapaiz
function UnidadeSelector() {
    var id = $('#EmpresaId').val();
    var model = { "id": id}
    console.log("Seleciona" + model);
    var valido = true;
    //if (model == null) { valido = false; }
    if (valido == true) {
        $("#Classe").empty();
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/EmpCCusto/GetUnidadeSelect/",
            traditional: true,
            data: model,
            success: function (data) {
                //alert(data.Message);
                console.log("Sucesso");
                console.log(data.Unidades);
                $("#Classe").empty();
                $("#Classe").append('<option value="">' +'Selecione' + '</option>');
                $.each(data.Unidades, function (index, element) {
                    $("#Classe").append('<option value="' + element.Value + '">' + element.Text + '</option>');
                    console.log("add");
                });
                //$('#Classe').remove();
                //$('#Classe').append();
            }
        });
        
    }
    else {
        $('#Classe').val('');
    }
}
function CCSelector() {
    var id = $('#SolicitaEmpresaId').val();
    console.log("Empresa selecionada é:",id);
    var model = { "id": id }
    console.log("Seleciona" + model);
    var valido = true;
    if (model.id == null || model.empty || model.id == '' || model.id == '0') {
        valido = false;
        console.log(model.id);
        $("#EmpCCustoId").empty();
        $("#EmpCCustoId").append('<option value="">' + "Selecione o C.Custo" + '</option>');
        $("#EmpCCustoId").prop('disabled', true);
    }
    if (valido == true) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/SolicitaEmpresas/GetCCSelect/",
            traditional: true,
            data: model,
            success: function (data) {
                //alert(data.Message);
                console.log("Sucesso");
                console.log(data.Ccusto);
                $("#EmpCCustoId").empty();
                $("#EmpCCustoId").append('<option value="">Selecione</option>');
                $.each(data.Ccusto, function (index, element) {
                    $("#EmpCCustoId").prop('disabled', false);
                    $("#EmpCCustoId").append('<option value="' + element.Value + '">' + element.Text + '</option>');

                });
                //$('#Classe').remove();
                //$('#Classe').append();
            }
        });
    }
    else {
        $('#Classe').val('');
    }
}

function CCProjSelector() {
    var id = $('#SolicitaEmpresaId').val();
    var model = { "id": id }
    console.log("Seleciona" + model);
    var valido = true;
    if (model.id == null || model.id == '0') {
        valido = false;
        console.log(model.id);
        $("#EmpCCustoId").empty();
        $("#EmpCCustoId").append('<option value="">' + "Selecione o C.Custo" + '</option>');
        $("#EmpCCustoId").prop('disabled', true);
    }
    if (valido == true) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/SolicitaEmpresas/GetCCSelect/",
            traditional: true,
            data: model,
            success: function (data) {
                //alert(data.Message);
                console.log("Sucesso");
                console.log(data.Ccusto);
                $("#EmpCCustoId").empty();
                $.each(data.Ccusto, function (index, element) {
                    $("#EmpCCustoId").prop('disabled', false);
                    $("#EmpCCustoId").append('<option value="' + element.Value + '">' + element.Text + '</option>');

                });
                //$('#Classe').remove();
                //$('#Classe').append();
            }
        });
    }
    else {
        $('#Classe').val('');
    }
}

function RemoverSolicitaEmp(id) {
    $("#MeuModal").load("/SolicitaEmpresas/Delete/" + id, function () {
        $('#myModal').modal("show");
    });
};

function RemoverCC(id) {
    $("#MeuModal").load("/SolicitaCusto/Delete/" + id, function () {
        $('#myModal').modal("show");
    });
};