using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class DesertCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus spike");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 12;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.aiStyle = 1;
			Projectile.timeLeft = 60;
			Main.projFrames[Projectile.type] = 2;
		}
		
		public override void OnSpawn() {
			Projectile.frame = Main.rand.Next(2);
		}

		public override void Kill(int timeLeft)
		{
			Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 31)].noGravity = true;
		}
	}
}