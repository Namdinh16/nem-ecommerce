using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.PermissionManagement;

namespace NemEcommerce.Admin.System.Roles
{
    public interface IRoleAppService : ICrudAppService
        <RoleDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateRoleDto,
        CreateUpdateRoleDto>
    {
        Task<PagedResultDto<RoleInListDto>> GetListFilterAsync(BaseListFilterDto input);
        Task<List<RoleInListDto>> GetListAllAsync();
        Task DeleteMultipleAsync(IEnumerable<Guid> ids);

        Task<GetPermissionListResultDto> GetPermissionAsync(string providerName, string providerKey);
        Task UpdatePermissionAsync(string providerName, string providerKey, UpdatePermissionsDto input);
    }
}
