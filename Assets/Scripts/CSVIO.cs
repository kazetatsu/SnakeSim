// SPDX-FileCopyrightText: 2025 Shinagwa Kazemaru
// SPDX-License-Identifier: BSD-2-Clause license

using UnityEngine;
using System.Collections.Generic;
using System.IO;

public static class CSVIO {
    public static List<float[]> Read(string fileName) {
        var ret = new List<float[]>();
        var reader = new StreamReader(Application.dataPath + "/" + fileName);
        string line = reader.ReadLine();
        if (line is null) return null;
        string[] sCols;
        sCols = line.Split(',');
        int nc = sCols.Length;
        while (true) {
            var fCols = new float[nc];
            for (int i = 0; i < nc; ++i)
                fCols[i] = float.Parse(sCols[i]);
            ret.Add(fCols);

            line = reader.ReadLine();
            if (line is null) break;
            sCols = line.Split(',');
        }
        reader.Close();
        return ret;
    }
}
