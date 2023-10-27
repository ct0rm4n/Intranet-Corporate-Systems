function myIP() {

    var xmlhttp = new XMLHttpRequest();
    xmlhttp.open("GET", 'http://meuip.com/api/meuip.php');
    xmlhttp.send();
    xmlhttp.onload = function (e) {
        console.log(xmlhttp.response);
    }
}
