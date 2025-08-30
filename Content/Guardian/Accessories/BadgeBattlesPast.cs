using System;
using System.Linq;
using OrchidMod.Content.Guardian.Weapons.Shields;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Accessories
{
	public class BadgeBattlesPast : OrchidModGuardianEquipable
	{
		float damageIncrease = 0f;
		int timer = 0;

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();

			int projectileType = ModContent.ProjectileType<GuardianShieldAnchor>();
			if (player.ownedProjectileCounts[projectileType] > 0)
			{
				var guardian = player.GetModPlayer<OrchidGuardian>();
				Projectile proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == projectileType);
				if (proj.localAI[0] - proj.ai[0] < 2)
				{
					damageIncrease = 0f;
					timer = 0;
				}

				if (proj.ai[0] > 0)
				{
					damageIncrease += 0.008f;
					timer = 30;
				}
				else if (timer > 0)
				{
					timer--;
					if (timer <= 0 || proj.ai[1] > 0)
					{
						damageIncrease = 0f;
						timer = 0;
					}
				}
				player.GetDamage<GuardianDamageClass>() += damageIncrease / (1 + guardian.GuardianBronzeShieldDamage);
			}
			else
			{
				timer = 0;
				damageIncrease = 0f;
			}
		}
	}
}