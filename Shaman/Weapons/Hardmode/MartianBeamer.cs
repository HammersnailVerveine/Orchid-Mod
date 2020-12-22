using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace OrchidMod.Shaman.Weapons.Hardmode
{
    public class MartianBeamer : OrchidModShamanItem
    {
		public override void SafeSetDefaults()
		{
			item.damage = 70;
			item.width = 30;
			item.height = 30;
			item.useTime = 18;
			item.useAnimation = 18;
			item.knockBack = 1.15f;
			item.rare = 8;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.UseSound = SoundID.Item91;
			item.autoReuse = true;
			item.shootSpeed = 7f;
			item.shoot = mod.ProjectileType("MartianBeamerProj");
			this.empowermentType = 1;
			this.empowermentLevel = 4;
			OrchidModGlobalItem orchidItem = item.GetGlobalItem<OrchidModGlobalItem>();
			orchidItem.shamanWeaponNoUsetimeReforge = true;
		}

		public override void SetStaticDefaults()
		{
		  DisplayName.SetDefault("Martian Beamer");
		  Tooltip.SetDefault("Shoots martian homing lasers"
							+"\nWeapon speed increases with the number of active shamanic bonds");
		}
		
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat) {
			mult *= player.GetModPlayer<OrchidModPlayer>().shamanDamage;
			
			Mod thoriumMod = ModLoader.GetMod("ThoriumMod");
			if (thoriumMod != null) {
                //int thoriumCrit = player.GetModPlayer<ThoriumPlayer>().allCrit; // Impossible : can't add [using ThoriumMod;] because I don't have the ThoriumMod.dll file
				ModPlayer thoriumPlayer = player.GetModPlayer(thoriumMod, "ThoriumPlayer");
				FieldInfo field = thoriumPlayer.GetType().GetField("martianDamage", BindingFlags.Public | BindingFlags.Instance);
				float martianDamage = (float)field.GetValue(thoriumPlayer);
				mult *= martianDamage;
            }
		}
		
		public override void UpdateInventory(Player player) {	
			int BuffsCount = Main.player[Main.myPlayer].GetModPlayer<OrchidModPlayer>().getNbShamanicBonds();
			
			item.useTime = 18 - (2 * BuffsCount);
			item.useAnimation = 18 - (2 * BuffsCount);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 64f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			
			for(int i = 0; i < Main.rand.Next(1, 1); i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				
				perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(-20));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
    }
}