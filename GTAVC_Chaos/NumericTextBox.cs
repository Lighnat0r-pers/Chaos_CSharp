using System;
using System.Windows.Forms;

namespace GTAVC_Chaos
{
    public class NumericTextBox : TextBox
    {
        private bool allowSpace = false;
        private bool allowDecimalSeparator = false;
        private bool allowNumberGroupSeparator = false;
        private bool allowNegativeSign = false;

        // Restricts the entry of characters to digits (including hex), the negative sign,
        // the decimal point, and editing keystrokes (backspace).
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            var numberFormatInfo = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
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
                // Swallow this invalid key
                e.Handled = true;
            }
        }

        public int IntValue
        {
            get { return Int32.Parse(this.Text); }
        }

        public decimal DecimalValue
        {
            get { return Decimal.Parse(this.Text); }
        }

        public bool AllowSpace
        {
            get { return this.allowSpace; }
            set { this.allowSpace = value; }
        }
        public bool AllowDecimalSeparator
        {
            get { return this.allowDecimalSeparator; }
            set { this.allowDecimalSeparator = value; }
        }
        public bool AllowNumberGroupSeparator
        {
            get { return this.allowNumberGroupSeparator; }
            set { this.allowNumberGroupSeparator = value; }
        }
        public bool AllowNegativeSign
        {
            get { return this.allowNegativeSign; }
            set { this.allowNegativeSign = value; }
        }
    }
}
