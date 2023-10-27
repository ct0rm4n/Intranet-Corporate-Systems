$(document).ready(function () {
    function split(val) {
        return val.split(/,\s*/);
    }

    function extractLast(term) {
        return split(term).pop();
    }

    $('#Quem').select2({
        placeholder: "Selecione o funcionario...",
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

    $('#Demandado').select2({
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
    
    $('#Responsavel').select2({
        placeholder: "Selecione o funcionario...",
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
});
function SelecionaUsuarioDemanda(id) {
    debugger;
    id = id.toString();
    console.log(id);
    
    $.ajax({
        type: "GET",
        url: "/Demanda/SelecionaDemandado/" + id,
        data: id,
        contentType: "application/x-www-form-urlencoded;charset=UTF-8",
        success: function (result) {
            debugger;
            $('#DemandadoImg').attr('src', '');
            console.log(result);
            debugger;
            console.log("Possui imagem" + result.imagem);
            $('#DemandadoImg').attr('src', result.imagem);    
        },
        error: function (result) {
            debugger;
            $('#DemandadoImg').attr('src', '/Images/Avatar/demandado.jpg');
        }
    });
}
function tooltipDemandaLegenda() {
    debugger
    //$(this).tooltip({ html: true });
    $("#myModal #DemandaAjuda").popover();
    
}
function split(val) {
    return val.split(/,\s*/);
}
function extractLast(term) {
    return split(term).pop();
}
function SelecionaUsuariosDemanda(request) {
    debugger;
    console.log(request);
    
    if (request!=null) {
        request =='todos';
    }
    
}
function VisualizaDemanda(id) {
    debugger;
    console.log(id);
    $("#loading").show();
    //var val = $("#CategoriasId").val();
    $("#MeuModal").load("/Demanda/EditAcoes/" + id, function () {
        debugger;
        $("#loading").hide();
        $('#myModal').modal("show");
        //$('#AlertReuniao').hide();
        console.log("load");
    });
}

function EditarDemanda(id) {
    $('#myModal').modal('hide');
    $("#loading").show();
    $("#MeuModal").load("/Demanda/EditAcoes/" + id,
        function (data) {
            debugger;
            $("#MeuModal").html(data);
            $("#loading").hide();
            $(".modal-body,  #modal-ata").css("width", 500);
            $('#myModal').modal("show");
        });
    return [true];
}

function InsereDemanda(id) {
    $('#myModal').modal('hide');
    $("#loading").show();
    $("#MeuModal").load("/Demanda/Create/" + id,
        function () {
            $("#loading").hide();
            $(".modal-body,  #modal-ata").css("width", 1200);
            $('#myModal').modal("show");
        });
}


function PesquisaAutocompleteUser(texto) {
    debugger;
    console.log(texto)
    if (texto == "" || texto == " " || texto == "todos") {
         texto = "todos"
    }
    debugger;
   
}

function ListaDemandaDemandao(reuniaoids) {
    //var situacao = $("#Situacao").val();
    console.log(reuniaoids);
    $(reuniaoids).select2({
        placeholder: "Selecione o funcionario...",
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
            cache: true
        }
    });
       
}
function FiltrarDemandaPorItem(searchFiler,qtddemandas) {
    if (searchFiler != null && searchFiler != "" && qtddemandas > 0) {
        if (searchFiler == 0 || searchFiler == "" && searchFiler == null) {
            $("#jqGridDemanda")[0].p.search = false;
            $.extend($("#jqGridDemanda")[0].p.postData, { filters: "" });
        }
        f = { groupOp: "OR", rules: [] };
        f.rules.push({ field: "Assunto", op: "cn", data: searchFiler });
        //f.rules.push({ field: "ItemAssunto", op: "cn", data: searchFilerAssunto });
        $("#jqGridDemanda")[0].p.search = true;
        $.extend($("#jqGridDemanda")[0].p.postData, { filters: JSON.stringify(f) });

        $("#gs_jqGridDemanda_Assunto").val(searchFiler)

        //$("#jqGridAtaAssuntosItens").jqGrid('setGridState', 'hidden');
        //$("#jqGridDemanda").jqGrid('setGridState', 'visible');
    }
}
