import { Page, Locator, expect } from '@playwright/test';

export class InventoryPage {
  readonly page: Page;
  readonly PageTitle: Locator;
  readonly FirstInventoryItem: Locator;
  readonly CartLink: Locator;

  constructor(page: Page) {
    this.page = page;
    this.PageTitle = page.locator(".title");
    this.FirstInventoryItem = page.locator(".inventory_item");
    this.CartLink = page.locator("#shopping_cart_container");
  }

  async GetPageTitle(): Promise<string> {
    return await this.PageTitle.innerText();
  }

  async IsInventoryVisible(): Promise<bool> {
    return await this.FirstInventoryItem.isVisible();
  }

  async IsCartVisible(): Promise<bool> {
    return await this.CartLink.isVisible();
  }
}
