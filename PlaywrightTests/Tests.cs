using NUnit.Framework;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace ContosoTraders
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        // This method grabs the configuration from the appsettings.test.json file
        public static IConfiguration InitConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json")
                .Build();
            return configuration;
        }

        [SetUp]
        // This method is called before each test to get the baseUrl from the configuration file
        public async Task SetUp()
        {
            var config = InitConfiguration();
            await Page.GotoAsync(config["baseUrl"]);
        }

        /* 
        Tests converted from TypeScript tests in the Contoso Traders repo
        https://github.com/microsoft/contosotraders-cloudtesting/tree/22eef00703aec6f8a73bffa67be002405c1a420d/src/ContosoTraders.Ui.Website/tests 
        */
        [Test]
        [Category("Header")]
        public async Task ShouldBeAbleToSearchByText()
        {
            await Page.GetByPlaceholder("Search by product name or search by image").FillAsync("laptops");
            await Page.GetByPlaceholder("Search by product name or search by image").PressAsync("Enter");
            await Expect(Page).ToHaveURLAsync(new Regex(".*suggested-products-list"));
        }

        [Test]
        [Category("Header")]
        public async Task ShouldBeAbleToSelectCategory()
        {
            await Page.GetByRole(AriaRole.Button, new() { Name = "All Categories" }).First.ClickAsync();
            await Page.Locator("#customized-menu").GetByText("Laptops").ClickAsync();
            await Expect(Page).ToHaveURLAsync(new Regex(".*laptops"));
        }

        [Test]
        [Category("Header")]
        public async Task ShouldBeAbleToHoverOverHeaderMenus()
        {
            await Page.GetByRole(AriaRole.Link, new() { Name = "All Products" }).First.HoverAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Laptops" }).First.HoverAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Controllers" }).First.HoverAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Mobiles" }).First.HoverAsync();
            await Page.GetByRole(AriaRole.Link, new() { Name = "Monitors" }).First.HoverAsync();
        }

        [Test]
        [Category("Header")]
        public async Task ShouldBeAbleToSelectHeaderMenu()
        {
            await Page.GetByRole(AriaRole.Link, new() { Name = "All Products" }).First.ClickAsync();
            await Expect(Page).ToHaveURLAsync(new Regex(".*all-products"));
        }
    }
}