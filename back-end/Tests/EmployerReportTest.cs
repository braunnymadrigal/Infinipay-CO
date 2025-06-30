using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

namespace Tests
{
  public class SeleniumEmployerReportTest
  {
    private IWebDriver _driver;
    private WebDriverWait _wait;

    [SetUp]
    public void Setup()
    {
      _driver = new EdgeDriver();
      _driver.Manage().Window.Maximize();
      _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
    }

    [TearDown]
    public void TearDown()
    {
      _driver.Quit();
    }

    [Test]
    public void LoginAndAccessToEmployerReport()
    {
      _driver.Navigate().GoToUrl("http://localhost:8080/LoginUser");

      _driver.FindElement(By.Id("userId")).SendKeys("testingemployer");
      _driver.FindElement(By.Id("userPassword")).SendKeys("testingEmployer123!");

      _driver.FindElement(By.CssSelector("form button[type='submit']")).Click();

      _wait.Until(driver => driver.Url.Contains("/MyProfile"));

      _driver.Navigate().GoToUrl("http://localhost:8080/ShowEmployerReport");

      var pageTitle = _wait.Until(driver =>
      {
        var h1 = driver.FindElement(By.TagName("h1"));
        return h1.Displayed ? h1 : null;
      });

      Assert.IsTrue(pageTitle.Text.Contains("Reporte de pagos del empleador"),
        "El título del reporte no se muestra correctamente.");
    }

    [Test]
    public void LoginAndVerifyEmployerReport()
    {
      _driver.Navigate().GoToUrl("http://localhost:8080/LoginUser");

      _driver.FindElement(By.Id("userId")).SendKeys("testingemployer");
      _driver.FindElement(By.Id("userPassword")).SendKeys("testingEmployer123!");
      _driver.FindElement(By.CssSelector("form button[type='submit']")).Click();

      _wait.Until(d => d.Url.Contains("/MyProfile"));

      _driver.Navigate().GoToUrl("http://localhost:8080/ShowEmployerReport");

      var reportContent = _wait.Until(driver =>
      {
        try
        {
          var element = driver.FindElement(By.CssSelector("div.mx-auto"));
          return element.Displayed ? element : null;
        }
        catch (NoSuchElementException)
        {
          return null;
        }
      });

      Assert.IsNotNull(reportContent
        , "No se encontró el contenedor del reporte del empleador.");
      Assert.IsTrue(reportContent.Displayed
        , "El contenedor del reporte no está visible.");

      var companyText =
        _driver.FindElement(By.XPath("//p[strong[contains(text()" +
        ",'Empresa:')]]"));
      var employerName =
        _driver.FindElement(By.XPath("//p[strong[contains(text()" +
        ",'Nombre del empleador:')]]"));

      Assert.IsTrue(companyText.Text.Contains("Empresa:")
        , "No se encontró el texto 'Empresa'.");
      Assert.IsTrue(employerName.Text.Contains("Nombre del empleador:")
        , "No se encontró el texto 'Nombre del empleador'.");

      var totalSalaries = _driver.FindElement(By.XPath("//p[span[contains(text()" +
        ",'Total salarios:')]]"));

      var totalCost = _driver.FindElement(By.XPath("//p[span[contains(text()" +
        ",'Costo total del empleador:')]]"));

      Assert.IsTrue(totalSalaries.Text.Contains("Total salarios")
        , "Total salarios no mostrado.");
      Assert.IsTrue(totalCost.Text.Contains("Costo total del empleador")
        , "Costo total del empleador no mostrado.");
    }

  }
}