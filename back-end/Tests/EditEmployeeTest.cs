using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using Microsoft.IdentityModel.Tokens;
using back_end.Application;

namespace Tests
{
  public class SeleniumEditEmployeeTest
  {
    private IWebDriver driver;
    private WebDriverWait wait;

    [SetUp]
    public void Setup()
    {
      driver = new EdgeDriver();
      driver.Manage().Window.Maximize();
      driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);


      wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
    }

    private void Login(string user, string password)
    {
      driver.Navigate().GoToUrl("http://localhost:8080/LoginUser");

      driver.FindElement(By.Id("userId")).SendKeys(user);
      driver.FindElement(By.Id("userPassword")).SendKeys(password);

      driver.FindElement(By.CssSelector("form button[type='submit']")).Click();
    }

    [TearDown]
    public void TearDown()
    {
      driver.Dispose();
    }

    [Test]
    public void LoginAndAccessEditEmployeePage() {
      Login("daniel.gomez", "Garatos24!");
      
      var employeeListBtn = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.container.my-5 > header > nav > div > div" +
        " > a:nth-child(6)")));

      var employeeListBtnTitle = employeeListBtn.Text;

      employeeListBtn.Click();

      var employeeList = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.container.mt-5.mb-5 > h2")));

      var employeeListTitle = employeeList.Text;

      wait.Until(driver => driver.Url.Contains("/EmployeesList"));

      var editBtn = wait.Until(driver => driver.FindElement(By.CssSelector(
      "#app > div.container.mt-5.mb-5 > table > tbody > tr > td:nth-child(6)" +
     " > a")));

      var editBtnText = editBtn.Text;

      editBtn.Click();

      var editTitle = wait.Until(driver => driver.FindElement(By.CssSelector(
       "#app > div:nth-child(2) > div > h2")));

      var editTitleText = editTitle.Text;

      Assert.IsTrue(employeeListBtnTitle.Contains("Empleados"));
      Assert.IsTrue(employeeListTitle.Contains("Lista de Empleados"));
      Assert.IsTrue(editBtnText.Contains("Editar"));
      Assert.IsTrue(editTitleText.Contains("Editar datos de perfil" +
       " para tu empleado"));
    }

    [Test]
    public void LoginAndVerifyEmployeeData()
    {
      Login("daniel.gomez", "Garatos24!");

      var employeeListBtn = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.container.my-5 > header > nav" +
        " > div > div > a:nth-child(6)")));

      employeeListBtn.Click();

      var detailsBtn = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr >" +
        " td:nth-child(6) > button")));

      var detailsBtnText = detailsBtn.Text;

      detailsBtn.Click();

      var name = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.modal.fade.show.d-block > div > div > div.modal-body >" +
        " p:nth-child(1)"))).Text;

      var id = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.modal.fade.show.d-block > div > div > div.modal-body >" +
        " p:nth-child(2)"))).Text;

      var email = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.modal.fade.show.d-block > div > div > div.modal-body >" +
        " p:nth-child(3)"))).Text;

      var phoneNumber = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.modal.fade.show.d-block > div > div > div.modal-body >" +
        " p:nth-child(4)"))).Text;

      var role = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.modal.fade.show.d-block > div > div > div.modal-body >" +
        " p:nth-child(5)"))).Text;

      var province = wait.Until(driver => driver.FindElement(By.CssSelector(
        "#province"))).Text;

      var district = wait.Until(driver => driver.FindElement(By.CssSelector(
        "#district"))).Text;

      var canton = wait.Until(driver => driver.FindElement(By.CssSelector(
        "#canton"))).Text;

      var otherSigns = wait.Until(driver => driver.FindElement(By.
        CssSelector("#otherSigns"))).Text;

      var closeDetailsBtn = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.modal.fade.show.d-block > div > div > div.modal-footer >" +
        " button")));

      closeDetailsBtn.Click();

      var editBtn = wait.Until(driver => driver.FindElement(By.CssSelector(
      "#app > div.container.mt-5.mb-5 > table > tbody > tr > td:nth-child(6)" +
     " > a")));

      editBtn.Click();

      var firstNameEdit = wait.Until(driver => driver.FindElement(
        By.CssSelector("#firstName"))).Text;

      var secondNameEdit = wait.Until(driver => driver.FindElement(
        By.CssSelector("#secondName"))).Text;

      var firstLastNameEdit = wait.Until(driver => driver.FindElement(
        By.CssSelector("#firstLastName"))).Text;

      var secondLastNameEdit = wait.Until(driver => driver.FindElement
      (By.CssSelector("#secondLastName"))).Text;

      var idEdit = wait.Until(driver => driver.FindElement(By.CssSelector(
      "#idNumber"))).Text;

      var roleEdit = wait.Until(driver => driver.FindElement(By.CssSelector(
      "#role"))).Text;

      var phoneNumberEdit = wait.Until(driver => driver.FindElement(By
        .CssSelector("#phoneNumber"))).Text;

      var emailEdit = wait.Until(driver => driver.FindElement(By.CssSelector(
      "#email"))).Text;

      var provinceEdit = wait.Until(driver => driver.FindElement(By.CssSelector(
      "#province"))).Text;

      var cantonEdit = wait.Until(driver => driver.FindElement(By.CssSelector(
      "#canton"))).Text;

      var districtEdit = wait.Until(driver => driver.FindElement(By.CssSelector(
      "#district"))).Text;

      var otherSignsEdit = wait.Until(driver => driver.FindElement(
        By.CssSelector("#otherSigns"))).Text;

      Assert.IsTrue(name.Contains(firstNameEdit));

      if (!secondNameEdit.IsNullOrEmpty())
      {
        Assert.IsTrue(name.Contains(secondNameEdit));
      }

      Assert.IsTrue(detailsBtnText.Contains("Ver detalles"));

      Assert.IsTrue(name.Contains(firstLastNameEdit));
      Assert.IsTrue(name.Contains(secondLastNameEdit));
      Assert.IsTrue(id.Contains(idEdit));
      Assert.IsTrue(phoneNumber.Contains(phoneNumberEdit));
      Assert.IsTrue(email.Contains(emailEdit));
      Assert.IsTrue(district.Contains(districtEdit));
      Assert.IsTrue(canton.Contains(cantonEdit));
      Assert.IsTrue(province.Contains(provinceEdit));
      Assert.IsTrue(otherSigns.Contains(otherSigns));

      Assert.IsFalse(role.Contains(roleEdit));
    }

    [Test]
    public void LoginAndEditFistName()
    {
      var newName = "Pepe";

      Login("daniel.gomez", "Garatos24!");

      var employeeListBtn = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.container.my-5 > header > nav" +
        " > div > div > a:nth-child(6)")));

      employeeListBtn.Click();

      var detailsBtn = wait.Until(driver => driver.FindElement(By.
      CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr >" +
        " td:nth-child(6) > button")));

      detailsBtn.Click();

      var oldFirstName = wait.Until(driver => driver.FindElement(
        By.CssSelector("#app > div.modal.fade.show.d-block > div > div >" +
        " div.modal-body > p:nth-child(1)"))).Text;

      var closeDetailsBtn = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.modal.fade.show.d-block > div > div > div.modal-footer >" +
        " button")));

      closeDetailsBtn.Click();

      var editBtn = wait.Until(driver => driver.FindElement(By.CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr > td:nth-child(6)"+
        " > a")));

      editBtn.Click();

      Thread.Sleep(5000);

      var firstNameEdit = wait.Until(driver => driver.FindElement(
        By.CssSelector("#firstName")));

      firstNameEdit.Clear();
      firstNameEdit.SendKeys(newName);

      var saveBtn = wait.Until(driver => driver.FindElement(By.CssSelector(
        "#app > div:nth-child(2) > div >" +
        " form > div.d-flex.justify-content-center > button")));

      ((IJavaScriptExecutor)driver)
          .ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline:" +
          " 'nearest'});", saveBtn);

      Thread.Sleep(5000);

      saveBtn.Click();

      detailsBtn = wait.Until(driver => driver.FindElement(By.
      CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr >" +
        " td:nth-child(6) > button")));

      detailsBtn.Click();

      var newFirstName = wait.Until(driver => driver.FindElement(
        By.CssSelector("#app > div.modal.fade.show.d-block > div > div >" +
        " div.modal-body > p:nth-child(1)"))).Text;

      Assert.IsFalse(oldFirstName.Contains(newName));
      Assert.IsFalse(oldFirstName.Contains(newFirstName));
      Assert.IsTrue(newFirstName.Contains(newName));
    }


    [Test]
    public void LoginAndEditSecondName()
    {
      var newName = "Roberto";

      Login("daniel.gomez", "Garatos24!");

      var employeeListBtn = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.container.my-5 > header > nav" +
        " > div > div > a:nth-child(6)")));

      employeeListBtn.Click();

      var detailsBtn = wait.Until(driver => driver.FindElement(By.
      CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr >" +
        " td:nth-child(6) > button")));

      detailsBtn.Click();

      var oldSecondName = wait.Until(driver => driver.FindElement(
        By.CssSelector("#app > div.modal.fade.show.d-block > div > div >" +
        " div.modal-body > p:nth-child(1)"))).Text;

      var closeDetailsBtn = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.modal.fade.show.d-block > div > div > div.modal-footer >" +
        " button")));

      closeDetailsBtn.Click();

      var editBtn = wait.Until(driver => driver.FindElement(By.CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr > td:nth-child(6)" +
        " > a")));

      editBtn.Click();
      Thread.Sleep(5000);

      var secondNameEdit = wait.Until(driver => driver.FindElement(
        By.CssSelector("#secondName")));

      secondNameEdit.Clear();
      secondNameEdit.SendKeys(newName);

      var saveBtn = wait.Until(driver => driver.FindElement(By.CssSelector(
        "#app > div:nth-child(2) > div >" +
        " form > div.d-flex.justify-content-center > button")));

      ((IJavaScriptExecutor)driver)
          .ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline:" +
          " 'nearest'});", saveBtn);

      Thread.Sleep(5000);

      saveBtn.Click();

      detailsBtn = wait.Until(driver => driver.FindElement(By.
      CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr >" +
        " td:nth-child(6) > button")));

      detailsBtn.Click();

      var newSecondName = wait.Until(driver => driver.FindElement(
        By.CssSelector("#app > div.modal.fade.show.d-block > div > div >" +
        " div.modal-body > p:nth-child(1)"))).Text;

      Assert.IsFalse(oldSecondName.Contains(newName));
      Assert.IsFalse(oldSecondName.Contains(newSecondName));
      Assert.IsTrue(newSecondName.Contains(newName));
    }

    [Test]
    public void LoginAndEditId()
    {
      var newUserId = "119509990";

      Login("daniel.gomez", "Garatos24!");

      var employeeListBtn = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.container.my-5 > header > nav" +
        " > div > div > a:nth-child(6)")));

      employeeListBtn.Click();

      var detailsBtn = wait.Until(driver => driver.FindElement(By.
      CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr >" +
        " td:nth-child(6) > button")));

      detailsBtn.Click();

      var oldId = wait.Until(driver => driver.FindElement(
        By.CssSelector("#app > div.modal.fade.show.d-block > div > div > " +
        "div.modal-body > p:nth-child(2)"))).Text;

      var closeDetailsBtn = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.modal.fade.show.d-block > div > div > div.modal-footer >" +
        " button")));

      closeDetailsBtn.Click();

      var editBtn = wait.Until(driver => driver.FindElement(By.CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr > td:nth-child(6)" +
        " > a")));

      editBtn.Click();
      Thread.Sleep(5000);
      var idEdit = wait.Until(driver => driver.FindElement(
        By.CssSelector("#idNumber")));

      idEdit.Clear();
      idEdit.SendKeys(newUserId);

      var saveBtn = wait.Until(driver => driver.FindElement(By.CssSelector(
        "#app > div:nth-child(2) > div >" +
        " form > div.d-flex.justify-content-center > button")));

      ((IJavaScriptExecutor)driver)
          .ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline:" +
          " 'nearest'});", saveBtn);

      Thread.Sleep(5000);

      saveBtn.Click();

      detailsBtn = wait.Until(driver => driver.FindElement(By.
      CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr >" +
        " td:nth-child(6) > button")));

      detailsBtn.Click();

      var newId = wait.Until(driver => driver.FindElement(
        By.CssSelector("#app > div.modal.fade.show.d-block > div > div" +
        " > div.modal-body > p:nth-child(2)"))).Text;

      Console.WriteLine(newUserId);
      Console.WriteLine(oldId);
      Console.WriteLine(newId);

      Assert.IsFalse(oldId.Contains(newUserId));
      Assert.IsFalse(oldId.Contains(newId));
      Assert.IsTrue(newId.Contains(newUserId));
    }

    [Test]
    public void LoginAndEditPhoneNumber()
    {
      var newPhoneNumber = "45975345";

      Login("daniel.gomez", "Garatos24!");

      var employeeListBtn = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.container.my-5 > header > nav" +
        " > div > div > a:nth-child(6)")));

      employeeListBtn.Click();

      var detailsBtn = wait.Until(driver => driver.FindElement(By.
      CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr >" +
        " td:nth-child(6) > button")));

      detailsBtn.Click();

      var oldPhoneNumber = wait.Until(driver => driver.FindElement(
        By.CssSelector("#app > div.modal.fade.show.d-block > div > div >" +
        " div.modal-body > p:nth-child(4)"))).Text;

      var closeDetailsBtn = wait.Until(driver => driver.FindElement(By.
        CssSelector(
        "#app > div.modal.fade.show.d-block > div > div > div.modal-footer >" +
        " button")));

      closeDetailsBtn.Click();

      var editBtn = wait.Until(driver => driver.FindElement(By.CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr > td:nth-child(6)" +
        " > a")));

      editBtn.Click();

      Thread.Sleep(5000);

      var idEdit = wait.Until(driver => driver.FindElement(
        By.CssSelector("#phoneNumber")));

      idEdit.Clear();
      idEdit.SendKeys(newPhoneNumber);

      var saveBtn = wait.Until(driver => driver.FindElement(By.CssSelector(
        "#app > div:nth-child(2) > div >" +
        " form > div.d-flex.justify-content-center > button")));

      ((IJavaScriptExecutor)driver)
          .ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline:" +
          " 'nearest'});", saveBtn);

      Thread.Sleep(5000);

      saveBtn.Click();

      detailsBtn = wait.Until(driver => driver.FindElement(By.
      CssSelector(
        "#app > div.container.mt-5.mb-5 > table > tbody > tr >" +
        " td:nth-child(6) > button")));

      detailsBtn.Click();

      var newPhone = wait.Until(driver => driver.FindElement(
        By.CssSelector("#app > div.modal.fade.show.d-block > div > div" +
        " > div.modal-body > p:nth-child(4)"))).Text;

      Assert.IsFalse(oldPhoneNumber.Contains(newPhoneNumber));
      Assert.IsFalse(oldPhoneNumber.Contains(newPhone));
      Assert.IsTrue(newPhone.Contains(newPhoneNumber));
    }
  }
}