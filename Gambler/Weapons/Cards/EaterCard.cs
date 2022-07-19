using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class EaterCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 8;
			Item.knockBack = 1f;
			Item.useAnimation = 30;
			Item.useTime = 30;

			this.cardRequirement = 5;
			this.cardSets = GamblerCardSets.Boss;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Corrupted Nightmare");
			Tooltip.SetDefault("Summon a corrupted worm, following your cursor"
							+ "\nRandomly releases rotten meat chunks"
							+ "\nCollecting them increases the worm damage and length");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.EaterCardProj1>();
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
				int newProjectile2 = (DummyProjectile(Projectile.NewProjectile(source, new Vector2(position.X - velocity.X * 3f, position.Y - velocity.Y * 3f), velocity, projType, damage, knockback, player.whoAmI), dummy));
				Main.projectile[newProjectile2].ai[0] = 1f;
				int newProjectile = (DummyProjectile(Projectile.NewProjectile(source, position, velocity, projType, damage, knockback, player.whoAmI), dummy));
				Main.projectile[newProjectile].ai[0] = 0f;
				Main.projectile[newProjectile2].ai[1] = newProjectile;
				Main.projectile[newProjectile].localAI[0] = newProjectile2;
				Main.projectile[newProjectile].netUpdate = true;
				Main.projectile[newProjectile2].netUpdate = true;
				SoundEngine.PlaySound(SoundID.Item1);
			}
			else
			{
				SoundEngine.PlaySound(SoundID.Item7);
			}
		}
	}
}
