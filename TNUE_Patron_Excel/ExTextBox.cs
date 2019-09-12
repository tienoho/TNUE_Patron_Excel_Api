using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TNUE_Patron_Excel
{
	public class ExTextBox : TextBox
	{
		private string hint;

		private Color hintColor = SystemColors.GrayText;

		[DefaultValue("")]
		public string Hint
		{
			get
			{
				return hint;
			}
			set
			{
				hint = value;
				Invalidate();
			}
		}

		public Color HintColor
		{
			get
			{
				return hintColor;
			}
			set
			{
				hintColor = value;
				Invalidate();
			}
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg == 15 && !Focused && string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(Hint))
			{
				using (Graphics dc = CreateGraphics())
				{
					TextRenderer.DrawText(dc, Hint, Font, base.ClientRectangle, HintColor, BackColor, TextFormatFlags.Default);
				}
			}
		}

		private bool ShouldSerializeHintColor()
		{
			return HintColor != SystemColors.GrayText;
		}

		private void ResetHintColor()
		{
			HintColor = SystemColors.GrayText;
		}
	}
}
