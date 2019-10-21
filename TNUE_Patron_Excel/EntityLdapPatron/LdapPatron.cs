using System.ComponentModel;

namespace TNUE_Patron_Excel.EntityLdapPatron
{
    internal class LdapPatron
    {
        [DisplayName("Mã")]
        public string userLogin { get; set; }
        [DisplayName("Mật khẩu")]
        public string userPassword { get; set; }
        [DisplayName("Email")]
        public string userMail { get; set; }
        [DisplayName("Số điện thoại")]
        public string telephoneNumber { get; set; }
        [DisplayName("Họ Tên")]
        public string HoTen { get; set; }
        [DisplayName("Ngày sinh")]
        public string NgaySinh { get; set; }
        [DisplayName("Lớp")]
        public string Lop { get; set; }
        [DisplayName("Khoa")]
        public string KhoaNganh { get; set; }
        [DisplayName("Khóa học")]
        public string KhoaHoc { get; set; }
    }
}
