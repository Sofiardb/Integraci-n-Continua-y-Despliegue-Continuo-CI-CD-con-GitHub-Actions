# Integración Continua y Despliegue Continuo (CI/CD) con GitHub Actions

## Introducción

En el contexto actual del desarrollo de software, la necesidad de entregar productos de calidad de manera frecuente y confiable ha impulsado la adopción de prácticas que automaticen y estandaricen los procesos de construcción, prueba y entrega de aplicaciones. En este marco, la **Integración Continua (CI)** y el **Despliegue Continuo (CD)** se han consolidado como prácticas fundamentales dentro de la ingeniería de software moderna.

La Integración Continua propone que los desarrolladores integren sus cambios al repositorio de código de forma frecuente, validando cada integración mediante procesos automáticos de compilación y pruebas. El Despliegue Continuo extiende este concepto automatizando la liberación de versiones del software, permitiendo que las aplicaciones estén siempre en un estado potencialmente desplegable.

Este trabajo se enmarca en estas prácticas y tiene como objetivo principal construir un cuerpo de conocimiento práctico que permita al lector **reproducir paso a paso una experiencia real de CI/CD**, utilizando **GitHub Actions** como herramienta central. A lo largo del documento se describe tanto el proceso técnico como las buenas prácticas de ingeniería de software aplicadas durante el desarrollo colaborativo del proyecto.

---

## Desarrollo práctico

### Replicación paso a paso del desarrollo y los commits

Con el fin de cumplir el objetivo principal del trabajo (permitir que el lector reproduzca una experiencia práctica completa) se describe a continuación un **paso a paso detallado para replicar el proceso de desarrollo** utilizando el código provisto en este proyecto.

#### Contenido del proyecto
Integracion-Continua-y-Despliegue-Continuo-CI-CD-con-GitHub-Actions/
├── .github/
│   └── workflows/
│       └── ci-cd.yml          # Pipeline de CI/CD
├── src/
│   └── CICDDemo.Api/          # Proyecto Web API
│       ├── Controllers/       # Controladores de la API
│       ├── Models/            # Modelos de datos
│       ├── Services/          # Servicios de negocio
│       └── Program.cs         # Punto de entrada
├── tests/
│   └── CICDDemo.Api.Tests/    # Proyecto de Tests
│       ├── Controllers/       # Tests de controladores
│       └── Services/          # Tests de servicios
│__ .gitignore
├── CICDDemo.sln               # Solución de Visual Studio
└── README.md                  # Este archivo

#### Tecnologías utilizadas

.NET 8 - Framework de desarrollo
ASP.NET Core Web API - Backend REST API
xUnit - Framework de testing
Moq - Framework de mocking
GitHub Actions - CI/CD Pipeline
Coverlet - Cobertura de código

Este procedimiento permite reconstruir en un nuevo repositorio Git el flujo de trabajo colaborativo, observando cómo los commits reflejan el avance incremental del sistema y cómo el pipeline de CI/CD se ejecuta automáticamente en cada etapa.


---


