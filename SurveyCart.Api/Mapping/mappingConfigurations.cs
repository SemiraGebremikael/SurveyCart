using SurveyCart.Api.Contracts.Questions;

namespace SurveyCart.Api.Mapping
{
    public class mappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<QuestionRequest, Question>()
                .Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer { Content = answer }));

            config.NewConfig<RegisterRequest, User>()
                .Map(dest => dest.UserName, src => src.Email);


        }
    }
}
