using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{   
    [Route("/api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo, UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var isValid = await _commentRepo.IsCommentValid(ModelState);

            if (isValid != null)
            {
                return BadRequest(isValid);
            }

            var comments =  await _commentRepo.GetAllCommentsAsync();

            var commentsDTO = comments.Select(x => x.ToCommentDTO());

            return Ok(commentsDTO);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetCommentByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDTO());
        }

        [HttpPost("{stockId:int}")]
        public async Task<ActionResult?> Create([FromRoute] int stockId, CreateCommentRequestDTO form)
        {   
            var isValid = await _commentRepo.IsCommentValid(ModelState);

            if (isValid != null)
            {
                return BadRequest(isValid);
            }

            if (!await _stockRepo.StockExistAsync(stockId))
            {
                return BadRequest("Stock doesn't exist");
            }

            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            
            var commentModel = form.ToCreateCommentDTO(stockId);
            commentModel.AppUserId = appUser.Id;
            await _commentRepo.CreateCommentAsync(commentModel);

            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id}, commentModel.ToCommentDTO());
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDTO updateComment)
        {
            var isValid = await _commentRepo.IsCommentValid(ModelState);

            if (isValid != null)
            {
                return BadRequest(isValid);
            }

            var comment = await _commentRepo.UpdateCommentAsync(id, updateComment);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDTO());
        }

        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var isValid = await _commentRepo.IsCommentValid(ModelState);

            if (isValid != null)
            {
                return BadRequest(isValid);
            }

            var comment = await _commentRepo.DeleteCommentAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}