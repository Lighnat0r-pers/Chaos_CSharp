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
            else if (allowDecimalSeparator && keyInput.Equals(decimalSeparator))
            {
                // Decimal separator is allowed when the option for it is set to true.
            }
            else if (allowNumberGroupSeparator && keyInput.Equals(groupSeparator))
            {
                // Number group separator is allowed when the option for it is set to true.
            }
            else if (allowNegativeSign && keyInput.Equals(negativeSign))
            {
                // Negative sign is allowed when the option for it is set to true.
            }
            else if (allowSpace && e.KeyChar == ' ')
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
            get { return Int32.Parse(Text); }
        }

        public decimal DecimalValue
        {
            get { return Decimal.Parse(Text); }
        }

        public bool AllowSpace
        {
            get { return allowSpace; }
            set { allowSpace = value; }
        }
        public bool AllowDecimalSeparator
        {
            get { return allowDecimalSeparator; }
            set { allowDecimalSeparator = value; }
        }
        public bool AllowNumberGroupSeparator
        {
            get { return allowNumberGroupSeparator; }
            set { allowNumberGroupSeparator = value; }
        }
        public bool AllowNegativeSign
        {
            get { return allowNegativeSign; }
            set { allowNegativeSign = value; }
        }
    }
}
