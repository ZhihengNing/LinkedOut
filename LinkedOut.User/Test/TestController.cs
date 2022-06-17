using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;

namespace LinkedOut.User.Test;

public class DllMake
{
    [DllImport(@"E:\code\NET\LinkedOut\x64\Debug\SubDll.dll", EntryPoint = "sub")] // 导入dll文件
    public static extern int sub(int x, int y);
    // public不是必要的。static必须，因为随类一起加载，
    // 而不是实例化时。extern必须，这说明是调用外部函数，而不是在指定的命名空间中。
}


[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    
    [HttpGet("")]
    public async Task TestDll()
    {
        var add = DllMake.sub(3,4);
        Console.WriteLine(add);
    }
}