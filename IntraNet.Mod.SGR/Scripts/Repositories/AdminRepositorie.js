$(function () {
    var userid = "";
    var username = "";
    $("#jqGridAdminUser").jqGrid({
        url: "/User/GetUser",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['Id', 'Matricula', 'Usuário', 'Nome', '<i class="fa fa-window-restore">&nbsp</i>Unidade', 'Setor', '<i class="fa fa-briefcase">&nbsp;</i>Setor', 'Endereço', '<i class="fa fa-phone-square">&nbsp;</i>Telefones', 'Email','Role'],
        colModel: [
            { key: true, hidden: true, name: 'Id', index: 'Id', editable: false, search: false },
            { key: false, name: 'Matricula', index: 'Matricula', editable: true, width: 50 },
            { key: false, name: 'UserName', index: 'UserName', editable: true, width: 80 },
            { key: false, name: 'FullName', index: 'FullName', editable: true, width: 140 },
            {
                key: false, hidden: false, name: 'Unidade', index: 'Unidade', editable: false, width: 85,
                formatter: 'select',
                edittype: 'select',
                stype: 'select',
                searchoptions: {
                    value: ':--Todos--;Vamtec Minas:Vamtec Minas;Vamtec Vitoria:Vamtec Vitoria;Vamtec Haztec:Vamtec Haztec;Vamfertil:Vamfertil;FiveStar:FiveStar;Vamtec Rio:Vamtec Rio'
                },
                editoptions: {
                    value: {
                        'Em andamento': 'Em andamento',
                        'Concluido': 'Concluido'
                    },
                    formatter: 'select'
                }
            },
            { key: false, hidden: true, name: 'Setor', index: 'Setor', editable: true },
            { key: false, name: 'SetorNome', index: 'SetorNome', editable: true, width: 140 },
            { key: false, name: 'Endereco', index: 'Endereco', editable: false,hidden: true },
            { key: false, name: 'PhoneNumber', index: 'PhoneNumber', editable: true, width: 100},
            { key: false, name: 'Email', index: 'Email', editable: true },
            { key: false, name: 'Roles', index: 'Roles', editable: true, width: 50 },
           
        ],
        pager: jQuery('#jqControlsAdminUser'),
        rowNum: 1000,
        rowList: [10, 20, 30, 40, 50, 1000],
        autowidth: true,
        height: 300,
        iconSet: "fontAwesome",
        resizable: true,
        toppager: false,
        viewrecords: true,
        caption: '<i class="fa fa-clipboard">&nbsp;</i>Controle &nbsp;<i class="fa fa-caret-right"></i>&nbsp; Usuários&nbsp;<i class="fa fa-id-card"></i>',
        emptyrecords: 'Não há reuniões cadastradas',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        loadonce: true, //Pesquisa
        forceClientSorting: true,
        multiselect: false,
        onSelectRow: function () {
            //pega o id da subgrid
            var rowId = $("#jqGridAdminUser").jqGrid('getGridParam', 'selrow');
            //pega o id da subgrid
            userid = $("#jqGridAdminUser").jqGrid('getCell', rowId, 'Id');
            username = $("#jqGridAdminUser").jqGrid('getCell', rowId, 'UserName');
            console.log("Usuário selecionado id: " + userid);
            $("#UserId").val(userid);
            $("#UserName").val(username)
        },
        ondblClickRow: function (rowid) {
            
        },
        subGrid: false,
        scrollOffset: 18,
        actionsNavOptions: {
            edittitle: "Alterar assunto",
            deltitle: "Remover o assunto",
            savetitle: "Confirmar alteração",
            canceltitle: "Cancelar edição",
        },
        gridComplete: function () {
            $('#jqGridAdminUser').jqGrid('filterToolbar', {
                stringResult: true,
                searchOnEnter: true,
                searchOperators: true,
                defaultSearch: "cn",
                beforeClear: function () {
                    //alert("beforeClear");
                },
                beforeSearch: function () {
                    //alert("beforeSearch");
                }
            });
        },
        loadComplete: function (data) {
            
        }
    })
});
$(function () {
    $("#jqGridEmpresa").jqGrid({
        url: "/PainelSGR/GetEmpresa",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['Codigo', '<b>Razao Social:</b>', 'Complemento', 'CodSiga', 'Ativo'],
        colModel: [
            { key: true, hidden: true, name: 'EmpresaId', index: 'EmpresaId', editable: false },
            { key: false, name: 'RazaoSocial', index: 'RazaoSocial', editable: true },
            { key: false, name: 'Complemento', index: 'Complemento', editable: true },
            { key: false, name: 'CodSiga', index: 'CodSiga', editable: true, width: 29 },
            {
                key: false, name: 'Ativo', index: 'Ativo', edittype: 'checkbox', editable: true, editoptions: { value: "true:false" },
                editrules: { required: true },
                formatter: "checkbox",
                width: 29
            }
        ],
        pager: jQuery('#jqControlsEmpresa'),
        rowNum: 20,
        rowList: [10, 20, 30, 40, 50, 1000],
        autowidth: true,
        height: 300,
        iconSet: "fontAwesome",
        resizable: true,
        toppager: false,
        viewrecords: true,
        caption: '<i class="fa fa-clipboard">&nbsp;</i>Controle &nbsp;<i class="fa fa-caret-right"></i>&nbsp; Empresas&nbsp;<i class="fa fa-window-maximize"></i>',
        emptyrecords: 'Não há Empresas cadastradas',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        loadonce: true,//Pesquisa
        multiselect: false,
        onSelectRow: function () {
            //pega o id da subgrid
            var rowId = $("#jqGridEmpresa").jqGrid('getGridParam', 'selrow');
            //pega o id da subgrid
            var empresaid = $("#jqGridEmpresa").jqGrid('getCell', rowId, 'EmpresaId');
            $("#EmpresaId").val(empresaid);
            var razao = $("#jqGridEmpresa").jqGrid('getCell', rowId, 'RazaoSocial');
            $("#RazaoSocial").val(razao);

            console.log("A empresa selecionada é" + empresaid+", Razao Social:"+razao);
        },
        subGrid: false,
        scrollOffset: 18,
        //subGridUrl: '/Reuniaos/GetReuniao',
        subGridRowExpanded: function (subgrid_id, row_id) {
            // we pass two parameters
            // subgrid_id is a id of the div tag created within a table
            // the row_id is the id of the row
            // If we want to pass additional parameters to the url we can use
            // the method getRowData(row_id) - which returns associative array in type name-value
            // here we can easy construct the following
            var subgrid_table_id, pager_id;
            subgrid_table_id = subgrid_id + "_t";
            pager_id = "p_" + subgrid_table_id;
            jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");
            jQuery("#" + subgrid_table_id).jqGrid({
                url: "/SetorEmp/GetSetorEmpSubgrid/" + row_id,
                //url: "/Reuniaos/ReunioesSubGrid/" + row_id,
                datatype: 'json',
                mtype: 'POST',
                colNames: ['SetorEmpId', 'SetorId', 'SetorDesc', 'EmpresaId'],
                colModel: [
                    { name: 'SetorEmpId', hidden: true, index: 'SetorEmpId', editable: false, key: true },
                    { key: false, name: 'SetorId', hidden: true, index: 'SetorId', editable: true },
                    { key: false, name: 'SetorDesc', hidden: false, index: 'SetorDesc', editable: false },
                    { key: false, name: 'EmpresaId', hidden: true, index: 'EmpresaId', editable: true }
                ],
                rowNum: 10,
                rowList: [10, 20, 30, 40, 50],
                width: 900,
                sortname: 'Id',
                sortorder: 'asc',
                height: 'auto',
                viewrecords: true,
                caption: 'Setor | Empresa:',
                emptyrecords: 'Não há setor disponiveis para essa empresa',
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                    Id: "0"
                },
            });
        }
    }).navGrid('#jqControlsEmpresa', { edit: false, add: false, del: false, refresh: false, search: false, searchtext: "Pesquisar" },
            {
                zIndex: 100,
                url: '/Tema/Edit',
                closeOnEscape: true,
                closeAfterEdit: true,
                recreateForm: true,
                afterComplete: function (response) {
                    if (response.responseText) {
                        alert(response.responseText);
                    }
                },
                afterSubmit: function () {
                    $(this).jqGrid("setGridParam", { datatype: 'json' });
                    return [true];
                }
            },
            {
                zIndex: 100,
                url: "/Tema/Create",
                closeOnEscape: true,
                closeAfterAdd: true,
                recreateForm: true,
                afterComplete: function (response) {
                    if (response.responseText) {
                        alert(response.responseText);
                    }
                },
                //funcao que faz atualização da tabela após criar, lembrando que só é necessario quando loadonce: true
                afterSubmit: function () {
                    $(this).jqGrid("setGridParam", { datatype: 'json' });
                    return [true];
                }
            },
            {
                zIndex: 100,
                url: "/Tema/Delete",
                closeOnEscape: true,
                closeAfterDelete: true,
                recreateForm: true,
                msg: "Are you sure you want to delete Tema... ? ",
                afterComplete: function (response) {
                    if (response.responseText) {
                        alert(response.responseText);
                    }
                }
        }).navButtonAdd('#jqControlsEmpresa',
        {
            caption: "Criar Empresa",
            toppager: false,
            cloneToTop: false,
            buttonicon: "fa fa-lg fa-fw fa-plus",
            onClickButton: function () {
                InsereEmpresaInter()
            },
            position: "last"
        }).navButtonAdd('#jqControlsEmpresa',
        {
            caption: "Alterar",
            toppager: false,
            cloneToTop: false,
            buttonicon: "fa fa-lg fa-fw fa-pencil",
            onClickButton: function () {
                var rowKey = jQuery("#jqGridUserAdmin").jqGrid('getGridParam', 'selrow');
                var rowObject = jQuery('#jqGridUserAdmin').getRowData(rowKey);
                console.log(rowObject)
                AlterareEmpresaInter()
            },
            position: "last"
        }).navButtonAdd('#jqControlsEmpresa',
        {
            caption: "Remover",
            toppager: false,
            cloneToTop: false,
            buttonicon: "fa fa-fw fa-trash-o",
            onClickButton: function () {
                RemoverEmpresaInter()
            },
            position: "last"
        });
});

