//------------------------------------------------------------------------
//
//  Name: Smoother.h
//
//  Desc: Template class to help calculate the average value of a history
//        of values. This can only be used with types that have a 'zero'
//        value and that have the += and / operators overloaded.
//
//        Example: Used to smooth frame rate calculations.
//
//  Author: Mat Buckland (fup@ai-junkie.com)
//
//------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NumSmoother {
  //this holds the history
  List<float>  m_History;

  int           m_iNextUpdateSlot;

  //to instantiate a Smoother pass it the number of samples you want
  //to use in the smoothing, and an exampe of a 'zero' type
  public NumSmoother(int SampleSize) {
    m_History = new List<float>(SampleSize);
    m_iNextUpdateSlot = 0;
  }

  //each time you want to get a new average, feed it the most recent value
  //and this method will return an average over the last SampleSize updates
  public float update(float MostRecentValue) {
    //overwrite the oldest value with the newest
    m_History[m_iNextUpdateSlot++] = MostRecentValue;

    //make sure m_iNextUpdateSlot wraps around. 
    if (m_iNextUpdateSlot == m_History.Count) m_iNextUpdateSlot = 0;

    //now to calculate the average of the history list
    return m_History.Sum() / m_History.Count;
  }
}

public class VecSmoother {
  List<Vector3> history;
  int nextUpdateSlot;

  public VecSmoother(int sample_size) {
    history = new List<Vector3>(sample_size);
    for (int i = sample_size; i --> 0;) { history.Add(Vector3.zero); }
    nextUpdateSlot = 0;
  }

  public Vector3 update(Vector3 most_recent_value) {
    history[nextUpdateSlot++] = most_recent_value;
    if (nextUpdateSlot == history.Count) nextUpdateSlot = 0;
    return history.Aggregate((a,b) => a + b) / history.Count;
  }
}