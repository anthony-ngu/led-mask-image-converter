using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LEDMaskImageConverter
{
    /// <summary>
    /// Interaction logic for CodeDisplayWindow.xaml
    /// </summary>
    public partial class CodeDisplayWindow : Window
    {
        public string codeText;

        public CodeDisplayWindow(string text)
        {
            InitializeComponent();

            codeText = text;
            richTextBox.Document.Blocks.Clear();
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(codeText)));
        }
    }
}
