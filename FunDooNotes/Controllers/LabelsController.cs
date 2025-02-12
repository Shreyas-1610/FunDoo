using ManagerLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Context;

namespace FunDooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly FunDooDbContext context;
        private readonly ILabelsManager manager;

        public LabelsController(FunDooDbContext context, ILabelsManager manager)
        {
            this.context = context;
            this.manager = manager;
        }


    }
}
