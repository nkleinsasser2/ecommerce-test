using BuildingBlocks.Web;
using Microsoft.AspNetCore.Mvc.Testing;

public class CategoriesApiTests : IClassFixture<WebApplicationFactory<ECommerce.Categories.Api.Program>>
{
    private readonly WebApplicationFactory<ECommerce.Categories.Api.Program> _factory;

    public CategoriesApiTests(WebApplicationFactory<ECommerce.Categories.Api.Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("environment", "Test");
        });
    }

    [Fact]
    public async Task Get_Categories_Should_Return_Success()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"{EndpointConfig.BaseApiPath}/catalog/category");
        response.EnsureSuccessStatusCode();
    }
}
