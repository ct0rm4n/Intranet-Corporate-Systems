$(document).ready(function () {
    $('#RateioAdd').hide();
    $('#DespesaAdd').hide();
    $('#DescricaoOutros').hide();
    $('#AlertRateio').hide();
    //$('#AlertEditRelatorio').hide();
    $('#AlertRelatC').hide();
    $('#Valor').calculator();
    $('#Mymodal, .DescricaoDiv').hide();
    $('.DescricaoDiv').hide();
    $('[data-toggle="tooltip"]').tooltip();
    setTimeout(function () { Calcular() }, 1000);
    situacao();
    $.validator.methods.range = function (value, element, param) {
        var globalizedValue = value.replace(",", ".");
        return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
    }

    $.validator.methods.number = function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    }
});

function situacao(situacao) {
    //var situacao = $("#Situacao").val();
    console.log(situacao);
    var sit = $(situacao).val();
    console.log(sit);
    switch (sit) {
        case 'Incompleto':
            $(situacao).css("color", "red");
            $(situacao).css("background-color", "red")
            break;
        case 'Pendente Revisao':
            $(situacao).css("color", "#FF8C00");
            $(situacao).css("background-color", "#FF8C00")
            break;
        case 'Pendente Aprovacao':
            $(situacao).css("color", "#BF00FF");
            $(situacao).css("background-color", "#BF00FF")
            break;
        case 'Aprovado':
            $(situacao).css("color", "#FFD700")
            $(situacao).css("background-color", "#FFD700");
            break;
        case 'Pagamento realizado':
            $(situacao).css("color", "#8B8B7A")
            $(situacao).css("background-color", "#8B8B7A");
            break;
        case 'Pagamento realizado utilizado':
            $(situacao).css("color", "#8B8B7A")
            $(situacao).css("background-color", "#8B8B7A");
            break;
        default:
            console.log('sem situacao')
            break;
    }

}
function devolucao(devolucao) {
    //var situacao = $("#Situacao").val();
    console.log(devolucao);
    var dev = $(devolucao).text();
    //console.log($(devolucao).text());
    if (dev.match(/-/)) {
        $(devolucao).css("color", "red")
        console.log("Negativo" + dev)
    }
    else{
        $(devolucao).css("color", "green")
        console.log("Positivo"+dev)
    }

}
var visivel_addRt = false;
function btnAddRateio() {
    if (visivel_addRt == false) {
        $('#RateioAdd').show();
        visivel_addRt = true;
    }
    else if (visivel_addRt == true) {
        $('#RateioAdd').hide();
        visivel_addRt = false;
    }
}
var visivel_addDp = false;
function btnAddDespesa() {
    if (visivel_addDp == false) {
        $('#DespesaAdd').show();
        visivel_addDp = true;
    }
    else if (visivel_addDp == true) {
        $('#DespesaAdd').hide();
        visivel_addDp = false;
    }
}
function btnCamcelarRateio() {
    $('#RateioAdd').hide();
    visivel_addRt = false;
}
function MascarasR() {
    var $AdiantamentoValor = $("#AdiantamentoValor");
    $AdiantamentoValor.mask('##0,00', { reverse: true });
    var $relatAdian = $("#Adiantamento");
    $relatAdian.mask('#.##0,00', { reverse: true });
    var $porcentagem = $("#Porcentagem");
    $porcentagem.mask('000', { reverse: true, placaholder: '000',min: 0,max: 100 });
    var $relatValor = $("#Valor");
    $relatValor.mask('00000,00', { reverse: true });
}
function Calcular() {
    //ALTERAR COR DO RESUMO DO RELTÓRIO CONFORME ADIANTAMENTO E DESPESAS
    valor = $("#Total").val();
    adiant = $("#AdiantamentoId").text();
	console.log(adiant);
	adiant = adiant.replace(",", ".");
	console.log(adiant);
	if (adiant < 0 || adiant == null || adiant == '') { adiant = 0; console.log("Adiantamento:" + adiant) }
	debugger;
    console.log("Total:"+valor +"Adiantamento:" +adiant);    
    var priceSum = parseFloat(valor) - (parseFloat(adiant)).toFixed(2);
    console.log(priceSum);
    if (priceSum < 0) { $("#AReceber").val(priceSum.toFixed(2)).css('color', 'red'); }
    if (priceSum > 0) { $("#AReceber").val(priceSum.toFixed(2)).css("color", "green"); }
};
function SetarCorDelvolucao() {
  console.log("Teste");
}
function CadastrarRelatController() {
    var validor = false;
    var userid = $('#UserId').val();
    var empresaid = $('#EmpresaId').val();
    var user = $('#UserName').val();
    var motivo = $('#Motivo').val();
    var saida = $('#Saida').val();
    var retorno = $('#Retorno').val();
    var aprovador = $('#AprovadorId').val();
    var dados = $('#DadosBancariosId').val();

    var token = $('input[name=__RequestVerificationToken]').val();
    validor = true;
    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO
    
    var model = { "EmpresaId": empresaid, "UserId": userid, "UserName": user, "Motivo": motivo, "Saida": saida, "Retorno": retorno, "AprovadorId": aprovador, "DadosBancariosId": dados }
    //debugger;
    console.log(model);
    debugger;
    console.log(validor);
    if (validor == true) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/Relatorio/CreateRelat/",
            traditional: true,
            cache: false,
            data: model,
            success: function (data) {
                console.log(data.id);
                alert(data.Message);
                debugger;
                if (data.inserido == true) {
                    $('#result').html("<div class='alert alert-success alert-dismissible' role='alert' align='left'><span></br>" + data.Message + "</br></span><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button></div>");
                    setTimeout(function () { window.location.replace('/Relatorio/Edit/' + data.Id); }, 2000);
                    return true;
                } else {
                    debugger;
                    $('#result').html("<div class='alert alert-danger alert-dismissible col-lg-10' role='alert' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro - Verifique o formulário</span></br></br><span>" + data.Message + "</span><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button></div>");
                }
            }
        });
    } else {
        //debugger;
        return false;
        alert("Ocorreu um erro");
        
    }
}
function SalvarEdicaoController(id) {
    var valido = false;
    var userid = $('#UserId').val();
    var empresaid = $('#EmpresaId').val();
    var user = $('#UserName').val();
    var motivo = $('#Motivo').val();
    var saida = $('#Saida').val();
    var retorno = $('#Retorno').val();
    var aprovador = $('#AprovadorId').val();
    var dados = $('#DadosBancariosId').val();
    var areceber = $('#AReceber').val();
    var obs =$('#Observacoes').val();
    var total =$('#Total').val();
    var adiantaId =$('#AdiantamentoId').val();
    var adiantaV = $('#AdiantamentoId').text();

    var token = $('input[name=__RequestVerificationToken]').val();

    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO
    valido = true;
    var model = { "RelatorioId": id, "EmpresaId": empresaid, "UserId": userid, "UserName": user, "Motivo": motivo, "Saida": saida, "Retorno": retorno, "AprovadorId": aprovador, "DadosBancariosId": dados, "AReceber": areceber.replace('.', ','), "Observacoes": obs, "Total": total.replace('.', ','), "AdiantamentoValor": adiantaV.replace('.', ''), "AdiantamentoId": adiantaId }
    console.log(model);
    if (valido != false) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/Relatorio/Edit/" + model,
            traditional: true,
            data: model,
            success: function (data) {        
                if (data.inserido == true) {
                    alert(data.Message);
                    $('#AlertEditRelatorio').html("<div class='alert alert-success alert-dismissible' role='alert' align='left'><span></br>" + data.Message + "</br></span><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button></div>");
                    window.scrollBy(0, -1000);
                    setTimeout(function () { window.location.replace('/Relatorio/'); }, 20000);
                    console.log("inserido:" + data.inserido);
                    //$("#PDF").prop('disabled', false);
                    //$(".PDF").prop('disabled', false);
                    
                }
                if (data.inserido == false) {
                    debugger;
                    window.scrollBy(0, -1000);
                    $("#AlertEditRelatorio").html("<div class='alert alert-danger alert-dismissible col-lg-10' role='alert' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro - Verifique o formulário</span></br></br><span>" + data.Message + "</span><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button></div>");                    
                }
            },
            error: function (data) {
                if (data.inserido == false) {
                    debugger;
                    alert("Erro: " + data.Message);
                    console.log("inserido:" + data.inserido);
                }
            }
        });
    } else {
        $('html,body').animate({ scrollTop: 0 }, 'slow');
        alert("Verifique os campos obrigatórios *");
    }
}
function CCustoSelector() {
    var id = $('#SolicitaCCustoId').val();
    var model = { "ccid": id }
    console.log("Seleciona" + model.ccid);
    var valido = true;
    if (model.ccid.empty || model.ccid =='0' || model.ccid =='') {
        valido = false;
        console.log("Erro.." + model.ccid);
    }
    if (valido == true) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/SolicitaCusto/GetCCSelect/",
            traditional: true,
            data: model,
            success: function (data) {
                //alert(data.Message);
                if (data != null) {
                    console.log("Sucesso");
                    console.log(data.Ccusto);
                    $("#Classe").empty();
                    $("#EmpCCustoId").empty();
                    $("#Classe").append('<option value="">' + "Selecione a Unidade" + '</option>');
                    $("#Prospect").prop('disabled', true);
                    $.each(data.Ccusto,
                        function(index, element) {
                            //$("#EmpCCustoId").prop('disabled', false);
                            $("#EmpCCustoId").append('<option value="' + element.Value + '">' + element.Text + '</option>');
                        });
                    //projeto ja pega do cc a unidade
                    $.each(data.Classe,
                        function(index, element) {
                            //$("#Classe").prop('disabled', false);
                            $("#Classe").append('<option value="' + element.Value + '">' + element.Text + '</option>');
                        });
                    //$("#Item").val(data.Item);
                    if (data.Prospect == true) {
                        $("#Prospect").prop('disabled', false);
                    }
                    else if(data.Prospect == false) {
                        $("#Prospect").prop('disabled', true);
                        $("#Prospect").val('');
                    }
                } else {
                    $("#Prospect").prop('disabled', true);                    
                    $("#EmpCCustoId").empty();
                    $("#Prospect").val('');
                }
            }
        });
    }
    else {
        console.log("Vazio");
        $("#EmpCCustoId").empty();
        $("#EmpCCustoId").append('<option value="">' + "00000000" + '</option>');

        $("#Prospect").val("");
        $("#Prospect").prop('disabled', true);

        $("#Classe").empty();
        $("#Classe").append('<option value="">' + "Selecione a Unidade" + '</option>');
    }
}
function CriarRateioController() {
    var valido = false;
    var empresaid = $('#EmpresaId').val();
    var userid = $('#UserId').val();
    var solicitacc = $('#SolicitaCCustoId').val();
    var empccid = $('#EmpCCustoId').val();
    var item = $('#Item').val();
    var classe = $('#UnidadeClasse').val();
    var prospect = $('#Prospect').val();
    var porcentagem = $('#Porcentagem').val();
    var relatorioid = $('#RelatorioId').val();

    var token = $('input[name=__RequestVerificationToken]').val();

    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO
    valido = true;
    
    valido = true;
    if (porcentagem <1 || porcentagem >100) {
        valido = false;
        $('#AlertRateio').show();
        $('#AlertRateio').append('<span class="error">*% Porcentagem - É necessario informar um valor vaido.</span></br>');
        console.log("Erro motivo");
    }
    if (solicitacc == '0' || solicitacc =='') {
        valido = false;
        $('#AlertRateio').show();
        $('#AlertRateio').append('<span class="error">*Centro de Custo - É necessario informar C.Custo.</span></br>');
        console.log("Erro motivo");
    }


    var model = { "SolicitanteCC": solicitacc, "UnidadeClasse": classe, "RelatorioId": relatorioid, "Item": item, "EmpCCustoId": empccid, "Prospect": prospect, "Porcentagem": porcentagem }
    console.log(model);

    if (valido != false) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/RateioItems/Create/" + model,
            traditional: true,
            data: model,
            success: function (data) {
                console.log(data.id);
                alert(data.Message);
                if (data.inserido == true) {
                    window.location.replace('/Relatorio/Edit/' + relatorioid);
                }
            }
        });
    } else {
        alert("Verifique os campos obrigatórios *");
    }
}
function DeletarRateio(id) {
    $("#MeuModal").load("/RateioItems/Delete/" + id, function () {
        $('#myModal').modal("show");
    });
};
