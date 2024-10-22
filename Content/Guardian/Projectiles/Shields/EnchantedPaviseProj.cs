using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using OrchidMod.Common.ModObjects;

namespace OrchidMod.Content.Guardian.Projectiles.Shields
{
	public class EnchantedPaviseProj : OrchidModGuardianProjectile
	{
		public override void SafeSetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 30;
			Projectile.friendly = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 60;
			Projectile.scale = 1f;
			Projectile.alpha = 96;
			Projectile.penetrate = -1;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			float lightMult = 0.25f + Math.Abs((1f * (Main.player[Main.myPlayer].GetModPlayer<OrchidPlayer>().Timer120 % 30) - 15) / 10f);
			return lightColor * lightMult;
		}

		public override void AI()
		{
			if (Projectile.velocity.Length() < 10f) Projectile.velocity *= 1.08f;
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.direction = Projectile.spriteDirection;

			if (Main.rand.NextBool(2))
			{
				int type = Main.rand.Next(3);
				if (type == 0) type = DustID.Enchanted_Gold;
				else if (type == 1) type = DustID.MagicMirror;
				else type = DustID.Enchanted_Pink;
				int dust = Dust.NewDust(Projectile.Center - new Vector2(10, 10), 20, 20, type);
				Main.dust[dust].velocity *= 0.2f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				Main.dust[dust].scale *= 1.8f;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int type = Main.rand.Next(3);
				if (type == 0) type = DustID.Enchanted_Gold;
				else if (type == 1) type = DustID.MagicMirror;
				else type = DustID.Enchanted_Pink;
				int dust = Dust.NewDust(Projectile.Center - new Vector2(10, 10), 20, 20, type);
				Main.dust[dust].velocity *= 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].noLight = true;
				Main.dust[dust].scale *= 1.8f;
			}
		}
	}
}