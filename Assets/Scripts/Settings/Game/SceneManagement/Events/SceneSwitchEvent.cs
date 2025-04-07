using System;
using Game.Settings.Common.Events;

namespace Game.Settings.SceneManagement
{
    public class SceneSwitchEvent : CustomEventBase<IEventListener, EventArgs>
    {
        public override void EventRaiseAction(IEventListener listener, object sender, EventArgs args)
        {
            listener.Listen(sender, args);
        }
    }
}
