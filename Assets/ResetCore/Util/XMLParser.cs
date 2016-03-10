//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Security;
//using UnityEngine;

//public class XMLParser
//{
//    public Dictionary<string, Dictionary<string, string>> LoadMap(string fileName, out string key)
//    {
//        key = Path.GetFileNameWithoutExtension(fileName);
//        SecurityElement xml = XMLParser.Load(fileName);
//        return XMLParser.LoadMap(xml);
//    }

//    public bool LoadMap(string fileName, out Dictionary<string, Dictionary<string, string>> map)
//    {
//        bool result;
//        try
//        {
//            SecurityElement xml = XMLParser.Load(fileName);
//            map = XMLParser.LoadMap(xml);
//            result = true;
//        }
//        catch (Exception ex)
//        {
//            Debug.logger.LogError("XMLParse", "File not exist: " + fileName);
//            Debug.logger.LogException(ex, null);
//            map = null;
//            result = false;
//        }
//        return result;
//    }

//    public static bool LoadIntMap(string fileName, bool isForceOutterRecoure, out Dictionary<int, Dictionary<string, string>> map)
//    {
//        bool result;
//        try
//        {
//            SecurityElement securityElement;
//            if (isForceOutterRecoure)
//            {
//                securityElement = XMLParser.LoadOutter(fileName);
//            }
//            else
//            {
//                string strFileName = StringEx.GetFilePathWithoutExtention(fileName);
//                securityElement = XMLParser.Load(strFileName);
//            }
//            if (securityElement == null)
//            {
//                Debug.logger.LogError("XMLParse", "File not exist: " + fileName);
//                map = null;
//                result = false;
//            }
//            else
//            {
//                map = XMLParser.LoadIntMap(securityElement, fileName);
//                result = true;
//            }
//        }
//        catch (Exception ex)
//        {
//            Debug.logger.LogError("XMLParse", "Load Int Map Error: " + fileName + "  " + ex.Message);
//            map = null;
//            result = false;
//        }
//        return result;
//    }

//    public static Dictionary<int, Dictionary<string, string>> LoadIntMap(SecurityElement xml, string source)
//    {
//        Dictionary<int, Dictionary<string, string>> dictionary = new Dictionary<int, Dictionary<string, string>>();
//        int num = 0;
//        foreach (SecurityElement securityElement in xml.Children)
//        {
//            num++;
//            if (securityElement.Children == null || securityElement.Children.Count == 0)
//            {
//                Debug.logger.LogWarning("XMLPrase", string.Concat(new object[]
//                    {
//                        "empty row in row NO.",
//                        num,
//                        " of ",
//                        source
//                    }));
//            }
//            else
//            {
//                int num2 = int.Parse((securityElement.Children[0] as SecurityElement).Text);
//                if (dictionary.ContainsKey(num2))
//                {
//                    Debug.logger.LogWarning("XMLPrase", string.Format("Key {0} already exist, in {1}.", num2, source));
//                }
//                else
//                {
//                    Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
//                    dictionary.Add(num2, dictionary2);
//                    for (int i = 1; i < securityElement.Children.Count; i++)
//                    {
//                        SecurityElement securityElement2 = securityElement.Children[i] as SecurityElement;
//                        string key;
//                        if (securityElement2.Tag.Length < 3)
//                        {
//                            key = securityElement2.Tag;
//                        }
//                        else
//                        {
//                            string a = securityElement2.Tag.Substring(securityElement2.Tag.Length - 2, 2);
//                            if (a == "_i" || a == "_s" || a == "_f" || a == "_l" || a == "_k" || a == "_m")
//                            {
//                                key = securityElement2.Tag.Substring(0, securityElement2.Tag.Length - 2);
//                            }
//                            else
//                            {
//                                key = securityElement2.Tag;
//                            }
//                        }
//                        if (securityElement2 != null && !dictionary2.ContainsKey(key))
//                        {
//                            if (string.IsNullOrEmpty(securityElement2.Text))
//                            {
//                                dictionary2.Add(key, "");
//                            }
//                            else
//                            {
//                                dictionary2.Add(key, securityElement2.Text.Trim());
//                            }
//                        }
//                        else
//                        {
//                            Debug.logger.LogWarning("XMLPrase", string.Format("Key {0} already exist, index {1} of {2}.", securityElement2.Tag, i, securityElement2.ToString()));
//                        }
//                    }
//                }
//            }
//        }
//        return dictionary;
//    }

