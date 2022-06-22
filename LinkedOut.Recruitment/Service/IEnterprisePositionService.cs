using LinkedOut.Common.Feign.Recruitment.Dto;
using LinkedOut.DB.Entity;
using LinkedOut.Recruitment.Domain;

namespace LinkedOut.Recruitment.Service;

public interface IEnterprisePositionService
{
    Task InsertPosition(Position position);

    Task DeletePosition(int jobId);
    
    Task<List<ApplicantVo>> GetAllApplicants(int jobId);

    Task<PositionDetailVo> GetPositionDetails(int unifiedId, int jobId);

    Task<List<PositionVo>> GetCompanyAllPosition(int unifiedId, int? momentId);
    
    Task<List<PositionDto>> GetPosition(int unifiedId);

}