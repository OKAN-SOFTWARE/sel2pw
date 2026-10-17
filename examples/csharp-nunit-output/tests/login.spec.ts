import { test, expect } from '@playwright/test';

test.describe('LoginTests', () => {
  test.beforeEach(async ({ page }) => {
    options.AddArgument("--headless=new");
    options.AddArgument("--no-sandbox");
    driver.Manage().Window.Maximize();
  });

  test.afterEach(async ({ page }) => {
    driver?.Quit();
  });

  test("ValidLoginLandsOnInventory", async ({ page }) => {
    loginPage.Open();
    loginPage.LoginAs("standard_user", "secret_sauce");

    Assert.That(inventoryPage.GetPageTitle(), Is.EqualTo("Products"));
    Assert.That(inventoryPage.IsInventoryVisible(), Is.True);
    Assert.That(inventoryPage.IsCartVisible(), Is.True);
  });

  test("LockedOutUserSeesError", async ({ page }) => {
    loginPage.Open();
    loginPage.LoginAs("locked_out_user", "secret_sauce");

    Assert.That(loginPage.GetErrorMessage(), Does.Contain("locked out"));
  });

});
