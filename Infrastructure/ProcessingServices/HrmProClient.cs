using System.Collections.Specialized;
using System.Dynamic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Settings;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models.Responses.HrmPro;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Contacts.Infrastructure.ProcessingServices;

internal class HrmProClient : IHrmProClient
{
    private readonly IMemoryCache cache;
    private readonly HttpClient client;
    private readonly HttpConfiguration httpConfiguration;
    private readonly IConfiguration configuration;

    public HrmProClient(HttpClient client,
                        IMemoryCache cache,
                        IOptions<List<HttpConfiguration>> options,
                        IConfiguration configuration)
    {
        this.cache = cache;
        this.configuration = configuration;

        httpConfiguration = options.Value?.SingleOrDefault(x => x.Name == HttpConfiguration.HRM_PRO)
            ?? throw new MissingConfigurationException();

        this.client = client;

        this.client.BaseAddress = new Uri(httpConfiguration.BaseUrl);
    }
    public async Task<ResponseWrapper<List<Department>>> GetDepartments(string token, string query, CancellationToken cancellationToken = default)
    {
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
            var departments = await
                response.Content.ReadFromJsonAsync<ResponseWrapper<List<Department>>>(cancellationToken);

            return departments ?? throw new NotImplementedException();
        }

        throw new NotImplementedException();
    }

    public async Task<ResponseWrapper<HrmListResponse<Position>>> GetPositions(string token, string query, CancellationToken cancellationToken = default)
    {
        var endpoint = httpConfiguration.Endpoints.Single(x => x.Name == HttpEndpoint.HrmPro_Positions);

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
            var positions = await
                response.Content.ReadFromJsonAsync<ResponseWrapper<HrmListResponse<Position>>>(cancellationToken);

            return positions ?? throw new NotImplementedException();
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
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(45),
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
            Console.WriteLine(await response.Content.ReadAsStringAsync());

            throw new NotImplementedException();
        }

    }
}
