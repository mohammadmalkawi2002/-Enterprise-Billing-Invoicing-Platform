using BillingInvoicingPlatform.Application.Dto.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillingInvoicingPlatform.Application.Interfaces
{
    public interface IAuthService
    {
        Task <AuthResponse> RegisterAsync(RegisterDto dto);
        Task<AuthResponse> LoginAsync(LoginDto dto);

   
        Task<string> AssignRoleAsync(string userId, string role);

        Task <UserDto?> GetUserByIdAsync(string userId);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(string userId);
        
        
    }
}
