using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumExtras.PageObjects;

namespace NUnitTestProject1
{
    public class Tests
    {
        private IWebDriver driver;

        private readonly string bbc_url = "https://www.bbc.com/";

        [FindsBy(How = How.XPath, Using = "//nav[@role='navigation']//a[@href='https://www.bbc.com/news']")]
        private IWebElement news_buton;

        [SetUp]
        public void SterBrowser(IWebDriver driver)
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(bbc_url);
            news_buton.Click();
            PageFactory.InitElements(driver, this);
        }

        [Test]
        public void CheckTheNameOfTheHeadlineArticle()
        {

            Assert.Pass();
        }
    }
}