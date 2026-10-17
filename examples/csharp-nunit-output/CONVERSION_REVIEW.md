# Conversion Review

Source: `/sessions/blissful-stoic-mendel/mnt/Converter/examples/csharp-nunit-target`
Output: `/sessions/blissful-stoic-mendel/mnt/Converter/examples/csharp-nunit-output`

## Summary

- Files scanned: **3**
- Page Objects converted: **2**
- Test classes converted: **1**
- Test methods converted: **2**
- Review items: **1**
  - manual: 0
  - warning: 0
  - info: 1

## Severity legend

- **manual** — auto-conversion not possible; you must rewrite this section.
- **warning** — converted but please double-check semantics.
- **info** — heads-up about a non-trivial mapping (e.g. WebDriverWait removed).

## Items by file

### Project-wide

| Severity | Note |
| --- | --- |
| info | Source stack: csharp-nunit. Detected 3 C# files (no .feature files) — using NUnit/MSTest → Playwright Test path. |

## Cheatsheet — Selenium → Playwright

| Selenium / TestNG | Playwright TS |
| --- | --- |
| `driver.get(url)` | `await page.goto(url)` |
| `driver.findElement(By.id("x")).click()` | `await page.locator('#x').click()` |
| `element.sendKeys("...")` | `await locator.fill('...')` |
| `element.getText()` | `await locator.innerText()` |
| `Assert.assertEquals(a, b)` | `expect(a).toBe(b)` |
| `@Test` | `test('...', async ({ page }) => { ... })` |
| `@BeforeMethod` | `test.beforeEach(...)` |
| `@DataProvider` | parameterised loop over rows |
| `WebDriverWait.until(...)` | _removed — Playwright auto-waits_ |
| `JavascriptExecutor.executeScript(js)` | `await page.evaluate(() => js)` |
| `Actions(driver).moveToElement(el).perform()` | `await locator.hover()` |
