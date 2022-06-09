using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace OrchidMod.Guardian.UI
{
	public class GuardianUIState : UIState
	{
		public GuardianUIFrame guardianRessourceFrame = new GuardianUIFrame();

		public override void OnInitialize()
		{
			guardianRessourceFrame.Width.Set(10f, 0f);
			guardianRessourceFrame.Height.Set(10f, 0f);
			guardianRessourceFrame.Left.Set(Main.screenWidth / 2, 0f);
			guardianRessourceFrame.Top.Set(Main.screenHeight / 2, 0f);
			guardianRessourceFrame.backgroundColor = Color.White;

			base.Append(guardianRessourceFrame);
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