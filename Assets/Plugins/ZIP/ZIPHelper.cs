using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;

/// <summary>
/// zip helper
/// </summary>
public static class ZIPHelper {

    /// <summary>
    /// zuo files
    /// </summary>
    /// <param name="paths">all paths.</param>
    /// <param name="outputPath">where put zip into.</param>
    /// <param name="password">zip's password.</param>
    public static void ZIP(string outputPath,string password=null,params string[] paths)
    {
        if(paths.Length > 0)
        {
            using (ZipOutputStream zipOut = new ZipOutputStream(File.Create(outputPath)))
            {
                zipOut.SetLevel(9);
                foreach (var file in paths)
                {
                    var entry = new ZipEntry(Path.GetFileName(file));
                    entry.DateTime = DateTime.Now;
                    zipOut.PutNextEntry(entry);

                    var buffer = File.ReadAllBytes(file);
                    zipOut.Write(buffer, 0, buffer.Length);
                }
                zipOut.Password = password;
                zipOut.Finish();
                zipOut.Close();
            }
        }
    }

    /// <summary>
    /// extract from zip.
    /// </summary>
    /// <param name="zipPath">zip file 's path.</param>
    /// <param name="baseDirectory">where zip into.</param>
    /// <param name="password">password when extract zip file.</param>
    public static void Extract(string zipPath,string baseDirectory, string password=null)
    {
        if (!string.IsNullOrEmpty(zipPath))
        {
            using(ZipInputStream zipIn = new ZipInputStream(File.OpenRead(zipPath)))
            {
                zipIn.Password = password;
                ZipEntry entry = null;

                if (!Directory.Exists(baseDirectory))
                {
                    Directory.CreateDirectory(baseDirectory);
                }
                while((entry = zipIn.GetNextEntry()) != null)
                {
                    using (FileStream streamWriter = File.Create(Path.Combine(baseDirectory,entry.Name)))
                    {

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = zipIn.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

}
