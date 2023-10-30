using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPF_TextEditor
{
    public static  class Validation
    {
        public static bool isAnyTextSelected(TextBox txtContent) { return txtContent.SelectedText.Length > 0; }
    }
}
