﻿using Csla.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Csla.Generators.TestObjects
{

  /// <summary>
  /// A second class including a private nested class for which automatic serialization code is to be generated
  /// This checks for naming clashes, as its child class is named the same as the one in NestingPOCO
  /// </summary>
  /// <remarks>The class is decorated with the AutoSerializable attribute so that it is picked up by our source generator</remarks>
  [AutoSerializable]
  public partial class NestingPOCO2
  {

    [AutoSerialized]
    private NestedPOCO _poco = new NestedPOCO() { Value = "Hello" };

    [AutoSerializable]
    protected internal partial class NestedPOCO
    {

      public string Value { get; set; }

    }

    public string GetValue()
    {
      return _poco.Value;
    }

    public void SetValue(string value)
    {
      _poco.Value = value;
    }

  }
}
