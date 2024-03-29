namespace TNUE_Patron_Excel.Tool
{
	internal class Unicode
	{
		public static string compound2Unicode(string str)
		{
			str = str.Replace("e\u0309", "ẻ");
			str = str.Replace("e\u0301", "é");
			str = str.Replace("e\u0300", "è");
			str = str.Replace("e\u0323", "ẹ");
			str = str.Replace("e\u0303", "ẽ");
			str = str.Replace("ê\u0309", "ể");
			str = str.Replace("ê\u0301", "ế");
			str = str.Replace("ê\u0300", "ề");
			str = str.Replace("ê\u0323", "ệ");
			str = str.Replace("ê\u0303", "ễ");
			str = str.Replace("y\u0309", "ỷ");
			str = str.Replace("y\u0301", "ý");
			str = str.Replace("y\u0300", "ỳ");
			str = str.Replace("y\u0323", "ỵ");
			str = str.Replace("y\u0303", "ỹ");
			str = str.Replace("u\u0309", "ủ");
			str = str.Replace("u\u0301", "ú");
			str = str.Replace("u\u0300", "ù");
			str = str.Replace("u\u0323", "ụ");
			str = str.Replace("u\u0303", "ũ");
			str = str.Replace("ư\u0309", "ử");
			str = str.Replace("ư\u0301", "ứ");
			str = str.Replace("ư\u0300", "ừ");
			str = str.Replace("ư\u0323", "ự");
			str = str.Replace("ư\u0303", "ữ");
			str = str.Replace("i\u0309", "ỉ");
			str = str.Replace("i\u0301", "í");
			str = str.Replace("i\u0300", "ì");
			str = str.Replace("i\u0323", "ị");
			str = str.Replace("i\u0303", "ĩ");
			str = str.Replace("o\u0309", "ỏ");
			str = str.Replace("o\u0301", "ó");
			str = str.Replace("o\u0300", "ò");
			str = str.Replace("o\u0323", "ọ");
			str = str.Replace("o\u0303", "õ");
			str = str.Replace("ơ\u0309", "ở");
			str = str.Replace("ơ\u0301", "ớ");
			str = str.Replace("ơ\u0300", "ờ");
			str = str.Replace("ơ\u0323", "ợ");
			str = str.Replace("ơ\u0303", "ỡ");
			str = str.Replace("ô\u0309", "ổ");
			str = str.Replace("ô\u0301", "ố");
			str = str.Replace("ô\u0300", "ồ");
			str = str.Replace("ô\u0323", "ộ");
			str = str.Replace("ô\u0303", "ỗ");
			str = str.Replace("a\u0309", "ả");
			str = str.Replace("a\u0301", "á");
			str = str.Replace("a\u0300", "à");
			str = str.Replace("a\u0323", "ạ");
			str = str.Replace("a\u0303", "ã");
			str = str.Replace("ă\u0309", "ẳ");
			str = str.Replace("ă\u0301", "ắ");
			str = str.Replace("ă\u0300", "ằ");
			str = str.Replace("ă\u0323", "ặ");
			str = str.Replace("ă\u0303", "ẵ");
			str = str.Replace("â\u0309", "ẩ");
			str = str.Replace("â\u0301", "ấ");
			str = str.Replace("â\u0300", "ầ");
			str = str.Replace("â\u0323", "ậ");
			str = str.Replace("â\u0303", "ẫ");
			str = str.Replace("E\u0309", "Ẻ");
			str = str.Replace("E\u0301", "É");
			str = str.Replace("E\u0300", "È");
			str = str.Replace("E\u0323", "Ẹ");
			str = str.Replace("E\u0303", "Ẽ");
			str = str.Replace("Ê\u0309", "Ể");
			str = str.Replace("Ê\u0301", "Ế");
			str = str.Replace("Ê\u0300", "Ề");
			str = str.Replace("Ê\u0323", "Ệ");
			str = str.Replace("Ê\u0303", "Ễ");
			str = str.Replace("Y\u0309", "Ỷ");
			str = str.Replace("Y\u0301", "Ý");
			str = str.Replace("Y\u0300", "Ỳ");
			str = str.Replace("Y\u0323", "Ỵ");
			str = str.Replace("Y\u0303", "Ỹ");
			str = str.Replace("U\u0309", "Ủ");
			str = str.Replace("U\u0301", "Ú");
			str = str.Replace("U\u0300", "Ù");
			str = str.Replace("U\u0323", "Ụ");
			str = str.Replace("U\u0303", "Ũ");
			str = str.Replace("Ư\u0309", "Ử");
			str = str.Replace("Ư\u0301", "Ứ");
			str = str.Replace("Ư\u0300", "Ừ");
			str = str.Replace("Ư\u0323", "Ự");
			str = str.Replace("Ư\u0303", "Ữ");
			str = str.Replace("I\u0309", "Ỉ");
			str = str.Replace("I\u0301", "Í");
			str = str.Replace("I\u0300", "Ì");
			str = str.Replace("I\u0323", "Ị");
			str = str.Replace("I\u0303", "Ĩ");
			str = str.Replace("O\u0309", "Ỏ");
			str = str.Replace("O\u0301", "Ó");
			str = str.Replace("O\u0300", "Ò");
			str = str.Replace("O\u0323", "Ọ");
			str = str.Replace("O\u0303", "Õ");
			str = str.Replace("Ơ\u0309", "Ở");
			str = str.Replace("Ơ\u0301", "Ớ");
			str = str.Replace("Ơ\u0300", "Ờ");
			str = str.Replace("Ơ\u0323", "Ợ");
			str = str.Replace("Ơ\u0303", "Ỡ");
			str = str.Replace("Ô\u0309", "Ổ");
			str = str.Replace("Ô\u0301", "Ố");
			str = str.Replace("Ô\u0300", "Ồ");
			str = str.Replace("Ô\u0323", "Ộ");
			str = str.Replace("Ô\u0303", "Ỗ");
			str = str.Replace("A\u0309", "Ả");
			str = str.Replace("A\u0301", "Á");
			str = str.Replace("A\u0300", "À");
			str = str.Replace("A\u0323", "Ạ");
			str = str.Replace("A\u0303", "Ã");
			str = str.Replace("Ă\u0309", "Ẳ");
			str = str.Replace("Ă\u0301", "Ắ");
			str = str.Replace("Ă\u0300", "Ằ");
			str = str.Replace("Ă\u0323", "Ặ");
			str = str.Replace("Ă\u0303", "Ẵ");
			str = str.Replace("Â\u0309", "Ẩ");
			str = str.Replace("Â\u0301", "Ấ");
			str = str.Replace("Â\u0300", "Ầ");
			str = str.Replace("Â\u0323", "Ậ");
			str = str.Replace("Â\u0303", "Ẫ");
			str = str.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
			return str;
		}
	}
}
