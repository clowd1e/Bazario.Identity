using Bazario.AspNetCore.Shared.Abstractions.Data;
using Bazario.Identity.Domain.RefreshTokens;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Bazario.Identity.Infrastructure.BackgroundJobs
{
    [DisallowConcurrentExecution]
    internal sealed class RemoveExpiredRefreshTokensBackgroundJob : IJob
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveExpiredRefreshTokensBackgroundJob> _logger;

        public RemoveExpiredRefreshTokensBackgroundJob(
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork,
            ILogger<RemoveExpiredRefreshTokensBackgroundJob> logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Starting removal of expired refresh tokens...");

            var expiredTokens = await _refreshTokenRepository.GetExpiredRefreshTokensAsync();

            if (!expiredTokens.Any())
            {
                _logger.LogInformation("No expired refresh tokens found.");
                return;
            }

            foreach (var token in expiredTokens)
            {
                await _refreshTokenRepository.DeleteAsync(token);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully removed {Count} expired refresh tokens.", expiredTokens.Count());
        }
    }
}
