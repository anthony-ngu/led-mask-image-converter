using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.IO;

namespace LEDMaskImageConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage srcImage;
        WriteableBitmap resultImage;

        // Format of mask

        int width = 35;
        int height = 20;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SourceImageFileDialogButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            
            // Set filter for file extension and default file extension
            dlg.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                SourceImageFileTextBox.Text = filename;

                srcImage = new BitmapImage();
                srcImage.BeginInit();
                srcImage.UriSource = new Uri(filename, UriKind.Absolute);
                srcImage.EndInit();
                SourceImage.Source = srcImage;
            }
        }

        private void ConvertImageButton_Click(object sender, RoutedEventArgs e)
        {
            double scale = srcImage.PixelHeight / height;
            if (scale < srcImage.PixelWidth / width)
            {
                scale = srcImage.PixelWidth / width;
            }

            ScaleTransform scaleTransform = new ScaleTransform(1/scale, 1/scale);

            TransformedBitmap transformedBitmap = new TransformedBitmap(srcImage, scaleTransform);

            int stride = transformedBitmap.PixelWidth * (transformedBitmap.Format.BitsPerPixel / 8);

            // Create data array to hold source pixel data
            byte[] data = new byte[stride * transformedBitmap.PixelHeight];

            // Copy source image pixels to the data array
            transformedBitmap.CopyPixels(data, stride, 0);

            WriteableBitmap writeableBitmap = new WriteableBitmap(transformedBitmap.PixelWidth, transformedBitmap.PixelHeight, transformedBitmap.DpiX, transformedBitmap.DpiY, transformedBitmap.Format, null);
            writeableBitmap.WritePixels(new Int32Rect(0, 0, transformedBitmap.PixelWidth, transformedBitmap.PixelHeight), data, stride, 0);

            ResultImage.Source = writeableBitmap;
            resultImage = writeableBitmap;
        }

        private void GetCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string text = "";

            Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create((BitmapSource)resultImage));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    System.Drawing.Color c = bitmap.GetPixel(i, j);
                    text += "{";
                    text += ((int)c.R).ToString() + ',';
                    text += ((int)c.G).ToString() + ',';
                    text += ((int)c.B).ToString();
                    text += "},";
                }
                text += '\n';
            }
            CodeDisplayWindow codeWindow = new CodeDisplayWindow(text);
            codeWindow.Show();
            codeWindow.Activate();
        }
    }
}
