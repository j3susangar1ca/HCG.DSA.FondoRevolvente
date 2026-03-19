# HCG.FondoRevolvente

# Especificación Funcional y de Interfaz Nativa — Edición Enterprise

## Sistema de Gestión de Fondo Revolvente · Hospital Civil de Guadalajara

### Documento de Diseño UX/UI · Alineado a Stack Tecnológico Definido

---

| Atributo                        | Valor                                                                                       |
| ------------------------------- | ------------------------------------------------------------------------------------------- |
| **Identificador del Documento** | FR-UX-SPEC-2026-003                                                                         |
| **Plataforma Objetivo**         | Windows 11 (Build 22621 / 22H2 y posteriores)                                               |
| **Plataforma Desarrollo**       | Linux Fedora KDE Plasma 43                                                                  |
| **Framework de Presentación**   | Uno Platform — WinUI 3 Flavor · Windows App SDK 1.6+                                        |
| **Patrón de UI**                | MVVM (Model-View-ViewModel) · CommunityToolkit.Mvvm                                         |
| **Sistema de Diseño**           | Microsoft Fluent Design System v2 · WinUI 3 · Windows Community Toolkit                     |
| **Ciclo de Vida del Proceso**   | 30 estados · 38 transiciones · 8 fases · 27 hitos · 5 reglas de negocio                     |
| **Roles Documentados**          | Administrador · Comprador DSA · Miembro CAA · Recursos Financieros · Almacén · Consulta DSA |
| **Estado del Documento**        | Aprobado para Diseño e Implementación                                                       |

---

## Tabla de Contenidos

| Sección        | Título                                                                   |
| -------------- | ------------------------------------------------------------------------ |
| **1**          | Introducción y Alcance del Documento                                     |
| **2**          | Arquitectura de Presentación — Stack Tecnológico desde la Perspectiva UX |
| **3**          | Modelo de Roles, Permisos y Seguridad de Interfaz                        |
| **4**          | Sistema Global de Diseño (Design System)                                 |
| **5**          | Módulo 01 — Pantalla de Autenticación                                    |
| **6**          | Módulo 02 — Shell Principal de la Aplicación                             |
| **7**          | Módulo 03 — Dashboard KPI en Tiempo Real                                 |
| **8**          | Módulo 04 — Gestión de Solicitudes: Vista de Lista                       |
| **9**          | Módulo 05 — Nueva Solicitud: Formulario de Creación                      |
| **10**         | Módulo 06 — Detalle de Solicitud: Expediente Completo                    |
| **11**         | Módulo 07 — Timeline de Hitos: Las 8 Fases del Proceso                   |
| **12**         | Módulo 08 — Gestión de Proveedores                                       |
| **13**         | Módulo 09 — Cotizaciones y Cuadro Comparativo                            |
| **14**         | Módulo 10 — Validación Fiscal: Panel CFDI                                |
| **15**         | Módulo 11 — Reportes y Exportación                                       |
| **16**         | Módulo 12 — Centro de Notificaciones en Tiempo Real                      |
| **17**         | Módulo 13 — Panel de Administración: Roles y Accesos                     |
| **18**         | Especificación Transversal: Motor de Estados y Transiciones Visuales     |
| **19**         | Guía de Motion y Microinteracciones                                      |
| **Apéndice A** | Sistema de Colores por Fase (StateBadge)                                 |
| **Apéndice B** | Matriz de Visibilidad de Módulos por Rol                                 |
| **Apéndice C** | Representación Visual de Reglas de Negocio                               |
| **Apéndice D** | Catálogo Completo de Animaciones Estándar                                |
| **Apéndice E** | Mapeo del Stack Tecnológico a Componentes de Interfaz                    |
| **Apéndice F** | Glosario de Términos Técnicos y de Diseño                                |

---

## 1. Introducción y Alcance del Documento

### 1.1 Propósito

Este documento constituye la **Especificación Funcional y de Interfaz Nativa** del Sistema de Gestión de Fondo Revolvente del Hospital Civil de Guadalajara (HCG). Su objetivo es definir de manera exhaustiva, precisa y tecnológicamente alineada la experiencia de usuario (UX), la interfaz de usuario (UI), los flujos de interacción, los comportamientos visuales y los criterios de aceptación de cada módulo del sistema, sirviendo como referencia canónica para los equipos de diseño e implementación.

El documento integra y extiende tres especificaciones previas (FR-UX-SPEC v1.0, FR-UX-SPEC v2.0, y el análisis generado a partir de ARCH-FR-2026-001 v3.0), eliminando redundancias, resolviendo ambigüedades y enriqueciendo cada sección con el detalle técnico necesario para la implementación en el stack tecnológico seleccionado.

### 1.2 Alcance Funcional

El sistema gestiona el ciclo de vida completo de las adquisiciones realizadas mediante el mecanismo de **Fondo Revolvente** del HCG, con un límite normativo de **$75,000 MXN** por operación (Ley de Compras del Estado de Jalisco, Art. 57 del Reglamento). Comprende:

- **30 estados** distribuidos en el ciclo de vida de cada solicitud de adquisición.
- **38 transiciones** de estado gobernadas por el motor `SolicitudStateMachine`.
- **27 hitos** de proceso, distribuidos en **8 fases** secuenciales.
- **5 reglas de negocio críticas** (RN-001 a RN-005) con validación en múltiples capas.
- **6 roles funcionales** con permisos diferenciados sobre cada módulo y acción.
- Integración con el **SAT** para validación CFDI 4.0, con gestión de resiliencia ante fallos del servicio externo.
- Integración con el **directorio activo (Active Directory)** del hospital para autenticación y autorización.
- Acceso al **repositorio de archivos SMB** del hospital para gestión documental de expedientes.
- Generación de **reportes oficiales** mediante motor de reporteo dedicado (SSRS).

### 1.3 Audiencia del Documento

| Audiencia                              | Uso Principal                                                                                                                        |
| -------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------ |
| **Equipo de Diseño UX/UI**             | Referencia de composición visual, components, motion y flujos de interacción                                                         |
| **Equipo de Desarrollo Frontend**      | Especificación de componentes, comportamientos y contratos de interfaz para implementación en Uno Platform / MVVM                    |
| **Equipo de Desarrollo Backend**       | Comprensión de los requerimientos de datos, estados y respuestas que la API debe satisfacer para el correcto funcionamiento de la UI |
| **Product Manager / Owner**            | Validación funcional de flujos, reglas de negocio y criterios de aceptación                                                          |
| **Comité de Auditoría y Cumplimiento** | Verificación del cumplimiento normativo representado en la interfaz (límites, trazabilidad, roles)                                   |
| **Equipo de QA**                       | Base para la construcción de casos de prueba de interfaz, flujos y comportamientos                                                   |

### 1.4 Restricciones del Documento

- Este documento **no contiene** fragmentos de código fuente en ningún lenguaje.
- Las referencias a tecnologías de implementación (Uno Platform, CommunityToolkit.Mvvm, SignalR, Polly, EF Core, Dapper, SSRS, SMB) se incluyen en el **Apéndice E** y en notas de implementación al pie de cada módulo, para contexto de alineación técnica.
- Los componentes de interfaz se nombran usando la **nomenclatura oficial de WinUI 3 y Windows Community Toolkit**, que representa el contrato de diseño nativo Windows 11.
- Las especificaciones de comportamiento visual son **normativas**: los implementadores deben reproducir los efectos descritos usando los medios nativos del framework seleccionado.

---

## 2. Arquitectura de Presentación — Stack Tecnológico desde la Perspectiva UX

### 2.1 Modelo de Capas de la Aplicación de Escritorio

La arquitectura de presentación está organizada en capas con responsabilidades claramente diferenciadas. Desde la perspectiva del diseño de interfaz, esta separación tiene implicaciones directas en cómo se modelan los datos, se gestionan los estados y se comunican las transiciones al usuario.

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│  CAPA DE PRESENTACIÓN — Uno Platform (WinUI 3 Flavor)                           │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  Vista (XAML/Uno):  Composición visual · Fluent Design Materials · Motion       │
│  ViewModel (MVVM):  Estado reactivo de la UI · Comandos · Notificaciones        │
│  CommunityToolkit.Mvvm: Propiedades observables para los 30 estados             │
│  SignalR Client:    Recepción de eventos en tiempo real → actualiza ViewModel   │
└─────────────────────────────────────────────────────────────────────────────────┘
                           ▼ HTTPS / WSS (Red interna hospitalaria)
┌─────────────────────────────────────────────────────────────────────────────────┐
│  CAPA DE API — ASP.NET Core Web API (.NET 8/10)                                 │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  Controladores RESTful · MediatR (CQRS) · JWT Auth (Active Directory)           │
│  SignalR Hub: Broadcast de cambios de estado en tiempo real                     │
│  Polly: Circuit Breaker + Retry para integración SAT (RN-004)                   │
└─────────────────────────────────────────────────────────────────────────────────┘
                           ▼ MediatR
┌─────────────────────────────────────────────────────────────────────────────────┐
│  CAPA DE DOMINIO — Clean Architecture                                           │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  SolicitudStateMachine: 30 estados · 38 transiciones                            │
│  Reglas de Negocio: RN-001..RN-005 · BloqueoEdicionService (RN-005)             │
│  RowVersion: Bloqueo optimista para concurrencia de edición                     │
└─────────────────────────────────────────────────────────────────────────────────┘
                           ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│  CAPA DE DATOS & INFRAESTRUCTURA                                                 │
