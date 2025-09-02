namespace SurveyCart.Api.Authentication;
public interface IJwtProvider
{
    public  (string token, int expiresIn) GenerateToken(User user, IEnumerable<string> roles, IEnumerable<string> permissions);
    public string? ValidateToken(string token);

}
