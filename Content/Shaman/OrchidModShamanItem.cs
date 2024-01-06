using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using OrchidMod.Common.Attributes;
using OrchidMod.Common;
using Terraria.Audio;

namespace OrchidMod.Content.Shaman
{
	public enum ShamanCatalystType : int
	{
		IDLE = 0,
		ROTATE = 1
	}

	public enum ShamanSummonMovement : int
	{
		CUSTOM = 0,
		TOWARDSTARGET = 1,
		FLOATABOVE = 2
	}

	[ClassTag(ClassTags.Shaman)]
	public abstract class OrchidModShamanItem : ModItem
	{
		public ShamanElement Element = ShamanElement.NULL;
		public Color? catalystEffectColor = null; // TODO: ...
		public ShamanCatalystType catalystType = ShamanCatalystType.IDLE;
		public ShamanSummonMovement catalystMovement = ShamanSummonMovement.CUSTOM;

		protected override bool CloneNewInstances => true;

		public sealed override void SetStaticDefaults()
		{
			Item.staff[Item.type] = true;
			SafeSetStaticDefaults();
		}

		public sealed override void SetDefaults()
		{
			Item.DamageType = ModContent.GetInstance<ShamanDamageClass>();

			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Thrust;

			OrchidModGlobalItem orchidItem = Item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeapon = true;

			SafeSetDefaults();

			orchidItem.shamanWeaponElement = (int)Element;
		}

		public sealed override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			OrchidShaman shaman = player.GetModPlayer<OrchidShaman>();
			Vector2 mousePosition = Main.MouseWorld;
			Vector2? catalystCenter = shaman.ShamanCatalystPosition;

			if (catalystCenter != null)
				position = catalystCenter.Value;

			Vector2 newMove = mousePosition - position;
			newMove.Normalize();
			newMove *= velocity.Length();
			velocity = newMove;

			if (SafeShoot(player, source, position, velocity, type, damage, knockback))
				NewShamanProjectile(player, source, position, velocity, type, damage, knockback);

