using Game.Settings.Common.Events;
using System;

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
