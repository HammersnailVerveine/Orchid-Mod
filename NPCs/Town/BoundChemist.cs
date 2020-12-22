using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using OrchidMod.WorldgenArrays;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.NPCs.Town
{
	public class BoundChemist : ModNPC
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Bound Chemist");
		}

		public override void SetDefaults() {
			npc.lifeMax = 250;
			npc.damage = 10;
			npc.defense = 15;
			npc.knockBackResist = 0.5f;
			npc.width = 28;
			npc.height = 36;
			npc.aiStyle = 0;
			npc.noGravity = false;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.friendly = true;
			npc.value = Item.buyPrice(0, 0, 0, 0);
		}

		public override void FindFrame(int frameHeight) {
			npc.frame.Y = 0;
			npc.rotation = 0f;
		}
		
		public override bool CanChat() {
			return true;
		}

		public override string GetChat() {
			return "Thanks, you spared me a lot of troubles right there, or... Let's not talk about it.";
		}
		
		public override void AI()
		{
			if (Main.netMode == NetmodeID.SinglePlayer) {
				Player player = Main.player[Main.myPlayer];
				if (player.talkNPC > -1) {
					if (Main.npc[player.talkNPC].type == npc.type) {
						npc.Transform(NPCType<NPCs.Town.Chemist>());
						OrchidWorld.foundChemist = true;
					}
				}
			} else {
				for (int index = 0; index < Main.player.Length; index++) {
					if (Main.player[index].talkNPC > -1) {
						if (Main.player[index].active && Main.npc[Main.player[index].talkNPC].type == npc.type) {
							npc.Transform(NPCType<NPCs.Town.Chemist>());
							OrchidWorld.foundChemist = true;
						}
					}
				}
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {
			bool foundNPC = (NPC.FindFirstNPC(NPCType<NPCs.Town.Chemist>()) + NPC.FindFirstNPC(NPCType<NPCs.Town.BoundChemist>())) > 0;
			bool inMineshaft = false;
			if (!foundNPC && !OrchidWorld.foundChemist) {
				Player player = Main.player[(int)Player.FindClosest(new Vector2(Main.maxTilesX / 2 * 16f, (Main.maxTilesY / 3 + 100) * 16f), 1, 1)];
				int MSMinPosX = (Main.maxTilesX / 2) - ((OrchidMSarrays.MSLenght * 15) / 2) + 10;
				int MSMinPosY = (Main.maxTilesY/3 + 100) + 10;
				Rectangle rect = new Rectangle(MSMinPosX, MSMinPosY, (OrchidMSarrays.MSLenght * 15) - 20, (OrchidMSarrays.MSHeight * 14) - 20);
				if (rect.Contains(new Point((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f)))) {
					inMineshaft = true;
				}
			}
			return !OrchidWorld.foundChemist && !foundNPC && inMineshaft ? 5f : 0f;
		}
	}
}
