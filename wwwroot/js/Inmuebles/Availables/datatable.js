const p_aviso = document.getElementById("p_aviso");
const btn_buscar = document.getElementById("btn_buscar");
const tbody = document.getElementById("tbody");
const date_desde = document.getElementById("desde");
const date_hasta = document.getElementById("hasta");

date_hasta.addEventListener("change", (e) => {

    if (new Date(date_desde.value) > new Date(e.target.value)) {
        p_aviso.innerText = "No puede elegir una fecha final anterior a la fecha de inicio.";
        btn_buscar.setAttribute("disabled", "disabled");
        table.clear();
        table.draw();
    } else {
        p_aviso.innerText = "";
        btn_buscar.removeAttribute("disabled");
    }
})

date_desde.addEventListener("change", (e) => {

    if (new Date(e.target.value) > new Date(date_hasta.value)) {
        p_aviso.innerText = "No puede elegir una fecha de inicio posterior a la fecha final.";
        btn_buscar.setAttribute("disabled", "disabled");
        table.clear();
        table.draw();
    } else {
        p_aviso.innerText = "";
        btn_buscar.removeAttribute("disabled");
    }
})

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
    p_aviso.innerText = "No puede elegir una fecha de fin anterior a la fecha de inicio.";
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

      if (Object.keys(parsed).length > 0) {

    for (i of parsed) {

      let propietario = "<a href=\"/Propietarios/Details/" + i.propietario.id + "\" />" + i.propietario.nombre + "</a>";
        let detalles = "<i class=\"fa fa-info-circle\"></i> <a href=\"/Inmuebles/Details/" + i.id + "\" />Detalles</a>"
        let contrato = "<i class=\"fas fa-file-signature\"></i> <a href=\"/Contratos/Create/" + i.id + "\" />Contrato</a>"
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
      } else {
          p_aviso.innerText = "No se obtuvieron resultados.";
      }
  } catch (e) {
    p_aviso.innerText = "Ocurrió un problema. Intenta nuevamente."
  }

});