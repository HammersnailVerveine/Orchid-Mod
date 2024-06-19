using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Accessories
{
	public class GuideShield : OrchidModGuardianItem
	{
		public override void SafeSetDefaults()
		{
			Item.width = 24;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.HeldItem.ModItem is OrchidModGuardianShield shield && Math.Abs((player.position.Y / 16f) - player.fallStart) > 25)
			{
				GuardianShieldAnchor anchor = shield.GetAnchor(player);
				if (anchor != null)
				{
					if (anchor.aimedLocation.Y > player.Center.Y && (Math.Abs(anchor.aimedLocation.X - player.Center.X) < 32f) && anchor.Projectile.ai[0] > 0f)
					{
						// Collision with the ground, do skating stuff
						Vector2 collision = Collision.TileCollision(player.position + new Vector2((player.width - Item.width) * 0.5f, player.height), Vector2.UnitY * 8f, Item.width, 16, false, false, (int)player.gravDir);
						if (collision != Vector2.UnitY * 8f)
						{
							player.fallStart = (int)(player.position.Y / 16f);
							player.fallStart2 = (int)(player.position.Y / 16f);
							player.GetModPlayer<OrchidGuardian>().AddSlam(1);
							SoundEngine.PlaySound(SoundID.Item37, anchor.Projectile.Center);
						}
					}
				}
			}
		}
	}
}