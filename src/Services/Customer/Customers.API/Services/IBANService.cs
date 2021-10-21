using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace CodeChallenge.Services.Customers.Api.Services
{
    public class IBANService : IIBANService
    {
        private readonly string _requestUrl;
        private readonly string _remoteDriverUrl;
        private readonly string IBAN_DISPLAY_CLASS = "ibandisplay";
        private readonly int JS_EXECUTE_TIMEOUT = 1000;

        public IBANService(IOptions<CustomerSettings> settings)
        {
            _requestUrl = settings.Value.IBANReqestUrl;
            _remoteDriverUrl = settings.Value.RemoteDriverUrl; 
        }

        public string GetRandomIBANAsync()
        {

            var chromeOptions = new ChromeOptions();
            chromeOptions.BrowserVersion = "94.0";
            chromeOptions.PlatformName = "Linux";
            IWebDriver driver = new RemoteWebDriver(new Uri(_remoteDriverUrl), chromeOptions);

            driver.Navigate().GoToUrl(_requestUrl);
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(JS_EXECUTE_TIMEOUT));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            var iBan = driver.FindElement(By.ClassName(IBAN_DISPLAY_CLASS)).Text;
            driver.Quit();

            return iBan;

        }
    }
}
