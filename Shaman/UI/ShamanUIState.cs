using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace OrchidMod.Shaman.UI
{
	public class ShamanUIState : UIState
	{
		public ShamanUIFrame shamanRessourceFrame = new ShamanUIFrame();

		public override void OnInitialize()
		{
			shamanRessourceFrame.Width.Set(94f, 0f);
			shamanRessourceFrame.Height.Set(180f, 0f);
			shamanRessourceFrame.Left.Set(Main.screenWidth - 100f, 0f);
			shamanRessourceFrame.Top.Set(Main.screenHeight - 130f, 0f);
			shamanRessourceFrame.backgroundColor = Color.White;

			base.Append(shamanRessourceFrame);
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