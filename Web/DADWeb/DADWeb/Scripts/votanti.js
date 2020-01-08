var restrict = function (tb) {
  tb.value = tb.value.replace(/[^a-zA-Z -]/g, '');
};

function submitForm() {
  var urlPost = 'https://0cxr9anbqk.execute-api.eu-central-1.amazonaws.com/dev/insert-voter-right';
  
  var cnp = document.getElementById("cnp").value;
  var judet = document.getElementById("judet").value;


  myString = JSON.stringify({
    CNP: cnp,
    Judet: judet
  });

  console.log(myString);

  $.ajax({
    type: 'POST',
    url: urlPost,
    data: myString,
    contentType: "application/json",

    success: function (data) {
      console.log("Am inserat votantul!");
    }
  });

  setTimeout(submitForm, 15000);
  window.location.reload();
}

function veziVotanti() {
  window.location.replace("/GetVotanti");
}