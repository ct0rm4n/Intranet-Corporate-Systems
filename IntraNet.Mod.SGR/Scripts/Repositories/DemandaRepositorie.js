function InserirDemanda() {
    var validor = false;

    var create = $("#Create").serialize();
    create.ReuniaoId = $("#ReuniaoId").val();

    console.log($("#ReuniaoId").val());
    var token = $('input[name=__RequestVerificationToken]').val();
    validor = true;
    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO  
   
    //debugger;
    console.log(create);
    console.log(validor);
    
    if (validor == true) {
        $.ajax({
            type: "POST",
            url: "/Demanda/CreateDemanda/",
            data: create,
            success: function (result) {
                debugger;
                if (result.success == 'true') {
                    debugger;
                    $('#result').html("<div class='alert alert-success col-lg-10' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><strong>Concluido</strong></br></br>" + result.message + "</div>");
                    //window.location.replace('/PainelSGR')
                }
                else {
                    debugger;
                    $('#result').html("<div class='alert alert-danger col-lg-10' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro - Verifique o formulário</span></br></br>" + result.message + "</div>");
                }
            }
        });
    } else {
        alert("Ocorreu um erro");
    }
}
function AlterarDemanda() {
    var validor = false;

    var create = $("#Edit").serialize();
    create.ReuniaoId = $("#ReuniaoId").val();

    console.log($("#ReuniaoId").val());
    var token = $('input[name=__RequestVerificationToken]').val();
    validor = true;
    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO  

    //debugger;
    console.log(create);
    console.log(validor);

    if (validor == true) {
        $.ajax({
            type: "POST",
            url: "/Demanda/CreateDemanda/",
            data: create,
            success: function (result) {
                debugger;
                if (result.success == 'true') {
                    debugger;
                    $('#result').html("<div class='alert alert-success col-lg-10' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><strong>Concluido</strong></br></br>" + result.message + "</div>");
                    //window.location.replace('/PainelSGR')
                }
                else {
                    debugger;
                    $('#result').html("<div class='alert alert-danger col-lg-10' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro - Verifique o formulário</span></br></br>" + result.message + "</div>");
                }
            }
        });
    } else {
        alert("Ocorreu um erro");
    }
}
