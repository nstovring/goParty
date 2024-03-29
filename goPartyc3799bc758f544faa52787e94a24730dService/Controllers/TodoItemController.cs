﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using goPartyc3799bc758f544faa52787e94a24730dService.DataObjects;
using goPartyc3799bc758f544faa52787e94a24730dService.Models;
using goPartyc3799bc758f544faa52787e94a24730dService.Helpers;

namespace goPartyc3799bc758f544faa52787e94a24730dService.Controllers
{
    public class TodoItemController : TableController<TodoItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            goPartyc3799bc758f544faa52787e94a24730dContext context = new goPartyc3799bc758f544faa52787e94a24730dContext();
            DomainManager = new EntityDomainManager<TodoItem>(context, Request);
        }

        // GET tables/TodoItem
        public IQueryable<TodoItem> GetAllTodoItems()
        {
            return Query();
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TodoItem> GetTodoItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
       // [AuthorizeClaims("http://schemas.microsoft.com/identity/claims/identityprovider", "facebook")]
        public Task<TodoItem> PatchTodoItem(string id, Delta<TodoItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
        //[AuthorizeClaims("http://schemas.microsoft.com/identity/claims/identityprovider", "facebook")]
        public async Task<IHttpActionResult> PostTodoItem(TodoItem item)
        {
            TodoItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959

       // [AuthorizeClaims("http://schemas.microsoft.com/identity/claims/identityprovider", "facebook")]
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}