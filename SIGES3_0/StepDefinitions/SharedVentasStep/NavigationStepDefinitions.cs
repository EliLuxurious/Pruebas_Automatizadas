using OpenQA.Selenium;
using Reqnroll;
using SIGES3_0.Pages.VentasPage;

namespace SIGES3_0.StepDefinitions.SharedStep
{
    /// <summary>
    /// Steps de navegación compartidos.
    /// </summary>
    [Binding]
    public class NavigationStepDefinitions
    {
        private readonly ReportesPage _reportesPage;

        public NavigationStepDefinitions(IWebDriver driver)
        {
            _reportesPage = new ReportesPage(driver);
        }

        // ── Módulo (ítem de menú principal) ──────────────────────────────────
        [When(@"el usuario accede al módulo '(.*)'")]
        public void WhenElUsuarioAccedeAlModulo(string modulo) =>
            _reportesPage.AccederModulo(modulo);

        // ── Submódulo (ítem de menú secundario) ──────────────────────────────
        [When(@"el usuario accede al submodulo '(.*)'")]
        public void WhenElUsuarioAccedeAlSubmodulo(string submodulo) =>
            _reportesPage.AccederSubmodulo(submodulo);
    }
}