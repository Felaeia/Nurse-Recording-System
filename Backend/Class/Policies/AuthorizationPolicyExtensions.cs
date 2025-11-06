namespace NurseRecordingSystem.Authorization
{
    public static class AuthorizationPolicyExtensions
    {
        public static IServiceCollection AddAppPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Policy for "User" (must have the "User" role)
                
                options.AddPolicy("UserPolicy", policy =>
                    policy.RequireRole("User"));

                // Policy for "Nurse" (must have the "Nurse" role)
                options.AddPolicy("NursePolicy", policy =>
                    policy.RequireRole("Nurse"));
            });

            return services;
        }
    }
}