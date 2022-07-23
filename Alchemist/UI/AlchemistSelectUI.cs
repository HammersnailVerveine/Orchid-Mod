using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrchidMod.Alchemist.Bag;
using OrchidMod.Alchemist.Misc;
using OrchidMod.Common;
using OrchidMod.Common.Graphics;
using OrchidMod.Common.UIs;
using OrchidMod.Utilities;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Alchemist.UI
{
	public partial class AlchemistSelectUI : OrchidUIState
	{
		private static bool CanInteract
			=> UISystem.GetUIState<AlchemistUIState>().Visible && !Fields.InitTimer.Active;
		private static OrchidAlchemist Alchemist
			=> Main.LocalPlayer.GetModPlayer<OrchidAlchemist>();

		// ...

		private bool visible;
		private Vector2 center;
		private List<SelectUIElement> selectUIElements;

		// ...
		public override float Priority
			=> 5f;

		public override bool Visible
		{
			get => visible;
			set
			{
				visible = value;
				if (value) OnChangeVisibleToTrue();
			}
		}

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
			=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		public override void OnInitialize()
		{
			Fields.Load();

			selectUIElements = new();
		}

		public override void Unload()
		{
			Fields.Unload();
		}

		public override void Update(GameTime gameTime)
		{
			if (!Visible) return;

			var player = Main.LocalPlayer;
			var distanceToCenter = Vector2.Distance(center, Main.MouseScreen);

			if (player.HeldItem.ModItem is not UIItemNew || distanceToCenter > 170)
			{
				Visible = false;
				return;
			}

			if (Fields.InitTimer.Active)
			{
				var elementsCount = selectUIElements.Count;
				var invertInitProgress = MathHelper.SmoothStep(0, 1, (Fields.InitTimer.Value - 1) / (float)Fields.InitTimer.InitValue);
				var initProgress = 1.0f - invertInitProgress;
				var rotationOffset = invertInitProgress * MathHelper.PiOver2;
				var positionOffset = Fields.PotionBackgroundTexture.Size() * 0.5f;

				var vectorToFirstCircle = new Vector2(0, -52 * initProgress);
				var vectorToSecondCircle = new Vector2(0, -103 * initProgress);

				var nbMaxInFirstCircle = 6;
				var nbInFirstCircle = Math.Min(elementsCount, nbMaxInFirstCircle);
				var nbInSecondCircle = Math.Max(elementsCount - nbMaxInFirstCircle, 0);

				for (int i = 0; i < nbInFirstCircle; i++)
				{
					var uiElement = selectUIElements.ElementAt(i);
					var rotation = MathHelper.TwoPi / nbInFirstCircle * i - rotationOffset;
					var position = center + vectorToFirstCircle.RotatedBy(rotation) - positionOffset;

					uiElement.Left.Set(position.X, 0);
					uiElement.Top.Set(position.Y, 0);

					uiElement.RotationFromCenter = rotation;
				}

				if (elementsCount > nbMaxInFirstCircle)
				{
					for (int i = nbMaxInFirstCircle; i < elementsCount; i++)
					{
						var j = i - nbMaxInFirstCircle;
						var uiElement = selectUIElements.ElementAt(i);
						var rotation = MathHelper.TwoPi / nbInSecondCircle * j - rotationOffset;
						var position = center + vectorToSecondCircle.RotatedBy(rotation) - positionOffset;

						uiElement.Left.Set(position.X, 0);
						uiElement.Top.Set(position.Y, 0);

						uiElement.RotationFromCenter = rotation;
					}
				}

				Recalculate();
			}

			base.Update(gameTime);
		}

		public override void Click(UIMouseEvent evt)
		{
			base.Click(evt);
			Visible = false;
		}

		public override void RightClick(UIMouseEvent evt)
		{
			base.RightClick(evt);
			Visible = false;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			/*var sbInfo = new SpriteBatchInfo(spriteBatch);

			spriteBatch.End();
			sbInfo.Begin(spriteBatch, SpriteSortMode.Deferred, BlendState.Additive);

			var texture = OrchidAssets.GetExtraTexture(20);
			var initProgress = MathHelper.SmoothStep(0, 1, 1.0f - Fields.InitTimer.Value / (float)Fields.InitTimer.InitValue);

			spriteBatch.Draw(texture.Value, center, null, Color.White * initProgress * 0.4f, (float)Main.timeForVisualEffects * 0.025f, texture.Size() * 0.5f, 0.475f * initProgress, SpriteEffects.None, 0f);

			spriteBatch.End();
			sbInfo.Begin(spriteBatch);*/
		}

		public void OnChangeVisibleToTrue()
		{
			var player = Main.LocalPlayer;
			var alchemist = Alchemist;

			if (player.HeldItem.ModItem is not UIItemNew uiItem) return;

			PlayerInput.SetZoom_UI();
			Fields.InitTimer.Restart();

			Elements.RemoveAll(i => i is SelectUIElement);

			selectUIElements.Clear();
			center = Main.MouseScreen;

			var maxPotionsInUI = 17;
			var potions = player.GetModPlayer<PotionBagPlayer>().GetPotionsFromInventoryAndBags().Take(maxPotionsInUI);
			var initPosition = center - Fields.PotionBackgroundTexture.Size() * 0.5f;
			//sortedPotions.Sort((x, y) => (x.ModItem as OrchidModAlchemistItem).element.CompareTo((y.ModItem as OrchidModAlchemistItem).element));

			UpdateMarkedPotions(alchemist, potions);

			SelectUIElement uiElement = new QuickMixUIElement();
			uiElement.Left.Set(initPosition.X, 0);
			uiElement.Top.Set(initPosition.Y, 0);

			Append(uiElement);
			selectUIElements.Add(uiElement);

			for (int i = 0; i < potions.Count(); i++)
			{
				var item = potions.ElementAt(i);
				uiElement = new PotionUIElement(item);

				uiElement.Left.Set(initPosition.X, 0);
				uiElement.Top.Set(initPosition.Y, 0);

				Append(uiElement);
				selectUIElements.Add(uiElement);
			}

			PlayerInput.SetZoom_World();
		}

		public void UpdateMarkedPotions(OrchidAlchemist alchemist, IEnumerable<Item> potions)
		{
			var counter = 0;

			foreach (var potion in potions)
			{
				if (alchemist.PotionIsMarked(potion.ModItem as OrchidModAlchemistItem)) counter++;
			}

			if (counter != alchemist.GetMarkedPotionsCount())
			{
				alchemist.ClearMarkedPotions();
			}
		}

		// ...

		private abstract class SelectUIElement : UIElement
		{
			public const float Scale = 0.9f;
			public float RotationFromCenter { get; set; }

			public abstract Asset<Texture2D> BackgroundTexture { get; }

			// ...

			public SelectUIElement()
			{
				Width.Set(BackgroundTexture.Width() * Scale, 0);
				Height.Set(BackgroundTexture.Height() * Scale, 0);
			}

			// ...

			public virtual void Update() { }
			public virtual void OnHover() { }
			public virtual void RightClick() { }
			public virtual void DrawSelf(SpriteBatch spriteBatch, float progress, Color color) { }

			// ...

			public sealed override void Update(GameTime gameTime)
			{
				Update();

				if (IsMouseHovering && CanInteract && CheckVectorIsInsideCircle(Main.MouseScreen))
				{
					OnHover();
				}
			}

			public sealed override void RightClick(UIMouseEvent evt)
			{
				if (IsMouseHovering && CanInteract && CheckVectorIsInsideCircle(Main.MouseScreen))
				{
					RightClick();
				}
			}

			protected sealed override void DrawSelf(SpriteBatch spriteBatch)
			{
				var initProgress = MathHelper.SmoothStep(0, 1, 1.0f - Fields.InitTimer.Value / (float)Fields.InitTimer.InitValue);
				var color = Color.White * initProgress;

				spriteBatch.Draw(BackgroundTexture.Value, new Vector2(Left.Pixels, Top.Pixels), null, color * 0.75f, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);

				DrawSelf(spriteBatch, initProgress, color);
			}

			// ...

			private bool CheckVectorIsInsideCircle(Vector2 vector)
			{
				var style = GetDimensions();
				var center = style.Center();
				var halfWidth = style.Width / 2;

				return Vector2.Distance(vector, center) <= halfWidth;
			}
		}

		private class QuickMixUIElement : SelectUIElement
		{
			public override Asset<Texture2D> BackgroundTexture
				=> Fields.QuickMixBackgroundTexture;

			public override void OnHover()
			{
				OrchidMouseText.SetTooltipsData(
					tooltipLines:
						new List<TooltipLine>()
						{
							new TooltipLine(OrchidMod.Instance, "0", "Quick Mixing") { OverrideColor = Color.Gold },
							new TooltipLine(OrchidMod.Instance, "1", "Mixes all potions marked with a star"),
							new TooltipLine(OrchidMod.Instance, "2", "All marks will be erased if at least one marked potion is missing"),
							new TooltipLine(OrchidMod.Instance, "3", $"Currently marked {Alchemist.GetMarkedPotionsCount()} potions"),
							new TooltipLine(OrchidMod.Instance, "4", "{Alt+Click on potion to mark}") { OverrideColor = Color.Gray }
						},
					style:
						Fields.TooltipsStyle
				);
			}
		}

		private class PotionUIElement : SelectUIElement
		{
			public Item Item { get; init; }
			public OrchidModAlchemistItem AlchemistItem { get; init; }
			public OrchidModGlobalItem GlobalItem { get; init; }
			public bool IsMarked { get; set; }
			public bool IsBlocked { get; set; }

			public PotionUIElement(Item item)
			{
				Item = item;
				AlchemistItem = Item.ModItem as OrchidModAlchemistItem;
				GlobalItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			}

			public override Asset<Texture2D> BackgroundTexture
				=> Fields.PotionBackgroundTexture;

			public override void Update()
			{
				var alchemist = Alchemist;
				var potencyCost = GlobalItem.alchemistPotencyCost;

				IsMarked = alchemist.PotionIsMarked(AlchemistItem);
				IsBlocked =
					alchemist.ContainsAlchemistElement(AlchemistItem.element)
					|| alchemist.alchemistPotency < potencyCost + 1
					|| alchemist.alchemistNbElements >= alchemist.alchemistNbElementsMax;
			}

			public override void OnHover()
			{
				var tooltips = new List<TooltipLine>()
				{
					new TooltipLine(OrchidMod.Instance, "0", Item.HoverName) { OverrideColor = ItemRarity.GetColor(Item.rare) },
					new TooltipLine(OrchidMod.Instance, "1", $"Element: {AlchemistItem.element.ToString().FirstCharToUpper(true)}")
				};

				if (Main.keyState.IsKeyDown(Main.FavoriteKey))
				{
					Main.cursorOverride = 3;
				}

				if (IsMarked)
				{
					tooltips.Add(new TooltipLine(OrchidMod.Instance, "2", "Marked for quick mixing"));
				}

				OrchidMouseText.SetTooltipsData(tooltipLines: tooltips, style: Fields.TooltipsStyle);
			}

			public override void RightClick()
			{
				if (Main.keyState.IsKeyDown(Main.FavoriteKey))
				{
					if (IsMarked) Alchemist.RemoveMarkedPotion(AlchemistItem);
					else Alchemist.AddMarkedPotion(AlchemistItem);
				}
				else
				{
					if (IsBlocked)
					{
						switch (Main.rand.NextBool(2))
						{
							case true:
								SoundEngine.PlaySound(SoundID.Item112);
								break;
							case false:
								SoundEngine.PlaySound(SoundID.Item111);
								break;
						}
					}
					else
					{
						AddFlask();
					}
					
				}
			}

			public override void DrawSelf(SpriteBatch spriteBatch, float progress, Color color)
			{
				var oldColor = color;

				color = IsBlocked ? new(70, 70, 70) : color;

				var vector = Fields.PotionBackgroundTexture.Size() * Scale;
				var itemType = Item.type;
				var itemTexture = TextureAssets.Item[itemType].Value;
				var rectangle = Main.itemAnimations[itemType] is null ? itemTexture.Frame(1, 1, 0, 0, 0, 0) : Main.itemAnimations[itemType].GetFrame(itemTexture, -1);
				var scale = 1f;
				var currentColor = color;
				var borderColor = GetBorderColor(AlchemistItem.element) * progress;

				ItemSlot.GetItemLight(ref currentColor, ref scale, Item, false);

				var num8 = 1f;

				if (rectangle.Width > 32 || rectangle.Height > 32)
				{
					num8 = (rectangle.Width <= rectangle.Height) ? (32f / rectangle.Height) : (32f / rectangle.Width);
				}

				num8 *= Scale;

				Vector2 position = new Vector2(Left.Pixels, Top.Pixels) + vector / 2f - rectangle.Size() * num8 / 2f;
				Vector2 origin = rectangle.Size() * (scale / 2f - 0.5f);

				spriteBatch.Draw(Fields.PotionBackgroundTexture.Value, new Vector2(Left.Pixels, Top.Pixels), null, color * 0.75f, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);

				Vector2 offset = Fields.PotionBorderTexture.Size() * 0.5f * Scale;
				spriteBatch.Draw(Fields.PotionBorderTexture.Value, new Vector2(Left.Pixels, Top.Pixels) + offset, null, oldColor * 0.75f, RotationFromCenter + MathHelper.PiOver4, Fields.PotionBorderTexture.Size() * 0.5f, Scale, SpriteEffects.None, 0);
				spriteBatch.Draw(Fields.PotionElementTexture.Value, new Vector2(Left.Pixels, Top.Pixels) + offset, null, borderColor * 0.75f, RotationFromCenter + MathHelper.PiOver4, Fields.PotionElementTexture.Size() * 0.5f, Scale, SpriteEffects.None, 0);

				if (IsMarked)
				{
					spriteBatch.Draw(Fields.PotionMarkedTexture.Value, new Vector2(Left.Pixels, Top.Pixels), null, oldColor * 0.75f, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
				}

				if (ItemLoader.PreDrawInInventory(Item, spriteBatch, position, rectangle, Item.GetAlpha(currentColor), Item.GetColor(color), origin, num8 * scale))
				{
					spriteBatch.Draw(itemTexture, position, new Rectangle?(rectangle), Item.GetAlpha(currentColor), 0f, origin, num8 * scale, SpriteEffects.None, 0f);

					if (Item.color != Color.Transparent)
					{
						spriteBatch.Draw(itemTexture, position, new Rectangle?(rectangle), Item.GetColor(color), 0f, origin, num8 * scale, SpriteEffects.None, 0f);
					}
				}

				ItemLoader.PostDrawInInventory(Item, spriteBatch, position, rectangle, Item.GetAlpha(currentColor), Item.GetColor(color), origin, num8 * scale);
			}

			public Color GetBorderColor(AlchemistElement elementType)
			{
				return elementType switch
				{
					AlchemistElement.WATER => new Color(0, 119, 190),
					AlchemistElement.FIRE => new Color(194, 38, 31),
					AlchemistElement.NATURE => new Color(75, 139, 59),
					AlchemistElement.AIR => new Color(167, 231, 255),
					AlchemistElement.LIGHT => new Color(255, 255, 102),
					AlchemistElement.DARK => new Color(138, 43, 226),
					_ => throw new NotImplementedException()
				};
			}

			// ...

			private void AddFlask()
			{
				var rightClickDust = GlobalItem.alchemistRightClickDust;
				var potencyCost = GlobalItem.alchemistPotencyCost;
				var colorR = GlobalItem.alchemistColorR;
				var colorG = GlobalItem.alchemistColorG;
				var colorB = GlobalItem.alchemistColorB;

				OrchidModAlchemistItem.playerAddFlask(Main.LocalPlayer, AlchemistItem.element, Item.type, Item.damage, potencyCost, rightClickDust, colorR, colorG, colorB);

				switch (Main.rand.Next(3))
				{
					case 1:
						SoundEngine.PlaySound(SoundID.Item86);
						break;
					case 2:
						SoundEngine.PlaySound(SoundID.Item87);
						break;
					default:
						SoundEngine.PlaySound(SoundID.Item85);
						break;
				}

				for (int k = 0; k < 5; k++)
				{
					Dust.NewDust(Main.LocalPlayer.Center, 10, 10, rightClickDust);
				}
			}
		}
	}

	/*public class AlchemistSelectUIState : OrchidUIState
	{
		public Color backgroundColor = Color.White;
		public Color backgroundColorGrayed = new Color(90, 90, 90);
		public Color backgroundColorDark = new Color(180, 180, 180);
		public Point mouseDiff = new Point(0, 0);
		private static int drawOffSet = 21;
		private static int drawSize = 42;
		public int nbAlchemistWeapons = 0;
		public double displayAngle = 0;
		public int distanceToPoint = 0;
		public Point displayPoint = new Point(0, 0);
		public List<Rectangle> displayRectangles = new List<Rectangle>();
		public List<Item> displayItems = new List<Item>();

		public static Texture2D resourceBack;
		public static Texture2D resourceItem;
		public static Texture2D resourceCross;
		public static Texture2D resourceSelected;
		public static Texture2D resourceBorder;

		public override int InsertionIndex(List<GameInterfaceLayer> layers)
			=> layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		public override void OnInitialize()
		{
			resourceBack = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistSelectItemBackground", AssetRequestMode.ImmediateLoad).Value;
			resourceItem = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistSelectItemBackground", AssetRequestMode.ImmediateLoad).Value;
			resourceCross = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistSelectItemBackgroundCross", AssetRequestMode.ImmediateLoad).Value;
			resourceSelected = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistSelectItemBackgroundSelected", AssetRequestMode.ImmediateLoad).Value;
			resourceBorder = ModContent.Request<Texture2D>("OrchidMod/Alchemist/UI/Textures/AlchemistSelectItemBackgroundBorder", AssetRequestMode.ImmediateLoad).Value;

			Width.Set(0f, 0f);
			Height.Set(0f, 0f);
			Left.Set(Main.screenWidth - 64f, 0f);
			Top.Set(Main.screenHeight - 64f, 0f);
			backgroundColor = Color.White;

			this.setRectangles();
			Recalculate();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Recalculate();
			Player player = Main.LocalPlayer;
			OrchidAlchemist modPlayer = player.GetModPlayer<OrchidAlchemist>();

			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			bool noReposition = false;

			if (!player.dead)
			{
				if (modPlayer.alchemistSelectUIDisplay && !modPlayer.alchemistSelectUIKeysDisplay)
				{
					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						if (modPlayer.alchemistNbElements > 0)
						{
							//float shootSpeed = 10f * modPlayer.alchemistVelocity;
							//int projType = ProjectileType<Alchemist.Projectiles.AlchemistProj>();
							SoundEngine.PlaySound(SoundID.Item106);
							modPlayer.alchemistSelectUIDisplay = false;
							modPlayer.alchemistShootProjectile = true;
							return;
						}
					}

					if (Main.mouseRight && Main.mouseRightRelease && !modPlayer.alchemistSelectUIInitialize)
					{
						for (int i = 0; i < this.displayRectangles.Count(); i++)
						{
							if (displayRectangles[i].Contains(new Point((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y)) && i <= displayItems.Count())
							{
								if (i == 0)
								{
									modPlayer.alchemistSelectUIDisplay = false;
								}
								else
								{
									Item item = displayItems[i - 1];
									if (item.type != 0)
									{
										OrchidModGlobalItem orchidItem = displayItems[i - 1].GetGlobalItem<OrchidModGlobalItem>();
										AlchemistElement element = orchidItem.alchemistElement;
										int damage = item.damage;
										int flaskType = item.type;
										int potencyCost = orchidItem.alchemistPotencyCost;
										int rightClickDust = orchidItem.alchemistRightClickDust;
										int colorR = orchidItem.alchemistColorR;
										int colorG = orchidItem.alchemistColorG;
										int colorB = orchidItem.alchemistColorB;
										bool noPotency = modPlayer.alchemistPotency < potencyCost + 1;
										bool alreadyContains = false;
										if ((int)element > 0 && (int)element < 7)
										{
											alreadyContains = modPlayer.alchemistElements[(int)element - 1];
										}
										if (alreadyContains || noPotency
										|| modPlayer.alchemistNbElements >= modPlayer.alchemistNbElementsMax
										|| element == AlchemistElement.NULL || flaskType == 0)
										{
											if (noPotency && !alreadyContains)
											{
												SoundEngine.PlaySound(SoundID.SplashWeak);
											}
											else
											{
												if (Main.rand.Next(2) == 0)
												{
													SoundEngine.PlaySound(SoundID.Item112);
												}
												else
												{
													SoundEngine.PlaySound(SoundID.Item111);
												}
											}
										}
										else
										{
											OrchidModAlchemistItem.playerAddFlask(player, element, flaskType, damage, potencyCost, rightClickDust, colorR, colorG, colorB);
											int rand = Main.rand.Next(3);
											switch (rand)
											{
												case 1:
													SoundEngine.PlaySound(SoundID.Item86);
													break;
												case 2:
													SoundEngine.PlaySound(SoundID.Item87);
													break;
												default:
													SoundEngine.PlaySound(SoundID.Item85);
													break;
											}

											for (int k = 0; k < 5; k++)
												Dust.NewDust(player.Center, 10, 10, rightClickDust);
										}
									}
								}
								noReposition = true;
							}
						}
					}

					if (modPlayer.alchemistSelectUIInitialize)
					{
						this.initUI(player, modPlayer, ref mouseDiff, ref displayPoint);
						return;
					}

					displayPoint.X = point.X - mouseDiff.X;
					displayPoint.Y = point.Y - mouseDiff.Y;

					String msg = "";
					bool anySelected = false;
					for (int i = 0; i < this.nbAlchemistWeapons + 1; i++)
					{
						bool selected = displayRectangles[i].Contains(new Point((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y));
						anySelected = anySelected ? anySelected : selected;
						if (i > 0)
						{
							Item item = this.displayItems[i - 1];
							if (item.type != 0)
							{
								OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
								AlchemistElement element = orchidItem.alchemistElement;
								bool elementSelected = false;
								if ((int)element > 0 && (int)element < 7)
								{
									elementSelected = modPlayer.alchemistElements[(int)element - 1];
								}
								Color borderColor = new Color(0, 0, 0);
								switch (element)
								{
									case AlchemistElement.WATER:
										borderColor = selected ? new Color(0, 119, 190) : new Color(0, 69, 140);
										break;
									case AlchemistElement.FIRE:
										borderColor = selected ? new Color(194, 38, 31) : new Color(104, 0, 0);
										break;
									case AlchemistElement.NATURE:
										borderColor = selected ? new Color(75, 139, 59) : new Color(25, 89, 9);
										break;
									case AlchemistElement.AIR:
										borderColor = selected ? new Color(116, 181, 205) : new Color(66, 131, 155);
										break;
									case AlchemistElement.LIGHT:
										borderColor = selected ? new Color(255, 255, 102) : new Color(205, 205, 52);
										break;
									case AlchemistElement.DARK:
										borderColor = selected ? new Color(138, 43, 226) : new Color(88, 0, 176);
										break;
									default:
										break;
								}
								Color color = selected ? elementSelected ? backgroundColorDark : backgroundColor : elementSelected ? backgroundColorGrayed : backgroundColorDark;
								spriteBatch.Draw(resourceBack, this.displayRectangles[i], color);
								spriteBatch.Draw(resourceBorder, this.displayRectangles[i], borderColor);
								resourceItem = TextureAssets.Item[item.type].Value;
								Rectangle insideRectangle = new Rectangle(displayRectangles[i].X + 6, displayRectangles[i].Y + 6, 30, 30);
								Rectangle itemRectangle = insideRectangle;
								if (((resourceItem.Width / 2) % 2) == 0)
								{
									itemRectangle = new Rectangle(displayRectangles[i].X + 7, displayRectangles[i].Y + 6, 28, 30);
								}
								spriteBatch.Draw(resourceItem, itemRectangle, color);
								msg = selected ? item.Name : msg;
							}
						}
						else
						{
							Color color = selected ? backgroundColor : backgroundColorDark;
							spriteBatch.Draw(resourceCross, this.displayRectangles[i], color);
							if (selected)
							{
								spriteBatch.Draw(resourceSelected, this.displayRectangles[i], color);
								msg = "Close";
							}
						}
					}
					if (msg != "")
					{
						ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.Value, msg, Main.MouseScreen + new Vector2(15f, 15f), new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor), 0f, Vector2.Zero, Vector2.One, -1f, 2f);
					}

					if (!noReposition && !anySelected && ((Main.mouseRight && Main.mouseRightRelease) || (Main.mouseLeft && Main.mouseLeftRelease)))
					{
						this.initUI(player, modPlayer, ref mouseDiff, ref displayPoint);
						return;
					}

					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						if (displayRectangles[0].Contains(new Point((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y)) && Main.mouseLeft && Main.mouseLeftRelease)
						{
							modPlayer.alchemistSelectUIDisplay = false;
						}
					}
				}
			}
		}

		public void initUI(Player player, OrchidAlchemist modPlayer, ref Point mouseDiff, ref Point displayPoint)
		{
			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			modPlayer.alchemistSelectUIInitialize = false;
			SoundEngine.PlaySound(SoundID.Item7);
			mouseDiff = new Point(point.X - (int)Main.MouseScreen.X, point.Y - (int)Main.MouseScreen.Y);
			displayPoint.X = point.X - mouseDiff.X;
			displayPoint.Y = point.Y - mouseDiff.Y;
			this.checkInventory(modPlayer);
			this.setRectangles();
		}

		public void checkInventory(OrchidAlchemist modPlayer)
		{
			this.nbAlchemistWeapons = 0;
			int val = this.displayRectangles.Count() - 1;
			this.displayItems = new List<Item>();

			foreach (Item item in Main.LocalPlayer.GetModPlayer<PotionBagPlayer>().GetPotionsFromInventoryAndBags())
			{
				if (item.type != 0)
				{
					OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
					if (orchidItem.alchemistWeapon)
					{
						this.nbAlchemistWeapons++;
						this.displayItems.Add(item);
						if (this.nbAlchemistWeapons >= val)
						{
							break;
						}
					}
				}
			}
			if (this.nbAlchemistWeapons > val)
			{
				this.nbAlchemistWeapons = val;
			}
			if (this.nbAlchemistWeapons > 0)
			{
				this.displayAngle = 360 / this.nbAlchemistWeapons;
			}
			else
			{
				this.displayAngle = 360;
			}
			this.distanceToPoint = (int)(drawOffSet * 3 + (drawOffSet * this.nbAlchemistWeapons / 4));
		}

		public void setRectangles()
		{
			this.displayRectangles = new List<Rectangle>();
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet, displayPoint.Y - drawOffSet, drawSize, drawSize));
			
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize, displayPoint.Y - drawOffSet - drawSize / 2 - 1, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - 0, displayPoint.Y - drawOffSet - drawSize - 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize, displayPoint.Y - drawOffSet - drawSize / 2 - 1, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize, displayPoint.Y - drawOffSet + drawSize / 2 + 1, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + 0, displayPoint.Y - drawOffSet + drawSize + 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize, displayPoint.Y - drawOffSet + drawSize / 2 + 1, drawSize, drawSize));
			
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - 0, displayPoint.Y - drawOffSet - drawSize * 2 - 4, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize, displayPoint.Y - drawOffSet - drawSize - drawSize / 2 - 3, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize, displayPoint.Y - drawOffSet - drawSize - drawSize / 2 - 3, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - 0, displayPoint.Y - drawOffSet + drawSize * 2 + 4, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize, displayPoint.Y - drawOffSet + drawSize + drawSize / 2 + 3, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize, displayPoint.Y - drawOffSet + drawSize + drawSize / 2 + 3, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize * 2, displayPoint.Y - drawOffSet - drawSize - 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize * 2, displayPoint.Y - drawOffSet - 0, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet - drawSize * 2, displayPoint.Y - drawOffSet + drawSize + 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize * 2, displayPoint.Y - drawOffSet - drawSize - 2, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize * 2, displayPoint.Y - drawOffSet + 0, drawSize, drawSize));
			this.displayRectangles.Add(new Rectangle(displayPoint.X - drawOffSet + drawSize * 2, displayPoint.Y - drawOffSet + drawSize + 2, drawSize, drawSize));
		}
	}*/
}