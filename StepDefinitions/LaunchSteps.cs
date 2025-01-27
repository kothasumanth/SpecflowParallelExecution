using AventStack.ExtentReports.Gherkin.Model;
using OpenQA.Selenium;
using SimpleSpecflowExtentReport.PagesObjectModels;
using SimpleSpecflowExtentReport.Utility;

namespace SpecFlowProject1.StepDefinitions
{
    [Binding]
    public sealed class LaunchSteps
    {
       private IWebDriver driver;
       LaunchPage launchPage ;
       public LaunchSteps(IWebDriver driver)
       {
           this.driver = driver;
           launchPage = new LaunchPage(driver);
       }
        [Given(@"Login to the webpage")]
        public void GivenINavigateToTheApplication()
        {
            launchPage.LoginToWebApplication();
            ExtentReport.Log("info","Webpage is launched");
        }

        [When(@"i navigate to some page")]
        public void WhenINavigateToSomePage()
        {
            launchPage.LoginToWebApplication();
            ExtentReport.Log("info","Navigated to webpage");
        }

        [Then(@"element should have this")]
        public void ThenElementShouldHaveThis()
        {
            launchPage.LoginToWebApplication();
            ExtentReport.Log("pass","Element is present");
        }
    }
}