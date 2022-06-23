using System.Runtime.InteropServices;
using LinkedOut.User.Helper;
using Microsoft.AspNetCore.Mvc;
using LinkedOutAdd;

namespace LinkedOut.User.Test;

public class DllMake
{
    
    //基于webapi的路径判断规则就是这样，对于直接xxx.dll来说，就是在当前项目的根路径下（而其他的需要根据相对路径获取）
    [DllImport("Dll/LinkedOut.Sub.dll", EntryPoint = "sub")] // 导入dll文件
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
        var one = new Add();
        var result = one.add(1, 4);
        Console.WriteLine(result);
        var add = EmailHelper.VerifyEmail("1094554173@qq.com");
        Console.WriteLine(add);
        var add1 = EmailHelper.VerifyEmail("p23");
        Console.WriteLine(add1);
        var sub = DllMake.sub(3,5);
        Console.WriteLine(sub);
    }

    [HttpGet("date")]
    public async Task<DateTime> TestDateTime(DateTime date)
    {
        
        Console.WriteLine(date);
        return date;
    }
    
}