│  ─────────────────────────────────────────────────────────────────────────────  │
│  EF Core (escrituras transaccionales) · Dapper (lecturas KPI y comparativos)   │
│  SQL Server Express 2022 · SMB 3.0 (repositorio de expedientes PDF)             │
│  SSRS (generación de reportes PDF/Excel legales)                                 │
└─────────────────────────────────────────────────────────────────────────────────┘
```

### 2.2 Implicaciones de Diseño del Patrón MVVM para la Interfaz

El patrón **MVVM** (Model-View-ViewModel) con `CommunityToolkit.Mvvm` tiene implicaciones directas en el comportamiento visual de la aplicación:

**Reactividad de los 30 estados:** Cada pantalla que visualiza el estado de una solicitud no requiere recarga manual. El ViewModel expone la propiedad del estado como observable; cuando el estado cambia (ya sea por acción del usuario local o por evento en tiempo real recibido vía SignalR), todos los componentes visuales que dependen de ese estado se actualizan automáticamente. Esto significa que el `StateBadge`, la disponibilidad de botones de acción, el color de la fila en el `DataGrid` y el avance del timeline de hitos se sincronizan simultáneamente, sin transiciones visuales desincronizadas.

**Comandos de UI:** Cada botón de acción principal (Autorizar, Rechazar, Validar CFDI, Confirmar Entrega) está ligado a un **Comando** del ViewModel. El estado de habilitación del botón (`IsEnabled`) se calcula reactivamente en función del estado actual de la solicitud y del rol del usuario autenticado. Esto garantiza que un botón nunca aparezca habilitado en un estado en el que la acción no es permitida por la máquina de estados del dominio.

**Separación Comando/Consulta (CQRS) en la UI:** Las operaciones de escritura (registrar hito, autorizar solicitud, validar CFDI) generan feedback visual inmediato de progreso al usuario (estado de carga en el botón) mientras la operación se procesa. Las operaciones de lectura (cargar el DataGrid, poblar el cuadro comparativo) se ejecutan independientemente y pueden reintentarse sin afectar el estado de las escrituras en proceso. Esta separación hace que la interfaz sea perceptiblemente más responsiva: el usuario nunca ve la pantalla bloqueada esperando que una operación de lectura termine antes de poder ejecutar una acción.

**Resiliencia de la conexión SignalR:** El cliente SignalR implementa reconexión automática con estrategia de backoff exponencial. La interfaz refleja el estado de la conexión en tiempo real de manera no intrusiva (indicador en el panel de notificaciones), sin interrumpir el flujo de trabajo del usuario. Durante períodos de reconexión, la aplicación continúa siendo funcional en modo de lectura y escritura directa; simplemente las actualizaciones en tiempo real se reanuden al restaurarse la conexión.

### 2.3 Capacidades de Sistema Operativo Requeridas

Para el correcto funcionamiento de la aplicación de escritorio en el entorno hospitalario, el manifiesto de la aplicación declara las siguientes capacidades que habilitan funcionalidades específicas de la interfaz:

| Capacidad                    | Funcionalidad de UI Habilitada                                                                                                                                                                               |
| ---------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `privateNetworkClientServer` | Conexión a la API interna del hospital y al hub SignalR. Sin esta capacidad, no es posible mostrar datos en tiempo real ni ejecutar transacciones.                                                           |
| `broadFileSystemAccess`      | Adjuntar y visualizar documentos desde el servidor de archivos SMB del hospital (\\Servidor\Compras_FR\). Habilita los selectores de archivo nativos de Windows para expedientes PDF y archivos XML de CFDI. |

### 2.4 Estructura del proyecto

HCG.FondoRevolvente/
├── 📄 HCG.FondoRevolvente.sln
├── 📄 Directory.Build.props
├── 📄 global.json
├── 📄 README.md
│
├── 📁 src/
│ │
│ ├── 📁 HCG.FondoRevolvente.Domain/
│ │ ├── 📁 Aggregates/
│ │ │ ├── Solicitud.cs
│ │ │ ├── Proveedor.cs
│ │ │ ├── Cotizacion.cs
│ │ │ └── Hito.cs
│ │ ├── 📁 ValueObjects/
│ │ │ ├── FolioDSA.cs
│ │ │ ├── MontoFondoRevolvente.cs
│ │ │ └── RfcProveedor.cs
│ │ ├── 📁 Enums/
│ │ │ ├── EstadoSolicitud.cs
│ │ │ ├── FaseProceso.cs
│ │ │ ├── RolAplicacion.cs
│ │ │ └── TipoHito.cs
│ │ ├── 📁 Constants/
│ │ │ ├── LimitesNegocio.cs
│ │ │ ├── CotizacionesRequeridas.cs
│ │ │ └── ConfiguracionBloqueo.cs
│ │ ├── 📁 Exceptions/
│ │ │ ├── DomainException.cs
│ │ │ ├── MontoExcedidoException.cs
│ │ │ ├── FraccionamientoDetectadoException.cs
│ │ │ ├── CotizacionesInsuficientesException.cs
│ │ │ ├── CfdiInvalidoException.cs
│ │ │ └── BloqueoEdicionException.cs
│ │ ├── 📁 Services/
│ │ │ ├── SolicitudStateMachine.cs
│ │ │ ├── BloqueoEdicionService.cs
│ │ │ └── ValidadorFraccionamientoService.cs
│ │ ├── 📁 Events/
│ │ │ ├── SolicitudCreadaEvent.cs
│ │ │ ├── EstadoCambiadoEvent.cs
│ │ │ └── HitoRegistradoEvent.cs
│ │ └── 📁 Interfaces/
│ │ ├── ISolicitudStateMachine.cs
│ │ ├── IBloqueoEdicionService.cs
│ │ └── IRepository.cs
│ │
│ ├── 📁 HCG.FondoRevolvente.Application/
│ │ ├── 📁 Common/
│ │ │ ├── 📁 Behaviors/
│ │ │ │ ├── ValidationBehavior.cs
│ │ │ │ ├── LoggingBehavior.cs
│ │ │ │ └── TransactionBehavior.cs
│ │ │ ├── 📁 Models/
│ │ │ │ ├── Result.cs
│ │ │ │ └── PaginatedList.cs
│ │ │ └── 📁 Mappings/
│ │ │ └── MappingProfile.cs
│ │ ├── 📁 Solicitudes/
│ │ │ ├── 📁 Commands/
│ │ │ │ ├── 📁 CrearSolicitud/
│ │ │ │ │ ├── CrearSolicitudCommand.cs
│ │ │ │ │ ├── CrearSolicitudCommandHandler.cs
│ │ │ │ │ ├── CrearSolicitudCommandValidator.cs
│ │ │ │ │ └── CrearSolicitudResponse.cs
│ │ │ │ ├── 📁 TransicionarEstado/
│ │ │ │ │ ├── TransicionarEstadoCommand.cs
│ │ │ │ │ └── TransicionarEstadoCommandHandler.cs
│ │ │ │ ├── 📁 RegistrarHito/
│ │ │ │ │ ├── RegistrarHitoCommand.cs
│ │ │ │ │ └── RegistrarHitoCommandHandler.cs
│ │ │ │ ├── 📁 AdquirirBloqueo/
│ │ │ │ │ ├── AdquirirBloqueoCommand.cs
│ │ │ │ │ └── AdquirirBloqueoCommandHandler.cs
│ │ │ │ └── 📁 LiberarBloqueo/
│ │ │ │ ├── LiberarBloqueoCommand.cs
│ │ │ │ └── LiberarBloqueoCommandHandler.cs
│ │ │ ├── 📁 Queries/
│ │ │ │ ├── 📁 ListarSolicitudes/
│ │ │ │ │ ├── ListarSolicitudesQuery.cs
│ │ │ │ │ ├── ListarSolicitudesQueryHandler.cs
│ │ │ │ │ └── SolicitudListItemDto.cs
│ │ │ │ ├── 📁 ObtenerSolicitudPorFolio/
│ │ │ │ │ ├── ObtenerSolicitudPorFolioQuery.cs
│ │ │ │ │ ├── ObtenerSolicitudPorFolioQueryHandler.cs
│ │ │ │ │ └── SolicitudDetalleDto.cs
│ │ │ │ └── 📁 ObtenerExpedienteCompleto/
│ │ │ │ ├── ObtenerExpedienteCompletoQuery.cs
│ │ │ │ └── ExpedienteCompletoDto.cs
│ │ │ └── 📁 EventHandlers/
│ │ │ └── SolicitudCreadaEventHandler.cs
│ │ ├── 📁 Cotizaciones/
│ │ │ ├── 📁 Commands/
│ │ │ │ ├── 📁 AgregarCotizacion/
│ │ │ │ │ ├── AgregarCotizacionCommand.cs
│ │ │ │ │ ├── AgregarCotizacionCommandHandler.cs
│ │ │ │ │ └── AgregarCotizacionCommandValidator.cs
│ │ │ │ └── 📁 SeleccionarProveedor/
│ │ │ │ ├── SeleccionarProveedorCommand.cs
│ │ │ │ └── SeleccionarProveedorCommandHandler.cs
│ │ │ └── 📁 Queries/
│ │ │ └── 📁 ObtenerCuadroComparativo/
│ │ │ ├── ObtenerCuadroComparativoQuery.cs
│ │ │ └── CuadroComparativoDto.cs
│ │ ├── 📁 ValidacionFiscal/
│ │ │ ├── 📁 Commands/
│ │ │ │ └── 📁 ValidarCfdi/
│ │ │ │ ├── ValidarCfdiCommand.cs
│ │ │ │ ├── ValidarCfdiCommandHandler.cs
│ │ │ │ └── ValidarCfdiResponse.cs
│ │ │ └── 📁 Queries/
│ │ │ └── 📁 ObtenerEstadoValidacion/
│ │ │ ├── ObtenerEstadoValidacionQuery.cs
│ │ │ └── EstadoValidacionDto.cs
│ │ ├── 📁 Proveedores/
│ │ │ ├── 📁 Commands/
│ │ │ │ └── 📁 CrearProveedor/
│ │ │ │ ├── CrearProveedorCommand.cs
│ │ │ │ └── CrearProveedorCommandHandler.cs
│ │ │ └── 📁 Queries/
│ │ │ └── 📁 BuscarProveedores/
│ │ │ ├── BuscarProveedoresQuery.cs
│ │ │ └── ProveedorDto.cs
│ │ ├── 📁 Dashboard/
│ │ │ └── 📁 Queries/
│ │ │ └── 📁 ObtenerKpiDashboard/
│ │ │ ├── ObtenerKpiDashboardQuery.cs
│ │ │ └── KpiDashboardDto.cs
│ │ ├── 📁 Reportes/
│ │ │ └── 📁 Queries/
│ │ │ ├── 📁 GenerarCuadroComparativo/
│ │ │ ├── 📁 GenerarReporteMensual/
│ │ │ └── 📁 GenerarExpedientePdf/
│ │ └── 📁 Interfaces/
│ │ ├── IApplicationDbContext.cs
│ │ ├── ICurrentUserService.cs
│ │ ├── IDateTimeService.cs
│ │ ├── ISatValidationService.cs
│ │ ├── ISmbFileService.cs
│ │ ├── ISsrsReportingService.cs
│ │ └── ISignalRNotificationService.cs
│ │
│ ├── 📁 HCG.FondoRevolvente.Infrastructure/
│ │ ├── 📁 Identity/
│ │ │ ├── ActiveDirectoryAuthenticationService.cs
│ │ │ ├── JwtTokenService.cs
│ │ │ └── CurrentUserService.cs
│ │ ├── 📁 ExternalServices/
│ │ │ ├── 📁 Sat/
│ │ │ │ ├── SatValidationService.cs
│ │ │ │ └── SatOptions.cs
│ │ │ ├── 📁 Smb/
│ │ │ │ ├── SmbFileService.cs
│ │ │ │ └── SmbOptions.cs
│ │ │ └── 📁 Ssrs/
│ │ │ ├── SsrsReportingService.cs
│ │ │ └── SsrsOptions.cs
│ │ ├── 📁 Resilience/
│ │ │ ├── SatCircuitBreakerPolicy.cs
│ │ │ ├── RetryPolicies.cs
│ │ │ └── PolicyRegistry.cs
│ │ ├── 📁 Security/
│ │ │ ├── RfcEncryptionService.cs
│ │ │ └── DataProtectionService.cs
│ │ ├── 📁 Notifications/
│ │ │ └── SignalRNotificationService.cs
│ │ └── 📁 Configuration/
│ │ ├── DependencyInjection.cs
│ │ └── InfrastructureSettings.cs
│ │
│ ├── 📁 HCG.FondoRevolvente.Persistence/
│ │ ├── ApplicationDbContext.cs
│ │ ├── 📁 Configurations/
│ │ │ ├── SolicitudConfiguration.cs
│ │ │ ├── ProveedorConfiguration.cs
│ │ │ ├── CotizacionConfiguration.cs
│ │ │ └── HitoConfiguration.cs
│ │ ├── 📁 Repositories/
│ │ │ ├── EfRepositoryBase.cs
│ │ │ ├── SolicitudRepository.cs
│ │ │ └── ProveedorRepository.cs
│ │ ├── 📁 ReadRepositories/
│ │ │ ├── DapperReadRepositoryBase.cs
│ │ │ ├── SolicitudReadRepository.cs
│ │ │ └── ReporteReadRepository.cs
│ │ ├── 📁 SqlQueries/
│ │ │ ├── GetCuadroComparativo.sql
│ │ │ ├── GetExpedienteCompleto.sql
│ │ │ └── GetDashboardKpi.sql
│ │ ├── 📁 Interceptors/
│ │ │ ├── AuditableEntityInterceptor.cs
│ │ │ └── RowVersionInterceptor.cs
│ │ ├── 📁 Migrations/
│ │ └── 📁 Configuration/
│ │ └── DependencyInjection.cs
│ │
│ ├── 📁 HCG.FondoRevolvente.Api/
│ │ ├── 📄 Program.cs
│ │ ├── 📄 appsettings.json
│ │ ├── 📄 appsettings.Development.json
│ │ ├── 📄 appsettings.Production.json
│ │ ├── 📁 Controllers/
│ │ │ ├── ApiControllerBase.cs
│ │ │ ├── SolicitudesController.cs
│ │ │ ├── CotizacionesController.cs
│ │ │ ├── ProveedoresController.cs
│ │ │ ├── ValidacionFiscalController.cs
│ │ │ ├── ReportesController.cs
│ │ │ ├── DashboardController.cs
│ │ │ └── AuthController.cs
│ │ ├── 📁 Hubs/
│ │ │ ├── SolicitudHub.cs
│ │ │ └── HubConstants.cs
│ │ ├── 📁 Middleware/
│ │ │ ├── ExceptionHandlingMiddleware.cs
│ │ │ ├── RequestLoggingMiddleware.cs
│ │ │ └── CurrentUserMiddleware.cs
│ │ ├── 📁 Filters/
│ │ │ └── ApiExceptionFilterAttribute.cs
│ │ └── 📁 Configuration/
│ │ ├── DependencyInjection.cs
│ │ └── SwaggerConfiguration.cs
│ │
│ └── 📁 HCG.FondoRevolvente.Client/
│ ├── 📄 App.xaml
│ ├── 📄 App.xaml.cs
│ │
│ ├── 📁 Platforms/
│ │ └── 📁 Windows/
│ │ ├── 📄 Package.appxmanifest
│ │ └── 📄 app.manifest
│ │
│ ├── 📁 Resources/
│ │ ├── 📄 AppResources.xaml
│ │ ├── 📁 Styles/
│ │ │ ├── 📄 ColoresSemanticos.xaml
│ │ │ ├── 📄 TipografiaSegoeVariable.xaml
│ │ │ ├── 📄 AnimacionesEstandar.xaml
│ │ │ ├── 📄 ButtonStyles.xaml
│ │ │ ├── 📄 TextBoxStyles.xaml
│ │ │ ├── 📄 DataGridStyles.xaml
│ │ │ ├── 📄 NavigationViewStyles.xaml
│ │ │ └── 📄 InfoBarStyles.xaml
│ │ ├── 📁 Fonts/
│ │ └── 📁 Images/
│ │ ├── 📄 logo_hcg_light.png
│ │ └── 📄 logo_hcg_dark.png
│ │
│ ├── 📁 Controls/
│ │ ├── 📁 Badges/
│ │ │ ├── 📄 StateBadge.xaml
│ │ │ ├── 📄 StateBadge.xaml.cs
│ │ │ ├── 📄 StateChip.xaml
│ │ │ ├── 📄 StateChip.xaml.cs
│ │ │ └── 📄 StateBadgeStyles.xaml
│ │ ├── 📁 Indicators/
│ │ │ ├── 📄 BloqueoIndicator.xaml
│ │ │ ├── 📄 BloqueoIndicator.xaml.cs
│ │ │ ├── 📄 MontoDisplay.xaml
│ │ │ └── 📄 MontoDisplay.xaml.cs
│ │ ├── 📁 Data/
│ │ │ ├── 📄 SolicitudDataGrid.xaml
│ │ │ ├── 📄 SolicitudDataGrid.xaml.cs
│ │ │ └── 📄 HitoTimelineControl.xaml
│ │ ├── 📁 Charts/
│ │ │ └── 📄 KpiAreaChart.xaml
│ │ └── 📁 Shell/
│ │ ├── 📄 CustomTitleBar.xaml
│ │ └── 📄 CustomTitleBar.xaml.cs
│ │
│ ├── 📁 Dialogs/
│ │ ├── 📄 ConfirmacionAccionDialog.xaml
│ │ ├── 📄 ConfirmacionAccionDialog.xaml.cs
│ │ ├── 📄 BloqueoOcupadoDialog.xaml
│ │ ├── 📄 RegistrarHitoDialog.xaml
│ │ ├── 📄 NuevaCotizacionDialog.xaml
│ │ └── 📄 NuevoProveedorDialog.xaml
│ │
│ ├── 📁 Templates/
│ │ ├── 📄 SolicitudItemTemplate.xaml
│ │ ├── 📄 HitoItemTemplate.xaml
│ │ ├── 📄 NotificacionItemTemplate.xaml
│ │ └── 📄 KpiCardTemplate.xaml
│ │
│ ├── 📁 Converters/
│ │ ├── 📄 EstadoToColorConverter.cs
│ │ ├── 📄 EstadoToIconConverter.cs
│ │ ├── 📄 EstadoToFaseConverter.cs
│ │ ├── 📄 MontoToProgressConverter.cs
│ │ ├── 📄 RolToVisibilityConverter.cs
│ │ ├── 📄 RfcMaskConverter.cs
│ │ ├── 📄 MontoToStringConverter.cs
│ │ └── 📄 BooleanToOpacityConverter.cs
│ │
│ ├── 📁 Behaviors/
│ │ ├── 📄 RevealFocusBehavior.cs
│ │ ├── 📄 ShakeAnimationBehavior.cs
│ │ └── 📄 ScrollToActivePhaseBehavior.cs
│ │
│ ├── 📁 Helpers/
│ │ ├── 📄 AnimationHelper.cs
│ │ ├── 📄 FluentMaterialHelper.cs
│ │ ├── 📄 FilePickerHelper.cs
│ │ └── 📄 ToastNotificationHelper.cs
│ │
│ ├── 📁 Services/
│ │ ├── 📁 Navigation/
│ │ │ ├── 📄 INavigationService.cs
│ │ │ └── 📄 NavigationService.cs
│ │ ├── 📁 Dialog/
│ │ │ ├── 📄 IDialogService.cs
│ │ │ └── 📄 DialogService.cs
│ │ ├── 📁 Notification/
│ │ │ ├── 📄 IUiNotificationService.cs
│ │ │ └── 📄 UiNotificationService.cs
│ │ ├── 📁 SignalR/
│ │ │ ├── 📄 ISignalRClientService.cs
│ │ │ └── 📄 SignalRClientService.cs
│ │ ├── 📁 Authentication/
│ │ │ ├── 📄 IAuthenticationClientService.cs
│ │ │ └── 📄 AuthenticationClientService.cs
│ │ ├── 📁 Api/
│ │ │ ├── 📄 IApiClientService.cs
│ │ │ ├── 📄 ApiClientService.cs
│ │ │ └── 📄 ApiClientOptions.cs
│ │ └── 📁 Settings/
│ │ ├── 📄 ILocalSettingsService.cs
│ │ └── 📄 LocalSettingsService.cs
│ │
│ ├── 📁 ViewModels/
│ │ ├── 📁 Base/
│ │ │ ├── 📄 ViewModelBase.cs
│ │ │ └── 📄 ValidatableViewModelBase.cs
│ │ ├── 📁 Module01_Authentication/
│ │ │ └── 📄 LoginViewModel.cs
│ │ ├── 📁 Module02_Shell/
│ │ │ ├── 📄 ShellViewModel.cs
│ │ │ └── 📄 NotificationBadgeViewModel.cs
│ │ ├── 📁 Module03_Dashboard/
│ │ │ └── 📄 DashboardViewModel.cs
│ │ ├── 📁 Module04_SolicitudList/
│ │ │ └── 📄 SolicitudListViewModel.cs
│ │ ├── 📁 Module05_SolicitudCreate/
│ │ │ ├── 📄 SolicitudCreateViewModel.cs
│ │ │ └── 📄 PartidaItemViewModel.cs
│ │ ├── 📁 Module06_SolicitudDetail/
│ │ │ └── 📄 SolicitudDetailViewModel.cs
│ │ ├── 📁 Module07_Timeline/
│ │ │ ├── 📄 TimelineViewModel.cs
│ │ │ └── 📄 HitoItemViewModel.cs
│ │ ├── 📁 Module08_Proveedores/
│ │ │ └── 📄 ProveedorViewModel.cs
│ │ ├── 📁 Module09_Cotizaciones/
│ │ │ ├── 📄 CotizacionViewModel.cs
│ │ │ └── 📄 CuadroComparativoViewModel.cs
│ │ ├── 📁 Module10_ValidacionCfdi/
│ │ │ └── 📄 CfdiValidationViewModel.cs
│ │ ├── 📁 Module11_Reportes/
│ │ │ └── 📄 ReportesViewModel.cs
│ │ ├── 📁 Module12_Notificaciones/
│ │ │ └── 📄 NotificationCenterViewModel.cs
│ │ └── 📁 Module13_Administracion/
│ │ ├── 📄 AdminViewModel.cs
│ │ ├── 📄 UsuariosRolesViewModel.cs
│ │ ├── 📄 AuditoriaViewModel.cs
│ │ └── 📄 ConfiguracionSistemaViewModel.cs
│ │
│ └── 📁 Views/
│ ├── 📁 Module01_Authentication/
│ │ ├── 📄 LoginPage.xaml
│ │ └── 📄 LoginPage.xaml.cs
│ ├── 📁 Module02_Shell/
│ │ ├── 📄 ShellPage.xaml
│ │ └── 📄 ShellPage.xaml.cs
│ ├── 📁 Module03_Dashboard/
│ │ ├── 📄 DashboardPage.xaml
│ │ └── 📄 DashboardPage.xaml.cs
│ ├── 📁 Module04_SolicitudList/
│ │ ├── 📄 SolicitudListPage.xaml
│ │ └── 📄 SolicitudListPage.xaml.cs
│ ├── 📁 Module05_SolicitudCreate/
│ │ ├── 📄 SolicitudCreatePage.xaml
│ │ └── 📄 SolicitudCreatePage.xaml.cs
│ ├── 📁 Module06_SolicitudDetail/
│ │ ├── 📄 SolicitudDetailPage.xaml
│ │ └── 📄 SolicitudDetailPage.xaml.cs
│ ├── 📁 Module07_Timeline/
│ │ ├── 📄 TimelinePage.xaml
│ │ └── 📄 TimelinePage.xaml.cs
│ ├── 📁 Module08_Proveedores/
│ │ ├── 📄 ProveedorPage.xaml
│ │ └── 📄 ProveedorPage.xaml.cs
│ ├── 📁 Module09_Cotizaciones/
│ │ ├── 📄 CotizacionPage.xaml
│ │ └── 📄 CotizacionPage.xaml.cs
│ ├── 📁 Module10_ValidacionCfdi/
│ │ ├── 📄 CfdiValidationPage.xaml
│ │ └── 📄 CfdiValidationPage.xaml.cs
│ ├── 📁 Module11_Reportes/
│ │ ├── 📄 ReportesPage.xaml
│ │ └── 📄 ReportesPage.xaml.cs
│ ├── 📁 Module12_Notificaciones/
│ │ ├── 📄 NotificationCenterFlyout.xaml
│ │ └── 📄 NotificationCenterFlyout.xaml.cs
│ └── 📁 Module13_Administracion/
│ ├── 📄 AdminPage.xaml
│ └── 📄 AdminPage.xaml.cs
│
└── 📁 tests/
├── 📁 HCG.FondoRevolvente.Domain.Tests/
│ ├── 📄 SolicitudStateMachineTests.cs
│ └── 📄 BusinessRulesTests.cs
├── 📁 HCG.FondoRevolvente.Application.Tests/
│ ├── 📄 CrearSolicitudCommandHandlerTests.cs
│ └── 📄 ValidarCfdiCommandHandlerTests.cs
├── 📁 HCG.FondoRevolvente.Integration.Tests/
│ └── 📄 SolicitudLifecycleTests.cs
└── 📁 HCG.FondoRevolvente.Client.Tests/
└── 📄 ViewModelTests.cs

---

## 3. Modelo de Roles, Permisos y Seguridad de Interfaz

### 3.1 Grupos de Seguridad y Roles de la Aplicación

La autenticación se realiza contra el **Active Directory** del hospital. Los grupos de AD determinan el JWT que recibe el cliente, que a su vez determina el rol de la sesión activa. La interfaz aplica **seguridad de presentación**: los elementos a los que un rol no tiene acceso no se deshabilitan — directamente no se renderizan. Esto aplica tanto a ítems del menú de navegación como a botones de acción y columnas de datos sensibles.

| Grupo Active Directory    | Rol en la Aplicación | Descripción Funcional                                                          |
| ------------------------- | -------------------- | ------------------------------------------------------------------------------ |
| `Administradores_Sistema` | **Administrador**    | Acceso completo a todos los módulos, acciones y datos del sistema.             |
| `Compradores_DSA`         | **Comprador DSA**    | Creación y gestión de solicitudes propias. Cotizaciones, proveedores.          |
| `CAA_Miembros`            | **Revisor CAA**      | Consulta de expedientes en estado de autorización CAA. Autorización y rechazo. |
| `Recursos_Financieros`    | **Finanzas**         | Gestión de hitos de validación fiscal (CFDI), pago y complemento.              |
| `Almacen_Staff`           | **Almacén**          | Confirmación de hitos de entrega y recepción de bienes.                        |
| `DSA_Staff`               | **Consulta DSA**     | Acceso de solo lectura a solicitudes, reportes y dashboard.                    |

### 3.2 Principio de Mínimo Privilegio en la Interfaz

La interfaz implementa el principio de **mínimo privilegio visual**: cada usuario ve únicamente lo que necesita para ejecutar su función. Los efectos concretos son:

- El menú de navegación (_NavigationView_) solo muestra los ítems relevantes al rol autenticado.
- Los botones de acción en el panel de Detalle de Solicitud se renderizan únicamente si el rol actual tiene permiso para ejecutar la acción **y** el estado actual de la solicitud permite esa transición.
- Los datos financieros detallados (RFC completo de proveedores, montos en detalle de partidas fiscales) son visibles solo para los roles con acceso a esa información.
- El filtrado del `DataGrid` de solicitudes aplica automáticamente el scope del rol: el Comprador DSA ve solo sus propias solicitudes; el Revisor CAA ve solo las que están en estados de autorización CAA; el rol de Almacén ve solo las que están en estados de entrega.

### 3.3 Gestión Visual del Bloqueo de Edición (RN-005)

El sistema implementa **bloqueo optimista de edición** para prevenir conflictos de concurrencia cuando dos usuarios intentan editar la misma solicitud simultáneamente. La lógica de bloqueo reside en el dominio (`BloqueoEdicionService`, campo `RowVersion`), pero su representación visual es parte integral del diseño de interfaz:

- Al intentar editar un expediente bloqueado por otro usuario, el sistema no lanza un error críptico: despliega un `ContentDialog` informativo con material _Acrylic_ que indica claramente: _"Este expediente está siendo editado por [Nombre Completo del Usuario] desde las [HH:MM]. Por favor, espere o contacte al usuario para coordinarse."_ El expediente se abre en modo de solo lectura.
- Si el usuario actual tiene el bloqueo activo, un _BloqueoIndicator_ prominente aparece en el encabezado del expediente (ícono de candado abierto en color acento + texto "[Tu nombre] — Editando ahora"). El bloqueo se renueva automáticamente en segundo plano cada 25 minutos (el TTL del servidor es de 30 minutos), sin interrumpir al usuario.
- Al abandonar la pantalla de edición (por navegación o cierre de la aplicación), el bloqueo se libera automáticamente. Si la sesión se cierra inesperadamente (crash, desconexión), el bloqueo expira automáticamente por TTL.

---

## 4. Sistema Global de Diseño (Design System)

### 4.1 Lenguaje Visual — Fluent Design System v2

La experiencia visual de la aplicación se fundamenta en los principios del **Microsoft Fluent Design System v2**, optimizado para Windows 11. Los cuatro pilares del lenguaje visual son:

**Materiales y Profundidad:** La aplicación usa una jerarquía de materiales que comunica la profundidad de cada capa de la interfaz. El material _Mica_ es el fondo base de todas las ventanas de la aplicación, creando integración visual con el escritorio del usuario al absorber y adaptarse a su color de acento y fondo de escritorio. El material _Acrylic_ se reserva para elementos superpuestos (paneles laterales, menús desplegables, tooltips enriquecidos) que necesitan comunicar su posición por encima del plano principal. El material _Smoke_ (oscurecimiento semitransparente del fondo) se aplica exclusivamente como contexto para `ContentDialog` modales críticos, comunicando que la interacción actual requiere atención completa antes de continuar.

**Tipografía Segoe UI Variable:** La jerarquía tipográfica utiliza la familia _Segoe UI Variable_, la fuente de sistema de Windows 11 que se adapta ópticamente según el tamaño y el contexto. Los niveles de la jerarquía son:

- _Display_ (28-36pt, Bold): Títulos de pantalla de alta jerarquía, números KPI prominentes.
- _Title_ (20-24pt, SemiBold): Encabezados de módulo, nombres de solicitudes en vista de detalle.
- _Subtitle_ (16-18pt, SemiBold): Encabezados de sección dentro de pantallas.
- _Body_ (14pt, Regular / SemiBold): Contenido principal, etiquetas de campos, texto de botones.
- _Caption_ (12pt, Regular): Metadatos secundarios, timestamps, texto de ayuda contextual.
- _Label_ (11pt, Regular): Etiquetas de columnas en tablas, textos de navegación colapsada.

**Iconografía Segoe Fluent Icons:** Todos los íconos de la aplicación provienen de la familia _Segoe Fluent Icons_ (la familia modernizada de Windows 11), que proporciona íconos de línea limpia con variantes de relleno para estados activos. Los íconos de la familia anterior _Segoe MDL2 Assets_ se aceptan donde no exista equivalente en Fluent Icons.

**Elevación y Sombras:** El sistema de elevación comunica la jerarquía z-order de los elementos:

- Shadow Depth 0: Elementos integrados en el plano de fondo (campos de texto embebidos, dividers).
- Shadow Depth 4: Tarjetas de contenido (_CardPanel_), paneles informativos.
- Shadow Depth 8: Panel de navegación, barras de herramientas fijas.
- Shadow Depth 16: Paneles superpuestos abiertos (_Flyout_, _SplitView_).
- Shadow Depth 64: `ContentDialog` modales.
- Shadow Depth 128: Ventanas de autenticación y ventanas secundarias flotantes.

### 4.2 Paleta de Colores del Sistema

La paleta de colores combina los colores de acento del sistema operativo del usuario (que cambian según su personalización de Windows 11) con colores semánticos fijos que comunican estados de proceso y alertas de negocio.

**Colores Semánticos Fijos (independientes del acento del sistema):**

| Token de Color             | Hex       | Uso                                                        |
| -------------------------- | --------- | ---------------------------------------------------------- |
| `SystemFillColorSuccess`   | `#6CCB5F` | Confirmaciones, hitos completados, validaciones exitosas   |
| `SystemFillColorCritical`  | `#C50F1F` | Estados de error, rechazos, validaciones fallidas          |
| `SystemFillColorCaution`   | `#F7630C` | Advertencias, alertas de proceso, estados de reintento     |
| `SystemFillColorAttention` | `#005FB7` | Información relevante, actualizaciones en tiempo real      |
| `SystemFillColorNeutral`   | `#69797E` | Estados neutros, solicitudes cerradas, elementos inactivos |

### 4.3 Componentes de Diseño Transversales

Los siguientes componentes son utilizados de manera consistente en todos los módulos de la aplicación. Su especificación aquí es normativa y aplica a todas las instancias.

**StateBadge (Pastilla de Estado):**
Componente reutilizable que representa el estado actual de una solicitud. Disponible en tres variantes de tamaño:

- _Small_ (altura 20px): Para uso en columnas de `DataGrid` con alta densidad de información.
- _Medium_ (altura 28px): Para uso estándar en tarjetas, paneles de vista previa y listas.
- _Large_ (altura 36px): Para el encabezado de la vista de Detalle de Solicitud, donde el estado debe ser inmediatamente prominente.

Cada instancia del _StateBadge_ incluye: ícono Segoe Fluent Icons representativo de la fase, texto del estado en tipografía _Caption_ SemiBold, y fondo de color correspondiente a la fase (ver Apéndice A). Los estados en proceso activo (`EN_*`) muestran un gradiente sutil animado en el fondo. Los estados de error (`*_INVALIDO`, `ERROR_*`, `RECHAZADO_*`) muestran animación _Pulse_ de borde. Los estados terminales (`CERRADO`, `CANCELADO`) tienen opacidad reducida al 85%.

**StateChip:**
Variante ultra-compacta del _StateBadge_, utilizada en contextos de muy alta densidad (tooltips, listas secundarias). Consiste únicamente en un punto de color (10px de diámetro) seguido del texto del estado en tipografía _Label_ Regular.

**BloqueoIndicator:**
Panel de advertencia contextual que aparece en el encabezado de cualquier expediente bajo edición. Contiene: ícono de candado (_LockFeedback_), texto "[Usuario] — Editando desde [HH:MM]", y un botón _HyperlinkButton_ "Liberar bloqueo" (visible solo si el usuario actual es quien tiene el bloqueo). Su fondo usa el color `SystemFillColorCaution` con opacidad del 15%, borde izquierdo de 3px en color caución.

**MontoDisplay:**
Componente de visualización monetaria normalizada. Formatea cualquier valor numérico como `$XX,XXX.XX MXN` usando la localización `es-MX`. En contextos de formulario, muestra adicionalmente una barra de progreso lineal que representa el porcentaje del límite de $75,000 MXN. El color de la barra transiciona de verde (< 70% del límite) a amarillo (70-90%) a rojo (> 90% o excedido).

---

## 🖥️ Módulo 01: Pantalla de Autenticación

### Propósito Funcional

Punto de entrada único e insalvable al sistema. Valida las credenciales institucionales del usuario contra el **directorio activo (Active Directory) del Hospital Civil de Guadalajara**, recupera los grupos de seguridad a los que pertenece, construye el contexto de permisos de la sesión y redirige al usuario al módulo de destino correspondiente a su rol. Ningún elemento funcional del sistema es accesible sin completar esta validación. El sistema implementa bloqueo progresivo de cuenta tras intentos fallidos repetidos.

### Composición Visual (Fluent Design)

La pantalla de autenticación se presenta como una ventana de dimensiones compactas y fijas (480 × 580 píxeles de área de contenido) centrada sobre el escritorio del usuario. La ventana no tiene bordes estándar de Windows; en su lugar, utiliza bordes redondeados de 12 píxeles con efecto de sombra profunda (Shadow Depth 128) que la hace flotar visualmente sobre el escritorio.

El material base de la ventana es **Acrylic** de alta saturación en su variante `AcrylicBackgroundFillColorDefaultBrush`, con un valor de luminosidad (_Tint Opacity_) de 0.85 en tema claro y 0.70 en tema oscuro. El fondo del escritorio se percibe a través del glasmorfismo creado por este material, reforzando la sensación de profundidad y la integración con el entorno del usuario. La ventana adopta automáticamente el color de acento del sistema para su borde exterior y el indicador de foco de los campos.

La composición interior se organiza verticalmente en cinco zonas:

1. **Zona de Identidad** (top, 120px): Logotipo institucional HCG centrado con tratamiento adaptativo al tema (versión oscura del logotipo en tema claro, versión clara en tema oscuro), seguido del título del sistema.
2. **Zona de Credenciales** (central, 160px): Campos de entrada con separación de 12px entre ellos.
3. **Zona de Recuperación** (central-inferior, 24px): Enlace de soporte técnico, alineado a la derecha.
4. **Zona de Acción** (inferior, 48px): Botón de acción principal de ancho completo.
5. **Zona de Estado** (base, variable): Barra de información contextual que emerge solo cuando existe un mensaje relevante (error, advertencia, información).

### Elementos de Interfaz (WinUI 3 & Toolkit)

| Componente                                       | Especificación Detallada                                                                                                                                                                                                                                                                                                                                              |
| ------------------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Contenedor Principal (Border)**                | Bordes redondeados 12px, material Acrylic, Shadow Depth 128, sin barra de título nativa. Padding interno: 32px horizontal, 40px vertical.                                                                                                                                                                                                                             |
| **Logotipo Institucional (Image)**               | Dimensiones 120×48px, centrado horizontalmente. Fuentes de imagen: versión SVG/PNG de alta resolución del HCG. Tratamiento adaptativo según el tema activo del sistema.                                                                                                                                                                                               |
| **Título del Sistema (TextBlock)**               | "Fondo Revolvente · HCG" · Segoe UI Variable Display · 20pt · SemiBold · `TextFillColorPrimary` · Centrado.                                                                                                                                                                                                                                                           |
| **Subtítulo (TextBlock)**                        | "Inicie sesión con sus credenciales institucionales" · Segoe UI Variable Text · 12pt · Regular · `TextFillColorSecondary` · Centrado · Margin-top: 4px.                                                                                                                                                                                                               |
| **Campo de Usuario (TextBox)**                   | Etiqueta superior "Usuario de red" · Placeholder "dominio\usuario" · Ícono prefijo: glyph _Contact_ (Segoe Fluent Icons) · Autocompletar: dominio institucional. Validación de formato en tiempo real (no vacío). Borde `SystemFillColorCritical` si vacío al intentar enviar.                                                                                        |
| **Campo de Contraseña (PasswordBox)**            | Etiqueta superior "Contraseña" · Placeholder "••••••••" · Ícono prefijo: glyph _Lock_ · Botón de revelación integrado en el lado derecho (revela mientras se presiona y mantiene, oculta al soltar). Tecla _Enter_ activa el proceso de validación.                                                                                                                   |
| **Enlace de Soporte (HyperlinkButton)**          | Texto "¿Problemas para ingresar?" · Tipografía 12pt Regular · Alineado a la derecha · Color de acento del sistema · Acción: abre el cliente de correo con dirección de soporte TI preconfigurada.                                                                                                                                                                     |
| **Botón Principal (Button — AccentButtonStyle)** | Texto "Iniciar sesión" · Ancho completo del diálogo · Height 40px · CornerRadius 4px · Color de acento del sistema. Estados: Default, Hover (oscurece 10%), Pressed (oscurece 20%, escala 0.98), Disabled (opacidad 40%), Loading (oculta texto, muestra ProgressRing).                                                                                               |
| **Indicador de Carga (ProgressRing)**            | Diámetro 20px · Color de acento · Superpuesto sobre el texto del botón durante la validación · Animación de entrada: FadeIn + ScaleFrom(0.8) en 150ms.                                                                                                                                                                                                                |
| **Barra de Estado (InfoBar)**                    | Colapsada por defecto · Emerge desde la parte superior del contenedor con SlideInFromTop (200ms, EaseOutQuart) cuando hay mensajes. Variantes: Error (credenciales incorrectas, cuenta bloqueada), Warning (problema de conectividad), Informational (mantenimiento programado). Incluye botón de cierre manual y, en variante Warning, botón de acción "Reintentar". |

