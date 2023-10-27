function InserirReuniao() {
    var validor = false;
    var create = $("#Create").serialize();
    
    var token = $('input[name=__RequestVerificationToken]').val();
    validor = true;
    var Participantes = $("#ListaParticipantes").val();
    var Moderador = $('#ListaModerador').val();
    console.log(validor);
   // var myTableArray = [];
   // $("table#ListaParticipantesT tr").each(function () {
       // var arrayOfThisRow = [];
        //var tableData = $(this).find('td');
        //if (tableData.length > 0) {
            //tableData.each(function () { arrayOfThisRow.push($(this).text()); });
           // myTableArray.push(arrayOfThisRow);
        //}
    //});
    var selectParti = document.getElementById('ListaParticipantes');
    var ListaP ="";
    for (var i = 0; i < selectParti.options.length; i++) {
        //ListaParticipantes[i] = selectParti.options[i].text;
        ListaP = ListaP + '&ListaParticipantes=' + selectParti.options[i].text;
    }
    var selectMod = document.getElementById('ListaModerador');
    //var ListaModerador = [];
    var ListaM = "";
    for (var i = 0; i < selectMod.options.length; i++) {
        //ListaModerador[i] = selectMod.options[i].text;
        ListaM = ListaM + '&ListaModerador=' + selectMod.options[i].text;
    }
    create = create + ListaM + ListaP
    console.log("Create Serializado:" + create);
    if (validor == true) {
        $.ajax({
            type: "POST",
            url: "/Reuniao/Create/",
            data: create,
            traditional:true,
            success: function (result) {
                if (result.success == 1) {
                    debugger;
                    $('#result').html("<div class='alert alert-success col-lg-10' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><strong>Concluido</strong></br></br>" + result.message + "</div>");
                    setTimeout(function () {
                        window.location.replace('/Reuniao/Ata/' + result.ReuniaoId)
                    }, 50000);
                }
                else {
                    debugger;
                    $('#result').html("<div class='alert alert-danger col-lg-10' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro - Verifique o formulário</span></br></br>" + result.message + "</div>");
                }                
            }
        });
    } else {
        alert("Ocorreu um erro");
    }
}
function RemoverReuniao() {

    debugger;
    var ID = $("#ReuniaoId").val();
    //var IP = myIP();
    $("#loading").show();
    debugger;
    $.ajax({
        type: "POST",
        url: "/Reuniao/Delete/" + ID,
        data: ID,
        success: function (result) {
            debugger;
            if (result.success == true) {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='fa fa-check-circle-o'>&nbsp;</i><strong>Concluido </strong></h2><hr><div class='container' align='center'><div class='alert alert-success col-lg-10' align='center'>" + result.message + "</div ></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='$('#myModal').hide()'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
                setTimeout(function () {
                    $('#myModal').hide();
                }, 30000);
            }
            else {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + result.message + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='RedirecionaReuniao(" + result.ReuniaoId + ")'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");

            }
        }
    });
}


function EditarReuniao() {
    var validor = false;
    var editar = $("#Edit").serialize();
    editar = editar + "&ReuniaoId="+$("#ReuniaoId").val();
    var token = $('input[name=__RequestVerificationToken]').val();
    var Participantes = $("#ListaParticipantes").val();
    var Moderador = $('#ListaModerador').val();
    console.log(validor);
    // var myTableArray = [];
    // $("table#ListaParticipantesT tr").each(function () {
    // var arrayOfThisRow = [];
    //var tableData = $(this).find('td');
    //if (tableData.length > 0) {
    //tableData.each(function () { arrayOfThisRow.push($(this).text()); });
    // myTableArray.push(arrayOfThisRow);
    //}
    //});
    var selectParti = document.getElementById('ListaParticipantes');
    var ListaP = "";
    for (var i = 0; i < selectParti.options.length; i++) {
        //ListaParticipantes[i] = selectParti.options[i].text;
        ListaP = ListaP + '&ListaParticipantes=' + selectParti.options[i].text;
    }
    var selectMod = document.getElementById('ListaModerador');
    //var ListaModerador = [];
    var ListaM = "";
    for (var i = 0; i < selectMod.options.length; i++) {
        //ListaModerador[i] = selectMod.options[i].text;
        ListaM = ListaM + '&ListaModerador=' + selectMod.options[i].text;
    }
    editar = editar + ListaM + ListaP
    validor = true;
    console.log("Editar Serializado:" + editar);
    debugger;
    if (validor == true) {
        $.ajax({
            type: "POST",
            url: "/Reuniao/Edit/",
            data: editar,
            traditional: true,
            success: function (result) {
                if (result.success == 1) {
                    debugger;
                    $('#result').html("<div class='alert alert-success col-lg-10' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><strong>Concluido</strong></br></br>" + result.message + "</div>");
                    setTimeout(function () {
                        window.location.replace('/Reuniao/Ata/' + result.ReuniaoId)
                    }, 50000);
                }
                else {
                    debugger;
                    $('#result').html("<div class='alert alert-danger col-lg-10' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro - Verifique o formulário</span></br></br>" + result.message + "</div>");
                }
            }
        });
    } else {
        alert("Ocorreu um erro");
    }
}