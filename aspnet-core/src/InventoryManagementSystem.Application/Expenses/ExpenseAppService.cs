using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using InventoryManagementSystem.Products;
using InventoryManagementSystem.Expenses.Dto;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.Expenses
{
    public class ExpenseService : AbpServiceBase, IExpenseService
    {
        private readonly IRepository<Expense, long> _expenseRepository;
        public ExpenseService(IRepository<Expense, long> expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }


        public async Task<ResponseMessagesDto> CreateOrEditAsync(CreateExpenseDto expenseDto)
        {
            ResponseMessagesDto result;
            if (expenseDto.Id == 0)
            {
                result = await CreateExpenseAsync(expenseDto);
            }
            else
            {
                result = await UpdateExpenseAsync(expenseDto);
            }
            return result;
        }

        private async Task<ResponseMessagesDto> CreateExpenseAsync(CreateExpenseDto expenseDto)
        {
            var result = await _expenseRepository.InsertAsync(new Expense()
            {
                Name = expenseDto.Name
            });

            await UnitOfWorkManager.Current.SaveChangesAsync();

            if (result.Id != 0)
            {
                return new ResponseMessagesDto()
                {
                    Id = result.Id,
                    SuccessMessage = AppConsts.SuccessfullyInserted,
                    Success = true,
                    Error = false,
                };
            }
            return new ResponseMessagesDto()
            {
                Id = 0,
                ErrorMessage = AppConsts.InsertFailure,
                Success = false,
                Error = true,
            };
        }

        private async Task<ResponseMessagesDto> UpdateExpenseAsync(CreateExpenseDto expenseDto)
        {
            var result = await _expenseRepository.UpdateAsync(new Expense()
            {
                Id = expenseDto.Id,
                Name = expenseDto.Name
            });

            if (result != null)
            {
                return new ResponseMessagesDto()
                {
                    Id = result.Id,
                    SuccessMessage = AppConsts.SuccessfullyUpdated,
                    Success = true,
                    Error = false,
                };
            }
            return new ResponseMessagesDto()
            {
                Id = 0,
                ErrorMessage = AppConsts.UpdateFailure,
                Success = false,
                Error = true,
            };
        }
        public async Task<ExpenseDto> GetById(long expenseId)
        {
            var result = await _expenseRepository.GetAll()
                .Where(i => i.Id == expenseId)
                .Select(i =>
                new ExpenseDto()
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .FirstOrDefaultAsync();
            return result;
        }


        public async Task<ResponseMessagesDto> DeleteAsync(long expenseId)
        {
            await _expenseRepository.DeleteAsync(new Expense()
            {
                Id = expenseId
            });

            return new ResponseMessagesDto()
            {
                Id = expenseId,
                SuccessMessage = AppConsts.SuccessfullyDeleted,
                Success = true,
                Error = false,
            };
        }

        public async Task<List<ExpenseDto>> GetAll()
        {
            var result = await _expenseRepository.GetAll().Select(i => new ExpenseDto()
            {
                Id = i.Id,
                Name = i.Name,
                CreatorUserId = i.CreatorUserId,
                CreationTime = i.CreationTime,
                LastModificationTime = i.LastModificationTime
            }).ToListAsync();
            return result;
        }
        public async Task<PagedResultDto<ExpenseDto>> GetPaginatedAllAsync(PagedExpenseResultRequestDto input)
        {
            var filteredExpenses = _expenseRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), x => x.Name.Contains(input.Name));

            var pagedAndFilteredExpenses = filteredExpenses
                .OrderBy(i => i.Name)
                .PageBy(input);

            var totalCount = await pagedAndFilteredExpenses.CountAsync();

            return new PagedResultDto<ExpenseDto>(
                totalCount: totalCount,
                items: await pagedAndFilteredExpenses.Select(i => new ExpenseDto()
                {
                    Id = i.Id,
                    Name = i.Name
                })
                    .ToListAsync());
        }
    }
}

