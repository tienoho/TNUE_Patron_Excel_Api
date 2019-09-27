using System.Text;

namespace TNUE_Patron_Excel.Z303
{
	internal class z305Block
	{
		public string tab5(string subLibrary, string id, string block)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<z305>");
			stringBuilder.Append("<record-action>A</record-action>");
			stringBuilder.Append("<z305-id>" + id + "</z305-id>");
			stringBuilder.Append("<z305-sub-library>" + subLibrary + "</z305-sub-library>");
			stringBuilder.Append("<z305-cash-limit>0.00</z305-cash-limit>");
			stringBuilder.Append("<z305-delinq-1>" + block + "</z305-delinq-1>");
			if (!block.Equals("05"))
			{
				stringBuilder.Append("<z305-delinq-n-1></z305-delinq-n-1>");
			}
			stringBuilder.Append("</z305>");
			return stringBuilder.ToString();
		}
	}
}
