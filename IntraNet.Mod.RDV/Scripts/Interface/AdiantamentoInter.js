$(document).ready(function () {
    $("#loading").hide();
})
function InserirAdiantamento() {
    $("#MeuModal").load("/Adiantamento/Create", function () {
        $('#myModal').modal("show");
    });

};
function AprovarAdiantamento(Id) {
    debugger;
    console.log(Id)
    $("#MeuModal").load("/Adiantamento/Details/" + Id, function () {
        $('#myModal').modal("show");
    });

};
function FinanceiroVisualizaAdiantamento(Id) {
    debugger;
    console.log(Id)
    $("#MeuModal").load("/Adiantamento/Details/" + Id, function () {
        $('#myModal').modal("show");
    });

};