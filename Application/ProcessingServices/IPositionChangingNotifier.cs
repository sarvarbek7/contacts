using Contacts.Application.ProcessingServices.Models;

namespace Contacts.Application.ProcessingServices;

public interface IPositionChangingNotifier : IObservable<PositionChangedMessage>
{
    void Notify(PositionChangedMessage message);
}