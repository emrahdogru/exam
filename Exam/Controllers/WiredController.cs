using Exam.Dto.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WiredController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<WiredSummary> Get()
        {
            return WiredSummary.GetWiredSummaries().Take(5);
        }
    }
}
