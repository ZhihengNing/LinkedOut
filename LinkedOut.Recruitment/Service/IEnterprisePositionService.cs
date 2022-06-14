using LinkedOut.DB.Entity;

namespace LinkedOut.Recruitment.Service;

public interface IEnterprisePositionService
{
    Task InsertPosition(Position position);

    Task DeletePosition(Position position);
    
    
}