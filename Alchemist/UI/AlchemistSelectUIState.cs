using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.UI;

namespace OrchidMod.Alchemist.UI
{
	public class AlchemistSelectUIState : UIState
	{
		public AlchemistSelectUIFrame alchemistSelectUIFrame = new AlchemistSelectUIFrame();
		public Vector2 mouseClicPosition = Main.MouseScreen;
		public Vector2 mouseClicPositionDiff = Main.MouseScreen;

		public override void OnInitialize()
		{
			alchemistSelectUIFrame.Width.Set(0f, 0f);
			alchemistSelectUIFrame.Height.Set(0f, 0f);
			alchemistSelectUIFrame.Left.Set(Main.screenWidth - 64f, 0f);
			alchemistSelectUIFrame.Top.Set(Main.screenHeight - 64f, 0f);
			alchemistSelectUIFrame.backgroundColor = Color.White;

			base.Append(alchemistSelectUIFrame);
		}

		public void updateMouseCoordinates()
		{
			this.mouseClicPosition = Main.MouseScreen;
			this.mouseClicPositionDiff = new Vector2(Main.screenWidth - this.mouseClicPosition.X, Main.screenHeight - this.mouseClicPosition.Y);
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