using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace OrchidMod.Content.Guardian.Accessories
{
	public class GuideShieldBounce : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 32;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 0, 65, 0);
			Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.HeldItem.ModItem is OrchidModGuardianShield shield)
			{
				GuardianShieldAnchor anchor = shield.GetAnchor(player);
				if (anchor != null)
				{
					if (anchor.aimedLocation.Y > player.Center.Y && (Math.Abs(anchor.aimedLocation.X - player.Center.X) < 32f) && anchor.Projectile.ai[0] > 0f)
					{
						// Collision with the ground, do skating stuff
						Vector2 collision = Collision.TileCollision(player.position + new Vector2((player.width - Item.width) * 0.5f, player.height), Vector2.UnitY * 8f, Item.width, 16, true, true, (int)player.gravDir);
						if (collision != Vector2.UnitY * 8f && player.velocity.Y > 1f)
						{
							if (Math.Abs((player.position.Y / 16f) - player.fallStart) > 25) player.GetModPlayer<OrchidGuardian>().AddSlam(1);
							player.velocity.Y *= -1f;
							player.fallStart = (int)(player.position.Y / 16f);
							player.fallStart2 = (int)(player.position.Y / 16f);
							SoundEngine.PlaySound(SoundID.Item56, anchor.Projectile.Center);
						}
					}
				}
			}
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<GuideShield>();
			recipe.AddIngredient(ItemID.PinkGel, 25);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}