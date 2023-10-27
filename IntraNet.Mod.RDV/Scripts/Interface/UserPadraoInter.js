$(document).ready(function () {
    var situacao_ = "#SituacaoRelat";
    situacao(situacao_);
    console.log("Colorir situacao");

});
function GerenciarAvatar() {
	$("#MeuModal").load("/Manage/InserirAvatar", function () {
		$('#myModal').modal("show");
	});
};
function PesquisaRelatorio() {
    $("#MeuModal").load("/Relatorio/Pesquisa", function () {
        $('#myModal').modal("show");
    });
};