
using Client.Infrastructure.ApiClient;
using Shared.RequestModels.Department;
using Shared.ResponceModels.Department;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Client.Infrastructure.ApiClientManagers
{
    public interface IDepartmentClient : IApiService
    {
        Task<PaginationResponseOfModel<DepartmentDto>> SearchAsync(ClientSearchRequest request);
        Task<IResult<Guid>> CreateAsync(DepartmentCreateEdit request);
        Task<IResult<Guid>> UpdateAsync(Guid id, DepartmentCreateEdit request);
        Task<IResult<Guid>> DeleteAsync(Guid id);
        Task<IResult<DepartmentDto>> GetByIdAsync(Guid id);
        Task<DepartmentDto> DepartmentDtoGetByIdAsync(Guid id);
    }

    public partial class ClientSearchRequest : ApiClient.PaginationFilter
    {

    }


    public class DepartmentClient : IDepartmentClient
    {
        private readonly HttpClient _httpClient;
        public DepartmentClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<Guid>> CreateAsync(DepartmentCreateEdit request)
        {
            var response = await _httpClient.PostAsJsonAsync(DepartmentEndPoints.Create, request);
            return await response.ToResult<Guid>();
        }
        public async Task<PaginationResponseOfModel<DepartmentDto>> SearchAsync(ClientSearchRequest request)
        {
            var a = await SearchAsyncResult(request);
            return a.Data;
        }
        public async Task<IResult<PaginationResponseOfModel<DepartmentDto>>> SearchAsyncResult(ClientSearchRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(DepartmentEndPoints.GetAll, request);
            return await response.ToResult<PaginationResponseOfModel<DepartmentDto>>();
        }

        public async Task<IResult<Guid>> UpdateAsync(Guid id, DepartmentCreateEdit request)
        {
            var response = await _httpClient.PutAsJsonAsync(DepartmentEndPoints.Edit + id.ToString(), request);
            return await response.ToResult<Guid>();
        }

        public async Task<IResult<Guid>> DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"{DepartmentEndPoints.Delete}/{id}");
            return await response.ToResult<Guid>();
        }
        public async Task<IResult<DepartmentDto>> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"{DepartmentEndPoints.Get}/{id}");
            return await response.ToResult<DepartmentDto>();
        }
        public async Task<DepartmentDto> DepartmentDtoGetByIdAsync(Guid id)
        {
            var data = await GetByIdAsync(id);

            return data.Data;
        }


    }
}