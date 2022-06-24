using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Weapons.Cards
{
	public class IvyChestCard : OrchidModGamblerItem
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 11;
			Item.crit = 4;
			Item.knockBack = 0.8f;
			Item.useAnimation = 75;
			Item.useTime = 75;
			Item.shootSpeed = 5f;
			this.cardRequirement = 1;
			this.gamblerCardSets.Add("Elemental");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Playing Card : Deep Forest");
			Tooltip.SetDefault("Releases bursts of leaves, able to be pushed with the cursor"
							+ "\nThe further they are pushed, the more damage they deal"
							+ "\nOnly 3 sets of leaves can exist at once");
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, bool dummy = false)
		{
			int projType = ProjectileType<Gambler.Projectiles.IvyChestCardProj>();

			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == projType && proj.owner == player.whoAmI)
				{
					proj.ai[1]++;
					if (proj.ai[1] >= 3f)
					{
						proj.Kill();
					}
					proj.netUpdate = true;
				}
			}

			for (int i = 0; i < 5 + Main.rand.Next(4); i++)
			{
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				velocity = velocity.RotatedByRandom(MathHelper.ToRadians(10)) * scale;
				DummyProjectile(Projectile.NewProjectile(source, position, velocity, projType, damage, knockback, player.whoAmI), dummy);
			}

			SoundEngine.PlaySound(SoundID.Grass); // (6 : 0) Grass/Web Cut
		}
	}
}
