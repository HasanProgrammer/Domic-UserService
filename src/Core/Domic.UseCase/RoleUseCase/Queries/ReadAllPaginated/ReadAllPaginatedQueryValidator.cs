﻿using Domic.Core.UseCase.Contracts.Interfaces;
using Domic.Core.UseCase.Exceptions;

namespace Domic.UseCase.RoleUseCase.Queries.ReadAllPaginated;

public class ReadAllPaginatedQueryValidator : IValidator<ReadAllPaginatedQuery>
{
    public Task<object> ValidateAsync(ReadAllPaginatedQuery input, CancellationToken cancellationToken)
    {
        if (input.PageNumber is null)
            throw new UseCaseException("تنظیم مقدار ( شماره صفحه ) الزامی می باشد !");

        if (input.CountPerPage is null)
            throw new UseCaseException("تنظیم مقدار ( تعداد برای هر صفحه ) الزامی می باشد !");

        return Task.FromResult(default(object));
    }
}