using UnityEngine;
using System.Collections;

public class Version
{
    public int x;
    public int y;
    public int z;
    public int w;

    public Version(int x, int y, int z, int w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public override string ToString()
    {
        return x + "." + y + "." + z + "." + w;
    }

    public static Version Parse(string value)
    {
        string[] values = value.Split('.');
        if (value.Length >= 4)
            return new Version(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]));
        else
            return new Version(0, 0, 0, 1);
    }

    public int Compare(Version ver1, Version ver2)
    {
        int[] num1 = new int[] { ver1.x, ver1.y, ver1.z, ver1.w };
        int[] num2 = new int[] { ver2.x, ver2.y, ver2.z, ver2.w };
        for (int i = 0; i < 4; i++)
        {
            if (num1[i] > num2[i])
            {
                return 1;
            }
            else if (num1[i] < num2[i])
            {
                return -1;
            }
        }
        return 0;
    }
}

public class VersionManager {

	
}
