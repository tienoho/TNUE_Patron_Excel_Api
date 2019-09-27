using System.Text;
using TNUE_Patron_Excel.Tool;

namespace TNUE_Patron_Excel.Z303
{
    internal class z305
    {
        public string tab5(Patron p, string block, string status)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<z305>");
            stringBuilder.Append("<record-action>A</record-action>");
            stringBuilder.Append("<z305-id>" + p.pationID + "</z305-id>");
            stringBuilder.Append("<z305-sub-library>ALEPH</z305-sub-library>");
            stringBuilder.Append("<z305-open-date>" + p.Day + "</z305-open-date>");
            stringBuilder.Append("<z305-update-date>" + p.Day + "</z305-update-date>");

            stringBuilder.Append(new ToolP().WriteStringCheckNull(bor_type(p.makh), "z305-bor-type"));
            //stringBuilder.Append("<z305-bor-type>" + bor_type(p.makh) + "</z305-bor-type>");

            stringBuilder.Append("<z305-bor-status>" + status + "</z305-bor-status>");
            stringBuilder.Append("<z305-registration-date>" + p.Day + "</z305-registration-date>");
            stringBuilder.Append("<z305-expiry-date>" + p.ngayHetHan + "</z305-expiry-date>");
            stringBuilder.Append("<z305-note></z305-note>");
            stringBuilder.Append("<z305-loan-permission>Y</z305-loan-permission>");
            stringBuilder.Append("<z305-photo-permission>Y</z305-photo-permission>");
            stringBuilder.Append("<z305-over-permission>Y</z305-over-permission>");
            stringBuilder.Append("<z305-multi-hold>Y</z305-multi-hold>");
            stringBuilder.Append("<z305-loan-check>Y</z305-loan-check>");
            stringBuilder.Append("<z305-hold-permission>Y</z305-hold-permission>");
            stringBuilder.Append("<z305-renew-permission>Y</z305-renew-permission>");
            stringBuilder.Append("<z305-rr-permission>Y</z305-rr-permission>");
            stringBuilder.Append("<z305-ignore-late-return>N</z305-ignore-late-return>");
            stringBuilder.Append("<z305-last-activity-date>00000000</z305-last-activity-date>");
            stringBuilder.Append("<z305-photo-charge>F</z305-photo-charge>");
            stringBuilder.Append("<z305-no-loan>0000</z305-no-loan>");
            stringBuilder.Append("<z305-no-hold>0000</z305-no-hold>");
            stringBuilder.Append("<z305-no-photo>0000</z305-no-photo>");
            stringBuilder.Append("<z305-no-cash>0000</z305-no-cash>");
            stringBuilder.Append("<z305-cash-limit></z305-cash-limit>");
            stringBuilder.Append("<z305-credit-debit></z305-credit-debit>");
            stringBuilder.Append("<z305-sum>0.00</z305-sum>");
            stringBuilder.Append("<z305-delinq-1>00</z305-delinq-1>");
            stringBuilder.Append("<z305-delinq-n-1></z305-delinq-n-1>");
            stringBuilder.Append("<z305-delinq-1-update-date></z305-delinq-1-update-date>");
            stringBuilder.Append("<z305-delinq-1-cat-name>MASTER</z305-delinq-1-cat-name>");
            stringBuilder.Append("<z305-delinq-2>00</z305-delinq-2>");
            stringBuilder.Append("<z305-delinq-n-2></z305-delinq-n-2>");
            stringBuilder.Append("<z305-delinq-2-update-date>" + p.Day + "</z305-delinq-2-update-date>");
            stringBuilder.Append("<z305-delinq-2-cat-name>MASTER</z305-delinq-2-cat-name>");
            stringBuilder.Append("<z305-delinq-3>00</z305-delinq-3>");
            stringBuilder.Append("<z305-delinq-n-3></z305-delinq-n-3>");
            stringBuilder.Append("<z305-delinq-3-update-date>" + p.Day + "</z305-delinq-3-update-date>");
            stringBuilder.Append("<z305-delinq-3-cat-name>MASTER</z305-delinq-3-cat-name>");