			return false;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			OrchidShaman shamanPlayer = player.GetModPlayer<OrchidShaman>();
			if (player.altFunctionUse == 2)
			{ // Right click
				if (shamanPlayer.GetShamanicBondValue(Element) >= 100 && !shamanPlayer.IsShamanicBondReleased(Element))
				{
					shamanPlayer.OnReleaseShamanicBond(this);
					SoundEngine.PlaySound(SoundID.DD2_GhastlyGlaivePierce, player.Center);

					Vector2 summonPosition = player.Center;
					Projectile anchor = Main.projectile[shamanPlayer.shamanCatalystIndex];
					if (anchor.active && anchor.type == ModContent.ProjectileType<CatalystAnchor>())
					{
						summonPosition = anchor.Center;
						anchor.Kill();
						shamanPlayer.shamanCatalystIndex = -1;
					}

					var index = Projectile.NewProjectile(null, summonPosition, Vector2.Zero, ModContent.ProjectileType<CatalystSummon>(), (int)player.GetDamage<ShamanDamageClass>().ApplyTo(Item.damage), Item.knockBack, player.whoAmI);

					if (Main.projectile[index].ModProjectile is CatalystSummon catalystSummon)
					{
						catalystSummon.SelectedItem = Type;
						CatalystSummonRelease(Main.projectile[index]);
						//Main.projectile[index].netUpdate = true; // Unnecessary?
					}
					else
					{
						Main.NewText("Error creating a new summon catalyst", Color.Red);
						return false;
					}

					int duration = (Item.GetGlobalItem<Prefixes.ShamanPrefixItem>().GetBondDuration() + shamanPlayer.ShamanBondDuration) * 60;

					switch (Element)
					{
						case ShamanElement.FIRE:
							shamanPlayer.ShamanFireBondReleased = true;
							shamanPlayer.ShamanFireBondPoll = 0f;
							shamanPlayer.ShamanFireBond = duration;
							shamanPlayer.shamanSummonFireIndex = index;
							CombatText.NewText(player.Hitbox, ShamanElementUtils.GetColor(ShamanElement.FIRE), "Fire Bond Released");
							SwitchToNextWeapon(player);
							break;
						case ShamanElement.WATER:
							shamanPlayer.ShamanWaterBondReleased = true;
							shamanPlayer.ShamanWaterBondPoll = 0f;
							shamanPlayer.ShamanWaterBond = duration;
							shamanPlayer.shamanSummonWaterIndex = index;
							CombatText.NewText(player.Hitbox, ShamanElementUtils.GetColor(ShamanElement.WATER), "Water Bond Released");
							SwitchToNextWeapon(player);
							break;
						case ShamanElement.AIR:
							shamanPlayer.ShamanAirBondReleased = true;
							shamanPlayer.ShamanAirBondPoll = 0f;
							shamanPlayer.ShamanAirBond = duration;
							shamanPlayer.shamanSummonAirIndex = index;
							CombatText.NewText(player.Hitbox, ShamanElementUtils.GetColor(ShamanElement.AIR), "Air Bond Released");
							SwitchToNextWeapon(player);
							break;
						case ShamanElement.EARTH:
							shamanPlayer.ShamanEarthBondReleased = true;
							shamanPlayer.ShamanEarthBondPoll = 0f;
							shamanPlayer.ShamanEarthBond = duration;
							shamanPlayer.shamanSummonEarthIndex = index;
							CombatText.NewText(player.Hitbox, ShamanElementUtils.GetColor(ShamanElement.EARTH), "Earth Bond Released");
							SwitchToNextWeapon(player);
							break;
						case ShamanElement.SPIRIT:
							shamanPlayer.ShamanSpiritBondReleased = true;
							shamanPlayer.ShamanSpiritBondPoll = 0f;
							shamanPlayer.ShamanSpiritBond = duration;
							shamanPlayer.shamanSummonSpiritIndex = index;
							CombatText.NewText(player.Hitbox, ShamanElementUtils.GetColor(ShamanElement.SPIRIT), "Spirit Bond Released");
							SwitchToNextWeapon(player);
							break;
						default:
							break;
					}

					OnReleaseShamanicBond(player, shamanPlayer);
				}
				else
				{
					switch (Element)
					{
						case ShamanElement.FIRE:
							if (shamanPlayer.ShamanFireBondReleased)
							{
								shamanPlayer.ShamanFireBondReleased = false;
								shamanPlayer.ShamanFireBond = 0;
							}
							break;
						case ShamanElement.WATER:
							if (shamanPlayer.ShamanWaterBondReleased)
							{
								shamanPlayer.ShamanWaterBondReleased = false;
								shamanPlayer.ShamanWaterBond = 0;
							}
							break;
						case ShamanElement.AIR:
							if (shamanPlayer.ShamanAirBondReleased)
							{
								shamanPlayer.ShamanAirBondReleased = false;
								shamanPlayer.ShamanAirBond = 0;
							}
							break;
						case ShamanElement.EARTH:
							if (shamanPlayer.ShamanEarthBondReleased)
							{
								shamanPlayer.ShamanEarthBondReleased = false;
								shamanPlayer.ShamanEarthBond = 0;
							}
							break;
						case ShamanElement.SPIRIT:
							if (shamanPlayer.ShamanSpiritBondReleased)
							{
								shamanPlayer.ShamanSpiritBondReleased = false;
								shamanPlayer.ShamanSpiritBond = 0;
							}
							break;
						default:
							break;
					}
				}
				return false;
			}

			// Left click

			if (shamanPlayer.IsShamanicBondReleased((ShamanElement)Element)) return false;
			return base.CanUseItem(player);
		}

