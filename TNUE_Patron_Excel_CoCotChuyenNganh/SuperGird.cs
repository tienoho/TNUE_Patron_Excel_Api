using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace TNUE_Patron_Excel
{
    public class SuperGird : DataGridView
    {
        public int _pageSize = 10;

        private BindingSource bs = new BindingSource();

        private BindingList<DataTable> tables = null;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        public void SetPagedDataSource(DataTable dataTable, BindingNavigator bnav)
        {
            DataTable dataTable2 = null;
            tables = new BindingList<DataTable>();
           // bnav = new BindingNavigator();
            int num = 1;
            foreach (DataRow row in dataTable.Rows)
            {
                if (num == 1)
                {
                    dataTable2 = dataTable.Clone();
                    tables.Add(dataTable2);
                }
                dataTable2.Rows.Add(row.ItemArray);
                if (PageSize < ++num)
                {
                    num = 1;
                }
            }
            bnav.BindingSource = bs;
            bs.DataSource = tables;
            bs.PositionChanged += bs_PositionChanged;
            bs_PositionChanged(bs, EventArgs.Empty);
        }

        private void bs_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                base.DataSource = tables[bs.Position];
            }
            catch
            {
            }
        }
    }
}
