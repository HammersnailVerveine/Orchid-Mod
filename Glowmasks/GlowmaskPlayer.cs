using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Glowmasks
{
	public class GlowmaskPlayer : ModPlayer
	{
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			/*
			 * player.armor[0-2] is armor slots
			 * player.armor[3-9] is accesories
			 * player.armor[10-12] is vanity armor
			 * player.armor[13-19] is vanity accesories
			*/

			void ItemGlowmaskLayer(PlayerDrawInfo drawInfo)
			{
				if (drawInfo.drawPlayer.HeldItem.modItem is Interfaces.IGlowingItem glowItem) glowItem.DrawItemGlowmask(drawInfo);
			}
			var index = layers.IndexOf(PlayerLayer.HeldItem);
			if (index >= 0) layers.Insert(index + 1, new PlayerLayer("OrchidMod", "HeldItemGlowmask", ItemGlowmaskLayer));

			index = layers.IndexOf(PlayerLayer.Head);
			if (index >= 0) layers.Insert(index + 1, HeadGlowmaskLayer);

			index = layers.IndexOf(PlayerLayer.Body);
			if (index >= 0) layers.Insert(index + 1, BodyGlowmaskLayer);

			index = layers.IndexOf(PlayerLayer.Arms);
			if (index >= 0) layers.Insert(index + 1, ArmsGlowmaskLayer);

			index = layers.IndexOf(PlayerLayer.Legs);
			if (index >= 0) layers.Insert(index + 1, LegsGlowmaskLayer);

			index = layers.IndexOf(PlayerLayer.Wings);
			if (index >= 0) layers.Insert(index + 1, WingsGlowmaskLayer);
		}

		private static readonly PlayerLayer HeadGlowmaskLayer = new PlayerLayer("OrchidMod", "HeadGlowmask", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo)
		{
			var player = drawInfo.drawPlayer;

			if (player.head > 0 && drawInfo.shadow == 0 && !player.invis)
			{
				Texture2D texture = null;
				Color color = new Color(250, 250, 250);

				if (player.head == OrchidMod.Instance.GetEquipSlot("MushroomBandana", EquipType.Head))
				{
					texture = ModContent.GetTexture("OrchidMod/Glowmasks/MushroomBandana_Head_Glowmask");
					color = new Color(250, 250, 250, 200) * OrchidWorld.alchemistMushroomArmorProgress;
				}
				else if (player.head == OrchidMod.Instance.GetEquipSlot("AbyssalHelm", EquipType.Head))
				{
					texture = ModContent.GetTexture("OrchidMod/Glowmasks/AbyssalHelm_Head_Glowmask");
				}

				if (texture == null) return;

				Vector2 position = new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.headPosition + drawInfo.headOrigin;
				var drawData = new DrawData(texture, position, new Rectangle?(player.bodyFrame), color * player.stealth, player.headRotation, drawInfo.headOrigin, 1f, drawInfo.spriteEffects, 0)
				{
					shader = drawInfo.headArmorShader
				};
				Main.playerDrawData.Add(drawData);
			}
		});

		private static readonly PlayerLayer BodyGlowmaskLayer = new PlayerLayer("OrchidMod", "BodyGlowmask", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo)
		{
			var player = drawInfo.drawPlayer;

			if (player.body > 0 && drawInfo.shadow == 0 && !player.invis)
			{
				Texture2D texture = null;
				Color color = new Color(250, 250, 250);

				if (player.body == OrchidMod.Instance.GetEquipSlot("MushroomTunic", EquipType.Body))
				{
					if (drawInfo.drawPlayer.Male) texture = ModContent.GetTexture("OrchidMod/Glowmasks/MushroomTunic_Body_Glowmask");
					else texture = ModContent.GetTexture("OrchidMod/Glowmasks/MushroomTunic_FemaleBody_Glowmask");
					color = new Color(250, 250, 250, 200) * OrchidWorld.alchemistMushroomArmorProgress;
				}
				else if (player.body == OrchidMod.Instance.GetEquipSlot("AbyssalChestplate", EquipType.Body))
				{
					if (drawInfo.drawPlayer.Male) texture = ModContent.GetTexture("OrchidMod/Glowmasks/AbyssalChestplate_Body_Glowmask");
					else texture = ModContent.GetTexture("OrchidMod/Glowmasks/AbyssalChestplate_FemaleBody_Glowmask");
				}

				if (texture == null) return;

				Vector2 position = new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (player.bodyFrame.Width / 2) + (player.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + player.height - player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2(player.bodyFrame.Width / 2, player.bodyFrame.Height / 2);
				var drawData = new DrawData(texture, position, new Rectangle?(player.bodyFrame), color * player.stealth, player.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0)
				{
					shader = drawInfo.bodyArmorShader
				};
				Main.playerDrawData.Add(drawData);
			}
		});

		private static readonly PlayerLayer ArmsGlowmaskLayer = new PlayerLayer("OrchidMod", "ArmsGlowmask", PlayerLayer.Arms, delegate (PlayerDrawInfo drawInfo)
		{
			var player = drawInfo.drawPlayer;

			if (player.body > 0 && drawInfo.shadow == 0 && !player.invis)
			{
				Texture2D texture = null;
				Color color = new Color(250, 250, 250);

				if (player.body == OrchidMod.Instance.GetEquipSlot("MushroomTunic", EquipType.Body))
				{
					texture = ModContent.GetTexture("OrchidMod/Glowmasks/MushroomTunic_Arms_Glowmask");
					color = new Color(250, 250, 250, 200) * OrchidWorld.alchemistMushroomArmorProgress;
				}
				else if (player.body == OrchidMod.Instance.GetEquipSlot("AbyssalChestplate", EquipType.Body))
				{
					texture = ModContent.GetTexture("OrchidMod/Glowmasks/AbyssalChestplate_Arms_Glowmask");
				}

				if (texture == null) return;

				Vector2 position = new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(player.bodyFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.bodyFrame.Height + 4f))) + player.bodyPosition + new Vector2((float)(player.bodyFrame.Width / 2), (float)(player.bodyFrame.Height / 2));
				var drawData = new DrawData(texture, position, new Rectangle?(player.bodyFrame), color * player.stealth, player.bodyRotation, drawInfo.bodyOrigin, 1f, drawInfo.spriteEffects, 0)
				{
					shader = drawInfo.bodyArmorShader
				};
				Main.playerDrawData.Add(drawData);
			}
		});

		private static readonly PlayerLayer LegsGlowmaskLayer = new PlayerLayer("OrchidMod", "LegsGlowmask", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo)
		{
			var player = drawInfo.drawPlayer;

			if (player.legs > 0 && drawInfo.shadow == 0 && (player.shoe != 15 && !player.wearsRobe) && !player.invis)
			{
				Texture2D texture = null;
				Color color = new Color(250, 250, 250);

				if (player.legs == OrchidMod.Instance.GetEquipSlot("MushroomLeggings", EquipType.Legs))
				{
					texture = ModContent.GetTexture("OrchidMod/Glowmasks/MushroomLeggings_Legs_Glowmask");
					color = new Color(250, 250, 250, 200) * OrchidWorld.alchemistMushroomArmorProgress;
				}
				else if (player.legs == OrchidMod.Instance.GetEquipSlot("AbyssalGreaves", EquipType.Legs))
				{
					texture = ModContent.GetTexture("OrchidMod/Glowmasks/AbyssalGreaves_Legs_Glowmask");
				}

				if (texture == null) return;

				Vector2 position = new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X - (float)(player.legFrame.Width / 2) + (float)(player.width / 2))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)player.height - (float)player.legFrame.Height + 4f))) + player.legPosition + drawInfo.legOrigin;
				var drawData = new DrawData(texture, position, new Rectangle?(player.legFrame), color * player.stealth, player.legRotation, drawInfo.legOrigin, 1f, drawInfo.spriteEffects, 0)
				{
					shader = drawInfo.legArmorShader
				};
				Main.playerDrawData.Add(drawData);
			}
		});

		private static readonly PlayerLayer WingsGlowmaskLayer = new PlayerLayer("OrchidMod", "WingsGlowmask", PlayerLayer.Wings, delegate (PlayerDrawInfo drawInfo)
		{
			var player = drawInfo.drawPlayer;
			if (player.wings <= 0 || drawInfo.shadow != 0) return;

			bool flag = (player.wings == 0 || player.velocity.Y == 0f) &&
						(player.inventory[player.selectedItem].type == ItemID.LeafBlower || player.inventory[player.selectedItem].type == ItemID.Clentaminator ||
						player.inventory[player.selectedItem].type == ItemID.HeatRay || player.inventory[player.selectedItem].type == ItemID.EldMelter ||
						player.turtleArmor || player.body == 106 || player.body == 170);

			if (!flag)
			{
				Texture2D texture = null;
				Color color = new Color(250, 250, 250);

				if (player.wings == OrchidMod.Instance.GetEquipSlot("AbyssalWings", EquipType.Wings))
				{
					texture = ModContent.GetTexture("OrchidMod/Glowmasks/AbyssalWings_Wings_Glowmask");
				}

				if (texture == null) return;

				Vector2 position = new Vector2((float)((int)(drawInfo.position.X - Main.screenPosition.X + (float)(player.width / 2) - (float)(9 * player.direction))), (float)((int)(drawInfo.position.Y - Main.screenPosition.Y + (float)(player.height / 2) + 2f * player.gravDir)));
				var drawData = new DrawData(texture, position, new Rectangle?(new Rectangle(0, Main.wingsTexture[player.wings].Height / 4 * player.wingFrame, texture.Width, texture.Height / 4)), color, player.bodyRotation, new Vector2((float)(texture.Width / 2), (float)(texture.Height / 8)), 1f, drawInfo.spriteEffects, 0)
				{
					shader = drawInfo.wingShader
				};

				Main.playerDrawData.Add(drawData);
			}
		});
	}
}
