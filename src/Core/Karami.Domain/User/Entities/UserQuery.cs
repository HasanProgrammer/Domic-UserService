#pragma warning disable CS0649

using Karami.Core.Domain.Contracts.Abstracts;
using Karami.Domain.PermissionUser.Entities;
using Karami.Domain.RoleUser.Entities;

namespace Karami.Domain.User.Entities;

public class UserQuery : EntityQuery<string>
{
    public string FirstName   { get; set; }
    public string LastName    { get; set; }
    public string Description { get; set; }
    public string Username    { get; set; }
    public string Password    { get; set; }
    public string PhoneNumber { get; set; }
    public string Email       { get; set; }

    /*---------------------------------------------------------------*/
    
    //Relations

    public ICollection<RoleUserQuery> RoleUsers             { get; set; }
    public ICollection<PermissionUserQuery> PermissionUsers { get; set; }
}