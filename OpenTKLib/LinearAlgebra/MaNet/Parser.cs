using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace OpenTKExtension
{
  public static class Parser
    {

      public static Matrix3 Substitute(string baseMatrix, string stringToSubtitute, double substitutionValue)
      {
          string working = baseMatrix.Replace(stringToSubtitute, substitutionValue.ToString("R"));
          Matrix3 mat = new Matrix3();
          return mat.Parse(working);

      }
    }
}
