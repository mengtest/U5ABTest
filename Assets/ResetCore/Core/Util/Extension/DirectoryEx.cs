using UnityEngine;
using System.Collections;
using System.IO;

namespace ResetCore.Util
{
    public class DirectoryEx
    {

        public static void DirectoryCopy(string from, string to, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(from);
            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + from);
            }
            if (!Directory.Exists(to))
            {
                Directory.CreateDirectory(to);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(to, file.Name);
                file.CopyTo(temppath, false);
            }
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(to, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }

}
