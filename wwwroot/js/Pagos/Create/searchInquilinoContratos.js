const p_aviso = document.getElementById("aviso");
const text_dni = document.getElementById("DniInquilino");
const btn_search = document.getElementById("searchBtn");

const select_contratos = document.getElementById("selectContratos");
let option_default = document.getElementById("default");
const fecha = document.getElementById("fecha");
const monto = document.getElementById("monto");
const tipo = document.getElementById("tipo");
const enviar = document.getElementById("enviar");

const expresiones = {
    usuario: /^[a-zA-Z0-9\_\-]{4,16}$/, // Letras, numeros, guion y guion_bajo
    nombre: /^[a-zA-ZÀ-ÿ\s]{1,40}$/, // Letras y espacios, pueden llevar acentos.
    password: /^.{4,12}$/, // 4 a 12 digitos.
    correo: /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/,
    dni: /^\d{8,14}$/ // 8 a 14 numeros.
}

text_dni.addEventListener("keyup", (e) => {
    if (e.keyCode == 13) {
        searchInquilinoContratos();
    }

    if (!expresiones.dni.test(e.target.value.trim())) {
        text_dni.classList.add("border");
        text_dni.classList.add("border-warning");

        p_aviso.innerText = `Ingrese un número de entre 8 y 14 dígitos`;
    } else {
        text_dni.classList.remove("border-warning");
        text_dni.classList.add("border-success");
        p_aviso.innerText = "";
    }
})

async function searchInquilinoContratos() {

    select_nodes = select_contratos.childNodes;
    for (let child of select_nodes) {
        select_contratos.removeChild(child);
    }
    select_contratos.setAttribute("disabled", "disabled");
    fecha.setAttribute("disabled", "disabled");
    monto.setAttribute("disabled", "disabled");
    tipo.setAttribute("disabled", "disabled");
    enviar.setAttribute("disabled", "disabled");


    let dni = text_dni.value.trim();

    try {
        const json = await fetch(`/Pagos/Inquilino/${dni}`);
        const response = await json.json();

        console.log("json parseado");
        console.log(response);

        if (!response.inquilino.id) {
            p_aviso.innerText = "No existe un inquilino con DNI: " + dni;
        } else {
            p_aviso.innerText = "Inquilino encontrado: " + response.inquilino.nombre;
            let contratos = response.contratos;
            if (contratos.length) {
                for (let c of contratos) {
                    let option = document.createElement("option");
                    option.value = c.id;
                    option.textContent = c.inmueble.direccion;
                    select_contratos.appendChild(option);
                }
                select_contratos.removeAttribute("disabled");
                select_contratos.focus();
                fecha.removeAttribute("disabled");
                monto.removeAttribute("disabled");
                tipo.removeAttribute("disabled");
                enviar.removeAttribute("disabled");

            } else {
                p_aviso.innerText = `El inquilino ${response.inquilino.nombre} no tiene contratos`;
            }
        }

    } catch (error) {
        p_aviso.innerText = "Debe ingresar un dni válido";
    }
}