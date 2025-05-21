using Contacts.Application.ProcessingServices;
using Contacts.Application.ProcessingServices.Models;

namespace Contacts.Infrastructure.ProcessingServices;

public class PositionChangingNotifier : IPositionChangingNotifier
{
    private readonly List<IObserver<PositionChangedMessage>> _subscribers = [];

    private readonly Lock _notifierLock = new();

    public void Notify(PositionChangedMessage message)
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber.OnNext(message);
        }
    }

    public IDisposable Subscribe(IObserver<PositionChangedMessage> observer)
    {
        if (observer is not null)
        {
            lock (_notifierLock)
            {
                _subscribers.Add(observer);
            }
        }

#pragma warning disable CS8604 // Possible null reference argument.
        return new Unsubscriber(_subscribers, observer);
#pragma warning restore CS8604 // Possible null reference argument.
    }

    private class Unsubscriber(List<IObserver<PositionChangedMessage>> subscribers, IObserver<PositionChangedMessage> observer) : IDisposable
    {
        private readonly Lock _unsubscriberLock = new();

        private readonly List<IObserver<PositionChangedMessage>> _subscribers = subscribers;
        private readonly IObserver<PositionChangedMessage> _observer = observer;

        public void Dispose()
        {
            if (_observer is not null)
            {
                lock (_unsubscriberLock)
                {
                    _subscribers.Remove(_observer);
                }
            }
        }
    }
}
