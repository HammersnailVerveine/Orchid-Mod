using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Weapons.Cards
{
	public class KingSlimeCard : OrchidModGamblerCard
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 31;
			Item.knockBack = 1f;
			Item.shootSpeed = 10f;
			Item.useAnimation = 30;
			Item.useTime = 30;

			cardRequirement = 3;
			cardSets.Add(GamblerCardSet.Boss);
			cardSets.Add(GamblerCardSet.Slime);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Playing Card : Blue King");
			/* Tooltip.SetDefault("Summons a bouncy slime, following your cursor, and jumping up to it"
							+ "\nGains in damage with fall distance and enemy hits, touching on the ground resets it"); */
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			int projType = ProjectileType<Content.Gambler.Projectiles.KingSlimeCardProj>();
			bool found = false;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
				{
					found = true;
					break;
				}
			}

			if (!found)
			{
				DummyProjectile(Projectile.NewProjectile(source, position, velocity, projType, damage, knockback, player.whoAmI), dummy);
				SoundEngine.PlaySound(SoundID.Item1);
			}
			else
			{
				SoundEngine.PlaySound(SoundID.Item7);
			}
		}
	}
}
