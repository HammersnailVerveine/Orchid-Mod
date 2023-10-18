using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Shaman.Projectiles.Thorium.OreOrbs.Big
{
	public class ValadiumScepterProj : OrchidModShamanProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 30;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
		}

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Valadium Bolt");
		}

		public override void AI()
		{
			Projectile.velocity.Y -= 0.15f;

			int dust = Dust.NewDust(Projectile.position, 1, 1, 70);
			Main.dust[dust].velocity /= 10f;
			Main.dust[dust].scale = 1f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].noLight = false;
			int dust2 = Dust.NewDust(Projectile.position, 1, 1, 112);
			Main.dust[dust2].velocity /= 1f;
			Main.dust[dust2].scale = 0.8f;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust2].noLight = true;
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 70);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity *= 10f;
			}
			spawnDustCircle(70, 10 + Main.rand.Next(16));
		}

		public void spawnDustCircle(int dustType, int distToCenter)
		{
			for (int i = 0; i < 10; i++)
			{
				double deg = (i * (72 + 5 - Main.rand.Next(10)));
				double rad = deg * (Math.PI / 180);

				float posX = Projectile.Center.X - (int)(Math.Cos(rad) * distToCenter) - Projectile.width / 2 + Projectile.velocity.X + 4;
				float posY = Projectile.Center.Y - (int)(Math.Sin(rad) * distToCenter) - Projectile.height / 2 + Projectile.velocity.Y + 4;

				Vector2 dustPosition = new Vector2(posX, posY);

				int index2 = Dust.NewDust(dustPosition, 1, 1, dustType, 0.0f, 0.0f, 0, new Color(), Main.rand.Next(30, 130) * 0.013f);

				Main.dust[index2].velocity *= 0f;
				Main.dust[index2].fadeIn = 1f;
				Main.dust[index2].scale = 1.5f;
				Main.dust[index2].noGravity = true;
			}
		}

		public override void SafeOnHitNPC(NPC target, int damage, float knockback, bool crit, Player player, OrchidShaman modPlayer)
		{
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null && Main.rand.Next(2) == 0)
			{
				target.AddBuff((thoriumMod.Find<ModBuff>("LightCurse").Type), 1 * 60);
			}

			if (modPlayer.shamanOrbBig != ShamanOrbBig.VALADIUM)
			{
				modPlayer.shamanOrbBig = ShamanOrbBig.VALADIUM;
				modPlayer.orbCountBig = 0;
			}
			modPlayer.orbCountBig++;

			if (modPlayer.orbCountBig == 3)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 30, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("ValadiumOrb").Type, 0, 0, Projectile.owner, 0f, 0f);

				if (player.FindBuffIndex(Mod.Find<ModBuff>("ShamanicBaubles").Type) > -1)
				{
					modPlayer.orbCountBig += 3;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("ValadiumOrb").Type, 1, 0, Projectile.owner, 0f, 0f);
					player.ClearBuff(Mod.Find<ModBuff>("ShamanicBaubles").Type);
				}
			}
			if (modPlayer.orbCountBig == 6)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X - 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("ValadiumOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 9)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X, player.position.Y - 40, 0f, 0f, Mod.Find<ModProjectile>("ValadiumOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 12)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 15, player.position.Y - 38, 0f, 0f, Mod.Find<ModProjectile>("ValadiumOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig == 15)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), player.Center.X + 30, player.position.Y - 30, 0f, 0f, Mod.Find<ModProjectile>("ValadiumOrb").Type, 0, 0, Projectile.owner, 0f, 0f);
			if (modPlayer.orbCountBig > 15)
			{
				knockback = 0f;
				if (!(target.boss || target.type == NPCID.TargetDummy) && target.knockBackResist > 0f)
				{
					target.velocity.X = Projectile.velocity.X > 0f ? 8f : -8f;
					target.velocity.Y = -10f;
					target.AddBuff((Mod.Find<ModBuff>("AquaBump").Type), 10 * 60);
				}
				spawnDustCircle(70, 35);
				modPlayer.orbCountBig = -3;
			}
		}
	}
}