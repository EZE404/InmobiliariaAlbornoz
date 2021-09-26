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
    } else {
        p_aviso.innerText = "";
        btn_buscar.removeAttribute("disabled");
        table.clear();
        table.draw();
    }
})

btn_buscar.addEventListener("click", async () => {

    let field_desde = new Date(date_desde.value).getTime();
    let field_hasta = new Date(date_hasta.value).getTime();

    p_aviso.innerText = "";
    table.clear();
    table.draw();

    if (!date_desde.value || !date_hasta.value) {
        p_aviso.innerText = "Ingrese un rango de fechas v&aacutelidas.";
        return;
    };

    if (field_desde > field_hasta) {
        p_aviso.innerText = "No puede elegir una fecha de fin anterior a la fecha de inicio.";
        return;
    }

    let data = new URLSearchParams();
    data.append("desde", date_desde.value);
    data.append("hasta", date_hasta.value);

    try {

        let res = await fetch("/Contratos/GetValidsByDates", {
            method: "POST",
            body: data
        });

        let parsed = await res.json();

        if (Object.keys(parsed).length > 0) {

            for (c of parsed) {

                let inmueble = "<a href=\"/Inmuebles/Details/" + c.inmueble.id + "\" /> Ver Inmueble " + c.idInmueble + "</a>";
                let inquilino = "<a href=\"/Inquilinos/Details/" + c.inquilino.id + "\" />" + c.inquilino.nombre + "</a>";
                let detalles = "<i class=\"fa fa-info-circle\"></i> <a href=\"/Contratos/Details/" + c.id + "\" />Detalles</a>"
                let pagos = "<i class=\"far fa-credit-card\"></i> <a href=\"/Contratos/Pagos/" + c.id + "\" />Pagos</a>"
                let actions = detalles + "<br/>" + pagos;
                let from = new Date(c.desde).toLocaleDateString();
                let to = new Date(c.hasta).toLocaleDateString();

                table.row.add(
                    [
                        inmueble,
                        inquilino,
                        from,
                        to,
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