# Análisis Numérico: Resolución de Funciones y Sistemas 🔢

Este repositorio contiene una potente herramienta computacional desarrollada en **.NET 8** diseñada para resolver problemas complejos de ingeniería mediante métodos numéricos. La solución expone diversos algoritmos matemáticos a través de una Web API y una interfaz gráfica interactiva.

## 🛠️ Métodos Implementados

### 🎯 Raíces de Funciones
* **Métodos Cerrados:** Bisección y Regla Falsa.
* **Métodos Abiertos:** Newton-Raphson y Secante.

### 📊 Ajuste de Curvas
* Regresión Lineal y Regresión Polinomial.

### 📐 Sistemas de Ecuaciones e Integración
* Resolución por Gauss-Jordan y Gauss-Seidel.
* Integración numérica mediante métodos de Trapecio y Simpson (Simple y Múltiple).

## 🚀 Tecnologías
* **Backend:** .NET 8 (C#)
* **Frontend:** HTML5, CSS3 y JavaScript (Visualización de cálculos en tiempo real).
* **Cálculo Simbólico:** Uso de librerías especializadas para procesamiento de funciones matemáticas.

## 📁 Estructura del Proyecto
El proyecto está modularizado por áreas de estudio:
* `/Controllers`: Exposición de los métodos mediante endpoints REST.
* `/wwwroot`: Interfaz web interactiva para la carga de datos y visualización de resultados.
* `/AnalisisNumerico_...`: Librerías de clases que contienen la lógica pura de los algoritmos.
