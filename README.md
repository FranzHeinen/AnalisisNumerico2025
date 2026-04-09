# 🔢 Análisis Numérico: Resolución de Funciones y Sistemas

Este repositorio contiene una herramienta desarrollada en **.NET 8** diseñada para resolver problemas complejos de ingeniería mediante métodos numéricos. La solución expone diversos algoritmos matemáticos a través de una Web API y una interfaz gráfica interactiva.

---

## 🚀 Guía de Uso Rápido (Local)

1. **Clonar el repositorio:** `git clone https://github.com/FranzHeinen/AnalisisNumerico2025.git`
2. **Ejecutar el Servidor:** Abrir la solución (`.sln`) en Visual Studio y presionar `F5` para iniciar la Web API en .NET 8.
3. **Seleccionar el Método:** En la interfaz interactiva, elige la unidad y el algoritmo deseado (ej. Bisección, Newton-Raphson, Gauss-Jordan).
4. **Ingresar Datos:** Define la función matemática (ej. `x^2 - 4`) y los parámetros requeridos.
5. **Calcular:** Presiona el botón para procesar los datos y visualizar la tabla de iteraciones detallada.

---

## 🛠️ Métodos Implementados

### 🎯 Raíces de Funciones
* **Métodos Cerrados:** Bisección y Regla Falsa.
* **Métodos Abiertos:** Newton-Raphson y Secante.

### 📊 Ajuste de Curvas
* Regresión Lineal y Regresión Polinomial.

### 📐 Sistemas de Ecuaciones e Integración
* Resolución por **Gauss-Jordan** y **Gauss-Seidel**.
* Integración numérica mediante métodos de **Trapecio** y **Simpson** (Simple y Múltiple).

---

## 💻 Tecnologías
* **Backend:** .NET 8 (C#)
* **Frontend:** HTML5, CSS3 y JavaScript (Visualización de cálculos en tiempo real).
* **Cálculo Simbólico:** Uso de librerías especializadas para procesamiento de funciones matemáticas.

---

## 📁 Estructura del Proyecto
* **/Controllers:** Exposición de los métodos mediante endpoints REST.
* **/wwwroot:** Interfaz web interactiva para la carga de datos y visualización de resultados.
* **/AnalisisNumerico_...:** Librerías de clases que contienen la lógica pura de los algoritmos.

---

## 👥 Equipo de Desarrollo
* [Franz Heinen](https://github.com/FranzHeinen)
* [Francisco Ambort](https://github.com/FranciscoAmbort)
