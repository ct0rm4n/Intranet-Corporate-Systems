var btnAlterar;
$(function () {
    $("#jqGridEmpresa").jqGrid({
        url: "/Empresa/GetEmpresa",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['Codigo', '<b>Razao Social:</b>','Complemento','CodSiga','Ativo'],
        colModel: [
            { key: true, hidden: true, name: 'EmpresaId', index: 'EmpresaId', editable: false },            
            { key: false, name: 'RazaoSocial', index: 'RazaoSocial', editable: true },
            { key: false, name: 'Complemento', index: 'Complemento', editable: true },
            { key: false, name: 'CodSiga', index: 'CodSiga', editable: true, width: 29 },
            {
                key: false, name: 'Ativo', index: 'Ativo', edittype: 'checkbox',editable: true,editoptions: { value: "true:false" },
                editrules: { required: true },
                formatter: "checkbox",
                width: 29
            }
        ],
        pager: jQuery('#jqControlsEmpresa'),
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50],
        width: 1000,
        height: 200,
        resizable: true,
        viewrecords: true,
        caption: 'Empresas:',
        emptyrecords: 'Não há empresas disponiveis',
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
            btnAlterar = $("#jqGridEmpresa").jqGrid('getCell', rowId, 'EmpresaId');
            $("#AlterarEmp").val(btnAlterar);
            console.log(btnAlterar);
        },

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
    })
    .navGrid('#jqControlsEmpresa', { edit: true, add: true, del: true, refresh: true, search: true, searchtext: "Pesquisar" },
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
    $("#jqGridUnidade").jqGrid({
        url: "/Unidades/GetUnidade",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['Codigo', '<b>Nome da Unidade:</b>','Empresa','RazaoSocial','Obs'],
        colModel: [
            { key: true, hidden: true, name: 'UnidadeId', index: 'UnidadeId', editable: false },
            { key: false, name: 'Nome', index: 'Nome', editable: true },
            { key: false, name: 'EmpresaId', index: 'EmpresaId', editable: true },
            { key: false, name: 'RazaoSocial', index: 'RazaoSocial', editable: true },
            { key: false, name: 'Observacao', index: 'Observacao', editable: true }
        ],
        pager: jQuery('#jqControlsUnidade'),
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50],
        width: 1000,
        height: 200,
        resizable: true,
        viewrecords: true,
        caption: 'Unidades:',
        emptyrecords: 'Não há unidades disponiveis',
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
            $("#jqGridUnidade").jqGrid('setGridState', 'hidden');
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
        colNames: ['Codigo', '<b>Nome do depto:</b>','Obs'],
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

//EMPRESA CCUSTO
$(function () {
    $("#jqGridEmpresaCC").jqGrid({
        url: "/EmpCCusto/GetEmpCC",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['<b>Codigo</b>', 'C.Custo', 'C.C Desc', 'Projeto','Prospect', '<b>EmpresaId:</b>', 'RazaoSocial', '<b>Classe</b>', 'Unidade', 'Item', 'Ativo'],
        colModel: [
            { key: true, hidden: true, name: 'EmpCCustoId', index: 'EmpCCustoId', editable: false },
            { key: false, name: 'CCusto', index: 'CCusto', editable: true, width: 50 },
            { key: false, name: 'CCustoDesc', index: 'CCustoDesc', editable: true },
            {
                key: false, name: 'Projeto', index: 'Projeto', edittype: 'checkbox', editable: true, editoptions: { value: "true:false" },
                editrules: { required: true },
                formatter: "checkbox",
                width: 45
            },
            {
                key: false, name: 'Prospect', index: 'Prospect', edittype: 'checkbox', editable: true, editoptions: { value: "true:false" },
                editrules: { required: true },
                formatter: "checkbox",
                width: 45
            },
            { key: false, hidden: true, name: 'EmpresaId', index: 'EmpresaId', editable: true },
            { key: false, hidden: false, name: 'RazaoSocial', index: 'RazaoSocial', editable: true, width: 60 },
            { key: false, hidden: true, name: 'Classe', index: 'Classe', editable: true },
            { key: false, name: 'Nome', index: 'Nome', editable: true, width: 70 },
            { key: false, name: 'Item', index: 'Item', editable: true, width: 45 },
            { key: false, name: 'Ativo', index: 'Ativo', edittype: 'checkbox',editable: true, editoptions: { value: "true:false" } ,
                editrules: { required: true },
                formatter: "checkbox",
                width: 25 }
        ],
        pager: jQuery('#jqControlsEmpresaCC'),
        rowNum: 30,
        rowList: [10, 20, 30, 40, 50],
        width: 1000,
        height: 200,
        resizable: true,
        viewrecords: true,
        caption: 'Centro de Custos:',
        emptyrecords: 'Não há cc disponiveis',
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
            //$("#jqGridEmpresaCC").jqGrid('setGridState', 'hidden');
            //or 'hidden' 
        },
        multiselect: false,
        subGrid: false
    }).navGrid('#jqControlsEmpresaCC', { edit: true, add: false, del: true, refresh: true, search: true, searchtext: "Pesquisar" },
        {
            zIndex: 100,
            url: '/EmpCCusto/Edit',
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
            url: "/EmpCCusto/Create",
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
            url: "/EmpCCusto/Delete",
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
