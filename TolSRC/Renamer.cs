using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace TolSRC
{
    public class Renamer
    {

        public bool StartRenamer(string sourceFolder, string destinationFolder, SortOrder sort, string namePattern=null)
        {
            if (string.IsNullOrEmpty(destinationFolder))
            {
                destinationFolder = @"" + sourceFolder + @"\Done";
            }
            
            string[] files = Directory.EnumerateFiles(sourceFolder).OrderBy(filename => filename).ToArray();
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }
            int startCounter, endCounter;
            if (sort == SortOrder.ASD)
            {
                startCounter = files.Length - 1;
                endCounter = 0;
            }
            else
            {
                endCounter = files.Length - 1;
                startCounter = 0;
            }
            if (sort == SortOrder.ASD)
            {
                for (; startCounter >= endCounter; startCounter--)
                {
                    //string extension = files[startCounter].Substring(files[startCounter].Length - 4);
                    string extension = files[startCounter].Split('.')[1];
                    string fileName = !string.IsNullOrEmpty(namePattern) ? namePattern : Path.GetFileName(files[startCounter]);
                    fileName = fileName.Contains('.') ? fileName.ToString().Replace(extension, string.Empty).Split('.')[1] : fileName.ToString();
                    string sufx = "";

                    if (startCounter  < 100)
                    {
                        if (startCounter < 10)
                            sufx = "00" + startCounter;
                        else
                            sufx = "0" + startCounter;
                    }
                    else
                    {
                        sufx = startCounter.ToString();
                    }
                    fileName = fileName.Substring(0, fileName.Length - 3) + (fileName.Contains("-") ? "" : "-")  + sufx + extension;

                    string targetPath = Path.Combine(destinationFolder, fileName);
                    CopyFile(Path.Combine(sourceFolder, files[startCounter]), targetPath);

                }
            }
            else
            {
                for (; startCounter <= endCounter; startCounter++)
                {
                    string extension = files[startCounter].Split('.')[1];
                    string fileName = !string.IsNullOrEmpty(namePattern) ? namePattern : Path.GetFileName(files[startCounter]);
                    fileName = fileName.Contains('.') ? fileName.ToString().Replace(extension, string.Empty).Split('.')[1] : fileName.ToString();
                    string sufx = "";

                    if (endCounter - startCounter < 100)
                    {
                        if (endCounter - startCounter < 10)
                            sufx = "00" + (endCounter - startCounter).ToString();
                        else
                            sufx = "0" + (endCounter - startCounter).ToString();
                    }
                    else
                    {
                        sufx = (endCounter - startCounter).ToString();
                    }
                    fileName = fileName + sufx  + "."+ extension;

                    string targetPath = Path.Combine(destinationFolder, fileName);
                    CopyFile(Path.Combine(sourceFolder, files[startCounter]), targetPath);

                }
            }
            return true;
        }


        public void CopyFile(string sourceFolder, string destinationFolder)
        {
            try
            {
                File.Copy(sourceFolder, destinationFolder);
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }

        public bool StartRenamer(string sourceFolder)
        {
            return StartRenamer(sourceFolder, "", SortOrder.DES);
        }

    }
}

public enum SortOrder
{
    ASD = 1,
    DES = 2,
}