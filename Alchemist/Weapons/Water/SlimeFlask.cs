using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class SlimeFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 8;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 2, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 29;
			this.colorR = 89;
			this.colorG = 152;
			this.colorB = 253;
			this.secondaryScaling = 5f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gelatinous Samples");
			Tooltip.SetDefault("Ignites when a fire element is used in the same reaction, causing a deflagration"
							+ "\n'Handcrafted jars are unfit for precise alchemy'");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.Bottle, 1);
			recipe.AddIngredient(ItemID.Gel, 5);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (alchProj.fireFlask.type != 0)
			{
				int type = ProjectileType<Alchemist.Projectiles.Water.SlimeFlaskProj>();
				int dmg = getSecondaryDamage(player, modPlayer, alchProj.nbElements);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, type, dmg, 0.5f, projectile.owner);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 10, 10, true, 1f, 1f, 5f, true, true, false, 0, 0, true);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 10, 10, true, 1.5f, 1f, 2f, true, true, false, 0, 0, true);
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 45);

				if (player.HasBuff(BuffType<Alchemist.Buffs.SlimeFlaskBuff>()))
				{
					int nb = 2 + Main.rand.Next(3);
					for (int i = 0; i < nb; i++)
					{
						Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.EmberVialProjAlt>();
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, spawnProj, 0, 0f, projectile.owner);
					}
					int itemType = ItemType<Alchemist.Weapons.Fire.EmberVial>();
					int dmgAlt = OrchidModAlchemistHelper.getSecondaryDamage(player, modPlayer, itemType, alchProj.nbElements, true);
					int rand = alchProj.nbElements + Main.rand.Next(2);
					for (int i = 0; i < rand; i++)
					{
						Vector2 vel = (new Vector2(0f, -3f).RotatedByRandom(MathHelper.ToRadians(60)));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vel.X, vel.Y, ProjectileType<Alchemist.Projectiles.Fire.EmberVialProj>(), dmgAlt, 0f, projectile.owner);
					}
				}
			}
		}
	}
}
