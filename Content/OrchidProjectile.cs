using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Content
{
	public abstract class OrchidProjectile : ModProjectile
	{
		public Player Owner => Main.player[Projectile.owner];

		// ...

		public override string Texture => "OrchidMod/Assets/Textures/Projectiles/" + this.Name;

		// ...

		public virtual void OnSpawn() { }
	}
}
