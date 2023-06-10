using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using DentedPixel;
using System;

public class GameMaster : MonoBehaviour {
  [SerializeField] double _roundDuration;
  [SerializeField] Typewriter _typewriter;
  [SerializeField] SentenceList[] _wordLists;
  [SerializeField] SentenceSRO[] _fixedSentences; // for if no wordlist
  [SerializeField] List<Typewriter.TypingStatistics> _stats;

  [SerializeField] double _timeRemaining;
  [SerializeField] bool _gameStarted = false;

  TMPro.TextMeshPro timerLabel;

  void Awake() {
    timerLabel = transform.GetChild(0).GetComponent<TMPro.TextMeshPro>();
  }

  // Start is called before the first frame update
  void Start() {
    foreach (var wordList in _wordLists) {
      wordList.Initialize();
    }

    _typewriter.SentenceSubmit += OnSentenceSubmit;
    _typewriter.Typo += OnTypo;
    _timeRemaining = _roundDuration;

    LeanTween.delayedCall(3, () => {
      GameStart();
    });
  }

  // Update is called once per frame
  void Update() {
    if (_timeRemaining >= 0 && _gameStarted)
      _timeRemaining -= Time.deltaTime;

    timerLabel.text = $"{((int)_timeRemaining) / 60}:{_timeRemaining % 60:00}";
  }

  void GameStart() {
    if (_wordLists.Length != _typewriter.PaperCount) {
      _typewriter.SetSentences(_fixedSentences);
      return;
    }

    _gameStarted = true;
    _typewriter.SetSentences(GetRandomSentences());
  }

  IEnumerable<SentenceSRO> GetRandomSentences() {
    HashSet<char> usedFirstCharacters = new();

    foreach (var wordList in _wordLists) {
      SentenceSRO sentenceSRO;
      do {
        sentenceSRO = wordList.GetRandomSentence();
      } while (usedFirstCharacters.Contains(sentenceSRO.sentence[0]));

      usedFirstCharacters.Add(sentenceSRO.sentence[0]);
      yield return sentenceSRO;
    }
  }

  void OnSentenceSubmit(Typewriter.TypingStatistics stats) {
    _stats.Add(stats);
    _typewriter.SetSentences(GetRandomSentences());
  }

  void OnTypo() {

  }
}
