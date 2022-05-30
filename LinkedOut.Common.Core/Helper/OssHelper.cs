using Aliyun.OSS;
using Aliyun.OSS.Common;
using LinkedOut.Common.Domain;
using LinkedOut.Common.Domain.Enum;
using LinkedOut.Common.Exception;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Serilog;

namespace LinkedOut.Common.Helper;

public static class OssHelper
{
    private static readonly JObject Json = JsonHelper
        .ReadConfigJson("../LinkedOut.Common.Core/config.json")
        .Value<JObject>("oss")!;

    private static readonly string Endpoint = Json.Value<string>("endpoint")!;

    private static readonly string AccessKeyId = Json.Value<string>("accessKeyId")!;

    private static readonly string AccessKeySecret = Json.Value<string>("accessKeySecret")!;

    private static readonly string BucketName = Json.Value<string>("bucketName")!;

    private static readonly string Prefix = $"https://{BucketName}.{Endpoint}/";
    
    private static readonly OssClient Client = new(Endpoint, AccessKeyId, AccessKeySecret);
    
    private static void PutObject(IFormFile file, string path)
    {
        if (file == null)
        {
            throw new ApiException("文件不能为空");
        }

        try
        {
            Client.PutObject(BucketName, path, file.OpenReadStream());
        }
        catch (System.Exception ex)
        {
            throw new ApiException(ex.Message);
        }
    }

    public static string UploadFile(IFormFile file, BucketType bucketType, int id)
    {
        var fileName = file.FileName;
        var path = BucketTypeHelper.GetBucketTypeStr(bucketType);
        path = $"{path}/{id}/{fileName}";
        PutObject(file, path);
        return Prefix + path;
        
    }


    public static void CreateBucket(string bucketName)
    {
        try
        {
            var bucket = Client.CreateBucket(bucketName);
        }
        catch (System.Exception ex)
        {
            throw new ApiException(ex.Message);
        }
    }

    public static bool ExistsObject(string key)
    {
        try
        {
            return Client.DoesObjectExist(BucketName, key);
        }
        catch (OssException ex)
        {
            Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Failed with error info: {0}", ex.Message);
        }

        return false;
    }


    public static void DeleteObject(string key)
    {
        try
        {
            Client.DeleteObject(BucketName,key);
        }
        catch (System.Exception ex)
        {
            throw new ApiException(ex.Message);
        }
    }

    public static List<OssObjectSummary> ListObjects(string bucketName)
    {
        var objectsList = new List<OssObjectSummary>();
        try
        {
            ObjectListing? result;
            var nextMarker = string.Empty;
            do
            {
                var listObjectsRequest = new ListObjectsRequest(bucketName)
                {
                    Marker = nextMarker
                };
                // 列举文件。
                result = Client.ListObjects(listObjectsRequest);
                foreach (var summary in result.ObjectSummaries)
                {
                    objectsList.Add(summary);
                    Console.WriteLine(summary.Key);
                }

                nextMarker = result.NextMarker;
            } while (result.IsTruncated);

        }
        catch (OssException ex)
        {
            Console.WriteLine("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3}",
                ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Failed with error info: {0}", ex.Message);
        }

        return objectsList;
    }


}