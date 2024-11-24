using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnswerSmith.Data;
using AnswerSmith.DTOs;
using AnswerSmith.Enums;
using AnswerSmith.Interfaces;
using AnswerSmith.Model;
using AnswerSmith.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnswerSmith.Controllers
{
    [ApiController]
    [Route("api/subject")]
    public class Controller_SubjectAPI : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        private readonly ISubjectHandler _subjectHandler;
        public Controller_SubjectAPI()
        {
            _subjectHandler = new Data_Subject();
            _subjectService = new Service_Subject(_subjectHandler);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddSubject([FromBody] DTO_Subject dto_Subject) {
            try {
                bool status = await _subjectService.AddSubject(dto_Subject);
                if (status) { return Ok(); }
                else { return BadRequest("Provided Data cannot be added."); }
            } catch (Exception e) {
                return BadRequest(e.Message);
            } 
        }


        [HttpPost("edit")]
        public async Task<IActionResult> EditSubject([FromBody] DTO_Subject dto_Subject) {
            try {
                bool status = await _subjectService.EditSubject(dto_Subject);
                if (status) { return Ok(); }
                else { return BadRequest("Provided Data cannot be saved."); }
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }

        
        [HttpGet("view/{code}")]
        public async Task<IActionResult> GetSubject([FromRoute] string code) {
            try {
                DTO_Subject_Detail dto_SubjectDetail = await _subjectService.GetSubject(code);
                return Ok(dto_SubjectDetail);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
        }


        // [HttpGet("view")]
        // public async Task<IActionResult> GetSubjects(string searchKeyword = "%",
        //     int? searchColumn = null,
        //     string filterKeyword = "%",
        //     string? filterColumn = null,
        //     string? where_Clause = null,
        //     int orderBy = (int) Enum_Class_OrderBy.Name, 
        //     int orderMode = (int) Enum_Any_OrderMode.ASC,
        //     string? orderBy_Clause = null,

        //     int pageNo = 1,
        //     int rowsPerPage = 20) {
        //     try {
        //         Tuple<List<DTO_Class>, Model.Model_Pagination_CurrentPage> Classes = await _classService.GetClasses(
        //             searchKeyword: searchKeyword,
        //             searchColumn: searchColumn,
        //             filterKeyword: filterKeyword,
        //             filterColumn: filterColumn,
        //             where_Clause: where_Clause,
        //             orderBy: orderBy, 
        //             orderMode: orderMode,
        //             orderBy_Clause: orderBy_Clause,

        //             pageNo: pageNo,
        //             rowsPerPage: rowsPerPage);
        //         return Ok(Classes);
        //     } catch (Exception e) {
        //         return BadRequest($"{{\"content\": \"{e.Message}\"}}");
        //     }
        // }

        [HttpPost("view")]
        public async Task<IActionResult> GetSubjects([FromBody] Model_PaginatedClientRequest clientRequest) {
            try {
                Tuple<List<DTO_Subject_Detail>, Model.Model_Pagination_CurrentPage> subjects = await _subjectService.GetSubjects(clientRequest); 
                return Ok(subjects);
            } catch (Exception e) {
                return BadRequest($"{{\"content\": \"{e.Message}\"}}");
            }
        }
        
        [HttpGet("KeyValue/{parentCode}")]
        public async Task<IActionResult> GetKeyValue([FromRoute] string parentCode) {
            try {
                List<Model_KeyValue> pairs = await _subjectService.GetKeyValuePair(parentCode); 
                return Ok(pairs);
            } catch (Exception e) {
                return BadRequest($"{{\"content\": \"{e}\"}}");
            }
        }


    }
}