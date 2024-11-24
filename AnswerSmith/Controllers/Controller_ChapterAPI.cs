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
    [Route("api/chapter")]
    public class Controller_ChapterAPI : ControllerBase
    {
        private readonly IChapterService _chapterService;
        private readonly IChapterHandler _chapterHandler;
        public Controller_ChapterAPI()
        {
            _chapterHandler = new Data_Chapter();
            _chapterService = new Service_Chapter(_chapterHandler);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddChapter([FromBody] DTO_Chapter dto_Chapter) {
            try {
                bool status = await _chapterService.AddChapter(dto_Chapter);
                if (status) { return Ok(); }
                else { return BadRequest($"{{\"content\": \"Provided Data cannot be added.\"}}"); }
            } catch (Exception e) {
                return BadRequest($"{{\"content\": \"{e.Message}\"}}");
            } 
        }


        [HttpPost("edit")]
        public async Task<IActionResult> EditChapter([FromBody] DTO_Chapter dto_Chapter) {
            try {
                bool status = await _chapterService.EditChapter(dto_Chapter);
                if (status) { return Ok(); }
                else { return BadRequest($"{{\"content\": \"Provided Data cannot be saved.\"}}"); }
            } catch (Exception e) {
                return BadRequest($"{{\"content\": \"{e.Message}\"}}");
            }
        }

        
        [HttpGet("view/{code}")]
        public async Task<IActionResult> GetChapter([FromRoute] string code) {
            try {
                DTO_Chapter_Detail dto_ChapterDetail = await _chapterService.GetChapter(code);
                return Ok(dto_ChapterDetail);
            } catch (Exception e) {
                return BadRequest($"{{\"content\": \"{e.Message}\"}}");
            }
        }

        [HttpPost("view")]
        public async Task<IActionResult> GetChapters([FromBody] Model_PaginatedClientRequest clientRequest) {
            try {
                Tuple<List<DTO_Chapter_Detail>, Model.Model_Pagination_CurrentPage> chapters = await _chapterService.GetChapters(clientRequest); 
                return Ok(chapters);
            } catch (Exception e) {
                return BadRequest($"{{\"content\": \"{e.Message}\"}}");
            }
        }
        
        [HttpGet("KeyValue/{parentCode}")]
        public async Task<IActionResult> GetKeyValue([FromRoute] string parentCode) {
            try {
                List<Model_KeyValue> pairs = await _chapterService.GetKeyValuePair(parentCode); 
                return Ok(pairs);
            } catch (Exception e) {
                return BadRequest($"{{\"content\": \"{e.Message}\"}}");
            }
        }


    }
}