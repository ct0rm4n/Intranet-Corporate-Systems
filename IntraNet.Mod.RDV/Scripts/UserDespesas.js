function CriarDesp(id) {
    console.log("editar teste" + id);
    $("#MeuModal").load("/Despesa/Create/" + id, function () {
        $('#myModal').modal("show");
    });
};

function ImagesDesp() {
    var id = $("#RelatorioId").val();
    console.log("Imagens do relatorio:" + id);
    $("#MeuModal").load("/Despesa/Slider/" + id, function () {
        $('#myModal').modal("show");
    });
};
