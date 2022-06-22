using NUnit.Framework;

namespace LinkedOut.User.Test;

public class Test
{

    [Test]
    public async Task TestReadFile()
    {
        var arr = new List<string>();
        
        var combine = Path.Combine(AppContext.BaseDirectory,"dbConfig.json");
        Console.WriteLine(combine);
        // var readFile = await FileHelper.ReadFile("mail.html",false);
        //
        // Console.WriteLine(readFile);
        // var doc = new HtmlDocument();
        // doc.LoadHtml(readFile);
        //
        // var elementbyId = doc.GetElementbyId("code");
        // elementbyId.InnerHtml = "4566";
        // Console.WriteLine(elementbyId.InnerHtml);
        // Console.WriteLine(doc.ParsedText);
    }
}