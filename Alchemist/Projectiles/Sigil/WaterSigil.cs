using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles.Sigil
{
	public class WaterSigil : AlchemistSigil
	{
		protected static Texture2D outlineTexture;
		protected override Texture2D GetOutlineTexture() => outlineTexture;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemical Water Sigil");
			element = AlchemistElement.WATER;
			outlineTexture ??= ModContent.Request<Texture2D>("OrchidMod/Alchemist/Projectiles/Sigil/WaterSigil_Outline", AssetRequestMode.ImmediateLoad).Value;
		}
	}
}
