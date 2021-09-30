using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Fire
{
	public class HellSlimeFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 25;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.FIRE;
			this.rightClickDust = 6;
			this.colorR = 253;
			this.colorG = 32;
			this.colorB = 3;
			this.secondaryDamage = 10;
			this.secondaryScaling = 10f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Slime Flask");
			Tooltip.SetDefault("Makes hit enemies bouncy and susceptible to fall damage"
							+ "\nHas a chance to release a catalytic lava slime bubble"
							+ "\nHitting a fire-coated enemy will spread lava droplets");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(null, "KingSlimeFlask", 1);
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient(null, "AlchemicStabilizer", 1);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidModGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f)
			{
				target.AddBuff(mod.BuffType("SlimeSlow"), 90 * (alchProj.nbElements * 2));
			}

			int rand = alchProj.nbElements;
			rand += alchProj.hasCloud() ? 2 : 0;
			if (Main.rand.Next(10) < rand && !alchProj.noCatalyticSpawn)
			{
				int dmg = getSecondaryDamage(player, modPlayer, alchProj.nbElements);
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.SlimeBubbleLava>();
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
			}

			if (modTarget.alchemistFire > 0)
			{
				int nb = Main.rand.Next(3) + 3;
				int dmg = (int)(getSecondaryDamage(player, modPlayer, alchProj.nbElements) * 1.5);
				int proj = ProjectileType<Alchemist.Projectiles.Fire.HellSlimeFlaskProj>();
				for (int i = 0; i < nb; i++)
				{
					Vector2 perturbedSpeed = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15)) * 0.75f;
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 1f, projectile.owner);
				}
			}
		}
	}
}
