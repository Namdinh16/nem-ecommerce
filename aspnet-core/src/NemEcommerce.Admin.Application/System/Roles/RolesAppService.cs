﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using NemEcommerce.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;

namespace NemEcommerce.Admin.System.Roles
{
    [Authorize(IdentityPermissions.Roles.Default, Policy = "AdminOnly")]
    public class RolesAppService : CrudAppService<
        IdentityRole,
        RoleDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateRoleDto,
        CreateUpdateRoleDto>, IRoleAppService
    {
        protected PermissionManagementOptions Options { get; }
        protected IPermissionManager PermissionManager { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; }

        public RolesAppService(IRepository<IdentityRole, Guid> repository, IPermissionManager permissionManager,
        IPermissionDefinitionManager permissionDefinitionManager,
        IOptions<PermissionManagementOptions> options,
        ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager)
            : base(repository)
        {
            Options = options.Value;
            PermissionManager = permissionManager;
            PermissionDefinitionManager = permissionDefinitionManager;
            SimpleStateCheckerManager = simpleStateCheckerManager;


            GetPolicyName = IdentityPermissions.Roles.Default;
            GetListPolicyName = IdentityPermissions.Roles.Default;
            CreatePolicyName = IdentityPermissions.Roles.Create;
            UpdatePolicyName = IdentityPermissions.Roles.Update;
            DeletePolicyName = IdentityPermissions.Roles.Delete;
        }


        [Authorize(IdentityPermissions.Roles.Delete)]
        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }


