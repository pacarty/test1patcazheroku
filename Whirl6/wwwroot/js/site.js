function loadPage() {
    document.getElementById("thing").innerHTML = "ballso";

    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            document.getElementById("thing").innerHTML = this.responseText;
        }
    };

    xhttp.open("GET", "api/Items/GetItems");
    xhttp.send();
}

function postThing() {
    var answer = document.getElementById("sometext").value;

    var data = new FormData();
    data.append("name", answer);
    data.append("isComplete", true);

    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            console.log(this.responseText);
        }
    };

    xhttp.open("POST", "api/Items/PostItem");
    xhttp.send(data);
}