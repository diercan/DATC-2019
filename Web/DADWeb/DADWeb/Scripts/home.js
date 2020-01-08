var myIndex1 = 0;
var myIndex2 = 0;
var myIndex3 = 0;
var myIndex4 = 0;
var myIndex5 = 0;
var myIndex6 = 0;
var urlGet = 'https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/get-voting-type';
var stopCandidati;
var tipAlegeri;
var countDownDate = new Date("Jan 9, 2020 21:00:00").getTime();
carousel();
getData();
checkIfStartVot();

document.getElementById("startVot").style.display = 'none';

function getData() {
  $.ajax({

    type: 'GET',
    url: urlGet,
    contentType: "application/json",

    success: function (data) {
      var entries = data.body;
      var myData = JSON.parse(entries);

      for (let data of myData) {
        stopCandidati = data.StopCandidati;
        tipAlegeri = data.TipAlegeri;
      }

      console.log(myData);

      if (tipAlegeri == "0") {
        document.getElementById("alegeriParlamentare").style.display = 'block';
        document.getElementById("alegeriPrezidentiale").style.display = 'block';
        document.getElementById("alegeriLocale").style.display = 'block';
        document.getElementById("alegeriEuropene").style.display = 'block';

        if (stopCandidati == "false") {
          document.getElementById("alegParlamentare").style.display = 'none';
          document.getElementById("alegPrezidentiale").style.display = 'none';
          document.getElementById("alegLocale").style.display = 'none';
          document.getElementById("alegEuropene").style.display = 'none';
        }
      }
      else {
      if (tipAlegeri != 1) {
        document.getElementById("alegeriParlamentare").style.display = 'none';
        document.getElementById("alegParlamentare").style.display = 'none';
      }
      if (tipAlegeri != 2) {
        document.getElementById("alegeriPrezidentiale").style.display = 'none';
        document.getElementById("alegPrezidentiale").style.display = 'none';
      }
      if (tipAlegeri != 3) {
        document.getElementById("alegeriLocale").style.display = 'none';
        document.getElementById("alegLocale").style.display = 'none';
      }
      if (tipAlegeri != 4) {
        document.getElementById("alegeriEuropene").style.display = 'none';
        document.getElementById("alegEuropene").style.display = 'none';
      }

      if (stopCandidati == "true") {
        document.getElementById("alegeriParlamentare").style.display = 'none';
        document.getElementById("alegeriPrezidentiale").style.display = 'none';
        document.getElementById("alegeriLocale").style.display = 'none';
        document.getElementById("alegeriEuropene").style.display = 'none';
        document.getElementById("startVot").style.display = 'block';
      }
      }
    }
  });
}

function carousel() {
  var i, j, k, a, b, c;
  var x = document.getElementsByClassName("mySlides1");
  var y = document.getElementsByClassName("mySlides2");
  var z = document.getElementsByClassName("mySlides3");
  var m = document.getElementsByClassName("mySlides4");
  var n = document.getElementsByClassName("mySlides5");
  var p = document.getElementsByClassName("mySlides6");

  for (i = 0; i < x.length; i++) {
    x[i].style.display = "none";
  }

  for (j = 0; j < y.length; j++) {
    y[j].style.display = "none";
  }

  for (k = 0; k < z.length; k++) {
    z[k].style.display = "none";
  }

  for (a = 0; a < m.length; a++) {
    m[a].style.display = "none";
  }

  for (b = 0; b < n.length; b++) {
    n[b].style.display = "none";
  }

  for (c = 0; c < p.length; c++) {
    p[c].style.display = "none";
  }

  myIndex1++;
  myIndex2++;
  myIndex3++;
  myIndex4++;
  myIndex5++;
  myIndex6++;

  if (myIndex1 > x.length) { myIndex1 = 1; }
  if (myIndex2 > y.length) { myIndex2 = 1; }
  if (myIndex3 > z.length) { myIndex3 = 1; }
  if (myIndex4 > m.length) { myIndex4 = 1; }
  if (myIndex5 > n.length) { myIndex5 = 1; }
  if (myIndex6 > p.length) { myIndex6 = 1; }

  x[myIndex1 - 1].style.display = "block";
  y[myIndex2 - 1].style.display = "block";
  z[myIndex3 - 1].style.display = "block";
  m[myIndex4 - 1].style.display = "block";
  n[myIndex5 - 1].style.display = "block";
  p[myIndex6 - 1].style.display = "block";

  setTimeout(carousel, 2000); // Change image every 2 seconds
}

function startVoting() {
  var urlPost = 'https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/start-vot';

  myString = JSON.stringify({
    State: "true"
  });

  $.ajax({
    type: 'POST',
    url: urlPost,
    data: myString,
    contentType: "application/json",

    success: function (data) {
      console.log("S-a inceput Votul!");
      document.getElementById("startVot").style.display = 'none';
      checkIfStartVot();
    }
  });

  document.getElementById("startVot").style.display = 'none';
  var x = setInterval(function () {
    // Get today's date and time
    var now = new Date().getTime();

    // Find the distance between now and the count down date
    var distance = countDownDate - now;

    // Time calculations for days, hours, minutes and seconds
    var days = Math.floor(distance / (1000 * 60 * 60 * 24));
    var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((distance % (1000 * 60)) / 1000);

    // Output the result in an element with id="demo"
    document.getElementById("countdown").innerHTML = days + "d " + hours + "h " + minutes + "m " + seconds + "s ";

    // If the count down is over, write some text 
    if (distance < 0) {
      clearInterval(x);
      document.getElementById("countdown").innerHTML = "EXPIRED";
    }
  }, 1000);
}

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

      if (checkStartVot == "true") {
        document.getElementById("startVot").style.display = 'none';
        var x = setInterval(function () {
          // Get today's date and time
          var now = new Date().getTime();

          // Find the distance between now and the count down date
          var distance = countDownDate - now;

          // Time calculations for days, hours, minutes and seconds
          var days = Math.floor(distance / (1000 * 60 * 60 * 24));
          var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
          var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
          var seconds = Math.floor((distance % (1000 * 60)) / 1000);

          // Output the result in an element with id="demo"
          document.getElementById("countdown").innerHTML = days + "d " + hours + "h " + minutes + "m " + seconds + "s ";

          // If the count down is over, write some text 
          if (distance < 0) {
            clearInterval(x);
            document.getElementById("countdown").innerHTML = "EXPIRED";
          }
        }, 1000);
      }
      else {
        document.getElementById("countdown").style.display = 'none';
      }
    },
    error: function (jqXHR, textStatus, errorThrown) {
      alert('error');
    }  
  });

  if (checkStartVot == "true") {
    document.getElementById("startVot").style.display = 'none';
  }
  else {
    document.getElementById("startVot").style.display = 'block';
  }
}


