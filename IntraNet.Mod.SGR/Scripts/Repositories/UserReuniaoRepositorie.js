


/*----------------------Gravar Usuários da reuniao---------------------------------------------------*/
function GravarUser(Id) {
    //moderador
    var listarrayM = new Array();
    var moderador = document.getElementById('UserMod');
    for (var x = 0; x < moderador.options.length; x++) {
        listarrayM.push(moderador.options[x].text);
    }
    //usuarios
    var listarrayU = new Array();
    var usuario = document.getElementById('UserName');
    for (var i = 0; i < usuario.options.length; i++) {
        listarrayU.push(usuario.options[i].text);
    }
    //listarrayU = JSON.stringify(listarrayU);
    //listarrayM = JSON.stringify(listarrayM);
    console.log(listarrayU);
    console.log(listarrayM);
    if (listarrayU != null && listarrayM != null && listarrayU != "" && listarrayM != "") {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: "/UserReuniao/CreateUser",
            traditional: true,
            data: {
                'listarrayU': listarrayU,
                'listarrayM': listarrayM,
                'ReuniaoId': Id
            },
            success: function (data) {
                alert(data.message);

            }
        });
    } else {
        alert("Erro ao tentar gravar, verifique se a reunião possui moderadores e participantes.");
    }
}
