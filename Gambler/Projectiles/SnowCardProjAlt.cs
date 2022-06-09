using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class SnowCardProjAlt : OrchidModGamblerProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Flake");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 26;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 180;
			Projectile.penetrate = -1;
			this.gamblingChipChance = 5;
		}
		
		public override void OnSpawn() {
			for (int i = 0 ; i < 3 ; i ++) {
				Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67)].noGravity = true;
			}
		}

		public override void SafeAI()
		{
			Projectile.velocity *= 0.95f;
			Projectile.rotation += (10f - Projectile.velocity.Length()) / 45f;
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X / 2;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y / 2;
			SoundEngine.PlaySound(2, (int)Projectile.position.X, (int)Projectile.position.Y, 50);
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0 ; i < 5 ; i ++) {
				Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 67)].noGravity = true;
			}
		}
	}
}