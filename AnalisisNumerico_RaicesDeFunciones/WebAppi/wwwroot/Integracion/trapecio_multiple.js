// ✅ 1. Inicializar GeoGebra al cargar la página (código reutilizado de tus otros archivos)
let ggbApp = null;
window.addEventListener("load", function () {
    ggbApp = new GGBApplet({
        appName: "graphing",
        width: 800,
        height: 560,
        showToolBar: false,
        showAlgebraInput: false,
        showMenuBar: false,
        appletOnLoad: function (api) {
            console.log("GeoGebra listo para Integración");
        }
    }, true);
    ggbApp.inject('ggb-element');
});

// ✅ 2. Función auxiliar para convertir la expresión matemática
// ✅ Reemplaza la función antigua con esta versión corregida
function convertirFuncionParaGeoGebra(fx) {
    let s = String(fx);

    // Convierte e^x a exp(x) para GeoGebra
    s = s.replace(/\be\s*\^\s*\(\s*([^()]+)\s*\)/gi, 'exp($1)');
    s = s.replace(/\be\s*\^\s*([-+]?\s*[^+\-*/()\s]+)/gi, 'exp($1)');

    return s
        .replace(/sqr/gi, "sqrt")
        .replace(/Abs/gi, "abs")
        .replace(/Log10/gi, "log10")
        .replace(/Log/gi, "ln")
        .replace(/Ln/gi, "ln")
        .replace(/Exp/gi, "exp")
        .replace(/Sen/gi, "sin");
}

// ✅ 3. Nueva función para dibujar la integral en GeoGebra
function graficarIntegral(fx, xi, xd) {
    if (!ggbApp || !ggbApp.getAppletObject) {
        console.error("GeoGebra no está listo.");
        return;
    }
    const ggb = ggbApp.getAppletObject();
    ggb.reset(); // Limpia la vista anterior

    try {
        const f = convertirFuncionParaGeoGebra(fx);

        // Dibuja la función f(x)
        ggb.evalCommand(`f(x) = ${f}`);

        // Dibuja el área de la integral usando el comando que mencionaste
        ggb.evalCommand(`integral = Integral(f, ${xi}, ${xd})`);

        // Personaliza los colores y el relleno para que se vea bien
        ggb.setColor("f", 0, 120, 0); // Línea de la función en verde oscuro
        ggb.setLineThickness("f", 5);
        ggb.setColor("integral", 34, 197, 94); // Relleno del área en verde claro
        ggb.setFilling("integral", 0.4);

        // Ajusta el zoom automáticamente para que el área sea visible
        ggb.evalCommand(`ZoomIn(${xi - 1}, ${-2}, ${xd + 1}, ${10})`);


    } catch (err) {
        console.error("Error al graficar la integral en GeoGebra:", err);
    }
}

// ✅ 4. Tu función 'calcular' original, ahora integrada con la gráfica
async function calcular() {
    const funcion = document.getElementById('funcion').value;
    const xi = parseFloat(document.getElementById('xi').value);
    const xd = parseFloat(document.getElementById('xd').value);
    const n = parseInt(document.getElementById('n').value);
    const resultadoDiv = document.getElementById('area-resultado');

    const requestData = { funcion, xi, xd, n };

    try {
        const response = await fetch('/api/integracion/trapecio-multiple', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(requestData)
        });

        const result = await response.json();

        if (response.ok) {
            resultadoDiv.textContent = result.area.toFixed(6);
            // ¡Aquí se llama a la nueva función para que dibuje!
            graficarIntegral(funcion, xi, xd);
        } else {
            resultadoDiv.textContent = `Error: ${result.error}`;
        }
    } catch (error) {
        resultadoDiv.textContent = 'Error de conexión.';
        console.error('Error en la llamada fetch:', error);
    }
}