            //stringBuilder.Append("<z305-field-1>" + p.hocBong + "</z305-field-1>");
            //stringBuilder.Append("<z305-field-2>" + p.qdCongNhan + "</z305-field-2>");
            //stringBuilder.Append("<z305-field-3>" + p.ChuyenNganh + "</z305-field-3>");
            stringBuilder.Append(new ToolP().WriteStringCheckNull(p.hocBong, "z305-field-1"));
            stringBuilder.Append(new ToolP().WriteStringCheckNull(p.qdCongNhan, "z305-field-2"));
            stringBuilder.Append(new ToolP().WriteStringCheckNull(p.ChuyenNganh, "z305-field-3"));

            stringBuilder.Append("<z305-hold-on-shelf>Y</z305-hold-on-shelf>");
            stringBuilder.Append("<z305-end-block-date>00000000</z305-end-block-date>");
            stringBuilder.Append("<z305-booking-permission>Y</z305-booking-permission>");
            stringBuilder.Append("<z305-booking-ignore-hours>N</z305-booking-ignore-hours>");
            stringBuilder.Append("<z305-rush-cat-request>Y</z305-rush-cat-request>");
            stringBuilder.Append("</z305>");
            stringBuilder.Append("<z305>");
            stringBuilder.Append("<record-action>A</record-action>");
            stringBuilder.Append("<z305-id>" + p.pationID + "</z305-id>");
            stringBuilder.Append("<z305-sub-library>LSP50</z305-sub-library>");
            stringBuilder.Append("<z305-open-date>" + p.Day + "</z305-open-date>");
            stringBuilder.Append("<z305-update-date>" + p.Day + "</z305-update-date>");
            stringBuilder.Append(new ToolP().WriteStringCheckNull(bor_type(p.makh), "z305-bor-type"));
            //stringBuilder.Append("<z305-bor-type>" + bor_type(p.makh) + "</z305-bor-type>");
            stringBuilder.Append("<z305-bor-status>" + status + "</z305-bor-status>");
            stringBuilder.Append("<z305-registration-date>" + p.Day + "</z305-registration-date>");
            stringBuilder.Append("<z305-expiry-date>" + p.ngayHetHan + "</z305-expiry-date>");
            stringBuilder.Append("<z305-note></z305-note>");
            stringBuilder.Append("<z305-loan-permission>Y</z305-loan-permission>");
            stringBuilder.Append("<z305-photo-permission>Y</z305-photo-permission>");
            stringBuilder.Append("<z305-over-permission>Y</z305-over-permission>");
            stringBuilder.Append("<z305-multi-hold>Y</z305-multi-hold>");
            stringBuilder.Append("<z305-loan-check>Y</z305-loan-check>");
            stringBuilder.Append("<z305-hold-permission>Y</z305-hold-permission>");
            stringBuilder.Append("<z305-renew-permission>Y</z305-renew-permission>");
            stringBuilder.Append("<z305-rr-permission>Y</z305-rr-permission>");
            stringBuilder.Append("<z305-ignore-late-return>N</z305-ignore-late-return>");
            stringBuilder.Append("<z305-last-activity-date>00000000</z305-last-activity-date>");
            stringBuilder.Append("<z305-photo-charge>F</z305-photo-charge>");
            stringBuilder.Append("<z305-no-loan>0000</z305-no-loan>");
            stringBuilder.Append("<z305-no-hold>0000</z305-no-hold>");
            stringBuilder.Append("<z305-no-photo>0000</z305-no-photo>");
            stringBuilder.Append("<z305-no-cash>0000</z305-no-cash>");
            stringBuilder.Append("<z305-cash-limit></z305-cash-limit>");
            stringBuilder.Append("<z305-credit-debit></z305-credit-debit>");
            stringBuilder.Append("<z305-sum>0.00</z305-sum>");
            stringBuilder.Append("<z305-delinq-1>00</z305-delinq-1>");
            stringBuilder.Append("<z305-delinq-n-1></z305-delinq-n-1>");
            stringBuilder.Append("<z305-delinq-1-update-date></z305-delinq-1-update-date>");
            stringBuilder.Append("<z305-delinq-1-cat-name>MASTER</z305-delinq-1-cat-name>");
            stringBuilder.Append("<z305-delinq-2>00</z305-delinq-2>");
            stringBuilder.Append("<z305-delinq-n-2></z305-delinq-n-2>");
            stringBuilder.Append("<z305-delinq-2-update-date>" + p.Day + "</z305-delinq-2-update-date>");
            stringBuilder.Append("<z305-delinq-2-cat-name>MASTER</z305-delinq-2-cat-name>");
            stringBuilder.Append("<z305-delinq-3>00</z305-delinq-3>");
            stringBuilder.Append("<z305-delinq-n-3></z305-delinq-n-3>");
            stringBuilder.Append("<z305-delinq-3-update-date>" + p.Day + "</z305-delinq-3-update-date>");
            stringBuilder.Append("<z305-delinq-3-cat-name>MASTER</z305-delinq-3-cat-name>");
            //stringBuilder.Append("<z305-field-1>" + p.hocBong + "</z305-field-1>");
            //stringBuilder.Append("<z305-field-2>" + p.qdCongNhan + "</z305-field-2>");
            //stringBuilder.Append("<z305-field-3>" + p.ChuyenNganh + "</z305-field-3>");

