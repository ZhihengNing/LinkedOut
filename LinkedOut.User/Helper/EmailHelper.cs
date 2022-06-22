using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using LinkedOut.Common.Exception;
using LinkedOut.Common.Helper;

namespace LinkedOut.User.Helper;


public static class EmailHelper
{
    private const string EmailTemplate = "mail.html";

    private const string BaseRandomStr = "abcdefghijklmnopqrstuvwxyz0123456789";

    [DllImport("Dll/LinkedOut.Verify.dll", EntryPoint = "email")] // 导入dll文件
    public static extern bool VerifyEmail(string s);
    
    private class EmailInfo
    {
        public string Code { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime Date { get; set; }
    }

    public static string GeneratorCode(int codeLength = 8)
    {
        var length = BaseRandomStr.Length;
        var random = new Random();
        var begin = "";
        for (var i = 0; i < codeLength; i++)
        {
            begin += BaseRandomStr[random.Next(0, length - 1)];
        }
        return begin;
    }

    public static async Task SendEmail(string email, string code)
    {
        if (email == null)
        {
            throw new ApiException("收件人的邮箱没写");
        }

        if (string.IsNullOrEmpty(code))
        {
            throw new ApiException("邮箱验证码为空");
        }

        var smtpClient = AppSettingHelper.App("email", "smtpClient");
        var sendAddress = AppSettingHelper.App("email", "sendAddress");
        var credential = AppSettingHelper.App("email", "credential");

        if (sendAddress == null || smtpClient == null || credential == null)
        {
            throw new ApiException("邮箱参数不完整");
        }

        var client = new SmtpClient(smtpClient);

        var send = new MailAddress(sendAddress);
        var receive = new MailAddress(email);
        var processEmail = await ProcessEmail(send, receive, new EmailInfo
        {
            Code = code,
            Email = email,
            Date = DateTime.Now
        });

        var networkCredential = new NetworkCredential(sendAddress, credential);
        client.Credentials = networkCredential;
        client.Send(processEmail);
    }

    private static async Task<MailMessage> ProcessEmail(MailAddress send, MailAddress receive, EmailInfo info)
    {
        var msg = new MailMessage(send, receive);
        var readFile = await FileHelper.ReadFile(EmailTemplate);

        var replace = readFile
            .Replace("{{code}}", info.Code)
            .Replace("{{email}}", info.Email)
            .Replace("{{date}}", info.Date.ToString(CultureInfo.InvariantCulture));

        msg.Subject = "邮件验证码";
        msg.IsBodyHtml = true;
        msg.Body = replace;
        return msg;
    }
}