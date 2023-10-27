function InsereAssunto() {
    var model = $("#Create").serialize();
    //model.ReuniaoId = $("#ReuniaoId").val();
    console.log(model);
    debugger;
    $("#loading").show();
    debugger;
    $.ajax({
        type: "POST",
        url: "/Assunto/Create/",
        data: model,
        success: function (result) {
            debugger;
            //$("#loading").show();
            if (result.success == true) {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='fa fa-check-circle-o'>&nbsp;</i><strong>Concluido </strong></h2><hr><div class='container' align='center'><div class='alert alert-success col-lg-10' align='center'>" + result.message + "</div ></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='RedirecionaReuniao(" + result.ReuniaoId +")'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
                setTimeout(function () {
                    window.location.replace('/Reuniao/Ata/' + model.ReuniaoId)
                }, 20000);
            }
            else {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + result.message + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='RedirecionaReuniao(" + result.ReuniaoId +")'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");

            }
        }
    });
}
function AlterarAssunto() {    
    var model = $("#EditAssunto").serialize();    
    $("#loading").show();
    //var IP = myIP();
    debugger;
    $.ajax({
        type: "POST",
        url: "/Assunto/EditAssunto/",
        data: model,
        success: function (result) {
            debugger;
            //$("#loading").show();
            if (result.success == true) {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='fa fa-check-circle-o'>&nbsp;</i><strong>Concluido </strong></h2><hr><div class='container' align='center'><div class='alert alert-success col-lg-10' align='center'>" + result.message + "</div ></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='RedirecionaReuniao(" + result.ReuniaoId +")'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
                setTimeout(function () {
                    window.location.replace('/Reuniao/Ata/' + model.ReuniaoId)
                }, 20000);
            }
            else {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + result.message + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='RedirecionaReuniao(" + result.ReuniaoId +")'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");

            }
        }
    });
}
function RemoverAssunto() {
    
    debugger;
    var ID = $("#AssuntoId").val();
    //var IP = myIP();
    $("#loading").show();
    debugger;
    $.ajax({
        type: "POST",
        url: "/Assunto/Delete/"+ID,
        data: ID,        
        success: function (result) {
            debugger;
            if (result.success == true) {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='fa fa-check-circle-o'>&nbsp;</i><strong>Concluido </strong></h2><hr><div class='container' align='center'><div class='alert alert-success col-lg-10' align='center'>" + result.message + "</div ></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='RedirecionaReuniao(" + result.ReuniaoId+")'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
                setTimeout(function () {
                    window.location.replace('/Reuniao/Ata/' + $("#ReuniaoId").val())
                }, 20000);
            }
            else {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + result.message + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='RedirecionaReuniao(" + result.ReuniaoId +")'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");

            }
        }
    });
}