$(function () {
    $("#jqGridUnidade").jqGrid({
        url: "/PainelSGR/GetUnidade",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['Codigo', '<b>Nome da Unidade:</b>', 'Empresa', 'Empresa','Cnpj','Estado','Endereco', 'Obs'],
        colModel: [
            { key: true, hidden: true, name: 'UnidadeId', index: 'UnidadeId', editable: false },
            { key: false, name: 'Nome', index: 'Nome', editable: true },
            { key: false, name: 'EmpresaId', index: 'EmpresaId', editable: true },
            { key: false, name: 'Empresa', index: 'Empresa', editable: true },
            { key: false, name: 'Cnpj', index: 'Cnpj', editable: true },
            { key: false, name: 'Estado', index: 'Estado', editable: true },
            { key: false, name: 'Endereco', index: 'Endereco', editable: true },
            { key: false, name: 'Observacao', index: 'Observacao', editable: true }
        ],
        pager: jQuery('#jqControlsUnidade'),
        rowNum: 20,
        rowList: [10, 20, 30, 40, 50, 1000],
        autowidth: true,
        height: 300,
        iconSet: "fontAwesome",
        resizable: true,
        toppager: false,
        viewrecords: true,
        caption: '<i class="fa fa-clipboard">&nbsp;</i>Controle &nbsp;<i class="fa fa-caret-right"></i>&nbsp; Unidades&nbsp;<i class="fa fa-window-maximize"></i>',
        emptyrecords: 'Não há reuniões cadastradas',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        loadonce: true,//Pesquisa
        gridComplete: function () {
            
        },
        multiselect: false,
        scrollOffset: 18,
        grouping: true,
        groupingView: {
            groupField: ['Empresa'],
            groupSummary: [true],
            groupColumnShow: [false],
            groupText: ['<b>{0} - {1} Item(s)</b>'],
            groupCollapse: false,
            groupOrder: ['asc']
        }
    }).navGrid('#jqControlsUnidade', { edit: true, add: true, del: true, refresh: true, search: true, searchtext: "Pesquisar" },
        {
            zIndex: 100,
            url: '/Tema/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            },
            afterSubmit: function () {
                $(this).jqGrid("setGridParam", { datatype: 'json' });
                return [true];
            }
        },
        {
            zIndex: 100,
            url: "/Tema/Create",
            closeOnEscape: true,
            closeAfterAdd: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            },
            //funcao que faz atualização da tabela após criar, lembrando que só é necessario quando loadonce: true
            afterSubmit: function () {
                $(this).jqGrid("setGridParam", { datatype: 'json' });
                return [true];
            }
        },
        {
            zIndex: 100,
            url: "/Tema/Delete",
            closeOnEscape: true,
            closeAfterDelete: true,
            recreateForm: true,
            msg: "Are you sure you want to delete Tema... ? ",
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        });
});

