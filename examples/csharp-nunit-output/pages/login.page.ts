import { Page, Locator, expect } from '@playwright/test';

export class LoginPage {
  readonly page: Page;
  readonly UsernameInput: Locator;
  readonly PasswordInput: Locator;
  readonly LoginButton: Locator;
  readonly ErrorMessage: Locator;

  constructor(page: Page) {
    this.page = page;
    this.UsernameInput = page.locator("#user-name");
    this.PasswordInput = page.locator("#password");
    this.LoginButton = page.locator("#login-button");
    this.ErrorMessage = page.locator("[data-test='error']");
  }

  async Open(): Promise<void> {
    await this.page.goto("https://www.saucedemo.com/");
  }

  async EnterUsername(username: string): Promise<void> {
    await this.UsernameInput.fill(username);
  }

  async EnterPassword(password: string): Promise<void> {
    await this.PasswordInput.fill(password);
  }

  async ClickLogin(): Promise<void> {
    await this.LoginButton.click();
  }

  async GetErrorMessage(): Promise<string> {
    return await this.ErrorMessage.innerText();
  }

  async LoginAs(username: string, password: string): Promise<void> {
    await this.EnterUsername(username);
    await this.EnterPassword(password);
    await this.ClickLogin();
  }
}
