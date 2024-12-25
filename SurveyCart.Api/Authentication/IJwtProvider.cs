namespace SurveyCart.Api.Authentication;
public interface IJwtProvider
{
    public  (string token, int expiresIn) GenerateToken(User user);
    public string? ValidateToken(string token);

}
