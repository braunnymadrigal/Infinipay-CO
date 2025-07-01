# Plan de ejecución XML: Mostrar los resultados de la planilla

- Se ejecuta planilla con una empresa de 1002 empleados
- Se ejecutan las planillas de enero 2025 hasta junio 2025 de todos los empleados
- Tablas principales involucradas:
    - Planilla
    - EmpleadoDePlanilla
    - DetallePago
    - DeduccionAPago
    - Deduccion
    - Empleador

## Ejecución sin índices

| Operación                   | Costo Total Estimado | IO Estimado | CPU Estimado | Filas Estimadas | Tablas / Uso                                      | Comentario                                                                                       |
| --------------------------- | -------------------- | ----------- | ------------ | --------------- | ------------------------------------------------- | ------------------------------------------------------------------------------------------------ |
| Nested Loops (Node 5)       | 0.0431688            | 0           | 0.0251051    | 6006            | Join `Planilla` (p) y `EmpleadoDePlanilla` (ep)   | Join interno anidado, estimación para \~6000 filas, eficiente si los índices son adecuados.          |
| Nested Loops (Node 6)       | 0.00739016           | 0           | 0.00002508   | 6               | Join `Planilla` (p) con `Empleador` (em)          | Join anidado pequeño, estimación de 6 filas, uso de índices para filtrado específico.            |
| Clustered Index Scan (7)    | 0.0032886            | 0.003125    | 0.0001636    | 6               | Tabla `Planilla`                                  | Escaneo del índice cluster para obtener planillas; bajo costo debido a pocas filas.              |
| Index Seek (8)              | 0.0040736            | 0.003125    | 0.0001581    | 1               | Tabla `Empleador` con filtro en `idPersonaFisica` | Busca específica en índice no cluster; eficiente para filtro por clave única.                    |
| Clustered Index Seek (9)    | 0.0106736            | 0.003125    | 0.0012581    | 1001            | Tabla `EmpleadoDePlanilla`                        | Seek en índice cluster por `idPlanilla` para obtener empleados relacionados; buena eficiencia.   |
| Sort (10)                   | 0.261831             | 0.0112613   | 0.161421     | 6006            | Tabla `DetallePago`                               | Ordena por `idPlanilla` y `idEmpleado`; mayor costo de CPU y IO debido a la cantidad de filas.   |
| Clustered Index Scan (11)   | 0.0891479            | 0.0823843   | 0.0067636    | 6006            | Tabla `DetallePago`                               | Escaneo completo del índice cluster; puede ser costoso si la tabla crece mucho.                  |
| Table Spool Lazy (12)       | 1.91092              | 0.01        | 0.000102991  | 14.95           | Tablas `DeduccionAPago` y `Deduccion`             | Almacenamiento temporal, mejora en operaciones repetidas.                 |
| Nested Loops Left Join (13) | 1.28416              | 0           | 0.00006248   | 14.95           | Join `DeduccionAPago` (dap) con `Deduccion` (d)   | Join externo izquierdo para obtener nombre de deducciones, enlaza tablas de deducciones.         |
| Clustered Index Scan (14)   | 1.23646              | 1.13728     | 0.0991775    | 14.95           | Tabla `DeduccionAPago`                            | Escaneo para obtener todas las deducciones aplicadas a pagos, tabla bastante grande (90k filas). |
| Clustered Index Scan (15)   | 0.00439334           | 0.0032035   | 0.0000796    | 1               | Tabla `Deduccion`                                 | Escaneo en tabla de deducciones (muy pequeña), rápido.                                           |

## Índices utilizados para optimizar la consulta

```CREATE NONCLUSTERED INDEX IX_DeduccionAPago_idDetallePago ON dbo.DeduccionAPago (idDetallePago);```

```CREATE NONCLUSTERED INDEX IX_DetallePago_Join ON dbo.DetallePago (idPlanilla, idEmpleado);```

```CREATE NONCLUSTERED INDEX IX_EmpleadoDePlanilla_Join ON dbo.EmpleadoDePlanilla (idPlanilla, idEmpleado);```

```CREATE NONCLUSTERED INDEX IX_Planilla_idPersonaJuridica ON dbo.Planilla (idPersonaJuridica);```

