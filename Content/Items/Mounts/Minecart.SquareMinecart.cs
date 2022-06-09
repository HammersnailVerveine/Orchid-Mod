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
			Item.width = 34;
			Item.height = 22;
			Item.rare = ItemRarityID.Cyan;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.mountType = ModContent.MountType<SquareMinecartMount>();
		}
	}

	public class SquareMinecartBuff : OrchidBuff
	{
		public override void SetStaticDefaults()
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

	public class SquareMinecartMount : ModMount
	{
		public override bool Autoload(ref string name, ref string texture, IDictionary<MountTextureType, string> extraTextures)
		{
			texture = "OrchidMod/Assets/Textures/Mounts/SquareMinecartMount_Front";
			extraTextures[MountTextureType.Front] = "OrchidMod/Assets/Textures/Mounts/SquareMinecartMount_Front";
			return true;
		}

		public override void SetStaticDefaults()
		{
			MountID.Sets.Cart[Type] = true;
			MountData.Minecart = true;
			MountData.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.Sparks);

			MountData.spawnDust = 16;
			MountData.buff = ModContent.BuffType<SquareMinecartBuff>();

			MountData.flightTimeMax = 0;
			MountData.fallDamage = 1f;
			MountData.runSpeed = 13f;
			MountData.dashSpeed = 13f;
			MountData.acceleration = 0.04f;
			MountData.jumpHeight = 15;
			MountData.jumpSpeed = 5.15f;
			MountData.blockExtraJumps = true;
			MountData.heightBoost = 10;

			MountData.playerYOffsets = new int[] { 8, 8, 8 };
			//mountData.xOffset = 1;
			MountData.yOffset = 13;
			MountData.bodyFrame = 3;
			MountData.playerHeadOffset = 14;

			MountData.totalFrames = 3;
			MountData.standingFrameCount = 1;
			MountData.standingFrameDelay = 12;
			MountData.standingFrameStart = 0;
			MountData.runningFrameCount = 3;
			MountData.runningFrameDelay = 12;
			MountData.runningFrameStart = 0;
			MountData.flyingFrameCount = 0;
			MountData.flyingFrameDelay = 0;
			MountData.flyingFrameStart = 0;
			MountData.inAirFrameCount = 0;
			MountData.inAirFrameDelay = 0;
			MountData.inAirFrameStart = 0;
			MountData.idleFrameCount = 0;
			MountData.idleFrameDelay = 0;
			MountData.idleFrameStart = 0;
			MountData.idleFrameLoop = false;

			if (Main.netMode != NetmodeID.Server)
			{
				MountData.textureWidth = MountData.frontTexture.Width;
				MountData.textureHeight = MountData.frontTexture.Height;
			}
		}
	}
}
