using System.Collections.Concurrent;
using LinkedOut.Common.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LinkedOut.Common.Helper;

//开启线程消费队列
public class ConsumerService : BackgroundService
{
    private readonly ILogger<ConsumerService> _logger;
    
    private static readonly ConcurrentQueue<FileElement> Queue = new();

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public ConsumerService(ILogger<ConsumerService> logger)
    {
        _logger = logger;
    }

    public static void AddToQueue(FileElement file)
    {
        Console.WriteLine("{0}加入队列了{1}", DateTime.Now, file.File.FileName);
        Queue.Enqueue(file);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var _ = Task.Run(() =>
        {
            while (true)
            {
                if (Queue.IsEmpty) continue;
                Queue.TryDequeue(out var fileElement);
                try
                {
                    OssHelper.UploadFile(fileElement!);
                    _logger.LogInformation("{Now}队列消费了{FileFileName}", DateTime.Now, fileElement.File.FileName);
                }
                catch (System.Exception e)
                {
                    _logger.LogError("抛出了异常{EMessage}", e.GetBaseException());
                }
            }
        }, stoppingToken);
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();
        return base.StopAsync(cancellationToken);
    }


}