### Comportamiento Visual y Motion (UX)

**Secuencia de Entrada de la Aplicación:**
Al iniciar la aplicación, la ventana de autenticación emerge desde el plano del escritorio mediante `EntranceThemeTransition` (300ms, curva `EaseOut`), creando la sensación de surgir desde el fondo. Los elementos internos aplican una secuencia _StaggeredAnimation_ con retrasos progresivos de 50ms entre cada elemento (logotipo → título → subtítulo → campo usuario → campo contraseña → enlace → botón), generando un efecto de cascada descendente que dirige naturalmente la atención del usuario hacia el campo de usuario, donde se coloca el foco automáticamente.

**Interacción con Campos:**
Al recibir foco, cada campo muestra un _RevealFocusVisual_: un borde luminoso de 2px en el color de acento del sistema con efecto _Glow_ sutil que simula iluminación interna. La transición de foco entre campo de usuario y campo de contraseña (mediante Tab o clic) usa una animación de deslizamiento del indicador de foco (_RevealFocus_) que comunica la dirección del movimiento.

**Estado de Carga (Validación):**
Al presionar "Iniciar sesión", el texto del botón ejecuta _FadeOut_ (150ms) mientras simultáneamente el _ProgressRing_ ejecuta `FadeIn + ScaleFrom(0.8)` (150ms). El botón entra en estado Disabled. Los campos de entrada también entran en estado de solo lectura durante la validación, previniendo modificaciones en proceso.

**Escenario Exitoso:**
El _ProgressRing_ ejecuta _FadeOut_. El botón muestra brevemente un ícono de checkmark con animación _PopIn_ (escala 0→1.2→1.0, 400ms). Tras una pausa de 200ms (permitir al usuario percibir el éxito), el diálogo completo ejecuta `ScaleTransform(1.0→0.0) + FadeOut` (250ms). La transición hacia el Shell Principal usa _ConnectedAnimation_ del logotipo institucional: el logotipo "viaja" desde su posición en el diálogo de autenticación hasta su posición en el encabezado del _NavigationView_, manteniendo continuidad visual entre las dos pantallas.

**Escenario de Error — Credenciales Inválidas:**
El _ProgressRing_ ejecuta _FadeOut_. Ambos campos ejecutan _ShakeAnimation_ horizontal (±8px, 3 oscilaciones, 400ms, curva lineal), comunicando el rechazo de manera intuitiva. El _InfoBar_ de Error emerge con _SlideInFromTop + EaseOutBack_ (200ms), mostrando el mensaje correspondiente al intento ("Credenciales incorrectas. Intento 2 de 3."). El campo de contraseña limpia su contenido automáticamente y recibe el foco.

**Escenario de Cuenta Bloqueada (3 intentos fallidos):**
Ambos campos se deshabilitan (opacidad 40%, `IsEnabled=false`). El _InfoBar_ de Error muestra: "Cuenta bloqueada por seguridad. Contacte al área de TI del HCG para desbloquear su acceso." El botón "Iniciar sesión" también se deshabilita. El _HyperlinkButton_ de soporte ejecuta una animación _Pulse_ sutil para atraer la atención hacia la opción de contacto.

**Escenario de Error de Conectividad:**
El _InfoBar_ de Warning emerge indicando: "No es posible conectar con el directorio del hospital. Verifique su conexión a la red interna." Se agrega un botón de acción "Reintentar" dentro del _InfoBar_. Los campos permanecen habilitados para cuando se restaure la conexión.

### Flujo Operativo (User Journey)

1. **Inicio:** El usuario ejecuta la aplicación. La ventana emerge centrada sobre el escritorio con animación de entrada. El foco se posiciona automáticamente en el campo de usuario.
2. **Captura de credenciales:** El usuario ingresa su nombre de usuario de red en formato `dominio\usuario` (o solo `usuario`, con el dominio completado automáticamente) y su contraseña institucional.
3. **Envío:** El usuario presiona el botón "Iniciar sesión" o la tecla Enter desde cualquier campo. El estado de carga se activa.
4. **Validación:** El sistema valida las credenciales contra el Active Directory del hospital. Recupera los grupos de seguridad y construye el contexto de permisos.
5. **Escenario exitoso:** El JWT de sesión se almacena en el contexto de la aplicación. La pantalla de autenticación cierra con animación. El Shell Principal carga con el módulo de destino del rol activo: Dashboard KPI para Admin/Consulta, Lista de Solicitudes para Comprador DSA y demás roles operativos.
6. **Escenario fallido:** Ver comportamientos de error arriba descritos. El usuario puede reintentar hasta 3 veces.

---

## 🖥️ Módulo 02: Shell Principal de la Aplicación

### Propósito Funcional

Marco estructural persistente que orquesta la navegación entre todos los módulos del sistema durante la sesión activa. El Shell no es un módulo funcional en sí mismo, sino el contenedor que provee: identidad de marca, contexto de usuario autenticado, navegación principal, acceso a notificaciones y el área de contenido donde se renderizan todos los módulos. Se adapta dinámicamente al rol del usuario autenticado, presentando únicamente los elementos de navegación y acción relevantes para ese rol.

### Composición Visual (Fluent Design)

La ventana principal del Shell utiliza material **Mica** como fondo base, el material emblema de Windows 11 que absorbe el color de acento del sistema y el fondo del escritorio del usuario, integrando visualmente la aplicación con el entorno del sistema operativo. Este efecto es exclusivo del renderizador nativo WinUI 3 de Uno Platform y requiere que la ventana tenga habilitado el _backdrop_ de composición de Windows.

La estructura espacial de la ventana se divide en cuatro regiones con roles precisos:

```
┌─ TitleBar Personalizada (altura 40px, ExtendedIntoTitleBar = true) ──────────────┐
│  [☰ HCG Logo · "Fondo Revolvente · HCG"] ─── [🔍 Búsqueda Global] ─── [🔔 n] [👤] [─][□][✕] │
├─ NavigationView Izquierdo ──────────────────────────────────────────────────────┤
│                                       │                                         │
│  [ícono] Dashboard KPI                │                                         │
│  [ícono] Solicitudes       [Badge: n] │                                         │
│  [ícono] Proveedores                  │     ContentFrame                        │
│  [ícono] Cotizaciones                 │     (módulo activo renderizado aquí)    │
│  [ícono] Hitos                        │                                         │
│  [ícono] Validación Fiscal            │                                         │
│  [ícono] Reportes                     │                                         │
│  ─────────────────────                │                                         │
│  [ícono] Administración               │                                         │
│                                       │                                         │
│  [👤 Avatar · Nombre · Rol]           │                                         │
└───────────────────────────────────────┴─────────────────────────────────────────┘
```

### Elementos de Interfaz (WinUI 3 & Toolkit)

**TitleBar Personalizada (ExtendedIntoTitleBar):**
La barra de título reemplaza completamente la barra estándar de Windows. La aplicación extiende su contenido hacia la zona de la TitleBar, colocando elementos funcionales en ese espacio:

| Elemento en TitleBar                  | Especificación                                                                                                                                                                                                                                                                                                          |
| ------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Botón Hamburguesa + Logotipo HCG**  | Botón de colapso/expansión del NavigationView (ícono ≡) + versión miniatura del logotipo institucional + nombre "Fondo Revolvente · HCG" en tipografía 13pt SemiBold.                                                                                                                                                   |
| **AutoSuggestBox Global (Centro)**    | Barra de búsqueda global centrada en la TitleBar. Ancho: 320px. Placeholder: "Buscar solicitud, proveedor, folio...". Busca en tiempo real por folio DSA, nombre de servicio, nombre de proveedor y RFC. Las sugerencias se agrupan por categoría (Solicitudes, Proveedores) en el panel desplegable con fondo Acrylic. |
| **Botón de Notificaciones**           | Ícono 🔔 de glyph _Alert_ (Segoe Fluent Icons). El `BadgeControl` numérico aparece sobre el ícono cuando hay notificaciones no leídas; usa tipografía 10pt Bold, fondo color de acento. Al hacer clic, despliega el panel lateral de notificaciones (Flyout).                                                           |
| **PersonPicture (Avatar de Usuario)** | Muestra las iniciales del usuario (ej: "MG") o su foto del directorio activo en un círculo de 32px de diámetro. Al hacer clic, despliega un Flyout con: nombre completo, rol activo, botón "Cerrar Sesión" y botón "Mis Preferencias".                                                                                  |
| **Controles Nativos de Ventana**      | Minimizar, Maximizar, Cerrar — posicionados en el extremo derecho de la TitleBar. Preservan el comportamiento estándar de Windows.                                                                                                                                                                                      |

**NavigationView (Panel Izquierdo):**
Implementado en modo _Left_, con soporte para colapso a modo _LeftCompact_ (48px) mediante el botón hamburguesa. Ancho expandido: 240px. El panel usa material _Mica Alt_ para diferenciarse ligeramente del área de contenido.

Ítems de menú renderizados según rol (los no accesibles al rol activo no aparecen):

| Ítem               | Ícono Fluent             | Roles que lo ven                                   | Badge                      |
| ------------------ | ------------------------ | -------------------------------------------------- | -------------------------- |
| Dashboard KPI      | `DataArea`               | Admin, Comprador, Consulta                         | —                          |
| Solicitudes        | `ClipboardBulletListLtr` | Admin, Comprador, CAA, Almacén, Finanzas, Consulta | ✅ Pendientes del rol      |
| Proveedores        | `Building`               | Admin, Comprador                                   | —                          |
| Cotizaciones       | `Money`                  | Admin, Comprador                                   | —                          |
| Hitos del Proceso  | `TaskListSquareLtr`      | Todos                                              | —                          |
| Validación Fiscal  | `ReceiptMoney`           | Admin, Finanzas                                    | ✅ En espera de validación |
| Reportes           | `DocumentTable`          | Todos (filtrado por rol)                           | —                          |
| _Separador visual_ | —                        | —                                                  | —                          |
| Administración     | `ShieldKeyhole`          | Solo Admin                                         | —                          |

**Footer del NavigationView:**
Zona inferior del panel de navegación con el componente _PersonPicture_ expandido (muestra nombre y rol bajo el avatar) y acceso al ítem de Configuración. Al colapsar el NavigationView, el footer solo muestra el avatar.

**ContentFrame (Marco de Contenido):**
El área de contenido principal donde se renderizan todos los módulos. Utiliza el patrón de Frame con historial de navegación, habilitando los botones de retroceso/avance integrados en la TitleBar (aparecen con animación _FadeIn_ cuando hay historial disponible). La navegación entre módulos utiliza transiciones animadas de _SlideInFromRight_ (avance) o _SlideInFromLeft_ (retroceso).

**InfoBar del Sistema (Global):**
Barra de información ubicada entre la TitleBar y el ContentFrame. Colapsada por defecto. Se activa automáticamente para avisos de sistema de alta prioridad: problemas de conectividad, mensajes del administrador, alertas de mantenimiento programado. No puede ser descartada por el usuario si el tipo es `Critical`; se descarta automáticamente cuando la condición que la originó se resuelve.

### Comportamiento Visual y Motion (UX)

**Transición de Módulos:**
Al seleccionar un ítem del _NavigationView_, el indicador de selección (barra vertical de 3px en color de acento) desliza fluidamente desde el ítem anterior hasta el nuevo, usando una animación de resorte (_Spring_) suave. El contenido del _ContentFrame_ anterior ejecuta _SlideOutLeft + FadeOut_ (250ms), mientras el nuevo módulo ejecuta _SlideInFromRight + FadeIn_ (250ms, retraso de 50ms). Los elementos individuales dentro del nuevo módulo aplican _StaggeredReveal_ con intervalos de 50ms.

**Actualizaciones en Tiempo Real (Badge):**
Al recibir un evento de notificación vía SignalR, el _BadgeControl_ del botón de notificaciones ejecuta _PopIn_ (escala 0→1.2→1.0, 300ms con resorte). Si ya existía un número, ejecuta _NumberFlip_: el número anterior desliza hacia arriba y desaparece mientras el nuevo aparece desde abajo. El ícono de campana ejecuta un _Shake_ sutil (2 oscilaciones, 200ms) para atraer la atención.

**Colapso/Expansión del NavigationView:**
La transición entre modo expandido (240px) y modo compacto (48px) usa una animación de ancho fluida con curva _EaseInOutQuart_ (250ms). Las etiquetas de texto de los ítems se desvanecen con _FadeOut_ durante el colapso y aparecen con _FadeIn_ durante la expansión, evitando el clipping visual de texto en la transición de ancho.

### Flujo Operativo (User Journey)

1. Tras la autenticación exitosa, el Shell carga con el módulo de destino del rol del usuario ya renderizado en el ContentFrame.
2. El NavigationView refleja exactamente los módulos accesibles para el rol, sin ítems deshabilitados ni grises.
3. Al navegar, el historial se mantiene; el usuario puede retroceder con el botón de retroceso de la TitleBar o con el gesto de deslizamiento lateral (si usa pantalla táctil).
4. El usuario puede colapsar el NavigationView para maximizar el área de trabajo usando el botón hamburguesa. La preferencia de estado (expandido/compacto) se persiste en la sesión.
5. Al hacer clic en el avatar → Flyout → "Cerrar Sesión": se solicita confirmación mediante _ContentDialog_ de una línea ("¿Cerrar la sesión activa?"), la sesión se invalida, el JWT se descarta del contexto, y la aplicación navega a la pantalla de Autenticación con animación inversa (_ConnectedAnimation_ del logotipo de retorno al diálogo).

---

## 🖥️ Módulo 03: Dashboard KPI — Métricas en Tiempo Real

### Propósito Funcional

Centro de operaciones visuales que consolida el estado operativo del sistema de Fondo Revolvente en una única pantalla de alta densidad informativa. Proporciona al Administrador, Comprador DSA y rol de Consulta una vista ejecutiva de: métricas de rendimiento del proceso (KPIs), solicitudes activas por estado y fase, alertas de proceso, y acceso rápido a elementos que requieren atención inmediata. Actualiza todos sus datos automáticamente en tiempo real sin intervención del usuario, aprovechando la conexión SignalR del Shell.

El Dashboard es el módulo de entrada por defecto para los roles **Administrador** y **Consulta DSA**. El rol **Comprador DSA** ve una versión filtrada que muestra únicamente las métricas de sus propias solicitudes.

### Composición Visual (Fluent Design)

El Dashboard utiliza material **Mica Alt** como fondo del área de contenido, creando un contraste visual sutil con el NavigationView (que usa Mica estándar), diferenciando claramente el panel de navegación del área funcional. El fondo Mica Alt aplica un tinte ligeramente más oscuro/saturado que el Mica base.

El área de contenido se organiza en un `ScrollViewer` vertical con tres zonas:

**Zona A — Barra de Control Superior (fija, 48px):** CommandBar con filtros de contexto temporal.

**Zona B — Cuadrícula de KPIs (scrolleable, 240px):** Grid de 4 columnas en resolución full-HD (1920×1080), 3 columnas en resolución estándar (1280×720), 2 columnas en ventanas estrechas. Cada tarjeta KPI usa material Acrylic (80% tinta), CornerRadius 8, Shadow Depth 4.

**Zona C — Tabla de Solicitudes Recientes (scrolleable, flex):** DataGrid de alto rendimiento con soporte de virtualización, que muestra las solicitudes más recientes o con mayor urgencia.

**Zona D — Panel de Alertas y Riesgos (scrolleable, colapsable):** Sección expandible al pie del scroll con la lista de alertas activas del proceso.

**Zona E — Gráficos de Tendencia (expandibles dentro de tarjetas KPI):** Al expandir una tarjeta KPI mediante su _Expander_, se despliega un gráfico de área debajo mostrando la tendencia histórica del indicador durante el ejercicio fiscal activo.

### Elementos de Interfaz (WinUI 3 & Toolkit)

**CommandBar Superior — Barra de Control:**

| Elemento                              | Especificación                                                                                                                                                                                                                      |
| ------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **ComboBox "Ejercicio Fiscal"**       | Selector de año fiscal (2026, 2025, 2024...). Al cambiar, todas las métricas del dashboard se recalculan con animación _CountUp_ simultánea en todas las tarjetas KPI.                                                              |
| **ToggleButton "Tiempo Real"**        | Botón de alternancia que activa/desactiva la suscripción SignalR de actualizaciones automáticas. Estado ON: texto "● Tiempo Real" con ícono de pulso animado en color de acento. Estado OFF: texto "○ Estático" en tono secundario. |
| **AppBarButton "Exportar Dashboard"** | Ícono _Download_. Despliega un _MenuFlyout_ con opciones: "Exportar como PDF", "Exportar como Excel (XLSX)". Genera el reporte del período seleccionado a través del motor SSRS.                                                    |
| **TextBlock "Última actualización"**  | Texto dinámico en tipografía _Caption_ secundario que muestra el timestamp de la última sincronización. Se actualiza silenciosamente con cada refresh.                                                                              |

**Tarjetas KPI — Especificación Individual:**

Cada tarjeta KPI sigue la misma estructura interna: ícono de glyph Fluent (esquina superior izquierda, 24px, color de acento), valor numérico principal (Segoe UI Variable Display, 36-48pt, Bold, `TextFillColorPrimary`), etiqueta descriptiva (14pt, Regular, `TextFillColorSecondary`), indicador de tendencia (ícono de flecha ↑/↓ + porcentaje de cambio en color verde/rojo), botón de expandir (`Expander` collapse/expand trigger en la esquina superior derecha).

| Tarjeta                      | Contenido                                                                                                                                                                                   | Comportamiento Especial                                                                                       |
| ---------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------- |
| **Solicitudes Activas**      | Número total de solicitudes en estados no terminales. Etiqueta: "en proceso".                                                                                                               | Clic navega a Lista de Solicitudes filtrada por "En Proceso".                                                 |
| **Monto Total Ejercido**     | Suma acumulada del ejercicio en formato `$XX,XXX.XX MXN`. _ProgressRing_ circular estático (estilo gauge) que visualiza el porcentaje del monto global respecto al presupuesto del período. | Si supera el 90% del presupuesto, el borde de la tarjeta cambia a `SystemFillColorCaution`.                   |
| **Tiempo Promedio de Ciclo** | Días promedio de resolución completa de solicitudes en el período. Indicador de semáforo: verde (dentro del benchmark), amarillo (25% sobre benchmark), rojo (50%+ sobre benchmark).        | Expander muestra histograma de distribución de tiempos.                                                       |
| **Solicitudes en Alerta**    | Número de solicitudes detenidas más de N días en un mismo estado (umbral configurable en Administración). Fondo de la tarjeta en `SystemFillColorCaution` al 10% cuando el valor es > 0.    | Clic navega a Lista de Solicitudes filtrada por "En Alerta". Tarjeta pulsa suavemente cuando su valor es > 0. |

**DataGrid "Solicitudes Recientes":**
Tabla de alto rendimiento con virtualización de datos (solo renderiza las filas visibles en el viewport). El DataGrid muestra las solicitudes ordenadas por fecha de última modificación descendente. Columnas:

| Columna              | Ancho | Descripción                                                                                                  |
| -------------------- | ----- | ------------------------------------------------------------------------------------------------------------ |
| Folio DSA            | 140px | Tipografía monoespaciada, ícono de documento prefijo. Clickeable → navega al Detalle.                        |
| Servicio Solicitante | flex  | Nombre del área que solicitó la compra. Truncado con ellipsis si excede el ancho.                            |
| Monto                | 120px | Formato `$XX,XXX.XX MXN`, alineado a la derecha.                                                             |
| Estado               | 180px | _StateBadge_ (variante Small). Color y texto del estado actual.                                              |
| Responsable          | 120px | Nombre del Comprador DSA asignado.                                                                           |
| Días en Estado       | 80px  | Número de días que lleva en el estado actual. Color: verde (<7 días), amarillo (7-15 días), rojo (>15 días). |

**Expander "Alertas y Riesgos Activos":**
Panel colapsable al pie del Dashboard. El encabezado del Expander muestra el número total de alertas activas con un _Badge_ de color caución. Al expandirse, muestra un _ListView_ de alertas organizadas por categoría:

- 🔴 Solicitudes con posible fraccionamiento detectado (RN-002).
- 🟡 Solicitudes en estado de error de validación SAT (`ERROR_SAT_REINTENTANDO`).
- 🟠 Solicitudes próximas a vencer plazos normativos.
- 🔵 Solicitudes con bloqueo de edición activo de más de 20 minutos.

Cada ítem de alerta en el ListView incluye: tipo de alerta (ícono + etiqueta), folio DSA afectado (enlace clickeable al Detalle), descripción del problema y tiempo transcurrido.

**Gráfico de Tendencia (dentro de Expander de tarjeta KPI):**
Al expandir una tarjeta, se despliega un `AreaChart` que renderiza la evolución del indicador durante el ejercicio fiscal activo. El gráfico usa:

- Eje X: Meses del ejercicio fiscal.
- Eje Y: Valor del KPI en su unidad correspondiente.
- Área: Relleno con gradiente del color de acento (100% opacidad en la línea, 0% en la base).
- Tooltip interactivo: Al pasar el cursor sobre el gráfico, aparece un tooltip con el valor exacto del punto más cercano y su fecha.

### Comportamiento Visual y Motion (UX)

**Carga inicial del Dashboard:**
El _ProgressRing_ central (diámetro 48px) aparece sobre el fondo Mica Alt mientras los datos se cargan. Las tarjetas KPI emergen de manera escalonada con _EntranceThemeTransition_ a medida que cada dato llega (retraso de 80ms entre tarjetas). Los valores numéricos de los KPIs ejecutan animación _CountUp_ (de 0 al valor real, 800ms, EaseOutQuart).

**Actualización en Tiempo Real:**
Cuando un evento SignalR actualiza un valor del Dashboard (nueva solicitud creada, cambio de estado), la tarjeta afectada muestra un fondo de `SystemFillColorAttention` al 20% que aparece instantáneamente y se desvanece en 2 segundos (_FadeOut_ suave). El valor numérico ejecuta _CountUp_ solo si el cambio es ≥ 5% del valor anterior (cambios mínimos se actualizan sin animación para evitar ruido visual). La barra delgada de _ProgressBar_ indeterminada de 2px en la parte superior del ContentFrame aparece brevemente durante cada sincronización de datos.

**Interacción con Tarjetas:**
_RevealHighlight_ al pasar el cursor sobre cualquier tarjeta. Al hacer clic en una tarjeta navegable, la tarjeta ejecuta _ScaleDown_ (0.98, 50ms) y la transición de navegación se inicia con _Connected Animation_ que lleva el elemento principal de la tarjeta hacia la nueva pantalla. Al hacer _hover_ sobre el área del gráfico de tendencia, el cursor cambia a crosshair y el punto de datos más cercano se resalta con un marcador circular.

**Indicadores de Estado de Alerta (Breathing):**
Los _StateBadge_ en el DataGrid para estados que requieren atención inmediata del usuario autenticado (ej: `EN_AUTORIZACION_CAA` para el rol CAA, `ERROR_SAT_REINTENTANDO` para el rol Finanzas) ejecutan animación _Breathing_: opacidad oscilando entre 100% y 65% en un ciclo de 2 segundos (EaseInOut), atrayendo la atención periférica del usuario sin ser disruptivos.

