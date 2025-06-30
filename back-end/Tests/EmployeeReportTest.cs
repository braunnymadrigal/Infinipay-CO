using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;

namespace Tests
{
  public class SeleniumEmployeeReportTest
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
    public void LoginAndAccessToEmployeeReport()
    {
      _driver.Navigate().GoToUrl("http://localhost:8080/LoginUser");

      // Nota: utilizar empleado de la base de datos compartida
      _driver.FindElement(By.Id("userId")).SendKeys("employeefront@gmail.com");
      _driver.FindElement(By.Id("userPassword")).SendKeys("Test17021988!");

      _driver.FindElement(By.CssSelector("form button[type='submit']")).Click();

      _wait.Until(driver => driver.Url.Contains("/MyProfile"));

      _driver.Navigate().GoToUrl("http://localhost:8080/ShowEmployeeReport");

      var titulo = _wait.Until(d => d.FindElement(By.TagName("h1")));

      Assert.IsTrue(titulo.Text.Contains("Reportes de pagos de planilla"));
    }

    [Test]
    public void LoginAndVerifyTheEmployeeReport()
    {
      _driver.Navigate().GoToUrl("http://localhost:8080/LoginUser");

      _driver.FindElement(By.Id("userId")).SendKeys("employeefront@gmail.com");
      _driver.FindElement(By.Id("userPassword")).SendKeys("Test17021988!");
      _driver.FindElement(By.CssSelector("form button[type='submit']")).Click();

      _wait.Until(driver => driver.Url.Contains("/MyProfile"));

      _driver.Navigate().GoToUrl("http://localhost:8080/ShowEmployeeReport");

      var reportContent = _wait.Until(driver =>
      {
        try
        {
          var element = driver.FindElement(By.CssSelector("div.container.mt-4"));
          return element.Displayed ? element : null;
        }
        catch (NoSuchElementException)
        {
          return null;
        }
      });

      Assert.IsNotNull(reportContent
        , "El contenedor del reporte no se encontró o no está visible.");

      var companyNameText =
        _driver.FindElement(By.XPath("//p[span/strong[contains(text()" +
        ", 'Empresa:')]]"));
      var employeeNameText =
        _driver.FindElement(By.XPath("//p[span/strong[contains(text()" +
        ", 'Nombre completo:')]]"));

      Assert.IsTrue(companyNameText.Text.Contains("Empresa:")
        , "Texto de empresa no encontrado.");
      Assert.IsTrue(employeeNameText.Text.Contains("Nombre completo:")
        , "Texto de nombre completo no encontrado.");
    }

  }
}