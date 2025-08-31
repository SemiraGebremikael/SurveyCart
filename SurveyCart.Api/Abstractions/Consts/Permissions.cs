namespace SurveyCart.Api.Abstractions.Consts;

public static class Permissions
{
    public static string Type { get; } = "Permissions";
    public  const string GetPolls = "polls:read";
    public const string AddPolls = "polls:add";
    public const string CreatePolls = "polls:create";
    public  const string UpdatePolls = "polls:update";
    public  const string DeletePolls = "polls:delete";

    public const string GetQuestion = "question:read";
    public const string AddQuestion = "question:add";
    public const string UpdateQuestion = "question:update";

    public const string GetUser = "user:read";
    public const string AddUser = "user:add";
    public const string UpdateUser = "user:update";

    public const string GetRole = "role:read";
    public const string AddRole = "role:add";
    public const string UpdateRole = "role:update";

    public const string Results = "results:read";


    public static IList<string> GetAllPermissions()=>
    typeof(Permissions)
     .GetFields(BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
     .Where(fi => fi.IsLiteral && !fi.IsInitOnly) 
     .Select(fi => fi.GetRawConstantValue()?.ToString()!)
     .ToList();

}