//    public static Dictionary<string, Dictionary<string, string>> LoadMap(SecurityElement xml)
//    {
//        Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
//        foreach (SecurityElement securityElement in xml.Children)
//        {
//            string text = (securityElement.Children[0] as SecurityElement).Text.Trim();
//            if (dictionary.ContainsKey(text))
//            {
//                Debug.logger.LogWarning("XMLPrase", string.Format("Key {0} already exist, in {1}.", text, xml.ToString()));
//            }
//            else
//            {
//                Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
//                dictionary.Add(text, dictionary2);
//                for (int i = 1; i < securityElement.Children.Count; i++)
//                {
//                    SecurityElement securityElement2 = securityElement.Children[i] as SecurityElement;
//                    if (securityElement2 != null && !dictionary2.ContainsKey(securityElement2.Tag))
//                    {
//                        if (string.IsNullOrEmpty(securityElement2.Text))
//                        {
//                            dictionary2.Add(securityElement2.Tag, "");
//                        }
//                        else
//                        {
//                            dictionary2.Add(securityElement2.Tag, securityElement2.Text.Trim());
//                        }
//                    }
//                    else
//                    {
//                        Debug.logger.LogWarning("XMLPrase", string.Format("Key {0} already exist, index {1} of {2}.", securityElement2.Tag, i, securityElement2.ToString()));
//                    }
//                }
//            }
//        }
//        return dictionary;
//    }

//    public static string LoadText(string fileName)
//    {
//        string result;
//        try
//        {
//            Debug.Log(fileName + "  本地加载xml");
//            result = Utils.LoadResource(fileName);
//        }
//        catch (Exception ex)
//        {
//            Debug.LogError(ex + "  " + fileName);
//            result = "";
//        }
//        return result;
//    }

//    public static byte[] LoadBytes(string fileName)
//    {
//        return null;
//    }

//    public static SecurityElement Load(string fileName)
//    {
//        string text = XMLParser.LoadText(fileName);
//        SecurityElement result;
//        if (string.IsNullOrEmpty(text))
//        {
//            result = null;
//        }
//        else
//        {
//            result = XMLParser.LoadXML(text);
//        }
//        return result;
//    }

//    public static SecurityElement LoadOutter(string fileName)
//    {
//        string text = Utils.LoadFile(fileName.Replace('\\', '/'));
//        SecurityElement result;
//        if (string.IsNullOrEmpty(text))
//        {
//            result = null;
//        }
//        else
//        {
//            result = XMLParser.LoadXML(text);
//        }
//        return result;
//    }

//    public static SecurityElement LoadXML(string xml)
//    {
//        SecurityElement result;
//        try
//        {
//            SecurityParser securityParser = new SecurityParser();
//            securityParser.LoadXml(xml);
//            result = securityParser.ToXml();
//        }
//        catch (Exception ex)
//        {
//            LoggerHelper.Except(ex, null);
//            result = null;
//        }
//        return result;
//    }

//    public static void SaveBytes(string fileName, byte[] buffer)
//    {
//        if (!Directory.Exists(Utils.GetDirectoryName(fileName)))
//        {
//            Directory.CreateDirectory(Utils.GetDirectoryName(fileName));
//        }
//        if (File.Exists(fileName))
//        {
//            File.Delete(fileName);
//        }
//        using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
//        {
//            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
//            {
//                binaryWriter.Write(buffer);
//                binaryWriter.Flush();
//                binaryWriter.Close();
//            }
//            fileStream.Close();
//        }
//    }

//    public static void SaveText(string fileName, string text)
//    {
//        if (!Directory.Exists(Utils.GetDirectoryName(fileName)))
//        {
//            Directory.CreateDirectory(Utils.GetDirectoryName(fileName));
//        }
//        if (File.Exists(fileName))
//        {
//            File.Delete(fileName);
//        }
//        using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
//        {
//            using (StreamWriter streamWriter = new StreamWriter(fileStream))
//            {
//                streamWriter.Write(text);
//                streamWriter.Flush();
//                streamWriter.Close();
//            }
//            fileStream.Close();
//        }
//    }
//}