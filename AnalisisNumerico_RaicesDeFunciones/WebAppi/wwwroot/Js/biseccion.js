// -------------------- Inicializar GeoGebra --------------------
let ggbApp = null;

window.addEventListener("load", function () {
    ggbApp = new GGBApplet({
        appName: "graphing",
        width: 900,   // más grande
        height: 560,
        showToolBar: false,
        showAlgebraInput: false,
        showMenuBar: false,
        appletOnLoad: function (api) {
            try { api.setPerspective && api.setPerspective("G"); } catch (e) { }
            try { api.setGridVisible(true); api.setAxesVisible(true, true); } catch (e) { }
            console.log("GeoGebra listo");
        }
    }, true);

    ggbApp.inject('ggb-element');
});



// -------------------- Formulario Bisección --------------------
document.getElementById("form-biseccion").addEventListener("submit", async function (e) {
    e.preventDefault();

    const requestData = {
        Funcion: document.getElementById("funcion").value,
        Xi: parseFloat(document.getElementById("xi").value),
        Xd: parseFloat(document.getElementById("xd").value),
        MaxIteraciones: parseInt(document.getElementById("maxIteraciones").value),
        Tolerancia: parseFloat(document.getElementById("tolerancia").value)
    };

    try {
        const response = await fetch("/api/MetodosCerrados/biseccion", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(requestData)
        });

        const data = await response.json();

        if (!response.ok) {
            renderResultado({ xr: '-', iteraciones: '-', error: data.error || 'Error', converge: false }, requestData);
        } else {
            renderResultado(data, requestData);
            updatePretty(); // ⬅️ refresca f(x) en LaTeX
            graficarEnGeoGebra(requestData.Funcion, (data.Xr ?? data.xr), requestData.Xi, requestData.Xd);
        }



        function convertirFuncionParaGeoGebra(fx) {
            let s = String(fx);

            // e^(...)  -> exp(...)
            s = s.replace(/\be\s*\^\s*\(\s*([^()]+)\s*\)/gi, 'exp($1)');
            // e^x      -> exp(x)
            s = s.replace(/\be\s*\^\s*([-+]?\s*[^+\-*/()\s]+)/gi, 'exp($1)');

            return s
                .replace(/Abs/gi, "abs")
                .replace(/Log10/gi, "log10")
                .replace(/Log/gi, "log")
                .replace(/Ln/gi, "ln")
                .replace(/Exp/gi, "exp")
                .replace(/Sen/gi, "sin");
        }



        // === Para LaTeX (mostrar bonito en el header) ===
        function toLatexFromInput(fx) {
            let s = String(fx).trim();

            // |...|
            s = s.replace(/Abs\s*\(([^()]+)\)/gi, '\\left|$1\\right|');

            // exp(...)  -> e^{...}
            s = s.replace(/\bexp\s*\(\s*([^()]+)\s*\)/gi, '\\mathrm{e}^{$1}'); // exp(x) -> e^{x}
            s = s.replace(/\bExp\s*\(\s*([^()]+)\s*\)/gi, '\\mathrm{e}^{$1}'); // Exp(x) -> e^{x}

            // e^(...)   -> e^{...}
            s = s.replace(/\be\s*\^\s*\(\s*([^()]+)\s*\)/gi, '\\mathrm{e}^{$1}');
            // e^x       -> e^{x} (x simple: x, -x, x^2, 2x, etc., sin espacios/operadores)
            s = s.replace(/\be\s*\^\s*([-+]?\s*[^+\-*/()\s]+)/gi, '\\mathrm{e}^{$1}');

            // funciones estándar
            s = s.replace(/\bLn\(/gi, '\\ln(')
                .replace(/\bLog\(/gi, '\\log(');

            // 3*x -> 3x   y resto de * como ·
            s = s.replace(/(\d)\s*\*\s*x/gi, '$1x');
            s = s.replace(/\*/g, '\\cdot ');

            return s;
        }



        // Renderiza el LaTeX en el banner f(x)= ...
        function updatePretty() {
            const raw = document.getElementById('funcion').value || '';
            const latex = toLatexFromInput(raw);
            const el = document.getElementById('fx-pretty');
            el.textContent = `\\( ${latex} \\)`;
            if (window.MathJax) { MathJax.typesetPromise([el]); }
        }


        // refrescar f(x) mientras escriben y al cargar
        document.getElementById('funcion').addEventListener('input', updatePretty);
        window.addEventListener('load', updatePretty);



    } catch (error) {
        document.getElementById("resultado").textContent = `Error de conexión: ${error}`;
    }
});

// -------------------- Conversión de funciones a sintaxis GeoGebra --------------------
function convertirFuncionParaGeoGebra(fx) {
    return String(fx)
        .replace(/Abs/gi, "abs")
        .replace(/Log10/gi, "log10")
        .replace(/Log/gi, "log")   // log natural
        .replace(/Ln/gi, "ln")
        .replace(/Exp/gi, "exp")
        .replace(/Sen/gi, "sin");  // si alguien escribe "Sen"
}


// -------------------- Función para graficar --------------------
function graficarEnGeoGebra(fx, raiz, xi, xd) {
    if (!ggbApp || !ggbApp.getAppletObject) {
        console.error("GeoGebra aún no está listo");
        return;
    }
    const ggb = ggbApp.getAppletObject();
    ggb.reset();

    try {
        const f = convertirFuncionParaGeoGebra(fx);
        ggb.evalCommand(`f(x) = ${f}`);

        ggb.evalCommand(`Xi = (${xi}, f(${xi}))`);
        ggb.evalCommand(`Xd = (${xd}, f(${xd}))`);
        ggb.setPointSize("Xi", 4); ggb.setPointSize("Xd", 4);
        ggb.setColor("Xi", 0, 0, 255);
        ggb.setColor("Xd", 0, 128, 0);

        if (raiz !== undefined && !isNaN(raiz)) {
            ggb.evalCommand(`A = (${raiz}, f(${raiz}))`);
            ggb.setPointSize("A", 6);
            ggb.setColor("A", 255, 0, 0);
        }

        // encuadre automático usando f(xi), f(xd)
        const yi = ggb.getValue(`f(${xi})`);
        const yd = ggb.getValue(`f(${xd})`);
        const yMin = Math.min(yi, yd, 0);
        const yMax = Math.max(yi, yd, 0);
        const yPad = Math.max(1, (yMax - yMin) * 0.2);
        ggb.setCoordSystem(
            Math.min(xi, xd) - 1,
            Math.max(xi, xd) + 1,
            yMin - yPad,
            yMax + yPad
        );
    } catch (err) {
        console.error("Error al graficar en GeoGebra:", err);
    }
}


function renderNumero(n, dec = 4) {
    if (n === null || n === undefined || isNaN(n)) return '-';
    return Number(n).toFixed(dec);  // siempre 4 decimales
}


function renderResultado(data, req) {
    const grid = document.getElementById('res-grid');
    if (!grid) return;

    const xr = (data.Xr ?? data.xr);
    const it = (data.iteraciones ?? data.Iteraciones ?? '-');
    const err = data.error;
    const conv = (data.converge ?? data.Converge ?? true);

    grid.innerHTML = `
    <div class="stat">
      <div class="label">Raíz (xr)</div>
      <div class="value">${renderNumero(xr, 6)}</div>
    </div>
    <div class="stat">
      <div class="label">Iteraciones</div>
      <div class="value">${it}</div>
    </div>
    <div class="stat">
      <div class="label">Error</div>
      <div class="value">${renderNumero(err, 6)}</div>
    </div>
    <div class="stat">
      <div class="label">Converge</div>
      <div class="value">${String(conv)}</div>
    </div>
    <div class="stat">
      <div class="label">Intervalo [Xi, Xd]</div>
      <div class="value">[ ${renderNumero(req.Xi, 4)} , ${renderNumero(req.Xd, 4)} ]</div>
    </div>
    <div class="stat">
      <div class="label">Tolerancia</div>
      <div class="value">${renderNumero(req.Tolerancia, 6)}</div>
    </div>`;
}

function num(n, dec = 4) {  // siempre con 4 decimales: 0.0000
    if (n === null || n === undefined || isNaN(n)) return '-';
    return Number(n).toFixed(dec);
}

function renderResultado(data, req) {
    const grid = document.getElementById('res-grid');
    if (!grid) return;

    const xr = (data.Xr ?? data.xr);
    const it = (data.iteraciones ?? data.Iteraciones ?? '-');
    const err = data.error;
    const conv = (data.converge ?? data.Converge ?? true);

    grid.innerHTML = `
    <div class="stat"><div class="label">Raíz (xr)</div><div class="value">${num(xr, 6)}</div></div>
    <div class="stat"><div class="label">Iteraciones</div><div class="value">${it}</div></div>
    <div class="stat"><div class="label">Error</div><div class="value">${num(err, 6)}</div></div>
    <div class="stat"><div class="label">Converge</div><div class="value">${String(conv)}</div></div>
    <div class="stat"><div class="label">Intervalo [Xi, Xd]</div><div class="value">[ ${num(req.Xi, 4)} , ${num(req.Xd, 4)} ]</div></div>
    <div class="stat"><div class="label">Tolerancia</div><div class="value">${num(req.Tolerancia, 6)}</div></div>
  `;
}
