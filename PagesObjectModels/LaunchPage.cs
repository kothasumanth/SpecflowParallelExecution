
using OpenQA.Selenium;
using SimpleSpecflowExtentReport.Utility;

namespace SimpleSpecflowExtentReport.PagesObjectModels
{
    public class LaunchPage
    {
       private IWebDriver driver;
       public LaunchPage(IWebDriver driver)
       {
           this.driver = driver;
       }
        public void LoginToWebApplication()
        {
            driver.Url = "https://www.google.com";            
        }
    }
}