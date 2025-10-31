using NurseRecordingSystem.DTO.AuthServiceDTOs;

namespace NurseRecordingSystem.Contracts.ServiceContracts.Auth
{
    public interface ISessionTokenService
    {
        /// <summary>
        /// Creates a new session token for a user.
        /// Note: This will first deactivate any other active tokens for this user.
        /// </summary>
        Task<SessionTokenDTO?> CreateSessionAsync(int authId);

        /// <summary>
        /// Refreshes an existing active session token with a new token value and expiry.
        /// </summary>
        Task<SessionTokenDTO?> RefreshSessionTokenAsync(int authId);

        /// <summary>
        /// Ends a user's active session by expiring the token.
        /// </summary>
        Task EndSessionAsync(int authId);

        /// <summary>
        /// Validates if an active, non-expired session exists for a user.
        /// </summary>
        // Signature changed from Task<SessionTokenDTO?>(byte[] token)
        Task<bool> ValidateTokenAsync(int authId);
    }
}