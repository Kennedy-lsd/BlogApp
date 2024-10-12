using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllCommentsAsync();
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<Comment> CreateCommentAsync(Comment commentModel);
        Task<Comment?> UpdateCommentAsync(int id, UpdateCommentRequestDTO updateComment);
        Task<Comment?> DeleteCommentAsync(int id);
    }
}