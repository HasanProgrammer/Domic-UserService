using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;
using Domic.Domain.Role.Contracts.Interfaces;
using Domic.UseCase.PermissionUseCase.Queries.ReadAllBasedOnRolesPaginated;

namespace Domic.UseCase.PermissionUseCase.Queries.ReadAllPaginated;

public class ReadAllBasedOnRolesPaginatedQueryValidator(IRoleCommandRepository roleCommandRepository) 
    : IValidator<ReadAllBasedOnRolesPaginatedQuery>
{
    public async Task<object> ValidateAsync(ReadAllBasedOnRolesPaginatedQuery input, CancellationToken cancellationToken)
    {
        var errors = new List<string>();
        
        if (input.PageNumber == null)
            throw new UseCaseException("تنظیم مقدار ( شماره صفحه ) الزامی می باشد !");

        if (input.CountPerPage == null)
            throw new UseCaseException("تنظیم مقدار ( تعداد برای هر صفحه ) الزامی می باشد !");

        if (!input.Roles.Any())
            throw new UseCaseException("هیچ نقشی انتخاب نشده است!");

        foreach (var role in input.Roles)
            if(!await roleCommandRepository.IsExistByIdAsync(role, cancellationToken))
                errors.Add(string.Format("نقشی با شناسه {0} موجود نمی باشد!", role));

        if (errors.Any())
            throw new UseCaseException(string.Join("|", errors));

        return Task.FromResult(default(object));
    }
}