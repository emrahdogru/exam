using Exam.Dto.Forms;
using Exam.Dto.Results;
using Exam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Exam.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExaminationsController : ControllerBase
    {
        // GET: api/<ExaminationsController>
        [HttpGet]
        public IEnumerable<ExaminationSummary> Get()
        {
            return Examination.All().ToArray().Select(x => new ExaminationSummary(x));
        }

        // GET api/<ExaminationsController>/5
        [HttpGet("{id}")]
        public ExaminationForm Get(ObjectId id)
        {
            var examination = Examination.Find(id);

            if (examination == null)
                return null;

            return new ExaminationForm(examination);
        }

        // POST api/<ExaminationsController>
        [HttpPost]
        public void Post([FromBody] ExaminationForm value)
        {
            var examination = value.GetBinded();
            examination.Save();
        }


        // DELETE api/<ExaminationsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
