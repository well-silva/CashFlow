using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : CashFlowClassFixture
{
    private const string _url = "/api/login";
    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _email = webApplicationFactory.UserTeamMember.GetEmail();
        _name = webApplicationFactory.UserTeamMember.GetName();
        _password = webApplicationFactory.UserTeamMember.GetPassword();
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
        var response = await DoPost(requestUri:_url, request:request);
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

        // Act
        var response = await DoPost(requestUri:_url, request: request, culture: culture);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

        // Assert
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }

}
