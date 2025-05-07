using System.Net;
using System.Net.Http.Json;
using System.Text;
using Contacts.Application.Common.Errors;
using Contacts.Application.Common.Exceptions;
using Contacts.Application.Common.Extensions;
using Contacts.Application.Common.Settings;
using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models;
using Contacts.Application.ProcessingServices.Models.Responses.Hrm;
using ErrorOr;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Contacts.Infrastructure.ProcessingServices;

internal class HrmClient : IHrmClient
{
    private readonly IMemoryCache cache;
    private readonly HttpClient client;
    private readonly HttpConfiguration httpConfiguration;
    private readonly IConfiguration configuration;

    public HrmClient(HttpClient client,
                     IMemoryCache cache,
                     IOptions<List<HttpConfiguration>> options,
                     IConfiguration configuration)
    {
        this.cache = cache;
        this.configuration = configuration;

        httpConfiguration = options.Value?.SingleOrDefault(x => x.Name == HttpConfiguration.HRM)
            ?? throw new MissingConfigurationException();

        this.client = client;

        this.client.BaseAddress = new Uri(httpConfiguration.BaseUrl);
    }

    public async ValueTask<List<OrganizationStructure>> GetStructure(string token, CancellationToken cancellationToken = default)
    {
        const string key = "hrm_structure";

        if (cache.TryGetValue(key, out List<OrganizationStructure>? cachedStructure))
        {
            return cachedStructure ?? [];
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
                response.Content.ReadFromJsonAsync<List<OrganizationStructure>>(cancellationToken);

            cache.Set(key, structure!, options: new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(45),
            });

            return structure!;
        }

        throw new NotImplementedException();
    }

    public async ValueTask<ErrorOr<Worker>> GetWorkerByPinfl(string pinfl,
                                                             string token,
                                                             CancellationToken cancellationToken = default)
    {
        string key = $"hrm_worker-{pinfl}";

        var cachedWorker = cache.Get<Worker>(key);

        if (cachedWorker is not null)
        {
            return cachedWorker;
        }

        HttpEndpoint endpoint =
            httpConfiguration.Endpoints.Single(x => x.Name == HttpEndpoint.HRM_CheckWorker);

        string url = endpoint.Path;

        url = url.BuildWithQueryParams(endpoint.QueryParams, pinfl);

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri(client.BaseAddress!, url),
            Method = HttpMethod.Parse(endpoint.Method),
        };

        request.Headers.TryAddWithoutValidation("Authorization", token);

        using HttpResponseMessage response =
            await client.SendAsync(request, cancellationToken);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var getWorkerResponse =
                await response.Content.ReadFromJsonAsync<CheckWorkerResponse>(cancellationToken);

            if (getWorkerResponse is null)
            {
                return ApplicationErrors.ExternalServerError;
            }

            cache.Set(key, getWorkerResponse.Worker, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return getWorkerResponse.Worker;
        }

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return ApplicationErrors.UserNotFoundInHRMSystem;
        }

        return ApplicationErrors.ExternalServerError;
    }

    public async ValueTask<LoginResponse> Login(CancellationToken cancellationToken = default)
    {
        const string key = "hrm_login";

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
                                 configuration["Credentials:HRM:Email"],
                                 configuration["Credentials:HRM:Password"]);

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
                // 1 minut before expiration
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(loginResponse!.ExpiresIn - 60)
            });

            return loginResponse;
        }

        throw new NotImplementedException();
    }
}
