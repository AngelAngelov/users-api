using System;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Models;
using Users.Core.Models;

namespace Users.Api.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("[controller]")]
    public class GraphQLController : Controller
    {
        /*
         * Controller allows CORS
         * Response is in JSON format
         * Route - /graphql/
         */

        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
        }

        /*
         * Public mothod using graphql. 
         * Allows queries for retrieving users data. 
         * Mutations are not allowed. If you want tot turn Mutation on 
         * uncomment line 12 in Models/UserSchema.cs
         */
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            ExecutionOptions executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = query.Variables.ToInputs()
            };

            ExecutionResult result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
