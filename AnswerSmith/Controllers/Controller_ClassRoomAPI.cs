using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.Data;
using AnswerSmith.DTOs;
using AnswerSmith.Interfaces;
using AnswerSmith.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnswerSmith.Controllers
{
    [ApiController]
    [Route("api/classroom")]
    public class Controller_ClassRoomAPI : ControllerBase
    {
        private readonly IClassHandler _classHandeler;
        private readonly IClassService _classService;
        public Controller_ClassRoomAPI()
        {
            _classHandeler = new Data_Class();
            _classService = new Service_ClassRoom(_classHandeler);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddClassroom([FromBody] DTO_Class dto_Class) {
            try {
                bool status = await _classService.AddClass(dto_Class);
                if (status) { return Ok(); }
                else { return BadRequest("Provided Data cannot be added."); }
            } catch (Exception e) {
                return BadRequest(e.Message);
            } 
        }


        [HttpPost("edit")]
        public async Task<IActionResult> EditClassroom([FromBody] DTO_Class dto_Class) {
            try {
                bool status = await _classService.EditClass(dto_Class);
                if (status) { return Ok(); }
                else { return BadRequest("Provided Data cannot be saved."); }
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        
        [HttpGet("view/{code}")]
        public async Task<IActionResult> GetClass([FromRoute] string code) {
            try {
                DTO_Class dto_Class = await _classService.GetClass(code);
                return Ok(dto_Class);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }


        [HttpGet("view")]
        public async Task<IActionResult> GetClasses(int pageNo = 1, int rowsPerPage = 20, string filter = "%", int orderBy = 3, int orderMode = 1) {
            try {
                Tuple<List<DTO_Class>, Model.Model_Pagination_CurrentPage> Classes = await _classService.GetClasses(pageNo, rowsPerPage, filter, orderBy, orderMode);
                return Ok(Classes);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }


    }
}