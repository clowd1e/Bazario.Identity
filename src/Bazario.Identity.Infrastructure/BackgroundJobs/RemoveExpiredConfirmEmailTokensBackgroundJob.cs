using Bazario.AspNetCore.Shared.Abstractions.Data;
using Bazario.Identity.Domain.ConfirmEmailTokens;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Bazario.Identity.Infrastructure.BackgroundJobs
{
    internal sealed class RemoveExpiredConfirmEmailTokensBackgroundJob : IJob
    {
        private readonly IConfirmEmailTokenRepository _confirmEmailTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveExpiredConfirmEmailTokensBackgroundJob> _logger;

        public RemoveExpiredConfirmEmailTokensBackgroundJob(
            IConfirmEmailTokenRepository confirmEmailTokenRepository,
            IUnitOfWork unitOfWork,
            ILogger<RemoveExpiredConfirmEmailTokensBackgroundJob> logger)
        {
            _confirmEmailTokenRepository = confirmEmailTokenRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Starting removal of expired confirm email tokens...");

            var expiredTokens = await _confirmEmailTokenRepository.GetExpiredConfirmEmailTokensAsync();

            if (!expiredTokens.Any())
            {
                _logger.LogInformation("No expired confirm email tokens found.");
                return;
            }

            foreach (var token in expiredTokens)
            {
                await _confirmEmailTokenRepository.DeleteAsync(token);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully removed {Count} expired confirm email tokens.", expiredTokens.Count());
        }
    }
}