$(function () {
    $("#jqGridSetor").jqGrid({
        url: "/Setor/GetSetor",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['Codigo', '<b>Nome do depto:</b>', 'Obs'],
        colModel: [
            { key: true, hidden: true, name: 'DepartamentoId', index: 'DepartamentoId', editable: false },
            { key: false, name: 'Nome', index: 'Nome', editable: true },
            { key: false, name: 'Observacao', index: 'Observacao', editable: true }
        ],
        pager: jQuery('#jqControlsSetor'),
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50],
        width: 1000,
        height: 200,
        resizable: true,
        viewrecords: true,
        caption: 'Setores:',
        emptyrecords: 'Não há Setores disponiveis',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        loadonce: true,//Pesquisa
        gridComplete: function () {
            $("#jqGridSetor").jqGrid('setGridState', 'hidden');
            //or 'hidden' 
        },
        multiselect: false,
        subGrid: true,
        scrollOffset: 18,
        //subGridUrl: '/Reuniaos/GetReuniao',
        subGridRowExpanded: function (subgrid_id, row_id) {
            // we pass two parameters
            // subgrid_id is a id of the div tag created within a table
            // the row_id is the id of the row
            // If we want to pass additional parameters to the url we can use
            // the method getRowData(row_id) - which returns associative array in type name-value
            // here we can easy construct the following
            var subgrid_table_id, pager_id;
            subgrid_table_id = subgrid_id + "_t";
            pager_id = "p_" + subgrid_table_id;
            jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");
            jQuery("#" + subgrid_table_id).jqGrid({
                url: "/Reuniaos/ReunioesSubGrid/" + row_id,
                //url: "/Reuniaos/ReunioesSubGrid/" + row_id,
                datatype: 'json',
                mtype: 'POST',
                colNames: ['Id', 'IdTema', 'Nome', 'Observac'],
                colModel: [
                    { name: 'Id', hidden: true, index: 'Id', editable: false, key: true },
                    { key: false, name: 'IdTema', hidden: true, index: 'IdTema', editable: true },
                    { key: false, name: 'Nome', index: 'Nome', editable: true },
                    { key: false, name: 'Observac', index: 'Observac', editable: true }
                ],
                rowNum: 10,
                rowList: [10, 20, 30, 40, 50],
                width: 900,
                sortname: 'Id',
                sortorder: 'asc',
                height: 'auto',
                viewrecords: true,
                caption: 'Reuniões do tema:',
                emptyrecords: 'Não há reuniões disponiveis para esse tema',
                jsonReader: {
                    root: "rows",
                    page: "page",
                    total: "total",
                    records: "records",
                    repeatitems: false,
                    Id: "0"
                }

            });
        }
    }).navGrid('#jqControlsDepartamento', { edit: true, add: true, del: true, refresh: true, search: true, searchtext: "Pesquisar" },
        {
            zIndex: 100,
            url: '/Tema/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            },
            afterSubmit: function () {
                $(this).jqGrid("setGridParam", { datatype: 'json' });
                return [true];
            }
        },
        {
            zIndex: 100,
            url: "/Tema/Create",
            closeOnEscape: true,
            closeAfterAdd: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            },
            //funcao que faz atualização da tabela após criar, lembrando que só é necessario quando loadonce: true
            afterSubmit: function () {
                $(this).jqGrid("setGridParam", { datatype: 'json' });
                return [true];
            }
        },
        {
            zIndex: 100,
            url: "/Tema/Delete",
            closeOnEscape: true,
            closeAfterDelete: true,
            recreateForm: true,
            msg: "Are you sure you want to delete Tema... ? ",
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        });
});
$(function() {
    $('#jqGridAdminUser').jqGrid('navGrid', '#jqControlsAdminUser', {
        cloneToTop: false, add: false, edit: false, del: false, search: false
    }, {
            zIndex: 100,
            url: '/Assunto/Create',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (confirm("Deseja alterar o funcionário?")) {
                    var rowKey = jQuery("#jqGridUserAdmin").jqGrid('getGridParam', 'selrow');
                    var rowObject = jQuery('#jqGridUserAdmin').getRowData(rowKey);
                    console.log(rowObject)
                    InsereUserAdmin(rowKey)
                }
            },
            afterSubmit: function () {
                $(this).jqGrid("setGridParam", { datatype: 'json' });
                return [true];
            }
        }, {
            zIndex: 100,
            url: '/Assunto/Edit',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            },
            afterSubmit: function () {
                $(this).jqGrid("setGridParam", { datatype: 'json' });
                return [true];
            }
        },
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
        }).navButtonAdd('#jqControlsAdminUser',
            {
                caption: "Criar Usuário",
                toppager: false,
                cloneToTop: false,
                buttonicon: "fa fa-lg fa-fw fa-plus",
                onClickButton: function () {
                    InsereUserAdmin()
                },
                position: "last"
        }).navButtonAdd('#jqControlsAdminUser',
        {
            caption: "Alterar",
            toppager: false,
            cloneToTop: false,
            buttonicon: "fa fa-lg fa-fw fa-pencil",
            onClickButton: function () {
                var rowKey = jQuery("#jqGridUserAdmin").jqGrid('getGridParam', 'selrow');
                var rowObject = jQuery('#jqGridUserAdmin').getRowData(rowKey);
                console.log(rowObject)
                AlterarUserAdmin(rowKey)
            },
            position: "last"
        }).navButtonAdd('#jqControlsAdminUser',
        {
            caption: "Remover",
            toppager: false,
            cloneToTop: false,
            buttonicon: "fa fa-fw fa-trash-o",
            onClickButton: function () {
                RemoverUserAdmin()
            },
            position: "last"
        });
})
function InserirFuncionario() {
    var validor = false;

    var create = $("#CreateAdmin").serialize();
    
    var token = $('input[name=__RequestVerificationToken]').val();
    validor = true;
    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO  

    //debugger;
    console.log(create);
    console.log(validor);

    if (validor == true) {
        $.ajax({
            type: "POST",
            url: "/User/CreateAdmin/",
            data: create,
            success: function (result) {
                debugger;
                if (result.success == 1) {
                    debugger;
                    $('#result').html("<div class='alert alert-success col-lg-10' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><strong>Concluido</strong></br></br>" + result.message + "</div>");                   
                    setTimeout(function () { window.location.replace('/PainelSGR/UserAdminSGR'); }, 5000);
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
function AlterarFuncionario() {
    var validor = false;
    var userid = $("#UserId").val(); 
    var edit = $("#EditAdmin").serialize();
    edit.Id = userid;
    var token = $('input[name=__RequestVerificationToken]').val();
    validor = true;
    //VALIDAÇÃO DOS CAMPOS DO FORMULÁRIO  

    //debugger;
    console.log(edit);
    console.log(validor);

    if (validor == true) {
        $.ajax({
            type: "POST",
            url: "/User/EditAdmin/",
            data: edit,
            success: function (result) {
                debugger;
                if (result.success == 1) {
                    debugger;
                    $('#result').html("<div class='alert alert-success col-lg-10' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><strong>Concluido</strong></br></br>" + result.message + "</div>");
                    setTimeout(function () { window.location.replace('/PainelSGR/UserAdminSGR'); }, 5000);
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

function RemoverFuncionario() {
    //console.log(model);
    debugger;
    var ID = $("#UserId").val();
    $("#loading").show();
    debugger;
    $.ajax({
        type: "POST",
        url: "/User/DeleteConfirmed/" + ID,
        data: ID,
        success: function (result) {
            if (result.success == true) {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='fa fa-check-circle-o'>&nbsp;</i><strong>Concluido!</strong></h2><hr><div class='container' align='center'><div class='alert alert-success col-lg-10' align='center'>" + result.message + "</div ></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'><i class='fa fa-refresh'></i>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
                setTimeout(function () { window.location.replace('/PainelSGR/UserAdminSGR'); }, 3000);
            }
            else {
                debugger;
                $("#MeuModal").html("<h2 align='center'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro</span></h2><div class='container' align='center'><div class='alert alert-danger col-lg-10' align='center'>" + result.message + "</div></div><div align='center' class='row col-lg-12'> <button class='btn btnDefault btnReuniao' onclick='FecharModal()'>&nbsp;OK&nbsp;</button></div>");
                $("#loading").hide();
                $('#myModal').modal("show");
                //ReloadSubGrid(model.AssuntoId);
            }
        }
    });
}