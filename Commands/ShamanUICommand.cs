using OrchidMod.Shaman.UI;
using OrchidMod;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;

namespace OrchidMod.Commands
{
	public class ShamanUICommand : ModCommand
	{
		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Command
		{
			get { return "UI"; }
		}
		
		public override string Description 
		{
			get { return "Reloads the Shaman UI"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{	
			OrchidMod.reloadShamanUI = true;
		}
	}
}