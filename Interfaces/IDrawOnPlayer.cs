using Terraria.ModLoader;

namespace OrchidMod.Interfaces
{
	public interface IDrawOnPlayer
	{
		void DrawOnPlayer(PlayerDrawInfo drawInfo);
	}

	public interface IDrawOnPlayerWithArms : IDrawOnPlayer
	{
		void DrawOnArms(PlayerDrawInfo drawInfo);
	}
}
