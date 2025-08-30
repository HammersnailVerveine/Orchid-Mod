using Terraria;

namespace OrchidMod.Content.Guardian
{
	public class BlockedEnemy
	{
		public NPC npc;
		public int time;
		
		public BlockedEnemy(NPC npc_, int time_)
		{
			this.npc = npc_;
			this.time = time_;
		}
	}
}