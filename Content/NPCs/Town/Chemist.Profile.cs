using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace OrchidMod.Content.NPCs.Town
{
	public class ChemistProfile : ITownNPCProfile
	{
		public int RollVariation()
			=> 0;

		public string GetNameForVariant(NPC npc)
			=> npc.getNewNPCName();

		public int GetHeadTextureIndex(NPC npc)
			=> ModContent.GetModHeadSlot(OrchidAssets.NPCsPath + "Chemist_Head");

		Asset<Texture2D> ITownNPCProfile.GetTextureNPCShouldUse(NPC npc)
		{
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
			{
				return ModContent.Request<Texture2D>(OrchidAssets.NPCsPath + "Chemist");
			}

			if (npc.altTexture == 1)
			{
				return ModContent.Request<Texture2D>(OrchidAssets.NPCsPath + "Chemist_Party");
			}

			return ModContent.Request<Texture2D>(OrchidAssets.NPCsPath + "Chemist");
		}
	}
}