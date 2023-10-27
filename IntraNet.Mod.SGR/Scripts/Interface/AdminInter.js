$(document).ready(function () {
    console.log("load");
    
    $('#jqGridReuniao').jqGrid('navGrid', '#jqControlsReuniao', { cloneToTop: true, del: false, add: false, edit: false, search: false }, {}, {}, {},
        {
            multipleSearch: true,
            showQuery: true,
            multipleGroup: true,
            caption: "Advanced Search",
            Find: "Apply",
            Reset: "Reset & Close",
            closeAfterReset: true,
            closeAfterSearch: false,
            searchOnEnter: true,

            recreateForm: true,
            recreateFilter: true,
            errorcheck: true,
            overlay: false
        })
        .navButtonAdd('#jqControlsReuniao', {
            caption: "Visualizar",
            toppager: true,
            cloneToTop: true,
            buttonicon: "fas fa-copy",
            onClickButton: function () {
                alert("Adding Row");
            },
            position: "last"
        })
        .navButtonAdd('#jqControlsReuniao', {
            caption: "Editar",
            toppager: true,
            cloneToTop: true,
            buttonicon: "fa fa-lg fa-fw fa-pencil",
            onClickButton: function () {
                alert("Adding Row");
            },
            position: "last"
        })
        .navButtonAdd('#jqControlsReuniao', {
            caption: "Criar",
            toppager: true,
            cloneToTop: true,
            buttonicon: "fas fa-plus",
            onClickButton: function () {
                alert("Adding Row");
            },
            position: "last"
        })
        .navButtonAdd('#jqControlsReuniao', {
            caption: "Abrir",
            toppager: true,
            cloneToTop: true,
            buttonicon: "fa-pencil-square-o",
            onClickButton: function () {
                if ($("#ReuniaoId").val() > 0 && $("#ReuniaoId").val() != null) {
                    window.location.replace('/Reuniao/Ata/' + $("#ReuniaoId").val() + ' ');
                    return [true];
                }
                else {
                    alert('Você deve selecionar a reunião que deseja abrir')
                }
            },
            position: "last"
        });
    $('#jqGridReuniao').jqGrid('filterToolbar', {
        stringResult: true,
        searchOnEnter: true,
        searchOperators: true,
        defaultSearch: "cn",
        beforeClear: function () {
            alert("beforeClear");
        },
        beforeSearch: function () {
            alert("beforeSearch");
        }
    });
    //recarregar para funfar os filtros    
});
/*-----------------------------------LIST BOX FUNCOES-------------------------------------------------*/
/*-----------------------------------itemreuniao------------------------------------------------------*/
function AddItem() {
    $("#GrupoId option:selected").appendTo("#ItemReuniaos");
    $("#ItemReuniaos option").attr("selected", false);
}
function RemoveItem() {
    $("#ItemReuniaos option:selected").appendTo("#GrupoId");
    $("#GrupoId option").attr("selected", false);
}
function AddAll() {
    $("#ItemReuniaos option").appendTo("#GrupoId");
    $("#GrupoId option").attr("selected", false);
}
function RemoveAll() {
    $("#GrupoId option").appendTo("#ItemReuniaos");
    $("#ItemReuniaos option").attr("selected", false);
}
function Alterar() {
    var id = $("#ReuniaoId").val();
    $("#MeuModal").load("/Dados/Edit/" + id, function () {
        $('#myModal').modal("show");        
    });
};
/*----------------------------------------UserReuniao------------------------------------*/
/*-----------------------------------LIST BOX BOTOES FUNCOES-------------------------------------------------*/
function AddUser() {
    console.log("Chama funcao adicionar usuario p")
    $("#UserId option:selected").appendTo("#UserName");
    $("#UserName option").attr("selected", false);
}
function RemoveUser() {
    $("#UserName option:selected").appendTo("#UserId");
    $("#UserName option").attr("selected", false);
}
function RemoveAllUser() {
    $("#UserName option").appendTo("#UserId");
    $("#IdUser option").attr("selected", false);
}
function AddMod() {
    $("#UserId option:selected").appendTo("#UserMod");
    $("#UserMod option").attr("selected", false);
}
function RemoveMod() {
    $("#UserMod option:selected").appendTo("#UserId");
    $("#UserMod option").attr("selected", false);
}
function RemoveAllMod() {
    $("#UserMod option").appendTo("#UserId");
    $("#UserId option").attr("selected", false);
}
/*-------------------------------------------------------------*/
function CreateReuniao() {
    $("#MeuModal").load("/Reuniao/CreateReuniao/", function () {
        $('#myModal').modal("show");
        //$('#AlertReuniao').hide();
        console.log("load");
    });
};
function GerenciarUser(Id) {
    
    $("#MeuModal").load("/UserReuniao/Create/"+Id, function () {
        $('#myModal').modal("show");
        $('#AlertReuniao').hide();
        console.log("load");
    });
};