### Flujo Operativo (User Journey)

1. El usuario llega al Dashboard tras la autenticación. El _ProgressRing_ indica la carga inicial. Las tarjetas KPI aparecen progresivamente con animación _CountUp_.
2. La conexión SignalR se establece en segundo plano. El _ToggleButton_ "Tiempo Real" cambia a estado ON automáticamente cuando la conexión está activa.
3. El usuario identifica una tarjeta de alerta con valor > 0 y hace clic → navega a Lista de Solicitudes con el filtro correspondiente preaplicado.
4. El usuario expande una tarjeta KPI para ver la tendencia del ejercicio → el gráfico renderiza con animación _StrokeReveal_ que dibuja las líneas progresivamente.
5. El usuario cambia el _ComboBox_ de ejercicio fiscal → todas las métricas se recalculan y los valores cambian con animación _CountUp_ simultánea.
6. Mientras el usuario trabaja, el Dashboard recibe actualizaciones automáticas. Las tarjetas afectadas pulsan brevemente para comunicar el cambio sin interrumpir el foco del usuario.
7. El usuario hace doble clic en una fila del DataGrid → _Connected Animation_ desde la fila hacia el módulo de Detalle de Solicitud.

---

## 🖥️ Módulo 04: Gestión de Solicitudes — Vista de Lista

### Propósito Funcional

Centro de gestión y consulta del universo de solicitudes de Fondo Revolvente. Es el módulo de trabajo principal para los roles de Comprador DSA y Administrador, y el módulo de consulta para los roles de CAA, Finanzas, Almacén y Consulta DSA. Presenta todas las solicitudes en un `DataGrid` filtrable, ordenable y con soporte de selección múltiple, permitiendo localizar, monitorear y actuar sobre expedientes específicos. Aplica filtros de visibilidad automáticos según el rol del usuario: el Comprador DSA ve únicamente sus propias solicitudes; el CAA ve solo las que están en estados de autorización; el Almacén ve solo las que están en estados de entrega.

### Composición Visual (Fluent Design)

La pantalla mantiene el material **Mica Alt** del Shell como fondo. Se organiza en tres franjas horizontales fijas y una zona de contenido flexible:

1. **Franja de Herramientas Superior** (CommandBar, 48px fija): Acciones y búsqueda.
2. **Franja de Filtros Rápidos** (TabView/Segmented, 40px fija): Pestañas de filtro inmediato.
3. **Franja de Filtros Avanzados** (Expander, colapsado por defecto, 0-200px): Filtros adicionales expandibles.
4. **Zona de Contenido Principal** (DataGrid, flex, todo el espacio restante): La tabla de solicitudes.
5. **Panel Lateral de Vista Previa** (SplitView, 0-300px, colapsado por defecto): Se activa al seleccionar una fila.

El `DataGrid` principal utiliza material `AcrylicInAppFillColorDefault` (80% tinta) para mejorar la legibilidad de los datos tabulares manteniendo la profundidad visual del Fluent Design.

### Elementos de Interfaz (WinUI 3 & Toolkit)

**CommandBar Superior:**

| Elemento                             | Visibilidad por Rol | Especificación                                                                                                                                                            |
| ------------------------------------ | ------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **AppBarButton "+ Nueva Solicitud"** | Admin, Comprador    | Ícono _Add_, color de acento, texto visible. Al presionar inicia navegación al Módulo 05.                                                                                 |
| **AppBarButton "Actualizar"**        | Todos               | Ícono _Sync_. Recarga la lista.                                                                                                                                           |
| **AppBarSeparator**                  | —                   | Divisor visual entre grupos de acciones.                                                                                                                                  |
| **AppBarButton "Exportar"**          | Todos (según rol)   | Ícono _Download_. Despliega MenuFlyout: "Exportar lista como Excel", "Exportar selección como PDF".                                                                       |
| **AppBarButton "Columnas"**          | Todos               | Ícono _ColumnTriple_. Despliega Flyout con CheckBox por columna para personalizar la vista del DataGrid. La configuración persiste en la sesión.                          |
| **AutoSuggestBox (Búsqueda)**        | Todos               | Ancho 320px. Placeholder: "Buscar por folio, servicio, proveedor...". Sugerencias agrupadas: [Folios] DSA-2026-001, [Servicios] Urgencias, [Proveedores] Distribuidora... |

**Barra de Filtros Rápidos (TabView/Segmented Control):**
Pestañas horizontales con fondo translúcido. La pestaña activa muestra indicador de subrayado en color de acento. Cada pestaña muestra un `BadgeControl` numérico entre paréntesis con la cantidad de solicitudes en esa categoría (se actualiza en tiempo real).

| Pestaña         | Filtro Aplicado                                | Visible para     |
| --------------- | ---------------------------------------------- | ---------------- |
| Todas           | Sin filtro de estado (sujeto al scope del rol) | Admin            |
| Mis Solicitudes | `usuario_atiende = usuario_actual`             | Comprador, Admin |
| En Proceso      | Estados activos (no terminales)                | Todos            |
| En Alerta       | Más de N días en un mismo estado               | Todos            |
| Cerradas        | Estado `CERRADO`, `CANCELADO`                  | Todos            |

**Expander "Filtros Avanzados":**
Panel colapsado por defecto. El encabezado del Expander muestra "Filtros avanzados" con un `BadgeControl` que indica cuántos filtros están activos actualmente. Al expandir, revela:

- _ComboBox_ "Fase del Proceso" (las 8 fases del ciclo de vida).
- _ComboBox_ "Estado Específico" (los 30 estados, filtrado por la fase seleccionada).
- _ComboBox_ "Ejercicio Fiscal" (2026, 2025...).
- _AutoSuggestBox_ "Servicio Solicitante" (busca en el catálogo de áreas).
- _DateRangePicker_ "Rango de Fechas de Recepción".
- _Slider_ "Rango de Monto" ($0 — $75,000 MXN).
- _Button_ "Aplicar Filtros" (acento) · _HyperlinkButton_ "Limpiar todos los filtros".

**DataGrid Principal (Windows Community Toolkit — DataGrid):**

El DataGrid usa virtualización de UI (solo renderiza las filas visibles) para mantener el rendimiento con grandes volúmenes de datos. Las columnas son:

| Columna               | Ancho  | Tipo                                 | Ordenable |
| --------------------- | ------ | ------------------------------------ | --------- |
| CheckBox de selección | 48px   | _CheckBox_ múltiple                  | No        |
| Folio DSA             | 140px  | Texto monoespaciado + ícono          | Sí        |
| Fecha Recepción       | 120px  | Fecha `dd/MMM/YYYY`                  | Sí        |
| Servicio Solicitante  | flex-1 | Texto, truncado                      | Sí        |
| Concepto / Artículos  | flex-1 | Texto resumen, truncado              | No        |
| Monto Estimado        | 140px  | _MontoDisplay_ alineado a la derecha | Sí        |
| Fase                  | 160px  | _StateBadge_ variante Small          | Sí        |
| Estado                | 180px  | _StateBadge_ variante Small          | Sí        |
| Responsable           | 120px  | Nombre de usuario                    | Sí        |
| Días en Estado        | 80px   | Número coloreado                     | Sí        |
| Acciones              | 48px   | Botón _More_ → ContextFlyout         | No        |

El `ContextFlyout` de cada fila (tres puntos) contiene: "Ver Detalle Completo", "Ver Timeline de Hitos", "Copiar Folio", y acciones de transición de estado disponibles para el rol (ej: "Autorizar" para CAA si el estado lo permite).

**SplitView (Panel de Vista Previa):**
Se activa al hacer clic en una fila del DataGrid (sin doble clic). Se desliza desde la derecha (300px) empujando el DataGrid. Muestra:

- Folio DSA (Title) + _StateBadge_ Large del estado actual.
- Monto con _MontoDisplay_.
- Servicio solicitante y responsable.
- Últimos 3 hitos completados (mini-timeline).
- _Button_ "Ver Expediente Completo" (acento, ancho completo).
- _Button_ "Cerrar vista previa" (neutro, pequeño).

**FloatingActionButton "Nueva Solicitud":**
Botón circular de 56px posicionado en la esquina inferior derecha del área de contenido (solo visible para roles Admin y Comprador). Muestra el ícono _Add_ (24px). Se oculta con _FadeOut_ durante scroll rápido y reaparece con _FadeIn_ cuando el scroll se detiene. Es un atajo alternativo al botón de la CommandBar.

### Comportamiento Visual y Motion (UX)

**Entrada a la pantalla:** Las filas del DataGrid aparecen con _StaggeredAnimation_ de _FadeIn + SlideInFromBottom_ (100ms por fila, limitado a un total de 400ms para evitar demoras perceptibles con listas largas). Las cabeceras del DataGrid aparecen primero, seguidas de las filas.

**Scroll:** El encabezado del DataGrid permanece fijo (_Sticky Header_) durante el scroll. Las filas que entran al viewport ejecutan _FadeIn_ rápido (80ms). Las cabeceras adquieren una sombra sutil de Shadow Depth 2 cuando hay filas scrolleadas bajo ellas, comunicando que el contenido superior está oculto.

**Interacción con filas:** _RevealHighlight_ al pasar el cursor sobre cada fila. Al hacer clic en una fila: escala sutil de _PointerDown_ (0.995), la casilla de CheckBox se marca con animación de escala, y el _SplitView_ de vista previa emerge con _SlideInFromRight_ (250ms).

**Actualizaciones en tiempo real (SignalR):** Cuando una solicitud visible en la lista cambia de estado (notificado por el hub SignalR), su fila ejecuta un _flash_ del color de la nueva fase (fondo aparece en 100ms y se desvanece en 600ms). El _StateBadge_ actualiza su valor con _CrossFade_ (200ms). El _InfoBar_ contextual en la parte superior de la pantalla emerge brevemente con el mensaje: _"DSA-2026-089 actualizó su estado a AUTORIZADO_CAA por María López."_

**Acciones en lote (selección múltiple):** Al seleccionar más de una fila, la _CommandBar_ actualiza sus botones para mostrar acciones de lote disponibles (_BatchAction_: "Exportar selección", acciones de transición si todas las seleccionadas están en el mismo estado). La transición de contenido de la CommandBar usa _CrossFade_ (150ms).

### Flujo Operativo (User Journey)

1. El usuario llega al módulo. La lista carga con el scope del rol automáticamente aplicado.
2. El usuario escribe en el _AutoSuggestBox_ → la lista filtra en tiempo real, mostrando sugerencias agrupadas.
3. Selecciona un filtro rápido ("En Alerta") → la pestaña activa cambia con animación de subrayado deslizante, la lista se actualiza.
4. Opcionalmente expande los Filtros Avanzados para combinaciones de filtro más específicas.
5. Hace clic en una fila de interés → el _SplitView_ de vista previa emerge. Si quiere el detalle completo, presiona "Ver Expediente Completo" → _Connected Animation_ de la fila hacia el Módulo 06 (Detalle de Solicitud).
6. El Comprador DSA presiona "+ Nueva Solicitud" → navega al Módulo 05 con transición _SlideInFromRight_.
7. En tiempo real, las filas se actualizan si otras sesiones modifican el estado de las solicitudes visibles.

---

## 🖥️ Módulo 05: Nueva Solicitud — Formulario de Creación

### Propósito Funcional

Interfaz guiada para que el Comprador DSA registre formalmente una nueva solicitud de adquisición al Fondo Revolvente. Captura el expediente inicial: datos del área solicitante, concepto de la compra, partidas de artículos con montos estimados, y genera el folio único DSA. El sistema aplica validaciones en línea durante la captura, incluyendo el límite máximo de $75,000 MXN (RN-001) y la detección preventiva de fraccionamiento de compra (RN-002). La solicitud creada entra automáticamente al ciclo de vida en estado `RECEPCIONADO`.

### Composición Visual (Fluent Design)

El formulario ocupa el _ContentFrame_ completo con material _Mica_ como fondo. La composición es de una sola columna (ancho máximo 840px, centrado) con un _ScrollViewer_ vertical. Las secciones del formulario se presentan como _CardPanel_ apilados verticalmente, cada uno con material de capa 1 (fondo ligeramente más claro que Mica, Shadow Depth 4, CornerRadius 8). La separación entre secciones es de 16px.

Un `CommandBar` fijo en la parte inferior de la ventana (siempre visible, por encima del ContentFrame) contiene los botones de acción principal. Un `BreadcrumbBar` fijo en la parte superior indica la posición del usuario en la jerarquía de navegación.

La sección de partidas (Sección 3) es la más compleja: contiene un `DataGrid` editable con una fila de entrada activa en cualquier momento, más las filas ya agregadas en modo de solo visualización. El `MontoDisplay` con la barra de progreso se posiciona como footer del `CardPanel` de partidas, actualización permanente y visible.

### Elementos de Interfaz (WinUI 3 & Toolkit)

**Barra de Navegación Superior (BreadcrumbBar):**
"Inicio > Solicitudes > Nueva Solicitud" con separadores de chevron. Cada elemento es clickeable y navega hacia atrás en el historial con animación correspondiente.

**Indicador de Folio Pendiente:**
En el extremo derecho del `BreadcrumbBar`, un _TextBlock_ en tipografía _Caption_ monoespaciada y color secundario muestra: "Folio a asignar: DSA-2026-— —". El guion triple indica que el folio se generará automáticamente al guardar.

---

**Sección 1 — "Datos del Solicitante"** (CardPanel):

| Componente                                      | Especificación                                                                   |
| ----------------------------------------------- | -------------------------------------------------------------------------------- |
| **TextBox "Servicio / Área Solicitante"**       | Obligatorio. Validación: no vacío. Sugerencias de áreas frecuentes del hospital. |
| **TextBox "Número de Oficio de Solicitud"**     | Obligatorio. Referencia documental del área solicitante. Formato libre.          |
| **DatePicker "Fecha de Recepción del Oficio"**  | Obligatorio. Por defecto: fecha actual. No puede ser fecha futura.               |
| **NumberBox "Ejercicio Fiscal"**                | Obligatorio. Por defecto: año en curso. Rango: año actual y año anterior.        |
| **TextBox "Nombre del Solicitante / Firmante"** | Opcional. Persona firmante del oficio en el área solicitante.                    |

---

**Sección 2 — "Concepto de Adquisición"** (CardPanel):

| Componente                                   | Especificación                                                                       |
| -------------------------------------------- | ------------------------------------------------------------------------------------ |
| **TextBox multilínea "Descripción General"** | Obligatorio. Máx. 500 caracteres con contador de caracteres visible (ej: "152/500"). |
| **ComboBox "Tipo de Bien o Servicio"**       | Obligatorio. Opciones: Material/Consumible, Servicio, Equipo/Activo, Medicamento.    |

---

**Sección 3 — "Partidas / Ítems a Adquirir"** (CardPanel con DataGrid editable):

El DataGrid de partidas tiene dos modos por fila: la fila de entrada activa (celdas editables en tiempo real) y las filas ya registradas (solo lectura, con botón de editar y eliminar en la última columna).

| Columna del DataGrid     | Tipo de Entrada       | Validación                                                                           |
| ------------------------ | --------------------- | ------------------------------------------------------------------------------------ |
| Código de Producto       | TextBox               | Alfanumérico. Al salir del campo, disparar verificación de fraccionamiento (RN-002). |
| Descripción del Artículo | TextBox               | Obligatorio. Máx. 200 caracteres.                                                    |
| Cantidad                 | NumberBox             | Entero positivo. Mínimo 1.                                                           |
| Unidad de Medida         | ComboBox              | Pieza, Caja, Kg, Lt, Servicio, Hora, etc.                                            |
| Precio Unitario Estimado | NumberBox (moneda)    | Decimal positivo. Formato $XX,XXX.XX.                                                |
| Subtotal                 | TextBlock (calculado) | `Cantidad × Precio Unitario`. Solo lectura, se recalcula en tiempo real.             |
| Acciones                 | StackPanel de botones | Botón ✏️ Editar + Botón 🗑️ Eliminar (visible solo en filas ya guardadas).            |

Bajo la cuadrícula de columnas, los botones de gestión de partidas:

- _AppBarButton_ "+ Agregar ítem" (ícono _Add_): Agrega una nueva fila editable al final del DataGrid con animación de deslizamiento desde abajo (_SlideInFromBottom_, 200ms).
- _AppBarButton_ "Limpiar ítems" (ícono _DeleteAll_): Limpia todas las partidas con confirmación mediante _ContentDialog_ mini.

**MontoDisplay — Footer de la Sección 3 (fijo dentro del CardPanel):**

```
┌── Resumen de Monto ──────────────────────────────────────────────────────────────┐
│  Subtotal (sin IVA):  $XX,XXX.XX MXN                                             │
│  IVA (16%):           $XX,XXX.XX MXN                                             │
│  TOTAL ESTIMADO:      $XX,XXX.XX MXN   ← tipografía Bold, grande                 │
│                                                                                   │
│  [████████████░░░░░░░░░░░░░░░░] 67% del límite ($75,000.00 MXN)                   │
└──────────────────────────────────────────────────────────────────────────────────┘
```

La barra de progreso cambia de color: verde (<70%), amarillo (70-90%), rojo (>90% o excedido). Si el total supera $75,000.00 MXN, la barra se llena completamente en rojo y un *InfoBar* tipo `Warning` emerge con borde rojo: *"⚠️ El monto total estimado ($XX,XXX.XX MXN) excede el límite máximo del Fondo Revolvente ($75,000.00 MXN). No es posible registrar esta solicitud hasta reducir el monto."\* El botón "Registrar Solicitud" se deshabilita automáticamente.

---

**Alerta de Fraccionamiento (Sección 4 — condicional):**
Esta sección solo aparece cuando el sistema detecta que uno o más códigos de producto ingresados fueron adquiridos en solicitudes anteriores dentro del mismo ejercicio fiscal (RN-002). Emerge como un _InfoBar_ de tipo `Warning` de alto contraste, posicionado inmediatamente bajo la Sección 3:

_"⚠️ Posible fraccionamiento detectado (RN-002): El código [CODIGO_PRODUCTO] fue adquirido en la solicitud [DSA-2026-XXX] del mismo ejercicio fiscal. Si esta compra es independiente y no constituye fraccionamiento, debe justificarlo expresamente."_

El _InfoBar_ incluye un _TextBox_ multi-línea dentro de él para que el usuario escriba la justificación de no-fraccionamiento, y un _CheckBox_ "Confirmo que esta compra no constituye fraccionamiento de adquisición" que debe marcarse para habilitar el registro. Mientras no se confirme esta justificación, el botón "Registrar Solicitud" permanece deshabilitado.

---

**CommandBar Inferior (Fija):**

| Botón                       | Estilo               | Comportamiento                                                                                                                                                                    |
| --------------------------- | -------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **"Registrar Solicitud"**   | AccentButtonStyle    | Valida todos los campos, ejecuta el registro. Deshabilitado si hay errores de validación activos (monto excedido, fraccionamiento sin justificación, campos obligatorios vacíos). |
| **"Guardar como Borrador"** | SecondaryButtonStyle | Guarda la solicitud en estado `BORRADOR` sin iniciar el ciclo de vida oficial.                                                                                                    |
| **"Cancelar"**              | TextButtonStyle      | Solicita confirmación (_ContentDialog_: "¿Descarta los cambios no guardados?") y regresa a la Lista de Solicitudes.                                                               |

### Comportamiento Visual y Motion (UX)

**Validación progresiva:** Los campos validan al perder el foco (`OnLostFocus`), no durante la escritura (para evitar mensajes de error prematuros). Los campos con error muestran borde `SystemFillColorCritical` (rojo) y un _ToolTip_ con el mensaje específico del error al pasar el cursor. Los campos correctamente validados muestran un destello breve del borde en color de éxito (verde, 300ms) antes de volver al borde neutral.

**Adición de ítem:** La nueva fila aparece con _SlideInFromBottom_ (200ms, EaseOutQuart). La barra de progreso del monto se actualiza con transición fluida de ancho conforme se calculan los subtotales.

**Envío del formulario:** Al presionar "Registrar Solicitud", se ejecuta una validación visual secuencial: los campos con error brillan brevemente en rojo en cascada de arriba hacia abajo. Si todos son válidos, el botón entra en estado de carga (_ProgressRing_). Al completarse exitosamente:

### Flujo Operativo (User Journey)

1. El Comprador DSA llega al formulario desde la Lista de Solicitudes.
2. Completa los datos del solicitante (Sección 1) y el concepto de compra (Sección 2).
3. Agrega los ítems de la compra en la Sección 3. El MontoDisplay calcula en tiempo real.
4. Si algún código de producto dispara la alerta de fraccionamiento (RN-002), el _InfoBar_ emerge. El usuario debe escribir la justificación y marcar el CheckBox de confirmación para continuar.
5. Si el monto total excede $75,000 MXN, el botón de registro se deshabilita hasta que se ajusten las partidas.
6. El usuario presiona "Registrar Solicitud". Validación visual secuencial.
7. **Resultado funcional:** La solicitud se crea con folio único `DSA-YYYY-NNN` asignado por el sistema. El estado inicial es `RECEPCIONADO`. El primer hito del proceso queda registrado automáticamente con timestamp y usuario. La pantalla navega de retorno a la Lista de Solicitudes con _SlideInFromLeft_. La nueva solicitud aparece en primer lugar con un destello verde transitorio (fondo verde al 20%, _FadeOut_ en 1.5 segundos). Un _InfoBar_ tipo `Success` en la Lista confirma: _"✅ Solicitud DSA-2026-089 registrada exitosamente. El expediente está en estado RECEPCIONADO."_

---

## 🖥️ Módulo 06: Detalle de Solicitud — Expediente Completo

### Propósito Funcional

Vista maestra y punto de acción central del sistema. Consolida en una sola pantalla toda la información del expediente de una solicitud: datos del formulario original, estado actual del ciclo de vida, historial de transiciones, hitos completados y pendientes, cotizaciones asociadas, y las **acciones disponibles para el usuario según su rol y el estado actual**. Es el módulo donde se ejecutan las transiciones de estado de mayor trascendencia (autorización CAA, confirmación de entrega, inicio de validación CFDI), cada una con su propio _ContentDialog_ de confirmación y registro de hito correspondiente.

### Composición Visual (Fluent Design)

La pantalla adopta una composición de **dos columnas** de ancho fijo en resoluciones ≥ 1280px y una sola columna en resoluciones inferiores:

- **Columna principal** (izquierda, 65%): _ScrollViewer_ con las secciones del expediente apiladas verticalmente como _CardPanel_ con Shadow Depth 4.
- **Panel lateral de acciones** (derecha, 35%): Fijo durante el scroll de la columna principal. Material _Acrylic_ (AcrylicInApp), CornerRadius 8, Shadow Depth 8. Contiene el estado actual, las acciones disponibles y el _BloqueoIndicator_ cuando aplica.

Un **encabezado de pantalla fijo** (por encima del scroll de ambas columnas) muestra el folio DSA, el _StateBadge_ Large del estado actual, y la información básica de la solicitud. El `BreadcrumbBar` se posiciona dentro de este encabezado.

### Elementos de Interfaz (WinUI 3 & Toolkit)

**Encabezado Fijo:**

| Elemento                | Especificación                                                                                                                                                                                                                                                  |
| ----------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **BreadcrumbBar**       | "Solicitudes > DSA-2026-089" — con navegación de retorno funcional.                                                                                                                                                                                             |
| **TextBlock Folio**     | "DSA-2026-089" · Segoe UI Variable Display · 28pt · Bold · `TextFillColorPrimary`.                                                                                                                                                                              |
| **StateBadge Large**    | Estado actual con color de fase, ícono y texto. Variante Large (36px de altura).                                                                                                                                                                                |
| **TextBlock Metadatos** | "Servicio de Urgencias · Recibida el 15/01/2026 · Atiende: Juan Pérez" · Tipografía Caption · Color secundario.                                                                                                                                                 |
| **BloqueoIndicator**    | Panel de advertencia naranja (visible solo cuando el expediente tiene un bloqueo de edición activo). Muestra: "🔒 Editando: [Nombre] desde [HH:MM]" · Si el bloqueo es del usuario actual: "🔓 Tú estás editando este expediente." con botón "Liberar bloqueo". |

