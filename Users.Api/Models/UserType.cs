using GraphQL.Types;
using Users.Core.Models;

namespace Users.Api.Models
{
    public class UserType : ObjectGraphType<User>
    {
        public UserType()
        {
            Name = "User";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the user.");
            Field(x => x.Email, true).Description("User's email address");
            Field(x => x.GivvenName, true).Description("The user's first name");
            Field(x => x.FamilyName, true).Description("The user's last name");
            Field(x => x.Created, type: typeof(DateGraphType)).Description("The date and time the user was added");
        }
    }
}
