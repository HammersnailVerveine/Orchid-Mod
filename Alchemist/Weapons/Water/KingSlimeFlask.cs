using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Water
{
	public class KingSlimeFlask : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			item.damage = 16;
			item.width = 30;
			item.height = 30;
			item.rare = 1;
			item.value = Item.sellPrice(0, 0, 10, 0);
			this.potencyCost = 2;
			this.element = AlchemistElement.WATER;
			this.rightClickDust = 56;
			this.colorR = 21;
			this.colorG = 83;
			this.colorB = 214;
			this.secondaryDamage = 5;
			this.secondaryScaling = 4f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slimy Flask");

			Tooltip.SetDefault("Makes hit enemies bouncy and susceptible to fall damage"
							+ "\nHas a chance to release a catalytic slime bubble");
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
			rand += player.HasBuff(BuffType<Alchemist.Buffs.KingSlimeFlaskBuff>()) ? 2 : 0;
			if (Main.rand.Next(10) < rand && !alchProj.noCatalyticSpawn)
			{
				int dmg = getSecondaryDamage(modPlayer, alchProj.nbElements);
				dmg += player.HasBuff(BuffType<Alchemist.Buffs.KingSlimeFlaskBuff>()) ? 5 : 0;
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.SlimeBubble>();
				Vector2 perturbedSpeed = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, proj, dmg, 0f, projectile.owner);
			}
		}
	}
}
