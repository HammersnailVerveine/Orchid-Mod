using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace OrchidMod.Common
{
	public class OrchidKeybinds : ILoadable
	{
		public static ModKeybind AlchemistReaction { get; private set; }
		public static ModKeybind AlchemistCatalyst { get; private set; }

		void ILoadable.Load(Mod mod)
		{
			AlchemistReaction = KeybindLoader.RegisterKeybind(mod, "Alchemist Hidden Reaction", "Mouse3");
			AlchemistCatalyst = KeybindLoader.RegisterKeybind(mod, "Alchemist Catalyst Tool Shortcut", Keys.Z);
		}

		void ILoadable.Unload()
		{
			AlchemistReaction = null;
			AlchemistCatalyst = null;
		}
	}
}