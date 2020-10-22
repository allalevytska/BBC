using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Collections;
using NUnit.Framework.Constraints;

namespace NUnitTestProject1
{
    public class Tests
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        private readonly string bbc_url = "https://www.bbc.com/";
        private readonly long timeout = 3000;

        public static string HEADLINE_TEXT = "England awaits new virus lockdown rules ";
        public static List<string> SECONDARY_TITLES = new List<string> {
            "Armenia-Azerbaijan truce broken minutes after deal",
            "Lives that could be reshaped by US Supreme Court",
            "The story of the blaze that destroyed Moria camp",
            "London Bridge attack hero could be released early",
            "Hay Festival severs UAE ties after assault claim" };
        public static string SEARCH_TEXT = "Samuel Paty was murdered after showing cartoons of the Prophet Muhammad in class.";
        public static string MY_CORONAVIRUS_STORY = "My coronavirus story";
        public static string NAME = "Alla";
        public static string EMAIL = "lyrae.aa@gmail";
        public static string LOCATION = "Ukraine";
        public static string EMAIL_ERROR_MESSAGE = "Email address is invalid";

        private string popUpId = "sign_in-bg";
        private string mayBeLaterPopUpSignInCSS = "button.sign_in-exit";
        private string newsButtonXPath = "//nav[@role='navigation']//a[@href='https://www.bbc.com/news']";
        private string headlineNameXPath = "(//a[@class='gs-c-promo-heading gs-o-faux-block-link__overlay-link gel-paragon-bold nw-o-link-split__anchor'])[1]";
        private string secondaryItemsTitlesCSS = "div[class*='secondary-item'] a[class*='promo']";
        private string bussinesCategoryArticleXPath = "//p[contains(text(),'Samuel')]";
        private string searchFieldCSS = "input#orb-search-q";
        private string coronavirusTabXPath = "(//a[@href='/news/coronavirus'])[1]";
        private string yourCoronaVirusStoryXPath = "(//a[@href='/news/have_your_say'])[1]";
        private string shareWithBBCXPath = "//a[@href='/news/10725415']";
        private string formCSS = "div.embed-content-container";
        private string textPlaceholderCSS = "div.embed-content-container textarea";
        private string nameInputCSS = "input[aria-label='Name']";
        private string emailInputCSS = "input[aria-label='Email address']";
        private string locationInputCSS = "input[aria-label='Location ']";
        private string firstCheckboxXPath = "(//input[@type='checkbox'])[1]";
        private string secondCheckboxXPath = "(//input[@type='checkbox'])[2]";
        private string thirdCheckboxXPath = "(//input[@type='checkbox'])[3]";
        private string submitButtonCSS = "button.button";
        private string emailErrorMessageCSS = "div.input-error-message";


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
        public void WaitForPageLoadComplete()
        {
            IWait<IWebDriver> wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(100000000));
            wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        private IWebElement MayBeLaterPopUpSignIn => driver.FindElement(By.CssSelector(mayBeLaterPopUpSignInCSS));
        public IWebElement NewsButton => driver.FindElement(By.XPath(newsButtonXPath));
        public IWebElement HeadlineName => driver.FindElement(By.XPath(headlineNameXPath));
        public IWebElement BussinessCategoryArticle => driver.FindElement(By.XPath(bussinesCategoryArticleXPath));
        public IWebElement SearchField => driver.FindElement(By.CssSelector(searchFieldCSS));
        public IWebElement CoronavirusTab => driver.FindElement(By.XPath(coronavirusTabXPath));
        public IWebElement YourCoronavirusStoryTab => driver.FindElement(By.XPath(yourCoronaVirusStoryXPath));
        public IWebElement ShareWithBBCButton => driver.FindElement(By.XPath(shareWithBBCXPath));
        public IWebElement Form => driver.FindElement(By.CssSelector(formCSS));
        public IWebElement TextPlaceholder => driver.FindElement(By.CssSelector(textPlaceholderCSS));
        public IWebElement NameInput => driver.FindElement(By.CssSelector(nameInputCSS));
        public IWebElement EmailInput => driver.FindElement(By.CssSelector(emailInputCSS));
        public IWebElement LocationInput => driver.FindElement(By.CssSelector(locationInputCSS));
        public IWebElement FirstCheckbox => driver.FindElement(By.XPath(firstCheckboxXPath));
        public IWebElement SecondCheckbox => driver.FindElement(By.XPath(secondCheckboxXPath));
        public IWebElement ThirdCheckbox => driver.FindElement(By.XPath(thirdCheckboxXPath));
        public IWebElement SubmitButton => driver.FindElement(By.CssSelector(submitButtonCSS));
        public IWebElement ErrorMessageText => driver.FindElement(By.CssSelector(emailErrorMessageCSS));

        [Test]
        public void CheckTheNameOfTheHeadlineArticle()
        {
            NewsButton.Click();
            WaitForElement(HeadlineName);
            Assert.IsTrue(HeadlineName.Text.Contains(HEADLINE_TEXT));
        }

        public IEnumerable<string> SecondaryItemTitles()
        {
            foreach (IWebElement item in driver.FindElements(By.CssSelector(secondaryItemsTitlesCSS)))
            {
                yield return item.Text;
            }
        }

        [Test]
        public void CheckTheSecondaryArticleTitles()
        {
            NewsButton.Click();
            WaitForPageLoadComplete();
            Assert.IsTrue(SECONDARY_TITLES.SequenceEqual(SecondaryItemTitles().ToList<string>()));
        }

        [Test]
        public void VarifyTheNameOfTheFirstArticleAgainstTheSearchName()
        {
            SearchField.SendKeys(SEARCH_TEXT + Keys.Enter);
            WaitForElement(BussinessCategoryArticle);
            Assert.AreEqual(SEARCH_TEXT, BussinessCategoryArticle.Text);
        }

        [Test]
        public void VerifyThatUserCanSubmitTheQuestiontoBBC()
        {
            NewsButton.Click();
            WaitForPageLoadComplete();
            if (driver.PageSource.Contains(popUpId))
            {
                MayBeLaterPopUpSignIn.Click();
            }
            WaitForElement(CoronavirusTab);
            CoronavirusTab.Click();
            WaitForElement(YourCoronavirusStoryTab);
            YourCoronavirusStoryTab.Click();
            WaitForElement(ShareWithBBCButton);
            ShareWithBBCButton.Click();
            WaitForElement(Form);
            TextPlaceholder.SendKeys(MY_CORONAVIRUS_STORY + Keys.Tab);
            NameInput.SendKeys(NAME + Keys.Tab);
            EmailInput.SendKeys(EMAIL + Keys.Tab + Keys.Tab);
            LocationInput.SendKeys(LOCATION + Keys.Tab + Keys.Tab);
            SecondCheckbox.Click();
            ThirdCheckbox.Click();
            SubmitButton.Click();
            WaitForElement(ErrorMessageText);
            Assert.AreEqual(EMAIL_ERROR_MESSAGE, ErrorMessageText.Text);

        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }
    }
}