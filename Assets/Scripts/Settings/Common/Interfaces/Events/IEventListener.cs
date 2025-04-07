

using System;

namespace Game.Settings.Common.Events
{
    public interface IEventListener
    {
        public void Listen(object sender, EventArgs e);
    }
}
