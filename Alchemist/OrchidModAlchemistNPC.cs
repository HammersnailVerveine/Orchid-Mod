using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist
{
    public class OrchidModAlchemistNPC : GlobalNPC
    {
		public int alchemistFire = 0;
		public int alchemistWater = 0;
		public int alchemistAir = 0;
		public int alchemistNature = 0;
		public int alchemistLight = 0;
		public int alchemistDark = 0;
		public int alchemistOil = 0;
		
		public bool airborne = false;
		
		public override bool InstancePerEntity => true;
		
		public void spreadOilFire(Vector2 startPosition, int damage, Player player) {
			if (Main.myPlayer == player.whoAmI) {
				Vector2 move = Vector2.Zero;
				float distance = 1000f;
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly){
						Vector2 newMove = Main.npc[k].Center - startPosition;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						if (distanceTo < distance) {
							OrchidModAlchemistNPC modTarget = Main.npc[k].GetGlobalNPC<OrchidModAlchemistNPC>();
							if (modTarget.alchemistOil > 0) {
								player.ApplyDamageToNPC(Main.npc[k], Main.DamageVar(damage), 5f, Main.LocalPlayer.direction, true);
								modTarget.alchemistOil = 0;
								modTarget.alchemistFire = 60 * 5;
							}
						}
					}
				}
			}
		}
		
		public override void DrawEffects(NPC npc, ref Color drawColor) {
			if (this.alchemistFire > 0) {
				if (Main.rand.Next(15) == 0) {
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 6, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.5f;
					Main.dust[dust].velocity.X *= 0f;
					if (Main.rand.NextBool(4)) {
						Main.dust[dust].noGravity = false;
						Main.dust[dust].scale *= 0.5f;
					}
				}
				Lighting.AddLight(npc.position, 0.5f, 0.2f, 0f);
			}
			
			if (this.alchemistOil > 0) {
				if (Main.rand.Next(15) == 0) {
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 184, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.5f;
					Main.dust[dust].velocity.X *= 0f;
					Main.dust[dust].scale *= 0.5f;
				}
				Lighting.AddLight(npc.position, 0.5f, 0.5f, 0f);
			}
			
			if (this.alchemistWater > 0) {
				if (Main.rand.Next(15) == 0) {
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 33, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.5f;
					Main.dust[dust].velocity.X *= 0f;
					Main.dust[dust].scale *= 0.5f;
				}
				Lighting.AddLight(npc.position, 0f, 0f, 0.5f);
			}
			
			if (this.alchemistNature > 0) {
				if (Main.rand.Next(15) == 0) {
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 163, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.5f;
					Main.dust[dust].velocity.X *= 0f;
					Main.dust[dust].scale *= 0.5f;
				}
				Lighting.AddLight(npc.position, 0f, 0.5f, 0.2f);
			}
			
			if (this.alchemistAir > 0) {
				if (Main.rand.Next(15) == 0) {
					int dust = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width + 4, npc.height + 4, 16, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100, default(Color), 3.5f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity *= 0.5f;
					Main.dust[dust].velocity.X *= 0f;
					Main.dust[dust].scale *= 0.5f;
				}
				Lighting.AddLight(npc.position, 0.1f, 0.3f, 0.3f);
			}
			
			if (npc.HasBuff(BuffType<Alchemist.Buffs.Debuffs.Attraction>()) && Main.time % 90 == 0) {
				OrchidModProjectile.spawnDustCircle(npc.Center, 60, 10, 10, true, 1.5f, 1f, 2f);
				OrchidModProjectile.spawnDustCircle(npc.Center, 60, 10, 12, true, 1.5f, 1f, 4f);
			}
		}
		
		public static bool AttractiteCanHome(NPC npc) {
			bool canHome = (npc.HasBuff(BuffType<Alchemist.Buffs.Debuffs.Attraction>()) || npc.HasBuff(BuffType<Alchemist.Buffs.Debuffs.Catalyzed>()));
			return canHome;
		}
		
		public override void ResetEffects(NPC npc) {
			this.alchemistFire -= this.alchemistFire > 0 ? 1 : 0;
			this.alchemistWater -= this.alchemistWater > 0 ? 1 : 0;
			this.alchemistAir -= this.alchemistAir > 0 ? 1 : 0;
			this.alchemistNature -= this.alchemistNature > 0 ? 1 : 0;
			this.alchemistLight -= this.alchemistLight > 0 ? 1 : 0;
			this.alchemistDark -= this.alchemistDark > 0 ? 1 : 0;
			this.alchemistOil -= this.alchemistOil > 0 ? 1 : 0;
			this.airborne = npc.velocity.Y != 0 ? true : false;
		}
	}
}  