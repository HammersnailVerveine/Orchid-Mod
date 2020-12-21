using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod
{
    public class ModGlobalTile : GlobalTile
    {
		public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem) {
			if (type == 28) {
				if (Main.rand.Next(1500) == 0) {
						Item.NewItem(i * 16, j * 16, 32, 48, ItemType<Gambler.Weapons.Cards.HealingPotionCard>());
				}
			}
			if (type == 31) {
				if (Main.tile[i, j].frameX == 1 * 36 && Main.tile[i, j].frameY == 0) {
					if (Main.rand.Next(6) == 0) {
						Item.NewItem(i * 16, j * 16, 32, 48, ItemType<Shaman.Weapons.BloodCaller>());
					}
					if (Main.rand.Next(5) == 0) {
						Item.NewItem(i * 16, j * 16, 32, 48, ItemType<Alchemist.Weapons.Catalysts.CrimtaneCatalyst>());
					}
				}
				if (Main.tile[i, j].frameX == 0  && Main.tile[i, j].frameY == 0) {
					if (Main.rand.Next(6) == 0) {
						Item.NewItem(i * 16, j * 16, 32, 48, ItemType<Shaman.Weapons.ShadowWeaver>());
					}
					if (Main.rand.Next(5) == 0) {
						Item.NewItem(i * 16, j * 16, 32, 48, ItemType<Alchemist.Weapons.Catalysts.DemoniteCatalyst>());
					}
				}
			}
		}

    }
}  