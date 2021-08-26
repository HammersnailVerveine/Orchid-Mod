using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace OrchidMod.Alchemist.UI
{
	public class AlchemistUIState : UIState
	{
		public AlchemistUIFrame alchemistRessourceFrame = new AlchemistUIFrame();

		public override void OnInitialize()
		{
			alchemistRessourceFrame.Width.Set(94f, 0f);
			alchemistRessourceFrame.Height.Set(180f, 0f);
			alchemistRessourceFrame.Left.Set(Main.screenWidth / 2 - 60f, 0f);
			alchemistRessourceFrame.Top.Set(Main.screenHeight / 2 - 40f, 0f);
			alchemistRessourceFrame.backgroundColor = Color.White;

			base.Append(alchemistRessourceFrame);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Recalculate();
			base.Draw(spriteBatch);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}