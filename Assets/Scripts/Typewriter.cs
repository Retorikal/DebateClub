using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Responsible for sentence reading, calculating per-sentence wpm, 
public class Typewriter : MonoBehaviour {
  public struct TypingStatistics {
    public double wpm;
    public int mistakes;
    public int charCount;
    public int punctuations;
    public double rating;
    public SentenceSRO sentence;
  }

  Paper[] papers;
  Paper currentPaper;
  bool sentenceFinished;

  public event Action<TypingStatistics> SentenceSubmit;

  // Start is called before the first frame update
  void Start() {
    papers = GetComponentsInChildren<Paper>();

    foreach (var paper in papers) {
      paper.SentenceFinish += OnSentenceFinish;
    }
  }

  // Set the senteces of all papers
  public void SetSentences(IEnumerable<SentenceSRO> sentences) {
    foreach (var item in EnumUtils.Zip(papers, sentences)) {
      item.first.Init(item.second);
    }
  }

  // Update is called once per frame
  void Update() {
    char input = '\0';

    // Read input

    if (input != '\0')
      UpdatePaper(input);
  }

  // 
  void UpdatePaper(char c) {
    if (c == '!' && sentenceFinished)
      Debug.Log("EXCLAMATION!");
    else
      Debug.Log("Typed" + c, this);
  }

  bool AttemptSubmitSentence() {
    if (!sentenceFinished)
      return false;

    var statistics = new TypingStatistics();
    statistics.

    return true;
  }

  void OnSentenceFinish() {
    sentenceFinished = true;
  }
}