		public sealed override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			player.itemLocation = (player.MountedCenter + new Vector2(player.direction * 12, player.gravDir * 24)).Floor();
			player.itemRotation = -player.direction * player.gravDir * MathHelper.PiOver4;
		}

		public sealed override void HoldItem(Player player)
		{
			var shaman = player.GetModPlayer<OrchidShaman>();

			if (!shaman.IsShamanicBondReleased(Element))
			{
				var catalystType = ModContent.ProjectileType<CatalystAnchor>();

				if (player.ownedProjectileCounts[catalystType] == 0)
				{
					var index = Projectile.NewProjectile(null, player.Center, Vector2.Zero, catalystType, 0, 0f, player.whoAmI);
					shaman.shamanCatalystIndex = index;

					var proj = Main.projectile[index];
					if (!(proj.ModProjectile is CatalystAnchor catalyst))
					{
						proj.Kill();
						shaman.shamanCatalystIndex = -1;
					}
					else catalyst.OnChangeSelectedItem(player);
				}
				else
				{
					var proj = Main.projectile.First(i => i.active && i.owner == player.whoAmI && i.type == catalystType);
					if (proj != null && proj.ModProjectile is CatalystAnchor catalyst)
					{
						if (catalyst.SelectedItem != player.selectedItem)
						{
							catalyst.OnChangeSelectedItem(player);
						}
					}
					else shaman.shamanCatalystIndex = -1;
				}
			}

			SafeHoldItem();
		}

		public sealed override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			SafeModifyWeaponDamage(player, ref damage);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.Text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.Text = damageValue + " shamanic damage";
			}

			if (Element > 0)
			{
				Color[] colors = new Color[5]
				{
					new Color(194, 38, 31),
					new Color(0, 119, 190),
					new Color(75, 139, 59),
					new Color(255, 255, 102),
					new Color(138, 43, 226)
				};

				string[] strType = new string[5] { "Fire", "Water", "Air", "Earth", "Spirit" };

				int index = tooltips.FindIndex(ttip => ttip.Mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
				if (index != -1) tooltips.Insert(index + 1, new TooltipLine(Mod, "BondType", $"Bond type: [c/{Terraria.ID.Colors.AlphaDarken(colors[(int)Element - 1]).Hex3()}:{strType[(int)Element - 1]}]"));
			}
		}

		public virtual void SafeSetStaticDefaults() { }
		public virtual void SafeSetDefaults() { }
		public virtual void SafeHoldItem() { }
		public virtual void SafeModifyWeaponDamage(Player player, ref StatModifier damage) { }
		public virtual bool SafeShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) => true;
		public virtual void ExtraAICatalyst(Projectile projectile, bool after) { }
		public virtual void PostAICatalyst(Projectile projectile) { }
		public virtual void PostDrawCatalyst(SpriteBatch spriteBatch, Projectile projectile, Player player, Color lightColor) { }
		public virtual bool PreAICatalyst(Projectile projectile) { return true; }
		public virtual bool PreDrawCatalyst(SpriteBatch spriteBatch, Projectile projectile, Player player, ref Color lightColor) { return true; }
		public virtual Vector2 CustomSummonMovementTarget(Projectile projectile) => Main.player[projectile.owner].Center; // Used to create a custom movement target for the summoned catalyst
		public virtual void CatalystSummonAI(Projectile projectile, int timeSpent) { } //  Used to create custom (attack) ai for the summoned catalyst
		public virtual void CatalystSummonRelease(Projectile projectile) { } // Called when the summoned catalyst is released (appears)
		public virtual void OnReleaseShamanicBond(Player player, OrchidShaman shamanPlayer) { } // Called when a bond is released, after the catlyst is summoned and everything else is handled
		public virtual void CatalystSummonKill(Projectile projectile, int timeSpent) { } // Called when the summoned catalyst is killed

		public virtual string CatalystTexture => "OrchidMod/Content/Shaman/CatalystTextures/" + this.Name + "_Catalyst";

		// ...

		public void SwitchToNextWeapon(Player player)
		{
			int selectedItem = player.selectedItem;
			int selectedItemType = player.inventory[player.selectedItem].type;

			if (player.selectedItem + 1 < 10)
			{
				for (int i = player.selectedItem + 1; i < 10; i++)
				{
					if (player.inventory[i].ModItem is OrchidModShamanItem && player.inventory[i].type != selectedItemType)
					{
						player.selectedItem = i;
						return;
					}
				}
			}

			for (int i = 0; i < player.selectedItem; i++)
			{
				if (player.inventory[i].ModItem is OrchidModShamanItem && player.inventory[i].type != selectedItemType)
				{
					player.selectedItem = i;
					return;
				}
			}
		}

		public int NewShamanProjectile(Player player, EntitySource_ItemUse source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, float ai0 = 0.0f, float ai1 = 0.0f)
		{
			int newProjectileIndex = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai0, ai1);
			Projectile newProjectile = Main.projectile[newProjectileIndex];
			OrchidModProjectile.setShamanBond(newProjectile, (int)Element);
			return newProjectileIndex;
		}

		public int NewShamanProjectileFromProjectile(Projectile projectile, Vector2 velocity, int type, int damage, float knockback, float ai0 = 0.0f, float ai1 = 0.0f)
		{
			int newProjectileIndex = Projectile.NewProjectile(projectile.GetSource_FromThis(), projectile.Center, velocity, type, damage, knockback, projectile.owner, ai0, ai1);
			Projectile newProjectile = Main.projectile[newProjectileIndex];
			OrchidModProjectile.setShamanBond(newProjectile, (int)Element);
			return newProjectileIndex;
		}
	}
}
