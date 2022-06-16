using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using OrchidMod.Common.Globals.NPCs;
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
			Item.damage = 25;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 10, 0);
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
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.Anvils);
			recipe.AddIngredient(null, "KingSlimeFlask", 1);
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient(null, "AlchemicStabilizer", 1);
			recipe.Register();
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f)
			{
				target.AddBuff(Mod.Find<ModBuff>("SlimeSlow").Type, 90 * (alchProj.nbElements * 2));
			}

			int rand = alchProj.nbElements;
			rand += alchProj.hasCloud() ? 2 : 0;
			if (Main.rand.Next(10) < rand && !alchProj.noCatalyticSpawn)
			{
				int dmg = getSecondaryDamage(player, modPlayer, alchProj.nbElements);
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.SlimeBubbleLava>();
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, perturbedSpeed, proj, dmg, 0f, projectile.owner);
			}

			if (modTarget.alchemistFire > 0)
			{
				int nb = Main.rand.Next(3) + 3;
				int dmg = (int)(getSecondaryDamage(player, modPlayer, alchProj.nbElements) * 1.5);
				int proj = ProjectileType<Alchemist.Projectiles.Fire.HellSlimeFlaskProj>();
				for (int i = 0; i < nb; i++)
				{
					Vector2 perturbedSpeed = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15)) * 0.75f;
					Projectile.NewProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, perturbedSpeed, proj, dmg, 1f, projectile.owner);
				}
			}
		}
	}
}
