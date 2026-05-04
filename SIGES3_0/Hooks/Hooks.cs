using SIGES3_0.Utility;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll.BoDi;
using System.IO;

namespace SIGES3_0.Hooks
{
    [Binding]
    public sealed class Hooks : ExtentReport
    {
        private readonly IObjectContainer _container;

        public Hooks(IObjectContainer container)
        {
            _container = container;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Console.WriteLine("Running before test run...");
            ExtentReportInit();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            Console.WriteLine("Running after test run...");
            ExtentReportTearDown();
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            Console.WriteLine("Running before feature...");
            var report = _extentReports ?? throw new InvalidOperationException("ExtentReports no fue inicializado.");
            _feature = report.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            Console.WriteLine("Running after feature...");
        }


        [BeforeScenario("@Testers")]
        public void BeforeScenarioWithTag()
        {
            Console.WriteLine("Runnig inside tagged hooks in SpecFlow");
        }

        [BeforeScenario(Order = 1)]
        public void FirstBeforeScenario(ScenarioContext scenarioContext)
        {
            IWebDriver driver = CreateChromeDriver();
            driver.Manage().Window.Maximize();

            _container.RegisterInstanceAs<IWebDriver>(driver);

            var feature = _feature ?? throw new InvalidOperationException("El feature actual no fue inicializado.");
            _scenario = feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            var scenarioTitle = scenarioContext.ScenarioInfo?.Title ?? "Escenario sin titulo";
            var testError = scenarioContext.TestError;

            if (testError == null)
            {
                Console.WriteLine($"[RESULTADO][PASS] {scenarioTitle}");
            }
            else
            {
                var message = testError.Message ?? string.Empty;
                if (message.Contains("[CSS-POPUP]", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"[RESULTADO][FAIL][CSS] {scenarioTitle} -> {message}");
                }
                else
                {
                    Console.WriteLine($"[RESULTADO][FAIL] {scenarioTitle} -> {message}");
                }
            }

            var driver = _container.Resolve<IWebDriver>();

            if (driver != null)
            {
                driver.Quit();
            }
        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            Console.WriteLine("Running after step...");
            var currentScenario = _scenario ?? throw new InvalidOperationException("El escenario actual no fue inicializado.");
            string stepType = scenarioContext.StepContext?.StepInfo?.StepDefinitionType.ToString() ?? string.Empty;
            string stepName = scenarioContext.StepContext?.StepInfo?.Text ?? string.Empty;
            var driver = _container.Resolve<IWebDriver>();

            // When scenario passed
            if (scenarioContext.TestError == null)
            {
                if (stepType == "Given")
                {
                    currentScenario.CreateNode<Given>(stepName);
                }
                else if (stepType == "When")
                {
                    currentScenario.CreateNode<When>(stepName);
                }
                else if (stepType == "Then")
                {
                    currentScenario.CreateNode<Then>(stepName);
                }
            }

            //When scenario fails
            if (scenarioContext.TestError != null)
            {
                var screenshotPath = addScreenshot(driver, scenarioContext);
                var media = MediaEntityBuilder
                    .CreateScreenCaptureFromPath(screenshotPath)
                    .Build();

                switch (stepType)
                {
                    case "Given":
                        currentScenario.CreateNode<Given>(stepName).Fail(scenarioContext.TestError.Message, media);
                        break;
                    case "When":
                        currentScenario.CreateNode<When>(stepName).Fail(scenarioContext.TestError.Message, media);
                        break;
                    case "Then":
                        currentScenario.CreateNode<Then>(stepName).Fail(scenarioContext.TestError.Message, media);
                        break;
                    case "And":
                        currentScenario.CreateNode<And>(stepName).Fail(scenarioContext.TestError.Message, media);
                        break;
                }
            }
        }

        private static IWebDriver CreateChromeDriver()
        {
            var chromeOptions = new ChromeOptions();
            var chromeBinary = ResolveChromeBinary();
            if (!string.IsNullOrEmpty(chromeBinary))
            {
                chromeOptions.BinaryLocation = chromeBinary;
            }

            chromeOptions.AddArgument("--disable-search-engine-choice-screen");
            chromeOptions.AddArgument("--no-first-run");
            chromeOptions.AddArgument("--disable-dev-shm-usage");

            var driverDirectory = ResolveChromeDriverDirectory();
            if (!string.IsNullOrEmpty(driverDirectory))
            {
                var service = ChromeDriverService.CreateDefaultService(driverDirectory);
                return new ChromeDriver(service, chromeOptions);
            }

            return new ChromeDriver(chromeOptions);
        }

        private static string ResolveChromeBinary()
        {
            var configuredPath = Environment.GetEnvironmentVariable("CHROME_BINARY_PATH");
            if (!string.IsNullOrWhiteSpace(configuredPath) && File.Exists(configuredPath))
            {
                return configuredPath;
            }

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            {
                const string macChromePath = "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome";
                if (File.Exists(macChromePath)) return macChromePath;
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                var winChromePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Google\Chrome\Application\chrome.exe");
                if (File.Exists(winChromePath)) return winChromePath;
                winChromePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Google\Chrome\Application\chrome.exe");
                if (File.Exists(winChromePath)) return winChromePath;
                winChromePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Google\Chrome\Application\chrome.exe");
                if (File.Exists(winChromePath)) return winChromePath;
            }

            return string.Empty;
        }

        private static string ResolveChromeDriverDirectory()
        {
            var configuredPath = Environment.GetEnvironmentVariable("CHROMEDRIVER_DIRECTORY");
            if (!string.IsNullOrWhiteSpace(configuredPath) && Directory.Exists(configuredPath))
            {
                return configuredPath;
            }

            var projectRoot = ResolveProjectRoot();
            var driversRoot = Path.Combine(projectRoot, "Drivers");

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            {
                var driverDirectory = Path.Combine(driversRoot, "chromedriver-mac-arm64");
                var driverBinary = Path.Combine(driverDirectory, "chromedriver");
                if (File.Exists(driverBinary)) return driverDirectory;
            }
            else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                var driverDirectory = Path.Combine(driversRoot, "chromedriver-win64");
                var driverBinary = Path.Combine(driverDirectory, "chromedriver.exe");
                if (File.Exists(driverBinary)) return driverDirectory;
            }

            return string.Empty;
        }

        private static string ResolveProjectRoot()
        {
            var current = new DirectoryInfo(AppContext.BaseDirectory);

            while (current != null)
            {
                if (string.Equals(current.Name, "bin", StringComparison.OrdinalIgnoreCase))
                {
                    return current.Parent?.FullName
                        ?? throw new DirectoryNotFoundException("No se pudo resolver la carpeta raiz del proyecto.");
                }

                current = current.Parent;
            }

            throw new DirectoryNotFoundException("No se pudo resolver la carpeta raiz del proyecto desde AppContext.BaseDirectory.");
        }
    }
}
