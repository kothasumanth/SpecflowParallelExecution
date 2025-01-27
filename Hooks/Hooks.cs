using BoDi;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using SimpleSpecflowExtentReport.Utility;

using NUnit.Framework;
[assembly: Parallelizable(ParallelScope.Fixtures)] //Can only parallelise Features
[assembly: LevelOfParallelism(8)] //Worker thread i.e. max amount of Features to run in Parallel

namespace SimpleSpecflowExtentReport.Hooks
{

    [Binding]
    public sealed class Hooks : ExtentReport
    {
        private readonly IObjectContainer _container;
        public static String dir = AppDomain.CurrentDomain.BaseDirectory;
        public static String testResultPath = dir.Replace("bin\\Debug\\net6.0", "TestResults");

        public Hooks(IObjectContainer container)
        {
            _container = container;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ExtentReport.ExtentReportInit();
        }

        [BeforeFeature]
        public static void CreateFeature(FeatureContext featureContext)
        {
            ExtentReport.AddFeature(featureContext);
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {

            //Launch the browser
            EdgeOptions edgeOptions = new EdgeOptions();
            // edgeOptions.UseChromium = true;
            // edgeOptions.AddArgument("headless");
            // edgeOptions.AddArgument("disable-gpu");
            // edgeOptions.AddArgument("no-sandbox");
            // edgeOptions.AddArgument("disable-dev-shm-usage");
            // edgeOptions.AddArgument("disable-extensions");
            // edgeOptions.AddArgument("disable-infobars");
            // edgeOptions.AddArgument("disable-popup-blocking");
            // edgeOptions.AddArgument("disable-notifications");
            // edgeOptions.AddArgument("disable-extensions-except");
            // edgeOptions.AddArgument("enable-automation");
            // edgeOptions.AddArgument("start-maximized");
            IWebDriver driver = new EdgeDriver(edgeOptions);

            _container.RegisterInstanceAs<IWebDriver>(driver);
            ExtentReport.AddScenario(scenarioContext);
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            //Take screenshot
            var driver = _container.Resolve<IWebDriver>();
            if (driver != null)
            {
                driver.Quit();
            }
        }

        [BeforeStep]
        public void BeforeStep(ScenarioContext scenarioContext)
        {
            ExtentReport.AddStep(scenarioContext);

        }

        [AfterStep]
        public void AfterStep(ScenarioContext scenarioContext)
        {
            string stepType = scenarioContext.StepContext.StepInfo.StepDefinitionType.ToString();
            string stepName = scenarioContext.StepContext.StepInfo.Text;
            var driver = _container.Resolve<IWebDriver>();
            if (scenarioContext.TestError != null)
            {
                ExtentReport.Log("fail", scenarioContext.TestError.Message);
                //Take screenshot
                ExtentReport.LogScreenshot(driver, scenarioContext);
            }
        }


        [AfterTestRun]
        public static void FlushReport()
        {
            ExtentReport.Flush();
        }
    }
}