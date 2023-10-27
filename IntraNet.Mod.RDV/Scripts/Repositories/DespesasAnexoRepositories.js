function DespesasAnexoCreate() {
    var input = document.getElementById('ImageFile');
    var file = input.files[0];
    var RelatorioId = $("#RelatorioId").val();
    var UserId = $("#UserId").val();
    var data = $("Create").serialize();
    var fd = new FormData();
    fd.append("RelatorioId", RelatorioId);
    fd.append("UserId", UserId);
    fd.append("ImageFile", file);
    console.log(fd);
    console.log(file)
    debugger;
    $.ajax({
        type: "POST",
        url: "/DespesasAnexo/Create/",
        processData: false,
        contentType: false,
        data: fd,
        success: function (result) {
            if (result.success == true) {
                debugger;
                $('#result').html("<div class='alert alert-success alert-dismissible' role='alert' align='left'><span></br>" + result.message + "</br></span><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button></div>");
                setTimeout(function () {
                    $('#myModal').modal('hide');
                    window.location.replace('/Relatorio/Edit/' + RelatorioId);
                }, 2000);
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