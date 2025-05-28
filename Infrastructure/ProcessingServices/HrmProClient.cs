using System.Net;
using System.Net.Http.Json;
using System.Text;
using Application.ProcessingServices;
using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Settings;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Requests.HrmPro;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Contacts.Infrastructure.ProcessingServices;

internal class HrmProClient : IHrmProClient
{
    private readonly IMemoryCache cache;
    private readonly HttpClient client;
    private readonly HttpConfiguration httpConfiguration;
    private readonly IConfiguration configuration;
    private readonly ILogger<HrmProClient> logger;
    private readonly ITranslationService translationService;

    public HrmProClient(HttpClient client,
                        IMemoryCache cache,
                        IOptions<List<HttpConfiguration>> options,
                        ILogger<HrmProClient> logger,
                        IConfiguration configuration,
                        ITranslationService translationService)
    {
        this.cache = cache;
        this.configuration = configuration;
        this.logger = logger;

        httpConfiguration = options.Value?.SingleOrDefault(x => x.Name == HttpConfiguration.HRM_PRO)
            ?? throw new MissingConfigurationException();

        this.client = client;

        this.client.BaseAddress = new Uri(httpConfiguration.BaseUrl);
        this.translationService = translationService;
    }
    public async Task<ResponseWrapper<List<Department>>> GetDepartments(string token,
                                                                        int organizationId,
                                                                        CancellationToken cancellationToken = default)
    {
        string key = $"hrm_pro_departments:{organizationId}";

        if (cache.TryGetValue(key, out ResponseWrapper<List<Department>>? cachedResponse))
        {
            if (cachedResponse is not null)
            {
                return cachedResponse;
            }
        }

        string query = $"?organizations={organizationId}";

        var endpoint = httpConfiguration.Endpoints.Single(x => x.Name == HttpEndpoint.HrmPro_Department);

        var path = endpoint.Path;

        if (!string.IsNullOrWhiteSpace(query))
        {
            path += query;
        }

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(client.BaseAddress!, path),
            Method = HttpMethod.Parse(endpoint.Method)
        };

        request.Headers.TryAddWithoutValidation("Authorization", token);

        var response = await client.SendAsync(request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var departmentsResponse = await
                response.Content.ReadFromJsonAsync<ResponseWrapper<List<Department>>>(cancellationToken);

            if (departmentsResponse is { Message: true })
            {
                cache.Set(key, departmentsResponse, TimeSpan.FromDays(1));

                return departmentsResponse;
            }

            throw new NotImplementedException();
        }