### Prerrequisitos
[.NET 8 SDK](https://dotnet.microsoft.com/es-es/download/dotnet/8.0)
[Git](https://git-scm.com/install/)


---

### Control de versiones y trabajo colaborativo con Git

El proyecto se desarrolla íntegramente en un repositorio Git, el cual permite registrar el historial completo de cambios y evidenciar la colaboración entre los distintos miembros del equipo. Cada funcionalidad se implementa de forma incremental mediante commits pequeños y descriptivos, lo que facilita la trazabilidad y el entendimiento del avance del trabajo.

**Acción práctica 1:**
**Paso 1: Crear un nuevo repositorio**
1. Crear un repositorio vacío en GitHub (sin README ni configuraciones iniciales).
      
      ### 1.1 Ir a GitHub
      1. Abre tu navegador y ve a [github.com](https://github.com)
      2. Inicia sesión con tu cuenta

      ### 1.2 Crear nuevo repositorio
      1. Haz clic en el botón **"+"** en la esquina superior derecha
      2. Selecciona **"New repository"**  
      
      ### 1.3 Configurar el repositorio
      Completa el formulario:

      | Campo | Valor |
      |-------|-------|
      | **Repository name** | `cicd-demo-csharp` |
      | **Description** | Demostración de CI/CD con GitHub Actions y C# |
      | **Visibility** | Public (o Private si prefieres) |
      | **Initialize this repository** | ❌ NO marcar nada |

      ### 1.4 Crear repositorio
      Haz clic en **"Create repository"**   

2. Clonar el repositorio localmente:

   ```bash
   git clone <url-del-repositorio>
   cd <nombre-del-repositorio>

**Paso 2: Commit inicial – estructura del proyecto**

1.Desde la raíz del repositorio clonado, ejecutar:

   ```bash
   # Crear la solución
   dotnet new sln -n CICDDemo

   # Crear el proyecto API (incluye Program.cs, appsettings, etc.)
   dotnet new webapi -n CICDDemo.Api -o src/CICDDemo.Api

   # Crear el proyecto de tests
   dotnet new xunit -n CICDDemo.Api.Tests -o tests/CICDDemo.Api.Tests

   # Añadir los proyectos a la solución
   dotnet sln add src/CICDDemo.Api/CICDDemo.Api.csproj
   dotnet sln add tests/CICDDemo.Api.Tests/CICDDemo.Api.Tests.csproj

   # Referenciar la API desde el proyecto de tests
   dotnet add tests/CICDDemo.Api.Tests/CICDDemo.Api.Tests.csproj reference src/CICDDemo.Api/CICDDemo.Api.csproj
   ```

2. Ejecutar el primer commit.
```bash
git add .
git commit -m "Commit inicial: Estructura del proyecto"
git push origin main
```

3. Verificar que el repositorio contiene la estructura base y que `dotnet build` y `dotnet test` se ejecutan correctamente.

**Buenas prácticas:**
- Evitar commits con múltiples responsabilidades.
- Mantener un historial de commits que refleje el avance progresivo del sistema.

---

### Integración Continua con GitHub Actions

Una vez creado el proyecto base, se incorpora un pipeline de Integración Continua utilizando GitHub Actions. Este pipeline se ejecuta automáticamente ante cada `push` o `pull request`, validando que el código compile correctamente y que las pruebas automatizadas se ejecuten sin errores.

El workflow define etapas de **build** y **test**, permitiendo detectar fallos de manera temprana y reduciendo el riesgo de integrar código defectuoso a la rama principal.

#### Flujo del pipeline

┌─────────────────────────────────────────────────────────────────┐
│                        TRIGGER                                  │
│         Push a main/master  |  Pull Request a main/master       │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    JOB: Build and Test                          │
│  ┌──────────┐  ┌──────────┐  ┌─────────┐  ┌─────────────────┐   │
│  │ Checkout │→ │ Setup    │→ │ Restore │→ │ Build (Release) │   │
│  │  Code    │  │ .NET 8   │  │ Deps    │  │                 │   │
│  └──────────┘  └──────────┘  └─────────┘  └─────────────────┘   │ 
│                                                    │            │
│                                                    ▼            │
│                                           ┌───────────────┐     │
│                                           │  Run Tests    │     │
│                                           │ + Coverage    │     │
│                                           └───────────────┘     │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    JOB: Code Analysis                           │
│  ┌──────────┐  ┌──────────┐  ┌─────────────────────────────┐    │
│  │ Checkout │→ │ Setup    │→ │ Verify Code Format          │    │
│  │  Code    │  │ .NET 8   │  │                             │    │
│  └──────────┘  └──────────┘  └─────────────────────────────┘    │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼ (Solo en push a main)
┌─────────────────────────────────────────────────────────────────┐
│                    JOB: Publish                                 │
│  ┌──────────┐  ┌──────────┐  ┌─────────┐  ┌────────────────┐    │
│  │ Checkout │→ │ Setup    │→ │ Publish │→ │ Upload         │    │
│  │  Code    │  │ .NET 8   │  │ App     │  │ Artifacts      │    │
│  └──────────┘  └──────────┘  └─────────┘  └────────────────┘    │
└─────────────────────────────────────────────────────────────────┘

#### Protección de Ramas (Branch Protection Rules)

Como parte del aseguramiento de la calidad y en coherencia con las prácticas de Integración Continua, se configuran reglas de protección sobre la rama main. Estas reglas garantizan que ningún cambio pueda integrarse directamente sin pasar previamente por el proceso de revisión y validación automática del pipeline.

En GitHub, esta configuración se realiza desde: **Settings → Branches → Add branch protection rule**

**Acción práctica 2:**

1. En **ruleset name** puede colocar: 'Protección para la rama main'
2. En **target branches**, haga click en **Add target → Include by pattern → escriba main → Add inclusion pattern**
3. Para la rama main, se recomienda habilitar las siguientes opciones:

      ✅ Require a pull request before merging

      ✅ Require status checks to pass before merging (seleccionar los jobs del workflow, por ejemplo: Build and Test y Code Analysis)

      ✅ Require branches to be up to date before merging

      ✅ Require review from at least 1 reviewer
4. Click en **Create**

De esta manera, cualquier intento de integración debe:

   - Realizarse mediante un Pull Request.

   - Ejecutar automáticamente el pipeline de CI.

   - Finalizar correctamente todas las verificaciones antes de permitir el merge.

Esta práctica fortalece la estabilidad del proyecto, evita la incorporación de código defectuoso en la rama principal y consolida la integración entre control de versiones y automatización CI/CD.


**Acción práctica 3:**
1. Crear la carpeta `.github/workflows`.
2. Copiar el archivo de workflow de GitHub Actions incluido en el proyecto.
3. Copiar el archivo .gitignore en la raíz del proyecto.
4. Ejecutar:
   ```bash
   git add .github tests
   git commit -m "ci: Agrego el pipeline CI/CD usando Github Actions"
   git push origin main
   ```
5. Verificar en la pestaña *Actions* de GitHub que el pipeline se ejecuta automáticamente.


**Buenas prácticas:**
- Incorporar el pipeline desde las primeras etapas del proyecto.
- Asegurar que el pipeline se ejecute tanto en pushes como en pull requests.
- Mantener el pipeline simple y alineado con el alcance del proyecto.

---

### Desarrollo incremental de la aplicación

La aplicación backend se construye de manera incremental, agregando funcionalidades en distintas ramas de trabajo que luego se integran a la rama principal mediante pull requests. Este flujo permite simular un entorno colaborativo real, donde cada integración es validada automáticamente por el pipeline de CI.


#### Testing automatizado

El proyecto incluye pruebas unitarias desarrolladas con xUnit, las cuales se ejecutan automáticamente como parte del pipeline. Esto permite validar el comportamiento de los servicios y controladores, asegurando que los cambios no introduzcan regresiones.


**Buenas prácticas:**
- Escribir tests que validen tanto casos normales como casos límite.
- Mantener las pruebas desacopladas de la implementación.
- Usar los tests como mecanismo de confianza para futuras modificaciones.

En la siguiente actividad práctica se explica cómo crear una rama de trabajo para desarrollar una funcionalidad. Se le proporcionan los archivos necesarios para implementarla. Sin embargo, es importante destacar que **el desarrollo de una aplicación real debe realizarse de forma incremental mediante varios commits** (por ejemplo: primero modelos, luego servicios, después controladores y, a continuación, los tests de esa funcionalidad), en lugar de agregar todo en un solo commit. De esta forma se siguen buenas prácticas: commits pequeños y con un propósito claro, lo que facilita la revisión y la trazabilidad. Así podrá observar el flujo completo: cambios en la rama → apertura del pull request → ejecución del pipeline → integración a `main` una vez que el pipeline finalice correctamente.

**Acción práctica 4:**

**PASO 1: Crear feature Calculadora**
1. Crear una rama de feature desde `main` `feature/calculadora`.
      ´´´ bash
      # Crear y cambiar a nueva rama
      git checkout -b feature/calculadora

      # Verificar que estás en la nueva rama
      git branch
      ```
2. Incorpore los siguientes archivos que implementan la nueva funcionalidad:
   **Archivos a copiar para implementar el feature:**
      src/CICDDemo.Api/Services/ICalculadoraService.cs
      src/CICDDemo.Api/Services/CalculadoraService.cs
      src/CICDDemo.Api/Models/ResultadoOperacion.cs
      src/CICDDemo.Api/Controllers/CalculadoraController.cs
   **Agregar: en src/CICDDemo.Api/Program.cs**
      ```csharp
      using CICDDemo.Api.Services;
      // ... después de builder.Services.AddSwaggerGen();
      builder.Services.AddScoped<ICalculadoraService, CalculadoraService>(); 
      ```
   **Archivos a copiar para implementar los tests:**
      tests/CICDDemo.Api.Tests/Services/CalculadoraServiceTests.cs
      tests/CICDDemo.Api.Tests/Controllers/CalculadoraControllerTests.cs
3. Ejecutar:
   ```bash
   git add .
   git commit -m "feat: Agrego la funcionalidad Calculadora"
   git push origin main
   ```
4. Abrir un pull request y observar la ejecución del pipeline (build + tests). Pasos en GitHub:
   - **4.1** Tras hacer `git push origin´ abre el repositorio en GitHub en tu navegador.
   - **4.2** Suele aparecer un banner amarillo: *"feature/calculadora had recent pushes"* con el botón **"Compare & pull request"**. Haz clic ahí. Si no aparece, ve a la pestaña **Pull requests** y clic en **"New pull request"**.
   - **4.3** En la página del PR, comprueba que la **base** sea `main` y la **compare** sea tu rama (p. ej. `feature/calculadora`). Escribe un título (p. ej. "feat: API Calculadora y tests") y, si quieres, una descripción. Haz clic en **"Create pull request"**.
   - **4.4** En la misma página del PR verás que GitHub ejecuta los *checks* (workflows). Aparecen en la sección de comprobaciones bajo el título o en la pestaña **"Checks"**. Espera a que termine el run.
   - **4.5** Para ver el pipeline con detalle: ve a la pestaña **Actions** del repositorio. El run más reciente será el disparado por tu PR. Ábrelo y revisa los jobs **Build and Test** y **Code Analysis**; en un PR no se ejecuta el job "Publish Artifacts" (solo en push a `main`).
   - **4.6** Si todos los checks pasan (marca verde), el PR está listo para revisión y merge. Si algo falla, revisa los logs del job que falló en *Actions* y corrige antes de mergear.

**PASO 2: Crear feature de Tareas**

1. Crear una rama de feature desde `main` `feature/tareas`.
      ´´´ bash
      # Crear y cambiar a nueva rama
      git checkout -b feature/tareas

      # Verificar que estás en la nueva rama
      git branch
      ```
2. Incorpore los siguientes archivos que implementan la nueva funcionalidad:
   **Archivos a copiar para implementar el feature:**
      src/CICDDemo.Api/Services/ITareaService.cs
      src/CICDDemo.Api/Services/TareaService.cs
      src/CICDDemo.Api/Models/Tarea.cs
      src/CICDDemo.Api/Controllers/TareasController.cs
   **Agregar: en src/CICDDemo.Api/Program.cs**
      ```csharp
      using CICDDemo.Api.Services;
      // ... después de builder.Services.AddSwaggerGen();
      builder.Services.AddScoped<ITareaService, TareaService>(); 
      ```
   **Archivos a copiar para implementar los tests:**
      tests/CICDDemo.Api.Tests/Services/TareaServiceTests.cs
      tests/CICDDemo.Api.Tests/Controllers/TareasControllerTests.cs
3. Ejecutar:
   ```bash
   git add .
   git commit -m "feat: Agrego la funcionalidad para Tareas"
   git push origin main

4. Replicar el paso 4 de **Crear feature Calculadora** pero con la rama `feature/tareas` desde Github.

**Buenas prácticas:**
- Usar una rama por funcionalidad.
- No integrar cambios a `main` sin validación automática.
- Revisar los resultados del pipeline antes de realizar el merge.


### Generación de artefactos

Como parte del proceso de Despliegue Continuo, el pipeline ya incluye un job que publica artefactos. Ese job **solo se ejecuta cuando hay un push a la rama `main`** (no en los runs provocados por un pull request). Tras cada merge a `main`, el push resultante dispara el workflow y se generan los artefactos de la aplicación. Estos representan una versión construida y validada del sistema, lista para su uso o despliegue.

**Acción práctica 5:**

1. **Mergear el primer Pull Request** (por ejemplo el de `feature/calculadora` o el de `feature/tareas` que hayas abierto en el paso anterior).
   - En GitHub, entra al PR y haz clic en **"Merge pull request"** (y confirma el merge).
   - Eso hace un **push a `main`** con los cambios integrados.

2. **Ir a la pestaña *Actions*** del repositorio. Verás un nuevo run del workflow disparado por ese push a `main`.

3. **Abrir ese run** (el más reciente). En la lista de jobs deberías ver:
   - *Build and Test*
   - *Code Analysis*
   - **Publish Artifacts** ← este job solo aparece en runs por push a `main`, no en runs por PR.

4. **Esperar a que el run termine en verde.** Si "Publish Artifacts" finalizó correctamente, en la parte inferior del run aparece la sección **Artifacts**.

5. **Descargar el artefacto:** haz clic en **`app-artifacts`** para descargar el ZIP con la publicación de la API.

6. **Repetir con el siguiente PR** (por ejemplo mergear `feature/tareas` si antes mergeaste `feature/calculadora`). Vuelve a *Actions*, abre el run del nuevo push a `main` y comprueba que se generó de nuevo el artefacto `app-artifacts` para esa versión del código.

Así se observa en la práctica que cada integración a `main` genera una nueva versión publicada y disponible en Artifacts.

**Buenas prácticas:**
- Publicar artefactos solo cuando el pipeline finaliza correctamente (el job ya está condicionado a push a `main`).
- Asociar cada artefacto a un estado consistente del código (cada run corresponde a un commit en `main`).

---

## Beneficios de implementar CI/CD

La implementación de prácticas de Integración Continua y Despliegue Continuo (CI/CD) genera beneficios significativos tanto en el ámbito técnico de la Ingeniería de Software como en otras dimensiones organizacionales.

### Impacto en la Ingeniería de Software

- **Detección temprana de errores:** La ejecución automática de builds y pruebas unitarias en cada integración permite identificar fallos en etapas iniciales del ciclo de desarrollo. Esto reduce el costo de corrección, ya que los defectos detectados tempranamente requieren menos esfuerzo que aquellos encontrados en producción.

- **Mejora sistemática de la calidad:** La validación constante del código mediante pruebas automatizadas y análisis estático contribuye a mantener estándares de calidad sostenidos en el tiempo. La automatización actúa como un mecanismo objetivo de verificación técnica.

- **Reducción de regresiones:** Cada cambio es validado contra el conjunto completo de pruebas, minimizando la probabilidad de que nuevas funcionalidades afecten comportamientos previamente correctos.

- **Integración continua real:** El trabajo en ramas con pull requests y validaciones automáticas disminuye los conflictos de integración y evita acumulaciones de cambios difíciles de fusionar.

- **Trazabilidad y auditoría técnica:** Cada commit, ejecución de pipeline y artefacto generado queda registrado, lo que facilita el seguimiento histórico de decisiones técnicas y versiones liberadas.

### Impacto en otras áreas

La implementación de CI/CD no solo produce mejoras en el proceso técnico de desarrollo, sino que también genera efectos positivos en otras áreas de la organización. En primer lugar, aumenta la previsibilidad en las entregas, ya que la automatización de compilaciones, pruebas y generación de artefactos reduce la incertidumbre asociada a tareas manuales y repetitivas. Al contar con procesos estandarizados y verificaciones automáticas, las estimaciones de tiempos y la planificación de versiones se vuelven más confiables.

Asimismo, CI/CD favorece la colaboración interdisciplinaria. Al compartir un mismo flujo automatizado, los equipos de desarrollo, testing y operaciones trabajan sobre una base común de validación y despliegue, lo que reduce fricciones y mejora la coordinación. Este enfoque se alinea con los principios de DevOps, promoviendo una integración más fluida entre áreas tradicionalmente separadas.

Finalmente, la generación automática de versiones construidas y verificadas incrementa la confianza del negocio en el proceso de entrega. Cada versión que llega a etapas posteriores del ciclo de vida ha superado controles técnicos definidos, lo que disminuye riesgos y aporta mayor seguridad a clientes y usuarios finales. De este modo, CI/CD contribuye no solo a la eficiencia técnica, sino también a la credibilidad y solidez organizacional.

---

## Desafíos y consideraciones profesionales

Si bien CI/CD aporta múltiples beneficios, su adopción requiere una implementación cuidadosa y un análisis contextual por parte de los profesionales responsables del proyecto.

### Diseño adecuado del pipeline

Un pipeline mal configurado puede volverse lento, complejo o ineficiente. Es fundamental definir claramente qué etapas son necesarias, qué validaciones deben ejecutarse en cada tipo de evento (push, pull request, release) y cómo equilibrar velocidad y profundidad de análisis. El exceso de automatización innecesaria puede generar demoras que afecten la productividad del equipo.

### Mantenimiento del conjunto de pruebas

El valor de CI/CD depende directamente de la calidad y cobertura de las pruebas automatizadas. Los profesionales deben mantener los tests actualizados, evitar pruebas frágiles o excesivamente dependientes de detalles internos de implementación y controlar el crecimiento del tiempo de ejecución del pipeline. Un conjunto de pruebas inestable puede generar desconfianza y reducir la efectividad del proceso.

### Gestión cultural y disciplina del equipo

La adopción de CI/CD implica un cambio cultural. No se trata únicamente de incorporar herramientas, sino de sostener prácticas como realizar commits pequeños y frecuentes, utilizar pull requests para toda integración, revisar código de forma sistemática y corregir rápidamente los builds fallidos. Sin compromiso del equipo, la automatización pierde su impacto positivo.

### Seguridad y protección del repositorio

Es necesario configurar adecuadamente reglas de protección de ramas, controles de acceso y manejo seguro de secretos y credenciales dentro del pipeline. Una configuración deficiente puede permitir integraciones no validadas o exponer información sensible.

### Escalabilidad del proceso

A medida que el proyecto crece, el pipeline debe evolucionar. Los profesionales deben evaluar la incorporación de análisis de calidad más avanzados, estrategias de versionado y aut


---

## Conclusión

En este trabajo se diseñó e implementó un pipeline de Integración Continua y Despliegue Continuo (CI/CD) utilizando GitHub Actions, logrando automatizar de manera efectiva los procesos de construcción, ejecución de pruebas y generación de artefactos para una aplicación backend desarrollada en .NET 8. A lo largo del proyecto se demostró, desde un enfoque práctico y reproducible, cómo configurar un flujo de CI/CD desde cero e integrarlo al control de versiones mediante Git y GitHub.

La incorporación de pruebas unitarias con xUnit, ejecutadas automáticamente en cada push y pull request, permitió evidenciar los beneficios concretos de la automatización: detección temprana de errores, prevención de regresiones y mayor confianza en la estabilidad del sistema. Asimismo, la configuración de reglas de protección de ramas reforzó la gestión y el control del repositorio, asegurando que solo código validado y revisado pudiera integrarse a la rama principal.

Desde la perspectiva de la Ingeniería de Software, el trabajo aplicó prácticas fundamentales como desarrollo incremental, commits atómicos, trabajo por ramas, revisión mediante pull requests, integración continua y generación sistemática de versiones publicables. Estas prácticas no solo mejoran la calidad técnica del producto, sino que también fortalecen la colaboración, la trazabilidad y la mantenibilidad del proyecto.

En conclusión, la adopción de CI/CD constituye una transformación en la forma de trabajar, orientada a la automatización, la calidad y la entrega continua de valor. El pipeline implementado demuestra cómo estas prácticas pueden incorporarse de manera concreta en proyectos reales, constituyendo un modelo replicable y escalable para futuros desarrollos.

---

## Referencias

- Fowler, M. (2006). Continuous integration. MartinFowler.com. https://martinfowler.com/articles/continuousIntegration.html
- Humble, J., & Farley, D. (2010). Continuous delivery: Reliable software releases through build, test, and deployment automation. Addison-Wesley Professional.
- Documentación oficial de GitHub Actions.
- Documentación oficial de .NET y xUnit.
