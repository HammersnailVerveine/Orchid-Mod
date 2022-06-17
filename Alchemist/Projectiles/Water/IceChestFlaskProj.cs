using OrchidMod.Alchemist.Weapons.Water;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Water
{
	public class IceChestFlaskProj : OrchidModAlchemistProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			for (int l = 0; l < Main.npc.Length; l++)
			{
				NPC target = Main.npc[l];
				if (Projectile.Hitbox.Intersects(target.Hitbox))
				{
					OrchidModAlchemistNPC modTarget = target.GetGlobalNPC<OrchidModAlchemistNPC>();
					target.AddBuff(BuffType<Debuffs.FlashFreeze>(), modTarget.alchemistWater > 0 ? 60 * 30 : 60 * 3);
				}
			}

			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (Projectile.owner == proj.owner && proj.active && Projectile.Hitbox.Intersects(proj.Hitbox))
				{
					if (IceChestFlask.smallProjectiles.Contains(proj.type))
					{
						int damage = Projectile.damage;
						int projType = ProjectileType<IceChestFlaskProjSmall>();
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), proj.Center.X, proj.Center.Y, 0f, 1f, projType, damage, 1f, Projectile.owner);
						proj.active = false;
					}
					if (IceChestFlask.bigProjectiles.Contains(proj.type))
					{
						int damage = Projectile.damage * 5;
						int projType = ProjectileType<IceChestFlaskProjBig>();
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), proj.Center.X, proj.Center.Y, 0f, 1f, projType, damage, 5f, Projectile.owner);
						proj.active = false;
					}
				}
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flash Freeze Aura");
		}
	}
}