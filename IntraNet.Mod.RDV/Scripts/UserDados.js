$(document).ready(function () {
    $("#dadosAdd").hide();
    var visivel;
    $("#btnAddDados").click(function () {
        if (visivel == true) {
            visivel = false;                       
            $("#dadosAdd").hide();                
        } 
        else {
            visivel = true
            $("#dadosAdd").show();
        }
        
    })
    $("#Alert").hide();
    Mascaras();
    setTimeout(function () { Mascaras() }, 3000);
});
function Mascaras() {
    var $seuCampoCpf = $("#Cpf");
    $seuCampoCpf.mask('000.000.000-00', { reverse: true });
    var $seuAgencia = $("#Agencia");
    $seuAgencia.mask('00000', { reverse: false });
    var $seuCCorrente = $("#ContaCorrente");
    $seuCCorrente.mask('000000000', { reverse: false });
    var $seuDv = $("#Dv");
    $seuDv.mask('00', { reverse: false });
    
}
function Editar(id) {
    console.log("editar teste" + id);
    $("#MeuModal").load("/Dados/Edit/" + id, function () {
        $('#myModal').modal("show");
    });
};
function Detalhes(id) {
    $("#MeuModal").load("/Dados/Details/" + id, function () {
        $('#myModal').modal("show");
    });
};
function Deletar(id) {
    $("#MeuModal").load("/Dados/Delete/" + id, function () {
        $('#myModal').modal("show");
    });
};
function CadastrarController() {
    var valido = false;
    var userid = $('#UserId').val();
    var banco = $('#Banco').val();
    var agencia = $('#Agencia').val();
    var contac = $('#ContaCorrente').val();
    var cpf = $('#Cpf').val();
    var dv = $('#Dv').val();

    var token = $('input[name=__RequestVerificationToken]').val();
 
    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO
    valido = true;
    if ((dv.length > 0) == false) {
        valido = false;
        $('#Alert').show();
        $('#Alert').append('<span class="error">*Digito - Informe o digito da agencia</span></br>');
        console.log(dv.length);
    }
    if ((agencia.length > 3) == false) {
        valido = false;
        $('#Alert').show();
        $('#Alert').append('<span class="error">*Agencia - Agencia são 5 digitos</span></br>');
        console.log(agencia.length);
    }
    if ((cpf.length > 6) == false) {
        valido = false;
        $('#Alert').show();
        $('#Alert').append('<span class="error">*CPF - Dê um CPF valido</span></br>');
        console.log(cpf.length);
    }
    if ((contac.length > 4) == false) {
        valido = false;
        $('#Alert').show();
        $('#Alert').append('<span class="error">*Conta Corrent - Informe sua conta</span></br>');
        console.log(cpf.length);
    }

    var model = { "UserId": userid, "Banco": banco,"Agencia": agencia,"Dv": dv, "ContaCorrente": contac, "Cpf": cpf }
    console.log(model);

    if (valido != false) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/Dados/CreateModel/"+model,
            traditional: true,
            data: model,
            success: function (data) {
                alert(data.Message);
                LimparDados();
                visivel = false;
                $("#dadosAdd").hide();
                $("#Alert").hide();
                window.location.replace('/Dados/Index');
            }
        });
    } else {
        alert("Verifique os campos obrigatórios *");
    }
}
function LimparDados() {
    $('#Agencia').val('');
    $('#ContaCorrente').val('');
    $('#Cpf').val('');
    $('#Dv').val('');
}