using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class SlimeFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 8;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 2, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 29;
			this.colorR = 89;
			this.colorG = 152;
			this.colorB = 253;
			this.secondaryScaling = 8f;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gelatinous Samples");
			/* Tooltip.SetDefault("Ignites if mixed with a fire element, causing a deflagration"
							+ "\n'Handcrafted jars are unfit for precise alchemy'"); */
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.Bottle, 1);
			recipe.AddIngredient(ItemID.Gel, 5);
			recipe.Register();
		}

		public override void KillSecond(int timeLeft, Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (alchProj.fireFlask.type != 0)
			{
				int type = ProjectileType<Alchemist.Projectiles.Water.SlimeFlaskProj>();
				int dmg = GetSecondaryDamage(player, alchProj.nbElements);
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, Vector2.Zero, type, dmg, 0.5f, projectile.owner);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 10, 10, true, 1f, 1f, 5f, true, true, false, 0, 0, true);
				OrchidModProjectile.spawnDustCircle(projectile.Center, 6, 10, 10, true, 1.5f, 1f, 2f, true, true, false, 0, 0, true);
				SoundEngine.PlaySound(SoundID.Item45, projectile.Center);

				if (player.HasBuff(BuffType<Buffs.SlimeFlaskBuff>()))
				{
					int nb = 2 + Main.rand.Next(3);
					for (int i = 0; i < nb; i++)
					{
						Vector2 vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(80)));
						int spawnProj = ProjectileType<Alchemist.Projectiles.Fire.EmberVialProjAlt>();
						SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, 0, 0f, projectile.owner);
					}
					int itemType = ItemType<Alchemist.Weapons.Fire.EmberVial>();
					int dmgAlt = modPlayer.GetSecondaryDamage(itemType, alchProj.nbElements);
					int rand = alchProj.nbElements + Main.rand.Next(2);
					for (int i = 0; i < rand; i++)
					{
						Vector2 vel = (new Vector2(0f, -3f).RotatedByRandom(MathHelper.ToRadians(60)));
						SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, ProjectileType<Alchemist.Projectiles.Fire.EmberVialProj>(), dmgAlt, 0f, projectile.owner);
					}
				}
			}
		}
	}
}
