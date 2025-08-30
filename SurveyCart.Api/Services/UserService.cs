using Microsoft.AspNetCore.Identity;
using SurveyCart.Api.Contracts.Users;

namespace SurveyCart.Api.Services
{
    public class UserService(
        UserManager<User> userManager,
        ILogger<PollService> logger
     ) : IUserService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly ILogger<PollService> _logger = logger;
        public async Task<Result<UserProfileResponse>>GetProfileAsync(string userId)
        {
            try 
                {


                var user = await _userManager.Users
               .Where(x => x.Id == userId)
               .ProjectToType<UserProfileResponse>()
               .SingleAsync();
                return Result.Success(user);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, $"Failed to process for get profile", ex.Message);
                throw;
            }

          
        }
 
        public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            try
                
            {
                //var user = await _userManager.FindByIdAsync(userId);
                //user = request.Adapt(user);
                //await _userManager.UpdateAsync(user!);
                await _userManager.Users
                    .Where(x => x.Id == userId)
                    .ExecuteUpdateAsync(setters =>
                        setters
                            .SetProperty(x => x.FirstName, request.FirstName)
                            .SetProperty(x => x.FirstName, request.FirstName)
                    );
                return Result.Success();
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, $"Failed to process for update profile", ex.Message);
                throw;
            }
        }

        public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            try 
            {
                var user = await _userManager.FindByIdAsync(userId);
                var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);
                if (result.Succeeded)
                {
                    return Result.Success();
                }
                var error = result.Errors.First();
                return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }
      
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, $"Failed to process for change password", ex.Message);
                throw;

            }
           
        }


    }
}
