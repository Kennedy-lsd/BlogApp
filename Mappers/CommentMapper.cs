using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDTO ToCommentDTO(this Comment commentModel)
        {
            return new CommentDTO
            {
                Id  = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedAt = commentModel.CreatedAt,
                StockId = commentModel.StockId,
            };
        }

        public static Comment ToCreateCommentDTO(this CreateCommentRequestDTO form, int stockId)
        {
            return new Comment
            {
                Title = form.Title,
                Content = form.Content,
                StockId = stockId,
            };
        }
    }
}