function InsereAcaoDemanda(model) {
    $("#loading").show();
    model.ReuniaoId = $("#ReuniaoId").val();
    model.AssuntoId = $("#AssuntoId").val();
    console.log(model);
    debugger;
    $.ajax({
        type: "POST",
        url: "/AcaoDemanda/CreateAcao/",
        data: model,
        success: function (result) {
            debugger;
            //$("#loading").show();
            if (result.success == true) {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='fa fa-check-circle-o'>&nbsp;</i><strong>Concluido </strong></h2><hr><div class='container' align='center'><div class='alert alert-success col-lg-10' align='center'>" + result.message + "</div ></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='RedirecionaReuniao(" + result.ReuniaoId + ")'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
            }
            else {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + result.message + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='RedirecionaReuniao(" + result.ReuniaoId + ")'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");

            }
        }
    });
}
function AlterarAcaoDemanda(model) {    
    $("#loading").show();
    //model.ReuniaoId = $("#ReuniaoId").val();
    //model.AssuntoId = $("#AssuntoId").val();
    console.log(model);
    debugger;
    $.ajax({
        type: "POST",
        url: "/AcaoDemanda/Edit/",
        data: model,
        success: function (result) {
            debugger;
            debugger;
            if (result.success == 1) {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='fa fa-check-circle-o'>&nbsp;</i><strong>Concluido </strong></h2><hr><div class='container' align='center'><div class='alert alert-success col-lg-10' align='center'>" + result.message + "</div ></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                //ReloadSubGridItens();
                $('#myModal').modal("show");
                debugger
                ReloadSubGrid(model.DemandaId);
            }
            else {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + result.message + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
            }
        }
    });
}
function RemoverAcaoDemanda(ID) {
    //console.log(model);
    debugger;
    //var ID = $("#AcaoDemandaId").val();
    //var IP = myIP();
    debugger;
    $.ajax({
        type: "POST",
        url: "/AcaoDemanda/Delete/"+ID,
        data: ID,        
        success: function (result) {
            debugger;
            if (result.success == true) {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='fa fa-check-circle-o'>&nbsp;</i><strong>Concluido </strong></h2><hr><div class='container' align='center'><div class='alert alert-success col-lg-10' align='center'>" + result.message + "</div ></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='RedirecionaReuniao(" + result.ReuniaoId + ")'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
                debugger
                ReloadSubGrid(model.DemandaId);
            }
            else {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + result.message + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='RedirecionaReuniao(" + result.ReuniaoId + ")'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
            }
        }
    });
}