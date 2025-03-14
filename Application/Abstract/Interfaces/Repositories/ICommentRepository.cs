using Application.Abstract.Interfaces.Base;
using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces.Repositories;

public interface ICommentRepository
    : IRepository<CommentEntity, Guid>
{
    Task<IEnumerable<CommentModel>> GetAllExtendedByTaskIdAsync(Guid taskId);

    Task<CommentModel?> GetByIdExtendedAsync(Guid id);
}
