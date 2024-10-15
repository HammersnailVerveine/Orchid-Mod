using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian;
using OrchidMod.Content.Shapeshifter;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod
{
	public class OrchidShapeshifter : ModPlayer
	{
		public OrchidPlayer modPlayer;
		public ShapeshifterShapeshiftAnchor ShapeshiftAnchor;
		public OrchidModShapeshifterShapeshift Shapeshift;

		// Can be edited by gear


		// Set effects, accessories, misc


		// Dynamic gameplay and UI fields

		public override void HideDrawLayers(PlayerDrawSet drawInfo)
		{
			if (ShapeshiftAnchor != null)
			{
				foreach (var layer in PlayerDrawLayerLoader.DrawOrder)
				{
				layer.Hide();
				}
			}
		}

		public override void Initialize()
		{
			modPlayer = Player.GetModPlayer<OrchidPlayer>();
		}

		public override void PostUpdate()
		{
			if (ShapeshiftAnchor != null)
			{
				Player.width = Shapeshift.ShapeshiftWidth;
				Player.height = Shapeshift.ShapeshiftHeight;
				Shapeshift.ShapeshiftAnchorAI(ShapeshiftAnchor.Projectile, ShapeshiftAnchor);
				Player.velocity = ShapeshiftAnchor.Projectile.velocity;
				Player.Center = ShapeshiftAnchor.Projectile.Center;
			}
			else
			{
				//Player.width = Player.defaultWidth;
				//Player.height = Player.defaultHeight;
				Shapeshift = null;
			}
		}

		public override void OnRespawn()
		{
		}

		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
		}
	}
}
