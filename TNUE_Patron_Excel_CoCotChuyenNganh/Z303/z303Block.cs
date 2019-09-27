using System.Text;

namespace TNUE_Patron_Excel.Z303
{
	internal class z303Block
	{
		public string tab3(string id)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<z303>");
			stringBuilder.Append("<match-id-type>00</match-id-type>");
			stringBuilder.Append("<match-id>" + id + "</match-id>");
			stringBuilder.Append("<record-action>A</record-action>");
			stringBuilder.Append("<z303-id>" + id + "</z303-id>");
			stringBuilder.Append("</z303>");
			return stringBuilder.ToString();
		}
	}
}
