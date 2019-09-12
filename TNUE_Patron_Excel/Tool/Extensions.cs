using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TNUE_Patron_Excel.Tool
{
	public static class Extensions
	{
		public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
		{
			return listToClone.Select((T item) => (T)item.Clone()).ToList();
		}

		public static T CloneObject<T>(this T obj) where T : class
		{
			if (obj == null)
			{
				return null;
			}
			MethodInfo method = obj.GetType().GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic);
			if (method != null)
			{
				return (T)method.Invoke(obj, null);
			}
			return null;
		}
	}
}
