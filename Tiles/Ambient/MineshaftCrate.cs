using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace OrchidMod.Tiles.Ambient
{
	public class MineshaftCrate : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Mineshaft Crate");
			AddMapEntry(new Color(100, 75, 50), name);
			dustType = 7;
			disableSmartCursor = true;
			adjTiles = new int[]{ TileID.WorkBenches };
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int rand = Main.rand.Next(100);
			if (rand >= 0 && rand < 10) for (int k = 0 ; k < Main.rand.Next(2)+1 ; k++ ) Item.NewItem(i * 16, j * 16, 32, 16, 22); // iron bar
			if (rand >= 10 && rand < 20) for (int k = 0 ; k < Main.rand.Next(2)+1 ; k++ ) Item.NewItem(i * 16, j * 16, 32, 16, 20); // copper bar
			if (rand >= 20 && rand < 30) for (int k = 0 ; k < Main.rand.Next(2)+1 ; k++ ) Item.NewItem(i * 16, j * 16, 32, 16, 19); // gold bar
			if (rand >= 30 && rand < 40) for (int k = 0 ; k < Main.rand.Next(2)+1 ; k++ ) Item.NewItem(i * 16, j * 16, 32, 16, 21); // silver bar
			if (rand >= 40 && rand < 50) for (int k = 0 ; k < Main.rand.Next(2)+1 ; k++ ) Item.NewItem(i * 16, j * 16, 32, 16, 703); // tin bar
			if (rand >= 50 && rand < 60) for (int k = 0 ; k < Main.rand.Next(2)+1 ; k++ ) Item.NewItem(i * 16, j * 16, 32, 16, 704); // lead bar
			if (rand >= 60 && rand < 70) for (int k = 0 ; k < Main.rand.Next(2)+1 ; k++ ) Item.NewItem(i * 16, j * 16, 32, 16, 705); // tungsten bar
			if (rand >= 70 && rand < 80) for (int k = 0 ; k < Main.rand.Next(2)+1 ; k++ ) Item.NewItem(i * 16, j * 16, 32, 16, 706); // platinum bar
			if (rand >= 80 && rand <= 90) for (int k = 0 ; k < Main.rand.Next(5)+5 ; k++ ) Item.NewItem(i * 16, j * 16, 32, 16, 166); // bomb
			if (rand == 91) Item.NewItem(i * 16, j * 16, 32, 16, 49); // Band of regeneration
			if (rand == 92) Item.NewItem(i * 16, j * 16, 32, 16, 50); // Magic Mirror
			if (rand == 93) Item.NewItem(i * 16, j * 16, 32, 16, 53); // Cloud in a bottle
			if (rand == 94) Item.NewItem(i * 16, j * 16, 32, 16, 54); // Hermes Boots
			if (rand == 95) Item.NewItem(i * 16, j * 16, 32, 16, 55); // Enchanted Boomerang
			if (rand == 96) Item.NewItem(i * 16, j * 16, 32, 16, 975); // Shoes spikes
			if (rand == 97) {
				Item.NewItem(i * 16, j * 16, 32, 16, 930); // Flare Gun
				for (int k = 0 ; k < Main.rand.Next(20)+30 ; k++ ) Item.NewItem(i * 16, j * 16, 32, 16, 931); //Flare
			}
			if (rand == 98) Item.NewItem(i * 16, j * 16, 32, 16, 997); // Extractinator
			if (rand == 99) for (int k = 0 ; k < 5 ; k++ ) Item.NewItem(i * 16, j * 16, 32, 16, 73); // gold coin
		}
	}
}