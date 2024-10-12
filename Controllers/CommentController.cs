using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{   
    [Route("/api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var comments =  await _commentRepo.GetAllCommentsAsync();

            var commentsDTO = comments.Select(x => x.ToCommentDTO());

            return Ok(commentsDTO);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetCommentByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDTO());
        }

        [HttpPost("{stockId}")]
        public async Task<ActionResult> Create([FromRoute] int stockId, CreateCommentRequestDTO form)
        {
            if (!await _stockRepo.StockExistAsync(stockId))
            {
                return BadRequest("Stock doesn't exist");
            }

            var commentModel = form.ToCreateCommentDTO(stockId);
            await _commentRepo.CreateCommentAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id}, commentModel.ToCommentDTO());
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDTO updateComment)
        {
            var comment = await _commentRepo.UpdateCommentAsync(id, updateComment);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDTO());
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var comment = await _commentRepo.DeleteCommentAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return NoContent();
        }


    }
}