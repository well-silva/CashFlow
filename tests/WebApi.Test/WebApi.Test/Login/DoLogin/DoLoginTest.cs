using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string _url = "/api/login";
    private readonly HttpClient _httpClient;
    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _email = webApplicationFactory.GetEmail();
        _name = webApplicationFactory.GetName();
        _password = webApplicationFactory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = new RequestLogin
        {
            Email = _email,
            Password = _password
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync(_url, request);
        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseData.RootElement.GetProperty("name").GetString().Should().Be(_name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorLoginInvalid(string culture)
    {
        // Arrange
        var request = RequestLoginBuilder.Build();

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));

        // Act
        var response = await _httpClient.PostAsJsonAsync(_url, request);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        // Assert
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

}
