using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using ProductStoreCSharp.TestSource;
using System.Text.RegularExpressions;

namespace ProductStoreCSharp
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class LoginTest : PageTest
    {
        // Test Data
        private static readonly string BASE_URL = Environment.GetEnvironmentVariable("BASE_URL") ?? "https://www.demoblaze.com";

        // Selectors
        private static readonly string loginLink = "#login2";
        private static readonly string usernameInput = "#loginusername";
        private static readonly string passwordInput = "#loginpassword";
        private static readonly string loginButton = "button:has-text(\"Log in\")";
        private static readonly string userGreeting = "#nameofuser";

        private static async Task LoginAsync(IPage page, string username, string password)
        {
            await page.ClickAsync(loginLink);
            await page.FillAsync(usernameInput, username);
            await page.FillAsync(passwordInput, password);
            await page.ClickAsync(loginButton);
        }

        [TestCaseSource(typeof(LoginTestSource), nameof(LoginTestSource.SuccessfulLoginShowsWelcomeMessageTestData))]
        public async Task SuccessfulLoginShowsWelcomeMessage(string username, string password)
        {
            await Page.GotoAsync(BASE_URL);
            await LoginAsync(Page, username, password);

            var userElement = Page.Locator(userGreeting);
            await Expect(userElement).ToBeVisibleAsync();
            await Expect(userElement).ToHaveTextAsync(new Regex($"Welcome {username}"));
        }

        [TestCaseSource(typeof(LoginTestSource), nameof(LoginTestSource.FailedLoginShowsWarning))]
        public async Task FailedLoginShowsWarning(string username, string password)
        {
            await Page.GotoAsync(BASE_URL);
            await LoginAsync(Page, username, password);

            Page.Dialog += async (_, dialog) => await dialog.DismissAsync();

            await Page.ClickAsync(loginButton);

            await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Log in" })).ToBeVisibleAsync();
            await Expect(Page.GetByRole(AriaRole.Link, new() { Name = "Sign up" })).ToBeVisibleAsync();
        }
    }
}
