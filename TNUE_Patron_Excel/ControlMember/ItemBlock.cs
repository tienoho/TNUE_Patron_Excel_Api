using System.ComponentModel;

namespace TNUE_Patron_Excel.ControlMember
{
	internal class ItemBlock
	{
        [DisplayName("PatronId")]
		public string PatronId
		{
			get;
			set;
		}
        [DisplayName("Mã")]
        public string Ma
		{
			get;
			set;
		}
        [DisplayName("Họ tên")]
        public string HoTen
		{
			get;
			set;
		}
	}
}
