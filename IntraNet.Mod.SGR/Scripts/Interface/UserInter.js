$(document).ready(function () {
    $("#loading").hide();
    $("#SelectAssuntos").on(function () {
        alert("Editando..")
    })
    console.log("Demandado..")
    $('a[rel=popover]').popover();
    $('[data-toggle="popover"]').popover({
        trigger: "hover",
        container: "body",
        html: true,
        placement: "rigth"
    }).css("width", "350px");
    // Tooltips
    //$('#DemandaAjuda').popover({
        //offset: 50,
        //placement: 'right'
    //});
    $('a[rel=popover]').popover();
    //$("#DemandaAjuda").data('popover').tip().css("z-index", 99999);
    
    if ($(".Situacao_item").val() != null && $("#Situacao_item").val() != "") {
        situacaoitem()
    }
    
});
function RedirecionaReuniao(id) {
    window.location.replace("/Reuniao/Ata/" + id );
}

function ReloadSubGrid(id) {
    $("#" + id).trigger("reloadGrid");
    $("#jqGridAtaAssuntos").expandSubGridRow(id);
}
function FecharModal() {
    $('#myModal').modal('hide');
}

function situacao(situacao) {
    debugger;
    switch ($(situacao).val()) {
    case 'Aberto':
        $(situacao).css("color", "red");
        $(situacao).css("background-color", "red")
        break;
    case 'Suspenso':
        $(situacao).css("color", "orange");
        $(situacao).css("background-color", "orange")
        break;
    case 'Encerrado':
        $(situacao).css("color", "orange");
        $(situacao).css("background-color", "green")
        break;
    default:
        
        break;
    }

}
function situacaoitem() {
    debugger;
    switch ($(".Situacao_item").val()) {
    case 'Aberto':
        $(".Situacao_item").css("color", "red");
        break;
    case 'Suspenso':
        $(".Situacao_item").css("color", "orange");
        break;
    case 'Encerrado':
        $(".Situacao_item").css("color", "orange");
        break;
    default:

        break;
    }

}

function proximo_segundo() {
    var hoje = new Date
    var hora = hoje.getHours()
    var minutos = hoje.getMinutes()
    var segundos = hoje.getSeconds()
    relogio = document.getElementById('relogio')
    relogio.value = hora + ":" + minutos + ":" + segundos
    setTimeout('proximo_segundo()', 1000)
}
function ReloadGrid() {
    $("#jqGridDemanda").jqGrid("setGridParam", { datatype: "json" })
        .trigger("reloadGrid", [{ current: true }]);
}
function ConfirmaAbrirReuniao() {
    $("#MeuModal").load("/Reuniao/Confirma/", function () {
        $('#myModal').modal("show");
        //$('#AlertReuniao').hide();
        console.log("load");
    });
};
function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
