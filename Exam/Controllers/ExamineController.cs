using Exam.Dto.Forms;
using Exam.Dto.Results;
using Exam.Models;
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
    public class ExamineController : ControllerBase
    {
        [HttpGet("{id}")]
        public ExaminationDetail Get(string id)
        {
            var examination = Examination.Find(id);

            if (examination == null)
                return null;

            return new ExaminationDetail(examination);
        }

        [HttpPost]
        public ExamineForm Post(ExamineForm form)
        {
            return form;
        }
    }
}
