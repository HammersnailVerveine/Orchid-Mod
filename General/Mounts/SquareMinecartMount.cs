using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.General.Mounts
{
	public class SquareMinecartMount : ModMountData
	{
		public override void SetDefaults()
		{
			MountID.Sets.Cart[Type] = true;
			mountData.Minecart = true;
			mountData.MinecartDust = new Action<Vector2>(DelegateMethods.Minecart.Sparks);

			mountData.spawnDust = 16;
			mountData.buff = ModContent.BuffType<Buffs.SquareMinecartBuff>();

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
