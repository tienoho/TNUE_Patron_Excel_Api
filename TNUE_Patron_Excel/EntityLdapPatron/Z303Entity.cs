using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNUE_Patron_Excel.EntityLdapPatron
{
    public class Z303Entity
    {
        [DisplayName("PatronId")]
        public string Z303_REC_KEY { get; set; }
        [DisplayName("Họ Tên")]
        public string Z303_NAME { get; set; }
        [DisplayName("Ngày sinh")]
        public string Z303_BIRTH_DATE { get; set; }
        [DisplayName("Lớp")]
        public string Z303_FIELD_1 { get; set; }
        [DisplayName("Khoa")]
        public string Z303_FIELD_2 { get; set; }
        [DisplayName("Khóa học")]
        public string Z303_FIELD_3 { get; set; }

    }
}
