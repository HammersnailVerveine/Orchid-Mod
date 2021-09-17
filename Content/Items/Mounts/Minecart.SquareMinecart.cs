using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Items.Mounts
{
	public class SquareMinecart : OrchidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Square Minecart");
			Tooltip.SetDefault("'Great for impersonating Orchid Devs!'"); // S-Pladison
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 22;
			item.rare = ItemRarityID.Cyan;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.mountType = ModContent.MountType<SquareMinecartMount>();
		}
	}

	public class SquareMinecartBuff : OrchidBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Minecart"); // Square Minecart (all vanilla minecarts have this name...)
			Description.SetDefault("Riding in a minecart");

			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<SquareMinecartMount>(), player);
			player.buffTime[buffIndex] = 10;
		}
	}

	public class SquareMinecartMount : ModMountData
	{
		public override bool Autoload(ref string name, ref string texture, IDictionary<MountTextureType, string> extraTextures)
		{
			texture = "OrchidMod/Assets/Textures/Mounts/SquareMinecartMount_Front";
			extraTextures[MountTextureType.Front] = "OrchidMod/Assets/Textures/Mounts/SquareMinecartMount_Front";
			return true;
		}

		public override void SetDefaults()
		{
			MountID.Sets.Cart[Type] = true;
			mountData.Minecart = true;
			mountData.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.Sparks);

			mountData.spawnDust = 16;
			mountData.buff = ModContent.BuffType<SquareMinecartBuff>();

			mountData.flightTimeMax = 0;
			mountData.fallDamage = 1f;
			mountData.runSpeed = 13f;
			mountData.dashSpeed = 13f;
			mountData.acceleration = 0.04f;
			mountData.jumpHeight = 15;
			mountData.jumpSpeed = 5.15f;
			mountData.blockExtraJumps = true;
			mountData.heightBoost = 10;

			mountData.playerYOffsets = new int[] { 8, 8, 8 };
			//mountData.xOffset = 1;
			mountData.yOffset = 13;
			mountData.bodyFrame = 3;
			mountData.playerHeadOffset = 14;

			mountData.totalFrames = 3;
			mountData.standingFrameCount = 1;
			mountData.standingFrameDelay = 12;
			mountData.standingFrameStart = 0;
			mountData.runningFrameCount = 3;
			mountData.runningFrameDelay = 12;
			mountData.runningFrameStart = 0;
			mountData.flyingFrameCount = 0;
			mountData.flyingFrameDelay = 0;
			mountData.flyingFrameStart = 0;
			mountData.inAirFrameCount = 0;
			mountData.inAirFrameDelay = 0;
			mountData.inAirFrameStart = 0;
			mountData.idleFrameCount = 0;
			mountData.idleFrameDelay = 0;
			mountData.idleFrameStart = 0;
			mountData.idleFrameLoop = false;

			if (Main.netMode != NetmodeID.Server)
			{
				mountData.textureWidth = mountData.frontTexture.Width;
				mountData.textureHeight = mountData.frontTexture.Height;
			}
		}
	}
}
