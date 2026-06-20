Feature: Registro y Validación de Precios de Combustible

    Background: Iniciar sesión en el sistema
        Given el usuario ingresa al ambiente "https://sigesoas.mimp-qa.sigesonline.com/#/public"
        When el usuario inicia sesión con usuario "ADMIN-GLOBAL" y contraseña "Admin2023Global*"


    @PreciosCombustible @Registro
    Scenario Outline: <ID_Caso> - Registro de nuevo precio con comprobacion en planta
        
        # Preparación: se registra un precio base para que el caso sea independiente
        When Se ingresa al módulo "Combustible" y submódulo "Precio Combustibles"
        And Se selecciona el boton Nuevo
        And Se selecciona el contrato "<Contrato>" y concepto "<Concepto>" en precios
        And Se ingresa el valor "<ValorBase>"
        And Se selecciona la fecha de vigencia del dia "<DiaBase>" dentro de "<AnosBase>" anos
        And Se ingresa el precio final en planta "<PrecioFinalBase>" y precio anterior "<PrecioAnteriorBase>"
        And Se hace clic en el boton COMPROBAR PRECIO
        And Se intenta adjuntar el archivo "<ArchivoBase>"
        Then Se verifica que el resultado del guardado de precio sea "EXITO_PRECIO"

        # Caso evaluado
        When Se ingresa al módulo "Combustible" y submódulo "Precio Combustibles"
        And Se selecciona el boton Nuevo
        And Se selecciona el contrato "<Contrato>" y concepto "<Concepto>" en precios
        And Se ingresa el valor "<Valor>"
        And Se selecciona la fecha de vigencia del dia "<DiaVigencia>" dentro de "<AnosVigencia>" anos
        And Se ingresa el precio final en planta "<PrecioFinal>" y precio anterior "<PrecioAnterior>"
        And Se hace clic en el boton COMPROBAR PRECIO
        And Se intenta adjuntar el archivo "<Archivo>"
        Then Se verifica que el resultado del guardado de precio sea "<ResultadoEsperado>"

    Examples:
        | ID_Caso      | Contrato               | Concepto   | ValorBase | DiaBase | AnosBase | PrecioFinalBase | PrecioAnteriorBase | ArchivoBase                                  | Valor | DiaVigencia | AnosVigencia | PrecioFinal | PrecioAnterior | Archivo                                      | ResultadoEsperado    |
        | CP-COMB-09   | CTR26002 \| G-95 - GLP | G-95 - GLP | 18.10     | 10      | 1        | 18.30           | 18.00              | C:\Users\MANUEL\Pictures\goleto adidas.jpg | 20.32 | 22          | 1            | 20.45       | 20.10          | C:\Users\MANUEL\Pictures\goleto adidas.jpg | EXITO_PRECIO         |
        | CP-COMB-22   | CTR26002 \| G-95 - GLP | G-95 - GLP | 19.10     | 11      | 1        | 19.30           | 18.00              | C:\Users\MANUEL\Pictures\goleto adidas.jpg | 21.00 | 11          | 1            | 21.50       | 20.10          | C:\Users\MANUEL\Pictures\goleto adidas.jpg | ERROR_MISMA_FECHA    |
        | CP-COMB-23   | CTR26002 \| G-95 - GLP | G-95 - GLP | 19.20     | 12      | 1        | 19.40           | 18.00              | C:\Users\MANUEL\Pictures\goleto adidas.jpg | 21.00 | 1           | 5            | 21.50       | 20.10          | C:\Users\MANUEL\Pictures\goleto adidas.jpg | ERROR_FECHA_FUERA    |
        | CP-COMB-26   | CTR26002 \| G-95 - GLP | G-95 - GLP | 19.30     | 13      | 1        | 19.50           | 18.00              | C:\Users\MANUEL\Pictures\goleto adidas.jpg | 0.00  | 14          | 1            | 0.00        | 20.10          | C:\Users\MANUEL\Pictures\goleto adidas.jpg | ERROR_VALOR_CERO     |
        | CP-COMB-27   | CTR26002 \| G-95 - GLP | G-95 - GLP | 19.40     | 15      | 1        | 19.60           | 18.00              | C:\Users\MANUEL\Pictures\goleto adidas.jpg | 16.80 | 16          | 1            | 17.00       | 20.10          | SIN_ARCHIVO                                  | ERROR_SIN_ADJUNTO    |
        | CP-COMB-28   | CTR26002 \| G-95 - GLP | G-95 - GLP | 19.50     | 17      | 1        | 19.70           | 18.00              | C:\Users\MANUEL\Pictures\goleto adidas.jpg | 14.50 | 1           | -1           | 14.80       | 20.10          | C:\Users\MANUEL\Pictures\goleto adidas.jpg | ERROR_FECHA_ANTERIOR |
        | CP-COMB-29   | CTR26002 \| G-95 - GLP | G-95 - GLP | 19.60     | 18      | 1        | 19.80           | 18.00              | C:\Users\MANUEL\Pictures\goleto adidas.jpg | 17.20 | 19          | 1            | 17.50       | 20.10          | C:\Users\MANUEL\Pictures\goleto adidas.jpg | EXITO_NUEVO_PRECIO   |
        | CP-PREC-DUPL | CTR26002 \| G-95 - GLP | G-95 - GLP | 20.32     | 20      | 1        | 20.45           | 20.10              | C:\Users\MANUEL\Pictures\goleto adidas.jpg | 20.32 | 20          | 1            | 20.45       | 20.10          | C:\Users\MANUEL\Pictures\goleto adidas.jpg | ERROR_DUPLICADO      |


    @PreciosCombustible @Edicion
    Scenario Outline: <ID_Caso> - Edicion de precio de combustible existente

        # Preparación: se crea un precio base para generar historial
        When Se ingresa al módulo "Combustible" y submódulo "Precio Combustibles"
        And Se selecciona el boton Nuevo
        And Se selecciona el contrato "<Contrato>" y concepto "<Concepto>" en precios
        And Se ingresa el valor "<ValorDesfasado>"
        And Se selecciona la fecha de vigencia del dia "<DiaDesfasado>" dentro de "<AnosDesfasado>" anos
        And Se ingresa el precio final en planta "<PrecioFinalDesfasado>" y precio anterior "<PrecioAnterior>"
        And Se hace clic en el boton COMPROBAR PRECIO
        And Se intenta adjuntar el archivo "<Archivo>"
        Then Se verifica que el resultado del guardado de precio sea "EXITO_PRECIO"

        # Preparación: se crea un precio actual para editar
        When Se ingresa al módulo "Combustible" y submódulo "Precio Combustibles"
        And Se selecciona el boton Nuevo
        And Se selecciona el contrato "<Contrato>" y concepto "<Concepto>" en precios
        And Se ingresa el valor "<ValorActual>"
        And Se selecciona la fecha de vigencia del dia "<DiaActual>" dentro de "<AnosActual>" anos
        And Se ingresa el precio final en planta "<PrecioFinalActual>" y precio anterior "<PrecioAnterior>"
        And Se hace clic en el boton COMPROBAR PRECIO
        And Se intenta adjuntar el archivo "<Archivo>"
        Then Se verifica que el resultado del guardado de precio sea "EXITO_PRECIO"

        # Caso evaluado
        When Se ingresa al módulo "Combustible" y submódulo "Precio Combustibles"
        And Se busca el contrato "<Contrato>" en la grilla principal
        And Se hace clic en editar el precio con estado "<EstadoInicial>"
        And Se ingresa el valor "<NuevoValor>"
        And Se selecciona la fecha de vigencia del dia "<NuevoDia>" dentro de "<NuevosAnos>" anos
        And Se intenta adjuntar el archivo "<Archivo>"
        And Se hace clic en el boton Guardar Edicion
        Then Se verifica que el resultado de la edicion sea "<ResultadoEsperado>"

    Examples:
        | ID_Caso    | Contrato               | Concepto   | ValorDesfasado | DiaDesfasado | AnosDesfasado | PrecioFinalDesfasado | PrecioAnterior | ValorActual | DiaActual | AnosActual | PrecioFinalActual | EstadoInicial | NuevoValor | NuevoDia | NuevosAnos | Archivo                                      | ResultadoEsperado |
        | CP-COMB-11 | CTR26002 \| G-95 - GLP | G-95 - GLP | 18.70          | 21           | 1             | 18.90                | 18.00          | 19.70       | 22        | 1          | 19.90             | ACTUAL        | 21.50      | 23       | 1          | C:\Users\MANUEL\Pictures\goleto adidas.jpg | EXITO_EDICION     |
        | CP-COMB-24 | CTR26002 \| G-95 - GLP | G-95 - GLP | 18.80          | 24           | 1             | 19.00                | 18.00          | 19.80       | 25        | 1          | 20.00             | ACTUAL        | -1.00      | 26       | 1          | C:\Users\MANUEL\Pictures\goleto adidas.jpg | ERROR_NEGATIVO    |
        | CP-COMB-25 | CTR26002 \| G-95 - GLP | G-95 - GLP | 18.90          | 27           | 1             | 19.10                | 18.00          | 19.90       | 28        | 1          | 20.10             | DESFASADO     | 21.50      | 28       | 1          | C:\Users\MANUEL\Pictures\goleto adidas.jpg | ERROR_DESFASADO   |