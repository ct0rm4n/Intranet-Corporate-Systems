function RetornoPesquisa() {
    //var token = $('input[name=__RequestVerificationToken]').val();
    //var file = document.getElementById('ImageFile').files[0];    
    var PalavraPasse = $("#PalavraPasse").val();
    var Situacao = $("#Situacao").val();
    var Inicial = $("#Inicial").val();
    var Final = $("#Final").val();    
    var fd = new FormData();
    fd.append("PalavraPasse", PalavraPasse);
    fd.append("Situacao", Situacao);
    fd.append("Inicial", Inicial);
    fd.append("Final", Final);
    var model = { "PalavraPasse": PalavraPasse, "Situacao": Situacao, "Inicial": Inicial, "Final": Final }
    console.log(model);


    debugger;
    $.ajax({
        type: "POST",
        url: "/Relatorio/PesquisaRetorno/",
        data: model,
        success: function (result) {
            if (result) {
                debugger;
                $('#myModal').modal("hide");
                $('.panel-body').html();
                $('.panel-body').html(result)
                debugger;
            }
            else {
                debugger;
                $('#result').html("<div class='alert alert-danger alert-dismissible col-lg-10' role='alert' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro - Verifique o formulário</span></br></br><span>" + result.message + "</span><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button></div>");
            }
            debugger;
        },
        error: function (er) {
            alert("Pcorreu um erro, verifique o formulário e certifique-se que nao esta inserindo uma imagem mais de uma vez");
        }
    });
}