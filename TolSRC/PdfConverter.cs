using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageMagick;
using System.IO;

namespace TolSRC
{
    public class PdfConverter
    {
        public string succsessMessage;
        public string pdfAPIsPath = @"c:\pdfAPIs";

        // Settings the density to 300 dpi will create an image with a better quality

        public string PdfToImage(string pdfPath)
        {
            return PdfToImage(pdfPath, "", "", SortOrder.DES);
        }

        public string PdfToImage(string pdfPath, string destination)
        {
            return PdfToImage(pdfPath, destination, "", SortOrder.DES);
        }

        public string PdfToImage(string pdfPath, string destination, string namePattarn)
        {
            return PdfToImage(pdfPath, destination, namePattarn, SortOrder.DES);
        }

        public string PdfToImage(string pdfPath, string destination = "", string namePattarn = "", SortOrder sortOrder = SortOrder.DES)
        {
            try
            {

                
                MagickNET.SetGhostscriptDirectory(pdfAPIsPath);
                MagickReadSettings settings = new MagickReadSettings();
                settings.Density = new Density(300, 300);
                settings.Format = MagickFormat.Pdf;
                
                //Document document = new Document(file);
                //RenderingSettings settings = new RenderingSettings();

                string fileName = "image-from-pdf";
                string imagesDestination = Path.GetDirectoryName(pdfPath) + @"\Done";
                if (!string.IsNullOrEmpty(namePattarn))
                {
                    fileName = namePattarn;
                }
                if (!string.IsNullOrEmpty(destination))
                {
                    imagesDestination = destination;
                }
                using (MagickImageCollection images = new MagickImageCollection())
                {
                    // Add all the pages of the pdf file to the collection
                    images.Read(pdfPath, settings);
                    int i = 0;
                    if (sortOrder == SortOrder.DES)
                        i = images.Count - 1;
                    if (!Directory.Exists(imagesDestination))
                    {
                        Directory.CreateDirectory(imagesDestination);
                    }
                    for (; ; )
                    {
                        MagickImage image = (MagickImage)images[i];
                        //Page currentPage = document.Pages[i];
                        //using (Bitmap bitmap = currentPage.Render((int)currentPage.Width, (int)currentPage.Height, settings))
                        //{
                        //    //bitmap.Save(Path.Combine(imagesDestination, fileName + i + ".png"));
                        //    bitmap.Save(string.Format("{0}.png", @"" + imagesDestination + @"\" + fileName.Replace(" ", "-") + i), ImageFormat.Jpeg);


                        //}

                        // Write page to file that contains the page number
                        //image.Write("Snakeware.Page" + page + ".png");
                        // Writing to a specific format works the same as for a single image
                        string fixedFileName = fileName;
                        if(i < 10)
                        {
                            fixedFileName += "00";
                        }
                        else if (i < 100)
                        {
                            fixedFileName += "0";
                        }
                        string imageName = @"" + imagesDestination + @"\" + fixedFileName.Replace(" ", "-") + i + ".jpg";
                        image.Write(imageName);
                        if (sortOrder == SortOrder.DES)
                        {
                            i--;
                            if (i < 0)
                                break;
                        }
                        else
                        {
                            i++;
                            if (i >= images.Count)
                                break;
                        }


                    }
                }
                return succsessMessage;
            }

            catch (Exception e)
            {
                return e.ToString();


            }
        }






    }
}
