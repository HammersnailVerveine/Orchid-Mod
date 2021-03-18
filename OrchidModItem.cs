using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod
{
	public abstract class OrchidModItem : ModItem
    {
		public bool glowmask; // Does this item have a glowmask?

		public override bool CloneNewInstances => true;

		public virtual void DrawPlayerGlowmask(PlayerDrawInfo drawInfo) { }

		public virtual void DrawPlayerArmsGlowmask(PlayerDrawInfo drawInfo) { }
	}
}