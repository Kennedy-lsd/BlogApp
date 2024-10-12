using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateCommentAsync(Comment commentModel)
        {
            await _context.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteCommentAsync(int id)
        {
            var stock = await _context.Comments.FindAsync(id);

            if (stock == null)
            {
                return null;
            }

            _context.Comments.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return null;
            }

            return comment;
        }

        public async Task<Comment?> UpdateCommentAsync(int id, UpdateCommentRequestDTO updateComment)
        {
            var stock = await _context.Comments.FindAsync(id);

            if (stock == null)
            {
                return null;
            }

            stock.Title = updateComment.Title;
            stock.Content = updateComment.Content;

            await _context.SaveChangesAsync();

            return stock;
        }
    }
}