using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace Tests
{
  public class SeleniumDeleteBenefitTest
  {
    private IWebDriver _driver;
    private WebDriverWait _wait;


    [SetUp]
    public void Setup()
    {
      this._driver = new FirefoxDriver();
      this._driver.Manage().Window.Maximize();
      this._wait = new WebDriverWait(this._driver, TimeSpan.FromSeconds(10));
    }
    
    [Test]
    public void DeleteBenefit_From_BenefitList_As_Employer()
    {
      _driver.Navigate().GoToUrl("http://localhost:8080/LoginUser");

      _driver.FindElement(By.Id("userId")).SendKeys("sofiajones");
      _driver.FindElement(By.Id("userPassword")).SendKeys("Clave12345!");
      _driver.FindElement(By.CssSelector("form button[type='submit']")).Click();

      _wait.Until(driver => driver.Url.Contains("/MyProfile"));
      _driver.Navigate().GoToUrl("http://localhost:8080/BenefitList");

      _wait.Until(driver => driver.FindElements(By.TagName("tr"))
          .Any(tr => tr.Text.Contains("Beneficio A Eliminar Con Selenium")));

      var benefitList = _driver.FindElement(By.TagName("table"));
      var filas = benefitList.FindElements(By.TagName("tr"));

      bool encontrado = false;

      foreach (var fila in filas)
      {
        if (fila.Text.Contains("Beneficio A Eliminar Con Selenium"))
        {
          var eliminarButton = fila.FindElement(By.Id("deleteBenefitButton"));
          eliminarButton.Click();
          encontrado = true;
          break;
        }
      }

      if (!encontrado)
      {
        Assert.Fail("No se encontró el beneficio a eliminar en la lista.");
      }

      var modalDeleteButton = _wait.Until(d => d.FindElement(By.Id("confirmDeleteButton")));
      modalDeleteButton.Click();

      _wait.Until(driver => !driver.PageSource.Contains("Beneficio A Eliminar Con Selenium"));

      benefitList = _wait.Until(d => d.FindElement(By.TagName("table")));
      filas = benefitList.FindElements(By.TagName("tr"));

      bool borrado = true;

      foreach (var fila in filas)
      {
        if (fila.Text.Contains("Beneficio A Eliminar Con Selenium"))
        {
          borrado = false;
          break;
        }
      }

      Assert.IsTrue(borrado, "El beneficio aún aparece en la lista, no se eliminó correctamente.");
    }



    [TearDown]
    public void TearDown()
    {
      this._driver.Quit();
      _driver.Dispose();
      _driver = null;
    }
  }
}