            stringBuilder.Append(new ToolP().WriteStringCheckNull(p.hocBong, "z305-field-1"));
            stringBuilder.Append(new ToolP().WriteStringCheckNull(p.qdCongNhan, "z305-field-2"));
            stringBuilder.Append(new ToolP().WriteStringCheckNull(p.ChuyenNganh, "z305-field-3"));

            stringBuilder.Append("<z305-hold-on-shelf>Y</z305-hold-on-shelf>");
            stringBuilder.Append("<z305-end-block-date>00000000</z305-end-block-date>");
            stringBuilder.Append("<z305-booking-permission>Y</z305-booking-permission>");
            stringBuilder.Append("<z305-booking-ignore-hours>N</z305-booking-ignore-hours>");
            stringBuilder.Append("<z305-rush-cat-request>Y</z305-rush-cat-request>");
            stringBuilder.Append("</z305>");
            stringBuilder.Append("<z305>");
            stringBuilder.Append("<record-action>A</record-action>");
            stringBuilder.Append("<z305-id>" + p.pationID + "</z305-id>");
            stringBuilder.Append("<z305-sub-library>LSP</z305-sub-library>");
            stringBuilder.Append("<z305-open-date>" + p.Day + "</z305-open-date>");
            stringBuilder.Append("<z305-update-date>" + p.Day + "</z305-update-date>");
            stringBuilder.Append(new ToolP().WriteStringCheckNull(bor_type(p.makh), "z305-bor-type"));
            //stringBuilder.Append("<z305-bor-type>" + bor_type(p.makh) + "</z305-bor-type>");
            stringBuilder.Append("<z305-bor-status>" + status + "</z305-bor-status>");
            stringBuilder.Append("<z305-registration-date>" + p.Day + "</z305-registration-date>");
            stringBuilder.Append("<z305-expiry-date>" + p.ngayHetHan + "</z305-expiry-date>");
            stringBuilder.Append("<z305-note></z305-note>");
            stringBuilder.Append("<z305-loan-permission>Y</z305-loan-permission>");
            stringBuilder.Append("<z305-photo-permission>Y</z305-photo-permission>");
            stringBuilder.Append("<z305-over-permission>Y</z305-over-permission>");
            stringBuilder.Append("<z305-multi-hold>Y</z305-multi-hold>");
            stringBuilder.Append("<z305-loan-check>Y</z305-loan-check>");
            stringBuilder.Append("<z305-hold-permission>Y</z305-hold-permission>");
            stringBuilder.Append("<z305-renew-permission>Y</z305-renew-permission>");
            stringBuilder.Append("<z305-rr-permission>Y</z305-rr-permission>");
            stringBuilder.Append("<z305-ignore-late-return>N</z305-ignore-late-return>");
            stringBuilder.Append("<z305-last-activity-date>00000000</z305-last-activity-date>");
            stringBuilder.Append("<z305-photo-charge>F</z305-photo-charge>");
            stringBuilder.Append("<z305-no-loan>0000</z305-no-loan>");
            stringBuilder.Append("<z305-no-hold>0000</z305-no-hold>");
            stringBuilder.Append("<z305-no-photo>0000</z305-no-photo>");
            stringBuilder.Append("<z305-no-cash>0000</z305-no-cash>");
            stringBuilder.Append("<z305-cash-limit></z305-cash-limit>");
            stringBuilder.Append("<z305-credit-debit></z305-credit-debit>");
            stringBuilder.Append("<z305-sum>0.00</z305-sum>");
            stringBuilder.Append("<z305-delinq-1>" + block + "</z305-delinq-1>");
            stringBuilder.Append("<z305-delinq-n-1></z305-delinq-n-1>");
            stringBuilder.Append("<z305-delinq-1-update-date></z305-delinq-1-update-date>");
            stringBuilder.Append("<z305-delinq-1-cat-name>MASTER</z305-delinq-1-cat-name>");
            stringBuilder.Append("<z305-delinq-2>00</z305-delinq-2>");
            stringBuilder.Append("<z305-delinq-n-2></z305-delinq-n-2>");
            stringBuilder.Append("<z305-delinq-2-update-date>" + p.Day + "</z305-delinq-2-update-date>");
            stringBuilder.Append("<z305-delinq-2-cat-name>MASTER</z305-delinq-2-cat-name>");
            stringBuilder.Append("<z305-delinq-3>00</z305-delinq-3>");
            stringBuilder.Append("<z305-delinq-n-3></z305-delinq-n-3>");
            stringBuilder.Append("<z305-delinq-3-update-date>" + p.Day + "</z305-delinq-3-update-date>");
            stringBuilder.Append("<z305-delinq-3-cat-name>MASTER</z305-delinq-3-cat-name>");

