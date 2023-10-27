$(document).ready(function() {
    $('#RevisaoAdd').hide();
});
var revisar_visivel = false;
function Revisar() {
    if (revisar_visivel == false) {
        revisar_visivel = true;
        $('#RevisaoAdd').show();
    } else {
        revisar_visivel = false;
        $('#RevisaoAdd').hide();
    }
};

function Enviar() {
    var relatorioid = $("#RelatorioId").val();
    var userid = $("#UserId").val();
    //var token = $('input[name=__RequestVerificationToken]').val();
    var mensagem = $('#Mensagem').val();

    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO
    valido = true;
    if ((mensagem.length > 5) == false) {
        valido = false;
        $('#Alert').show();
        $('#Alert').append('<span class="error">*Mensagem - Informe o que deve ser alterado no relatório</br></span></br>');
        alert("mensagem pequena");
    }
    var visivel = true;
    
    var model = { "userid": userid, "texto": mensagem, "id": relatorioid }
    console.log(model);

    if (valido != false) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/Relatorio/EnviarRevisao/" + model,
            traditional: true,
            data: model,
            success: function (data) {
                alert(data.Message);
                $("#RevisaoAdd").hide();
                //alert("Faça valer a pena cada segundo como se fosse o ultimo");
            }
        });
    } else {
        alert("Verifique os campos obrigatórios *");
    }
}

function AprovarRev(id) {
    var valido = false;
    if (confirm("Tem certeza que o relatório está coerente? Após confirmação o Gestor responsavel pelo aprovamento será notificado.")) {
        valido = true;
        if (valido != false) {
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/Relatorio/AprovarRev/" + id,
                traditional: true,
                data: id,
                success: function (data) {
                    alert(data.Message);
                    history.go(0);
                    return true;
                }
            });
        } else {
            alert("Ocorreu um erro, procure seu suporte de T.I *");
        }
    }    
}

function AprovarRelat(id) {
    var valido = false;
    if (confirm("Tem certeza que o relatório está coerente? Após confirmação o financeiro responsavel será notificado para realizar o reembolso.")) {
        valido = true;
        if (valido != false) {
            $.ajax({
                type: "POST",
                dataType: "json",
                url: "/Relatorio/AprovarRelat/" + id,
                traditional: true,
                data: id,
                success: function (data) {
                    alert(data.Message);
                    history.go(0);
                    return true;
                }
            });
        } else {
            alert("Ocorreu um erro, procure seu suporte de T.I *");
        }
    }
}