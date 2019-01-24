using GraphQL.Types;
using Users.Core.Interfaces;
using System;

namespace Users.Api.Models
{
    public class UsersQuery : ObjectGraphType
    {
        public UsersQuery(IUserRepository userRepository)
        {
            Field<UserType>(
                "user",
                arguments: new QueryArguments(new QueryArgument<IdGraphType> { Name = "id"}),
                resolve: context => userRepository.Get(context.GetArgument<Guid>("id")));

            Field<ListGraphType<UserType>>(
                "users",
                resolve: context => userRepository.All());
        }
    }
}
