using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models;
using Contacts.Domain.PhoneNumbers;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Contacts.Infrastructure.ProcessingServices;

public class PositionChangingReceiver(IServiceProvider serviceProvider,
    ILogger<PositionChangingReceiver> logger) : IPositionChangingReceiver
{
    public void OnCompleted()
    {
        logger.LogInformation("PositionChangingNotifier finishes sending messages.");
    }

    public void OnError(Exception error)
    {
        logger.LogError(error, "Subscriber receives error.");
    }

    public async void OnNext(PositionChangedMessage value)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var phoneNumberRepository = scope.ServiceProvider.GetRequiredService<IRepository<PhoneNumber, Guid>>();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var query = phoneNumberRepository.GetAll(x => x.ActiveAssignedPositionUser.ExternalId == value.UserExternalId &&
                                                      x.ActiveAssignedPositionId == value.PositionId,
                                                tracked: false);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        int? activeAssignPositionUserId = null;

        await query.ExecuteUpdateAsync(setters =>
            setters.SetProperty(x => x.ActiveAssignedPositionUserId,
                                     activeAssignPositionUserId));
    }
}
