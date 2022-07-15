namespace FemurNeedGenerator
{
    using System;
    using System.Linq;
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.Events.EventArgs;

    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "moddedmcplayer (made for Peach#4865)";
        public override string Name { get; } = "FemurNeedGenerator";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(5, 2, 1);

        private EventHandler Handler;
        
        public override void OnEnabled()
        {
            Handler = new EventHandler(this);
            Exiled.Events.Handlers.Scp106.Containing += Handler.Containing;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Scp106.Containing -= Handler.Containing;
            Handler = null;
            base.OnDisabled();
        }
    }

    public class EventHandler
    {
        private Config _cfg;
        public EventHandler(Plugin plg) => _cfg = plg.Config;
        
        public void Containing(ContainingEventArgs ev)
        {
            if (Generator.Get(GeneratorState.Engaged).Count() < _cfg.RequiredAmount)
            {
                ev.ButtonPresser.Broadcast(5, "Missing " + (_cfg.RequiredAmount - Generator.Get(GeneratorState.Engaged).Count()) + " generators.");
                ev.IsAllowed = false;
            }
        }
    }
}