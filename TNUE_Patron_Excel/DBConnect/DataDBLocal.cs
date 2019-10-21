using System.Collections.Generic;
using TNUE_Patron_Excel.Tool;
using TNUE_Patron_Excel.EntityLdapPatron;
using System;

namespace TNUE_Patron_Excel.DBConnect
{
    public static class DataDBLocal
    {
        public static List<Z308> listZ308 { get; set; }
        public static List<Z303Entity> listZ303 { get; set; }
        public static string pathUserLog = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\UploadPatronLog";
    }
}
