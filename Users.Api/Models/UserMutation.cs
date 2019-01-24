using GraphQL.Types;
using System;
using Users.Core.Interfaces;
using Users.Core.Models;

namespace Users.Api.Models
{
    public class UserMutation : ObjectGraphType
    {
        public UserMutation(IUserRepository userRepository)
        {
            Name = "UsersMutations";

            Field<UserType>(
                "createUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                        var user = context.GetArgument<UserCreate>("user");
                        return userRepository.Add(user);
                });

            Field<UserType>(
                "editUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<UserInputType>> { Name = "user" }
                ),
                resolve: context =>
                {
                    var user = context.GetArgument<User>("user");

                    if (user.Id == null)
                    {
                        context.Errors.Add(new GraphQL.ExecutionError("User id missing"));
                    }

                    return userRepository.Edit(user);
                });

            Field<UserType>(
                "deleteUser",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }
                ),
                resolve: context =>
                {
                    return userRepository.Remove(context.GetArgument<Guid>("id"));
                });
        }
    }
}
