const p_aviso = document.getElementById("p_aviso");
const btn_buscar = document.getElementById("btn_buscar");
const tbody = document.getElementById("tbody");
const date_desde = document.getElementById("desde");
const date_hasta = document.getElementById("hasta");

btn_buscar.addEventListener("click", async () => {

  let field_desde = new Date(date_desde.value).getTime();
  let field_hasta = new Date(date_hasta.value).getTime();

  p_aviso.innerText = "";
  table.clear();
  table.draw();

  if (!date_desde.value || !date_hasta.value) {
    p_aviso.innerText = "Ingrese un rango de fechas válidas";
    return;
  };

  if(field_desde > field_hasta) {
    p_aviso.innerText = "No puede elegir una fecha de fin anterior a la fecha de inicio";
    return;
  }

  let data = new URLSearchParams();
  data.append("desde", date_desde.value);
  data.append("hasta", date_hasta.value);

  try {

    let res = await fetch("/Inmuebles/GetAvailablesByDates", {
      method: "POST",
      body: data
    });

    let parsed = await res.json();



    for (i of parsed) {

      let propietario = "<a href=\"/Propietarios/Details/" + i.propietario.id + "\" />" + i.propietario.nombre + "</a>";
      let detalles = "<a href=\"/Inmuebles/Details/" + i.id + "\" /><i class=\"fa fa-info-circle\"></i> Detalles</a>"
      let contrato = "<a href=\"/Contratos/Create/" + i.id + "\" /><i class=\"fas fa-file-signature\"></i> Contrato</a>"
      let actions = detalles+"<br/>"+contrato;


      table.row.add(
        [
          i.direccion,
          i.tipoNombre,
          i.usoNombre,
          i.ambientes,
          propietario,
          actions
        ]
      );
    }

    table.draw();

  } catch (e) {
    p_aviso.innerText = "Ocurrió un problema. Intenta nuevamente"
  }

});