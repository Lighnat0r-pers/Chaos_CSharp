using System;
using System.Windows.Forms;
using System.Globalization;

namespace GTAVC_Chaos
{
    public class NumericTextBox : TextBox
    {
        bool allowSpace = false;
        bool allowDecimalSeparator = false;
        bool allowNumberGroupSeparator = false;
        bool allowNegativeSign = false;

        // Restricts the entry of characters to digits (including hex), the negative sign,
        // the decimal point, and editing keystrokes (backspace).
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string groupSeparator = numberFormatInfo.NumberGroupSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;

            string keyInput = e.KeyChar.ToString();

            if (Char.IsDigit(e.KeyChar))
            {
                // Digits are OK
            }
            else if (e.KeyChar == '\b')
            {
                // Backspace key is OK
            }
            //    else if ((ModifierKeys & (Keys.Control | Keys.Alt)) != 0)
            //    {
            //     // Let the edit control handle control and alt key combinations
            //    }
            else if (this.allowDecimalSeparator && keyInput.Equals(decimalSeparator))
            {
                // Decimal separator is allowed when the option for it is set to true.
            }
            else if (this.allowNumberGroupSeparator && keyInput.Equals(groupSeparator))
            {
                // Number group separator is allowed when the option for it is set to true.
            }
            else if (this.allowNegativeSign && keyInput.Equals(negativeSign))
            {
                // Negative sign is allowed when the option for it is set to true.
            }
            else if (this.allowSpace && e.KeyChar == ' ')
            {
                // Space is allowed when the option for it is set to true.
            }
            else
            {
                // Swallow this invalid key and beep
                e.Handled = true;
                //    MessageBeep();
            }
        }

        public int IntValue
        {
            get
            {
                return Int32.Parse(this.Text);
            }
        }

        public decimal DecimalValue
        {
            get
            {
                return Decimal.Parse(this.Text);
            }
        }

        public bool AllowSpace
        {
            set
            {
                this.allowSpace = value;
            }

            get
            {
                return this.allowSpace;
            }
        }
        public bool AllowDecimalSeparator
        {
            set
            {
                this.allowDecimalSeparator = value;
            }

            get
            {
                return this.allowDecimalSeparator;
            }
        }
        public bool AllowNumberGroupSeparator
        {
            set
            {
                this.allowNumberGroupSeparator = value;
            }

            get
            {
                return this.allowNumberGroupSeparator;
            }
        }
        public bool AllowNegativeSign
        {
            set
            {
                this.allowNegativeSign = value;
            }

            get
            {
                return this.allowNegativeSign;
            }
        }
    }
}