        throw new NotImplementedException();
    }

    public async Task<ResponseWrapper<HrmListResponse<Position>>> GetPositions(string token,
                                                                               ListPositionsQuery query,
                                                                               CancellationToken cancellationToken = default)
    {
        string key = $"hrm_pro_positions:{query.OrganizationId}";

        var endpoint = httpConfiguration.Endpoints.Single(x => x.Name == HttpEndpoint.HrmPro_Positions);

        var path = endpoint.Path;

        var pathWithQuery = path + query.GetQueryString(translationService);

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(client.BaseAddress!, pathWithQuery),
            Method = HttpMethod.Parse(endpoint.Method)
        };

        request.Headers.TryAddWithoutValidation("Authorization", token);

        var response = await client.SendAsync(request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var positionsResponse = await
                response.Content.ReadFromJsonAsync<ResponseWrapper<HrmListResponse<Position>>>(cancellationToken);

            if (positionsResponse is { Message: true })
            {
                // Task.Run(async () =>
                // {
                //     if (!query.DepartmentId.HasValue)
                //     {
                //         if (positionsResponse.Data.Total > query.PerPage)
                //         {
                            
                //         }
                //     }
                //     else
                //     {
                //     }
                // }, cancellationToken);

                return positionsResponse;
            }

            throw new NotImplementedException();
        }

        throw new NotImplementedException();
    }

    public async Task<ResponseWrapper<HrmListResponse<Position>>> GetPositions(string token, string query, CancellationToken cancellationToken = default)
    {
        var endpoint = httpConfiguration.Endpoints.Single(x => x.Name == HttpEndpoint.HrmPro_Positions);

        var path = endpoint.Path;

        var pathWithQuery = path + query;

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(client.BaseAddress!, pathWithQuery),
            Method = HttpMethod.Parse(endpoint.Method)
        };

        request.Headers.TryAddWithoutValidation("Authorization", token);

        var response = await client.SendAsync(request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var positionsResponse = await
                response.Content.ReadFromJsonAsync<ResponseWrapper<HrmListResponse<Position>>>(cancellationToken);

            if (positionsResponse is { Message: true })
            {
                // Task.Run(async () =>
                // {
                //     if (!query.DepartmentId.HasValue)
                //     {
                //         if (positionsResponse.Data.Total > query.PerPage)
                //         {
                            
                //         }
                //     }
                //     else
                //     {
                //     }
                // }, cancellationToken);

                return positionsResponse;
            }

            throw new NotImplementedException();
        }

        throw new NotImplementedException();
    }

    public async Task<ResponseWrapper<HrmListResponse<DepartmentSearchItem>>> GetSearchDepartments(string token, string query, CancellationToken cancellationToken = default)
    {
        // TODO department search
        var endpoint = httpConfiguration.Endpoints.Single(x => x.Name == HttpEndpoint.HrmPro_DepartmentsSearch);

        string path = endpoint.Path;

        if (!string.IsNullOrWhiteSpace(query))
        {
            path += query;
        }

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(client.BaseAddress!, path),
            Method = HttpMethod.Parse(endpoint.Method)
        };

        request.Headers.TryAddWithoutValidation("Authorization", token);

        var response = await client.SendAsync(request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var departments = await
                response.Content.ReadFromJsonAsync<ResponseWrapper<HrmListResponse<DepartmentSearchItem>>>(cancellationToken);


            return departments!;
        }


        throw new NotImplementedException();
    }

    public async Task<ResponseWrapper<List<Organization>>> GetStructure(string token, CancellationToken cancellationToken = default)
    {
        const string key = "hrm_pro_structure";

        if (cache.TryGetValue(key, out ResponseWrapper<List<Organization>>? cachedStructure))
        {
            return cachedStructure!;
        }

        var endpoint = httpConfiguration.Endpoints.Single(x => x.Name == HttpEndpoint.HRM_Structure);

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(client.BaseAddress!, endpoint.Path),
            Method = HttpMethod.Parse(endpoint.Method)
        };

        request.Headers.TryAddWithoutValidation("Authorization", token);

        var response = await client.SendAsync(request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var structure = await
                response.Content.ReadFromJsonAsync<ResponseWrapper<List<Organization>>>(cancellationToken);

            cache.Set(key, structure!, options: new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
            });

            return structure!;
        }

        throw new NotImplementedException();
    }

    public async Task<ResponseWrapper<HrmListResponse<WorkerResponse>>> GetWorkers(string token, string query, CancellationToken cancellationToken = default)
    {
        var endpoint = httpConfiguration.Endpoints.Single(x => x.Name == HttpEndpoint.HrmPro_Workers);

        var path = endpoint.Path;

        if (!string.IsNullOrWhiteSpace(query))
        {
            path += query;
        }

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(client.BaseAddress!, path),
            Method = HttpMethod.Parse(endpoint.Method)
        };

        request.Headers.TryAddWithoutValidation("Authorization", token);

        var response = await client.SendAsync(request,
            cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var workers = await
                response.Content.ReadFromJsonAsync<ResponseWrapper<HrmListResponse<WorkerResponse>>>(cancellationToken);

            return workers ?? throw new NotImplementedException();
        }

        throw new NotImplementedException();
    }

    public async Task<LoginResponse> Login(CancellationToken cancellationToken = default)
    {
        const string key = "hrm_pro_login";

        if (cache.TryGetValue(key, out LoginResponse? cachedResponse))
        {
            return cachedResponse!;
        }

        HttpEndpoint endpoint =
            httpConfiguration.Endpoints.Single(x => x.Name == HttpEndpoint.HRM_Login);

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(client.BaseAddress!, endpoint.Path),
            Method = HttpMethod.Parse(endpoint.Method),
        };

        var body = string.Format(endpoint.Body ?? throw new MissingConfigurationException(nameof(endpoint.Body)),
                                 configuration["Credentials:HRM_PRO:Phone"],
                                 configuration["Credentials:HRM_PRO:Password"]);

        using StringContent content = new(body,
                                          Encoding.UTF8,
                                          "application/json");

        request.Content = content;

        using HttpResponseMessage response =
            await client.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var loginResponse =
                await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken);

            cache.Set(key, loginResponse, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            return loginResponse!;
        }
        else
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync(cancellationToken));

            throw new NotImplementedException();
        }

    }
}
