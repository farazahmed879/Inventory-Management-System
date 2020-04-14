using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using InventoryManagementSystem.Expenses.Dto;

namespace InventoryManagementSystem.Expenses
{
    public interface IExpenseService : IApplicationService
    {
        Task<ResponseMessagesDto> CreateOrEditAsync(CreateExpenseDto expenseDto);

        Task<ExpenseDto> GetById(long expenseId);

        Task<ResponseMessagesDto> DeleteAsync(long expenseId);

        Task<List<ExpenseDto>> GetAll(long? tenantId);

        Task<PagedResultDto<ExpenseDto>> GetPaginatedAllAsync(PagedExpenseResultRequestDto input);
    }
}