| Operación                 | Costo Total Estimado | IO Estimado | CPU Estimado | Filas Estimadas | Dónde se usa / Contexto                                                   | Comentario                                        |
| ------------------------- | -------------------- | ----------- | ------------ | --------------- | ------------------------------------------------------------------------- | ------------------------------------------------- |
| Merge Join (NodeId=4)     | 0.33                 | 0           | 0.0314       | 6,006           | Join principal entre `Planilla`, `DetallePago` y `EmpleadoDePlanilla`     | Operación principal del join                      |
| Nested Loops (NodeId=5)   | 0.043                | 0           | 0.0251       | 6,006           | Join interno entre `Planilla` y `EmpleadoDePlanilla`                      | Join eficiente con índices                        |
| Nested Loops (NodeId=6)   | 0.0074               | 0           | 0.000025     | 6               | Join entre `Planilla` y `Empleador` (filtrado por `idPersonaJuridica`)    | Join pequeño con pocas filas                      |
| Clustered Index Scan (7)  | 0.0033               | 0.0031      | 0.00016      | 6               | Scan de tabla `Planilla` con filtro reducido                              | Tabla pequeña, bajo costo                         |
| Index Seek (8)            | 0.0041               | 0.0031      | 0.00016      | 1               | Seek en índice no clusterizado de tabla `Empleador`                       | Búsqueda específica por GUID                      |
| Index Seek (9)            | 0.0107               | 0.0031      | 0.0013       | 1,001           | Seek con filtro en índice `IX_EmpleadoDePlanilla_Join`                    | Búsqueda por `idPlanilla` en `EmpleadoDePlanilla` |
| Sort (10)                 | 0.257                | 0.0113      | 0.1614       | 6,006           | Ordenamiento sobre `DetallePago` por columnas `idPlanilla` e `idEmpleado` | Costoso en CPU, posible candidato a optimización  |
| Clustered Index Scan (11) | 0.085                | 0.078       | 0.0068       | 6,006           | Scan completo en tabla `DetallePago`                                      | Scan de tabla mediana                             |
| Table Spool (12)          | 0.67                 | 0.01        | 0.0001       | 15              | Reuso temporal de resultados en join entre `DeduccionAPago` y `Deduccion` | Optimiza accesos repetidos                        |
| Nested Loops (13)         | 0.0402               | 0           | 0.00006      | 15              | Join Left Outer entre `DeduccionAPago` y `Deduccion`                      | Join de deducciones                               |
| Index Seek (15)           | 0.0033               | 0.0031      | 0.00017      | 15              | Seek en índice no clusterizado `IX_DeduccionAPago_idDetallePago`          | Busca deducciones por `idDetallePago`             |
| Clustered Index Seek (17) | 0.0324               | 0.0031      | 0.00016      | 1               | Seek en índice clusterizado `PK_DeduccionAPago`                           | Acceso específico a `DeduccionAPago`              |
| Clustered Index Scan (18) | 0.0044               | 0.0032      | 0.00008      | 1               | Scan en tabla `Deduccion`                                                 | Tabla pequeña, bajo costo                         |

## Comparación

| Aspecto                        | Sin índices                                                          | Con índices optimizados                                                            |
| ------------------------------ | -------------------------------------------------------------------- | ---------------------------------------------------------------------------------- |
| **Operación principal**        | Nested Loops y Clustered Index Scans                                 | Merge Join principal, Nested Loops y Index Seeks eficientes                        |
| **Costo total estimado mayor** | Table Spool (\~1.91), Sort (\~0.26), Clustered Index Scan            | Table Spool (\~0.67), Sort (\~0.25), Clustered Index Scan (\~0.085)                |
| **Uso de IO**                  | Escaneos completos en tablas grandes (DetallePago, DeduccionAPago)   | Seek en índices no clusterizados para filtros (EmpleadoDePlanilla, DeduccionAPago) |
| **CPU estimado**               | Mayor en Sort y Scans, especialmente en DeduccionAPago               | Menor CPU en escaneos gracias a índices; Sort aún costoso                          |
| **Filas estimadas**            | Similar en ambos casos (\~6,000 filas principales, \~15 deducciones) | Similar, filas no cambian, pero acceso más eficiente                               |
| **Join entre tablas grandes**  | Nested Loops con scans y spools, menos eficiente                     | Merge Join y Nested Loops con Index Seek, más eficiente                            |
| **Operaciones costosas**       | Escaneo completo de tablas grandes y Table Spool costoso             | Table Spool optimizado, menor costo general                                        |

## Conclusión

| Sin índices                            | Con índices                                |
| -------------------------------------- | ------------------------------------------ |
| Más escaneos completos, más consumo IO | Más búsquedas específicas (seek), menos IO |
| Mayor costo total estimado             | Menor costo total estimado                 |
| Operaciones de join menos eficientes   | Merge join y nested loops optimizados      |
| Mayor uso CPU y recursos               | Menor uso CPU y mejor rendimiento          |

