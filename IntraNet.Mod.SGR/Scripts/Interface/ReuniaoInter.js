$(document).ready(function() {
    $('#UserId').select2({
        placeholder: "Selecione o funcionario...",
        minimumInputLength: 1,

        closeOnSelect: true,
        escapeMarkup: function (markup) {
            return markup;
        },
        templateResult: function (data) {
            return data.html;
        },
        templateSelection: function (data) {
            
            return data.text;
        },
        formatNoMatches: function () { return "Pesquisa não encontrada"; }, formatInputTooShort: function (input, min) { return "Digite " + (min - input.length) + " caracteres para pesquisar"; },
        multiple: true,
        ajax: {
            url: "/Demanda/PesquisaAutocompleteUser/",
            dataType: 'json',
            data: function (params) {
                console.log(params.term)
                
                return {
                    term: params.term
                };
            },
            processResults: function (data) {
                debugger;
                return {
                    results: $.map(data.autotexto,
                        function (usr) {
                            console.log(usr);
                            return {
                                id: usr.UserName,
                                text: usr.UserName,
                                html: '<div><img src="' +
                                    usr.Img +
                                    '" id="AvatarUserForm" style="width: 30px;height: 30px;-webkit-border-radius: 60px; border-radius: 60px;border: 10px;"></img><span>&nbsp;' +
                                    usr.UserName +
                                    '</span></div><div style="color:blue">' +
                                    usr.Email +
                                    '</div><div><small>' +
                                    usr.Setor +
                                    '</small></div>',
                                title: usr.Email
                            };
                        })
                }
            },
            cache: false
        }
    });
    
})
function InserirReuniaoInter() {
    $("#MeuModal").load("/Reuniao/Create",
        function () {
            $('#myModal').modal("show");
        });
    return [true];
}
function chamateste() {
    alert("teste");
}
function EditarReuniaoInter(id) {
    debugger;
    $("#loading").show();
    $("#MeuModal").load("/Reuniao/Edit/" + id,
        function () {
            $("#loading").hide();
            $('#myModal').modal("show");
        });
    return [true];

}
function ExibirResumo() {
    debugger;
    $("#loading").show();
    $("#ResumoPanel").css('display', 'block')
    $("#loading").hide();
}

//gerir usuários - tabela criar reuniao ou editar
function addRow(tableID) {
    // Get a reference to the table
    $("#loading").show();
    let tableRef = document.getElementById(tableID);
    var nome;
    // Insert a row at the end of the table
    let newRow = tableRef.insertRow(-1);
    let usuarioslista = $("#UserId").val();
    console.log(usuarioslista)
    debugger
    if (usuarioslista) {
        for (var i = 0; i < usuarioslista.length; i++) {
            
            var nomeU = usuarioslista[i].toString().replace('.', '');
            var string = "<tr id='" + nomeU + "'><td >" + usuarioslista[i] + "</td><td>" + $("#Acesso").val() + "</td> <td><i onclick='RemoveRow(\"" + nomeU + "\",\"" + $("#Acesso").val() + "\")' class='fa fa-fw fa-trash-o'></i></td></tr>";
            if ($("#Acesso").val() == "Moderador") {
                //caso tenha acesso remove do listbox hidden
                $("#ListaModerador option[value='" + usuarioslista[i] + "']").remove();
                $("#ListaParticipantes option[value='" + usuarioslista[i] + "']").remove();
                RemoveRow(nomeU, "Participante")//remove da grid
                RemoveRow(nomeU, "Moderador")
                $("#UserId option[value='" + usuarioslista[i] + "']").remove();//input do adiconar 
                //adiciona a string concatenada a grid
                $('#ListaParticipantesT').append(string)
                //adiciona ao listBox Hidden Moderador
                var option = $("<option />").val(usuarioslista[i]).text(usuarioslista[i]);
                $("#ListaModerador").append(option)
            }
            else if ($("#Acesso").val() == "Participante") {
                //caso tenha acesso remove do listbox
                $("#ListaModerador option[value='" + usuarioslista[i] + "']").remove();
                $("#ListaParticipantes option[value='" + usuarioslista[i] + "']").remove();
                RemoveRow(nomeU, "Participante")//remove da grid
                RemoveRow(nomeU, "Moderador")
                $("#UserId option[value='" + usuarioslista[i] + "']").remove();
                //adiciona usuário a grid
                $('#ListaParticipantesT').append(string)
                var option = $("<option />").val(usuarioslista[i]).text(usuarioslista[i]);
                $("#ListaParticipantes").append(option)
            }
            else {
                $('#result').html("<div class='alert alert-danger col-lg-10' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro - não é possivel adicionar sem especificar o acesso</span></br></div>");
            }
        }
    }else {
         $('#result').html("<div class='alert alert-danger col-lg-10' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro - adicione os participante</span></br></div>");
    }
    $("#Acesso").val('');
    console.log("Adicionado a model de moderador" + $("#ListaParticipantes").val())
    console.log("Adicionado a model de moderador" + $("#ListaModerador").val())
    $("#loading").hide();
}
function RemoveRow(nome,acesso) {
    $("#loading").show();    
    
    if (acesso= "Moderador") {
        $("#ListaModerador option[value='" + nome + "']").remove();
        console.log("Usuário removido da lista de moderador" + nome)        
    } else {
        $("#ListaParticipantes option[value='" + nome + "']").remove();
        console.log("Usuário removido da lista de participante" + nome)
    }
    $("table#ListaParticipantesT #" + nome).remove();
    $("table#ListaParticipantesT tr#" + nome).closest('tr').remove();
    $("table#ListaParticipantesT tr").each(function () {
        
        var tableData = $(this).find('td');
        
        //alert(tableData)
    })

    //alert("Usuário a remover:" + nome + "ACesso dele é:" +acesso )
    debugger;
    $("#loading").hide();
}
//alert botão excluir reuiniao
function RemoverReuniaoInter() {
    //console.log(model);
    debugger;
    var ID = $("#ReuniaoId").val();
    if (ID) {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal")
            .html(
                "<h2 align='center'><i class='fa fa-lg fa-fw fa-trash-o'>&nbsp;</i><strong>Remover a reuniao </strong></h2><hr><div class='container' align='center'><div class='alert alert-danger col-lg-12' align='center'>" +
                "Tem certeza que deseja remover a reunião: &nbsp;" +
                $("#Nome").val() + "." +
                "</div ></div><div align='center' class='row col-lg-12'><button class='btn btnDefault btnReuniao' onclick='RemoverReuniao()'><i class='fa fa-fw fa-trash-o'></i>&nbsp;Confirmar&nbsp;</button> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'><i class='fa fa-lg fa-fw fa-ban'></i>&nbsp;Cancelar&nbsp;</button></div>");

        $("#loading").hide();
        $('#myModal').modal('show');
    } else {
        $('#myModal').modal('hide');
        $("#loading").show();
        $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + "Selecione a reunião" + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
        $("#loading").hide();
        $('#myModal').modal('show');
    }

}
