using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ID;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Terraria.DataStructures;
using OrchidMod.Alchemist.UI;
using OrchidMod;

namespace OrchidMod.Gambler.UI
{
	public class GamblerUIState: UIState
    {
		public GamblerUIFrame gamblerRessourceFrame = new GamblerUIFrame();
		
		public override void OnInitialize()
        {
			gamblerRessourceFrame.Width.Set(94f, 0f);
			gamblerRessourceFrame.Height.Set(180f, 0f);
			gamblerRessourceFrame.Left.Set(Main.screenWidth / 2 + 30f, 0f);
			gamblerRessourceFrame.Top.Set(Main.screenHeight / 2 - 50f, 0f);
			gamblerRessourceFrame.backgroundColor = Color.White;
			
			base.Append(gamblerRessourceFrame);
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