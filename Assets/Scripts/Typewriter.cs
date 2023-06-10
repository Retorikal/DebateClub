using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Responsible for sentence reading, calculating per-sentence wpm, 
public class Typewriter : MonoBehaviour {
  [System.Serializable]
  public class TypingStatistics {
    public double lpm;
    public System.DateTime startTime;
    public int mistakes;
    public int punctuations; // Between 0 to 10 too
    public double rating; // between 0 to 10
    public SentenceSRO sentence;
  }

  public int PaperCount { get { return _papers.Length; } }

  Paper[] _papers;
  Paper _currentPaper;
  TypingStatistics _currentStatistics;
  bool _sentenceFinished;

  public event Action<TypingStatistics> SentenceSubmit;
  public event Action Typo;

  // Set the senteces of all papers
  public void SetSentences(IEnumerable<SentenceSRO> sentences) {
    var r = new System.Random();
    foreach (var item in EnumUtils.Zip(EnumUtils.Shuffled(_papers), sentences)) {
      item.first?.Init(item.second);
    }
  }

  // Awake
  void Awake() {
    _papers = GetComponentsInChildren<Paper>();
  }

  // Start is called before the first frame update
  void Start() {
    foreach (var paper in _papers) {
      paper.SentenceFinish += OnSentenceFinish;
    }
  }


  // Update is called once per frame
  void Update() {
    // Read input
    string s = Input.inputString;
    if (!(s == "")) {
      char c = s[0];
      switch ((int)c) {
        case 8: // backspace
          Debug.Log("No need to backspace.", this);
          break;
        case 13: // enter
          AttemptSubmitSentence();
          break;
        default:
          UpdatePaper(c);
          break;
      }
    }
  }

  // Handle paper state based on input
  void UpdatePaper(char c) {
    if (c == '!' && _sentenceFinished) {
      bool festive = false;
      _currentPaper.AddExclamationMark(festive);
      _currentStatistics.punctuations++;
      Debug.Log("EXCLAMATION!");
    } else {
      // Check and assign the SentenceSRO if not locked in yet
      if (_currentPaper == null) {
        _currentStatistics = new TypingStatistics {
          startTime = System.DateTime.Now,
          mistakes = 0
        };

        foreach (var paper in _papers) {
          if (!paper.AdvanceNextLetter(c))
            continue;

          Debug.Log("Sentence locked in: " + paper.Sentence, this);
          _currentPaper = paper;
          _currentStatistics.sentence = _currentPaper.SentenceSRO;
          break;
        }

        return;
      }

      bool isCorrect = _currentPaper.AdvanceNextLetter(c);
      _currentStatistics.mistakes += isCorrect ? 0 : 1;
      Debug.Log("Typed " + c + (isCorrect ? "(Hit)" : "(Miss)"), this);
    }
  }

  bool AttemptSubmitSentence() {
    Debug.Log("AttemptSubmitSentence", this);
    _currentPaper = null;
    if (!_sentenceFinished)
      return false;

    SentenceSubmit?.Invoke(_currentStatistics);

    return true;
  }

  void OnSentenceFinish() {
    Debug.Log("OnSentenceFinish", this);
    var timeDiff = (System.DateTime.Now - _currentStatistics.startTime).Seconds;
    _currentStatistics.lpm = 60 * (_currentPaper.SentenceSRO.sentence.Length / timeDiff);

    _sentenceFinished = true;
  }
}
