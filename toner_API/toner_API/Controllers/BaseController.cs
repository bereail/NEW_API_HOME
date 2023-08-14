using Microsoft.AspNetCore.Mvc;
using toner_API.Models;

namespace toner_API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly tonerStoreContext _dbContext;

        public BaseController(tonerStoreContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
