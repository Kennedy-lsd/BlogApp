using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllCommentsAsync();
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<Comment> CreateCommentAsync(Comment commentModel);
        Task<Comment?> UpdateCommentAsync(int id, UpdateCommentRequestDTO updateComment);
        Task<Comment?> DeleteCommentAsync(int id);

        /// <summary>
        /// Validates the comment by checking the provided ModelStateDictionary.
        /// </summary>
        /// <param name="modelState">The ModelStateDictionary containing validation information.</param>
        /// <returns>
        /// Returns the ModelStateDictionary if it is invalid, otherwise returns null.
        /// </returns>
        Task<ModelStateDictionary?> IsCommentValid(ModelStateDictionary ModelState);
    }
}