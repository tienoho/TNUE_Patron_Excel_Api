using System;
using System.Collections.Generic;

namespace TNUE_Patron_Excel
{
	internal class DTOPatron : IComparable
	{
		public string _ma
		{
			get;
			set;
		}

		public string _hoTen
		{
			get;
			set;
		}

		public string patronID
		{
			get;
			set;
		}

		public static List<DTOPatron> listBlock
		{
			get;
			set;
		}

		public int CompareTo(object obj)
		{
			DTOPatron dTOPatron = obj as DTOPatron;
			if (string.Compare(_ma.ToString(), dTOPatron._ma.ToString(), ignoreCase: true) == 1)
			{
				return 1;
			}
			if (string.Compare(_ma.ToString(), dTOPatron._ma.ToString(), ignoreCase: true) == 0)
			{
				return 0;
			}
			return -1;
		}
	}
}
