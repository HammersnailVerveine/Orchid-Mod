using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Common.ModSystems;
using OrchidMod.Content.Guardian.Misc;
using OrchidMod.Utilities;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Guardian.Armors.Horizon
{
	[AutoloadEquip(EquipType.Head)]
	public class GuardianHorizonHead : OrchidModGuardianEquipable
	{
		public override void SafeSetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 7, 0, 0);
			Item.rare = ItemRarityID.Red;
			Item.defense = 28;
		}

		public override void UpdateEquip(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			modPlayer.GuardianGuardMax += 3;
			player.GetCritChance<GuardianDamageClass>() += 25;
			player.endurance += 0.1f;
			player.aggro += 500;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ItemType<GuardianHorizonChest>() && legs.type == ItemType<GuardianHorizonLegs>();
		}

		public override void UpdateArmorSet(Player player)
		{
			OrchidGuardian modPlayer = player.GetModPlayer<OrchidGuardian>();
			player.setBonus = "While above 50% life, consumes 20 life to recover guardian charges";
			modPlayer.GuardianHorizon = true;
		}

		/*
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			spriteBatch.DrawSimpleItemGlowmaskInWorld(Item, new Color(250, 250, 250, 200) * OrchidWorld.alchemistMushroomArmorProgress, rotation, scale);
		}
		*/

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Color color = new Color(229, 181, 142) * 0.5f;
			Lighting.AddLight(Item.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}

		public override void EquipFrameEffects(Player player, EquipType type)
		{
			Color color = new Color(229, 181, 142) * 0.75f;
			Lighting.AddLight(player.Center, color.R / 255f, color.G / 255f, color.B / 255f);
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient<HorizonFragment>(10);
			recipe.AddIngredient(ItemID.LunarBar, 8);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}

	public class GuardianHorizonHeadGlowmask : PlayerDrawLayer
	{
		public override Position GetDefaultPosition()
		{
			return new AfterParent(PlayerDrawLayers.Head);
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			/*
			SpriteBatch spriteBatch = Main.spriteBatch;
			spriteBatch.End(out SpriteBatchSnapshot spriteBatchSnapshot);
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			*/

			Player drawPlayer = drawInfo.drawPlayer;
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}

			if (drawPlayer.armor[10].type == ItemType<GuardianHorizonHead>() || (drawPlayer.armor[10].type == ItemID.None && drawPlayer.armor[0].type == ItemType<GuardianHorizonHead>()))
			{
				Color color = drawPlayer.GetImmuneAlphaPure(Color.White, drawInfo.shadow);

				Texture2D texture = Request<Texture2D>("OrchidMod/Content/Guardian/Armors/Horizon/GuardianHorizonHead_Head_Glow").Value;
				Vector2 drawPos = drawInfo.Position - Main.screenPosition + new Vector2(drawPlayer.width / 2 - drawPlayer.bodyFrame.Width / 2, drawPlayer.height - drawPlayer.bodyFrame.Height + 4f) + drawPlayer.headPosition;
				Vector2 headVect = drawInfo.headVect;
				DrawData drawData = new DrawData(texture, drawPos.Floor() + headVect, drawPlayer.bodyFrame, color, drawPlayer.headRotation, headVect, 1f, drawInfo.playerEffect, 0)
				{
					shader = GameShaders.Armor.GetShaderIdFromItemId(ItemType<HorizonDye>())
				};
				//GameShaders.Misc["OrchidMod:HorizonGlow"].Apply(drawData);
				drawInfo.DrawDataCache.Add(drawData);
			}
			/*
			spriteBatch.End();
			spriteBatch.Begin(spriteBatchSnapshot);
			*/
		}
	}
}