---

**Panel Lateral de Acciones** (columna derecha, fijo):

El panel lateral tiene tres zonas:

1. **Zona de Estado:** _StateBadge_ Medium del estado actual (redundante con el encabezado pero incluido para visibilidad al hacer scroll). Nombre de la fase actual. Días en el estado actual.

2. **Zona de Acciones Disponibles:** Lista de botones de acción. Los botones renderizados son **exclusivamente los que son ejecutables** para la combinación de `{estado_actual, rol_usuario}`. No hay botones deshabilitados ni grises en esta zona — o un botón es ejecutable y se muestra, o no se muestra.

Ejemplos de acciones por rol y estado:

| Estado                | Rol       | Botones Disponibles                                                               |
| --------------------- | --------- | --------------------------------------------------------------------------------- |
| `EN_AUTORIZACION_CAA` | CAA       | "✅ Autorizar Solicitud" (verde) · "❌ Rechazar Solicitud" (rojo)                 |
| `EN_VALIDACION_CFDI`  | Finanzas  | "🧾 Ir a Validación CFDI" (acento)                                                |
| `EN_ENTREGA`          | Almacén   | "📦 Confirmar Recepción de Bienes" (verde) · "⚠️ Reportar Discrepancia" (naranja) |
| `RECHAZADO_CAA`       | Comprador | "🔄 Ajustar y Reenviar a CAA" (acento)                                            |
| `RECEPCIONADO`        | Comprador | "✏️ Editar Solicitud" · "➡️ Enviar a Validación" (acento)                         |

3. **Zona de Documentos:** _HyperlinkButton_ "📎 Ver documentos del expediente" que abre el visor de archivos del servidor SMB. _HyperlinkButton_ "📊 Ver Cuadro Comparativo" (visible si hay cotizaciones).

---

**Sección "Datos del Expediente"** (CardPanel, columna principal):

Presenta en modo de solo lectura todos los campos capturados en el formulario de creación. Diseño de dos columnas de campos, con etiquetas en tipografía _Caption_ secundaria y valores en tipografía _Body_ primaria. Al pie, el `MontoDisplay` con el total del expediente y la barra de progreso (en modo solo lectura, sin el umbral de RN-001 activo como restricción, solo como referencia visual).

_AppBarButton_ "✏️ Editar Datos": Solo visible si el rol y el estado permiten la edición. Al presionar, intenta adquirir el bloqueo de edición (RN-005). Si el bloqueo se adquiere exitosamente, los campos transicionan a modo editable con animación de _FadeIn_ de los bordes de edición. Si el bloqueo está ocupado por otro usuario, se muestra el _ContentDialog_ informativo de bloqueo.

---

**Sección "Cotizaciones Registradas"** (CardPanel):

_DataGrid_ de cotizaciones con columnas: Proveedor, RFC (parcialmente enmascarado: `AAAA######***`), Monto Total ($MXN), Días de Entrega, Condiciones, PDF (ícono de adjunto), Seleccionada (ícono ⭐ o ✅). El proveedor seleccionado tiene fondo de fila en color de acento al 10%.

_AppBarButton_ "+ Agregar Cotización": Visible solo si el estado de la solicitud está en la fase de cotización y el rol es Comprador/Admin.

_HyperlinkButton_ "Ver Cuadro Comparativo completo": Navega al Módulo 09.

---

**Sección "Resumen de Timeline de Hitos"** (CardPanel):

Vista compacta del timeline: una línea de tiempo horizontal simplificada que muestra los 8 hitos de fase (iconos de check verde para completados, ícono de reloj para el activo, iconos grises para pendientes). Bajo la línea de tiempo, los últimos 5 hitos registrados en formato de lista: ícono + nombre del hito + fecha/hora + usuario.

_HyperlinkButton_ "Ver timeline completo" en el pie del CardPanel → navega al Módulo 07 con _Connected Animation_ de la línea de tiempo compacta expandiéndose hacia la vista completa.

---

**Sección "Historial de Cambios de Estado"** (Expander colapsado por defecto):

_ListView_ cronológico inverso de todas las transiciones del expediente. Cada ítem: timestamp en formato `dd/MM/YYYY HH:MM:SS`, estado anterior (con _StateBadge_ Small), flecha →, estado nuevo (con _StateBadge_ Small), usuario que realizó la transición, observaciones. El _ListView_ soporta selección de un ítem para ver las observaciones completas en un _Flyout_ lateral.

### Comportamiento Visual y Motion (UX)

**Entrada al módulo:** El _StateBadge_ Large del encabezado ejecuta _PopIn_ con escala (0.8→1.05→1.0, 400ms) al cargar. Las secciones de la columna principal aparecen con _StaggeredAnimation_: cada _CardPanel_ hace _FadeIn + SlideInFromBottom_ (80ms de desfase entre secciones).

**Cambio de estado en tiempo real:** Si mientras el usuario está en esta pantalla se recibe una notificación SignalR de cambio de estado de esta solicitud, el _StateBadge_ del encabezado cambia con _CrossFade_ (300ms). El panel lateral de acciones actualiza su lista de botones con _FadeOut/FadeIn_. El _InfoBar_ del encabezado emerge con el mensaje del cambio. La sección de Historial de Cambios de Estado actualiza su _ListView_ con el nuevo ítem que aparece con _SlideInFromTop_.

**Ejecución de una acción (ej: Autorizar):**

1. El miembro CAA presiona "✅ Autorizar Solicitud".
2. El fondo de la pantalla oscurece gradualmente con material _Smoke_ (300ms, opacidad 0→0.6).
3. El _ContentDialog_ emerge desde el centro con _ScaleFrom(0.8) + FadeIn_ (250ms): título "Confirmar autorización de DSA-2026-089", descripción de la consecuencia, campo de _TextBox_ para observaciones opcionales, botones "Confirmar Autorización" (acento verde) y "Cancelar" (neutro).
4. Al confirmar: el _ContentDialog_ cierra con _ScaleDown + FadeOut_ (200ms), el _Smoke_ se disipa (250ms).
5. El _StateBadge_ cambia al nuevo estado con _CrossFade_. El panel de acciones actualiza sus botones.
6. El timeline de hitos actualiza el hito correspondiente de ⬜ a ✅ con animación _PopIn_.
7. Un _InfoBar_ tipo _Success_ emerge en el encabezado: _"✅ Solicitud autorizada por CAA. El expediente avanza a [Siguiente Estado]."_

### Flujo Operativo (User Journey)

1. El usuario llega al Detalle desde la Lista de Solicitudes, el Dashboard, o una notificación.
2. El expediente completo carga con toda su información consolidada.
3. El usuario revisa los datos, cotizaciones, y el estado del timeline.
4. El usuario ejecuta la acción disponible para su rol y el estado actual de la solicitud (Autorizar, Rechazar, Confirmar Entrega, etc.).
5. El _ContentDialog_ de confirmación asegura que las acciones irreversibles sean intencionales.
6. La transición de estado se registra en el sistema. El expediente actualiza su apariencia visual reflejando el nuevo estado.
7. El usuario puede navegar al Timeline completo (Módulo 07) o al Cuadro Comparativo (Módulo 09) desde los enlaces dentro del Detalle.

---

## 🖥️ Módulo 07: Timeline de Hitos — Las 8 Fases del Proceso

### Propósito Funcional

Visualización interactiva y completa del progreso de una solicitud a través de las 8 fases y 27 hitos que constituyen el proceso de adquisición de Fondo Revolvente del HCG. Proporciona a cualquier usuario con acceso al expediente una comprensión clara e inmediata de: dónde se encuentra actualmente la solicitud, qué acciones han ocurrido y cuándo, quién las realizó, y qué hitos restan por completar. Para los roles con capacidad de registro de hitos, permite registrar la completación de un hito pendiente directamente desde esta vista.

### Composición Visual (Fluent Design)

El módulo se estructura como un **carril de fases vertical** con scroll. Cada fase es un bloque visual expandible (_Expander_) que contiene su lista de hitos internos. La composición usa el 100% del ancho del `ContentFrame`. El fondo es _Mica Alt_.

**Tratamiento visual de fases según estado:**

| Condición de la Fase   | Tratamiento Visual                                                                                                                                          |
| ---------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Completada**         | Fondo del CardPanel en color de la fase al 15% de opacidad. Encabezado con ícono ✅ en verde. Todos los hitos internos con ícono ✅.                        |
| **En Curso (activa)**  | Borde izquierdo de 4px en color de acento. Fondo en color de acento al 8% de opacidad. Encabezado con ícono de reloj animado 🔄. Pre-expandida por defecto. |
| **Pendiente (futura)** | Sin color especial. Fondo neutro semitransparente. Encabezado en tipografía secundaria de menor contraste. Colapsada por defecto.                           |
| **Con error/rechazo**  | Borde izquierdo de 4px en `SystemFillColorCritical`. Fondo en rojo al 8%. Encabezado con ícono ❌.                                                          |

### Las 8 Fases y sus Hitos

| #   | Fase                                | Color de Referencia      | Hitos Principales                                                                                                        |
| --- | ----------------------------------- | ------------------------ | ------------------------------------------------------------------------------------------------------------------------ |
| 1   | **Recepción y Validación Inicial**  | `#0078D4` Azul           | Recepción del oficio, Asignación de folio, Validación documental inicial                                                 |
| 2   | **Autorización CAA**                | `#FF8C00` Naranja        | Envío al Comité CAA, Revisión por CAA, Resolución (Autorización o Rechazo), Reenvío tras corrección (si aplica)          |
| 3   | **Estudio de Mercado / Cotización** | `#00B7C3` Cyan           | Solicitud de cotizaciones, Recepción cotización proveedor 1, 2, 3 (mínimo 3 requeridas — RN-003), Cotizaciones completas |
| 4   | **Selección de Proveedor / Pedido** | `#E74F2D` Naranja Cálido | Elaboración del cuadro comparativo, Selección de proveedor, Generación de orden de compra                                |
| 5   | **Entrega de Bienes o Servicios**   | `#6DC253` Verde Claro    | Confirmación de pedido al proveedor, Notificación de entrega, Recepción en almacén del HCG                               |
| 6   | **Validación Fiscal (CFDI)**        | `#9B4F9E` Púrpura        | Solicitud de CFDI al proveedor, Carga del XML en el sistema, Validación ante SAT, Confirmación CFDI válido               |
| 7   | **Pago**                            | `#107C10` Verde Oscuro   | Trámite de pago en Recursos Financieros, Registro de número de transferencia/cheque, Confirmación de pago ejecutado      |
| 8   | **Cierre del Expediente**           | `#69797E` Gris Azulado   | Carga de complemento de pago (si aplica), Revisión de expediente completo, Cierre oficial del expediente                 |

### Elementos de Interfaz (WinUI 3 & Toolkit)

**Encabezado del Módulo (fijo):**

| Elemento                            | Especificación                                                                                                                                                                                             |
| ----------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **BreadcrumbBar**                   | "Solicitudes > DSA-2026-089 > Timeline de Hitos"                                                                                                                                                           |
| **StateBadge Large**                | Estado actual de la solicitud con color de fase.                                                                                                                                                           |
| **TextBlock de Fase Activa**        | "Fase 3 de 8: Estudio de Mercado / Cotización" · Subtitle 16pt SemiBold.                                                                                                                                   |
| **ProgressBar "Hitos Completados"** | Barra horizontal. Etiqueta: "14 de 27 hitos completados — 52%". Color de progreso = color de la fase activa. Animación: El ancho de la barra se actualiza con transición fluida cuando se completan hitos. |

**Expander de Fase (bloque de cada fase):**

Encabezado del Expander:

- Número de fase (Badge circular con número + color de la fase).
- Nombre de la fase (Typography: Body, SemiBold).
- Ícono de estado de la fase (✅ / 🔄 animado / ⬜ / ❌).
- Fechas de inicio y fin (si la fase está completada).
- Contador interno: "3 de 4 hitos completados" en tipografía _Caption_.

Contenido del Expander (ListView de hitos):
Cada hito es un ítem del ListView con la siguiente estructura:

```
[Ícono Estado] [Nombre del Hito] ─────────── [Fecha/Hora si completado]
               [Usuario que lo registró]   [Botón "Ver detalles" / "Registrar"]
               [Observaciones (preview truncado)]
```

- **Ícono de estado del hito:**
  - ✅ verde sólido: Completado.
  - 🔄 azul animado (_Pulse_): En proceso activo / pendiente de completar (hito que puede ser registrado ahora por el rol activo).
  - ⬜ gris: Pendiente futuro (no disponible aún).
  - ❌ rojo: Fallido / rechazado.
  - ⚠️ naranja pulsante: Requiere atención del usuario actual.

**Botón "Registrar Hito" (contextual):**
Solo aparece para hitos en estado 🔄 (activo) cuando el rol del usuario tiene permiso para registrar ese tipo de hito. Al presionar, el fondo de la pantalla oscurece con _Smoke_ y emerge un _ContentDialog_:

- Título: "Registrar hito: [Nombre del hito]"
- _TextBox_ multilínea "Observaciones" (obligatorio para hitos de rechazo o con discrepancia, opcional para los demás).
- _Button_ "📎 Adjuntar Documento" (abre el selector de archivo nativo de Windows, para documentos de soporte). El archivo adjunto se guarda en el repositorio SMB del hospital.
- Botones: "Registrar Hito" (acento) y "Cancelar".

**Línea de Tiempo Visual Lateral:**
En el margen izquierdo del carril de fases, una línea vertical continua (4px de ancho) que conecta todas las fases de arriba hacia abajo. La sección de la línea correspondiente a fases completadas tiene el color de esa fase. La sección de la fase activa tiene el color de acento y un efecto de pulso animado (_Pulse_ de opacidad, loop de 2s). Las secciones de fases futuras son grises.

### Comportamiento Visual y Motion (UX)

**Entrada al módulo:** Las fases aparecen con _StaggeredAnimation_ (100ms de desfase entre bloques). La fase activa está pre-expandida. El viewport hace scroll automático hacia la fase activa con animación suave si está fuera del área visible inicial.

**Expansión/Colapso de fase:** Animación de altura fluida (expansión hacia abajo, colapso hacia arriba), con curva _EaseInOutQuart_ (300ms). Los hitos internos aparecen con _StaggeredFadeIn_ (50ms de desfase entre ítems) al expandir.

**Dibujo de la línea de tiempo:** La línea vertical lateral se dibuja con animación _StrokeReveal_ de arriba hacia abajo (600ms, _EaseOut_) al cargar por primera vez. Las secciones coloreadas (fases completadas) se revelan progresivamente.

**Completación de un hito en tiempo real:** Si un hito se registra mientras el usuario está en esta pantalla (ya sea por acción del propio usuario o notificado por SignalR), el ícono del hito transiciona de ⬜/🔄 a ✅ con animación _PopIn_ (escala 0→1.2→1.0, 400ms). La barra de progreso general avanza con animación de ancho fluida. El contador del encabezado del Expander actualiza su número con _NumberFlip_.

### Flujo Operativo (User Journey)

1. El usuario accede al Timeline desde el Detalle de Solicitud (enlace _"Ver timeline completo"_) o desde el NavigationView (ítem "Hitos del Proceso" + selección de solicitud).
2. La vista carga con la fase activa expandida y centrada. El scroll se posiciona automáticamente en la fase activa.
3. El usuario puede expandir cualquier fase completada o pendiente para ver el detalle de sus hitos.
4. Al hacer clic en un hito completado, su _Expander_ interno se expande mostrando las observaciones y el documento adjunto (si existe), con un _HyperlinkButton_ para abrir el documento desde el servidor de archivos.
5. El usuario con rol autorizado hace clic en el botón "Registrar Hito" de un hito activo → _ContentDialog_ de registro → completa observaciones y adjunta documento si es necesario → presiona "Registrar Hito".
6. **Resultado funcional:** El hito queda registrado con timestamp, usuario y observaciones. El ícono cambia a ✅ con animación _PopIn_. Si este hito completa todos los hitos de la fase, la fase completa cambia visualmente a "Completada" y la siguiente fase transiciona a "En Curso".

---

## 🖥️ Módulo 08: Gestión de Proveedores

### Propósito Funcional

Directorio centralizado de los proveedores que participan en el proceso de cotización del Fondo Revolvente del HCG. Gestiona el registro de nuevos proveedores (nombre comercial, RFC, datos de contacto, giro) y proporciona una vista del historial de participación en cotizaciones anteriores. Es la fuente de datos del Módulo de Cotizaciones (Módulo 09), donde los proveedores se buscan y seleccionan para registrar sus propuestas económicas. El RFC de cada proveedor es un dato de alta sensibilidad fiscal que se almacena cifrado y se muestra parcialmente enmascarado según el rol.

### Composición Visual (Fluent Design)

Patrón **Master-Detail** de dos paneles: panel maestro izquierdo (40% del ancho) con la lista de proveedores y barra de búsqueda, y panel de detalle derecho (60% del ancho) con la información completa del proveedor seleccionado. El panel de detalle usa material _Acrylic_ (AcrylicInApp) con CornerRadius 8 y Shadow Depth 8 para diferenciarlo visualmente del panel maestro. El fondo base es _Mica Alt_.

En resoluciones inferiores a 1024px, la vista cambia a modo de una columna: la lista ocupa toda la pantalla y el detalle se navega hacia adelante (con botón de retroceso para volver a la lista).

### Elementos de Interfaz (WinUI 3 & Toolkit)

**Panel Maestro (Lista de Proveedores):**

| Elemento                             | Especificación                                                                                                                                                                                                                                                                                                                          |
| ------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **AutoSuggestBox**                   | Búsqueda por nombre comercial, RFC o giro. Sugerencias aparecen al escribir 2+ caracteres.                                                                                                                                                                                                                                              |
| **ComboBox "Filtrar por Giro"**      | Categorías: Material Médico, Papelería, Equipo, Servicios, Farmacéutico, Otros.                                                                                                                                                                                                                                                         |
| **AppBarButton "+ Nuevo Proveedor"** | Solo visible para Admin y Comprador. Abre _ContentDialog_ de registro.                                                                                                                                                                                                                                                                  |
| **ListView**                         | Cada ítem del ListView muestra: Nombre comercial (Body, Bold), RFC parcialmente enmascarado (`AAAA######***` para roles sin acceso fiscal completo), Giro (Caption, secundario), Última cotización (Caption, terciario, ej: "Última: 15/01/2026"). El ítem seleccionado tiene fondo de color de acento al 12% y borde izquierdo de 3px. |

**Panel de Detalle del Proveedor:**

| Elemento                                 | Especificación                                                                                                                                                                                                                                                                                                       |
| ---------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **TextBlock Nombre Comercial**           | Title, 20pt, Bold.                                                                                                                                                                                                                                                                                                   |
| **TextBlock RFC**                        | Body, monoespaciada. Para roles con acceso fiscal (Admin, Finanzas): RFC completo visible. Para otros roles: parcialmente enmascarado. Ícono de verificación ✅ si el RFC fue validado ante SAT en alguna transacción anterior. _InfoBar_ naranja si hay validaciones CFDI fallidas registradas para este proveedor. |
| **Expander "Datos de Contacto"**         | Dirección completa, teléfono, correo electrónico, nombre del representante.                                                                                                                                                                                                                                          |
| **Expander "Historial de Cotizaciones"** | DataGrid compacto con columnas: Folio DSA, Fecha, Monto Cotizado, Fue seleccionado (✅/❌). Máximo 10 entradas visibles, con botón "Ver historial completo".                                                                                                                                                         |
| **CommandBar del detalle**               | "✏️ Editar" (Admin/Comprador) · "📄 Ver Cotizaciones Activas".                                                                                                                                                                                                                                                       |

**ContentDialog "Nuevo Proveedor"** (con Smoke + Acrylic):

