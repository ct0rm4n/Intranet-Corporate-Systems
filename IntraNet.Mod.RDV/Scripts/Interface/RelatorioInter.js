$(document).ready(function () {
    $("#loading").hide();
});
function GerenciarComprovantes(id) {
    $("#MeuModal").load("/DespesasAnexo/ManegeImages/"+id, function () {
        $('#myModal').modal("show");
    });
};

