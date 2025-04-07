

using Game.Settings.Common.Events;
using System;

namespace Game.Settings.Common.Events
{
    public interface ICustomEvent<TListener, TEventArgs> : IDisposable
        where TListener : IEventListener
        where TEventArgs : EventArgs
    {
        public void Subscribe(TListener listener);
        public void Unsubscribe(TListener listener);
        public void RaiseEvent(object sender, TEventArgs args);
    }
}