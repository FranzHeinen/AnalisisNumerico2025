// ========== GeoGebra ==========
let ggbPoly = null;
let ggbPolyReadyResolve;
const ggbPolyReady = new Promise(res => ggbPolyReadyResolve = res);

document.addEventListener("DOMContentLoaded", () => {
    const params = {
        appName: "graphing",
        width: 900, height: 520,
        showToolBar: false, showAlgebraInput: false, showMenuBar: false,
        perspective: "G", enableUndoRedo: false,
        appletOnLoad: api => {
            ggbPoly = api;
            console.log("GeoGebra cargado correctamente.");
            ggbPolyReadyResolve();
        }
    };

    // NO USAR .setHTML5Codebase porque genera error de Access Denied en localhost
    const app = new GGBApplet(params, true);
    app.inject('ggb-poly');
});

async function ggbClearPoly() {
    await ggbPolyReady;
    try {
        ggbPoly.evalCommand("Delete[All]");
    } catch (e) {
        console.warn("No se pudo limpiar GeoGebra", e);
    }
}

async function ggbAddPointsPoly(pts) {
    await ggbPolyReady;
    pts.forEach((p, i) => ggbPoly.evalCommand(`P${i + 1}=(${p[0]},${p[1]})`));
}

function polyDef(coef) {
    const terms = coef.map((a, i) => {
        if (Math.abs(a) < 1e-12) return null;
        if (i === 0) return `${a}`;
        if (i === 1) return `${a}*x`;
        return `${a}*x^${i}`;
    }).filter(Boolean);
    return `f(x)=${terms.join("+")}`.replace(/\+\-/g, "-");
}

async function ggbPlotPoly(coef) {
    await ggbPolyReady;
    const def = polyDef(coef);
    ggbPoly.evalCommand(def);
    ggbPoly.setLineThickness("f", 5);
    ggbPoly.setColor("f", 34, 197, 94);
}

async function ggbFitPoly(pts) {
    await ggbPolyReady;
    if (pts.length === 0) return;
    const xs = pts.map(p => p[0]), ys = pts.map(p => p[1]);
    const xmin = Math.min(...xs), xmax = Math.max(...xs);
    const ymin = Math.min(...ys), ymax = Math.max(...ys);
    const padX = (xmax - xmin || 1) * 0.2, padY = (ymax - ymin || 1) * 0.2;
    ggbPoly.evalCommand(`ZoomIn[${xmin - padX},${ymin - padY},${xmax + padX},${ymax + padY}]`);
}

// ========== UI ==========
const rpTbody = document.getElementById("rp-tbody");
document.getElementById("rp-add").addEventListener("click", () => {
    const tr = document.createElement("tr");
    tr.innerHTML = `<td><input type="number" step="any" value="0"/></td>
                    <td><input type="number" step="any" value="0"/></td>
                    <td><button class="row-del">✕</button></td>`;
    rpTbody.appendChild(tr);
});
rpTbody.addEventListener("click", (e) => {
    if (e.target.classList.contains("row-del")) {
        e.target.closest("tr").remove();
    }
});

function leer() {
    const pts = [];
    for (const tr of rpTbody.querySelectorAll("tr")) {
        const x = parseFloat(tr.children[0].querySelector("input").value);
        const y = parseFloat(tr.children[1].querySelector("input").value);
        if (Number.isFinite(x) && Number.isFinite(y)) pts.push([x, y]);
    }
    return pts;
}

const END_POLI = "/api/RegresionPolinomial";
document.getElementById("rp-calc").addEventListener("click", async () => {
    const Puntos = leer();
    const Grado = parseInt(document.getElementById("rp-grado").value || "2");
    const Tolerancia = parseFloat(document.getElementById("rp-tol").value || "0.8");

    if (Puntos.length < Grado + 1) {
        alert(`Ingresá al menos ${Grado + 1} puntos.`);
        return;
    }

    let res;
    try {
        res = await fetch(END_POLI, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ Puntos, Grado, Tolerancia })
        });
    } catch (err) {
        console.error(err);
        alert("No se pudo contactar al backend.");
        return;
    }

    if (!res.ok) {
        const txt = await res.text();
        console.error("Error HTTP", res.status, txt);
        alert(`Error ${res.status}: ${txt}`);
        return;
    }

    const data = await res.json();
    console.log("Polinomial -> respuesta", data);

    document.getElementById("rp-fx").textContent = data.Funcion ?? data.funcion ?? "—";
    document.getElementById("rp-r").textContent = Number(data.Correlacion ?? data.correlacion ?? 0).toFixed(2) + " %";
    document.getElementById("rp-ok").textContent = data.EfectividadAjuste ?? data.efectividadAjuste ?? "—";

    const C = data.Coeficientes ?? data.coeficientes ?? [];
    await ggbClearPoly();
    await ggbAddPointsPoly(Puntos);
    await ggbPlotPoly(C);
    await ggbFitPoly(Puntos);
});
