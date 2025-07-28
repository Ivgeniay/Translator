using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Translate.Application.Services
{
    public class TranslationHub : Hub
    {
        private readonly ILogger<TranslationHub> _logger;

        public TranslationHub(ILogger<TranslationHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Клиент подключился: {ConnectionId}", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Клиент отключился: {ConnectionId}", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}