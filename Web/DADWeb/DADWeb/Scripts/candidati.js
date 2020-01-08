var listOfCandidati;
getData();

function getData() {

  var API_URL_GET = 'https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/show-info';
  const table = document.getElementById("candidati");
  let dataHtml = '';

  $.ajax({

    type: 'GET',
    url: API_URL_GET,
    contentType: "application/json",

    success: function (data) {
      console.log("Am luat datele!");
      var entries = data.body;
      listOfCandidati = JSON.parse(entries);
      console.log(listOfCandidati);

      for (let candidat of listOfCandidati) {
        dataHtml += '<tr><td>' + candidat.NumeCandidat + '</td><td>' + candidat.Partid + '</td><td>' + candidat.Proiect + '</td></tr>' ;
      }
      table.innerHTML = dataHtml;
    }
  });
}