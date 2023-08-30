using Microsoft.EntityFrameworkCore;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;

namespace NextAPI.Dal.Repositories;

public class MessagesRepository : IBaseRepository<Message>
{
    private readonly ApplicationDbContext _db;
    
    public MessagesRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<Message[]> GetAll()
    {
        return await _db.Messages
            .OrderBy(x => x.Id)
            .Include(x => x.Author)
            .Include(x => x.Receiver)
            .ToArrayAsync();
    }

    public async Task<Message?> GetById(int messageId)
    {
        return await _db.Messages.FindAsync(messageId);
    }

    public async Task Update(Message message)
    {
        _db.Messages.Update(message);
        await _db.SaveChangesAsync();
    }

    public async Task Add(Message message)
    {
        await _db.Messages.AddAsync(message);
        await _db.SaveChangesAsync();
    }
    
    public async Task Delete(Message message)
    {
        _db.Remove(message);
        await _db.SaveChangesAsync();
    }
}