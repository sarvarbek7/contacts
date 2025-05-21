using Contacts.Application.ProcessingServices.Models;

namespace Contacts.Application.ProcessingServices;

public interface IPositionChangingReceiver :  IObserver<PositionChangedMessage>;