using Microsoft.Xna.Framework;
using OrchidMod.Content.Guardian.Projectiles.Shields;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace OrchidMod.Content.Guardian.Weapons.Shields
{
	public class ThoriumLeafShield : OrchidModGuardianShield
	{

		public override void SafeSetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.rare = ItemRarityID.White;
			Item.value = Item.sellPrice(0, 0, 0, 30);
			Item.knockBack = 2.5f;
			Item.damage = 22;
			Item.useTime = 60;
			distance = 30f;
			slamDistance = 120f;
			blockDuration = 80;
			slamAutoReuse = false;
			shouldFlip = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddIngredient(ItemID.Wood, 10);
			recipe.Register();
		}

		private void LeafBurst(Player player, Projectile shield, int time, int spinTime = 30, int quantity = 3)
		{
            Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Grass, shield.position);
			float dir = (Main.MouseWorld - player.Center).ToRotation();
			Vector2 spread = new Vector2(0, 1).RotatedBy(dir);
			for (int i = 0; i < quantity; i++)
			{
				float side = (i - ((quantity - 1) / 2f)) * -shield.direction;
				int leaf = Projectile.NewProjectile(Item.GetSource_FromThis(), shield.Center, spread.RotatedByRandom(Main.rand.NextFloat()) * side, ModContent.ProjectileType<ThoriumLeafShieldProj>(), (int)(shield.damage * 0.8f), Item.knockBack, player.whoAmI);
				Main.projectile[leaf].ai[0] = dir + (side + Main.rand.NextFloat() - 0.5f) * 0.1f;
				Main.projectile[leaf].ai[1] = spinTime;
				Main.projectile[leaf].timeLeft = time;
                int dust = Dust.NewDust(shield.Center - new Vector2(4, 4), 0, 0, DustID.Grass);
			}
		}

		public override void Protect(Player player, Projectile shield)
		{
			if (IsLocalPlayer(player))
			{
				LeafBurst(player, shield, 45, 51);
				GuardianShieldAnchor anchor = shield.ModProjectile as GuardianShieldAnchor;
				shield.ai[0] = 0;
				anchor.NeedNetUpdate = true;
			}
		}

		public override void SlamHitFirst(Player player, Projectile shield, NPC npc)
		{
			if (IsLocalPlayer(player))
			{
				LeafBurst(player, shield, 62 - (int)((player.Center - shield.Center).Length() / 120), 30);
				GuardianShieldAnchor anchor = shield.ModProjectile as GuardianShieldAnchor;
				shield.friendly = false;
				shield.ai[1] = 0;
				anchor.isSlamming = 0;
				anchor.NeedNetUpdate = true;
			}
		}
	}
}
