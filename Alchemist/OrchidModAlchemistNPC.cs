using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
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

		public bool airborne = false;

		public override bool InstancePerEntity => true;

		public void spreadOilFire(Vector2 startPosition, int damage, Player player)
		{
			if (Main.myPlayer == player.whoAmI)
			{
				OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
				float distance = 1000f;
				for (int k = 0; k < Main.maxNPCs ; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].dontTakeDamage && !Main.npc[k].friendly)
					{
						Vector2 newMove = Main.npc[k].Center - startPosition;
						float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
						if (distanceTo < distance)
						{
							OrchidModAlchemistNPC modTarget = Main.npc[k].GetGlobalNPC<OrchidModAlchemistNPC>();
							if (modTarget.alchemistWater > 0)
							{
								bool crit = (Main.rand.Next(101) <= modPlayer.alchemistCrit + 4);
								player.ApplyDamageToNPC(Main.npc[k], Main.DamageVar(damage), 5f, player.direction, crit);
								modTarget.alchemistWater = 0;
								modTarget.alchemistFire = 60 * 10;
							}
						}
					}
				}
			}
		}

		public void setOffset(ref int offSetX, ref int offSetY, ref int nbCoatings)
		{
			offSetX = -9;
			while (nbCoatings > 0 && offSetX > -29)
			{
				nbCoatings--;
				offSetX -= 10;
			}
			offSetY -= 20;
		}

		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			int nbCoatings = -1;
			nbCoatings += this.alchemistFire > 0 ? 1 : 0;
			nbCoatings += this.alchemistWater > 0 ? 1 : 0;
			nbCoatings += this.alchemistNature > 0 ? 1 : 0;
			nbCoatings += this.alchemistAir > 0 ? 1 : 0;
			nbCoatings += this.alchemistLight > 0 ? 1 : 0;
			nbCoatings += this.alchemistDark > 0 ? 1 : 0;
			int offSetX = -9;
			int offSetY = -30;
			setOffset(ref offSetX, ref offSetY, ref nbCoatings);
			nbCoatings--;

			if (this.alchemistFire > 0)
			{
				Rectangle rect = new Rectangle((int)(npc.Center.X + offSetX - Main.screenPosition.X), (int)(npc.Center.Y + offSetY - Main.screenPosition.Y), 18, 18);
				spriteBatch.Draw(OrchidMod.coatingTextures[0], rect, null, Color.White);
				offSetX += 20;
				if (offSetX > 30) setOffset(ref offSetX, ref offSetY, ref nbCoatings);
			}

			if (this.alchemistWater > 0)
			{
				Rectangle rect = new Rectangle((int)(npc.Center.X + offSetX - Main.screenPosition.X), (int)(npc.Center.Y + offSetY - Main.screenPosition.Y), 18, 18);
				spriteBatch.Draw(OrchidMod.coatingTextures[1], rect, null, Color.White);
				offSetX += 20;
				if (offSetX > 30) setOffset(ref offSetX, ref offSetY, ref nbCoatings);
			}

			if (this.alchemistNature > 0)
			{
				Rectangle rect = new Rectangle((int)(npc.Center.X + offSetX - Main.screenPosition.X), (int)(npc.Center.Y + offSetY - Main.screenPosition.Y), 18, 18);
				spriteBatch.Draw(OrchidMod.coatingTextures[2], rect, null, Color.White);
				offSetX += 20;
				if (offSetX > 30) setOffset(ref offSetX, ref offSetY, ref nbCoatings);
			}

			if (this.alchemistAir > 0)
			{
				Rectangle rect = new Rectangle((int)(npc.Center.X + offSetX - Main.screenPosition.X), (int)(npc.Center.Y + offSetY - Main.screenPosition.Y), 18, 18);
				spriteBatch.Draw(OrchidMod.coatingTextures[3], rect, null, Color.White);
				offSetX += 20;
				if (offSetX > 30) setOffset(ref offSetX, ref offSetY, ref nbCoatings);
			}

			if (this.alchemistLight > 0)
			{
				Rectangle rect = new Rectangle((int)(npc.Center.X + offSetX - Main.screenPosition.X), (int)(npc.Center.Y + offSetY - Main.screenPosition.Y), 18, 18);
				spriteBatch.Draw(OrchidMod.coatingTextures[4], rect, null, Color.White);
				offSetX += 20;
				if (offSetX > 30) setOffset(ref offSetX, ref offSetY, ref nbCoatings);
			}

			if (this.alchemistDark > 0)
			{
				Rectangle rect = new Rectangle((int)(npc.Center.X + offSetX - Main.screenPosition.X), (int)(npc.Center.Y + offSetY - Main.screenPosition.Y), 18, 18);
				spriteBatch.Draw(OrchidMod.coatingTextures[5], rect, null, Color.White);
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (npc.HasBuff(BuffType<Alchemist.Buffs.Debuffs.Attraction>()) && Main.time % 90 == 0)
			{
				OrchidModProjectile.spawnDustCircle(npc.Center, 60, 10, 10, true, 1.5f, 1f, 2f);
				OrchidModProjectile.spawnDustCircle(npc.Center, 60, 10, 12, true, 1.5f, 1f, 4f);
			}
		}

		public static bool AttractiteCanHome(NPC npc)
		{
			bool canHome = (npc.HasBuff(BuffType<Alchemist.Buffs.Debuffs.Attraction>()) || npc.HasBuff(BuffType<Alchemist.Buffs.Debuffs.Catalyzed>()));
			return canHome;
		}

		public override void ResetEffects(NPC npc)
		{
			this.alchemistFire -= this.alchemistFire > 0 ? 1 : 0;
			this.alchemistWater -= this.alchemistWater > 0 ? 1 : 0;
			this.alchemistAir -= this.alchemistAir > 0 ? 1 : 0;
			this.alchemistNature -= this.alchemistNature > 0 ? 1 : 0;
			this.alchemistLight -= this.alchemistLight > 0 ? 1 : 0;
			this.alchemistDark -= this.alchemistDark > 0 ? 1 : 0;
			this.airborne = npc.velocity.Y != 0 ? true : false;
		}
	}
}