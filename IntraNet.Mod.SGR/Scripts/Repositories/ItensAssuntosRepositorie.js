function InsereItemAssunto(model) {
   
    $("#loading").show();
    model.ReuniaoId = $("#ReuniaoId").val();
    model.AssuntoId = $("#AssuntoId").val();
    console.log(model);
    debugger;
    $.ajax({
        type: "POST",
        url: "/ItemAssunto/Create/",
        data: model,
        success: function (result) {
            debugger;
            if (result.success == 1) {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='fa fa-check-circle-o'>&nbsp;</i><strong>Concluido </strong></h2><hr><div class='container' align='center'><div class='alert alert-success col-lg-10' align='center'>" + result.message + "</div ></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                //ReloadSubGridItens();
                $('#myModal').modal("show");
                debugger
                setTimeout(function () {
                    $("#loading").hide();
                    window.location.replace('/Reuniao/Ata/' + $("#ReuniaoId").val())
                }, 5000);
                
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
function AlterarItemAssunto(model) {
    model.ReuniaoId = $("#ReuniaoId").val();
    model.AssuntoId = $("#AssuntoId").val();
    console.log(model);
    $("#loading").show();
    debugger;
    $.ajax({
        type: "POST",
        url: "/ItemAssunto/Edit/",
        data: model,
        success: function (result) {
            debugger;
            if (result.success == true) {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='fa fa-check-circle-o'>&nbsp;</i><strong>Concluido </strong></h2><hr><div class='container' align='center'><div class='alert alert-success col-lg-10' align='center'>" + result.message + "</div ></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
                debugger
                setTimeout(function () {
                    window.location.replace('/Reuniao/Ata/' + $("#ReuniaoId").val())
                }, 5000);
                //ReloadSubGrid(model.AssuntoId);
            }
            else {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + result.message + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
                //ReloadSubGrid(model.AssuntoId);
            }
        }
    });
}
function RemoverItemAssunto() {
    //console.log(model);
    debugger;
    var ID = $("#ItemAssuntoId").val();
    $("#loading").show();
    debugger;
    $.ajax({
        type: "POST",
        url: "/ItemAssunto/Delete/" + ID,
        data: ID,
        success: function (result) {
            if (result.success == true) {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='fa fa-check-circle-o'>&nbsp;</i><strong>Concluido </strong></h2><hr><div class='container' align='center'><div class='alert alert-success col-lg-10' align='center'>" + result.message + "</div ></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
                debugger
                setTimeout(function () {
                    window.location.replace('/Reuniao/Ata/' + $("#ReuniaoId").val())
                }, 20000);
                //ReloadSubGrid(model.AssuntoId);
            }
            else {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + result.message + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
                //ReloadSubGrid(model.AssuntoId);
            }
        }
    });
}
