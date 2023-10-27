
$(document).ready(function () {
    $(".oculta").hide();
    $(".modal-dialog").find('#Descricao').hide();
    $('#Mymodal, #Descricao').hide();
    $('#Mymodal, .masterTooltip').hover(function () {    
        $('#Mymodal').on('#MeuModal', function () {
        $('.tooltips').tooltip();
    });

    var title = $(this).attr('title');
    $(this).data('tipText', title).removeAttr('title');
    $('<p class="tooltip"></p>')
            .html(title)
            .appendTo('body')
            .fadeIn('fast');
    }, function () {

    $(this).attr('title', $(this).data('tipText'));
        $('.tooltip').remove();
    }).mousemove(function (e) {

        // Get X Y coordinates
        var mousex = e.pageX + 20;
        var mousey = e.pageY + -25;
    $('.tooltip').css({ top: mousey, left: mousex, html: true });
    });

});

function SelecionaVeiculo() {
    //var id = $("#Valor").val();
    var valor = $("#TipoDespesaId").val();
    var id = $("#Descricao").val();
    var model = { "id": id }
    debugger
    console.log(valor);
    if (valor == 11) {
        console.log("Entrou");
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/Despesa/GetVeiculoSelect/",
            data: model,
            success: function (result) {
                console.log(result.Valor);
                if (result.Valor > 0) {
                    var val = result.Valor.toString().replace('.',',');
                    console.log(val);
                    $("#Valor").val(val);
                }
            }
        });
    }
}

function LimparDados() {
    $('#Agencia').val('');
    $('#ContaCorrente').val('');
    $('#Cpf').val('');
    $('#Dv').val('');
}

function FormularioDespesa() {
    $("#MeuModal").find('#DescricaoOutros').hide();
};

function InserirDespesa(Id) {    
    $("#MeuModal").load("/Despesa/Create/" + Id, function () {        
        $('#myModal').modal("show");
        $('#Mymodal, .DescricaoDiv').hide();
    });
    
};

function VerDesp(id) {
    console.log("editar teste" + id);
    $("#MeuModal").load("/Despesa/Details/" + id, function () {
        $('#myModal').modal("show");
    });
};

function DetalhesDesp(id) {
    $("#MeuModal").load("/Despesa/Details/" + id, function () {
        $('#myModal').modal("show");
    });
};

function DeletarDesp(id) {
    $("#MeuModal").load("/Despesa/Delete/" + id, function () {
        $('#myModal').modal("show");
    });
};
//quando seleciona o tipo  da despesa
function TipoDespesaSelector() {
    var id = $('#TipoDespesaId').val();
    var model = { "id": id }
    console.log("Seleciona" + id);
    var valido = true;
    //if (model == null) { valido = false; }
    if (id !== '' && id !== null) {
        $("#Descricao").empty();
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/Despesa/GetDescricaoSelect/",
            data: model,
            success: function (result) {
                if (result.Outros == true) {
                    console.log("remove")
                    $('#Mymodal, .DescricaoDiv').show();
                    $('#Descricao').val('');
                    $('#Mymodal, .DescricaoTeste').html('Descrição:');
                } else if (result.Veiculo == true) {
                    console.log("Veiculo")
                    $('#Mymodal, .DescricaoDiv').show();
                    $('#Descricao').val('');
                    $('#Mymodal, .DescricaoTeste').html('Quilometragem:');
                    $('#Descricao').after('<a class="glyphicon glyphicon-question-sign" data-toggle="tooltip" title="Até 200 km é 0.85 R$ / Litro</br>De 202 Km Até 499 Km é 0.75 R$</br>De 500 Km Até 1.999 Km é 0.65 R$</br> De 2.000 maior é 0.60 R$"></a>');
                    var veiculo = $('#DescricaoTeste').val();
                    console.log(veiculo)
                }
                else {
                    $('#Mymodal, .DescricaoDiv').hide();
                    $('#Descricao').val('N/A');
                }
            }
        });

    }
    else {
        $('#DescricaoOutros').hide();
    }
};
function LimparDespesa() {
    $('#result, .alert').html('');
    $('#ImageFile').val('');
    $("#TipoDespesaId").val('');
    $("#Descricao").val('');
    $("#Observacao").val('');
    $("#Valor").val('');
};