            stringBuilder.Append(new ToolP().WriteStringCheckNull(p.hocBong, "z305-field-1"));
            stringBuilder.Append(new ToolP().WriteStringCheckNull(p.qdCongNhan, "z305-field-2"));
            stringBuilder.Append(new ToolP().WriteStringCheckNull(p.ChuyenNganh, "z305-field-3"));

            //stringBuilder.Append("<z305-field-1>" + p.hocBong + "</z305-field-1>");
            //stringBuilder.Append("<z305-field-2>" + p.qdCongNhan + "</z305-field-2>");
            //stringBuilder.Append("<z305-field-3>" + p.ChuyenNganh + "</z305-field-3>");

            stringBuilder.Append("<z305-hold-on-shelf>Y</z305-hold-on-shelf>");
            stringBuilder.Append("<z305-end-block-date>00000000</z305-end-block-date>");
            stringBuilder.Append("<z305-booking-permission>Y</z305-booking-permission>");
            stringBuilder.Append("<z305-booking-ignore-hours>N</z305-booking-ignore-hours>");
            stringBuilder.Append("<z305-rush-cat-request>Y</z305-rush-cat-request>");
            stringBuilder.Append("</z305>");
            return stringBuilder.ToString();
        }

        public string bor_type(string str)
        {
            if (str != null || str != "")
            {
                string text = str.Trim();
                string text2 = text;
                if (text2 != null && text2 == "Phòng CNTT-TV")
                {
                    return "TV";
                }
                return "";
            }
            return str;
        }
        private string WriteStringCheckNull(string str, string field)
        {
            if (str != null && str != "" && !str.Equals(""))
            {
                return "<" + field + ">" + str + "</" + field + ">";
            }
            else
            {
                return "";
            }
        }
    }
}
