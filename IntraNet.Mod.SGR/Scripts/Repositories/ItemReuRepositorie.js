function InserirItens(id) {
    // PEGA O ULTIMO ID DA REUNIAO CADASTRADA
    debugger;
    var listarray = new Array();
    var select = document.getElementById('ItemReuniaos');

    for (var i = 0; i < select.options.length; i++) {
        listarray.push(select.options[i].text);
    }
    debugger;
    var model = { 'listarray': listarray, "ReuniaoId": id }
    debugger;
    if (listarray != null) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/ItemReuniao/CreateItem",
            traditional: true,
            data: model,
            success: function (data) {
                alert(data.Message);
                console.log("Salvo!" + listarray);
                
            }
        });
    } else {
        alert("Verifique os campos obrigatórios, descrição e lista de agrupamentos.");
    }
}