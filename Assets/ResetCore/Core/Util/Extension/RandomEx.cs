﻿using UnityEngine;
using System.Collections;
using System;

public class RandomEx {

    public static System.Random CreateRandom()
    {
        long ticks = DateTime.Now.Ticks;
        return new System.Random(((int)(((ulong)ticks) & 0xffffffffL)) | ((int)(ticks >> 0x20)));
    }
}
