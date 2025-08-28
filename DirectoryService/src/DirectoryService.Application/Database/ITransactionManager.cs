﻿using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;

namespace DirectoryService.Application.Database;

public interface ITransactionManager
{
    Task<Result<ITransactionScope, Error>>
        BeginTransactionAsync(CancellationToken cancellationToken);

    Task<UnitResult<Error>> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}