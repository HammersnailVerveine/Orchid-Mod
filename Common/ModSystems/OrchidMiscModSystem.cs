using Terraria.ModLoader;
namespace OrchidMod.Common.ModSystems
{
	public class OrchidMiscModSystem : ModSystem
	{
		public static int SlamDropCooldown = 0;

		public override void PreUpdateWorld()
		{
			SlamDropCooldown++;
		}
	}
}