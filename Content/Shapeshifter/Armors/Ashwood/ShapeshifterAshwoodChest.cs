using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModObjects;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Shapeshifter.Armors.Ashwood
{
	[AutoloadEquip(EquipType.Body)]
	public class ShapeshifterAshwoodChest : OrchidModShapeshifterItem
	{
		public static Texture2D TextureGlow;

		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 8;
			TextureGlow ??= ModContent.Request<Texture2D>(Item.ModItem.Texture + "_Body_Glow", AssetRequestMode.ImmediateLoad).Value;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage<ShapeshifterDamageClass>() += 0.07f;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.AshWood, 30);
			recipe.AddIngredient(ItemID.HellstoneBar, 16);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}


	public class ShapeshifterAshwoodChestGlowmask : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Torso);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}

			if (drawPlayer.armor[11].type == ItemType<ShapeshifterAshwoodChest>() || (drawPlayer.armor[11].type == ItemID.None && drawPlayer.armor[1].type == ItemType<ShapeshifterAshwoodChest>()))
			{
				OrchidPlayer modPlayer = drawPlayer.GetModPlayer<OrchidPlayer>();
				float colorFlicker = (0.4f + (float)(Math.Sin(modPlayer.Timer * 0.04851f) * 0.1f) + (float)(Math.Sin(modPlayer.Timer * 0.123f) * 0.05f) + (float)(Math.Sin(modPlayer.Timer * 0.31461f) * 0.02f) + (float)(Math.Sin(modPlayer.Timer * 0.07124f) * 0.03f));
				Color color = drawPlayer.GetImmuneAlphaPure(Color.White * colorFlicker, drawInfo.shadow);

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame = new(0, 0, 40, 56);
				if (drawPlayer.Male)
				{
					if (drawPlayer.compositeFrontArm.enabled || drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56))
					{
						frame = new(0, 2, 40, 56); //walking bop
					}
					if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 5, 40, 56) && !drawPlayer.compositeFrontArm.enabled)
					{
						frame = new(40, 0, 40, 56); //jumping frame
					}
				}
				else
				{
					frame = new(0, 112, 40, 56);
					if (drawPlayer.compositeFrontArm.enabled || drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56))
					{
						frame = new(0, 114, 40, 56); //walking bop
					}
					if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 5, 40, 56) && !drawPlayer.compositeFrontArm.enabled)
					{
						frame = new(40, 112, 40, 56); //jumping frame
					}
				}
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(ShapeshifterAshwoodChest.TextureGlow, position, frame, color, rotation, origin, 1f, spriteEffects, 0);
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}

	public class ShapeshifterAshwoodChestGlowmaskShoulder : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.ArmOverItem);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}

			if (drawPlayer.armor[11].type == ItemType<ShapeshifterAshwoodChest>() || (drawPlayer.armor[11].type == ItemID.None && drawPlayer.armor[1].type == ItemType<ShapeshifterAshwoodChest>()) && (drawPlayer.bodyFrame != new Rectangle(0, 56 * 5, 40, 56) || drawPlayer.compositeFrontArm.enabled))
			{
				OrchidPlayer modPlayer = drawPlayer.GetModPlayer<OrchidPlayer>();
				float colorFlicker = (0.5f + (float)(Math.Sin(modPlayer.Timer * 0.04851f) * 0.1f) + (float)(Math.Sin(modPlayer.Timer * 0.123f) * 0.05f) + (float)(Math.Sin(modPlayer.Timer * 0.31461f) * 0.02f) + (float)(Math.Sin(modPlayer.Timer * 0.07124f) * 0.03f));
				Color color = drawPlayer.GetImmuneAlphaPure(Color.White * colorFlicker, drawInfo.shadow);

				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
				Vector2 origin = drawInfo.bodyVect;
				Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
				Rectangle frame = new(0, 56, 40, 56);
				if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56))
				{
					frame = new(0, 58, 40, 56); //walking bop
				}
				if (!drawPlayer.Male)
				{
					frame = new(0, 168, 40, 56);
					if (drawPlayer.bodyFrame == new Rectangle(0, 56 * 7, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 8, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 9, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 14, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 15, 40, 56) || drawPlayer.bodyFrame == new Rectangle(0, 56 * 16, 40, 56))
					{
						frame = new(0, 170, 40, 56); //walking bop
					}
				}
				float rotation = drawPlayer.bodyRotation;
				SpriteEffects spriteEffects = drawInfo.playerEffect;

				DrawData drawData = new(ShapeshifterAshwoodChest.TextureGlow, position, frame, color, rotation, origin, 1f, spriteEffects, 0);
				drawInfo.DrawDataCache.Add(drawData);
			}
		}
	}
}
