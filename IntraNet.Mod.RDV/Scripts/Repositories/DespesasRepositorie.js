
function DespesasCreate(novo) {
    //var token = $('input[name=__RequestVerificationToken]').val();
    //var file = document.getElementById('ImageFile').files[0];
    var RelatorioId = $("#RelatorioId").val();
    var TipoDespesaId = $("#TipoDespesaId").val();
    var Descricao = $("#Descricao").val();
    var Obs = $("#Observacao").val();
    var Valor = $("#Valor").val();
    var UserId = $("#UserId").val();
    var data = $("Create").serialize();
    var fd = new FormData();
    //fd.append("ImageFile", file);
    fd.append("TipoDespesaId", TipoDespesaId);
    fd.append("RelatorioId", RelatorioId);
    fd.append("Descricao", Descricao);
    fd.append("Observacao", Obs);
    fd.append("Valor", Valor);
    fd.append("UserId", UserId);
    debugger;
    $.ajax({
        type: "POST",
        url: "/Despesa/CreateDesp/",
        processData: false,
        contentType: false,
        data: fd,
        success: function (result) {
            if (result.success == true) {
                debugger;
                $('#result').html("<div class='alert alert-success alert-dismissible' role='alert' align='left'><span></br>" + result.message + "</br></span><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button></div>");
                setTimeout(function () {
                    if (novo == true) {
                        LimparDespesa();
                    }
                    else if (novo == false) {
                        $('#myModal').modal('hide');
                        window.location.replace('/Relatorio/Edit/' + RelatorioId);
                    };
                },2000);
                
            }
            else {
                $('#result').html("<div class='alert alert-danger alert-dismissible col-lg-10' role='alert' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro - Verifique o formulário</span></br></br><span>" + result.message + "</span><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button></div>");
            }
        },
        error: function (er) {
            alert("Pcorreu um erro, verifique o formulário e certifique-se que nao esta inserindo uma imagem mais de uma vez");
        }
    });
}
