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

namespace OrchidMod.Alchemist.UI
{
	public class AlchemistBookUIState: UIState
    {
		public AlchemistBookUIFrame alchemistRessourceFrame = new AlchemistBookUIFrame();
		
		public override void OnInitialize()
        {
			alchemistRessourceFrame.Width.Set(94f, 0f);
			alchemistRessourceFrame.Height.Set(180f, 0f);
			alchemistRessourceFrame.Left.Set(Main.screenWidth / 2, 0f);
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