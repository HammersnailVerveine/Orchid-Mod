using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using OrchidMod.Content.Guardian.Projectiles.Standards;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Warhammers
{
	public class DesertWarhammer : OrchidModGuardianHammer
	{

		public override void SafeSetDefaults()
		{
			Item.width = 36;
			Item.height = 36;
			Item.value = Item.sellPrice(0, 0, 30, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.DD2_MonkStaffSwing;
			Item.knockBack = 9f;
			Item.shootSpeed = 10f;
			Item.damage = 73;
			Item.useTime = 25;
			Range = 30;
			GuardStacks = 1;
			SlamStacks = 1;
			ReturnSpeed = 1f;
		}

		public override bool ThrowAI(Player player, OrchidGuardian guardian, Projectile projectile, bool weak)
		{

			if (Main.rand.NextBool(5))
			{
				Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Electric, Scale: Main.rand.NextFloat(0.8f, 1f));
				dust.velocity = dust.velocity * 0.25f + projectile.velocity * 0.2f;
			}

			return true;
		}

		//TODO: fix this up...
		/*public override void ExtraAI(Player player, OrchidGuardian guardian, Projectile projectile)
		{
			if (Main.rand.NextBool(60))
			{
				foreach (NPC npc in Main.npc)
				{
					if (OrchidModProjectile.IsValidTarget(npc) && npc.Center.Distance(projectile.Center) <= 160f + npc.width * 0.5f)
					{
						SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, projectile.Center);
						int type = ModContent.ProjectileType<DesertStandardProj>();
						Projectile newProj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), projectile.Center, Vector2.UnitY * -10f, type, (int)guardian.GetGuardianDamage(Item.damage * 0.35f), 1f, player.whoAmI);
						newProj.CritChance = guardian.GetGuardianCrit();
						break;
					}
				}
			}
		}*/

		public override void OnThrowHit(Player player, OrchidGuardian guardian, NPC target, Projectile projectile, float knockback, bool crit, bool Weak)
		{
			SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap, projectile.Center);
			/*int type = ModContent.ProjectileType<DesertStandardProj>();
			Projectile newProj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), target.Center, Vector2.UnitY * -10f, type, (int)guardian.GetGuardianDamage(Item.damage * 0.35f), 1f, player.whoAmI, target.whoAmI);
			newProj.CritChance = guardian.GetGuardianCrit();*/
		}
	}
}
