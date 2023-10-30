using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_TextEditor
{
    public static class FileManage
    {
        public static string Path { get; set; }=String.Empty;
        public static void WriteFile(string content) =>File.WriteAllText(Path, content);
        public static string ReadFile() => File.ReadAllText(Path);

    }

}
