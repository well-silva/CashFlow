﻿using CashFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Delete;
public class DeleteExpenseTest : CashFlowClassFixture
{
    private const string Url = "/api/expenses";

    private readonly string _token;
    private readonly long _expenseId;

    public DeleteExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.UserTeamMember.GetToken();
        _expenseId = webApplicationFactory.Expense.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete(requestUri: $"{Url}/{_expenseId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        result = await DoGet(requestUri: $"{Url}/{_expenseId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorExpenseNotFound(string culture)
    {
        var result = await DoDelete(requestUri: $"{Url}/1000", token: _token, culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages
                                .ResourceManager
                                .GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
