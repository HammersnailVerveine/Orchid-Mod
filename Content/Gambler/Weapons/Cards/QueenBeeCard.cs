using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Weapons.Cards
{
	public class QueenBeeCard : OrchidModGamblerCard
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 11;
			Item.knockBack = 1f;
			Item.useAnimation = 30;
			Item.useTime = 30;

			this.cardRequirement = 4;
			cardSets.Add(GamblerCardSet.Boss);
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Playing Card : Poisonous Queen");
			/* Tooltip.SetDefault("Summons a bee hive, following your cursor"
							+ "\nShake it to summon bees"); */
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			if (Main.mouseLeft && Main.mouseLeftRelease || modPlayer.gamblerJustSwitched)
			{
				int projType = ProjectileType<Content.Gambler.Projectiles.QueenBeeCardProj>();
				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
					{
						proj.Kill();
						break;
					}
				}
				modPlayer.gamblerJustSwitched = false;
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
