using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Responsible for sentence reading, calculating per-sentence wpm, 
public class Typewriter : MonoBehaviour {
  public class TypingStatistics {
    public double lpm;
    public System.DateTime startTime;
    public int mistakes;
    public int punctuations;
    public double rating;
    public SentenceSRO sentence;
  }

  Paper[] papers;
  Paper currentPaper;
  TypingStatistics currentStatistics;
  bool sentenceFinished;

  public event Action<TypingStatistics> SentenceSubmitted;

  // Start is called before the first frame update
  void Start() {
    papers = GetComponentsInChildren<Paper>();

    foreach (var paper in papers) {
      paper.SentenceFinished += OnSentenceFinish;
    }
  }

  // Set the senteces of all papers
  public void SetSentences(IEnumerable<SentenceSRO> sentences) {
    foreach (var item in EnumUtils.Zip(papers, sentences)) {
      item.first?.Init(item.second);
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
    if (c == '!' && sentenceFinished) {
      bool festive = false;
      currentPaper.AddExclamationMark(festive);
      Debug.Log("EXCLAMATION!");
    } else {
      if (currentPaper == null) {
        currentStatistics.startTime = System.DateTime.Now;
      }


      bool isCorrect = currentPaper.AdvanceNextLetter(c);
      currentStatistics.mistakes += isCorrect ? 1 : 0;
      Debug.Log("Typed" + c, this);
    }
  }

  void AssignPaper() {
    currentStatistics = new TypingStatistics {
      sentence = currentPaper.SentenceSRO,
    };
  }

  bool AttemptSubmitSentence() {
    if (!sentenceFinished)
      return false;

    SentenceSubmitted?.Invoke(currentStatistics);

    return true;
  }

  void OnSentenceFinish() {
    var timeDiff = (System.DateTime.Now - currentStatistics.startTime).Seconds;
    currentStatistics.lpm = 60 * (currentPaper.SentenceSRO.sentence.Length / timeDiff);

    sentenceFinished = true;
  }
}
