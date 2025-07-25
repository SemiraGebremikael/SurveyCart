namespace SurveyCart.Api.Abstractions.Consts
{
    public  static class RegexPatterns
    {
        public const string Password = "(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{8,}";
    }
}
