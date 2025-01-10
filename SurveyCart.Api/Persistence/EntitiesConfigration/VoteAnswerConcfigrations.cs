
namespace SurveyCart.Api.Persistence.EntitiesConfigration;
public class VoteAnswerConcfigrations : IEntityTypeConfiguration<VoteAnswer>
{
    public void Configure(EntityTypeBuilder<VoteAnswer> builder)
    {
        builder.HasIndex(x =>  new { x.VoteId, x.QuestionId }).IsUnique();


    }

}
