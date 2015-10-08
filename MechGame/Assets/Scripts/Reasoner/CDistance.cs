﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class CDistance : Consideration {
  public Transform target;
  public float     maxDistance;

  public override float Utility {
    get {
      var dir = (target.position - transform.position);
      return Mathf.Max(Mathf.Min(utilCurve.Evaluate(dir.magnitude / maxDistance), 1), 0);
    }
  }

  public override float InvUtility {
  	get {
  		return 1 - Utility;
  	}
  }
}