        [Authorize(IdentityPermissions.Roles.Default)]
        public async Task<List<RoleInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();

            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<IdentityRole>, List<RoleInListDto>>(data);

        }


        [Authorize(IdentityPermissions.Roles.Default)]
        public async Task<PagedResultDto<RoleInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.Keyword), x => x.Name.Contains(input.Keyword));

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<RoleInListDto>(totalCount, ObjectMapper.Map<List<IdentityRole>, List<RoleInListDto>>(data));
        }


        [Authorize(IdentityPermissions.Roles.Create)]
        public async override Task<RoleDto> CreateAsync(CreateUpdateRoleDto input)
        {
            var query = await Repository.GetQueryableAsync();
            var isNameExisted = query.Any(x => x.Name == input.Name);
            if (isNameExisted)
            {
                throw new BusinessException(NemEcommerceDomainErrorCodes.RoleNameAlreadyExists)
                    .WithData("Name", input.Name);
            }
            var role = new IdentityRole(Guid.NewGuid(), input.Name);
            role.ExtraProperties[RoleConsts.DescriptionFieldName] = input.Description;
            var data = await Repository.InsertAsync(role);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return ObjectMapper.Map<IdentityRole, RoleDto>(data);
        }


        [Authorize(IdentityPermissions.Roles.Update)]
        public async override Task<RoleDto> UpdateAsync(Guid id, CreateUpdateRoleDto input)
        {

            var role = await Repository.GetAsync(id);
            if (role == null)
            {
                throw new EntityNotFoundException(typeof(IdentityRole), id);
            }

            // Check if the new name is different from the current one
            if (role.Name != input.Name)
            {
                // Check if the new name is already existed
                var isNameExisted = await Repository.AnyAsync(x => x.Name == input.Name && x.Id != id);
                if (isNameExisted)
                {
                    throw new BusinessException(NemEcommerceDomainErrorCodes.RoleNameAlreadyExists)
                        .WithData("Name", input.Name);
                }
            }

            // Create a new role with updated properties
            var updatedRole = new IdentityRole(id, input.Name);
            updatedRole.ConcurrencyStamp = role.ConcurrencyStamp;
            updatedRole.ExtraProperties[RoleConsts.DescriptionFieldName] = input.Description;

            // Replace the existing role with the new one
            await Repository.DeleteAsync(role);
            updatedRole = await Repository.InsertAsync(updatedRole);

            // Save changes
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return ObjectMapper.Map<IdentityRole, RoleDto>(updatedRole);
        }


        [Authorize(IdentityPermissions.Roles.Default)]
        public async Task<GetPermissionListResultDto> GetPermissionAsync(string providerName, string providerKey)
        {
            //await CheckProviderPolicy(providerName);

            var result = new GetPermissionListResultDto
            {
                EntityDisplayName = providerKey,
                Groups = new List<PermissionGroupDto>()
            };


            var groups = await PermissionDefinitionManager.GetGroupsAsync();
            foreach (var group in groups.Where(x => x.Name.StartsWith("AbpIdentity") || x.Name.StartsWith("NemEcommerceAdmin")))
            {
                var groupDto = CreatePermissionGroupDto(group);

                var neededCheckPermissions = new List<PermissionDefinition>();

                var permissions = group.GetPermissionsWithChildren()
                    .Where(x => x.IsEnabled)
                    .Where(x => !x.Providers.Any() || x.Providers.Contains(providerName));


                foreach (var permission in permissions)
                {
                    if (permission.Parent != null && !neededCheckPermissions.Contains(permission.Parent))
                    {
                        continue;
                    }

                    if (await SimpleStateCheckerManager.IsEnabledAsync(permission))
                    {
                        neededCheckPermissions.Add(permission);
                    }
                }

                if (!neededCheckPermissions.Any())
                {
                    continue;
                }

                var grantInfoDtos = neededCheckPermissions
                    .Select(CreatePermissionGrantInfoDto)
                    .ToList();

                var multipleGrantInfo = await PermissionManager.GetAsync(neededCheckPermissions.Select(x => x.Name).ToArray(), providerName, providerKey);

                foreach (var grantInfo in multipleGrantInfo.Result)
                {
                    var grantInfoDto = grantInfoDtos.First(x => x.Name == grantInfo.Name);

                    grantInfoDto.IsGranted = grantInfo.IsGranted;

                    foreach (var provider in grantInfo.Providers)
                    {
                        grantInfoDto.GrantedProviders.Add(new ProviderInfoDto
                        {
                            ProviderName = provider.Name,
                            ProviderKey = provider.Key,
                        });
                    }

                    groupDto.Permissions.Add(grantInfoDto);
                }

                if (groupDto.Permissions.Any())
                {
                    result.Groups.Add(groupDto);
                }
            }

            return result;
        }


        private PermissionGrantInfoDto CreatePermissionGrantInfoDto(PermissionDefinition permission)
        {
            return new PermissionGrantInfoDto
            {
                Name = permission.Name,
                DisplayName = permission.DisplayName?.Localize(StringLocalizerFactory),
                ParentName = permission.Parent?.Name,
                AllowedProviders = permission.Providers,
                GrantedProviders = new List<ProviderInfoDto>()
            };
        }



        private PermissionGroupDto CreatePermissionGroupDto(PermissionGroupDefinition group)
        {


            return new PermissionGroupDto
            {
                Name = group.Name,
                DisplayName = group.DisplayName?.Localize(StringLocalizerFactory),
                Permissions = new List<PermissionGrantInfoDto>()
            };
        }


        [Authorize(IdentityPermissions.Roles.Update)]
        public virtual async Task UpdatePermissionAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            //await CheckProviderPolicy(providerName);

            foreach (var permissionDto in input.Permissions)
            {
                await PermissionManager.SetAsync(permissionDto.Name, providerName, providerKey, permissionDto.IsGranted);
            }
        }

        protected virtual async Task CheckProviderPolicy(string providerName)
        {
            var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
            if (policyName.IsNullOrEmpty())
            {
                throw new AbpException($"No policy defined to get/set permissions for the provider '{providerName}'. Use {nameof(PermissionManagementOptions)} to map the policy.");
            }

            await AuthorizationService.CheckAsync(policyName);
        }
    }
}
