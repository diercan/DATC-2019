getData();

function getData() {

  var API_URL_GET = 'https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/get-voters-right';
  const table = document.getElementById("votanti");
  let dataHtml = '';

  $.ajax({

    type: 'GET',
    url: API_URL_GET,
    contentType: "application/json",

    success: function (data) {
      console.log("Am luat datele!");
      var entries = data.body;
      listOfVotanti = JSON.parse(entries);
      console.log(listOfVotanti);

      for (let votanti of listOfVotanti) {
        dataHtml += '<tr><td>' + votanti.CNP + '</td><td>' + votanti.Judet + '</td></tr>';
      }
      table.innerHTML = dataHtml;
    }
  });
}