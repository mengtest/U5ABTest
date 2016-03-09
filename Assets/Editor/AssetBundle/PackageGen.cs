using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

public class PackageGen {

	[MenuItem("Assets/AssetBundle/生成整包")]
    public static void CompressZipPackage()
    {
        //if (EditorPrefs.HasKey("Version"))
        //{

        //}
        CompressHelper.CompressDirectory(PathConfig.bundleRootPath, PathConfig.bundlePkgExportPath + "/test");
        Debug.logger.Log("压缩完成");
    }

    [MenuItem("Assets/AssetBundle/解压整包")]
    public static void DecompressZipPackage()
    {
        CompressHelper.DecompressToDirectory(PathConfig.bundleRootPath, PathConfig.bundlePkgExportPath + "/test");
        Debug.logger.Log("解压完成");
    }

    public static void CompressFiles(string sourcePath, string[] filePath, string outputFilePath, int zipLevel)
    {
        Stream target = new FileStream(outputFilePath, FileMode.OpenOrCreate);
        sourcePath = Path.GetFullPath(sourcePath);
        int startIndex = string.IsNullOrEmpty(sourcePath) ? Path.GetPathRoot(sourcePath).Length : sourcePath.Length;
        using (ZipOutputStream stream = new ZipOutputStream(target))
        {
            stream.SetLevel(zipLevel);

            foreach (string str in filePath)
            {
                string input = str.Substring(startIndex).Replace(@"\", "/");
                string name = input.StartsWith(@"/") ? input.ReplaceFirst(@"/", "", 0) : input;
                stream.PutNextEntry(new ZipEntry(name));
                Debug.Log(name);
                if (!str.EndsWith(@"/"))
                {
                    byte[] buffer = new byte[0x800];
                    using (FileStream stream2 = File.OpenRead(str))
                    {
                        int num2;
                        while ((num2 = stream2.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            stream.Write(buffer, 0, num2);
                        }
                    }
                }
            }

            stream.Finish();
        }
    }
}
