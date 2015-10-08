using UnityEngine;
using System.Collections;

public abstract class Consideration : MonoBehaviour {
  public AnimationCurve utilCurve;

  public abstract float Utility { get; }

  public float InvUtility { get { return 1 - Utility; } }
}