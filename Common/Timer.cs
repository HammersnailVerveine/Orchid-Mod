using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace OrchidMod.Common
{
	public struct Timer
	{
		public bool Active
			=> getGlobalCountFunc() < endTime;

		public int UnclampedValue
			=> (int)((long)endTime - getGlobalCountFunc());

		public uint Value
		{
			get => (uint)Math.Max(0, UnclampedValue);
			set
			{
				this.endTime = getGlobalCountFunc() + Math.Max(0, value);
				this.value = value;	
			}
		}

		// ...

		private readonly Func<uint> getGlobalCountFunc = GetFuncByType(TimerType.Game);
		private uint endTime = 0;
		private uint value = 0;

		// ...

		public Timer()
		{
			getGlobalCountFunc = GetFuncByType(TimerType.Game);
			Value = 0;
		}

		public Timer(uint value)
		{
			getGlobalCountFunc = GetFuncByType(TimerType.Game);
			Value = value;
		}

		public Timer(uint value, TimerType type)
		{
			getGlobalCountFunc = GetFuncByType(type);
			Value = value;
		}

		public void Set(uint value)
			=> Value = Math.Max(value, Value);

		public void Restart()
			=> Value = value;

		// ...

		private static readonly Func<uint> GetGameUpdateCountFunc = () => Main.GameUpdateCount;
		private static readonly Func<uint> GetInterfaceUpdateCountFunc = () => GameUpdateCountWithoutPause.Value;
		
		private static Func<uint> GetFuncByType(TimerType type)
		{
			return type switch
			{
				TimerType.Game => GetGameUpdateCountFunc,
				TimerType.Interface => GetInterfaceUpdateCountFunc,
				_ => throw new NotImplementedException(),
			};
		}

		// ...

		private class GameUpdateCountWithoutPause : ILoadable
		{
			public static uint Value { get; private set; }

			void ILoadable.Load(Mod mod)
			{
				// This method is private and is called only in DoUpdate
				// So, in theory, it shouldn't break anything
				On.Terraria.Main.UpdateWindyDayState += (orig, main) =>
				{
					orig(main);
					Value++;
				};

				WorldGen.Hooks.OnWorldLoad += ResetCounter;
			}

			void ILoadable.Unload()
				=> WorldGen.Hooks.OnWorldLoad -= ResetCounter;

			private static void ResetCounter()
				=> Value = 0u;
		}
	}
}