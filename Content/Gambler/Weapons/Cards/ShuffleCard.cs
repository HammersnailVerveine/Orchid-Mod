using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace OrchidMod.Content.Gambler.Weapons.Cards
{
	public class ShuffleCard : OrchidModGamblerCard
	{
		public override void SafeSetDefaults()
		{
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Blue;
			Item.damage = 4;
			Item.knockBack = 3f;
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.shootSpeed = 10f;
			cardRequirement = 0;
		}

		public override void GamblerShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback, bool dummy = false)
		{
			OrchidGambler modPlayer = player.GetModPlayer<OrchidGambler>();
			int projType = ProjectileType<Projectiles.ShuffleCardProj>();
			float aiType = Main.rand.Next(4);
			int count = 0;
			int damageCount = damage + (int)(modPlayer.GetNbGamblerCards() * 1.2f);
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (player.whoAmI == proj.owner && proj.active && proj.ai[1] != 6f && proj.type == projType)
				{
					aiType = proj.ai[0];
					count++;
					damageCount = damage * (count + 1);
					proj.damage = damageCount;
					proj.netUpdate = true;
				}
			}
			if (count < 5)
			{
				int newProjInt = DummyProjectile(Projectile.NewProjectile(source, position, Vector2.Zero, projType, damageCount, knockback, player.whoAmI), dummy);
				Projectile newProj = Main.projectile[newProjInt];
				newProj.ai[1] = (float)(count + 1);
				newProj.ai[0] = (float)aiType;
				newProj.netUpdate = true;
				if (count == 4)
					SoundEngine.PlaySound(SoundID.Item35);
				else 
					SoundEngine.PlaySound(SoundID.Item1);
			}
			else 
				SoundEngine.PlaySound(SoundID.Item7);
		}
	}
}
