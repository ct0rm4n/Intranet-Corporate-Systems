function RelatorioDemandasAcoesInter(){
    debugger;
    $("#loading").show();
    $("#MeuModal").load("/Gestao/RelatorioDemandasAcoes",
        function () {
            $("#loading").hide();
            $('#myModal').modal("show");
        });
    return [true];

}