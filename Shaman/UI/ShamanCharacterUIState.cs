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

namespace OrchidMod.Shaman.UI
{
	public class ShamanCharacterUIState: UIState
    {
		public ShamanCharacterUIFrame shamanCharacterRessourceFrame = new ShamanCharacterUIFrame();
		
		public override void OnInitialize()
        {
			shamanCharacterRessourceFrame.Width.Set(0f, 0f);
			shamanCharacterRessourceFrame.Height.Set(0f, 0f);
			shamanCharacterRessourceFrame.Left.Set(Main.screenWidth / 2, 0f);
			shamanCharacterRessourceFrame.Top.Set(Main.screenHeight / 2, 0f);
			shamanCharacterRessourceFrame.backgroundColor = Color.White;
			
			base.Append(shamanCharacterRessourceFrame);
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