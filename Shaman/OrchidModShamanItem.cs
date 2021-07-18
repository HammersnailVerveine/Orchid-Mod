using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Shaman
{
	public abstract class OrchidModShamanItem : OrchidModItem
	{
		public int empowermentType = 0;
		
		public virtual void SafeSetDefaults() {}
		public virtual void SafeHoldItem() {}
		
		public virtual bool SafeShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
			return true;
		}

		public sealed override void SetDefaults() {
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
			item.noMelee = true;
			item.noUseGraphic = true;
			Item.staff[item.type] = true;
			item.crit = 4;
			item.useStyle = 3;
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeapon = true;
			SafeSetDefaults();
		}
		
		public sealed override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			modPlayer.shamanDrawWeapon = item.useTime;
			Vector2 mousePosition = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
			
			Vector2 catalystCenter = modPlayer.shamanCatalystPosition + new Vector2(modPlayer.shamanCatalystTexture.Width / 2, modPlayer.shamanCatalystTexture.Height / 2);
			
			if (Collision.CanHit(position, 0, 0, position + (catalystCenter - position), 0, 0)) {
				position = catalystCenter;
			}
			
			Vector2 newMove = mousePosition - position;
			newMove.Normalize();
			newMove *= new Vector2(speedX, speedY).Length();
			speedX = newMove.X;
			speedY = newMove.Y;
			
			return SafeShoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}
		
		public sealed override void HoldItem(Player player) {
			OrchidModPlayer modPlayer = player.GetModPlayer<OrchidModPlayer>();
			if (modPlayer.shamanCatalyst < 1) {
				int projType = ProjectileType<CatalystAnchor>();
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, projType, 0, 0f, player.whoAmI);
			}
			
			if (modPlayer.shamanSelectedItem != item.type) {
				modPlayer.shamanSelectedItem = item.type;
				string textureLocation = "OrchidMod/Shaman/CatalystTextures/" + this.Name + "_Catalyst";
				modPlayer.shamanCatalystTexture = ModContent.GetTexture(textureLocation);
			}
			
			modPlayer.shamanCatalyst = 3;
			SafeHoldItem();
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;
		}
		
		public override void GetWeaponCrit(Player player, ref int crit)
		{
			crit += player.GetModPlayer<OrchidModPlayer>().shamanCrit;
		}
		
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (Main.rand.Next(101) <= ((OrchidModPlayer)player.GetModPlayer(mod, "OrchidModPlayer")).shamanCrit) crit = true;
			else crit = false;
		}
		
		public override bool CloneNewInstances => true;


		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null) {
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " shamanic damage";
			}
			
			Mod thoriumMod = OrchidMod.ThoriumMod;
			if (thoriumMod != null)
			{
				int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("ItemName"));
				if (index != -1)
				{
					tooltips.Insert(index + 1, new TooltipLine(mod, "ShamanTag", "-Shaman Class-") // 00C0FF
					{
						overrideColor = new Color(0, 192, 255)
					});
				}
			}
			
			if (empowermentType > 0)
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

				int index = tooltips.FindIndex(ttip => ttip.mod.Equals("Terraria") && ttip.Name.Equals("Knockback"));
				if (index != -1) tooltips.Insert(index + 1, new TooltipLine(mod, "BondType", $"Bond type: [c/{Terraria.ID.Colors.AlphaDarken(colors[empowermentType - 1]).Hex3()}:{strType[empowermentType - 1]}]"));
			}
		}
	}
}
