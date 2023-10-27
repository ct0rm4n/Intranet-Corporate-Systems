function InsereAssuntoInterInter() {
    debugger;
    var id = $("#ReuniaoId").val();
    console.log("Insrir assunto na reuniao:"+id);
    $("#loading").show();
    //var val = $("#CategoriasId").val();
    $("#MeuModal").load("/Assunto/Create/" + id, function () {
        debugger;
        $("#loading").hide();
        //$(".modal-body,  #modal-ata").css("width", 1200);
        $('#myModal').modal("show");
        //$('#AlertReuniao').hide();
        console.log("load");
    });
}

function EditarAssuntoInter() {
    var id = $("#AssuntoId").val();
    debugger
    if (id) {
        console.log("Editar assunto:" + id);
        $("#loading").show();
        //var val = $("#CategoriasId").val();
        $("#MeuModal").load("/Assunto/Edit/" + id, function () {
            debugger;
            $("#loading").hide();
            //$(".modal-body,  #modal-ata").css("width", 1200);
            $('#myModal').modal("show");
            //$('#AlertReuniao').hide();
            console.log("load");
        });
    } else {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + "Selecione o assunto" + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
        $("#loading").hide();
        $('#myModal').modal('show');
    }
}
function SelecionaAssuntoInter(term) {
    $("#AssuntoId").val(term);    
    console.log("Assunto escolhido" + term);
}

function RemoverAssuntoInter() {
    var ID = $("#AssuntoId").val();
    if (ID) {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal")
            .html(
                "<h2 align='center'><i class='fa fa-lg fa-fw fa-trash-o'>&nbsp;</i><strong>Remover assunto </strong></h2><hr><div class='container' align='center'><div class='alert alert-danger col-lg-12' align='center'>" +
                "Tem certeza que deseja remover o assunto: &nbsp;" +
                $("#DescAssunto").val() +", os itens relacionádos tambem serão removidos"+
                "</div ></div><div align='center' class='row col-lg-12'><button class='btn btnDefault btnReuniao' onclick='RemoverAssunto()'><i class='fa fa-fw fa-trash-o'></i>&nbsp;Confirmar&nbsp;</button> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'><i class='fa fa-lg fa-fw fa-ban'></i>&nbsp;Cancelar&nbsp;</button></div>");

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