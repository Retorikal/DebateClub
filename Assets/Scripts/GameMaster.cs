using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMaster : MonoBehaviour {
  [SerializeField] double _roundDuration;
  [SerializeField] Typewriter _typewriter;
  [SerializeField] SentenceList[] _wordLists;
  [SerializeField] SentenceSRO[] _fixedSentences; // for if no wordlist
  [SerializeField] List<Typewriter.TypingStatistics> _stats;

  [SerializeField] double _timeRemaining;
  [SerializeField] bool _isGameStarted = false;
  [SerializeField] UnityEvent _gameFinish;
  [SerializeField] UnityEvent _gameStart;

  TMPro.TextMeshProUGUI _timerLabel;
  RoundCompleteDisplay _roundCompleteDisplay;

  void Awake() {
    var canvas = transform.GetChild(0);
    _timerLabel = canvas.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
    _roundCompleteDisplay = transform.GetChild(1).GetComponent<RoundCompleteDisplay>();
    _gameFinish ??= new UnityEvent();
    _gameStart ??= new UnityEvent();
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
    if (_timeRemaining > 0 && _isGameStarted)
      _timeRemaining -= Time.deltaTime;
    else {
      _timeRemaining = 0;
      GameFinish();
    }

    _timerLabel.text = $"{((int)_timeRemaining) / 60}:{_timeRemaining % 60:00}";
  }

  // Start da game
  void GameStart() {
    if (_wordLists.Length != _typewriter.PaperCount) {
      _typewriter.SetSentences(_fixedSentences);
      return;
    }

    _isGameStarted = true;
    _typewriter.SetSentences(GetRandomSentences());
    _gameStart.Invoke();
  }

  void GameFinish() {
    _gameFinish.Invoke();
    _roundCompleteDisplay.Init(new RoundCompleteDisplay.DisplayStats() {
      // Stat to display..
    });
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
