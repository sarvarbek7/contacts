using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Nodes;
using Application.Services.Foundations;
using Contacts.Application.Handlers.Interfaces;
using Contacts.Application.ProcessingServices;
using Contacts.Domain.PhoneNumbers;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.Handlers;

internal class HrmHandler(IHrmProClient httpClient,
                          IBaseService<PhoneNumber, Guid> phoneNumberService) : IHrmHandler
{
    public async Task<ExpandoObject> GetPositionsWithPhoneNumbers(string queryParams, CancellationToken cancellationToken = default)
    {
//         {
//   "message": true,
//   "error": false,
//   "data": {
//     "current_page": 1,
//     "total": 1,
//     "data": [
//       {
//         "id": 1,
//         "position": {
//           "id": 84,
//           "name": "boshqaruv raisi"
//         },
//         "rate": 1,
//         "real_rate": 1
//       }
//     ]
//   }
// }
        var login = await httpClient.Login(cancellationToken);

        dynamic positionsResponse = await httpClient.GetPositions(login.TokenValue, queryParams, cancellationToken);

        List<(int index, int workerId)> bag = [];

        JsonElement positionData = positionsResponse.data.GetProperty("data");

        var positions = JsonArray.Create(positionData)!;

        for (int i = 0; i < positions.Count; i++)
        {
            bag.Add((i, (int)positions[i]!["position"]!["id"]!));
        }

        var positionIds = bag.Select(x => x.workerId).ToList();

        List<PhoneNumber> phoneNumbers = [];

        if (positionIds.Count > 0)
        {
            var storedPhoneNumbersForWorkers = await phoneNumberService.GetAll(x => positionIds.Contains(x.ActiveAssignedPositionId!.Value),
                                                                               tracked: false)
                .ToListAsync(cancellationToken);

            phoneNumbers.AddRange(storedPhoneNumbersForWorkers);
        }


        foreach (var (index, positionId) in bag)
        {
            var userPhoneNumbers =
                phoneNumbers.Where(x => x.ActiveAssignedPositionId == positionId);

            if (userPhoneNumbers.Any())
            {
                JsonNode? position = positions[index];

                JsonArray phoneNumbersJsonArray = [];

                foreach (var phoneNumber in userPhoneNumbers)
                {
                    JsonObject phoneNumberJson = [];
                    phoneNumberJson.Add("id", phoneNumber.Id);
                    phoneNumberJson.Add("number", phoneNumber.Number);

                    phoneNumbersJsonArray.Add(phoneNumberJson);
                }

                position!.AsObject().Add("phoneNumbers", phoneNumbersJsonArray);

                positions[index] = position.DeepClone();
            }
        }

        var updatedData = new JsonObject
        {
            ["data"] = positions
        };

        // Replace the 'data' property in workersResponse
        var workersResponseObject = (IDictionary<string, object>)positionsResponse;
        workersResponseObject["data"] = updatedData;

        return positionsResponse;
    }

    public async Task<ExpandoObject> GetWorkersWithPhoneNumbers(string queryParams, CancellationToken cancellationToken = default)
    {
        var login = await httpClient.Login(cancellationToken);

        dynamic workersResponse = await httpClient.GetWorkers(login.TokenValue, queryParams, cancellationToken);

        List<(int index, int workerId)> bag = [];

        JsonElement workerData = workersResponse.data.GetProperty("data");

        var workers = JsonArray.Create(workerData)!;

        for (int i = 0; i < workers.Count; i++)
        {
            bag.Add((i, (int)workers[i]!["worker"]!["id"]!));
        }

        var workerIds = bag.Select(x => x.workerId).ToList();

        List<PhoneNumber> phoneNumbers = [];

        if (workerIds.Count > 0)
        {
            var storedPhoneNumbersForWorkers = await phoneNumberService.GetAll(x => workerIds.Contains(x.ActiveAssignedUser.ExternalId),
                                                                               tracked: false)
                .Include(x => x.ActiveAssignedUser)
                .ToListAsync(cancellationToken);

            phoneNumbers.AddRange(storedPhoneNumbersForWorkers);
        }


        foreach (var (index, workerId) in bag)
        {
            var userPhoneNumbers =
                phoneNumbers.Where(x => x.ActiveAssignedUser!.ExternalId == workerId);

            if (userPhoneNumbers.Any())
            {
                JsonNode? worker = workers[index];

                JsonArray phoneNumbersJsonArray = [];

                foreach (var phoneNumber in userPhoneNumbers)
                {
                    JsonObject phoneNumberJson = [];
                    phoneNumberJson.Add("id", phoneNumber.Id);
                    phoneNumberJson.Add("number", phoneNumber.Number);

                    phoneNumbersJsonArray.Add(phoneNumberJson);
                }

                worker.AsObject().Add("phoneNumbers", phoneNumbersJsonArray);

                workers[index] = worker.DeepClone();
            }
        }

        var updatedData = new JsonObject
        {
            ["data"] = workers
        };

        // Replace the 'data' property in workersResponse
        var workersResponseObject = (IDictionary<string, object>)workersResponse;
        workersResponseObject["data"] = updatedData;

        return workersResponse;
    }
}
