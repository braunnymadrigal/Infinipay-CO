using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Tests
{
    class CompanyCreationTest
    {
        private const int MILISECONDS_TO_WAIT = 4000;
        private const string BASE_URL = "http://localhost:8080/";
        private const string CREATE_EMPLOYER_URL = "RegisterEmployer";
        private const string CREATE_COMPANY_URL = "RegisterCompany";
        private const string LOGIN_URL = "LoginUser";
        private const string PROFILE_URL = "MyProfile";
        private const string COMPANIES_URL = "CompanyList";

        private const string ADMIN_USERNAME = "braunny_madrigal";
        private const string ADMIN_PASSWORD = "braunnyBRAUNNY1234?";

        private const string COMPANY_VALID_NAME = "Colchonería JIRON";
        private const string COMPANY_VALID_ASSOCIATION = "Asociación JIRON";
        private const string COMPANY_VALID_DESCRIPTION = "Contamos con gran variedad de modelos para diferentes necesidades.";
        private const string COMPANY_VALID_ID = "3101009591";
        private const string COMPANY_VALID_PHONE_NUMBER = "23970516";
        private const string COMPANY_VALID_EMAIL = "jiron@gmail.com";
        private const string COMPANY_VALID_USERNAME = "pedro_vargas";
        private const string COMPANY_VALID_PROVINCE = "Guanacaste";
        private const string COMPANY_VALID_COUNTY = "Bagaces";
        private const string COMPANY_VALID_DISTRICT = "Fortuna";
        private const string COMPANY_VALID_EXTRA = "100 metros este de la escuela de Salitral";

        IWebDriver _driver;
        WebDriverWait _wait;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromMilliseconds(MILISECONDS_TO_WAIT));
        }

        [TearDown]
        public void Teardown()
        {
            _driver.Close();
        }

        [Test]
        public void CreateCompany()
        {
            try
            {
                var js = (IJavaScriptExecutor)_driver;
                _driver.Manage().Window.Maximize();
                _driver.Navigate().GoToUrl(BASE_URL + CREATE_EMPLOYER_URL);

                var employerValidElements = new Dictionary<string, string>
                {
                    {"firstName", "Pedro"},
                    {"firstLastName", "Vargas"},
                    {"secondLastName", "Mora"},
                    {"username", "pedro_vargas"},
                    {"password", "pedroPEDRO1234?"},
                    {"idNumber", "103970516"},
                    {"phoneNumber", "63970516"},
                    {"email", "pedrovargas@gmail.com"},
                    {"province", "Guanacaste"},
                    {"canton", "Bagaces"},
                    {"district", "Fortuna"},
                    {"otherSigns", "100 metros este de la escuela"}
                };

                var employerInvalidElements = new Dictionary<string, string>
                {
                    {"firstName", "Pedro123"},
                    {"username", "pedro Vargas"},
                    {"password", "12345"},
                    {"idNumber", "123"},
                    {"phoneNumber", "123"},
                    {"email", "pedro@.com"}
                };

                var employerGender = _driver.FindElement(By.CssSelector("#gender > option:nth-child(2)"));
                employerGender.Click();

                var employerBirthDay = _driver.FindElement(By.CssSelector("#birthDay > option:nth-child(2)"));
                employerBirthDay.Click();

                var employerBirthMonth = _driver.FindElement(By.CssSelector("#birthMonth > option:nth-child(2)"));
                employerBirthMonth.Click();

                var employerBirthYear = _driver.FindElement(By.CssSelector("#birthYear > option:nth-child(2)"));
                employerBirthYear.Click();

                var employerButton = _driver.FindElement(By.CssSelector("form button[type='submit']"));

                js.ExecuteScript("arguments[0].click();", employerButton);
                Assert.That(_driver.Url, Is.EqualTo(BASE_URL + CREATE_EMPLOYER_URL));

                foreach (var element in employerValidElements)
                {
                    fillEmployerInput(element.Key, element.Value);
                }

                foreach (var element in employerInvalidElements)
                {
                    fillEmployerInput(element.Key, element.Value);
                    js.ExecuteScript("arguments[0].click();", employerButton);
                    Assert.That(_driver.Url, Is.EqualTo(BASE_URL + CREATE_EMPLOYER_URL));
                    fillEmployerInput(element.Key, employerValidElements[element.Key]);
                }

                foreach (var element in employerInvalidElements)
                {
                    fillEmployerInput(element.Key, element.Value);
                }

                js.ExecuteScript("arguments[0].click();", employerButton);
                Assert.That(_driver.Url, Is.EqualTo(BASE_URL + CREATE_EMPLOYER_URL));

                foreach (var element in employerInvalidElements)
                {
                    fillEmployerInput(element.Key, employerValidElements[element.Key]);
                }

                js.ExecuteScript("arguments[0].click();", employerButton);
                _wait.Until(driver => driver.Url.EndsWith(CREATE_COMPANY_URL));
                Assert.That(_driver.Url, Is.EqualTo(BASE_URL + CREATE_COMPANY_URL));


                _driver.FindElement(By.Id("employerUsername"))
                    .SendKeys(COMPANY_VALID_USERNAME);
                _driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/form/div[2]/input"))
                    .SendKeys(COMPANY_VALID_NAME);
                _driver.FindElement(By.XPath("/html/body/div/div/div/div[2]/form/div[3]/input"))
                    .SendKeys(COMPANY_VALID_ASSOCIATION);
                _driver.FindElement(By.Id("description"))
                    .SendKeys(COMPANY_VALID_DESCRIPTION);
                _driver.FindElement(By.CssSelector("#creationDay > option:nth-child(2)"))
                    .Click();
                _driver.FindElement(By.CssSelector("#creationMonth > option:nth-child(2)"))
                    .Click();
                _driver.FindElement(By.CssSelector("#creationYear > option:nth-child(2)"))
                    .Click();
                _driver.FindElement(By.Id("idNumber"))
                    .SendKeys(COMPANY_VALID_ID);
                _driver.FindElement(By.Id("phoneNumber"))
                    .SendKeys(COMPANY_VALID_PHONE_NUMBER);
                _driver.FindElement(By.Id("email"))
                    .SendKeys(COMPANY_VALID_EMAIL);
                _driver.FindElement(By.Id("province"))
                    .SendKeys(COMPANY_VALID_PROVINCE);
                _driver.FindElement(By.Id("canton"))
                    .SendKeys(COMPANY_VALID_COUNTY);
                _driver.FindElement(By.Id("district"))
                    .SendKeys(COMPANY_VALID_DISTRICT);
                _driver.FindElement(By.Id("otherSigns"))
                    .SendKeys(COMPANY_VALID_EXTRA);
                _driver.FindElement(By.CssSelector("#benefits > option:nth-child(2)"))
                    .Click();
                _driver.FindElement(By.CssSelector("#paymentType > option:nth-child(2)"))
                    .Click();

                var companyButton = _driver.FindElement(By
                    .CssSelector("#app > div > div.card.p-4.mx-auto > form > div.d-flex.justify-content-center.mt-4 > button"));
                js.ExecuteScript("arguments[0].click();", companyButton);
                _wait.Until(driver => driver.Url.EndsWith(BASE_URL));
                Assert.That(_driver.Url, Is.EqualTo(BASE_URL));

                _driver.Navigate().GoToUrl(BASE_URL + LOGIN_URL);
                _wait.Until(driver => driver.Url.EndsWith(LOGIN_URL));
                Assert.That(_driver.Url, Is.EqualTo(BASE_URL + LOGIN_URL));

                _driver.FindElement(By.Id("userId"))
                    .SendKeys(ADMIN_USERNAME);
                _driver.FindElement(By.Id("userPassword"))
                    .SendKeys(ADMIN_PASSWORD);

                var loginButton = _driver.FindElement(By
                    .CssSelector("form button[type='submit']"));
                js.ExecuteScript("arguments[0].click();", loginButton);

                _wait.Until(driver => driver.Url.EndsWith(PROFILE_URL));
                Assert.That(_driver.Url, Is.EqualTo(BASE_URL + PROFILE_URL));

                _driver.Navigate().GoToUrl(BASE_URL + COMPANIES_URL);
                _wait.Until(driver => driver.Url.EndsWith(COMPANIES_URL));
                Assert.That(_driver.Url, Is.EqualTo(BASE_URL + COMPANIES_URL));

                _wait.Until(driver => driver.FindElement(By.TagName("tbody")));

                var tableBody = _driver.FindElement(By.TagName("tbody"));
                var rows = tableBody.FindElements(By.TagName("tr"));
                var foundCompany = false;
                var i = 0;

                while (i < rows.Count && !foundCompany)
                {
                    var row = rows[i];
                    var cells = row.FindElements(By.TagName("td"));
 
                    if (cells[0].Text.Contains(COMPANY_VALID_NAME))
                    {
                        var deleteButton = row.FindElement(By.XPath(".//button[contains(text(), 'Eliminar')]"));
                        js.ExecuteScript("arguments[0].click();", deleteButton);
                        foundCompany = true;
                    }
                    ++i;
                }

                if (!foundCompany)
                {
                    throw new Exception("Company is not being shown on the list of companies.");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        private void fillEmployerInput(string key, string value)
        {
            var formElement = _driver.FindElement(By.Id(key));
            formElement.Clear();
            formElement.SendKeys(value);
        }
    }
}
