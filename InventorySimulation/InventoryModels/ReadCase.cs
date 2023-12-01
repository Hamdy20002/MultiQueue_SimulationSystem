using System;
using System.Collections.Generic;
using System.IO;

public class Program
{
    public static List<List<List<float>>> Run(string filePath)
    {
       
        List<List<List<float>>> res = new List<List<List<float>>>();
        List<List<float>> outer = new List<List<float>>();
        List<float> inputs = new List<float>();

        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                if (inputs.Count > 0)
                {
                    outer.Add(new List<float>(inputs));
                    inputs.Clear();
                }

                if (outer.Count > 0 && outer[0].Count > 0)
                {
                    res.Add(new List<List<float>>(outer));
                    outer.Clear();
                }
                continue;
            }

            var values = line.Split(',');
            foreach (var value in values)
            {
                if (float.TryParse(value, out float parsedValue))
                {
                    inputs.Add(parsedValue);
                }
                else
                {
                    // Handle the parsing error (if any)
                }
            }
        }

        if (inputs.Count > 0)
        {
            outer.Add(new List<float>(inputs));
        }

        if (outer.Count > 0 && outer[0].Count > 0)
        {
            res.Add(new List<List<float>>(outer));
        }

        return res;

    }
    }


