var restrict = function (tb) {
  tb.value = tb.value.replace(/[^a-zA-Z -]/g, '');
};

function submitForm() {
  var checkboxValue = 'false';
  var tipAlegeri;
  var urlPost = 'https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/insert-info';
  var urlPostTipAlegeri = 'https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/insert-voting-type';
  var myString = [];
  var myStringForSecondApi = [];

  //var name = document.getElementById("name").value;
  //var prenume = document.getElementById("prenume").value;
  var numeCandidat = document.getElementById("name").value + " " + document.getElementById("prenume").value;
  var partid = document.getElementById("partid").value;
  var detalii = document.getElementById("detalii").value;
  var stopCandidati = document.getElementById('stop');

  var alegeriParlamentare = document.getElementsByName("alegeriParlamentare");
  var alegeriPrezidentiale = document.getElementsByName("alegeriPrezidentiale");
  var alegeriLocale = document.getElementsByName("alegeriLocale");
  var alegeriEuropene = document.getElementsByName("alegeriEuropene");

  if (numeCandidat && partid && detalii) {
    if (stopCandidati.checked) {
      checkboxValue = 'true';
    }
    
    if (alegeriParlamentare.length == 1)
      tipAlegeri = '1';
    if (alegeriPrezidentiale.length == 1)
      tipAlegeri = '2';
    if (alegeriLocale.length == 1)
      tipAlegeri = '3';
    if (alegeriEuropene.length == 1)
      tipAlegeri = '4';

    myString = JSON.stringify({
      Partid: partid,
      NumeCandidat: numeCandidat,
      Proiect: detalii
    });

    myStringForSecondApi = JSON.stringify({
      StopCandidati: checkboxValue,
      TipAlegeri: tipAlegeri
    });

    console.log(myStringForSecondApi);

    $.ajax({
      type: 'POST',
      url: urlPost,
      data: myString,
      contentType: "application/json",

      success: function (data) {
        console.log("Am trimis datele!");
      }
    });

    $.ajax({
      type: 'POST',
      url: urlPostTipAlegeri,
      data: myStringForSecondApi,
      contentType: "application/json",

      success: function (data) {
        console.log("Am trimis datele!");
      }
    });

    //setTimeout(submitForm, 15000);

    window.location.reload();
  }
}
  