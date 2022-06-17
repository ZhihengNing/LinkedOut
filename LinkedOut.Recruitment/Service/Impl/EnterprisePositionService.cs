using LinkedOut.DB;
using LinkedOut.DB.Entity;

namespace LinkedOut.Recruitment.Service.Impl;

public class EnterprisePositionService: IEnterprisePositionService
{

    private readonly LinkedOutContext _context;

    public EnterprisePositionService(LinkedOutContext context)
    {
        _context = context;
    }

    public async Task InsertPosition(Position position)
    {
        await _context.Positions.AddAsync(position);
        
        await _context.SaveChangesAsync();
    }

    public Task DeletePosition(Position position)
    {
        throw new NotImplementedException();
    }
}