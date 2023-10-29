using System;
using System.Collections.Generic;
using System.IO;

public class Program
{
    public static List<List<List<float>>> Run(string filePath)
    {
        # region get started

        List<float> inputs = new List<float>();
        List<List<float>> outer = new List<List<float>>();
        List<List<List<float>>> res = new List<List<List<float>>>();

        float ss = 0f;
        int currentLineIndex = 0;
        int store = 0;

        #endregion

        # region ReadLines& Use it

        var line = File.ReadAllLines(filePath);
        
        


        foreach (var i in line)
        {
            if (i == "" & store == 4)
            {
                res.Add(outer);
                outer = new List<List<float>>();
            }

            if (line.Length > 0 & i != "")
            {

                if (store == 4)
                {
                    inputs = new List<float>();

                    var x = i.Split(',');
                    try
                    {
                        if (float.TryParse(x[0], out ss))
                        {
                            inputs.Add(ss);
                        }
                        if (float.TryParse(x[1], out ss))
                        {
                            inputs.Add(ss);
                        }
                    }

                    catch
                    {
                        currentLineIndex++;
                        continue;
                    }
                    outer.Add(inputs);

                    if (currentLineIndex == (line.Length - 1))
                    {
                        res.Add(outer);
                        break;
                    }
                    currentLineIndex++;
                    continue;
                }

                if (float.TryParse(i, out ss))
                {
                    inputs.Add(ss);
                    store++;
                    if (store == 4)
                    {
                        outer.Add(inputs);
                    }
                }
            }

            currentLineIndex++;
        }

        # endregion

        return res;
    }

}