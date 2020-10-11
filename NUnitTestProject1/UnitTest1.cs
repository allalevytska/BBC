using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NUnitTestProject1
{
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        private readonly string bbc_url = "https://www.bbc.com/";
        private readonly long timeout = 3000;

        public static string HEADLINE_TEXT = "National lockdown possible, says top UK scientist";
        public static List<string> SECONDARY_TITLES = new List<string> { 
            "UK at virus 'tipping point', top scientist warns",
            "North Korea documentary films undercover 'arms deals'",
            "Planet Mars is at its 'biggest and brightest'",
            "India's Covid-19 outbreak in 200 seconds",
            "The death of the Full Moon Party" };
        public static string SEARCH_WORD = "Microsoft makes remote work option permanent";

        private string PopUpSignInCSS = "button.sign_in-exit";
        private string newsButtonXPath = "//nav[@role='navigation']//a[@href='https://www.bbc.com/news']";
        private string headlineNameXPath = "(//a[@class='gs-c-promo-heading gs-o-faux-block-link__overlay-link gel-paragon-bold nw-o-link-split__anchor'])[1]";
        private string secondaryItemsTitlesCSS = "div[class*='secondary-item']";
        private string bussinesCategoryArticleXPath = "//a[@href]/span[@aria-hidden='false']";
        private string searchFieldCSS = "input#orb-search-q";
        private string coronavirusTabXPath = "(//a[@href='/news/coronavirus'])[1]";
        private string yourCoronaVirusStoryXPath = "(//a[@href='/news/have_your_say'])[1]";
        private string shareWithBBCXPath = "(//h3[@class='gs-c-promo-heading__title gel-pica-bold nw-o-link-split__text'])[19]";

        public void WaitForPageLoadComplete()
        {
            IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readystate").Equals("Complete"));
        }

        public void WaitForElement(IWebElement element)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            do
            {
                if (element.Displayed)
                {
                    return;
                }

            } while (stopwatch.ElapsedMilliseconds < timeout);
            throw new ElementNotVisibleException("Element not visible");
        }

        [SetUp]
        public void StartBrowser()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            driver.Navigate().GoToUrl(bbc_url);
        }

        private IWebElement SkipPopUpSignIn => driver.FindElement(By.CssSelector(PopUpSignInCSS));
        public IWebElement NewsButton => driver.FindElement(By.XPath(newsButtonXPath));
        public IWebElement HeadlineName => driver.FindElement(By.XPath(headlineNameXPath));
        public IWebElement BussinessCategoryArticleTitle => driver.FindElement(By.XPath(bussinesCategoryArticleXPath));
        public IWebElement SearchField => driver.FindElement(By.CssSelector(searchFieldCSS));
        public IWebElement CoronavirusTab => driver.FindElement(By.XPath(coronavirusTabXPath));
        public IWebElement YourCoronavirusStoryTab => driver.FindElement(By.XPath(yourCoronaVirusStoryXPath));
        public IWebElement ShareWithBBCButton => driver.FindElement(By.XPath(shareWithBBCXPath));
        
        [Test]
        public void CheckTheNameOfTheHeadlineArticle()
        {
            NewsButton.Click();
            WaitForElement(HeadlineName);
            Assert.IsTrue(HeadlineName.Text.Contains(HEADLINE_TEXT));
        }

        [Test]
        public void CheckTheSecondaryArticleTitles()
        {
            List<string> SecondaryItemTitles = driver.FindElements(By.CssSelector(secondaryItemsTitlesCSS)).Select(a => a.Text).ToList();
            Assert.IsTrue(SecondaryItemTitles.SequenceEqual(SECONDARY_TITLES)); 
        }

        [Test]
        public void VarifyTheNameOfTheFirstArticleAgainstTheSearchName()
        {
            SearchField.SendKeys(SEARCH_WORD);
            WaitForElement(BussinessCategoryArticleTitle);
            Assert.AreEqual(SEARCH_WORD, BussinessCategoryArticleTitle.Text);
        }

        [Test]
        public void VerifyThatUserCanSubmitTheQuestiontoBBC()
        {
            CoronavirusTab.Click();
            YourCoronavirusStoryTab.Click();
            ShareWithBBCButton.Click();
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }
    }
}