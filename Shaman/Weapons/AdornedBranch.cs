using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace OrchidMod.Shaman.Weapons
{
	public class AdornedBranch : OrchidModShamanItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 5;
			Item.width = 34;
			Item.height = 32;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.knockBack = 1.25f;
			Item.rare = 1;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.UseSound = SoundID.Item8;
			Item.shootSpeed = 3f;
			Item.shoot = Mod.Find<ModProjectile>("AdornedBranchProj").Type;
			this.empowermentType = 1;
			this.energy = 5;
		}

		public override void SafeSetStaticDefaults()
		{
			DisplayName.SetDefault("Adorned Branch");
			Tooltip.SetDefault("Shoots a burst of splinters"
							  + "\nShoots more projectiles if you have 2 or more active shamanic bonds");
		}

		public override bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();

			for (int i = 0; i < Main.rand.Next(3) + 3; i++)
			{
				Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.ToRadians(30));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale;
				this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}

			if (OrchidModShamanHelper.getNbShamanicBonds(player, modPlayer, Mod) > 1)
			{
				for (int i = 0; i < Main.rand.Next(3); i++)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
					float scale = 1f - (Main.rand.NextFloat() * .3f);
					perturbedSpeed = perturbedSpeed * scale;
					this.NewShamanProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
				}
			}
			return false;
		}
	}
}

