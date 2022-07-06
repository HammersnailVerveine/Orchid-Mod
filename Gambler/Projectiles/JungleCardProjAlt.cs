using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Gambler.Projectiles
{
	public class JungleCardProjAlt : OrchidModGamblerProjectile
	{
		bool spinDir;
		int baseTimeLeft;
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore");
		}

		public override void SafeSetDefaults()
		{
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.friendly = false;
			Projectile.tileCollide = true;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 600;
			Projectile.extraUpdates = 1;
			Main.projFrames[Projectile.type] = 2;
		}
		
		public override void OnSpawn() {
			for (int i = 0 ; i < 3 ; i ++) {
				Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163)].noGravity = true;
			}
			Projectile.frame = Main.rand.Next(2);
			spinDir = Main.rand.Next(2) == 0 ? true : false;
			baseTimeLeft = Projectile.timeLeft;
		}

		public override void SafeAI()
		{
			if (Projectile.timeLeft < baseTimeLeft - 120) {
				if (Projectile.friendly == false) {
					Projectile.timeLeft -= Main.rand.Next(20);
					Projectile.friendly = true;
					Projectile.netUpdate = true;
				}
				Projectile.velocity *= 0.975f;
				
				Vector2 move = Vector2.Zero;
				float distance = 200f;
				bool target = false;
				bool dummy = Projectile.GetGlobalProjectile<OrchidModGlobalProjectile>().gamblerDummyProj;
				for (int k = 0; k < 200; k++)
				{		
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly && Main.npc[k].lifeMax > 5 && (dummy ? Main.npc[k].type == NPCID.TargetDummy : Main.npc[k].type != NPCID.TargetDummy))
					{
						Vector2 newMove = Main.npc[k].Center - Projectile.Center;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						if (distanceTo < distance)
						{
							move = newMove;
							distance = distanceTo;
							target = true;
						}
					}
				}
				if (target)
				{
					if (Projectile.timeLeft % 60 == 0) {
						move.Normalize();
						move *= 3f;
						move = move.RotatedByRandom(MathHelper.ToRadians(Main.rand.Next(30)));
						Projectile.velocity = move;
						
						for (int i = 0 ; i < 2 ; i ++) {
							Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163)];
							dust.velocity *= 0.5f;
							dust.noGravity = true;
						}
					}
				} else {
					if (Projectile.timeLeft % 45 == 0) {
						Projectile.velocity.Normalize();
						Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(180));
						for (int i = 0 ; i < 2 ; i ++) {
							Dust dust = Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163)];
							dust.velocity *= 0.5f;
							dust.noGravity = true;
						}
					}
				}
			} else {
				Projectile.velocity *= 0.95f;
			}
			Projectile.rotation += (Projectile.velocity.Length() / 45f + 0.05f) * (spinDir ? 1 : -1);
		}
		
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X / 2;
			if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y / 2;
			return false;
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0 ; i < 3 ; i ++) {
				Main.dust[Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 163)].noGravity = true;
			}
		}
	}
}