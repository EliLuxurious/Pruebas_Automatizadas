using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace FLOTA_VEHICULAR.Pages.Combustible
{
    public class ControlConsumoCombustiblesPage
    {
        private IWebDriver driver;
        private VerAbastecimientosPage abastecimientosPage;

        public ControlConsumoCombustiblesPage(IWebDriver driver)
        {
            this.driver = driver;
            abastecimientosPage = new VerAbastecimientosPage(driver);
        }

        WebDriverWait Wait(int seconds = 20)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        }

        public void FiltrarFechas(string anoDesde)
        {
            var wait = Wait(15);
            Console.WriteLine($"⏳ Configurando fecha DESDE el año {anoDesde} interactuando con el calendario...");

            // Limpiamos pantalla
            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(1000);

            // ==========================
            // FECHA DESDE (Usando lógica de VerAbastecimientosPage)
            // ==========================
            // 1. Clic en el primer ícono de calendario de la pantalla (DESDE)
            By locIconoDesde = By.XPath("(//mat-datepicker-toggle//button)[1]");
            IWebElement btnCalDesde = wait.Until(ExpectedConditions.ElementToBeClickable(locIconoDesde));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCalDesde);
            Thread.Sleep(500);
            try { btnCalDesde.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCalDesde); }
            Thread.Sleep(1500);

            // 2. Clic en el botón superior (Ej: "MAR 2026") para cambiar a la vista de AÑOS
            By btnPeriodo = By.XPath("//button[contains(@class, 'mat-calendar-period-button')]");
            IWebElement btnPer = wait.Until(ExpectedConditions.ElementToBeClickable(btnPeriodo));
            try { btnPer.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnPer); }
            Thread.Sleep(1000);

            // 3. Retrocedemos en la vista de años hasta encontrar el año que queremos (2021)
            By btnPrev = By.XPath("//button[contains(@class, 'mat-calendar-previous-button')]");
            bool anoEncontrado = false;

            for (int i = 0; i < 6; i++) // Bucle para retroceder
            {
                var cellAno = driver.FindElements(By.XPath($"(//mat-datepicker-content)[last()]//div[contains(@class, 'mat-calendar-body-cell-content') and normalize-space()='{anoDesde}']"));

                if (cellAno.Count > 0 && cellAno[0].Displayed)
                {
                    // Encontramos el año, le damos clic
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", cellAno[0]);
                    anoEncontrado = true;
                    Thread.Sleep(1000);
                    break;
                }
                else
                {
                    // Si no está visible, le damos a la flecha izquierda "<"
                    IWebElement prevBtn = wait.Until(ExpectedConditions.ElementToBeClickable(btnPrev));
                    try { prevBtn.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", prevBtn); }
                    Thread.Sleep(500);
                }
            }

            if (!anoEncontrado) throw new Exception($"🚨 FALLO QA: No se pudo encontrar el año {anoDesde} en el calendario.");

            // 4. Seleccionamos el primer mes de la cuadrícula (Enero)
            By primerMes = By.XPath("(//mat-datepicker-content)[last()]//td[contains(@class, 'mat-calendar-body-cell')][1]//div[contains(@class, 'mat-calendar-body-cell-content')]");
            IWebElement mesEnero = wait.Until(ExpectedConditions.ElementToBeClickable(primerMes));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", mesEnero);
            Thread.Sleep(1000);

            // 5. Seleccionamos el DÍA 1 en la cuadrícula de días
            string xpathDia = $"(//mat-datepicker-content)[last()]//*[contains(@class, 'mat-calendar-body-cell') and not(contains(@class, 'mat-calendar-body-disabled'))]//div[contains(@class, 'mat-calendar-body-cell-content') and normalize-space()='1']";
            IWebElement divNumero = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpathDia)));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", divNumero);
            Thread.Sleep(1500);

            Console.WriteLine($"✅ Fecha DESDE (01/01/{anoDesde}) seleccionada exitosamente.");

            // ==========================
            // FECHA HASTA
            // ==========================
            // 6. Clic en el segundo ícono de calendario de la pantalla (HASTA)
            By locIconoHasta = By.XPath("(//mat-datepicker-toggle//button)[2]");
            IWebElement btnCalHasta = wait.Until(ExpectedConditions.ElementToBeClickable(locIconoHasta));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnCalHasta);
            Thread.Sleep(500);
            try { btnCalHasta.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCalHasta); }
            Thread.Sleep(1500);

            // 7. Angular le pone la clase 'today' al día actual, ¡le damos clic directo!
            By locHoy = By.XPath("(//mat-datepicker-content)[last()]//div[contains(@class, 'mat-calendar-body-today')]");
            IWebElement divHoy = wait.Until(ExpectedConditions.ElementIsVisible(locHoy));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", divHoy);
            Thread.Sleep(1500);

            Console.WriteLine("✅ Fecha HASTA (Día de hoy) seleccionada exitosamente.");
        }

        public void FiltrarPorPlaca(string placa)
        {
            var wait = Wait();
            Console.WriteLine("⏳ Buscando filtro de Placa...");

            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(Keys.Escape).Perform();
            Thread.Sleep(1000);

            // Seleccionable de Placa
            By locPlaca = By.XPath("(//mat-form-field[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'placa')]//mat-select)[last()]");
            IWebElement comboPlaca = wait.Until(ExpectedConditions.ElementExists(locPlaca));

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", comboPlaca);
            Thread.Sleep(1000);

            try { comboPlaca.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", comboPlaca); }
            Thread.Sleep(3000);

            new OpenQA.Selenium.Interactions.Actions(driver).SendKeys(placa).Perform();
            Thread.Sleep(1500);

            abastecimientosPage.SeleccionarOpcionConScrollVirtual(placa);
            Thread.Sleep(2000);
        }

        public void ClicBuscar()
        {
            var wait = Wait();
            // Botón de Buscar de tu flujo
            By locBuscar = By.XPath("//button[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'buscar')] | //mat-icon[contains(text(), 'search')]/ancestor::button");
            IWebElement btnBuscar = wait.Until(ExpectedConditions.ElementToBeClickable(locBuscar));

            try { btnBuscar.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnBuscar); }

            Console.WriteLine("⏳ Buscando registros en la Base de Datos...");
            Thread.Sleep(6000); // Dar tiempo a la grilla para que cargue la placa
        }

        public void ClicLupa()
        {
            var wait = Wait(20);
            Console.WriteLine("⏳ Buscando la Lupa en los resultados de la tabla...");

            // 🔥 CORRECCIÓN: Buscamos el ícono 'search' ESTRICTAMENTE dentro de las filas de la tabla (tbody/tr)
            By locLupa = By.XPath("(//tbody//tr//mat-icon[contains(text(), 'search')] | //tr//mat-icon[contains(text(), 'search')])[1]");

            try
            {
                IWebElement btnLupa = wait.Until(ExpectedConditions.ElementExists(locLupa));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", btnLupa);
                Thread.Sleep(1000);

                try { wait.Until(ExpectedConditions.ElementToBeClickable(btnLupa)).Click(); }
                catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnLupa); }

                Console.WriteLine("✅ Clic en la Lupa ejecutado con éxito. Abriendo modal de detalle de consumo...");
                Thread.Sleep(4000);
            }
            catch (WebDriverTimeoutException)
            {
                // Si falla, ahora te avisará por qué falló
                throw new Exception("🚨 FALLO QA: No se encontró la 'Lupa'. La grilla está VACÍA para esta Placa. Probablemente necesitemos ajustar el filtro de Fechas (DESDE/HASTA) para encontrar su historial.");
            }
        }

        // 🔥 NUEVO MÉTODO: Valida la matemática, el color de la celda y los valores límite del Feature
        // 🔥 NUEVO MÉTODO 100% AUTÓNOMO: Ya no depende del Feature, depende de la Regla de Negocio BI 09
        public void ValidarReglaDeNegocioAutonoma()
        {
            var wait = Wait(15);
            Console.WriteLine($"\n⏳ Evaluando Reglas de Negocio BI-09 de forma autónoma...");

            By locFila = By.XPath("(//mat-dialog-container//tbody)[last()]/tr[1]");
            IWebElement filaDetalle = wait.Until(ExpectedConditions.ElementIsVisible(locFila));

            var celdas = filaDetalle.FindElements(By.TagName("td"));
            if (celdas.Count < 9)
                throw new Exception("🚨 FALLO QA: La grilla de detalle no tiene las 9 columnas esperadas.");

            // 1. Extraemos los valores de la fila
            double kmInicial = double.Parse(celdas[3].Text.Trim().Replace(".", "").Replace(",", "."));
            double kmFinal = double.Parse(celdas[4].Text.Trim().Replace(".", "").Replace(",", "."));
            double recorridoUI = double.Parse(celdas[5].Text.Trim().Replace(".", "").Replace(",", "."));
            double galones = double.Parse(celdas[6].Text.Trim().Replace(".", "").Replace(",", "."));
            double rangoFijoUI = double.Parse(celdas[7].Text.Trim().Replace(".", "").Replace(",", "."));
            double rangoPercibidoUI = double.Parse(celdas[8].Text.Trim().Replace(".", "").Replace(",", "."));

            Console.WriteLine($"📊 DATOS DE LA UI -> Km Inicial: {kmInicial} | Km Final: {kmFinal} | Recorrido: {recorridoUI} | Galones: {galones} | Rango Fijo: {rangoFijoUI} | Rango Percibido: {rangoPercibidoUI}");

            // ==========================================
            // REGLA 1: CÁLCULO DEL RECORRIDO
            // ==========================================
            double recorridoCalculado = kmFinal - kmInicial;
            if (recorridoCalculado != recorridoUI)
                throw new Exception($"🚨 BUG MATEMÁTICO: El Recorrido UI ({recorridoUI}) no coincide con el cálculo ({kmFinal} - {kmInicial} = {recorridoCalculado}).");

            Console.WriteLine("✅ REGLA 1 OK: El Recorrido (Km Final - Km Inicial) es matemáticamente correcto.");

            // ==========================================
            // REGLA 2: CÁLCULO DEL RANGO PERCIBIDO (Rendimiento)
            // ==========================================
            double rendimientoCalculado = 0;
            if (galones > 0)
            {
                rendimientoCalculado = Math.Round(recorridoCalculado / galones, 3);
                if (Math.Abs(rendimientoCalculado - rangoPercibidoUI) > 0.05)
                    throw new Exception($"🚨 BUG MATEMÁTICO: Rendimiento incorrecto. UI dice {rangoPercibidoUI}, cálculo real es {rendimientoCalculado}.");

                Console.WriteLine("✅ REGLA 2 OK: El Rango Percibido (Recorrido / Galones) es correcto.");
            }

            // ==========================================
            // REGLA 3: TOLERANCIA Y COLOR (BI-09)
            // ==========================================
            // A) Determinar el porcentaje de tolerancia según el Km Final
            double porcentajeTolerancia = (kmFinal >= 100000) ? 0.20 : 0.10;

            // B) Calcular el Descuento de Tolerancia (x% del Rango Percibido)
            double descuentoTolerancia = rendimientoCalculado * porcentajeTolerancia;

            // C) Calcular el Rango Fijo Establecido Final
            double rangoFijoEstablecidoFinal = rangoFijoUI - descuentoTolerancia;

            // D) Evaluar la condición final
            bool deberiaSerAceptable = rendimientoCalculado >= rangoFijoEstablecidoFinal;

            Console.WriteLine($"⚙️ MOTOR DE REGLAS BI-09:");
            Console.WriteLine($"   - Porcentaje de Tolerancia aplicado: {porcentajeTolerancia * 100}% (Porque Km Final es {(kmFinal >= 100000 ? ">= 100k" : "< 100k")})");
            Console.WriteLine($"   - Rango Fijo Establecido Final calculado: {rangoFijoUI} - {descuentoTolerancia} = {rangoFijoEstablecidoFinal}");
            Console.WriteLine($"   - ¿Rendimiento ({rendimientoCalculado}) >= Rango Final ({rangoFijoEstablecidoFinal})? -> {deberiaSerAceptable}");

            // E) Extraer el color real que Angular le puso a la celda
            string htmlCelda = celdas[8].GetAttribute("outerHTML").ToLower();

            if (deberiaSerAceptable)
            {
                if (!htmlCelda.Contains("acceptable") || htmlCelda.Contains("unacceptable"))
                    throw new Exception($"🚨 BUG VISUAL: Según la regla BI 09, el consumo DEBE SER VERDE (Aceptable). Pero el sistema lo pintó de otro color.");

                Console.WriteLine("✅ REGLA 3 OK: El sistema cumplió la lógica de negocio y pintó el consumo como ACEPTABLE (Verde).");
            }
            else
            {
                if (!htmlCelda.Contains("unacceptable") && !htmlCelda.Contains("danger") && !htmlCelda.Contains("bad") && !htmlCelda.Contains("red"))
                    throw new Exception($"🚨 BUG VISUAL: Según la regla BI 09, el consumo DEBE SER ROJO (No Aceptable). Pero el sistema lo permitió como verde.");

                Console.WriteLine("✅ REGLA 3 OK: El sistema cumplió la lógica de negocio y pintó la anomalía como NO ACEPTABLE (Rojo).");
            }
        }
        public void CerrarModal()
        {
            var wait = Wait();
            // XPath exacto que me pasaste para Cerrar
            By locCerrar = By.XPath("(//mat-icon[contains(text(), 'close')])[last()]");
            IWebElement btnCerrar = wait.Until(ExpectedConditions.ElementToBeClickable(locCerrar));

            try { btnCerrar.Click(); } catch { ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", btnCerrar); }

            Console.WriteLine("✅ Modal cerrado con éxito.");
            Thread.Sleep(2000);
        }
    }
}