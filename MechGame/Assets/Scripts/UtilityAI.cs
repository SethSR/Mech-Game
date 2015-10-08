// Video: www.gdcvault.com/play/1021848/Building-a-Better-Centaur-AI
using UnityEngine;
using System.Reflection;

public class UtilityAI {
  public float compensatedScore(float score, float numConsiderations) {
    var modificationFactor      = 1 - (1 / numConsiderations);
    var makeUpValue             = (1 - score) * modificationFactor;
    var finalConsiderationScore = score + (makeUpValue * score);
    return finalConsiderationScore;
  }

  public void getPublic(GameObject obj) {
    const BindingFlags flags = /*BindingFlags.NonPublic | */BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

    FieldInfo [] fields = obj.GetType().GetFields(flags);
    foreach (FieldInfo fieldInfo in fields) {
      Debug.Log("Obj: " + obj.name + ", Field: " + fieldInfo.Name);
    }

    PropertyInfo [] properties = obj.GetType().GetProperties(flags);
    foreach (PropertyInfo propertyInfo in properties) {
      Debug.Log("Obj: " + obj.name + ", Property: " + propertyInfo.Name);
    }
  }
}