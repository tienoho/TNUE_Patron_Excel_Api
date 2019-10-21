using System.ComponentModel;

namespace TNUE_Patron_Excel.Tool
{
    public class Z308
    {
        public string Z308_REC_KEY { get; set; }

        public string Z308_VERIFICATION { get; set; }

        public string Z308_VERIFICATION_TYPE { get; set; }

        public string Z308_ID { get; set; }

        public string Z308_STATUS { get; set; }

        public string Z308_ENCRYPTION { get; set; }

        public string Z308_UPD_TIME_STAMP { get; set; }
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
