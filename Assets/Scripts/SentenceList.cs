
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;
using System;

[Serializable]
public class SentenceList {
  public SentenceSRO.Tone tone;
  public TextAsset sentenceFile;
  List<SentenceSRO> _sentenceList;

  public void Initialize() {
    _sentenceList ??= new();

    foreach (var text in sentenceFile.text.Split('\n')) {
      var sentence = ScriptableObject.CreateInstance<SentenceSRO>();
      sentence.tone = tone;
      sentence.sentence = text.Trim();

      _sentenceList.Add(sentence);
    }
  }

  public SentenceSRO GetRandomSentence() {
    return _sentenceList[UnityEngine.Random.Range(0, _sentenceList.Count)];
  }
}
