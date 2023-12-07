﻿using Karami.Core.UseCase.Contracts.Interfaces;

namespace Karami.UseCase.UserUseCase.Commands.CheckExist;

public class CheckExistCommand : IQuery<bool>
{
    public string UserId { get; set; }
}