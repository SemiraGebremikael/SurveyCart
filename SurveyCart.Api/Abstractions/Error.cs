namespace SurveyCart.Api.Abstractions
{
    public record Error(string cod , string Dscription)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
    }
   
}
