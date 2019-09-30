using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNUE_Patron_Excel.Config;

namespace TNUE_Patron_Excel.Tool
{
    public class FormProvider
    {
        private static ConfigLdap _configLdap;
        public static ConfigLdap sConfigLdap
        {
            get
            {
                if (_configLdap == null)
                {
                    _configLdap = new ConfigLdap();
                }
                return _configLdap;
            }
        }
        private static ConfigAleph _configAleph;
        public static ConfigAleph sConfigAleph
        {
            get
            {
                if (_configAleph == null)
                {
                    _configAleph = new ConfigAleph();
                }
                return _configAleph;
            }
        }
        private static ConfigDataBase _configDataBase;
        public static ConfigDataBase sConfigDataBase
        {
            get
            {
                if (_configDataBase == null)
                {
                    _configDataBase = new ConfigDataBase();
                }
                return _configDataBase;
            }
        }
    }
}
