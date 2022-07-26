using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
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
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.friendly = false;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Lily Purification Aura");
		}

		public override void AI()
		{
			int size = 4;
			int i = (int)(Projectile.Center.X / 16);
			int j = (int)(Projectile.Center.Y / 16);
			for (int k = i - size; k <= i + size; k++)
			{
				for (int l = j - size; l <= j + size; l++)
				{
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
					{
						Tile baseTile = Framing.GetTileSafely(k, l);
						if (baseTile.TileType == ModContent.TileType<Content.Items.Materials.JungleLilyTile>() && baseTile.TileFrameX % 36 == 0 && baseTile.TileFrameY == 0 && baseTile.HasTile)
						{
							OrchidModProjectile.spawnDustCircle(new Vector2((k + 1) * 16, (l + 1) * 16), ModContent.DustType<Content.Dusts.BloomingDust>(), 5, 8, true, 1.5f, 1f, 3f, true, true, false, 0, 0, true);

							for (int w = 0; w < 2; w++)
							{
								for (int q = 0; q < 2; q++)
								{
									Tile tile = Framing.GetTileSafely(k + w, l + q);
									tile.HasTile = false;
									//NetMessage.SendTileSquare(-1, k, l, 1);
								}
							}

							if (Projectile.owner == Main.myPlayer)
								Item.NewItem(new EntitySource_TileBreak(k, l), (k + 1) * 16, (l + 1) * 16, 0, 0, ModContent.ItemType<Content.Items.Materials.JungleLilyBloomed>());
						}
					}
				}
			}

			Player player = Main.LocalPlayer;
			if (player.Center.Distance(Projectile.Center) < Projectile.width / 2)
			{
				foreach (int buffID in clearedBuffs)
				{
					player.ClearBuff(buffID);
				}
			}
		}
	}
}