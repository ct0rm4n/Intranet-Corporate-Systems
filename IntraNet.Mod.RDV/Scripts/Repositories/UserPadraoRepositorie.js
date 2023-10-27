function CreateAvatar() {
	var id = $("#ImageFile").val();
    console.log(id);    
    var file = document.getElementById('ImageFile').files[0];
  
    var fd = new FormData();
    fd.append("ImageFile", file);
    fd.append("ImagePath",id)
    
    debugger;
	$.ajax({
		type: "POST",
		url: "/Manage/InserirAvatarUser/",
		processData: false,
		contentType: false,
        data: fd,
		success: function (result) {
			if (result.success == true) {
				debugger;
				$('#result').html("<div class='alert alert-success alert-dismissible' role='alert' align='left'><span></br>" + result.message + "</br></span><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button></div>");
			}
			else {
			    debugger;
				$('#result').html("<div class='alert alert-danger alert-dismissible col-lg-10' role='alert' align='left'><i class='glyphicon glyphicon-exclamation-sign'>&nbsp;</i><span>Ocorreu um erro - Verifique o formulário</span></br></br><span>" + result.message + "</span><button type='button' class='close' data-dismiss='alert' aria-label='Close'><span aria-hidden='true'>×</span></button></div>");
			}
		},
		error: function (er) {
		    debugger;
			alert("Pcorreu um erro, verifique o formulário e certifique-se que nao esta inserindo uma imagem mais de uma vez");
		}
	});
}