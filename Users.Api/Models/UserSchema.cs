using GraphQL;
using GraphQL.Types;

namespace Users.Api.Models
{
    public class UserSchema : Schema
    {
        public UserSchema(IDependencyResolver resolver): base(resolver)
        {
            Query = resolver.Resolve<UsersQuery>();
            //Uncomment this to allow users mutations
            //Mutation = resolver.Resolve<UserMutation>();
        }
    }
}


 