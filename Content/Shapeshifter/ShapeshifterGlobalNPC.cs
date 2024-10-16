using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content.Shapeshifter
{
	internal class ShapeshifterGlobalNPC : GlobalNPC
	{
		public bool SageOwlDebuff;
		
		public override bool InstancePerEntity => true;
		public override void ResetEffects(NPC npc) {
			SageOwlDebuff = false;
		}

		public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers) {
			if (SageOwlDebuff) {
				modifiers.FlatBonusDamage += 3;
			}
		}

		public override void DrawEffects(NPC npc, ref Color drawColor) {
			if (SageOwlDebuff) {
				drawColor.R = 0;
			}
		}
	}
}
