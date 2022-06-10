using Terraria.ModLoader;

namespace OrchidMod.Common.Commands
{
	public class ShamanUICommand : ModCommand
	{
		public override CommandType Type
			=> CommandType.Chat;
		public override string Command
			=> "UI";
		public override string Description
			=> "Reloads the Shaman UI";

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			OrchidMod.reloadShamanUI = true;
		}
	}
}