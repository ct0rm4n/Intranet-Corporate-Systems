$(function () {
    $("#jqGridReuniao").jqGrid({
        url: "/Reuniao/GetReuniao",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['ReuniaoId', '<b>Nome da Reuniao:</b>', '<b>Tema</b>', 'Observacão','<i class="fa fa-users">&nbsp;</i>Participantes','Total Demandas', 'Ativo'],
        colModel: [
            { key: true, hidden: true, name: 'ReuniaoId', index: 'ReuniaoId', editable: false },
            { key: false, name: 'Nome', index: 'Nome', editable: true },
            {
                key: false, name: 'Tema', index: 'Tema', editable: true, width: 60,// stype defines the search type control - in this case HTML select (dropdownlist)
                stype: "select",
                // searchoptions value - name values pairs for the dropdown - they will appear as options

                searchoptions: {               
                    dataUrl: "/Tema/ListaTemas",
                    type: "GET",
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: false,
                    buildSelect: function (data) {
                        var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                        s += '<option value="">--Selecione--</option>';
                        console.log("Tamanho" + response.d.length)
                        console.log("json" + response.d)
                        if (response.d && response.d.length) {
                            for (i = 0, l = response.d.length; i < l; i += 1) {
                                ri = response.d[i];
                                s += '<option value="' + ri + '">' + ri + '</option>';
                            }
                        }
                        return s + '</select>';
                    }
                }
            },
            { key: false, hidden: true, name: 'Observac', index: 'Observac', editable: false, search: false },
            { key: false, hidden: false, name: 'Participantes', index: 'Participantes', editable: true, search: false },
            { key: false, nome: 'TotalDemandas', index: 'TotalDemandas', editable: true, width: 50, search: false},
            {
                key: false, name: 'Ativo', index: 'Ativo', edittype: 'checkbox', editable: true, editoptions: { value: "true:false" },
                editrules: { required: true },
                formatter: "checkbox",
                width: 29,
                search: false
            }
        ],
        pager: jQuery('#jqControlsReuniao'),
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50],
        width: 1200,
        height: 370,
        iconSet: "fontAwesome",
        resizable: true,
        toppager: true,
        viewrecords: true,
        caption: '<i class="fa fa-file-text-o">&nbsp;</i>Reuniões:',
        emptyrecords: 'Não há registro de reuniões para esse usuário',
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
            var rowId = $("#jqGridReuniao").jqGrid('getGridParam', 'selrow');
            //pega o id da subgrid
            btnAlterar = $("#jqGridReuniao").jqGrid('getCell', rowId, 'ReuniaoId');
            $("#ReuniaoId").val(btnAlterar);
            console.log(btnAlterar);
        },
        subGrid: false,
        scrollOffset: 18        
    })
});
$(function () {
    $("#jqGridReuniaoUser").jqGrid({
        url: "/Reuniao/GetReuniaoUser",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['Id', 'Situação', '<i class="fa fa-pencil-square-o">&nbsp;</i><b>Nome da Reuniao:</b>', '<b>Setor</b>', '<i class="fa fa-key">&nbsp;</i><b>Acesso</b>', 'Observacão', '<i class="fa fa-users">&nbsp;</i>Participantes', '<i class="fa fa-wrench">&nbsp;</i>Suas/<br/>Total Demandas'],
        colModel: [
            { key: true, hidden: false, name: 'ReuniaoId', index: 'ReuniaoId', editable: false, width: 20, search: false },
            {
                key: false, hidden: false, name: 'Situacao', index: 'Situacao', editable: false, width: 55,
                formatter: 'select',
                edittype: 'select',
                stype: 'select',
                searchoptions: {
                    value: ':--Todos--;Em andamento:Em andamento;Incompleta:Incompleta;Concluido:Concluido'
                },
                editoptions: {
                    value: {
                        'Em andamento': 'Em andamento',
                        'Concluido': 'Concluido'
                    },
                    formatter: 'select'
                }
            },
            { key: false, name: 'Nome', index: 'Nome', editable: true },
            {
                key: false, name: 'Tema', index: 'Tema', editable: true, width: 75,// stype defines the search type control - in this case HTML select (dropdownlist)
                stype: "select",
                // searchoptions value - name values pairs for the dropdown - they will appear as options

                searchoptions: {
                    dataUrl: "/Tema/ListaTemas",
                    type: "GET",
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: false,
                    buildSelect: function (data) {
                        var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                        s += '<option value="">--Todos--</option>';
                        console.log("Tamanho" + response.d.length)
                    
                        console.log("json" + response.d)
                        if (response.d && response.d.length) {
                            for (i = 0, l = response.d.length; i < l; i += 1) {
                                ri = response.d[i];
                                s += '<option value="' + ri + '">' + ri + '</option>';
                            }
                        }
                        return s + '</select>';
                    }
                }
            },
            {
                key: false, name: 'Acesso', index: 'Acesso',width:50, editable: true, search: true,
                stype: 'select',
                searchoptions: {
                    value: ':--Todos--;Moderador:Moderador;Participante:Participante',
                    sopt: ['eq']
                }
            },
            { key: false, hidden:true, name: 'Observac', index: 'Observac', editable: true, search: false },
            { key: false, hidden: false, name: 'Participantes', index: 'Participantes', editable: true, search: false, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; }  },
            { key: false, name: 'TotalDemandas', index: 'TotalDemandas', editable: false, width: 60, search: false }
        ],
        pager: jQuery('#jqControlsReuniaoUser'),
        rowNum: 100,
        rowList: [10, 20, 30, 40, 50,1000],
        autowidth: true,
        height: 200,
        iconSet: "fontAwesome",
        resizable: true,
        toppager: false,
        viewrecords: true,
        caption: '<i class="fa fa-file-text-o"></i>&nbsp;Reuniões &nbsp;',
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
            var rowId = $("#jqGridReuniaoUser").jqGrid('getGridParam', 'selrow');
            //pega o id da subgrid
            var reuniaoid = $("#jqGridReuniaoUser").jqGrid('getCell', rowId, 'ReuniaoId');
            var nome = $("#jqGridReuniaoUser").jqGrid('getCell', rowId, 'Nome');

            $("#ReuniaoId").val(reuniaoid);
            $("#Nome").val(nome);

            //console.log(btnAlterar);
        },
        ondblClickRow: function (rowid) {
            window.location.replace('/Reuniao/Ata/' + rowid);
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
            //$("#jqGridReuniaoUser").regional["pt-br"]
            //alert("Regional PT-BR")
        },
        loadComplete: function (data) {

        }, inlineEditing: {
            keys: true, position: "afterSelected"
        },
    })
});
//Reuniao/Ata - Assuntos
$(function () {
    var Id = $("#ReuniaoId").val();
    $("#jqGridAtaAssuntos").jqGrid({
        url: "/Assunto/GetAssuntosReuniao/"+ Id,
        datatype: 'json',
        mtype: 'Get',
        colNames: ['AssuntoId','ReuniaoId', '<b>Situação:</b>', '<b>Descrição</b>', 'QuemInseriu', 'InseridoEm','Delete',''],
        colModel: [           
            { key: true, hidden: true, name: 'AssuntoId', index: 'AssuntoId', editable: true },
            { key: false, hidden: true, name: 'ReuniaoId', index: 'ReuniaoId', editable: true },
            {
                key: false,
                name: 'Situacao',
                index: 'Situacao',
                editable: true,
                formatter: 'select',
                edittype: 'select',
                stype: 'select',
                searchoptions: {
                    value: ':--Todos--;Aberto:Aberto;Suspenso:Suspenso;Encerrado:Encerrado'
                },
                editoptions: {
                    value: {
                        'Aberto': 'Aberto',
                        'Encerrado': 'Encerrado',
                        'Suspenso': 'Suspenso'
                    },
                    formatter: 'select'
                },
                width: 8
            },            
            {
                key: false, name: 'DescricaoAs', index: 'DescricaoAs', width: 40, editable: true, edittype: 'textarea',
                editoptions: { rows: 4 }, search: true },            
            { key: false, hidden: true, name: 'QuemInseriu', index: 'QuemInseriu', editable: true, search: false },
            {
                key: false,
                name: 'InseridoEm',
                index: 'InseridoEm',
                editable: false,
                hidden: true,
                formatter: "date",
                formatoptions: { srcformat: "ISO8601Long", newformat: "d/m/y" },
                width: 10
            },
            {
                key: false,
                name: 'Delete',
                index: 'Delete',
                edittype: 'checkbox',
                editable: true,
                hidden:true,
                editoptions: { value: "true:false" },
                editrules: { required: true },
                formatter: "checkbox",
                search: true
            },
            {
                name: "act", template: "actions", width: 20, formatter: "actions", formatoptions: {
                    keys: true,
                    addbutton: true,
                    editbutton: true,
                    delbutton: true,
                    addOption: {
                        onclickSubmit: function (options, rowid) {
                            var rowKey = jQuery("#jqGridAtaAssuntos").jqGrid('getGridParam', 'selrow');
                            var rowObject = jQuery('#jqGridAtaAssuntos').getRowData(rowKey);
                            console.log(rowObject)
                            rowObject.ReuniaoId = $("#ReuniaoId").val();
                            rowObject.AssuntoId = 0;
                            InsereAssunto(rowObject);
                        }
                    },
                    delOptions: {                         
                        oneditfunc: function (rowid) {
                            debugger;                            
                            var rowKey = jQuery("#jqGridAtaAssuntos").jqGrid('getGridParam', 'selrow');
                            var rowObject = jQuery('#jqGridAtaAssuntos').getRowData(rowKey);
                            console.log(rowObject)
                            if (isNumber(rowid) == true) {
                                RemoverAssunto(rowObject);
                            }                                
                        },                         
                        onclickSubmit: function (options, rowid) {
                            var rowKey = jQuery("#jqGridAtaAssuntos").jqGrid('getGridParam', 'selrow');
                            var rowObject = jQuery('#jqGridAtaAssuntos').getRowData(rowKey);
                            console.log(rowObject)
                            //SE O ID DO ASSUNTO FOR NUMERCIO É EDIÇÃO
                            if (isNumber(rowid) == true) {
                                RemoverAssunto(rowObject);
                            }
                        }
                    },
                    restoreAfterError: true,
                    serializeRowData: function (data) { return JSON.stringify(data); },
                    onSuccess: function (jqXHR) {
                        console.log(jqXHR.statusText);
                    },
                    onError: function (rowid, jqXHR, textStatus) {
                        console.log(jqXHR.statusText);
                    },
                    
                    afterSave: function (rowid) {
                        var rowKey = jQuery("#jqGridAtaAssuntos").jqGrid('getGridParam', 'selrow');
                        var rowObject = jQuery('#jqGridAtaAssuntos').getRowData(rowKey);
                        console.log(rowObject)
                        //SE O ID DO ASSUNTO FOR NUMERCIO É EDIÇÃO
                        if (isNumber(rowid) == true) {
                            if (confirm("Deseja confirmar a alteração no assunto?")) {
                                AlterarAssunto(rowObject);
                            }
                        }
                        else {
                            console.log(rowObject)
                            rowObject.ReuniaoId = $("#ReuniaoId").val();
                            rowObject.AssuntoId = 0;

                            InsereAssunto(rowObject);
                        }
                        console.log(rowObject)

                    },                    
                }
            }
        ],
        pager: jQuery('#jqControlsAtaAssuntos'),
        rowNum: 1000,
        rowList: [1000, 10, 20, 30, 40, 50],
        autowidth: true,
        height: 'auto',
        iconSet: "fontAwesome",
        resizable: true,
        toppager: true,
        cloneToTop: false,
        pgbuttons: false,
        viewrecords: false,
        pgtext: "",
        pginput: false,
        caption: '<i class="fa fa-file-text-o"></i>&nbsp;Reunião &nbsp;&nbsp;<i class="fa fa-caret-right"></i>&nbsp;Assuntos',
        emptyrecords: 'Não há assuntos cadastradas',
        inlineEditing: {
            keys: true, position: "afterSelected"       
        },
        actionsNavOptions: {
            edittitle: "Alterar assunto",
            deltitle: "Remover o assunto",
            savetitle: "Confirmar alteração",
            canceltitle: "Cancelar edição",            
        },
        onSelectRow: function(id) {
            var selRowId = $("#jqGridAtaAssuntos").jqGrid('getGridParam', 'selrow');
            var celValue = $("#jqGridAtaAssuntos").jqGrid('getCell', selRowId, 'AssuntoId');
            var assuntoValue = $("#jqGridAtaAssuntos").jqGrid('getCell', selRowId, 'DescricaoAs');
            var demandasValue = $("#jqGridAtaAssuntos").jqGrid('getCell', selRowId, 'TotalDemandas');
            console.log("Selecionado Item que possui a quantidade de demanda" + demandasValue);
            var seleciona = celValue;
            $("AssuntoId").val(seleciona);
            console.log(assuntoValue)
            var searchFiler = assuntoValue, f;
            if (searchFiler.length === 0) {
                $("#jqGridDemanda")[0].p.search = false;
                $.extend($("#jqGridDemanda")[0].p.postData, { filters: "" });
            }
            f = { groupOp: "OR", rules: [] };
            f.rules.push({ field: "Assunto", op: "cn", data: searchFiler });
            $("#jqGridDemanda")[0].p.search = true;
            $.extend($("#jqGridDemanda")[0].p.postData, { filters: JSON.stringify(f) });
            $("#gs_jqGridDemanda_Assunto").val(assuntoValue)
            if (demandasValue > 0) {
                //$("#jqGridDemanda").jqGrid('setGridState', 'visible');
                debugger;
            } else {
                alert("Item Não possui demandas");
            }
        
            
        },
        gridComplete: function () {
            
            var GridAtaAssuntos_rows = $('#jqGridAtaAssuntos').jqGrid('getDataIDs');
            console.log("qtd Assuntos", GridAtaAssuntos_rows);
            $('#jqGridAtaAssuntos').jqGrid('navGrid', '#jqControlsAtaAssuntos', {
                cloneToTop: true, add: false, edit: false, del: false, search: false
            }, {
                    zIndex: 100,
                    url: '/Assunto/Create',
                    closeOnEscape: true,
                    closeAfterEdit: true,
                    recreateForm: true,
                    afterComplete: function (response) {
                        if (confirm("Deseja confirmar a alteração no assunto?")) {
                            var rowKey = jQuery("#jqGridAtaAssuntos").jqGrid('getGridParam', 'selrow');
                            var rowObject = jQuery('#jqGridAtaAssuntos').getRowData(rowKey);
                            console.log(rowObject)
                            AlterarAssunto(rowObject);
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
                })


            //In case of usage more old version of jqGrid the code should be like below

            var inlineEditOptions = {
                extraparam: {
                    environment: function () {
                        return alert('Teste');
                    }
                }
            };
            var cont = +1;
            if (cont == 1) {
                $('#jqGridAtaAssuntos').jqGrid('filterToolbar', {
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
            }
            //funcoes dos botões de baixo da grid
            $('#jqGridAtaAssuntos').jqGrid('inlineNav', '#jqControlsAtaAssuntos', {
                cloneToTop: true,
                add: true,
                addtext: "Novo Assunto",
                addicon: "fa fa-fw fa-plus",
                edit: true,
                editicon: "fa-pencil",
                edittext: "Editar Assunto",
                save: true,
                afterSave: function () {
                    debugger;
                    console.log("O assunto a ser inserido possui Id:" + id);
                    if (confirm("Deseja confirmar a alteração no assunto?")) {
                        var rowKey = jQuery("#jqGridAtaAssuntos").jqGrid('getGridParam', 'selrow');
                        var rowObject = jQuery('#jqGridAtaAssuntos').getRowData(rowKey);
                        console.log(rowObject)
                        InsereAssunto(rowObject);
                    }
                },
                addParams: {
                    position: "afterSelected",
                    addRowParams: {
                        // the parameters of editRow used to edit new row
                        keys: true,
                        oneditfunc: function (rowid) {
                            //alert("Somente o moderador pode cadastrar itens");
                        },

                        aftersavefunc: function (id) {
                            var rowKey = jQuery("#jqGridAtaAssuntos").jqGrid('getGridParam', 'selrow');
                            var rowObject = jQuery('#jqGridAtaAssuntos').getRowData(rowKey);
                            //console.log(rowObject)

                            rowObject.ReuniaoId = $("#ReuniaoId").val();
                            rowObject.AssuntoId = 0;
                            console.log(rowObject)
                            InsereAssunto(rowObject);
                        },
                    }
                },
                saveicon: "fa-floppy-o",
                savetext: "Savar",
                cancel: true,
                cancelicon: "fa-ban",
                canceltext: "Cancel",
                search: true,
                searchtext: "Seaech",
                editParams: {
                    aftersavefunc: function (id) {
                        if (isNumber(id) == true) {
                            if (confirm("Deseja confirmar a alteração no assunto?")) {
                                var rowKey = jQuery("#jqGridAtaAssuntos").jqGrid('getGridParam', 'selrow');
                                var rowObject = jQuery('#jqGridAtaAssuntos').getRowData(rowKey);
                                console.log(rowObject)

                                AlterarAssunto(rowObject);
                            }
                        }
                    },
                    beforesavefunc: function (id) {
                        alert('antes')
                    },
                    keys: false,
                    oneditfunc: null,
                    successfunc: null,
                    //url: '/Assunto/Edit',          
                    errorfunc: null,
                    afterrestorefunc: null,
                    restoreAfterError: true,
                    mtype: "POST"
                }
            });
            
            //percorre a grid de assuntos e faz alteracao de cores 
            for (x = 0; x <= GridAtaAssuntos_rows.length; x++) {
                var situacao = $('#jqGridAtaAssuntos').jqGrid('getCell', GridAtaAssuntos_rows[x], 'Situacao');
                console.log(situacao);
                if (situacao == 'Aberto') {
                    $('#jqGridAtaAssuntos').jqGrid('setCell', GridAtaAssuntos_rows[x], 'Situacao', 'Aberto', { color: 'red' });
                    console.log('Situacao');
                }
                if (situacao == 'Encerrado') {
                    $('#jqGridAtaAssuntos').jqGrid('setCell', GridAtaAssuntos_rows[x], 'Situacao', 'Encerrado', { color: 'green' });
                }
                if (situacao == 'Suspenso') {
                    $('#jqGridAtaAssuntos').jqGrid('setCell', GridAtaAssuntos_rows[x], 'Situacao', 'Suspenso', { color: 'orange' });
                }
            }
            
        },
        loadComplete: function (data) {
            
        },        
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
        subGrid: true,
        scrollOffset: 18,
        beforeSelectRow: function (rowid, e) {

        },
        subGridRowExpanded: function (subgrid_id, row_id) {            
            var subgrid_table_id, pager_id;
            var seleciona = row_id;
            console.log("Assunto aberto é:" + row_id + " é")
            var ID = row_id;
            //esconde as outras subgrids ao selecionar uma nova
            var expanded = jQuery("td.sgexpanded", "#jqGridAtaAssuntos")[0];
            $("#AssuntoId").val(seleciona);
            //console.log(expanded.length);
            if (expanded) {
                setTimeout(function () {
                    $(expanded).trigger("click");
                }, 100);
            }
            console.log("Assunto aberto é:" + row_id + " é")
            subgrid_table_id = subgrid_id + "_t";
            pager_id = "p_" + subgrid_table_id;
            jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");            
            jQuery("#" + subgrid_table_id).jqGrid({
                url: "/ItemAssunto/ItemAssuntoSubGrid/" + row_id,
                datatype: 'json',
                mtype: 'POST',
                colNames: [
                    'ItemAssuntoId', 'Assunto', '<i class="glyphicon glyphicon-info-sign"></i>&nbsp;Situação', '<i class="glyphicon glyphicon-record"> </i> Pri.', 'Assunto', 'Id', '<i class="fa fa-thumb-tack"></i>Descrição do Item', '<i class="fa fa-calendar-check-o" aria-hidden="true">&nbsp;</i>Responsaveis',
                    'Inserido em', 'QuemInseriu', '<i class="fa fa-pencil-square-o" aria-hidden="true">&nbsp;</i>Quem Solic.', '<i class="fa fa-users">&nbsp;</i>Demandados', '<i class="fa fa-wrench">&nbsp;</i>',''
                ],
                colModel: [
                    { key: true, hidden: true, name: 'ItemAssuntoId', index: 'ItemAssuntoId', editable: false },
                    { key: true, hidden: true, name: 'Assunto', index: 'Assunto', editable: false },
                    {
                        key: false,
                        name: 'Situacao',
                        index: 'Situacao',
                        editable: true,
                        formatter: 'select',
                        edittype: 'select',
                        stype: 'select',
                        searchoptions: {
                            value: ':--Todos--;Aberto:Aberto;Suspenso:Suspenso;Encerrado:Encerrado'
                        },
                        editoptions: {
                            value: {
                                'Aberto': 'Aberto',
                                'Encerrado': 'Encerrado',
                                'Suspenso': 'Suspenso'
                            },
                            formatter: 'select'
                        }
                        , width: 7
                    },
                    {
                        key: false,
                        name: 'Prioridade',
                        index: 'Prioridade',
                        edittype: 'checkbox',
                        editable: true,
                        editoptions: { value: "true:false" },
                        editrules: { required: true },
                        formatter: "checkbox",
                        search: false,
                        width: 4
                    },
                    { key: false, hidden: true, name: 'AssuntoId', index: 'AssuntoId', editable: true },
                    { key: false, hidden: true, name: 'ReuniaoId', index: 'ReuniaoId', editable: true },
                    {
                        key: false, name: 'DescricaoItem', index: 'DescricaoItem', width: 23, editable: true, edittype: 'textarea',
                        editoptions: { rows: 5 }, search: true },
                    {
                        key: false, hidden: false, name: 'Responsavel', index: 'Responsavel', editable: true, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; } ,// stype defines the search type control - in this case HTML select (dropdownlist)
                        stype: "select",
                        edittype: 'select',
                        //chama função userreuniao
                        searchoptions: {
                            dataUrl: "/UserReuniao/ListaUsuarioReuniao/" + (ID = Id),
                            type: "GET",
                            contentType: 'application/json; charset=utf-8',
                            dataType: "json",
                            cache: false,
                            buildSelect: function (data) {
                                var response = jQuery.parseJSON(data), s = '<select class="Resp" >', i, l, ri;
                                s += '<option value="">--Todos--</option>';
                                console.log("Tamanho" + response.participantes.length)
                                console.log("json" + response.participantes)
                                if (response.participantes && response.participantes.length) {
                                    for (i = 0, l = response.participantes.length; i < l; i += 1) {
                                        ri = response.participantes[i];
                                        s += '<option value="' + ri + '">' + ri + '</option>';
                                    }
                                }
                                return s + '</select>';
                            }
                        },
                        editoptions: {
                            dataInit: function (element) {
                                $(element).width(120).select2({
                                    placeholder: "funcionario ou setor.",
                                    minimumInputLength: 0,
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
                                    multiple: false,
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
                                        cache: true
                                    }
                                });
                            }
                        }
                        , width: 12
                    },
                    {
                        key: false,
                        name: 'InseridoEm',
                        index: 'InseridoEm',
                        hidden: true,
                        editable: true,
                        formatter: "date",
                        formatoptions: { srcformat: "ISO8601Long", newformat: "Y/m/d" }
                        , width: 39
                    },
                    { key: false, hidden: true, name: 'QuemInseriu', index: 'QuemInseriu', editable: false },
                    {
                        key: false, hidden: false, name: 'Quem', index: 'Quem', editable: false, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; },// stype defines the search type control - in this case HTML select (dropdownlist)
                        stype: "select",
                        edittype: 'select',
                        //chama função userreuniao
                        searchoptions: {
                            dataUrl: "/UserReuniao/ListaUsuarioReuniao/" + (ID = Id),
                            type: "GET",
                            contentType: 'application/json; charset=utf-8',
                            dataType: "json",
                            cache: false,
                            sopt: ["cn"],
                            buildSelect: function (data) {
                                var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                                s += '<option value="">--Todos--</option>';
                                console.log("Tamanho" + response.participantes.length)
                                console.log("json" + response.participantes)
                                if (response.participantes && response.participantes.length) {
                                    for (i = 0, l = response.participantes.length; i < l; i += 1) {
                                        ri = response.participantes[i];
                                        s += '<option value="' + ri + '">' + ri + '</option>';
                                    }
                                }
                                return s + '</select>';
                            }
                        }
                        , width: 12
                    },
                    {
                        key: false, hidden: false, name: 'Demandado', index: 'Demandado', editable: false, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; } ,// stype defines the search type control - in this case HTML select (dropdownlist)
                        stype: "select",
                        edittype: 'select',
                        defaultSearch: 'cn',
                        //chama função userreuniao
                        searchoptions: {
                            dataUrl: "/UserReuniao/ListaUsuarioReuniao/" + (ID = Id),
                            type: "GET",
                            contentType: 'application/json; charset=utf-8',
                            dataType: "json",
                            cache: false,
                            sopt: ["cn"],
                            buildSelect: function (data) {
                                var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                                s += '<option value="">--Selecione--</option>';
                                console.log("Tamanho" + response.participantes.length)
                                console.log("json" + response.participantes)
                                if (response.participantes && response.participantes.length) {
                                    for (i = 0, l = response.participantes.length; i < l; i += 1) {
                                        ri = response.participantes[i];
                                        s += '<option value="' + ri + '">' + ri + '</option>';
                                    }
                                }
                                return s + '</select>';
                            }
                        },
                        editoptions: {
                            dataUrl: "/UserReuniao/ListaUsuarioReuniao/" + (ID = Id),
                            type: "GET",
                            contentType: 'application/json; charset=utf-8',
                            dataType: "json",
                            cache: false,
                            buildSelect: function (data) {
                                var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                                s += '<option value="">--Selecione--</option>';
                                console.log("Tamanho" + response.participantes.length)
                                console.log("json" + response.participantes)
                                if (response.participantes && response.participantes.length) {
                                    for (i = 0, l = response.participantes.length; i < l; i += 1) {
                                        ri = response.participantes[i];
                                        s += '<option value="' + ri + '">' + ri + '</option>';
                                    }
                                }
                                return s + '</select>';
                            }
                        }
                        , width: 12
                    },
                    { key: false, hidden: false, name: 'TotalDemandas', index: 'TotalDemandas', width: 4, editable: false,search: false },
                    {
                        name: "act", template: "actions", width: 100, formatter: "actions", formatoptions: {
                            keys: true,
                            addbutton: true,
                            editbutton: true,
                            delbutton: true,
                            addOption: {
                                onclickSubmit: function (options, rowid) {
                                    var rowKey = jQuery("#" + subgrid_table_id).jqGrid('getGridParam', 'selrow');
                                    var rowObject = jQuery("#" + subgrid_table_id).getRowData(rowKey);
                                    rowObject.AssuntoId = $("AssuntoId").val();
                                    //rowObject.AssuntoId = rowid;                                    
                                    if (isNumber(rowid) == true) {
                                        console.log(rowObject)
                                        AlterarItemAssunto(rowObject);
                                    } else {
                                        console.log(rowObject)
                                        selRowId = $("#jqGridAtaAssuntos").jqGrid('getGridParam', 'selrow'),
                                            celValue = myGrid.jqGrid('getCell', selRowId, 'AssuntoId');
                                        rowObject.ItemAssuntoId = "-1";
                                        InsereItemAssunto(rowObject)
                                    }

                                }
                            },
                            delOptions: {
                                oneditfunc: function (rowid) {
                                    debugger;
                                    var rowKey = jQuery("#" + subgrid_table_id).jqGrid('getGridParam', 'selrow');
                                    var rowObject = jQuery("#" + subgrid_table_id).getRowData(rowKey);
                                    console.log(rowObject)
                                    if (isNumber(rowid) == true) {
                                        RemoverItemAssunto(rowObject);
                                    }
                                },
                                onclickSubmit: function (options, rowid) {
                                    var rowKey = jQuery("#" + subgrid_table_id).jqGrid('getGridParam', 'selrow');
                                    var rowObject = jQuery("#" + subgrid_table_id).getRowData(rowKey);
                                    console.log(rowObject)
                                    //SE O ID DO ASSUNTO FOR NUMERCIO É EDIÇÃO
                                    if (isNumber(rowid) == true) {
                                        RemoverItemAssunto(rowObject);
                                    }
                                }
                            },
                            restoreAfterError: true,
                            serializeRowData: function (data) { return JSON.stringify(data); },
                            onSuccess: function (jqXHR) {
                                console.log(jqXHR.statusText);
                            },
                            onError: function (rowid, jqXHR, textStatus) {
                                console.log(jqXHR.statusText);
                            },

                            afterSave: function (rowid) {
                                var rowKey = jQuery("#" + subgrid_table_id).jqGrid('getGridParam', 'selrow');
                                var rowObject = jQuery("#" + subgrid_table_id).getRowData(rowKey);
                                rowObject.ReuniaoId = $("#ReuniaoId").val();
                                console.log("Id do assunro" + seleciona)

                                rowObject.AssuntoId = seleciona;

                                console.log(rowObject)
                                //SE O ID DO ASSUNTO FOR NUMERCIO É EDIÇÃO
                                if (isNumber(rowid) == true) {
                                    if (confirm("Deseja confirmar a alteração no item?")) {
                                        AlterarItemAssunto(rowObject);
                                    }
                                }
                                else {
                                    rowObject.ItemAssuntoId = 0;
                                    InsereItemAssunto(rowObject);
                                }
                                console.log(rowObject)

                            },
                        }
                    }
                ],
                rowNum: 10,
                pager: pager_id,
                rowList: [10, 20, 30, 40, 50],
                autowidth: true,
                height: 200,
                sortname: 'Id',
                sortorder: 'asc',
                height: 'auto',
                iconSet: "fontAwesome",
                viewrecords: false,
                pgbuttons: false,
                pginput: false,
                pgtext: "",
                caption: '<i class="fa fa-thumb-tack">&nbsp</i>Itens&nbsp',
                emptyrecords: 'Não há itens para este assunto!',
                inlineEditing: {
                    keys: true, position: "afterSelected"
                },
                actionsNavOptions: {
                    edittitle: "Alterar assunto",
                    deltitle: "Remover o assunto",
                    savetitle: "Confirmar alteração",
                    canceltitle: "Cancelar edição",
                    demandatitle: "Gerenc. Demandas(Plano de Ação)",
                    demandaicon: "fa fa-calendar fa-2x",
                    custom: [
                        {
                            action: "demanda", position: "first",
                            onClick: function (options) {
                                var itemid = $("#ItemAssuntoId").val();
                                VisualizaItem(itemid);
                            }
                        }]
                },
                onSelectRow: function (id) {
                    
                    
                },
                loadComplete: function () {
                    
                },
                gridComplete: function () {
                    $('#' + subgrid_table_id).jqGrid('inlineNav', '#' + pager_id, {
                        add: true,
                        cloneToTop: true,
                        toppager: true,
                        addtext: "Novo Item",
                        addicon: "fa fa-fw fa-plus",
                        edit: true,
                        editicon: "fa-pencil",
                        edittext: "Editar Item",
                        save: true,
                        search: false,
                        refreshtext: "Atualizar",
                        view: false,
                        afterSave: function () {
                            debugger;
                            console.log("O item a ser inserido possui Id:" + id);
                            if (confirm("Confirmar a alteração no item?")) {
                                //$('#' + subgrid_table_id).jqGrid('inlineNav', '#' + pager_id, {

                                var rowKey = jQuery('#' + subgrid_table_id).jqGrid('getGridParam', 'selrow');
                                var rowObject = jQuery('#' + subgrid_table_id).getRowData(rowKey);
                                console.log(rowObject)
                                AlterarItemAssunto(rowObject);
                            }
                        },
                        addParams: {
                            position: "afterSelected",
                            addRowParams: {
                                // the parameters of editRow used to edit new row
                                keys: true,
                                oneditfunc: function (rowid) {

                                },
                                aftersavefunc: function (id) {
                                    var rowKey = jQuery('#' + subgrid_table_id).jqGrid('getGridParam', 'selrow');
                                    var rowObject = jQuery('#' + subgrid_table_id).getRowData(rowKey);
                                    console.log(rowObject)

                                    rowObject.ReuniaoId = $("#ReuniaoId").val();
                                    rowObject.AssuntoId = seleciona;
                                    rowObject.ItemAssuntoId = "-1";
                                    InsereItemAssunto(rowObject);
                                },
                            }
                        },
                        saveicon: "fa-floppy-o",
                        savetext: "Savar",
                        cancel: true,
                        cancelicon: "fa-ban",
                        canceltext: "Cancelar",
                        editParams: {
                            aftersavefunc: function (id) {
                                if (isNumber(id) == true) {
                                    if (confirm("Confirmar a alteração no assunto?")) {
                                        var rowKey = jQuery('#' + subgrid_table_id).jqGrid('getGridParam', 'selrow');
                                        var rowObject = jQuery('#' + subgrid_table_id).getRowData(rowKey);
                                        console.log(rowObject)

                                        AlterarItemAssunto(rowObject);
                                    }
                                }
                            },
                            beforesavefunc: function (id) {
                                alert('antes')
                            },
                            keys: false,
                            oneditfunc: null,
                            successfunc: null,
                            //url: '/Assunto/Edit',          
                            errorfunc: null,
                            afterrestorefunc: null,
                            restoreAfterError: true,
                            mtype: "POST"
                        }
                    });
                    //customização de botoes
                    $("#" + subgrid_table_id).jqGrid('filterToolbar', {
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
                loadonce: true, //Pesquisa
                forceClientSorting: true,
                multiselect: false,
                subGrid: false,
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
});
//Itens Agrupados por assuntos
$(function () {
    var Id = $("#ReuniaoId").val();
    $("#jqGridAtaAssuntosItens").jqGrid({
        url: "/ItemAssunto/GetAssuntosItensReuniao/" + Id,
        datatype: 'json',
        mtype: 'Get',
        colNames: [
            'ItemAssuntoId', 'Assunto', 'AssuntoId', '<i class="glyphicon glyphicon-info-sign"></i>&nbsp;Sit.', 'Pri.', 'Id', 'Descrição do Item', '<i class="fa fa-calendar-check-o" aria-hidden="true">&nbsp;</i>Responsáveis',
            'Inserido em', 'QuemInseriu', '<i class="fa fa-pencil-square-o" aria-hidden="true">&nbsp;</i>Quem Solic.', '<i class="fa fa-users">&nbsp;</i>Demandados', '<i class="fa fa-wrench">&nbsp;</i>', 'Ações'
        ],
        colModel: [
            { key: true, hidden: true, name: 'ItemAssuntoId', index: 'ItemAssuntoId', editable: false },
            {
                key: false,
                hidden: true,
                name: 'Assunto',
                index: 'Assunto',
                editable: true,
                width: 8,
                edittype: 'select',
                stype: 'select',
                editoptions: {
                    dataUrl: "/ItemAssunto/ListaAssuntos/" + (ID = Id),
                    type: "GET",
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: true,
                    buildSelect: function (data) {
                        var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                        s += '<option value="0">--Selecione</option>';
                        console.log("Tamanho" + response.assuntos.length)
                        console.log("json" + response.assuntos)
                        if (response.assuntos && response.assuntos.length) {
                            for (i = 0, l = response.assuntos.length; i < l; i += 1) {
                                ri = response.assuntos[i].AssuntoId;
                                console.log("Ide:" + ri)
                                s += '<option value="' + ri + '">' + response.assuntos[i].DescricaoAs + '</option>';
                            }
                        }
                        return s + '</select>';
                    }
                }
            },
            {
                key: false,
                hidden: false,
                name: 'AssuntoId',
                index: 'AssuntoId',
                editable: true,
                width: 8,
                edittype: 'select',
                stype: 'select',
                editoptions: {
                    dataUrl: "/ItemAssunto/ListaAssuntos/" + (ID = Id),
                    type: "GET",
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: false,
                    buildSelect: function (data) {
                        var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                        s += '<option value="0">--Selecione--</option>';
                        console.log("Tamanho" + response.assuntos.length)
                        console.log("json" + response.assuntos)
                        if (response.assuntos && response.assuntos.length) {
                            for (i = 0, l = response.assuntos.length; i < l; i += 1) {
                                ri = response.assuntos[i].AssuntoId;
                                console.log("Ide:"+ri)
                                s += '<option onchange="SelecionaAssuntoInter(' + response.assuntos[i].AssuntoId + ')" value="' + response.assuntos[i].AssuntoId + '">' + response.assuntos[i].DescricaoAs + '</option>';
                            }
                        }
                        return s + '</select>';
                    },
                    dataEvents: [{ type: 'change', fn: function (e) { debugger; SelecionaAssuntoInter($('select[name=AssuntoId]').val()) } }]
                }
            },
            {
                key: false,
                name: 'Situacao',
                index: 'Situacao',
                editable: true,
                formatter: 'select',
                edittype: 'select',
                stype: 'select',
                searchoptions: {
                    value: ':--Todos--;Aberto:Aberto;Suspenso:Suspenso;Encerrado:Encerrado'
                },
                editoptions: {
                    value: {
                        'Aberto': 'Aberto',
                        'Encerrado': 'Encerrado',
                        'Suspenso': 'Suspenso'
                    },
                    formatter: 'select'
                }
                , width: 7
                , align: 'left'
            },
            {
                key: false,
                name: 'Prioridade',
                index: 'Prioridade',
                edittype: 'checkbox',
                editable: true,
                editoptions: { value: "true:false" },
                editrules: { required: true },
                formatter: "checkbox",
                search: false,
                width: 4
            },
            
            { key: false, hidden: true, name: 'ReuniaoId', index: 'ReuniaoId', editable: true },
            {
                key: false, name: 'DescricaoItem', index: 'DescricaoItem', width: 23, editable: true, edittype: 'textarea',
                editoptions: { rows: 5 }, search: true
            },
            {
                key: false, hidden: false, name: 'Responsavel', index: 'Responsavel', editable: true, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; },// stype defines the search type control - in this case HTML select (dropdownlist)
                stype: "select",
                edittype: 'select',
                //chama função userreuniao
                searchoptions: {
                    dataUrl: "/UserReuniao/ListaUsuarioReuniao/" + (ID = Id),
                    type: "GET",
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: false,
                    buildSelect: function (data) {
                        var response = jQuery.parseJSON(data), s = '<select class="Resp" >', i, l, ri;
                        s += '<option value="">--Todos--</option>';
                        console.log("Tamanho" + response.participantes.length)
                        console.log("json" + response.participantes)
                        if (response.participantes && response.participantes.length) {
                            for (i = 0, l = response.participantes.length; i < l; i += 1) {
                                ri = response.participantes[i];
                                s += '<option value="' + ri + '">' + ri + '</option>';
                            }
                        }
                        return s + '</select>';
                    }
                },
                editoptions: {
                    dataUrl: "/UserReuniao/ListaUsuarioReuniao/" + (ID = Id),
                    type: "GET",
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: false,
                    buildSelect: function (data) {
                        var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                        s += '<option value="">--Selecione--</option>';
                        console.log("Tamanho" + response.participantes.length)
                        console.log("json" + response.participantes)
                        if (response.participantes && response.participantes.length) {
                            for (i = 0, l = response.participantes.length; i < l; i += 1) {
                                ri = response.participantes[i];
                                s += '<option value="' + ri + '">' + ri + '</option>';
                            }
                        }
                        return s + '</select>';
                    }
                }, width: 12
            },
            {
                key: false,
                name: 'InseridoEm',
                index: 'InseridoEm',
                hidden: true,
                editable: true,
                formatter: "date",
                formatoptions: { srcformat: "ISO8601Long", newformat: "Y/m/d" }
                , width: 39
            },
            { key: false, hidden: true, name: 'QuemInseriu', index: 'QuemInseriu', editable: false },
            {
                key: false, hidden: false, name: 'Quem', index: 'Quem', editable: false, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; },// stype defines the search type control - in this case HTML select (dropdownlist)
                stype: "select",
                edittype: 'select',
                //chama função userreuniao
                searchoptions: {
                    dataUrl: "/UserReuniao/ListaUsuarioReuniao/" + (ID = Id),
                    type: "GET",
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: false,
                    sopt: ["cn"],
                    buildSelect: function (data) {
                        var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                        s += '<option value="">--Todos--</option>';
                        console.log("Tamanho" + response.participantes.length)
                        console.log("json" + response.participantes)
                        if (response.participantes && response.participantes.length) {
                            for (i = 0, l = response.participantes.length; i < l; i += 1) {
                                ri = response.participantes[i];
                                s += '<option value="' + ri + '">' + ri + '</option>';
                            }
                        }
                        return s + '</select>';
                    }
                }
                , width: 12
            },
            {
                key: false, hidden: false, name: 'Demandado', index: 'Demandado', editable: false, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; },// stype defines the search type control - in this case HTML select (dropdownlist)
                stype: "select",
                edittype: 'select',
                defaultSearch: 'cn',
                //chama função userreuniao
                searchoptions: {
                    dataUrl: "/UserReuniao/ListaUsuarioReuniao/" + (ID = Id),
                    type: "GET",
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: false,
                    sopt: ["cn"],
                    buildSelect: function (data) {
                        var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                        s += '<option value="">--Selecione--</option>';
                        console.log("Tamanho" + response.participantes.length)
                        console.log("json" + response.participantes)
                        if (response.participantes && response.participantes.length) {
                            for (i = 0, l = response.participantes.length; i < l; i += 1) {
                                ri = response.participantes[i];
                                s += '<option value="' + ri + '">' + ri + '</option>';
                            }
                        }
                        return s + '</select>';
                    }
                },
                editoptions: {
                    dataUrl: "/UserReuniao/ListaUsuarioReuniao/" + (ID = Id),
                    type: "GET",
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: false,
                    buildSelect: function (data) {
                        var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                        s += '<option value="">--Selecione--</option>';
                        console.log("Tamanho" + response.participantes.length)
                        console.log("json" + response.participantes)
                        if (response.participantes && response.participantes.length) {
                            for (i = 0, l = response.participantes.length; i < l; i += 1) {
                                ri = response.participantes[i];
                                s += '<option value="' + ri + '">' + ri + '</option>';
                            }
                        }
                        return s + '</select>';
                    }
                }
                , width: 12
            },
            { key: false, hidden: false, name: 'TotalDemandas', index: 'TotalDemandas', width: 4, editable: false, search: false },
            {
                name: "act", template: "actions", width: 100, formatter: "actions", formatoptions: {
                    keys: true,
                    addbutton: true,
                    editbutton: true,
                    delbutton: false,
                    addOption: {
                        onclickSubmit: function (options, rowid) {
                            var rowKey = $("jqGridAtaAssuntosItens").jqGrid('getGridParam', 'selrow');
                            var rowObject = $("jqGridAtaAssuntosItens").getRowData(rowKey);
                            rowObject.AssuntoId = $("AssuntoId").val();
                            //rowObject.AssuntoId = rowid;                                    
                            if (isNumber(rowid) == true) {
                                console.log(rowObject)
                                AlterarItemAssunto(rowObject);
                            } else {
                                console.log(rowObject)
                                selRowId = $("jqGridAtaAssuntosItens").jqGrid('getGridParam', 'selrow'),
                                    celValue = myGrid.jqGrid('getCell', selRowId, 'AssuntoId');
                                rowObject.ItemAssuntoId = "-1";
                                InsereItemAssunto(rowObject)
                            }

                        }
                    },
                    delOptions: {
                        oneditfunc: function (rowid) {
                            debugger;
                            var rowKey = $("#jqGridAtaAssuntosItens").jqGrid('getGridParam', 'selrow');
                            var rowObject = $("#jqGridAtaAssuntosItens").getRowData(rowKey);
                            console.log(rowObject)
                            if (isNumber(rowid) == true) {
                                RemoverItemAssuntoInter(rowid);
                            }
                        },
                        onclickSubmit: function (options, rowid) {
                            var rowKey = $("#jqGridAtaAssuntosItens").jqGrid('getGridParam', 'selrow');
                            var rowObject = $("#jqGridAtaAssuntosItens").getRowData(rowKey);
                            console.log(rowObject)
                            //SE O ID DO ASSUNTO FOR NUMERCIO É EDIÇÃO
                            if (isNumber(rowid) == true) {
                                RemoverItemAssunto(rowObject);
                            }
                        }
                    },
                    restoreAfterError: true,
                    serializeRowData: function (data) { return JSON.stringify(data); },
                    onSuccess: function (jqXHR) {
                        console.log(jqXHR.statusText);
                    },
                    onError: function (rowid, jqXHR, textStatus) {
                        console.log(jqXHR.statusText);
                    },
                    afterSave: function (rowid) {
                        var rowKey = $("#jqGridAtaAssuntosItens").jqGrid('getGridParam', 'selrow');
                        var rowObject = $("#jqGridAtaAssuntosItens").getRowData(rowKey);
                        console.log(rowObject)                        //SE O ID DO ASSUNTO FOR NUMERCIO É EDIÇÃO
                        if (isNumber(rowid) == true) {
                            if (confirm("Deseja confirmar a alteração no item?")) {
                                $("#AssuntoId").val()
                                rowObject.AssuntoId = $("#AssuntoId").val();
                                debugger;
                                AlterarItemAssunto(rowObject);
                            }
                        }
                        else {
                            rowObject.ItemAssuntoId = 0;
                            InsereItemAssunto(rowObject);
                        }
                        console.log(rowObject)

                    },
                    onSelectRow: function (id) {
                        alert("teste");
                    }
                }
            }
        ],
        pager: jQuery('#jqControlsAtaAssuntosItens'),
        rowNum: 1000,
        rowList: [1000, 10, 20, 30, 40, 50],
        autowidth: true,
        height: 200,
        iconSet: "fontAwesome",
        resizable: true,
        toppager: true,
        cloneToTop: false,
        pgbuttons: true,
        viewrecords: true,
        pgtext: "",
        pginput: false,
        caption: '<i class="fa fa-file-text-o"></i>&nbsp;Assuntos &nbsp;<i class="fa fa-caret-right"></i>&nbsp;<i class="fa fa-thumb-tack"></i>&nbsp;Itens',
        emptyrecords: 'Não há assuntos cadastradas',
        inlineEditing: {
            keys: true,
            position: "frist"
        },
        actionsNavOptions: {
            edittitle: "Alterar assunto",
            deltitle: "Remover o assunto",
            savetitle: "Confirmar alteração",
            canceltitle: "Cancelar edição",
            demandatitle: "Atribuir demanda",
            deletetitle: "Remover item",
            demandaicon: "fa fa-calendar-plus-o",
            deleteicon: "fa fa-fw fa-trash-o",
            verdemandasicon: "fa fa-calendar",
            verdemandastitle: "Ver as demandas do item",
            custom: [
                {
                    action: "demanda", position: "first",
                    onClick: function (options) {
                        InsereDemanda(options.rowid)
                        console.log("option.rowid é" + options.rowid + "Item hidden é" + $("#ItemAssuntoId").val());

                        
                        var itemid = $("#ItemAssuntoId").val();
                        //ExportaPDF(itemid);
                    }
                },
                {
                    action: "verdemandas", position: "first",
                    onClick: function (options) {
                        let selRowId = $("#jqGridAtaAssuntosItens").jqGrid('getGridParam', 'selrow');
                        let celValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'ItemAssuntoId');
                        let celAssuntoIdValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'AssuntoId');
                        let assuntoValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'Assunto');
                        let itemValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'DescricaoItem');
                        let qtddemandas = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'TotalDemandas');

                        let seleciona = celValue;
                        $("#AssuntoId").val(celAssuntoIdValue);
                        $("#DescAssunto").val(assuntoValue);
                        $("#ItemAssuntoId").val(celValue);
                        $("#DescItemAssunto").val(itemValue);
                        console.log("Seleciona item:" + celValue + " do assunto:" + assuntoValue)
                        if (qtddemandas > 0) {
                            let searchFiler = assuntoValue, f;
                            FiltrarDemandaPorItem(searchFiler, qtddemandas);
                        }
                        

                    }
                },
                 {
                     action: "delete", position: "last",
                     onClick: function (options) {
                         var selRowId = $("#jqGridAtaAssuntosItens").jqGrid('getGridParam', 'selrow');
                         var celValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'ItemAssuntoId');
                         var celAssuntoIdValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'AssuntoId');
                         var assuntoValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'Assunto');
                         var itemValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'DescricaoItem');
                         var seleciona = celValue;
                         $("#AssuntoId").val(celAssuntoIdValue);
                         $("#DescAssunto").val(assuntoValue);
                         $("#ItemAssuntoId").val(celValue);
                         $("#DescItemAssunto").val(itemValue);
                         RemoverItemAssuntoInter(celValue)
                     }
                 }]
        },
        gridComplete: function () {
            //Export = "";
            //$('#jqGridDemanda').jqGrid('setGridState', 'hidden'); //or 'hidden'
            var rowIds = $("#jqGridAtaAssuntosItens").getDataIDs();
            $.each(rowIds, function (index, rowId) {
                $("#jqGridAtaAssuntosItens").expandSubGridRow(rowId);
            });
            var rows = $("#jqGridAtaAssuntosItens").jqGrid('getDataIDs');
            console.log("qtd", rows);
            //$("[aria-describedby=jqGridAtaAssuntosItens_Assunto]").html("");
            //$("[aria-describedby=jqGridAtaAssuntosItens_Assunto]").css('visibility', 'hidden');
            //percorre a grid de assuntos e faz alteracao de cores 
            for (a = 0; a <= rows.length; a++) {
                var situacao = $("#jqGridAtaAssuntosItens").jqGrid('getCell', rows[a], 'Situacao');
                console.log(situacao);
                if (situacao == 'Aberto') {
                    $("#jqGridAtaAssuntosItens").jqGrid('setCell', rows[a], 'Situacao', 'Aberto', { color: 'red' });
                    
                }
                if (situacao == 'Encerrado') {
                    $("#jqGridAtaAssuntosItens").jqGrid('setCell', rows[a], 'Situacao', 'Encerrado', { color: 'green' });
                   
                    
                }
                if (situacao == 'Suspenso') {
                    $("#jqGridAtaAssuntosItens").jqGrid('setCell', rows[a], 'Situacao', 'Suspenso', { color: 'orange' });
                    
                }
            }
            
            
            
        },
        loadComplete: function () {
            
        },               
        //quando seleciona a linha permite alterar os campos e quando apertar entrer ira salvar
        onSelectRow: function (id) {
            let selRowId = $("#jqGridAtaAssuntosItens").jqGrid('getGridParam', 'selrow');
            let celValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'ItemAssuntoId');
            let celAssuntoIdValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'AssuntoId');
            let assuntoValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'Assunto');
            let itemValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'DescricaoItem');
            let qtddemandas = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'TotalDemandas');

            let seleciona = celValue;
            $("#AssuntoId").val(celAssuntoIdValue);
            $("#DescAssunto").val(assuntoValue);
            $("#ItemAssuntoId").val(celValue);
            $("#DescItemAssunto").val(itemValue);
            $("#Qtddemandas").val(qtddemandas)
            console.log("Seleciona item:" + celValue + " do assunto:" + assuntoValue)
            if (qtddemandas > 0) {
                let searchFiler = assuntoValue, f;
                FiltrarDemandaPorItem(searchFiler, qtddemandas);
                //$("#jqGridAtaAssuntosItens").jqGrid('setGridState', 'hidden');
                //$("#jqGridDemanda").jqGrid('setGridState', 'visible');
            }
            
            
            debugger;
        },
        ondblClickRow: function (rowid) {
            //var selRowId_ = $("#jqGridAtaAssuntosItens").jqGrid('getGridParam', 'selrow');
            //var qtddemandas_ = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId_, 'TotalDemandas');
            if ($("#Qtddemandas").val() > 0) {
                $("#jqGridAtaAssuntosItens").jqGrid('setGridState', 'hidden');
                $("#jqGridDemanda").jqGrid('setGridState', 'visible');
            }
        },
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
        subGrid: false,
        scrollOffset: 18,
        grouping: true,
        groupingView: {
            groupField: ['Assunto'],
            groupSummary: [true],
            groupColumnShow: [false],
            groupText: ['<b>{0} - {1} Item(s)</b>&nbsp<i class="fa fa-thumb-tack" title="Itens do Assunto: {0}"></i>&nbsp;<button class="btn btn-default btnReuniaoItemOPT" id="CriarItem" onclick="AdicionarItemInter()"><i class="fa fa-lg fa-fw fa-plus">&nbsp;</i>&nbsp;Novo Item</button>'],
            groupCollapse: false,
            groupOrder: ['asc']
        },
        beforeSelectRow: function (rowid, e) {

        },
    })
});
//Reuniao/ata - DemandasGrid
$(function () {
    initDateEdit = function (elem) {
        $(elem).datepicker({
            dateFormat: "d-m-y",
            autoSize: true,
            changeYear: true,
            changeMonth: true,
            showButtonPanel: true,
            showWeek: true
        });
    },
    initDateSearch = function (elem) {
        setTimeout(function () {
            initDateEdit(elem);
        }, 100);
    };
    $("#jqGridDemanda").jqGrid({
        url: "/Demanda/GetDemanda/" + $('#ReuniaoId').val(),
        datatype: 'json',
        type: 'Get',
        colNames: [
            'DemandaId', '<i class="glyphicon glyphicon-info-sign"></i><b>Sit.</b>', 'ReuniaoId', '<b>Assunto</b>', '<b><i class="fa fa-thumb-tack">&nbsp;</i> item</b>', 'O que', 'Como','Porque','Quando',
            'Onde', 'Demandador','Demandados', 'Quanto','Ações'
        ],
        colModel: [
            { key: true, hidden: true, name: 'DemandaId', index: 'DemandaId', editable: false, sorttype: "integer", sortable: true },
            {
                key: false,
                name: 'Situacao',
                index: 'Situacao',
                editable: true,
                formatter: 'select',
                edittype: 'select',
                stype: 'select',
                searchoptions: {
                    value: ':--Todos--;Aberto:Aberto;Suspenso:Suspenso;Encerrado:Encerrado'
                },
                editoptions: {
                    value: {
                        'Aberto': 'Aberto',
                        'Encerrado': 'Encerrado',
                        'Suspenso': 'Suspenso'
                    },
                    formatter: 'select'
                }
                , width: 50
            },
            { key: false, name: 'ReuniaoId', hidden: true, index: 'ReuniaoId', editable: true },
            {
                key: false, name: 'Assunto', index: 'Assunto', editable: true, width: 75, stype: 'select',
                searchoptions: {
                    dataUrl: "/Assunto/ListaAssunto/" + $('#ReuniaoId').val(),
                    type: "GET",
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: false,
                    buildSelect: function (data) {
                        console.log($('#ReuniaoId').val());
                        var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                        s += '<option value=""></option>';
                        console.log("Tamanho" + response.d.length)
                        console.log("json" + response.d)
                        if (response.d && response.d.length) {
                            for (i = 0, l = response.d.length; i < l; i += 1) {
                                ri = response.d[i];
                                s += '<option value="' + ri + '">' + ri + '</option>';
                            }
                        }
                        return s + '</select>';
                    }
                }
            },
            {
                key: false, name: 'Item', index: 'Item', editable: true, width: 75, search: true, stype: 'select',
                searchoptions: {
                    dataUrl: "/ItemAssunto/ListaItem/" + $('#ReuniaoId').val(),
                    type: "GET",
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: false,
                    buildSelect: function (data) {
                        var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                        s += '<option value=""></option>';
                        console.log("Tamanho" + response.d.length)
                        console.log("json" + response.d)
                        if (response.d && response.d.length) {
                            for (i = 0, l = response.d.length; i < l; i += 1) {
                                ri = response.d[i];
                                s += '<option value="' + ri + '">' + ri + '</option>';
                            }
                        }
                        return s + '</select>';
                    }
                }
            },
            { key: false, name: 'Oque', index: 'Oque', editable: true, width: 180, edittype: 'textarea', editoptions: { rows: "6", cols: "35" }, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; }  },
            { key: false, name: 'Como', index: 'Como', editable: true, width: 180, edittype: 'textarea', editoptions: { rows: "6", cols: "35" }, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; },  search: false },
            { key: false, name: 'Porque', index: 'Porque', editable: true, width: 180, edittype: 'textarea', editoptions: { rows: "6", cols: "35" }, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; } , search: false },        
            {
                name: "Quando", width: 70, align: "center", sorttype: "date", frozen: true,
                formatter: "date", formatoptions: { newformat: "d-m-y", reformatAfterEdit: true }, datefmt: "d-m-y",
                searchoptions: {
                    sopt: ["le"], // or any other search operation
                    dataInit: initDateSearch
                }
            },
            { key: false, name: 'Onde', index: 'Onde', editable: true, width: 90 },            
            {
                key: false, name: 'Quem', index: 'Quem', editable: true, stype: 'select', width: 110, defaultSearch: 'cn', cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; },
                searchoptions: {
                    dataUrl: "/Reuniao/ListaUsuarios",
                    type: "GET",
                    sopt: ["cn"],
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: false,
                    buildSelect: function (data) {
                        var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                        s += '<option value=""></option>';
                        console.log("Tamanho" + response.d.length)
                        console.log("json" + response.d)
                        if (response.d && response.d.length) {
                            for (i = 0, l = response.d.length; i < l; i += 1) {
                                ri = response.d[i];
                                s += '<option value="' + ri + '">' + ri + '</option>';
                            }
                        }
                        return s + '</select>';
                    }
                },
                formatter: function (cellvalue, options, rowObject) {
                    // format the cellvalue to new format
                    return '<span style="white-space: normal">' + cellvalue + '</span>'
                }
            },
            {
                key: false, name: 'Demandado', index: 'Demandado', editable: true, stype: 'select', width: 150, defaultSearch: 'cn', cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; },
                searchoptions: {
                    dataUrl: "/Reuniao/ListaUsuarios",
                    type: "GET",
                    sopt: ["cn"],
                    contentType: 'application/json; charset=utf-8',
                    dataType: "json",
                    cache: false,
                    buildSelect: function (data) {
                        var response = jQuery.parseJSON(data), s = '<select>', i, l, ri;
                        s += '<option value=""></option>';
                        console.log("Tamanho" + response.d.length)
                        console.log("json" + response.d)
                        if (response.d && response.d.length) {
                            for (i = 0, l = response.d.length; i < l; i += 1) {
                                ri = response.d[i];
                                s += '<option value="' + ri + '">' + ri + '</option>';
                            }
                        }
                        return s + '</select>';
                    }
                },
                formatter: function (cellvalue, options, rowObject) {
                    // format the cellvalue to new format
                    return '<span style="white-space: normal">' + cellvalue + '</span>'
                }
            },
            { key: false, name: 'Quanto', index: 'Quanto', editable: true, width: 60, search: false, cellattr: function (rowId, tv, rawObject, cm, rdata) { return 'style="white-space: normal;"'; } },
            {
                name: "act", template: "actions", width: 50, formatter: "actions", formatoptions: {
                    keys: true,
                    addbutton: false,
                    editbutton: false,
                    delbutton: false                    
                }
            }
        ],
        pager: jQuery('#jqControlsDemanda'),
        editable: true,
        ignoreCase: true,//desable case sensistive - na hora da pesquisa ajuda leras
        sortname: 'Id',
        sortorder: 'desc',
        shrinkToFit: false,
        width: 400,
        autowidth: true,
        autoencode: true,
        iconSet: "fontAwesome",
        resizable: false,
        rowNum: 5,
        loadonce: true, //Pesquisa
        forceClientSorting: true,
        rowList: [5, 10, 30, 100, 500],
        //pgbuttons: true, // disable page control like next, back button
        //pgtext: true, // disable pager text like 'Page 0 of 10'
        viewrecords: true, // disable current view record text like 'View 1-10 of 100'
        caption: '<i class="fa fa-calendar"></i>&nbsp;<b>Demandas</b> &nbsp;<i class="fa fa-caret-right"></i>&nbsp <i class="fa fa-wrench">&nbsp;</i>Açoes necessarias:',
        emptyrecords: 'Nao ha demandas disponiveis',
        actionsNavOptions: {
            edittitle: "Alterar demanda",
            deltitle: "Remover demanda",
            savetitle: "Confirmar alteração",
            canceltitle: "Cancelar",            
            deletetitle: "Remover item",
            deleteicon: "fa fa-fw fa-trash-o",
            editartitle: "Alterar demanda",
            editaricon: "fa fa-fw fa-pencil",
            custom: [                
                 {
                     action: "editar", position: "first",
                     onClick: function (options) {
                         var selRowId = $("#jqGridDemanda").jqGrid('getGridParam', 'selrow');
                         var celValue = $("#jqGridDemanda").jqGrid('getCell', selRowId, 'DemandaId');                         
                         $("#DemandaId").val(celValue);
                         VisualizaDemanda(celValue)
                     }
                 },
                 {
                     action: "delete", position: "last",
                     onClick: function (options) {
                         var selRowId = $("#jqGridAtaAssuntosItens").jqGrid('getGridParam', 'selrow');
                         var celValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'ItemAssuntoId');
                         var celAssuntoIdValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'AssuntoId');
                         var assuntoValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'Assunto');
                         var itemValue = $("#jqGridAtaAssuntosItens").jqGrid('getCell', selRowId, 'DescricaoItem');
                         var seleciona = celValue;
                         $("#AssuntoId").val(celAssuntoIdValue);
                         $("#DescAssunto").val(assuntoValue);
                         $("#ItemAssuntoId").val(celValue);
                         $("#DescItemAssunto").val(itemValue);
                         RemoverItemAssuntoInter(celValue)
                     }
                 }]
        },
        gridComplete: function () {
            //Export = "";
            //$('#jqGridDemanda').jqGrid('setGridState', 'hidden'); //or 'hidden'
            var rowIds = $('#jqGridDemanda').getDataIDs();
            $.each(rowIds, function (index, rowId) {
                $('#jqGridDemanda').expandSubGridRow(rowId);
            }); 
            var rows = $('#jqGridDemanda').jqGrid('getDataIDs');
            console.log("qtd", rows);
            //percorre a grid de assuntos e faz alteracao de cores 
            for (a = 0; a <= rows.length; a++) {
                var situacao = $('#jqGridDemanda').jqGrid('getCell', rows[a], 'Situacao');
                console.log(situacao);
                if (situacao == 'Aberto') {
                    $('#jqGridDemanda').jqGrid('setCell', rows[a], 'Situacao', 'Aberto', { color: 'red' });
                    //$('#jqGridAssunto').jqGrid('setCell', rows[a], 'Situacao', 'Aberto', { color: 'green' });
                }
                if (situacao == 'Encerrado') {
                    $('#jqGridDemanda').jqGrid('setCell', rows[a], 'Situacao', 'Encerrado', { color: 'green' });
                }
                if (situacao == 'Suspenso') {
                    $('#jqGridDemanda').jqGrid('setCell', rows[a], 'Situacao', 'Suspenso', { color: 'orange' });
                }
            }
            
            $('#jqGridDemanda').jqGrid('filterToolbar',
                {
                    stringResult: true,
                    searchOnEnter: true,
                    searchOperators: true,
                    defaultSearch: "cn",
                    beforeClear: function() {
                        //alert("beforeClear");
                    },
                    beforeSearch: function() {
                        //alert("beforeSearch");
                    }
                });
        },
        //quando seleciona a linha permite alterar os campos e quando apertar entrer ira salvar
        onSelectRow: function () {
            var rowIdDem = $("#jqGridDemanda").jqGrid('getGridParam', 'selrow');
            var demanda = $("#jqGridDemanda").jqGrid('getCell', rowIdDem, 'DemandaId');
        },
        loadComplete: function (data) {
            
        },
        //chama funcao que faz edção
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        multiselect: false,
        subGrid: true,
        scrollOffset: 18,
        subGridRowExpanded: function (subgrid_id, row_id) {
            var subgrid_table_id, pager_id;
            var seleciona = row_id;
            console.log("Demanda aberto é:" + row_id + " é")
            var ID = row_id;
            //esconde as outras subgrids ao selecionar uma nova
            var expanded = jQuery("td.sgexpanded", "#jqGridDemandas")[0];
            $("#DemandaId").val(seleciona);
            //console.log(expanded.length);
            if (expanded) {
                setTimeout(function () {
                    $(expanded).trigger("click");
                }, 100);
            }
            console.log("Assunto aberto é:" + row_id + " é")
            subgrid_table_id = subgrid_id + "_t";
            pager_id = "p_" + subgrid_table_id;
            jQuery("#" + subgrid_id).html("<table id='" + subgrid_table_id + "' class='scroll'></table><div id='" + pager_id + "' class='scroll'></div>");
            jQuery("#" + subgrid_table_id).jqGrid({
                url: "/AcaoDemanda/GetAcaoDemandaSubGrid/" + row_id,
                datatype: 'json',
                mtype: 'POST',
                colNames: [
                    'AcaoDemandaId', 'ReuniaoId', 'DemandaId', '<i class="fa fa-calendar-check-o">&nbsp;</i>Feito', '<b><i class="fa fa-wrench">&nbsp;</i>Descritivo das Ações</b>',
                    'Data', 'Demandado', 'QuemInseriu','Observação', 'InseridoEm', 'Ação'
                ],
                colModel: [
                    { key: true, hidden: true, name: 'AcaoDemandaId', index: 'AcaoDemandaId', editable: false },
                    { key: false, hidden: true, name: 'ReuniaoId', index: 'ReuniaoId', editable: false },
                    { key: false, hidden: true, name: 'DemandaId', index: 'AssuntoId', editable: true },
                    {
                        key: false,
                        name: 'Feito',
                        index: 'Feito',
                        edittype: 'checkbox',
                        editable: true,
                        editoptions: { value: "true:false" },
                        editrules: { required: true },
                        formatter: "checkbox",
                        search: false,
                        width: 15
                    },
                    {
                        key: false,
                        name: 'Descricao',
                        index: 'Descricao',
                        width: 80,
                        editable: true,
                        edittype: 'textarea',
                        editoptions: { rows: 4 },
                        search: false
                    },
                    {
                        key: false,
                        name: 'Data',
                        index: 'Data',
                        hidden: false,
                        editable: true,
                        width: 20,
                        align: "center", sorttype: "date", frozen: true,
                        formatter: "date", formatoptions: { newformat: "d-m-y", reformatAfterEdit: true }, datefmt: "d-m-y",
                        searchoptions: {
                            sopt: ["le"], // or any other search operation
                            dataInit: initDateSearch
                        },
                        editoptions: {
                            sopt: ["le"], // or any other search operation
                            dataInit: initDateSearch,
                            datefmt: "d-m-y"
                        }
                    },
                    {
                        key: false,
                        hidden: false,
                        name: 'Demandado',
                        index: 'Demandado',
                        editable: true,
                        cellattr: function(rowId, tv, rawObject, cm, rdata) {
                            return 'style="white-space: normal;"';
                        }, // stype defines the search type control - in this case HTML select (dropdownlist)
                        stype: "select",
                        edittype: 'select',
                        //chama função userreuniao
                        searchoptions: {
                            dataUrl: "/UserReuniao/ListaUsuarioReuniao/" + (ID = $("#ReuniaoId").val()),
                            type: "GET",
                            contentType: 'application/json; charset=utf-8',
                            dataType: "json",
                            cache: false,
                            buildSelect: function(data) {
                                var response = jQuery.parseJSON(data), s = '<select class="Resp" >', i, l, ri;
                                s += '<option value="">--Todos--</option>';
                                console.log("Tamanho" + response.participantes.length)
                                console.log("json" + response.participantes)
                                if (response.participantes && response.participantes.length) {
                                    for (i = 0, l = response.participantes.length; i < l; i += 1) {
                                        ri = response.participantes[i];
                                        s += '<option value="' + ri + '">' + ri + '</option>';
                                    }
                                }
                                return s + '</select>';
                            }
                        },
                        editoptions: {
                            dataInit: function(element) {
                                $(element).width(120).select2({
                                    placeholder: "funcionario ou setor.",
                                    minimumInputLength: 0,
                                    closeOnSelect: true,
                                    escapeMarkup: function(markup) {
                                        return markup;
                                    },
                                    templateResult: function(data) {
                                        return data.html;
                                    },
                                    templateSelection: function(data) {
                                        return data.text;
                                    },
                                    multiple: false,
                                    ajax: {
                                        url: "/Demanda/PesquisaAutocompleteUser/",
                                        dataType: 'json',
                                        data: function(params) {
                                            console.log(params.term)
                                            return {
                                                term: params.term
                                            };
                                        },
                                        processResults: function(data) {
                                            debugger;
                                            return {
                                                results: $.map(data.autotexto,
                                                    function(usr) {
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
                                        cache: true
                                    }
                                });
                            }
                        },
                        width: 20
                    },
                    { key: false, hidden: true, name: 'QuemInseriu', index: 'QuemInseriu', editable: false },
                    {
                        key: false,
                        name: 'Observacao',
                        index: 'Observacao',
                        width: 80,
                        editable: true,
                        edittype: 'textarea',
                        editoptions: { rows: 4 },
                        search: false
                    },
                    {
                        key: false,
                        name: 'InseridoEm',
                        index: 'InseridoEm',
                        hidden: true,
                        editable: true,
                        formatter: "date",
                        formatoptions: { srcformat: "ISO8601Long", newformat: "Y/m/d" },
                        width: 39
                    },
                    {
                        name: "act",
                        template: "actions",
                        width: 100,
                        formatter: "actions",
                        formatoptions: {
                            keys: true,
                            addbutton: true,
                            editbutton: true,
                            delbutton: false,
                            addOption: {
                                onclickSubmit: function(options, rowid) {
                                    var rowKey = jQuery("#" + subgrid_table_id).jqGrid('getGridParam', 'selrow');
                                    var rowObject = jQuery("#" + subgrid_table_id).getRowData(rowKey);
                                    rowObject.DemandaId = $("DemandaId").val();
                                    debugger
                                    //rowObject.AssuntoId = rowid;                                    
                                    if (isNumber(rowid) == true) {
                                        console.log(rowObject)
                                        debugger
                                        AlterarAcaoDemanda(rowObject);
                                    } else {
                                        rowObject.AcaoDemandaId = 0;
                                        selRowId = $("#jqGridDemanda").jqGrid('getGridParam', 'selrow'),
                                            celValue = myGrid.jqGrid('getCell', selRowId, 'DemandaId');
                                        //rowObject.DemandaId = celValue;
                                        console.log("Acao a ser cadastrada" + rowObject)
                                        InsereAcaoDemanda(rowObject)
                                    }

                                }
                            },
                            delOptions: {                                
                            },
                            restoreAfterError: true,
                            serializeRowData: function(data) { return JSON.stringify(data); },
                            onSuccess: function(jqXHR) {
                                console.log(jqXHR.statusText);
                            },
                            onError: function(rowid, jqXHR, textStatus) {
                                console.log(jqXHR.statusText);
                            },

                            afterSave: function(rowid) {
                                var rowKey = jQuery("#" + subgrid_table_id).jqGrid('getGridParam', 'selrow');
                                //var celValue = jQuery("#" + subgrid_table_id).jqGrid('getCell', rowKey, 'DemandaId');
                                var rowObject = jQuery("#" + subgrid_table_id).getRowData(rowKey);
                                rowObject.ReuniaoId = $("#ReuniaoId").val();
                                console.log("Id do assunro" + seleciona)                                
                                rowObject.DemandaId = row_id;
                                
                                console.log(rowObject)
                                //SE O ID DO ASSUNTO FOR NUMERCIO É EDIÇÃO
                                if (isNumber(rowid) == true) {
                                    if (confirm("Deseja confirmar a alteração na ação?")) {
                                        AlterarAcaoDemanda(rowObject);
                                    }
                                } else {
                                    rowObject.AcaoDemandaId = 0;
                                    console.log("Acao a ser cadastrada" + rowObject)
                                    InsereAcaoDemanda(rowObject)
                                }
                                console.log(rowObject)

                            },
                        }
                    }
                ],
                rowNum: 10,
                pager: pager_id,
                rowList: [10, 20, 30, 40, 50],
                width: 1200,
                resizable: false,
                autowidth: false,
                height: 200,
                sortname: 'Id',
                sortorder: 'asc',
                height: 'auto',
                iconSet: "fontAwesome",
                viewrecords: false,
                pgbuttons: false,
                pginput: false,
                pgtext: "",
                caption: 'Ações da demanda',
                emptyrecords: 'Não há açoes para está demanda!',
                inlineEditing: {
                    keys: true, position: "afterSelected"
                },
                actionsNavOptions: {
                    edittitle: "Alterar assunto",
                    deltitle: "Remover o assunto",
                    savetitle: "Confirmar alteração",
                    canceltitle: "Cancelar edição",
                    deltitle: "Gerenc. Demandas(Plano de Ação)",
                    deletetitle: "Remover ação",                    
                    deleteicon: "fa fa-fw fa-trash-o",
                    custom: [
                        {
                            action: "delete", position: "last",
                            onClick: function (options) {
                                var rowKey = jQuery("#" + subgrid_table_id).jqGrid('getGridParam', 'selrow');                                
                                var celValue = jQuery("#" + subgrid_table_id).jqGrid('getCell', rowKey, 'AcaoDemandaId');
                                debugger;
                                RemoverAcaoInter(celValue)
                            }                        
                        }]
                },

                onSelectRow: function (id) {


                },
                loadComplete: function () {
                    $('#' + subgrid_table_id).closest("div.ui-jqgrid-titlebar").hide();
                    $('.ui-subgrid .ui-jqgrid-titlebar').hide();
                    $(".ui-jqgrid-titlebar", "#gview_subgridgrid").hide();
                    //$(".subgrid-data").css("display","none");
                },
                gridComplete: function () {
                    $('#' + subgrid_table_id).jqGrid('inlineNav', '#' + pager_id, {
                        add: true,
                        cloneToTop: true,
                        toppager: false,
                        addtext: "Nova Ação",
                        addicon: "fa fa-fw fa-plus",
                        edit: false,
                        editicon: "fa-pencil",
                        edittext: "Editar Ação",
                        save: false,
                        search: false,
                        refreshtext: "Atualizar",
                        view: false,
                        afterSave: function () {
                            debugger;
                            console.log("O item a ser inserido possui Id:" + id);
                            if (confirm("Confirmar a alteração no item?")) {                              
                                var rowKey = jQuery('#' + subgrid_table_id).jqGrid('getGridParam', 'selrow');                                
                                var rowObject = jQuery("#" + subgrid_table_id).getRowData(rowKey);
                                rowObject.ReuniaoId = $("#ReuniaoId").val();
                                console.log("Id do assunro" + seleciona)
                                rowObject.DemandaId = row_id;
                                console.log(rowObject)
                                AlterarAcaoDemanda(rowObject);
                            }
                        },
                        addParams: {
                            position: "afterSelected",
                            addRowParams: {
                                // the parameters of editRow used to edit new row
                                keys: true,
                                oneditfunc: function (rowid) {

                                },
                                aftersavefunc: function (rowid) {
                                    var rowKey = jQuery("#" + subgrid_table_id).jqGrid('getGridParam', 'selrow');
                                    //var celValue = jQuery("#" + subgrid_table_id).jqGrid('getCell', rowKey, 'DemandaId');
                                    var rowObject = jQuery("#" + subgrid_table_id).getRowData(rowKey);
                                    rowObject.ReuniaoId = $("#ReuniaoId").val();
                                    console.log("Id do assunro" + seleciona)
                                    rowObject.DemandaId = row_id;
                                    //rowObject.AcaoDemandaId = 0;
                                    console.log(rowObject)
                                    //SE O ID DO ASSUNTO FOR NUMERCIO É EDIÇÃO
                                    if (isNumber(rowid) == true) {
                                        if (confirm("Deseja confirmar a alteração na ação?")) {
                                            AlterarAcaoDemanda(rowObject);
                                        }
                                    } else {
                                        rowObject.AcaoDemandaId = 0;
                                        console.log("Acao a ser cadastrada" + rowObject)
                                        InsereAcaoDemanda(rowObject)
                                    }
                                    console.log(rowObject)
                                },
                            }
                        },
                        saveicon: "fa-floppy-o",
                        savetext: "Savar",
                        cancel: false,
                        cancelicon: "fa-ban",
                        canceltext: "Cancelar",
                        editParams: {
                            aftersavefunc: function (id) {
                                var rowKey = jQuery("#" + subgrid_table_id).jqGrid('getGridParam', 'selrow');
                                //var celValue = jQuery("#" + subgrid_table_id).jqGrid('getCell', rowKey, 'DemandaId');
                                var rowObject = jQuery("#" + subgrid_table_id).getRowData(rowKey);
                                rowObject.ReuniaoId = $("#ReuniaoId").val();
                                console.log("Id do assunro" + seleciona)
                                rowObject.DemandaId = row_id;
                                rowObject.AcaoDemandaId = 0;
                                console.log(rowObject)
                                //SE O ID DO ASSUNTO FOR NUMERCIO É EDIÇÃO
                                if (isNumber(rowid) == true) {
                                    if (confirm("Deseja confirmar a alteração na ação?")) {
                                        AlterarAcaoDemanda(rowObject);
                                    }
                                } else {
                                    console.log("Acao a ser cadastrada" + rowObject)
                                    InsereAcaoDemanda(rowObject)
                                }
                                
                            },
                            beforesavefunc: function (id) {
                                alert('antes')
                            },
                            keys: false,
                            oneditfunc: null,
                            successfunc: null,
                            //url: '/Assunto/Edit',          
                            errorfunc: null,
                            afterrestorefunc: null,
                            restoreAfterError: true,
                            mtype: "POST"
                        }
                    });
                    //customização de botoes
                    $("#" + subgrid_table_id).jqGrid('filterToolbar', {
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
                loadonce: true, //Pesquisa
                forceClientSorting: true,
                multiselect: false,
                subGrid: false,
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
    });
});
$(function () {
    //assunto filtro e botoes
    $('#jqGridAtaAssuntosItens').jqGrid('inlineNav', '#jqControlsAtaAssuntosItens', {
        add: true,
        cloneToTop: false,
        toppager: false,
        addtext: "Novo Item",
        addicon: "fa fa-fw fa-plus",
        edit: false,
        editicon: "fa-pencil",
        edittext: "Editar Item",
        save: false,
        search: false,
        refreshtext: "Atualizar",
        view: false,
        afterSave: function () {
            debugger;
            console.log("O item a ser inserido possui Id:" + id);
            if (confirm("Confirmar a alteração no item?")) {
                //$('#' + subgrid_table_id).jqGrid('inlineNav', '#' + pager_id, {

                var rowKey = $('#jqGridAtaAssuntosItens').jqGrid('getGridParam', 'selrow');
                var rowObject = $('#jqGridAtaAssuntosItens').getRowData(rowKey);
                console.log(rowObject)
                AlterarItemAssunto(rowObject);
            }
        },
        addParams: {
            position: "afterSelected",
            addRowParams: {
                // the parameters of editRow used to edit new row
                keys: true,
                oneditfunc: function (rowid) {

                },
                aftersavefunc: function (id) {
                    var rowKey = $('#jqGridAtaAssuntosItens').jqGrid('getGridParam', 'selrow');
                    var rowObject = $('#jqGridAtaAssuntosItens').getRowData(rowKey);
                    console.log(rowObject)

                    rowObject.ReuniaoId = $("#ReuniaoId").val();
                    rowObject.AssuntoId = seleciona;
                    rowObject.ItemAssuntoId = "-1";
                    InsereItemAssunto(rowObject);
                },
            }
        },
        saveicon: "fa-floppy-o",
        savetext: "Savar",
        cancel: false,
        cancelicon: "fa-ban",
        canceltext: "Cancelar",
        editParams: {
            aftersavefunc: function (id) {
                if (isNumber(id) == true) {
                    if (confirm("Confirmar a alteração no assunto?")) {
                        var rowKey = $('#jqGridAtaAssuntosItens').jqGrid('getGridParam', 'selrow');
                        var rowObject = $('#jqGridAtaAssuntosItens').getRowData(rowKey);
                        console.log(rowObject)
                        console.log("O assunto a ser editado:" + $("#AssuntoId").val());
                        rowObject.AssuntoId = $("#AssuntoId").val();
                        AlterarItemAssunto(rowObject);
                    }
                }
            },
            beforesavefunc: function (id) {
                alert('antes')
            },
            keys: false,
            oneditfunc: null,
            successfunc: null,
            //url: '/Assunto/Edit',          
            errorfunc: null,
            afterrestorefunc: null,
            restoreAfterError: true,
            mtype: "POST"
        }
    });
    $('#jqGridAtaAssuntosItens').jqGrid('filterToolbar', {
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
    }).navButtonAdd('#jqControlsAtaAssuntosItens',
        {
            caption: "Novo Assunto",
            toppager: false,
            cloneToTop: true,
            buttonicon: "fa fa-plus",
            onClickButton: function () {
                debugger
                InsereAssuntoInterInter();
            },
            position: "last"
        }).navButtonAdd('#jqControlsAtaAssuntosItens',
        {
            caption: "Alterar",
            toppager: false,
            cloneToTop: true,
            buttonicon: "fa fa-pencil",
            onClickButton: function () {
                EditarAssuntoInter()
            },
            position: "last"
        }).navButtonAdd('#jqControlsAtaAssuntosItens',
        {
            caption: "Remover",
            toppager: false,
            cloneToTop: true,
            buttonicon: "fa fa-trash-o",
            onClickButton: function () {
                RemoverAssuntoInter();
            },
            position: "last"
        });
    //demanda filtros e botoes
    $('#jqGridDemanda').jqGrid('setGroupHeaders', {
        useColSpanStyle: false,
        groupHeaders: [
            { startColumnName: 'Quem', numberOfColumns: 2, titleText: '<em><i class="fa fa-users">&nbsp;</i><b>Envolvidos</b></em>' }
        ]
    });
    $('#jqGridReuniaoUser').jqGrid('navGrid',
            '#jqControlsReuniaoUser',
            {
                cloneToTop: true,
                del: false,
                add: false,
                edit: false,
                search: false
            },
            {},
            {},
            {},
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
        .navButtonAdd('#jqControlsReuniaoUser',
            {
                caption: "Criar",
                toppager: false,
                cloneToTop: true,
                buttonicon: "fas fa-plus",
                onClickButton: function() {
                    InserirReuniaoInter()
                },
                position: "last"
            }).navButtonAdd('#jqControlsReuniaoUser',
            {
                caption: "Alterar",
                toppager: false,
                cloneToTop: true,
                buttonicon: "fa fa-lg fa-fw fa-pencil",
                onClickButton: function() {
                    var rowId = "";
                    rowId = $("#ReuniaoId").val();
                    EditarReuniaoInter(rowId)                   

                },
                position: "last"
            }).navButtonAdd('#jqControlsReuniaoUser',
            {
                caption: "Remover",
                toppager: false,
                cloneToTop: true,
                buttonicon: "fa fa-fw fa-trash-o",
                onClickButton: function () {
                    RemoverReuniaoInter()
                },
                position: "last"
            });



    
    $('#jqGridReuniaoUser').jqGrid('filterToolbar', {
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
    
    initDateEdit = function (elem) {
        $(elem).datepicker({
            dateFormat: "dd-M-yy",
            autoSize: true,
            changeYear: true,
            changeMonth: true,
            showButtonPanel: true,
            showWeek: true
        });
    },
    initDateSearch = function (elem) {
        setTimeout(function () {
            initDateEdit(elem);
        }, 100);
    };
    $(".ui-pg-button> .fa fa-lg fa-fw fa-pencil").addClass('ui-state-disabled');

    $("#loading").hide();
    $('#jqGridDemanda').jqGrid('setGridState', 'hidden');



})