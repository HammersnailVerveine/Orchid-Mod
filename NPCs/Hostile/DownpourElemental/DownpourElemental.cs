using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.NPCs.Hostile.DownpourElemental
{
	public class DownpourElemental : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storm Spirit");
			Main.npcFrameCount[NPC.type] = 12;
		}

		public override void SetDefaults()
		{
			NPC.width = 178;
			NPC.height = 110;
			NPC.aiStyle = 14;
			NPC.damage = 40;
			NPC.defense = 30;
			NPC.lifeMax = 5000;
			NPC.HitSound = SoundID.NPCHit30;
			NPC.DeathSound = SoundID.NPCDeath33;
			NPC.value = 15000f;
			NPC.knockBackResist = 0.05f;
			NPC.friendly = false;
			NPC.alpha = 16;
			NPC.noTileCollide = true;
		}

		public override void OnKill()
		{
			Item.NewItem(NPC.getRect(), Mod.Find<ModItem>("DownpourCrystal").Type);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 15; i++)
			{
				Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 16, 2 * hitDirection, -2f, 100);
				dust.noGravity = true;
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return Main.raining && spawnInfo.Player.ZoneSkyHeight && Main.hardMode ? .03f : 0f;
		}

		private const int AI_State_Slot = 0;
		private const int AI_Timer_Slot = 1;
		private const int AI_Anim_Slot = 2;
		private const int AI_State_Count = 3;

		private const int State_Moving = 0;
		private const int State_Casting = 1;

		public float AI_State
		{
			get => NPC.ai[AI_State_Slot];
			set => NPC.ai[AI_State_Slot] = value;
		}

		public float AI_Timer
		{
			get => NPC.ai[AI_Timer_Slot];
			set => NPC.ai[AI_Timer_Slot] = value;
		}

		public float AI_Anim
		{
			get => NPC.ai[AI_Anim_Slot];
			set => NPC.ai[AI_Anim_Slot] = value;
		}

		public float AI_Count
		{
			get => NPC.ai[AI_State_Count];
			set => NPC.ai[AI_State_Count] = value;
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
				NPC.velocity.Y = 0;
				NPC.velocity.X = 0;

				if ((int)AI_Count % 12 == 0)
				{
					Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y - 200, 0f, 0f, Mod.Find<ModProjectile>("DownpourElementalCloud").Type, 0, 0f, Main.myPlayer);
					if ((int)AI_Count != 0 && NPC.target == Main.myPlayer)
					{
						Player player = Main.player[NPC.target];
						Vector2 delta;
						delta.X = player.Center.X - NPC.Center.X;
						delta.Y = player.Center.Y - NPC.Center.Y + 200;
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
						Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y - 200, delta.X, delta.Y, Mod.Find<ModProjectile>("DownpourElementalCloudProj").Type, damage, 3f, Main.myPlayer);
						SoundEngine.PlaySound(2, (int)NPC.Center.X, (int)NPC.Center.Y - 200, 93);
					}
					NPC.netUpdate = true;
				}
			}

			Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, 16, 0f, 0f, 100);
			dust.noGravity = true;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.spriteDirection = NPC.direction;
			NPC.frame.Y = (int)AI_Anim * frameHeight;
		}
	}
}
