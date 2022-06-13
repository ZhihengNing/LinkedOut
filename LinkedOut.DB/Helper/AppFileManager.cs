using System.ComponentModel.DataAnnotations;
using LinkedOut.Common.Domain.Enum;
using LinkedOut.Common.Helper;
using LinkedOut.DB.Domain;
using LinkedOut.DB.Entity;
using Microsoft.AspNetCore.Http;

namespace LinkedOut.DB.Helper;

public class AppFileManager
{
    private readonly LinkedOutContext _context;

    public AppFileManager(LinkedOutContext context)
    {
        _context = context;
    }

    public void AddToAppFile(List<IFormFile>? files, BucketType bucketType, [Required] int associatedId)
    {
        if (files == null || files.Count == 0) return;

        files.AsParallel().ForAll(item =>
        {
            AddToAppFile(item,bucketType,associatedId);
        });
    }

    public void AddToAppFile(IFormFile? file, BucketType bucketType, [Required] int associatedId)
    {
        if (file == null) return;
        var uploadFile = OssHelper.UploadFile(file, bucketType, associatedId);
        var appFile = new AppFile
        {
            AssociatedId = associatedId,
            FileType = (int) AppFileType.Tweet,
            Url = uploadFile,
            Name = file.FileName
        };
        _context.Add(appFile);
    }

}