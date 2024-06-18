using Microsoft.Xna.Framework;
using OrchidMod.Common.ModObjects;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Alchemist.Projectiles.Nature
{
	public class JungleLilyFlaskReaction : OrchidModAlchemistProjectile
	{
		private static readonly List<int> ClearedBuffs = new() { BuffID.Bleeding, BuffID.Poisoned, BuffID.OnFire, BuffID.Confused, BuffID.Cursed };

		// ...

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
			// DisplayName.SetDefault("Jungle Lily Purification Aura");
		}

		public override void AI()
		{
			int i = (int)(Projectile.Center.X / 16);
			int j = (int)(Projectile.Center.Y / 16);

			KillFlowers(i, j);

			Player player = Main.LocalPlayer;
			if (player.Center.Distance(Projectile.Center) < Projectile.width / 2)
			{
				foreach (int buffID in ClearedBuffs)
				{
					player.ClearBuff(buffID);
				}
			}
		}

		// ...

		private static void KillFlowers(int i, int j)
		{
			const int size = 4;

			for (int k = i - size; k <= i + size; k++)
			{
				for (int l = j - size; l <= j + size; l++)
				{
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
					{
						Tile tile = Framing.GetTileSafely(k, l);

						if (tile.TileType == ModContent.TileType<Content.Items.Materials.JungleLilyTile>() && tile.TileFrameX % 36 == 0 && tile.TileFrameY == 0 && tile.HasTile)
						{
							OrchidModProjectile.spawnDustCircle(new Vector2((k + 1) * 16, (l + 1) * 16), ModContent.DustType<Content.Dusts.BloomingDust>(), 5, 8, true, 1.5f, 1f, 3f, true, true, false, 0, 0, true);

							WorldGen.KillTile(k, l, false, false, true);

							if (Main.netMode == NetmodeID.MultiplayerClient)
							{
								NetMessage.SendData(MessageID.TileSquare, -1, -1, null, 0, k, l);
							}

							Item.NewItem(new EntitySource_TileBreak(k, l), (k + 1) * 16, (l + 1) * 16, 0, 0, ModContent.ItemType<Content.Items.Materials.JungleLilyBloomed>());
						}
					}
				}
			}
		}
	}
}