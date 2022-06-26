using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace OrchidMod.Content.NPCs.Town
{
	public class CroupierProfile : ITownNPCProfile
	{
		public int RollVariation()
			=> 0;

		public string GetNameForVariant(NPC npc)
			=> npc.getNewNPCName();

		public int GetHeadTextureIndex(NPC npc)
			=> ModContent.GetModHeadSlot(OrchidAssets.NPCsPath + "Croupier_Head");

		Asset<Texture2D> ITownNPCProfile.GetTextureNPCShouldUse(NPC npc)
			=> ModContent.Request<Texture2D>(OrchidAssets.NPCsPath + "Croupier");
	}
}