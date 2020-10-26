using System;

namespace CashTrak.Extensions
{
    public static class EventExtensions
    {
        public static void Raise(this EventHandler handler, object sender, EventArgs args) =>
            handler?.Invoke(sender, args);

        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs args)
            where TEventArgs : EventArgs => handler?.Invoke(sender, args);
    }
}