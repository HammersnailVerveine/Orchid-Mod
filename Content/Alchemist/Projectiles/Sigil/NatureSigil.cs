using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace OrchidMod.Content.Alchemist.Projectiles.Sigil
{
	public class NatureSigil : AlchemistSigil
	{
		protected static Texture2D outlineTexture;
		protected override Texture2D GetOutlineTexture() => outlineTexture;
		
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Alchemical Nature Sigil");
			element = AlchemistElement.NATURE;
			outlineTexture ??= ModContent.Request<Texture2D>("OrchidMod/Content/Alchemist/Projectiles/Sigil/NatureSigil_Outline", AssetRequestMode.ImmediateLoad).Value;
		}
	}
}
