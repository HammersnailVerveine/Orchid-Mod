using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.NPCs.Hostile.DownpourElemental
{
	public class DownpourElemental : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storm Spirit");
			Main.npcFrameCount[npc.type] = 12;
		}

		public override void SetDefaults()
		{
			npc.width = 178;
			npc.height = 110;
			npc.aiStyle = 14;
			npc.damage = 40;
			npc.defense = 30;
			npc.lifeMax = 5000;
			npc.HitSound = SoundID.NPCHit30;
			npc.DeathSound = SoundID.NPCDeath33;
			npc.value = 15000f;
			npc.knockBackResist = 0.05f;
			npc.friendly = false;
			npc.alpha = 16;
			npc.noTileCollide = true;
		}

		public override void NPCLoot()
		{
			Item.NewItem(npc.getRect(), mod.ItemType("DownpourCrystal"));
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 15; i++)
			{
				Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 16, 2 * hitDirection, -2f, 100);
				dust.noGravity = true;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return Main.raining && spawnInfo.player.ZoneSkyHeight && Main.hardMode ? .03f : 0f;
		}

		private const int AI_State_Slot = 0;
		private const int AI_Timer_Slot = 1;
		private const int AI_Anim_Slot = 2;
		private const int AI_State_Count = 3;

		private const int State_Moving = 0;
		private const int State_Casting = 1;

		public float AI_State
		{
			get => npc.ai[AI_State_Slot];
			set => npc.ai[AI_State_Slot] = value;
		}

		public float AI_Timer
		{
			get => npc.ai[AI_Timer_Slot];
			set => npc.ai[AI_Timer_Slot] = value;
		}

		public float AI_Anim
		{
			get => npc.ai[AI_Anim_Slot];
			set => npc.ai[AI_Anim_Slot] = value;
		}

		public float AI_Count
		{
			get => npc.ai[AI_State_Count];
			set => npc.ai[AI_State_Count] = value;
		}

		public override void AI()
		{
			AI_Timer++;

			if (AI_Timer >= 10 || (AI_Timer >= 5 && AI_State == State_Casting))
			{
				AI_Timer = 0;
				AI_Anim++;
				AI_Count++;
				if (AI_Anim == 12)
				{
					AI_Anim = 0;
				}
			}

			if (((int)AI_Count >= 120 && AI_State == State_Casting) || ((int)AI_Count >= 60 && AI_State == State_Moving))
			{
				AI_Count = 0;
				AI_State = AI_State == State_Casting ? State_Moving : State_Casting;
			}

			if (AI_State == State_Casting)
			{
				npc.velocity.Y = 0;
				npc.velocity.X = 0;

				if ((int)AI_Count % 12 == 0)
				{
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 200, 0f, 0f, mod.ProjectileType("DownpourElementalCloud"), 0, 0f, Main.myPlayer);
					if ((int)AI_Count != 0 && npc.target == Main.myPlayer)
					{
						Player player = Main.player[npc.target];
						Vector2 delta;
						delta.X = player.Center.X - npc.Center.X;
						delta.Y = player.Center.Y - npc.Center.Y + 200;
						float magnitude = (float)Math.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
						if (magnitude > 0)
						{
							delta *= 5f / magnitude;
						}
						else
						{
							delta = new Vector2(0f, 5f);
						}
						int damage = 50;
						if (Main.expertMode)
						{
							damage = (int)(damage / Main.expertDamage);
						}
						delta *= 2f;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 200, delta.X, delta.Y, mod.ProjectileType("DownpourElementalCloudProj"), damage, 3f, Main.myPlayer);
						Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y - 200, 93);
					}
					npc.netUpdate = true;
				}
			}

			Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, 16, 0f, 0f, 100);
			dust.noGravity = true;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			npc.frame.Y = (int)AI_Anim * frameHeight;
		}
	}
}
