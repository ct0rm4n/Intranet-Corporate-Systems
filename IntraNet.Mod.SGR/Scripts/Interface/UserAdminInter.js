
function VisualizaUserAdmin(id) {
    debugger;
    console.log(id);
    $("#loading").show();
    //var val = $("#CategoriasId").val();
    $("#MeuModal").load("/Demanda/EditAcoes/" + id, function () {
        debugger;
        $("#loading").hide();
        $(".modal-body,  #modal-ata").css("width", 1200);
        $('#myModal').modal("show");
        //$('#AlertReuniao').hide();
        console.log("load");
    });
}

function AlterarUserAdmin() {
    var userid = $("#UserId").val();
    if (userid) {
        console.log("Usuário quye sera alterado" + userid);
        $('#myModal').modal('hide');
        $("#loading").show();
        debugger
        $("#MeuModal").load("/User/Edit/" + userid,
            function () {
                $("#loading").hide();
                $(".modal-dialog").css("width", 800);
                $('#myModal').modal("show");
            });
        return [true];
    } else {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + "Selecione um usuário" + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
        $("#loading").hide();
        $('#myModal').modal('show');
    }
}

function InsereUserAdmin(id) {
    $('#myModal').modal('hide');
    $("#loading").show();
    $("#MeuModal").load("/User/Create",
        function () {
            $("#loading").hide();
            $(".modal-dialog").css("width", 800);
            $('#myModal').modal("show");
        });
}

function RemoverUserAdmin() {
    //console.log(model);
    debugger;
    var ID = $("#UserId").val();
    if (ID != null) {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal")
            .html(
                "<h2 align='center'><i class='fa fa-lg fa-fw fa-trash-o'>&nbsp;</i><strong>Remover usuário </strong></h2><hr><div class='container' align='center'><div class='alert alert-danger col-lg-12' align='center'>" +
                "Tem certeza que deseja remover a conta do usuário &nbsp;" +
                $("#UserName").val() +
                "</div ></div><div align='center' class='row col-lg-12'><button class='btn btnDefault btnReuniao' onclick='RemoverFuncionario()'><i class='fa fa-fw fa-trash-o'></i>&nbsp;Confirmar&nbsp;</button> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'><i class='fa fa-lg fa-fw fa-ban'></i>&nbsp;Cancelar&nbsp;</button></div>");

        $("#loading").hide();
        $('#myModal').modal('show');
    } else {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + "Selecione um usuário"+ "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
        $("#loading").hide();
        $('#myModal').modal('show');
    }
    
}
// crud inserir impresa,
// EMPRESA POUSSUI FILIAIS - UNIDADES
function AlterareEmpresaInter() {
    var empresaid = $("#EmpresaId").val();
    console.log("Usuário quye sera alterado" +empresaid);
    $('#myModal').modal('hide');
    $("#loading").show();
    debugger
    $("#MeuModal").load("/Empresas/Edit/" + empresaid,
        function () {
            $("#loading").hide();
            $(".modal-dialog").css("width", 800);
            $('#myModal').modal("show");
        });
    return [true];
}

function InsereEmpresaInter() {
    $('#myModal').modal('hide');
    $("#loading").show();
    $("#MeuModal").load("/Empresas/Create",
        function () {
            $("#loading").hide();
            $(".modal-dialog").css("width", 800);
            $('#myModal').modal("show");
        });
}

function RemoverEmpresa() {
    //console.log(model);
    debugger;
    var ID = $("#UserId").val();
    if (ID != null) {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal")
            .html(
                "<h2 align='center'><i class='fa fa-lg fa-fw fa-trash-o'>&nbsp;</i><strong>Remover usuário </strong></h2><hr><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" +
                "Tem certeza que deseja remover a conta do usuário &nbsp;" +
                $("#UserName").val() +
                "</div ></div><div align='center' class='row col-lg-12'><button class='btn btnDefault btnReuniao' onclick='RemoverFuncionario()'><i class='fa fa-fw fa-trash-o'></i>&nbsp;Confirmar&nbsp;</button> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'><i class='fa fa-lg fa-fw fa-ban'></i>&nbsp;Cancelar&nbsp;</button></div>");

        $("#loading").hide();
        $('#myModal').modal('show');
    } else {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + "Selecione um usuário" + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
        $("#loading").hide();
        $('#myModal').modal('show');
    }

}