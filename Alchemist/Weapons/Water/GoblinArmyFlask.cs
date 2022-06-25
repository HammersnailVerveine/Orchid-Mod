using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using OrchidMod.Common.Globals.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class GoblinArmyFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 10;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 184;
			this.colorR = 22;
			this.colorG = 22;
			this.colorB = 22;
			this.secondaryDamage = 50;
			this.secondaryScaling = 15f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblin Oil");
			Tooltip.SetDefault("Using a fire element in the same attack will drastically increase damage"
							+ "\nThis will also damage and spread alchemical fire to all nearby water coated enemies"
							+ "\nHas a chance to release a catalytic oil bubble, coating nearby enemies in water on reaction");
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidAlchemist modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (alchProj.fireFlask.type != 0)
			{
				int dmg = GetSecondaryDamage(player, alchProj.nbElements);
				modTarget.spreadOilFire(target.Center, dmg, Main.player[projectile.owner]);
				SoundEngine.PlaySound(SoundID.Item45);
			}

			int rand = alchProj.nbElements;
			rand += alchProj.hasCloud() ? 2 : 0;
			if (Main.rand.Next(6) < rand && !alchProj.noCatalyticSpawn)
			{
				int dmg = 0;
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.OilBubble>();
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
				SpawnProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, perturbedSpeed, proj, dmg, 0f, projectile.owner);
			}
		}

		public override void AddVariousEffects(Player player, OrchidAlchemist modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (alchProj.fireFlask.type != 0)
			{
				projectile.damage += (int)player.GetDamage<AlchemistDamageClass>().ApplyTo(30);
			}
		}
	}
}
