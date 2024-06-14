using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaelsToolBox_2.Math;

public static class Normalizer
{
    public static float[] Floats(float[] input)
    {
        // if input size is 0 or 1 we do not need to do anything
        if (input.Length <= 1) return input;

        float[] newValues = new float[input.Length];

        // get total value
        float total = 0f;
        foreach (var item in input) total += item;

        // divide everything by total value
        for (int i = 0; i < input.Length; i++) newValues[i] = input[i] / total;

        // return the normalized array
        return newValues;
    }
}
