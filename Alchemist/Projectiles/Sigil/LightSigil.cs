using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace OrchidMod.Alchemist.Projectiles.Sigil
{
	public class LightSigil : AlchemistSigil
	{
		protected static Texture2D outlineTexture;
		protected override Texture2D GetOutlineTexture() => outlineTexture;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alchemical Light Sigil");
			element = AlchemistElement.LIGHT;
			outlineTexture ??= ModContent.Request<Texture2D>("OrchidMod/Alchemist/Projectiles/Sigil/LightSigil_Outline", AssetRequestMode.ImmediateLoad).Value;
		}
	}
}
