namespace LinkedOut.Common.Helper;

public static class FileHelper
{
    //webapi项目就是不需要这个
    private const string BasicPath = "../../../";

    public static async Task<string> ReadFile(string path,bool isWebApi=true)
    {
        if (isWebApi)
        {
            return await File.ReadAllTextAsync(path);
        }

        return await File.ReadAllTextAsync(BasicPath + path);
    }
    
}