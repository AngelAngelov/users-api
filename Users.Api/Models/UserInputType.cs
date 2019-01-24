using GraphQL.Types;

namespace Users.Api.Models
{
    public class UserInputType : InputObjectGraphType
    {
        public UserInputType()
        {
            Name = "UserInput";
            Field<IdGraphType>("id");
            Field<NonNullGraphType<StringGraphType>>("givvenName");
            Field<NonNullGraphType<StringGraphType>>("familyName");
            Field<NonNullGraphType<StringGraphType>>("email");
        }
    }
}
