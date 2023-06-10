using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
using DentedPixel;
using System;

public class GameMaster : MonoBehaviour {
  [SerializeField] Typewriter _typewriter;
  [SerializeField] SentenceList[] _wordLists;
  [SerializeField] SentenceSRO[] _fixedSentences; // for if no wordlist
  [SerializeField] List<Typewriter.TypingStatistics> _stats;

  // Start is called before the first frame update
  void Start() {
    foreach (var wordList in _wordLists) {
      wordList.Initialize();
    }

    _typewriter.SentenceSubmitted += OnSentenceSubmitted;

    LeanTween.delayedCall(3, () => {
      GameStart();
    });
  }

  // Update is called once per frame
  void Update() {

  }

  void GameStart() {
    if (_wordLists.Length != _typewriter.PaperCount) {
      _typewriter.SetSentences(_fixedSentences);
      return;
    }

    _typewriter.SetSentences(GetRandomSentences());
  }

  IEnumerable<SentenceSRO> GetRandomSentences() {
    foreach (var wordList in _wordLists) {
      yield return wordList.GetRandomSentence();
    }
  }

  void OnSentenceSubmitted(Typewriter.TypingStatistics stats) {
    _stats.Add(stats);
    _typewriter.SetSentences(GetRandomSentences());
  }
}
