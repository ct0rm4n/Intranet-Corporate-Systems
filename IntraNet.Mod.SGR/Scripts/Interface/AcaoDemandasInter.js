function form_visibility(id) {
    var e = document.getElementById(id);
    if ($(id).css('display') == 'none') {
        $(id).css('display', 'block')
    }
    else {
        InsereAcaoDemanda(id)
    }
}
function AcaoDemandaHabilitaFeito() {
    debugger;
    alert('Teste');
    if ($('#Feito').is(':checked')) {
        $("#Data").prop("disabled", false)
    } else {
        $("#Data").val('')
        $("#Data").prop("disabled", true)
    }
}
function RemoverAcaoInter(ID) {
    //console.log(model);
    debugger;
    //var ID = $("#AcaoDemandaId").val();
    if (ID) {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal")
            .html(
                "<h2 align='center'><i class='fa fa-lg fa-fw fa-trash-o'>&nbsp;</i><strong>Remover registro de ação</strong></h2><hr><div class='container' align='center'><div class='alert alert-danger col-lg-12' align='center'>" +
                "Tem certeza que deseja remover o histórico da demanda &nbsp;" +                
                "</div ></div><div align='center' class='row col-lg-12'><button class='btn btnDefault btnReuniao' onclick='RemoverAcaoDemanda("+ID+")'><i class='fa fa-fw fa-trash-o'></i>&nbsp;Confirmar&nbsp;</button> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'><i class='fa fa-lg fa-fw fa-ban'></i>&nbsp;Cancelar&nbsp;</button></div>");

        $("#loading").hide();
        $('#myModal').modal('show');
    } else {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + "Selecione um assunto" + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
        $("#loading").hide();
        $('#myModal').modal('show');
    }

}