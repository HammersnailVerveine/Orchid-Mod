using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Map;
using Terraria.ModLoader;
using Terraria.UI;

namespace OrchidMod.Content.Shapeshifter
{
	public class ShapeshifterMapLayer : ModMapLayer
	{
		public override void Draw(ref MapOverlayDrawContext context, ref string text) {
			const float scaleIfNotSelected = 1f;
			const float scaleIfSelected = 1f;

			foreach (Player player in Main.player)
			{
				if (player.active)
				{
					ShapeshifterShapeshiftAnchor anchor = player.GetModPlayer<OrchidShapeshifter>().ShapeshiftAnchor;
					if (anchor != null)
					{
						if (context.Draw(anchor.TextureShapeshiftIcon, anchor.Projectile.Center / 16f, Color.White, new SpriteFrame(1, 1, 0, 0), scaleIfNotSelected, scaleIfSelected, Alignment.Center).IsMouseOver)
						{
							text = player.name;
						}
					}
				}
			}

		}
	}
}