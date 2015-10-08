using UnityEngine;
using System.Collections;

public class CDistance : Consideration {
  public Transform target;
  public float     max;

  public override float Utility {
    get {
      var dir = (target.position - transform.position);
      return Mathf.Max(Mathf.Min(utilCurve.Evaluate(dir.magnitude / max), 1), 0);
    }
  }
}