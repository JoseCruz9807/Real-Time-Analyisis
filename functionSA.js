// UDF de ejemplo que devuelve la suma de dos valores.


function main(x) {
    var date = new Date(new Date().toLocaleString('en-US', { timeZone: 'America/Mexico_City' }))
      var dateStr =
    ("00" + (date.getMonth() + 1)).slice(-2) + "/" +
    ("00" + date.getDate()).slice(-2) + "/" +
    date.getFullYear() + " " +
    ("00" + date.getHours()).slice(-2) + ":" +
    ("00" + date.getMinutes()).slice(-2) + ":" +
    ("00" + date.getSeconds()).slice(-2)
      return dateStr
  }