| Campo               | Tipo               | Validación                                                                                                                                                                                          |
| ------------------- | ------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Nombre Comercial    | TextBox            | Obligatorio.                                                                                                                                                                                        |
| RFC                 | TextBox            | Obligatorio. Formato: 12-13 caracteres alfanuméricos (persona física AAAA######XXX, moral AAA######XX). Validación de formato carácter a carácter: borde verde si formato válido, rojo si inválido. |
| Giro / Categoría    | ComboBox           | Obligatorio.                                                                                                                                                                                        |
| Representante Legal | TextBox            | Opcional.                                                                                                                                                                                           |
| Teléfono            | TextBox            | Opcional. Formato de teléfono mexicano (10 dígitos).                                                                                                                                                |
| Correo Electrónico  | TextBox            | Opcional. Validación de formato email.                                                                                                                                                              |
| Dirección           | TextBox multilínea | Opcional.                                                                                                                                                                                           |
| Botones             | —                  | "Guardar Proveedor" (acento) · "Cancelar".                                                                                                                                                          |

### Comportamiento Visual y Motion (UX)

**Selección en la lista:** Al hacer clic en un proveedor del ListView, el panel de detalle actualiza su contenido con _CrossFade_ (200ms). El ítem seleccionado muestra el borde izquierdo de color acento deslizando desde el ítem anterior con animación suave.

**Validación del RFC en tiempo real:** El campo de RFC valida el formato carácter a carácter. El borde del campo transiciona progresivamente: neutro (mientras escribe) → amarillo (formato parcialmente válido) → verde (formato completo y válido) → rojo (formato inválido al perder el foco). Un _ToolTip_ explica el error de formato si el campo queda en rojo.

### Flujo Operativo (User Journey)

1. El usuario escribe en el _AutoSuggestBox_ → la lista filtra en tiempo real.
2. Selecciona un proveedor → el panel de detalle actualiza con _CrossFade_.
3. Expande "Datos de Contacto" y "Historial de Cotizaciones" para revisar la información.
4. Para registrar un nuevo proveedor: "+ Nuevo Proveedor" → _ContentDialog_ con _Smoke_.
5. Completa el formulario, RFC se valida en tiempo real, guarda.
6. **Resultado:** El proveedor aparece en la lista con animación _SlideInFromTop_ en la posición que le corresponde según el orden. _InfoBar_ Success: _"✅ Proveedor [Nombre] registrado correctamente en el directorio."_

---

## 🖥️ Módulo 09: Cotizaciones y Cuadro Comparativo

### Propósito Funcional

Módulo de gestión de las propuestas económicas de proveedores para una solicitud específica. Opera en el contexto de una solicitud concreta (siempre se accede desde el Detalle de una solicitud, no de manera aislada). Gestiona el registro de cotizaciones individuales, verifica el cumplimiento de la Regla de Negocio RN-003 (mínimo 3 cotizaciones de proveedores distintos), y proporciona el **Cuadro Comparativo** — la herramienta de análisis que permite al Comprador DSA seleccionar al proveedor ganador con criterios documentados. La selección del proveedor es irreversible salvo intervención del Administrador.

### Composición Visual (Fluent Design)

El módulo usa un `TabView` (control de pestañas de tabulación) con dos pestañas principales: **"Cotizaciones Recibidas"** y **"Cuadro Comparativo"**. El `TabView` se posiciona inmediatamente bajo el encabezado del módulo (que muestra el folio de la solicitud relacionada). El fondo es _Mica Alt_ con el material _Acrylic_ aplicado a los paneles de contenido de cada pestaña.

### Elementos de Interfaz (WinUI 3 & Toolkit)

**Encabezado del Módulo (fijo):**

| Elemento                         | Especificación                                                                                                                                                                                                                                                                                                                                                    |
| -------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **BreadcrumbBar**                | "Solicitudes > DSA-2026-089 > Cotizaciones"                                                                                                                                                                                                                                                                                                                       |
| **InfoBar RN-003 (persistente)** | Banner coloreado que muestra el progreso de cumplimiento de la regla de mínimo 3 cotizaciones. Estados: "📋 Cotizaciones: 1 de 3 mínimas requeridas" (naranja) → "📋 Cotizaciones: 2 de 3" (amarillo) → "✅ Mínimo de cotizaciones cumplido (3+). Puede continuar." (verde). Este _InfoBar_ nunca puede ser descartado manualmente; se actualiza automáticamente. |

---

**Pestaña 1: "Cotizaciones Recibidas"**

_CommandBar_ de la pestaña:

- _AppBarButton_ "+ Agregar Cotización" (acento): Abre _ContentDialog_ de registro. Solo visible si la solicitud está en fase de cotización y el rol es Comprador/Admin.
- _AppBarButton_ "Exportar lista" (ícono _Download_): Exporta la lista de cotizaciones en formato tabular.

_DataGrid_ de cotizaciones registradas (columnas):

| Columna             | Ancho | Descripción                                                                                     |
| ------------------- | ----- | ----------------------------------------------------------------------------------------------- |
| Proveedor           | flex  | Nombre comercial del proveedor.                                                                 |
| RFC                 | 140px | RFC enmascarado para roles sin acceso fiscal.                                                   |
| Monto Total         | 140px | Suma de todos los ítems cotizados, formato `$XX,XXX.XX MXN`.                                    |
| Días de Entrega     | 80px  | Días hábiles ofrecidos.                                                                         |
| Condiciones de Pago | 120px | Texto libre (ej: "Crédito 30 días").                                                            |
| CFDI / PDF          | 60px  | Ícono de adjunto clickeable que abre el documento desde el servidor SMB.                        |
| Seleccionada        | 80px  | ⭐ si es la cotización seleccionada como ganadora, — si no.                                     |
| Acciones            | 48px  | _ContextFlyout_: "Ver Detalle", "Editar" (si edición permitida), "Eliminar" (con confirmación). |

_ContentDialog_ "Agregar Cotización" (Smoke + Acrylic):

1. **AutoSuggestBox "Proveedor"**: Busca en el directorio de proveedores mientras el usuario escribe. Las sugerencias muestran nombre + RFC. Al seleccionar, los datos del proveedor se pre-llenan.
2. **DataGrid editable de partidas**: Los ítems de la solicitud se pre-cargan como filas. El usuario captura el precio unitario cotizado por el proveedor para cada ítem. El subtotal y el total se calculan automáticamente.
3. **NumberBox "Días de Entrega Ofrecidos"**: Días hábiles. Entero positivo.
4. **TextBox "Condiciones de Pago"**: Texto libre.
5. **Button "📎 Adjuntar PDF de Cotización"**: Selector de archivo nativo de Windows (`.pdf` únicamente). Al adjuntar, muestra el nombre del archivo y un ícono de previsualización. El archivo se guarda en el repositorio SMB del hospital bajo la ruta del expediente.
6. Botones: "Guardar Cotización" (acento) · "Cancelar".

---

**Pestaña 2: "Cuadro Comparativo"**

El Cuadro Comparativo es el artefacto formal de análisis de propuestas. Su contenido es vinculante y se genera como reporte oficial (PDF/Excel) para el expediente.

_DataGrid_ del Cuadro Comparativo — formato especial de tabla pivotada:

- **Filas**: Cada ítem/partida de la solicitud, más una fila de "TOTAL".
- **Columnas dinámicas**: Una columna de descripción del ítem (fija izquierda) + una columna por cada proveedor cotizante (dinámicas, hasta N proveedores). Los encabezados de columna de proveedor muestran: nombre del proveedor + días de entrega + condiciones de pago.
- **Celdas de precio**: El precio unitario cotizado por cada proveedor para cada partida. La celda con el precio más bajo por partida se resalta con fondo verde al 15%.
- **Fila TOTAL**: Suma del monto total de cada proveedor. La columna con el monto total más bajo se resalta con borde de color de acento.

Bajo la tabla comparativa:

| Elemento                                       | Especificación                                                                                                                                                                                                                                                                         |
| ---------------------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **RadioButton "Seleccionar proveedor"**        | Un RadioButton al pie de cada columna de proveedor. Solo uno puede estar activo. Al seleccionar, la columna completa muestra un borde de color de acento de 2px y fondo de acento al 5%.                                                                                               |
| **TextBox "Justificación de Selección"**       | Campo obligatorio cuando el proveedor seleccionado NO es el de menor monto total. Texto libre de máx. 500 caracteres. Validación: si proveedor seleccionado ≠ menor monto y este campo está vacío, el botón de confirmar permanece deshabilitado con un _ToolTip_ explicando la razón. |
| **Button "Confirmar Selección de Proveedor"**  | AccentButtonStyle. Habilitado solo si: hay ≥ 3 cotizaciones Y se ha seleccionado un proveedor Y (si aplica) la justificación está completa. Al presionar → _ContentDialog_ de confirmación final con advertencia de irreversibilidad.                                                  |
| **AppBarButton "Exportar Cuadro Comparativo"** | Genera el documento oficial del cuadro comparativo en formato PDF o Excel mediante SSRS. Disponible para Admin y Comprador.                                                                                                                                                            |

### Comportamiento Visual y Motion (UX)

**Transición entre pestañas:** El contenido de cada pestaña transiciona con _FadeTransition_ (150ms) al cambiar de pestaña.

**Agregado de cotización:** Al guardar una nueva cotización, el _InfoBar_ RN-003 actualiza su contador con _NumberFlip_. El DataGrid de cotizaciones inserta la nueva fila con _SlideInFromTop_ (200ms). Si se alcanza el mínimo de 3 cotizaciones con esta adición, el _InfoBar_ cambia a verde con animación de _CrossFade_ y destella brevemente en verde brillante para comunicar el cumplimiento del requisito.

**Selección de proveedor en el cuadro comparativo:** Al seleccionar un _RadioButton_, la columna del proveedor seleccionado recibe borde de acento con animación de _FadeIn_ del borde. Las columnas no seleccionadas se atenúan ligeramente (opacidad 0.85). El botón de confirmar transiciona de _Disabled_ a _Enabled_ con _FadeIn_ cuando se cumplen todas las condiciones.

**Confirmación de selección:** El _ContentDialog_ emerge con _Smoke_. Al confirmar, la selección queda fija. El Cuadro Comparativo entra en modo de **solo lectura** completo: el _RadioButton_ del proveedor ganador muestra ícono de trofeo 🏆 en lugar del punto de selección. El botón de confirmar desaparece, reemplazado por el texto "Proveedor seleccionado: [Nombre] — Selección confirmada el [fecha]".

### Flujo Operativo (User Journey)

1. El Comprador DSA accede al módulo de cotizaciones desde el Detalle de la Solicitud.
2. En la pestaña "Cotizaciones Recibidas", agrega las cotizaciones de los proveedores consultados.
3. El _InfoBar_ RN-003 progresa de 1/3 a 2/3 a 3/3 con cada cotización agregada.
4. Al alcanzar 3+ cotizaciones, cambia a la pestaña "Cuadro Comparativo" para analizar las propuestas.
5. Selecciona el proveedor ganador con el _RadioButton_ de su columna. Si no es el de menor precio, escribe la justificación en el campo de texto.
6. Presiona "Confirmar Selección" → _ContentDialog_ de confirmación final.
7. **Resultado funcional:** El proveedor seleccionado queda registrado en el expediente. El estado de la solicitud avanza al siguiente hito (Generación de Pedido). El cuadro comparativo queda en modo solo lectura como evidencia documental. El hito de "Selección de proveedor" queda registrado con timestamp y usuario.

---

## 🖥️ Módulo 10: Validación Fiscal — Panel CFDI

### Propósito Funcional

Panel especializado para el proceso de recepción, carga y validación del Comprobante Fiscal Digital por Internet (CFDI 4.0) emitido por el proveedor seleccionado, operado por el rol de **Recursos Financieros**. Ejecuta la validación de autenticidad del CFDI contra el servicio del **SAT (Servicio de Administración Tributaria)**, gestionando visualmente los estados de procesamiento, los resultados de validación (válido/inválido) y los escenarios de fallo del servicio externo con reintentos automáticos (política Retry + Circuit Breaker — RN-004). Este módulo es el punto de contacto entre el sistema de gestión hospitalario y la infraestructura fiscal del SAT.

### Composición Visual (Fluent Design)

El módulo adopta una composición de panel central enfocado (ancho máximo 640px, centrado en el ContentFrame) con fondo _Mica Alt_. El contenido se organiza en tres `CardPanel` apilados verticalmente, cada uno representando una fase del proceso de validación. La claridad y la reducción de elementos distractores son prioritarias en este módulo, dado que las acciones ejecutadas tienen consecuencias fiscales y legales directas.

El _CardPanel_ central de estado de validación usa Shadow Depth 16 para destacarlo como el elemento de mayor relevancia en pantalla. Los estados de resultado (válido/inválido/reintentando) se comunican con cambios de color del borde del _CardPanel_ además de los íconos.

### Elementos de Interfaz (WinUI 3 & Toolkit)

**BreadcrumbBar:** "Solicitudes > DSA-2026-089 > Validación CFDI"

**CardPanel 1 — "Contexto de la Validación":**

| Elemento                                 | Especificación                                                                                                                              |
| ---------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------- |
| **TextBlock Proveedor Seleccionado**     | Nombre del proveedor + RFC (completo, visible para rol Finanzas).                                                                           |
| **MontoDisplay Cotización Seleccionada** | Monto de la cotización seleccionada como referencia para verificar que el CFDI cuadre.                                                      |
| **InfoBar informativo**                  | "El CFDI debe coincidir con el proveedor y monto de la cotización seleccionada. La validación verifica la autenticidad fiscal ante el SAT." |

**CardPanel 2 — "Archivo CFDI":**

| Elemento                                          | Especificación                                                                                                                                                                                                                                                                                                                                                                 |
| ------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **Button "📎 Adjuntar Archivo CFDI (XML)"**       | Selector de archivo nativo de Windows. Solo acepta archivos `.xml`. Al adjuntar, el archivo se guarda en el repositorio SMB del hospital.                                                                                                                                                                                                                                      |
| **Panel de vista previa del CFDI (post-adjunto)** | Una vez adjuntado el XML, el sistema extrae y muestra en modo solo lectura: UUID/Folio Fiscal del CFDI, Nombre y RFC del Emisor, Nombre y RFC del Receptor (HCG), Total del CFDI, Fecha de Emisión. Icono ✅/❌ comparando el RFC del Emisor con el RFC del proveedor seleccionado y el monto con la cotización (pre-validación de consistencia local antes de enviar al SAT). |

**CardPanel 3 — "Estado de Validación SAT":**

Este CardPanel es el centro de la experiencia del módulo. Su contenido cambia según el estado del proceso de validación:

| Estado                                            | Contenido del CardPanel                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| ------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Inicial (sin archivo adjunto)**                 | Ícono 📋 gris, texto: "Adjunte el archivo XML del CFDI para iniciar la validación."                                                                                                                                                                                                                                                                                                                                                                           |
| **Listo para validar**                            | Ícono ✅ verde outline, texto: "CFDI adjunto y consistente. Presione 'Iniciar Validación'." · _Button_ "🧾 Iniciar Validación Fiscal" (AccentButtonStyle, ancho completo).                                                                                                                                                                                                                                                                                    |
| **Validando (en proceso)**                        | _ProgressRing_ (36px, color de acento) · TextBlock: "Verificando autenticidad del CFDI con el servicio fiscal del SAT... Intento 1 de 3." · _Button_ "Cancelar validación" (TextButtonStyle). Borde del CardPanel en color de acento.                                                                                                                                                                                                                         |
| **Resultado: CFDI Válido**                        | Ícono ✅ 64px (verde sólido, animación _PopIn_) · TextBlock Bold: "CFDI Válido" · TextBlock secundario: "RFC del emisor verificado. Monto coincide. UUID registrado en el SAT." · _InfoBar_ Success verde. · _Button_ "Registrar Validación Exitosa" (acento verde). Borde del CardPanel en verde.                                                                                                                                                            |
| **Resultado: CFDI Inválido**                      | Ícono ❌ 64px (rojo, animación _Shake_) · TextBlock Bold: "CFDI Inválido" · TextBlock secundario: Descripción del error específico del SAT (ej: "UUID no registrado en el SAT", "RFC del emisor no corresponde"). · _InfoBar_ Error rojo. · _Button_ "Cargar nuevo CFDI" (neutro). Borde del CardPanel en rojo.                                                                                                                                               |
| **Reintentando automáticamente**                  | Ícono ⚠️ pulsante (naranja, _Pulse_ loop) · TextBlock: "El servicio fiscal no responde. Reintentando automáticamente..." · _ProgressBar_ de cuenta regresiva (muestra segundos hasta el próximo intento) · TextBlock: "Intento [N] de 3 — Próximo intento en [X] segundos." · _Button_ "Cancelar reintentos" (TextButtonStyle). Borde del CardPanel en naranja.                                                                                               |
| **Reintentos agotados (Circuit Breaker abierto)** | Ícono 🔌 gris · TextBlock Bold: "Servicio fiscal temporalmente no disponible" · _InfoBar_ Warning: "No fue posible conectar con el SAT tras 3 intentos. La solicitud queda en estado 'ERROR_SAT_REINTENTANDO'. El sistema reintentará automáticamente cuando el servicio esté disponible. No es necesaria ninguna acción adicional de su parte en este momento." · _Button_ "Notificar a soporte TI" (TextButtonStyle). Borde del CardPanel en naranja tenue. |

**Expander "Historial de Intentos de Validación":**
Lista cronológica de todos los intentos de validación: timestamp de inicio, resultado (Exitoso/Fallo SAT/Timeout/Inválido), descripción del error si aplica, duración del intento. Colapsado por defecto.

### Comportamiento Visual y Motion (UX)

**Transiciones entre estados del CardPanel 3:**
Todos los cambios de estado del CardPanel central usan _CrossFade_ (300ms) para el contenido interno. El cambio de color del borde usa una transición de color fluida (400ms). Los íconos de resultado (✅ o ❌) aparecen con animación _PopIn_ (escala 0→1.2→1.0, 400ms con resorte). El fondo del CardPanel pulsa brevemente en el color del resultado (verde 200ms para éxito, rojo 200ms para fallo) antes de normalizar.

**Estado de reintento:** La _ProgressBar_ de cuenta regresiva bajo el texto de reintento avanza en tiempo real mostrando los segundos restantes. Al llegar a 0, el ícono del nuevo intento pulsa brevemente y el contador se actualiza.

**Validación exitosa completa:** Tras presionar "Registrar Validación Exitosa", el botón entra en estado de carga brevemente → el módulo cierra con _ConnectedAnimation_ que lleva el ícono ✅ de vuelta hacia el _StateBadge_ del encabezado del Detalle de Solicitud (donde el estado ya refleja el avance a `EN_PAGO`).

### Flujo Operativo (User Journey)

1. El usuario de Recursos Financieros accede al panel desde el Detalle de Solicitud (botón "Ir a Validación CFDI" visible cuando el estado es `EN_VALIDACION_CFDI`).
2. Revisa el contexto: proveedor, RFC, monto de referencia.
3. Adjunta el archivo XML del CFDI. El sistema extrae y muestra los datos del CFDI para revisión pre-validación.
4. Si los datos extraídos no coinciden con el proveedor/monto esperado, un _InfoBar_ Warning alerta de la inconsistencia antes de continuar.
5. Presiona "Iniciar Validación Fiscal". El proceso comienza.
6. **Escenario exitoso:** Validación completada en el primer intento (o en los reintentos). Estado: CFDI Válido. Usuario presiona "Registrar Validación Exitosa". La solicitud avanza a `EN_PAGO`.
7. **Escenario de CFDI inválido:** SAT rechaza el CFDI. El usuario debe solicitar al proveedor una nueva factura corregida. Presiona "Cargar nuevo CFDI" para adjuntar el nuevo XML y repetir el proceso.
8. **Escenario de fallo del servicio SAT:** El sistema gestiona automáticamente los reintentos. Si se agotan, la solicitud queda en `ERROR_SAT_REINTENTANDO` y el sistema reintentará cuando el servicio SAT esté disponible.

---

## 🖥️ Módulo 11: Reportes y Exportación

### Propósito Funcional

Centro de generación de documentos oficiales y reportes de gestión. Proporciona acceso a los reportes del sistema, generados por el motor **SSRS (SQL Server Reporting Services)** del hospital en formatos estándar para documentación oficial (PDF) y análisis de datos (Excel/XLSX). Cada tipo de reporte está disponible según el rol del usuario. Los reportes en PDF son documentos con validez documental para el expediente de la solicitud (el Cuadro Comparativo oficial, por ejemplo).

### Composición Visual (Fluent Design)

La pantalla se organiza como una galería de tarjetas de reporte (_GridView_ con tarjetas de _CardPanel_) sobre fondo _Mica Alt_. Las tarjetas se distribuyen en una cuadrícula responsiva: 3 columnas en resoluciones ≥1280px, 2 columnas en resoluciones menores. Cada tarjeta tiene Shadow Depth 4, CornerRadius 8, altura fija de 200px.

### Elementos de Interfaz (WinUI 3 & Toolkit)

**Barra de Herramientas Superior:**

| Elemento                  | Especificación                                                                             |
| ------------------------- | ------------------------------------------------------------------------------------------ |
| **TextBlock Título**      | "Reportes y Documentos Oficiales" · Display 24pt Bold.                                     |
| **AutoSuggestBox**        | Búsqueda de reporte por nombre. Filtra las tarjetas visibles en tiempo real.               |
| **ToggleButton de Vista** | Alterna entre vista de cuadrícula (_GridView_) y vista de lista (_ListView_ más compacto). |

**Tarjetas de Reporte (GridView):**

Cada tarjeta tiene:

- Ícono de tipo de reporte (64px, glyph Fluent).
- Nombre del reporte (Subtitle 16pt SemiBold).
- Descripción breve (Caption, 2 líneas).
- Parámetros de configuración (inline en la tarjeta):
  - _AutoSuggestBox_ o _ComboBox_ según el parámetro (Folio DSA, Ejercicio Fiscal, Mes/Año).
  - _SegmentedControl_ de formato: "PDF" · "Excel".
- Botón "📥 Generar" (AccentButtonStyle).

| Reporte                                | Parámetros                 | Roles Permitidos |
| -------------------------------------- | -------------------------- | ---------------- |
| **Cuadro Comparativo de Cotizaciones** | Folio DSA · Formato        | Admin, Comprador |
| **Dashboard KPI del Ejercicio**        | Ejercicio Fiscal · Formato | Admin, Consulta  |
| **Reporte Mensual de Actividad**       | Mes · Año · Formato        | Admin, Consulta  |
| **Expediente Completo de Solicitud**   | Folio DSA (solo PDF)       | Todos            |
| **Log de Auditoría de Estados**        | Rango de fechas · Formato  | Solo Admin       |

**Panel de Historial de Generación (Expander):**
Al pie de la pantalla, un _Expander_ con el historial de reportes generados por el usuario en la sesión activa: nombre del reporte, parámetros usados, timestamp, y botón "📥 Descargar de nuevo" (regenera el reporte con los mismos parámetros).

### Comportamiento Visual y Motion (UX)

**Generación de reporte:** Al presionar "Generar", el botón entra en estado de carga (_ProgressRing_ + texto "Generando..."). La tarjeta muestra un _ProgressBar_ indeterminado en su borde inferior (animación de pulso). Al completarse (el motor SSRS entrega el archivo), el botón retorna a su estado normal y un _TeachingTip_ emerge desde el botón ofreciendo las opciones: "Abrir ahora" y "Guardar en equipo" (abre el diálogo de guardado nativo de Windows, con la carpeta de expedientes del SMB preseleccionada como destino sugerido).

**Hover sobre tarjetas:** _RevealHighlight_ al pasar el cursor. Las tarjetas escalan ligeramente (1.0→1.02, 150ms) al hacer hover.

### Flujo Operativo (User Journey)

1. El usuario navega al módulo de Reportes desde el _NavigationView_.
2. Selecciona la tarjeta del reporte deseado (o lo busca en el _AutoSuggestBox_).
3. Configura los parámetros inline en la tarjeta (folio, período, formato).
4. Presiona "Generar". El sistema invoca el motor SSRS con los parámetros proporcionados.
5. Al completarse, el usuario elige abrir el documento o guardarlo en el equipo.
6. El reporte se registra en el historial de generación de la sesión.

---

## 🖥️ Módulo 12: Centro de Notificaciones en Tiempo Real

### Propósito Funcional

Sistema de comunicación asíncrona que mantiene a todos los usuarios informados sobre eventos relevantes del sistema sin interrumpir su flujo de trabajo. Recibe actualizaciones del **SignalR Hub** del servidor en tiempo real y las presenta de forma contextual, jerárquica y filtrada por rol. Persiste el historial de notificaciones durante la sesión activa. Para eventos críticos, emite notificaciones nativas de Windows 11 (Toast) que son visibles incluso con la aplicación minimizada.

### Composición Visual (Fluent Design)

El centro de notificaciones es un `Flyout` de 380px de ancho fijo que emerge desde el ícono de campana de la _TitleBar_. Material: _Acrylic_ (AcrylicSystemBackdrop) con Shadow Depth 64. El `Flyout` no es modal: el usuario puede continuar interactuando con la aplicación mientras está abierto.

### Elementos de Interfaz (WinUI 3 & Toolkit)

**Estructura del Flyout:**

| Elemento                       | Especificación                                                                                                                                                     |
| ------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| **Encabezado**                 | TextBlock "Notificaciones" (Body, Bold) + _HyperlinkButton_ "Marcar todas como leídas" (alineado a la derecha).                                                    |
| **TabView interno**            | Pestañas: "Recientes" (todas las notificaciones de la sesión) · "No leídas" (filtrado).                                                                            |
| **ListView de notificaciones** | Control virtualizado con scroll interno. Orden cronológico inverso (más reciente arriba).                                                                          |
| **Pie del Flyout**             | Indicador de estado de la conexión SignalR: punto verde sólido (conectado) / punto naranja pulsante (reconectando) / punto gris (desconectado). + Texto de estado. |

**Estructura de un ítem de notificación:**

```
[Ícono Tipo]  [Título Bold 13pt]                        [Timestamp relativo]
              [Descripción 12pt color secundario]
              [HyperlinkButton "Ver Detalle"]  [Button "Descartar"]
```

El fondo de los ítems no leídos usa el color de acento al 8% de opacidad. Los ítems leídos tienen fondo neutro. Un punto de 8px de diámetro en color de acento aparece a la izquierda del ícono para ítems no leídos.

**Tipos de notificaciones y sus íconos:**

| Tipo                          | Ícono | Color   | Condición                                                   |
| ----------------------------- | ----- | ------- | ----------------------------------------------------------- |
| Cambio de estado de solicitud | 🔄    | Azul    | Solicitud del scope del usuario transicionó de estado       |
| Alerta de proceso             | ⚠️    | Naranja | Solicitud detectada en situación de riesgo                  |
| Bloqueo de edición            | 🔒    | Gris    | Un expediente que el usuario necesita editar está bloqueado |
| Validación CFDI fallida       | 🧾❌  | Rojo    | Error de validación en solicitud del scope del usuario      |
| Acción completada             | ✅    | Verde   | Hito completado en solicitud que el usuario monitoreaba     |
| Reconexión en tiempo real     | 🔌    | Naranja | Pérdida y recuperación de la conexión SignalR               |

**Toast de Windows 11 (notificación nativa del sistema):**
Para eventos de alta prioridad (solicitudes rechazadas que requieren acción urgente, errores de CFDI de alto impacto, bloqueos de edición de expedientes críticos), el sistema emite una notificación _Toast_ nativa de Windows 11. El _Toast_ incluye: nombre de la aplicación + ícono, título del evento, descripción breve, botón de acción rápida "Ver Expediente" (que lleva directamente al Detalle de la solicitud relacionada). El _Toast_ se descarta automáticamente en 5 segundos; el usuario puede expandirlo durante esos 5 segundos para ver más detalle.

### Comportamiento Visual y Motion (UX)

**Llegada de nueva notificación:**

1. El `BadgeControl` en la TitleBar ejecuta _PopIn_ (0→1.2→1.0, 300ms, resorte).
2. Si el número aumenta de N a N+1, ejecuta _NumberFlip_ (el número anterior desliza hacia arriba y desaparece; el nuevo aparece desde abajo).
3. El ícono de campana ejecuta _Shake_ sutil (2 oscilaciones, 200ms) para atraer atención periférica.
4. Si el `Flyout` de notificaciones está abierto, la nueva notificación aparece en la parte superior del _ListView_ con _SlideInFromTop_ (200ms).
5. Si el `Flyout` está cerrado y el evento es de alta prioridad, se emite el _Toast_ nativo de Windows 11.

**Marcar como leída:** Al hacer clic en cualquier ítem de notificación o en "Ver Detalle", el ítem se marca como leído: el punto de indicador hace _FadeOut_ (300ms), el fondo del ítem transiciona del color de acento al neutro con _CrossFade_ (500ms). El _BadgeControl_ numérico decrementa con _NumberFlip_.

**Estado de reconexión:** Si la conexión SignalR se pierde, el indicador en el pie del _Flyout_ cambia a naranja pulsante con texto "Reconectando... (intento 2 de 5)". El ícono de campana en la TitleBar adquiere una superposición visual de punto naranja. Al reconectarse, el indicador vuelve a verde sólido con animación _FadeIn_ y se emite una notificación informativa interna: "Conexión en tiempo real restaurada."

**Conexión desconectada persistentemente:** Si la reconexión falla tras todos los intentos, el indicador cambia a gris sólido con _ToolTip_: "Sin conexión en tiempo real. Los datos pueden no estar actualizados. Actualice manualmente las pantallas para obtener los datos más recientes."

### Flujo Operativo (User Journey)

1. La conexión SignalR se establece automáticamente en segundo plano al iniciar la sesión. El indicador de estado en el pie del _Flyout_ confirma la conexión activa.
2. El usuario trabaja en cualquier módulo. Al ocurrir un evento relevante en el sistema (otro usuario cambia el estado de una solicitud del scope del usuario), la notificación llega automáticamente.
3. El _BadgeControl_ se actualiza y el ícono de campana shakea. El usuario hace clic en la campana → el _Flyout_ de notificaciones emerge.
4. El usuario revisa la notificación y hace clic en "Ver Detalle" → la aplicación navega directamente al Detalle de la solicitud relacionada, con el _Flyout_ cerrándose con _FadeOut_.
5. Para eventos críticos (ej: CFDI rechazado del área de Finanzas), el _Toast_ nativo emerge en el escritorio. El usuario puede accionar "Ver Expediente" directamente desde el _Toast_ para navegar al módulo pertinente.

---

## 🖥️ Módulo 13: Panel de Administración — Roles y Accesos

### Propósito Funcional

Panel de control exclusivo del rol **Administrador del Sistema**. Proporciona las herramientas para: gestionar la asignación de roles internos de la aplicación a usuarios del directorio activo, supervisar el historial de auditoría de todas las acciones ejecutadas en el sistema, configurar los parámetros de negocio del sistema (montos máximos, mínimos de cotizaciones, tiempos de bloqueo), y monitorear el estado operativo de los servicios integrados (servicio de validación SAT, servidor de archivos SMB, hub SignalR). La autenticación de usuarios no se gestiona aquí — se delega al directorio activo del hospital.

### Composición Visual (Fluent Design)

El módulo usa material **Mica Alt** de tono ligeramente diferenciado al del resto de la aplicación, comunicando visualmente que este es un área de configuración de alta responsabilidad, no de operación diaria. La estructura usa un segundo nivel de _NavigationView_ (ancho fijo 180px, siempre visible, sin colapso) en la columna izquierda, con el área de contenido de la subsección activa a la derecha.

### Elementos de Interfaz (WinUI 3 & Toolkit)

**NavigationView Secundario (columna izquierda, 180px):**

| Ítem                      | Ícono      |
| ------------------------- | ---------- |
| Usuarios y Roles          | `People`   |
| Auditoría de Acciones     | `History`  |
| Configuración del Sistema | `Settings` |

---

**Subsección: "Usuarios y Roles"**

_CommandBar_: _AutoSuggestBox_ "Buscar usuario" · _AppBarButton_ "Actualizar".

_DataGrid_ de usuarios:

| Columna              | Especificación                                                                                                |
| -------------------- | ------------------------------------------------------------------------------------------------------------- |
| Nombre Completo      | Nombre y apellidos del usuario.                                                                               |
| Usuario de Red       | `dominio\usuario`, tipografía monoespaciada.                                                                  |
| Rol en la Aplicación | _ComboBox_ editable inline con los 6 roles disponibles. Al modificar, emerge _ContentDialog_ de confirmación. |
| Último Acceso        | Timestamp `dd/MM/YYYY HH:MM`.                                                                                 |
| Estado               | _ToggleSwitch_ Activo/Inactivo. Filas con estado Inactivo se muestran con opacidad 0.6.                       |

_ContentDialog_ de confirmación de cambio de rol: "¿Cambiar el rol de [Nombre Completo] de **[Rol Anterior]** a **[Rol Nuevo]**? Este cambio se aplicará en el próximo inicio de sesión del usuario."

---

**Subsección: "Auditoría de Acciones"**

_CommandBar_ con filtros:

- _DateRangePicker_ "Período".
- _ComboBox_ "Tipo de Acción" (Creación de solicitud, Transición de estado, Registro de hito, Validación CFDI, Generación de reporte, Cambio de rol, Inicio/Cierre de sesión).
- _ComboBox_ "Usuario" (todos los usuarios del sistema).
- _AppBarButton_ "Exportar Log de Auditoría" (Excel).

_DataGrid_ de auditoría (columnas):

| Columna         | Ancho                        |
| --------------- | ---------------------------- |
| Timestamp       | 160px                        |
| Usuario         | 120px                        |
| Tipo de Acción  | 180px                        |
| Folio Afectado  | 130px                        |
| Estado Anterior | 160px (con StateBadge Small) |
| Estado Nuevo    | 160px (con StateBadge Small) |
| IP de Origen    | 120px                        |

---

**Subsección: "Configuración del Sistema"**

Dos _CardPanel_ dentro del área de contenido:

_CardPanel_ "Parámetros de Negocio":

| Parámetro                                            | Tipo de Input     | Restricciones                                                                                                           |
| ---------------------------------------------------- | ----------------- | ----------------------------------------------------------------------------------------------------------------------- |
| Monto Máximo Fondo Revolvente (MXN)                  | _NumberBox_       | Campo protegido. Al modificar → _ContentDialog_ doble confirmación con campo de razón del cambio.                       |
| Mínimo de Cotizaciones Requeridas (RN-003)           | _NumberBox_       | Entero positivo ≥ 1. Misma protección.                                                                                  |
| Tiempo de Expiración de Bloqueo de Edición (minutos) | _NumberBox_       | Rango 5-120 minutos.                                                                                                    |
| Días en Estado para Activar Alerta                   | _NumberBox_       | Umbral para el Dashboard de alertas.                                                                                    |
| Botón "Guardar Cambios"                              | _Button_ (acento) | Solo habilitado si hay cambios no guardados. Requiere _ContentDialog_ de confirmación con resumen de cambios a aplicar. |

_CardPanel_ "Estado de Servicios Integrados":

| Servicio                            | Indicador           | Información Adicional                                                                                 |
| ----------------------------------- | ------------------- | ----------------------------------------------------------------------------------------------------- |
| Servicio de Validación Fiscal (SAT) | Semáforo verde/rojo | Timestamp de última verificación exitosa. Si rojo: timestamp del fallo y número de errores recientes. |
| Servidor de Archivos SMB            | Semáforo verde/rojo | Ruta UNC y timestamp de última escritura exitosa.                                                     |
| Hub de Notificaciones SignalR       | Semáforo verde/rojo | Número de conexiones activas en este momento.                                                         |

Si algún indicador está en rojo, muestra un _InfoBar_ de `Warning` en el área de la sección con el mensaje de error y un _Button_ "Intentar reconexión manual".

### Comportamiento Visual y Motion (UX)

**Transición entre subsecciones:** _SlideInFromRight_ (avance en el NavigationView secundario) o _SlideInFromLeft_ (retroceso), 200ms, EaseOutQuart.

**Filas de usuarios inactivos:** Transición suave de opacidad al activar/desactivar el _ToggleSwitch_ (400ms, EaseInOut).

**Indicadores de estado de servicios:** Los indicadores en rojo ejecutan _Pulse_ (opacidad oscilante 1.0→0.5→1.0, ciclo de 2 segundos) para atraer atención. Al cambiar de rojo a verde (servicio recuperado), el indicador ejecuta _PopIn_ en verde como confirmación visual.

### Flujo Operativo (User Journey)

1. El Administrador accede al módulo desde el ítem "⚙️ Administración" del NavigationView principal.
2. En "Usuarios y Roles": busca un usuario, modifica su rol en el _ComboBox_ inline, confirma en el _ContentDialog_ → el cambio se registra para aplicarse en el próximo inicio de sesión del usuario afectado.
3. En "Auditoría": filtra por período y tipo de acción, revisa el log, exporta para cumplimiento normativo.
4. En "Configuración": ajusta parámetros de negocio con doble confirmación. Monitorea el estado de servicios en tiempo real.

---

## 18. Especificación Transversal: Motor de Estados y Transiciones Visuales

### 18.1 Los 30 Estados del Ciclo de Vida

El `SolicitudStateMachine` del dominio rige las 38 transiciones permitidas entre los 30 estados del ciclo de vida de una solicitud. Desde la perspectiva de la interfaz, cada estado tiene: un nombre normalizado, una etiqueta legible para el usuario, un color de fase, un ícono Segoe Fluent Icons, y un comportamiento visual especial (si aplica).

| Estado (Normalizado)      | Etiqueta UI                | Fase | Color     | Ícono                      | Comportamiento Visual                 |
| ------------------------- | -------------------------- | ---- | --------- | -------------------------- | ------------------------------------- |
| `RECEPCIONADO`            | Recepcionado               | 1    | `#616161` | Mail                       | —                                     |
| `EN_REVISION`             | En revisión                | 1    | `#616161` | Search                     | —                                     |
| `VALIDADO`                | Validado                   | 1    | `#0078D4` | CheckMark                  | —                                     |
| `RECHAZADO_VALIDACION`    | Rechazado en revisión      | 1    | `#C50F1F` | Cancel                     | Pulse                                 |
| `EN_FRACCIONAMIENTO`      | Alerta fraccionamiento     | 1    | `#BC4B00` | Warning                    | Pulse                                 |
| `EN_AUTORIZACION_CAA`     | En autorización CAA        | 2    | `#FF8C00` | Accounts                   | Breathing (para rol CAA)              |
| `AUTORIZADO_CAA`          | Autorizado por CAA         | 2    | `#FF8C00` | CheckMark                  | —                                     |
| `RECHAZADO_CAA`           | Rechazado por CAA          | 2    | `#C50F1F` | Cancel                     | Pulse                                 |
| `RECHAZADO_CAA_REINTENTO` | Reintento tras rechazo CAA | 2    | `#F7A600` | Refresh                    | —                                     |
| `SIN_COTIZACIONES`        | Sin cotizaciones           | 3    | `#00B7C3` | ShoppingCart               | —                                     |
| `EN_COTIZACION`           | En cotización              | 3    | `#00B7C3` | ShoppingCart               | Gradiente animado                     |
| `COTIZACION_COMPLETA`     | Cotizaciones completas     | 3    | `#00B7C3` | ShoppingCartCheckmark      | —                                     |
| `CUADRO_COMPARATIVO`      | Elaborando comparativo     | 4    | `#E74F2D` | TableSimple                | Gradiente animado                     |
| `PROVEEDOR_SELECCIONADO`  | Proveedor seleccionado     | 4    | `#E74F2D` | Trophy                     | —                                     |
| `PEDIDO_GENERADO`         | Pedido generado            | 4    | `#E74F2D` | Document                   | —                                     |
| `EN_ENTREGA`              | En entrega                 | 5    | `#6DC253` | Truck                      | Gradiente animado                     |
| `ENTREGADO`               | Entregado                  | 5    | `#6DC253` | TruckCheckmark             | —                                     |
| `CON_DISCREPANCIA`        | Con discrepancia           | 5    | `#BC4B00` | TruckWarning               | Pulse                                 |
| `EN_RECEPCION_BIENES`     | Recibiendo bienes          | 5    | `#5C2D91` | Package                    | Gradiente animado                     |
| `RECEPCIONADO_BIENES`     | Bienes recepcionados       | 5    | `#5C2D91` | PackageCheckmark           | —                                     |
| `EN_VALIDACION_CFDI`      | Validando CFDI             | 6    | `#9B4F9E` | ProtectedDocument          | Gradiente animado (para rol Finanzas) |
| `CFDI_VALIDO`             | CFDI válido                | 6    | `#9B4F9E` | ProtectedDocumentCheckmark | —                                     |
| `CFDI_INVALIDO`           | CFDI inválido              | 6    | `#C50F1F` | ProtectedDocumentError     | Pulse                                 |
| `ERROR_SAT_REINTENTANDO`  | Error SAT / Reintentando   | 6    | `#BC4B00` | CloudError                 | Pulse intenso                         |
| `EN_PAGO`                 | Tramitando pago            | 7    | `#107C10` | Money                      | Gradiente animado                     |
| `PAGADO`                  | Pago ejecutado             | 7    | `#107C10` | MoneyCheckmark             | —                                     |
| `ERROR_PAGO`              | Error en pago              | 7    | `#C50F1F` | MoneyError                 | Pulse                                 |
| `EN_CIERRE`               | En proceso de cierre       | 8    | `#69797E` | Folder                     | Gradiente animado                     |
| `CERRADO`                 | Cerrado                    | 8    | `#0B5710` | FolderCheckmark            | Opacidad reducida (85%)               |
| `CANCELADO`               | Cancelado                  | 8    | `#C50F1F` | FolderOff                  | Opacidad reducida (85%)               |

### 18.2 Comportamientos Visuales Especiales de Estados

**Gradiente animado (estados `EN_*`):**
El fondo del _StateBadge_ muestra un gradiente horizontal que se desplaza de izquierda a derecha en un bucle de 2 segundos (velocidad suave, `LinearEasing`). El gradiente va del color de la fase al 70% del color de la fase en su punto más claro. Comunica que el proceso está activamente en movimiento.

**Pulse (estados de error/alerta):**
El borde del _StateBadge_ oscila en opacidad: 100% → 70% → 100%, ciclo de 2 segundos (`EaseInOut`). Atrae la atención periférica del usuario hacia solicitudes que requieren atención sin ser disruptivo.

**Breathing (estados de acción requerida para el rol activo):**
Igual que _Pulse_ pero aplicado sobre toda la fila del `DataGrid` (el fondo de la fila completa oscila suavemente). Solo se activa para los estados que el rol activo puede accionar en este momento. Por ejemplo: las filas en estado `EN_AUTORIZACION_CAA` ejecutan _Breathing_ para el rol CAA; las filas en `EN_VALIDACION_CFDI` para el rol Finanzas.

**Opacidad reducida (estados terminales):**
Los estados `CERRADO` y `CANCELADO` muestran todos sus elementos con opacidad 85%, comunicando que son registros históricos que ya no requieren acción.

---

## 19. Guía de Motion y Microinteracciones

### 19.1 Principios Rectores del Motion Design

El sistema de animaciones de la aplicación se rige por cuatro principios que gobiernan todas las decisiones de motion:

1. **Propósito:** Cada animación comunica algo específico: estado del sistema, resultado de una acción, relación espacial entre pantallas, o progreso de un proceso. Las animaciones decorativas sin función comunicativa no están permitidas.

2. **Naturalidad:** Las curvas de animación imitan el comportamiento físico real: los objetos aceleran al inicio y desaceleran al llegar a su destino (no se mueven a velocidad constante). Las curvas estándar son `EaseOut` para aparición de elementos (el elemento llega suavemente a su posición final) y `EaseIn` para desaparición (el elemento sale rápidamente, sin hesitación).

3. **Eficiencia:** Las duraciones son las mínimas necesarias para ser perceptibles y comunicar el estado. Las microinteracciones son entre 50-200ms. Las transiciones de pantalla son entre 200-300ms. Las animaciones de resultado (éxito/error) son entre 300-500ms. Las secuencias de entrada escalonada (_StaggeredAnimation_) se limitan a un total de 400ms máximo, independientemente del número de elementos.

4. **Coherencia:** Animaciones del mismo tipo (entrada, salida, éxito, error) se comportan de manera idéntica en toda la aplicación. El usuario desarrolla expectativas y la interfaz las cumple consistentemente.

### 19.2 Catálogo Completo de Animaciones Estándar

| Animación              | Duración                      | Curva de Aceleración               | Uso Primario                                                  |
| ---------------------- | ----------------------------- | ---------------------------------- | ------------------------------------------------------------- |
| **FadeIn**             | 150ms                         | EaseOut                            | Aparición suave de elementos, resultados de carga             |
| **FadeOut**            | 150ms                         | EaseIn                             | Desaparición de elementos, cierre de paneles                  |
| **SlideInFromRight**   | 250ms                         | EaseOutQuart                       | Navegación hacia adelante en la jerarquía                     |
| **SlideInFromLeft**    | 250ms                         | EaseOutQuart                       | Navegación hacia atrás en la jerarquía                        |
| **SlideInFromTop**     | 200ms                         | EaseOutQuart                       | Aparición de Flyouts, InfoBars, notificaciones                |
| **SlideInFromBottom**  | 200ms                         | EaseOutQuart                       | Aparición de ContentDialogs, rows de DataGrid                 |
| **SlideOutLeft**       | 250ms                         | EaseIn                             | Salida de pantalla al navegar hacia adelante                  |
| **SlideOutRight**      | 250ms                         | EaseIn                             | Salida de pantalla al navegar hacia atrás                     |
| **ScaleUp**            | 200ms                         | EaseOutBack                        | Énfasis de elementos, aparición de ContentDialog              |
| **ScaleDown**          | 150ms                         | EaseIn                             | Reducción y colapso de elementos                              |
| **PopIn**              | 300ms                         | Spring(stiffness:0.8, damping:0.4) | Íconos de resultado (✅/❌), badges, confirmaciones de acción |
| **Bounce**             | 400ms                         | EaseOutBounce                      | Celebraciones, confirmaciones de éxito mayores                |
| **Shake**              | 400ms                         | Linear                             | Error de validación de credenciales, acción rechazada         |
| **Pulse**              | 2000ms                        | EaseInOut (loop infinito)          | Estados de error en badges, indicadores de atención           |
| **Breathing**          | 2000ms                        | EaseInOut (loop infinito)          | Filas del DataGrid con acciones pendientes para el rol        |
| **Gradiente**          | 2000ms                        | Linear (loop infinito)             | Fondos de StateBadge en estados activos (`EN_*`)              |
| **CountUp**            | 800ms                         | EaseOutQuart                       | Animación de valores numéricos en KPI cards                   |
| **NumberFlip**         | 300ms                         | EaseOutQuart                       | Cambio de contadores en Badges                                |
| **StrokeReveal**       | 600ms                         | EaseOut                            | Dibujo progresivo de la línea de tiempo del Timeline          |
| **StaggeredReveal**    | 50ms por elemento, máx. 400ms | EaseOut (por elemento)             | Carga progresiva de listas, tarjetas, secciones               |
| **CrossFade**          | 200ms                         | EaseInOut                          | Cambio de contenido en el mismo espacio (estado, modo)        |
| **HighlightFlash**     | 100ms entrada + 600ms salida  | Instantáneo + EaseOut              | Actualización en tiempo real de una celda o tarjeta           |
| **ConnectedAnimation** | 400ms                         | EaseOutQuint                       | Transición de elemento compartido entre dos pantallas         |

### 19.3 Microinteracciones por Componente

**Button:**

- _Hover_: Fondo oscurece 10%, escala 1.0→1.01 en 100ms (_EaseOut_). _RevealFocusVisual_ de borde luminoso de 1px.
- _Pressed_: Escala 1.0→0.98 en 50ms. Fondo oscurece 20%.
- _Released_: Escala retorna a 1.0 en 100ms (_EaseOutBack_ suave).
- _Disabled_: Opacidad 40%. Sin interacciones de hover.
- _Focus (teclado)_: _RevealFocusVisual_ de 2px en color de acento, instantáneo.
- _Loading_: Texto hace _FadeOut_ (100ms), _ProgressRing_ hace _FadeIn + ScaleFrom(0.8)_ (150ms).

**TextBox / PasswordBox:**

- _Focus_: Borde inferior transiciona de 1px neutro a 2px color de acento (200ms _EaseOut_). _RevealFocusVisual_ de 2px.
- _Typing_: Sin animación de fondo — solo el caret parpadeante estándar de Windows.
- _Validation Error (OnLostFocus)_: Borde cambia a `SystemFillColorCritical` (rojo) en 150ms. Shake sutil del campo (±4px, 3 oscilaciones, 300ms) si el error es de formato crítico.
- _Validation Success_: Borde destella color verde brevemente (200ms) antes de normalizar.

**DataGrid Row:**

- _Hover_: Fondo tintado aparece con _FadeIn_ (100ms). _RevealHighlight_ sigue el cursor dentro de la fila.
- _Hover entre filas adyacentes_: La transición del _RevealHighlight_ entre filas es continua y fluida (sin salto).
- _Click (selección)_: Escala de presión suave (0.995, 50ms). CheckBox se marca con animación de escala. _SplitView_ de preview emerge.
- _Real-time update_: _HighlightFlash_ del color de la nueva fase de estado.

**CardPanel / Tarjeta KPI:**

- _Hover_: Elevación aumenta (Shadow Depth 4→8, 150ms). Escala 1.0→1.02 (150ms, _EaseOut_). _RevealHighlight_ interior.
- _Pressed_: Escala 1.0→0.98 (50ms). Shadow Depth aumenta visualmente.

**NavigationView Item:**

- _Hover_: Fondo tintado aparece con _FadeIn_ (100ms).
- _Selected_: Indicador de barra vertical (3px, color de acento) desliza desde el ítem anterior con animación de resorte (_Spring_) suave.
- _Badge actualiza_: _PopIn_ o _NumberFlip_ según si es nuevo o incrementa.

**Expander:**

- _Expand_: Animación de altura de colapsado a expandido con curva _EaseInOutQuart_ (300ms). Contenido interno ejecuta _StaggeredFadeIn_.
- _Collapse_: Inverso, con curva _EaseInOutQuart_ (250ms). El contenido hace _FadeOut_ antes de que el collapse de altura comience.

**ContentDialog:**

- _Aparición_: Fondo _Smoke_ llega a su opacidad final en 300ms (_EaseOut_). El diálogo hace _ScaleFrom(0.85) + FadeIn_ en 250ms (_EaseOutBack_).
- _Cierre (confirmación)_: Inverso — diálogo hace _ScaleDown + FadeOut_ (200ms), _Smoke_ se disipa (250ms).
- _Cierre (cancelación)_: Igual al cierre de confirmación pero sin la consecuencia funcional.

### 19.4 Transiciones de Pantalla

**Navegación hacia adelante (profundidad de jerarquía):**

- Pantalla actual: _SlideOutLeft + FadeOut_ (250ms, _EaseIn_).
- Pantalla nueva: _SlideInFromRight + FadeIn_ (250ms, _EaseOutQuart_, retraso de 50ms).
- Elementos individuales de la pantalla nueva: _StaggeredReveal_ (50ms de desfase entre elementos, máximo total de 400ms).

**Navegación hacia atrás:**

- Pantalla actual: _SlideOutRight + FadeOut_ (250ms, _EaseIn_).
- Pantalla anterior: _SlideInFromLeft + FadeIn_ (250ms, _EaseOutQuart_).

**Connected Animation (entre pantallas relacionadas):**

- Elemento origen en la pantalla A: Permanece visible y se convierte en el punto de partida de la animación.
- Transición: El elemento (ej: una fila del DataGrid, una tarjeta KPI) interpola simultáneamente posición, escala y opacidad desde su origen hasta su destino en la pantalla B (400ms, _EaseOutQuint_).
- Elementos restantes de la pantalla B: _FadeIn_ con retraso hasta que la _ConnectedAnimation_ alcanza el 50% de su duración, creando la sensación de que el elemento conectado "abre" la nueva pantalla.

---

## Apéndice A: Sistema de Colores por Fase (StateBadge)

| Fase del Proceso                | Color Principal | Código Hex | Dark Mode Hex | Estados Incluidos                                                      |
| ------------------------------- | --------------- | ---------- | ------------- | ---------------------------------------------------------------------- |
| Recepción / Validación Inicial  | Azul Windows    | `#0078D4`  | `#60CDFF`     | RECEPCIONADO, EN_REVISION, VALIDADO                                    |
| Autorización CAA                | Amarillo Ámbar  | `#F7A600`  | `#FFD52E`     | EN_AUTORIZACION_CAA, AUTORIZADO_CAA, RECHAZADO_CAA_REINTENTO           |
| Cotización / Estudio de Mercado | Cyan            | `#00B7C3`  | `#00E5EB`     | EN_COTIZACION, COTIZACION_COMPLETA, SIN_COTIZACIONES                   |
| Pedido / Orden de Compra        | Naranja Cálido  | `#E74F2D`  | `#FF8C78`     | CUADRO_COMPARATIVO, PROVEEDOR_SELECCIONADO, PEDIDO_GENERADO            |
| Entrega de Bienes               | Verde Claro     | `#6DC253`  | `#92D06A`     | EN_ENTREGA, ENTREGADO                                                  |
| Recepción de Bienes             | Púrpura Oscuro  | `#5C2D91`  | `#9B7FD6`     | EN_RECEPCION_BIENES, RECEPCIONADO_BIENES, CON_DISCREPANCIA             |
| Validación Fiscal (CFDI)        | Púrpura Medio   | `#9B4F9E`  | `#C57FC8`     | EN_VALIDACION_CFDI, CFDI_VALIDO, CFDI_INVALIDO, ERROR_SAT_REINTENTANDO |
| Pago                            | Verde Oscuro    | `#107C10`  | `#54B454`     | EN_PAGO, PAGADO, ERROR_PAGO                                            |
| Cierre del Expediente           | Gris Azulado    | `#69797E`  | `#9AA8AC`     | EN_CIERRE, CERRADO                                                     |
| Rechazado / Cancelado           | Rojo Crítico    | `#C50F1F`  | `#FF6B6B`     | RECHAZADO\_\*, CFDI_INVALIDO, CANCELADO                                |
| Alerta / Advertencia            | Naranja Oscuro  | `#BC4B00`  | `#E87B2A`     | EN_FRACCIONAMIENTO, ERROR_SAT_REINTENTANDO, CON_DISCREPANCIA, bloqueos |

---

## Apéndice B: Matriz de Visibilidad de Módulos por Rol

| Módulo                    | Admin                             | Comprador DSA                   | CAA                          | Finanzas                      | Almacén                        | Consulta DSA             |
| ------------------------- | --------------------------------- | ------------------------------- | ---------------------------- | ----------------------------- | ------------------------------ | ------------------------ |
| Pantalla de Autenticación | ✅                                | ✅                              | ✅                           | ✅                            | ✅                             | ✅                       |
| Shell Principal           | ✅ Completo                       | ✅ Filtrado por rol             | ✅ Filtrado                  | ✅ Filtrado                   | ✅ Filtrado                    | ✅ Solo lectura          |
| Dashboard KPI             | ✅ Global completo                | ✅ Sus solicitudes              | ❌ No visible                | ❌ No visible                 | ❌ No visible                  | ✅ Global solo lectura   |
| Lista de Solicitudes      | ✅ Todas                          | ✅ Propias                      | ✅ Solo estado CAA           | ✅ Solo estado pago/CFDI      | ✅ Solo estado entrega         | ✅ Todas, solo lectura   |
| Nueva Solicitud           | ✅                                | ✅                              | ❌                           | ❌                            | ❌                             | ❌                       |
| Detalle de Solicitud      | ✅ + edición + todas las acciones | ✅ + edición propias            | ✅ + Autorizar/Rechazar      | ✅ + CFDI/Pago                | ✅ + Confirmar entrega         | ✅ Solo lectura          |
| Timeline de Hitos         | ✅ + registro de cualquier hito   | ✅ + registro hitos de su fase  | ✅ + registro hito CAA       | ✅ + registro hitos CFDI/Pago | ✅ + registro hitos de entrega | ✅ Solo lectura          |
| Proveedores               | ✅ + crear/editar                 | ✅ + crear                      | ❌                           | ❌                            | ❌                             | ❌                       |
| Cotizaciones              | ✅ + crear/editar/seleccionar     | ✅ + crear/editar/seleccionar   | ❌                           | ✅ Solo lectura               | ❌                             | ✅ Solo lectura          |
| Validación CFDI           | ✅ Completo                       | ❌                              | ❌                           | ✅ Completo                   | ❌                             | ❌                       |
| Reportes                  | ✅ Todos                          | ✅ Propios + cuadro comparativo | ✅ Expedientes en estado CAA | ✅ Reportes financieros       | ❌                             | ✅ Todos en solo lectura |
| Notificaciones            | ✅ Todas                          | ✅ Scope de rol                 | ✅ Scope de rol              | ✅ Scope de rol               | ✅ Scope de rol                | ✅ Scope de rol          |
| Administración            | ✅ Completo                       | ❌                              | ❌                           | ❌                            | ❌                             | ❌                       |

---

## Apéndice C: Representación Visual de las Reglas de Negocio

| Clave RN   | Nombre de la Regla                                | Trigger Funcional                                        | Representación Visual en UI                                                                                                                            | Comportamiento al Incumplimiento                                                                                                                                                          |
| ---------- | ------------------------------------------------- | -------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------ | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **RN-001** | Monto máximo $75,000 MXN                          | Nueva Solicitud: captura de partidas                     | _MontoDisplay_ con barra de progreso coloreada (verde → amarillo → rojo).                                                                              | _InfoBar_ Warning emerge. Botón "Registrar Solicitud" se deshabilita. La barra se llena en rojo.                                                                                          |
| **RN-002** | Prohibición de fraccionamiento de compra          | Nueva Solicitud: al salir del campo "Código de Producto" | _InfoBar_ Warning de alta visibilidad con descripción del patrón detectado. Campo de justificación obligatorio. _CheckBox_ de confirmación explícita.  | Botón "Registrar Solicitud" permanece deshabilitado hasta confirmar la justificación.                                                                                                     |
| **RN-003** | Mínimo 3 cotizaciones de proveedores distintos    | Módulo de Cotizaciones: al registrar cada cotización     | _InfoBar_ persistente con contador de progreso: "N de 3 cotizaciones" (naranja) → "✅ Mínimo cumplido" (verde).                                        | El botón "Confirmar Selección de Proveedor" en el Cuadro Comparativo permanece deshabilitado hasta que se registren ≥ 3 cotizaciones.                                                     |
| **RN-004** | CFDI válido para proceder al pago                 | Módulo de Validación CFDI: al ejecutar la validación     | Panel de estado con transiciones visuales claras entre estados: Listo → Validando → Válido / Inválido / Reintentando / Circuit Breaker abierto.        | Si el CFDI es inválido: estado `CFDI_INVALIDO` con _InfoBar_ Error y descripción del error del SAT. Si el servicio SAT no responde: reintentos automáticos con barra de cuenta regresiva. |
| **RN-005** | Bloqueo optimista de edición (un editor a la vez) | Al intentar editar cualquier expediente                  | _BloqueoIndicator_ en el encabezado del Detalle de Solicitud. _ContentDialog_ informativo al intentar editar un expediente bloqueado por otro usuario. | El expediente se abre en modo de solo lectura. No se deshabilita la pantalla — solo las acciones de edición.                                                                              |

---

## Apéndice D: Catálogo de Animaciones con Especificación Completa

| Animación              | Duración                           | Curva                         | Parámetros Adicionales                                                                                  | Uso                                      |
| ---------------------- | ---------------------------------- | ----------------------------- | ------------------------------------------------------------------------------------------------------- | ---------------------------------------- |
| **FadeIn**             | 150ms                              | `EaseOut`                     | Opacidad 0→1                                                                                            | Aparición de elementos, resultados       |
| **FadeOut**            | 150ms                              | `EaseIn`                      | Opacidad 1→0                                                                                            | Desaparición, cierre                     |
| **SlideInFromRight**   | 250ms                              | `EaseOutQuart`                | TranslateX +48px→0, FadeIn simultáneo                                                                   | Navegación adelante                      |
| **SlideInFromLeft**    | 250ms                              | `EaseOutQuart`                | TranslateX -48px→0, FadeIn simultáneo                                                                   | Navegación atrás                         |
| **SlideInFromTop**     | 200ms                              | `EaseOutQuart`                | TranslateY -24px→0, FadeIn simultáneo                                                                   | Flyouts, InfoBars                        |
| **SlideInFromBottom**  | 200ms                              | `EaseOutQuart`                | TranslateY +24px→0, FadeIn simultáneo                                                                   | ContentDialogs, nuevas filas             |
| **ScaleUp**            | 200ms                              | `EaseOutBack` (overshoot 0.3) | Escala 0.85→1.0 + FadeIn                                                                                | ContentDialog, confirmaciones            |
| **PopIn**              | 300ms                              | `Spring(k=0.8, d=0.4)`        | Escala 0→1.2→1.0                                                                                        | Íconos de resultado, badges              |
| **Bounce**             | 400ms                              | `EaseOutBounce`               | Escala 0→1.3→0.9→1.05→1.0                                                                               | Éxito mayor, celebración                 |
| **Shake**              | 400ms                              | `Linear`                      | TranslateX 0→+8→-8→+8→-8→+4→-4→0                                                                        | Error de credenciales, rechazo           |
| **Pulse**              | 2000ms                             | `EaseInOut` (loop)            | Opacidad 1.0→0.65→1.0                                                                                   | Badges de error, alertas de estado       |
| **Breathing**          | 2000ms                             | `EaseInOut` (loop)            | Opacidad fondo fila 0.0→0.12→0.0                                                                        | Filas con acciones pendientes            |
| **Gradiente**          | 2000ms                             | `Linear` (loop)               | GradientOffset 0%→100%→0%                                                                               | Fondos de badges en estados `EN_*`       |
| **CountUp**            | 800ms                              | `EaseOutQuart`                | Valor 0→ValorFinal, interpolado                                                                         | Números KPI                              |
| **NumberFlip**         | 300ms                              | `EaseOutQuart`                | Número anterior: TranslateY 0→-20 + FadeOut. Número nuevo: TranslateY +20→0 + FadeIn                    | Badges, contadores                       |
| **HighlightFlash**     | 100ms + 600ms                      | Instantáneo + `EaseOut`       | Fondo = color fase (100ms instantáneo) → Fondo transparente (600ms EaseOut)                             | Actualizaciones en tiempo real de celdas |
| **StrokeReveal**       | 600ms                              | `EaseOut`                     | Recorte de trayectoria de 0% a 100%                                                                     | Línea de tiempo del Timeline             |
| **StaggeredReveal**    | 50ms por elemento, max 400ms total | `EaseOut` por elemento        | FadeIn + SlideInFromBottom en cascada                                                                   | Carga de listas, tarjetas                |
| **CrossFade**          | 200ms                              | `EaseInOut`                   | Elemento A: FadeOut (100ms). Elemento B: FadeIn (100ms, retraso 50ms). Superposición en el punto medio. | Cambio de estado en el mismo espacio     |
| **ConnectedAnimation** | 400ms                              | `EaseOutQuint`                | Posición, escala y opacidad interpolados simultáneamente del origen al destino                          | Navegación entre pantallas relacionadas  |

---

## Apéndice E: Mapeo del Stack Tecnológico a Componentes de Interfaz

Este apéndice establece la correspondencia entre las decisiones de stack tecnológico y su manifestación concreta en la experiencia de interfaz de usuario. No contiene código: describe las relaciones conceptuales y de diseño entre capas.

### E.1 Uno Platform (WinUI 3 Flavor) — Capa de Presentación

Uno Platform con el renderer WinUI 3 es la tecnología que ejecuta el renderizado de todos los componentes de interfaz descritos en este documento. Su implicación de diseño más relevante es la **fidelidad nativa**: todos los materiales Fluent Design (Mica, Acrylic, Smoke), los controles WinUI 3 (NavigationView, DataGrid, ContentDialog), y las animaciones de composición de Windows (Connected Animation, RevealHighlight, RevealFocus) se ejecutan a través del motor de composición nativo de Windows 11, garantizando la apariencia y comportamiento idénticos a los de las aplicaciones de inbox de Microsoft (Configuración, Explorador de Archivos, etc.).

El patrón **MVVM con CommunityToolkit.Mvvm** determina que cada módulo de interfaz tiene un ViewModel dedicado que:

- Expone el estado del `EstatusSolicitud` como una propiedad observable. Cuando el estado cambia (por acción del usuario o por evento SignalR), todos los componentes visuales ligados a esa propiedad (StateBadge, botones de acción, colores de fila en el DataGrid) se actualizan automáticamente sin código adicional.
- Expone _Comandos_ (`ICommand`) para cada botón de acción disponible. La lógica de `CanExecute` del Comando determina si el botón está habilitado, calculando en tiempo de ejecución si la combinación `{rol, estado_actual}` permite esa acción.
- Mantiene colecciones reactivas (`ObservableCollection`) de hitos, cotizaciones y solicitudes, de modo que las adiciones y modificaciones en tiempo real se reflejan inmediatamente en los DataGrid y ListView sin recargas manuales.

### E.2 SignalR Client — Tiempo Real en la UI

El cliente SignalR es la tecnología que habilita la **actualización automática en tiempo real** de todos los módulos sin que el usuario deba recargar manualmente. Las implicaciones de diseño son:

- El Dashboard KPI puede mostrar sus métricas actualizándose en tiempo real sin botón de recarga.
- Las filas del DataGrid de la Lista de Solicitudes cambian de color y estado en tiempo real cuando otras sesiones modifican el estado de esas solicitudes.
- El Centro de Notificaciones recibe eventos y actualiza el BadgeControl instantáneamente.
- El Timeline de Hitos actualiza el ícono del hito de ⬜ a ✅ en tiempo real cuando otro usuario registra el hito.

La política de reconexión automática con backoff exponencial ([0, 2000ms, 5000ms, 10000ms, 30000ms]) garantiza que la interfaz sea resiliente: el usuario experimenta interrupciones breves de las actualizaciones automáticas durante la reconexión, pero el sistema se recupera sin intervención manual.

### E.3 MediatR + CQRS — Impacto en la Responsividad de la UI

El patrón CQRS (Command Query Responsibility Segregation) implementado con MediatR determina que la interfaz nunca mezcla operaciones de lectura y escritura en la misma operación:

- **Comandos (Escritura):** Cuando el usuario ejecuta una acción (registrar hito, autorizar, validar CFDI), la interfaz muestra inmediatamente el estado de procesamiento (botón en modo de carga, ProgressRing) y puede continuar respondiendo a otras interacciones. No bloquea la pantalla esperando que la escritura complete.
- **Consultas (Lectura):** Los DataGrid y KPI Cards se alimentan de consultas optimizadas para lectura (implementadas con Dapper en el servidor). Estas consultas son rápidas y no interfieren con las operaciones de escritura en curso. El Dashboard puede actualizar sus métricas mientras el usuario está ejecutando una transición de estado en otra solicitud.

### E.4 Polly (Circuit Breaker) — Representación Visual en el Módulo CFDI

La política de resiliencia Polly con patrón Retry + Circuit Breaker para la integración con el SAT se manifiesta directamente en los estados visuales del **Módulo 10 — Validación Fiscal**:

| Estado de Polly                                              | Estado Visual en el Módulo                                                                                                            |
| ------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------- |
| Circuit Breaker **Cerrado** (servicio SAT disponible)        | Estado "Validando" normal con ProgressRing.                                                                                           |
| **Retry** en progreso (1 de 3 intentos)                      | Estado "Validando..." con contador de intento (N de 3) y ProgressBar de cuenta regresiva.                                             |
| **Retry** agotado, Circuit Breaker **Abierto**               | Estado "Error SAT / Reintentos agotados" con ícono de señal roja e InfoBar explicativa. Solicitud en estado `ERROR_SAT_REINTENTANDO`. |
| Circuit Breaker **Semi-Abierto** (probando recuperación)     | El sistema reintenta en segundo plano. La UI muestra el estado `ERROR_SAT_REINTENTANDO` sin acción requerida del usuario.             |
| Circuit Breaker **Cerrado** nuevamente (servicio recuperado) | Notificación automática vía SignalR al usuario responsable. La solicitud queda lista para reintentar la validación.                   |

### E.5 EF Core + Dapper — Latencia Percibida en la UI

La distinción entre EF Core (escrituras) y Dapper (lecturas) determina las expectativas de latencia que la interfaz debe comunicar al usuario:

- **Operaciones de escritura (EF Core):** Registro de hitos, creación de solicitudes, cambios de estado. Se ejecutan transaccionalmente. La interfaz muestra indicadores de carga (ProgressRing en botones) hasta que la operación confirma éxito.
- **Operaciones de lectura (Dapper):** Cuadro Comparativo, Dashboard KPI, Lista de Solicitudes con filtros complejos. Optimizadas para velocidad. La interfaz puede mostrar datos parciales mientras el resto carga (renderización progresiva), reduciendo la percepción de espera.

### E.6 SSRS — Generación de Reportes

El Motor de Reportes SSRS genera los documentos PDF y Excel en el servidor. La interfaz gestiona el estado de esta generación asíncrona:

- Al presionar "Generar Reporte", el botón entra en estado de carga y la tarjeta de reporte muestra un _ProgressBar_ indeterminado.
- El tiempo de generación varía según la complejidad del reporte y el volumen de datos. La interfaz no se bloquea durante este tiempo — el usuario puede navegar a otros módulos.
- Al completarse, el TeachingTip emerge desde el botón con las opciones de descarga. El archivo se entrega al cliente para guardarse en el equipo (con la carpeta del servidor SMB preseleccionada como destino sugerido usando la capacidad `broadFileSystemAccess` del manifiesto).

### E.7 SMB 3.0 — Gestión Documental en la UI

El acceso al servidor de archivos SMB del hospital (ruta `\\Servidor\Compras_FR\`) se manifiesta en la interfaz en dos contextos principales:

- **Adjuntar documentos:** Los selectores de archivo nativos de Windows (al adjuntar PDF de cotización, XML de CFDI, documentos de hitos) navegan al servidor SMB como destino sugerido, gracias a la capacidad `broadFileSystemAccess` declarada en el manifiesto.
- **Abrir documentos:** Los vínculos a documentos adjuntos en el Detalle de Solicitud y en el Timeline de Hitos abren el archivo directamente desde la ruta SMB usando la aplicación predeterminada de Windows para ese tipo de archivo (Adobe Acrobat para PDF, etc.).

---

## Apéndice F: Glosario de Términos Técnicos y de Diseño

| Término                | Definición                                                                                                                                                                                                     |
| ---------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Solicitud**          | Expediente de adquisición creado por un Comprador DSA que pasa por las 8 fases del proceso de Fondo Revolvente. Unidad fundamental del sistema.                                                                |
| **Folio DSA**          | Identificador único alfanumérico de cada solicitud. Formato: `DSA-AAAA-NNN` (ej: DSA-2026-089). Generado automáticamente por el sistema al crear la solicitud.                                                 |
| **Hito**               | Punto de control dentro del proceso de adquisición. Representa un evento discreto con timestamp, usuario y observaciones. Existen 27 hitos distribuidos en 8 fases.                                            |
| **Fase**               | Agrupación principal del proceso de adquisición. Existen 8 fases secuenciales. Cada fase contiene entre 2 y 5 hitos.                                                                                           |
| **Estado**             | Condición actual de una solicitud en su ciclo de vida. Existen 30 estados posibles. Cada estado pertenece a una fase y determina qué acciones están disponibles para cada rol.                                 |
| **Transición**         | Cambio de un estado a otro dentro del ciclo de vida. Existen 38 transiciones permitidas, gobernadas por el `SolicitudStateMachine`.                                                                            |
| **CFDI**               | Comprobante Fiscal Digital por Internet (versión 4.0). Documento fiscal electrónico emitido por el proveedor. Requerido para autorizar el pago de adquisiciones.                                               |
| **CAA**                | Comité de Adquisiciones y Arrendamientos. Órgano colegiado del HCG que autoriza solicitudes que cumplen los criterios de Fondo Revolvente.                                                                     |
| **RN-001**             | Regla de Negocio 1: El monto total de una solicitud no puede exceder $75,000 MXN (conforme Ley de Compras del Estado de Jalisco).                                                                              |
| **RN-002**             | Regla de Negocio 2: Prohibición de fraccionamiento de adquisiciones (dividir una compra mayor en múltiples solicitudes para evadir el límite).                                                                 |
| **RN-003**             | Regla de Negocio 3: Se requieren mínimo 3 cotizaciones de proveedores distintos antes de seleccionar un proveedor.                                                                                             |
| **RN-004**             | Regla de Negocio 4: El CFDI del proveedor debe ser validado como auténtico ante el SAT antes de proceder al pago.                                                                                              |
| **RN-005**             | Regla de Negocio 5: Bloqueo optimista de edición — solo un usuario puede editar un expediente al mismo tiempo. El bloqueo expira tras 30 minutos de inactividad.                                               |
| **Mica**               | Material de fondo de Windows 11 que crea translucidez adaptada al color de acento y fondo de escritorio del usuario.                                                                                           |
| **Acrylic**            | Material de fondo translúcido con desenfoque gaussiano del contenido que hay detrás. Usado en elementos superpuestos (Flyouts, paneles).                                                                       |
| **Smoke**              | Material de oscurecimiento semitransparente aplicado al fondo cuando un ContentDialog modal está activo. Comunica que el foco debe estar en el diálogo.                                                        |
| **RevealHighlight**    | Efecto visual de luz que sigue el cursor dentro de un elemento interactivo, simulando una fuente de luz en el puntero.                                                                                         |
| **RevealFocusVisual**  | Indicador visual de foco de teclado: borde luminoso de 2px en el color de acento del sistema alrededor del elemento enfocado.                                                                                  |
| **ConnectedAnimation** | Animación que mantiene continuidad visual entre dos pantallas durante la navegación: un elemento "viaja" de su posición en la pantalla origen a su posición en la pantalla destino.                            |
| **StateBadge**         | Componente visual reutilizable que representa el estado de una solicitud con color de fase, ícono y texto. Disponible en 3 tamaños (Small, Medium, Large).                                                     |
| **BloqueoIndicator**   | Componente visual que indica que un expediente tiene un bloqueo de edición activo, mostrando quién lo tiene y desde qué hora.                                                                                  |
| **MontoDisplay**       | Componente de visualización monetaria normalizada que formatea valores como `$XX,XXX.XX MXN` e incluye una barra de progreso respecto al límite de $75,000 MXN.                                                |
| **MVVM**               | Model-View-ViewModel: patrón de arquitectura de UI que separa la lógica de presentación (ViewModel) de la vista (controles de interfaz), habilitando la reactividad automática de la UI ante cambios de datos. |
| **CQRS**               | Command Query Responsibility Segregation: patrón que separa las operaciones de escritura (Comandos) de las de lectura (Consultas), optimizando cada tipo de manera independiente.                              |
| **SignalR**            | Tecnología de Microsoft para comunicación en tiempo real entre servidor y clientes, usando WebSockets. Habilita las actualizaciones automáticas de la UI sin recarga manual.                                   |
| **Circuit Breaker**    | Patrón de resiliencia que "abre" (deja de intentar) las llamadas a un servicio externo cuando detecta múltiples fallos consecutivos, protegiendo al sistema de cascadas de errores.                            |
| **SMB**                | Server Message Block: protocolo de red para compartir archivos en una red local. El hospital usa SMB 3.0 para el repositorio de expedientes PDF en `\\Servidor\Compras_FR\`.                                   |
| **SSRS**               | SQL Server Reporting Services: motor de reporteo de Microsoft que genera documentos PDF y Excel a partir de plantillas y datos de SQL Server.                                                                  |
| **SAT**                | Servicio de Administración Tributaria: autoridad fiscal de México. La aplicación consulta al SAT para validar la autenticidad de los CFDIs emitidos por los proveedores.                                       |

---

**Fin del Documento de Especificación Funcional y de Interfaz Nativa — Edición Enterprise v3.0**

_Documento unificado y extendido a partir de ARCH-FR-2026-001 v3.0, FR-UX-SPEC v1.0 y FR-UX-SPEC v2.0_

_Lead Windows UI/UX Architect & Product Manager Funcional — Hospital Civil de Guadalajara · Enero 2026_

_Stack de Presentación: Uno Platform (WinUI 3 Flavor) · MVVM · CommunityToolkit.Mvvm · SignalR Client_

_Sistema de Diseño: Microsoft Fluent Design System v2 · WinUI 3 · Windows Community Toolkit_

_Referencia Arquitectónica: Clean Architecture · CQRS (MediatR) · Domain-Driven Design · State Machine Pattern_
