using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles.Sigil
{
	public class FireSigil : AlchemistSigil
	{
		protected static Texture2D outlineTexture;
		protected override Texture2D GetOutlineTexture() => outlineTexture;
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Alchemical Fire Sigil");
			element = AlchemistElement.FIRE;
			outlineTexture ??= ModContent.Request<Texture2D>("OrchidMod/Alchemist/Projectiles/Sigil/FireSigil_Outline", AssetRequestMode.ImmediateLoad).Value;
		}
	}
}
