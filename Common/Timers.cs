using System;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Common
{
	public abstract class Timer
	{
		protected abstract uint GlobalCounter { get; }

		public bool Active
			=> GlobalCounter < endTime;

		public int UnclampedValue
			=> (int)((long)endTime - GlobalCounter);

		public uint Value
		{
			get => (uint)Math.Max(0, UnclampedValue);
			set
			{
				endTime = GlobalCounter + Math.Max(0, value);
				time = value;
			}
		}

		// ...

		private uint endTime = 0;
		private uint time = 0;

		// ...

		public Timer(uint value)
			=> Value = value;

		public void Set(uint value)
			=> Value = Math.Max(value, Value);

		public void Restart()
			=> Value = time;
	}

	public class GameTimer : Timer
	{
		public GameTimer(uint value) : base(value) { }

		protected override uint GlobalCounter
			=> Main.GameUpdateCount;

		public static implicit operator GameTimer(uint value) => new(value);
	}

	public class InterfaceTimer : Timer
	{
		public InterfaceTimer(uint value) : base(value) { }

		protected override uint GlobalCounter
			=> InterfaceTimerCounter.GameUpdateCountWithoutPause;

		public static implicit operator InterfaceTimer(uint value) => new(value);

		// ...

		private class InterfaceTimerCounter : ILoadable
		{
			public static uint GameUpdateCountWithoutPause { get; set; }

			void ILoadable.Load(Mod mod)
			{
				// This method is private and is called only in DoUpdate
				// So, in theory, it shouldn't break anything
				On.Terraria.Main.UpdateWindyDayState += (orig, main) =>
				{
					orig(main);
					GameUpdateCountWithoutPause++;
				};

				WorldGen.Hooks.OnWorldLoad += ResetCounter;
			}

			void ILoadable.Unload()
				=> WorldGen.Hooks.OnWorldLoad -= ResetCounter;

			private static void ResetCounter()
				=> GameUpdateCountWithoutPause = 0u;
		}
	}
}