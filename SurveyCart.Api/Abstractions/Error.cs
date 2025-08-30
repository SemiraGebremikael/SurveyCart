namespace SurveyCart.Api.Abstractions
{
    public record Error(string cod , string Dscription, int ? StatusCodes)
    {
        public static readonly Error None = new(string.Empty, string.Empty, null);
    }
   
}

