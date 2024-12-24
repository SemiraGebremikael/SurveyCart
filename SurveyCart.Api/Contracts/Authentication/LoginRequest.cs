using System.Globalization;

namespace SurveyCart.Api.Contracts.Authentication
{
    public record LoginRequest(
        string Email,
        string Password);
}
