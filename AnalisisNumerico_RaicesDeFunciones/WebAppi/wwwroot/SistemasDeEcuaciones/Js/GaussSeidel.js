function generarMatriz() {
    const n = parseInt(document.getElementById("size").value);
    const container = document.getElementById("matrix");
    container.innerHTML = "";

    const tabla = document.createElement("table");
    tabla.classList.add("matriz");

    // ---- THEAD: x, y, z, w, ... , TI ----
    const thead = document.createElement("thead");
    const trHead = document.createElement("tr");

    const nombres = ["x", "y", "z", "w", "v"]; // primeros 4 como en tu diseño
    for (let j = 0; j < n; j++) {
        const th = document.createElement("th");
        th.textContent = nombres[j] ?? `x${j + 1}`; // si hay más de 4: x5, x6...
        trHead.appendChild(th);
    }

    const thTi = document.createElement("th");
    thTi.textContent = "TI";
    thTi.classList.add("ti-header");
    trHead.appendChild(thTi);

    thead.appendChild(trHead);
    tabla.appendChild(thead);

    // ---- TBODY con inputs ----
    const tbody = document.createElement("tbody");

    for (let i = 0; i < n; i++) {
        const tr = document.createElement("tr");

        // coeficientes A
        for (let j = 0; j < n; j++) {
            const td = document.createElement("td");
            const input = document.createElement("input");
            input.type = "number";
            input.step = "any";
            input.id = `a-${i}-${j}`;
            td.appendChild(input);
            tr.appendChild(td);
        }

        // término independiente b
        const tdTi = document.createElement("td");
        tdTi.classList.add("ti-cell");
        const inputTi = document.createElement("input");
        inputTi.type = "number";
        inputTi.step = "any";
        inputTi.id = `b-${i}`;
        tdTi.appendChild(inputTi);
        tr.appendChild(tdTi);

        tbody.appendChild(tr);
    }

    tabla.appendChild(tbody);
    container.appendChild(tabla);
}

function calcular() {
    const size = parseInt(document.getElementById("size").value);
    const A = [];
    const b = [];

    for (let i = 0; i < size; i++) {
        const fila = [];
        for (let j = 0; j < size; j++) {
            const valor = parseFloat(document.getElementById(`a-${i}-${j}`).value) || 0;
            fila.push(valor);
        }
        A.push(fila);

        const terminoIndep = parseFloat(document.getElementById(`b-${i}`).value) || 0;
        b.push(terminoIndep);
    }

    const tolerancia = parseFloat(document.getElementById("tolerance").value);
    const iteraciones = parseInt(document.getElementById("iterations").value);

    const datos = {
        A,
        b,
        Tolerancia: tolerancia,
        Iteraciones: iteraciones
    };


    const resultado = document.getElementById("resultado");
    resultado.innerHTML = `<p>Calculando...</p>`;

    fetch("https://localhost:7114/api/sistemas/gaussseidel", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(datos)
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(err => {
                    throw new Error(err.error || "Error en la respuesta del servidor");
                });
            }
            return response.json();
        })
        .then(solucion => {
            pintarBonitoResultado(solucion, "Gauss-Seidel");
        })
        .catch(error => {
            resultado.innerHTML = `<p style="color:red;">Error: ${error.message}</p>`;
            console.error("Error al calcular Gauss-Seidel:", error);
        });
}
function pintarBonitoResultado(solucion, metodo) {
    // Normalizar: soporta [..] o { resultado: [..] } o un objeto plano
    let arr = Array.isArray(solucion)
        ? solucion
        : (Array.isArray(solucion?.resultado) ? solucion.resultado : Object.values(solucion));

    arr = arr.map(n => Number(n)); // asegurar números

    const resultado = document.getElementById("resultado");
    resultado.innerHTML = `
    <div class="res-card">
      <div class="res-header">
        <span>Resultado del sistema</span>
        <span class="badge">${metodo}</span>
      </div>
      <div class="res-grid">
        ${arr.map((val, idx) => `
          <div class="stat">
            <div class="label">${
            ["x", "y", "z", "w", "v"][idx] ?? `x${idx + 1}`
            }</div>
            <div class="value">${Number.isFinite(val) ? val.toFixed(9) : "-"}</div>
          </div>
        `).join("")}
      </div>
    </div>
  `;
}
