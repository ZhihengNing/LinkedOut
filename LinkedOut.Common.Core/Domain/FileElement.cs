using LinkedOut.Common.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace LinkedOut.Common.Domain;

public class FileElement
{
    public IFormFile? File { get; set; }

    public BucketType BucketType { get; set; }

    public int AssociateId { get; set; }
}