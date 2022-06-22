using Microsoft.Xna.Framework;
using OrchidMod.Alchemist.Projectiles;
using OrchidMod.Common.Globals.NPCs;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Weapons.Nature
{
	public class LivingSapVial : OrchidModAlchemistItem
	{
		public override void SafeSetDefaults()
		{
			Item.damage = 4;
			Item.width = 30;
			Item.height = 30;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 0, 15, 0);
			this.potencyCost = 1;
			this.element = AlchemistElement.NATURE;
			this.rightClickDust = 153;
			this.colorR = 255;
			this.colorG = 148;
			this.colorB = 0;
			this.secondaryDamage = 0;
			this.secondaryScaling = 3f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Living Sap Flask");
			Tooltip.SetDefault("Creates a healing living sap bubble if used with other ingredients"
							+ "\nIf an air element is used, healing is doubled"
							+ "\nHas a chance to release a bigger, catalytic sap bubble"
							+ "\nOn reaction, heals players and coats enemies in alchemical nature");
		}

		public override void KillSecond(int timeLeft, Player player, OrchidModPlayer modPlayer, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			if (alchProj.nbElements > 1) {
				int dmg = getSecondaryDamage(player, modPlayer, alchProj.nbElements);
				if (alchProj.airFlask.type != 0) {
					dmg *= 2;
				}
				int spawnProj = ProjectileType<Alchemist.Projectiles.Nature.LivingSapVialProj>();
				Vector2 vel = (new Vector2(0f, -2f).RotatedByRandom(MathHelper.ToRadians(20)));
				Projectile.NewProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, dmg, 0f, projectile.owner);
				int nb = 2 + Main.rand.Next(2);
				for (int i = 0; i < nb; i++)
				{
					vel = (new Vector2(0f, -(float)(3 + Main.rand.Next(4))).RotatedByRandom(MathHelper.ToRadians(90)));
					spawnProj = ProjectileType<Alchemist.Projectiles.Nature.LivingSapVialProjAlt>();
					Projectile.NewProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, vel, spawnProj, 0, 0f, projectile.owner);
				}
			}
		}

		public override void OnHitNPCSecond(NPC target, int damage, float knockback, bool crit, Player player, OrchidModPlayer modPlayer,
		OrchidModAlchemistNPC modTarget, OrchidGlobalNPC modTargetGlobal, AlchemistProj alchProj, Projectile projectile, OrchidModGlobalItem globalItem)
		{
			int rand = alchProj.nbElements;
			rand += alchProj.hasCloud() ? 2 : 0;
			if (Main.rand.Next(10) < rand)
			{
				int dmg = getSecondaryDamage(player, modPlayer, alchProj.nbElements + 5);
				int proj = ProjectileType<Alchemist.Projectiles.Reactive.LivingSapBubble>();
				Vector2 newVelocity = new Vector2(0f, -5f).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(player.GetSource_Misc("Alchemist Attack"), projectile.Center, newVelocity, proj, dmg, 0f, projectile.owner);
			}
		}
	}
}
