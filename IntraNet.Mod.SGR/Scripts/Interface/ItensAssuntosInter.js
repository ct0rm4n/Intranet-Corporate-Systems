function VisualizaItem(id) {
    debugger;
    console.log(id);
    //var val = $("#CategoriasId").val();
    $("#MeuModal").load("/ItemAssunto/ExibeDemandas/"+id, function () {
        $('#myModal').modal("show");
        //$("#myModal").dialog("option", "width", 800);
        //$("#myModal").css("width", 800);
        $(".modal-body,  #modal-ata").css("width",1200);
        //$('#AlertReuniao').hide();
        console.log("load");
    });
}
function ExportaPDF(id) {
    debugger;
    console.log(id);
    //var val = $("#CategoriasId").val();
    $.ajax({
        type: "POST",
        dataType: "json",
        url: "/ReportCrystal/UserReportDemandaPorItem/" + id,
        traditional: true,
        data: id,
        success: function (data) {
            console.log("Desu, foi pra funcao e retornou:"+data);

        }
    });
}


function RemoverItemAssuntoInter() {
    //console.log(model);
    debugger;
    var ID = $("#ItemAssuntoId").val();
    if (ID) {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal")
            .html(
                "<h2 align='center'><i class='fa fa-lg fa-fw fa-trash-o'>&nbsp;</i><strong>Remover item assunto </strong></h2><hr><div class='container' align='center'><div class='alert alert-danger col-lg-12' align='center'>" +
                "Tem certeza que deseja remover o assunto: &nbsp;" +
                $("#DescItemAssunto").val()  +
                "</div ></div><div align='center' class='row col-lg-12'><button class='btn btnDefault btnReuniao' onclick='RemoverItemAssunto()'><i class='fa fa-fw fa-trash-o'></i>&nbsp;Confirmar&nbsp;</button> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'><i class='fa fa-lg fa-fw fa-ban'></i>&nbsp;Cancelar&nbsp;</button></div>");

        $("#loading").hide();
        $('#myModal').modal('show');
    } else {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + "Selecione um item" + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
        $("#loading").hide();
        $('#myModal').modal('show');
    }

}
function AdicionarItemInter() {
    //console.log("Voce quer adicionar um novo item ao assunto" + id + ".")
    $("#jqGridAtaAssuntosItens_iladd").trigger('click');

}