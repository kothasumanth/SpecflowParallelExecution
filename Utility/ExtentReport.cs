using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V105.SystemInfo;

namespace SimpleSpecflowExtentReport.Utility
{
    public class ExtentReport
    {
        public static String dir = AppDomain.CurrentDomain.BaseDirectory;
        public static string testResultPath = Directory.GetParent(dir).Parent.Parent.Parent.FullName + "\\TestResults\\";
        private static ExtentReports extent;
        private static ExtentTest feature;
        private static ExtentTest scenario;
        private static ExtentTest step;

        public static void ExtentReportInit()
        {
            try
            {
                var configPath = Directory.GetParent(dir).Parent.Parent.Parent.FullName + "\\extent-config.xml";
                var sparkReporter = new ExtentSparkReporter(testResultPath + "index.html");
                sparkReporter.LoadConfig(configPath);

                extent = new ExtentReports();
                // Add system info (optional)
                extent.AddSystemInfo("Browser", "Edge");
                extent.AddSystemInfo("Tester", "kotha.sumanth@gmail.com");
                extent.AttachReporter(sparkReporter);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading config: " + ex.Message);
            }
        }

        public static void AddFeature(FeatureContext featureContext)
        {
            feature = extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
        }

        public static void AddScenario(ScenarioContext scenarioContext)
        {

            scenario = feature.CreateNode<Scenario>(scenarioContext.ScenarioInfo.Title);
        }

        public static void AddStep(ScenarioContext scenarioContext)
        {
            if (scenarioContext.StepContext.StepInfo.StepDefinitionType == TechTalk.SpecFlow.Bindings.StepDefinitionType.Given)
            {
                step = scenario.CreateNode<Given>(scenarioContext.StepContext.StepInfo.Text);
            }
            else if (scenarioContext.StepContext.StepInfo.StepDefinitionType == TechTalk.SpecFlow.Bindings.StepDefinitionType.When)
            {
                step = scenario.CreateNode<When>(scenarioContext.StepContext.StepInfo.Text);
            }
            else if (scenarioContext.StepContext.StepInfo.StepDefinitionType == TechTalk.SpecFlow.Bindings.StepDefinitionType.Then)
            {
                step = scenario.CreateNode<Then>(scenarioContext.StepContext.StepInfo.Text);
            }
        }

        public static void LogScreenshot(IWebDriver driver, ScenarioContext scenarioContext)
        {
            step.AddScreenCaptureFromPath(addScreenshot(driver, scenarioContext));
        }

        public static void Flush()
        {
            extent.Flush();
        }

        public static string addScreenshot(IWebDriver driver, ScenarioContext scenarioContext)
        {
            ITakesScreenshot takesScreenshot = (ITakesScreenshot)driver;
            Screenshot screenshot = takesScreenshot.GetScreenshot();
            string screenshotLocation = Path.Combine(testResultPath, scenarioContext.ScenarioInfo.Title + ".png");
            screenshot.SaveAsFile(screenshotLocation, ScreenshotImageFormat.Png);
            return screenshotLocation;
        }

        public static void Log(string statusType, string message)
        {
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string Message = $"{timeStamp} - {statusType.ToUpper().Trim()} - {message}";
            switch (statusType.ToLower().Trim())
            {
                case "info":
                    step.Log(Status.Info, MarkupHelper.CreateLabel(Message, ExtentColor.Blue));
                    break;
                case "pass":
                    step.Log(Status.Pass, MarkupHelper.CreateLabel(Message, ExtentColor.Green));
                    break;
                case "fail":
                    step.Log(Status.Fail, MarkupHelper.CreateLabel(Message, ExtentColor.Red));
                    break;
                case "warning":
                    step.Log(Status.Warning, MarkupHelper.CreateLabel(Message, ExtentColor.Yellow));
                    break;
            }
        }
    }
}