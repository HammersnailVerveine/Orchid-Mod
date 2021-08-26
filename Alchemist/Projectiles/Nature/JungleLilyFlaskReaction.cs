using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.Projectiles.Nature
{
	public class JungleLilyFlaskReaction : OrchidModAlchemistProjectile
	{
		public static List<int> clearedBuffs = setClearedBuffs();
		public static List<int> setClearedBuffs()
		{
			List<int> buffs = new List<int>();
			buffs.Add(30); // Bleeding
			buffs.Add(20); // Poisoned
			buffs.Add(24); // On Fire
			buffs.Add(31); // Confused
			buffs.Add(23); // Cursed
			return buffs;
		}

		public override void SafeSetDefaults()
		{
			projectile.width = 100;
			projectile.height = 100;
			projectile.friendly = false;
			projectile.aiStyle = 0;
			projectile.timeLeft = 1;
			projectile.tileCollide = false;
			projectile.scale = 1f;
			projectile.alpha = 255;
			projectile.penetrate = -1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Lily Purification Aura");
		}

		public override void AI()
		{
			if (projectile.owner == Main.myPlayer)
			{
				int size = 4;
				int i = (int)(projectile.Center.X / 16);
				int j = (int)(projectile.Center.Y / 16);

				for (int k = i - size; k <= i + size; k++)
				{
					for (int l = j - size; l <= j + size; l++)
					{
						if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
						{
							Tile baseTile = Framing.GetTileSafely(k, l);
							if (baseTile.type == TileType<Tiles.Ambient.JungleLilyTile>() && baseTile.frameX == 0 && baseTile.frameY == 0)
							{
								for (int w = 0; w < 2; w++)
								{
									for (int q = 0; q < 2; q++)
									{
										Tile tile = Framing.GetTileSafely(k + w, l + q);
										tile.active(false);
										NetMessage.SendTileSquare(-1, k, l, 1);
									}
								}
								//WorldGen.SquareTileFrame(k, l, true);
								Item.NewItem((k + 1) * 16, (l + 1) * 16, 0, 0, ItemType<Alchemist.Misc.JungleLilyItemBloomed>());
								OrchidModProjectile.spawnDustCircle(new Vector2((k + 1) * 16, (l + 1) * 16), DustType<Dusts.BloomingDust>(), 5, 8, true, 1.5f, 1f, 3f, true, true, false, 0, 0, true);
							}
						}
					}
				}

				Player player = Main.player[Main.myPlayer]; // < TEST MP
				foreach (int buffID in clearedBuffs)
				{
					player.ClearBuff(buffID);
				}
			}
		}
	}
}