using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_TextEditor
{
    public static  class Notifications
    {
        public static string NotifTextSavedSuccessfully(string fileName)
        {
            return $"Text saved successfully to {fileName}.";
        }

        public static string NotifAutoSaveIsChecked(bool isChecked)
        {
            if (isChecked)  return $"Auto-Save has been turned ON.";
            return $"Auto-save has been turned OFF.";
        }
    }
}
