checkIfStartVot();

function checkIfStartVot() {
  var myUrl = 'https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/check-voting-state';
  var checkStartVot = "false";

  $.ajax({

    type: 'GET',
    url: myUrl,
    contentType: "application/json",

    success: function (data) {
      console.log("Am luat datele!");
      var entries = data.body;
      var myData = JSON.parse(entries);

      for (let data of myData) {
        checkStartVot = data.State;
      }

      if (checkStartVot == "false") {
        document.getElementById("startVotTrue").style.display = 'none';
      }
      else {
        document.getElementById("startVotFalse").style.display = 'none';
      }

      chartData();
    }
  });
}

function chartData() {
  var myUrlCharData = 'https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/show-info';
  var myChartInfo = [];
  var i = 0;
  var allVotes = 0;
  let dataHtml = '';
  const table = document.getElementById("candidati");

  $.ajax({

    type: 'GET',
    url: myUrlCharData,
    contentType: "application/json",

    success: function (data) {
      console.log("Am luat datele!");
      var entries = data.body;
      var myData = JSON.parse(entries);

      for (let data of myData) {
        myChartInfo[i] = { 'label': data.NumeCandidat, 'y': parseInt(data.Voturi) };
        allVotes += parseInt(data.Voturi);
        i++;
      }

      var chart = new CanvasJS.Chart("chartContainer", {
        theme: "light1", 
        animationEnabled: true,	
        //axisY: {
        //  valueFormatString: "0'%'"
        //},
        data: [
          {
            // Change type to "bar", "area", "spline", "pie",etc.
            type: "column",
            dataPoints: myChartInfo
          }
        ]
      });
      chart.render();

      for (let candidat of myData) {
        dataHtml += '<tr><td>' + candidat.NumeCandidat + '</td><td>' + (candidat.Voturi * 100 / allVotes) + "%" +'</td></tr>';
      }
      table.innerHTML = dataHtml;
    },
    error: function (jqXHR, textStatus, errorThrown) {
      alert('error');
    }
  });
}

