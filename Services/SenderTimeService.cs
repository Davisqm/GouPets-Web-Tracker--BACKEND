using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PetsApi.Services
{
  public class SenderTimeService : IHostedService, IDisposable
  {
    private readonly IEmailService _emailService;
    private readonly ILogger<SenderTimeService> logger;
    private readonly ISammService _sammService;
    private Timer timer;

    public SenderTimeService(ILogger<SenderTimeService> logger, IEmailService emailService, ISammService sammService)
    {
      this.logger = logger;
      this._emailService = emailService;
      this._sammService = sammService;
    }

    public void  Dispose()
    {
      timer?.Dispose();
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
      timer = new Timer(o => 
      {
        _emailService.GetGpsPetsClient2();
      },
        null,
        TimeSpan.Zero,
        //TimeSpan.FromSeconds(20));
        TimeSpan.FromHours(4));

      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      logger.LogInformation("Printing worker stopping");
      return Task.CompletedTask;
    }
  }
}
