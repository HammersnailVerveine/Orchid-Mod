using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchidMod.Common.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class ClassTagAttribute : Attribute
	{
		public readonly ClassTags Tag;

		public ClassTagAttribute(ClassTags tag)
		{
			Tag = tag;
		}
	}
}