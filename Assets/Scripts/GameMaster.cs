using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMaster : MonoBehaviour {
  [SerializeField] double _roundDuration;
  [SerializeField] Typewriter _typewriter;
  [SerializeField] SentenceList[] _wordLists;
  [SerializeField] SentenceSRO[] _fixedSentences; // for if no wordlist

  [SerializeField] double _timeRemaining;
  [SerializeField] double _hypeLevel;
  [SerializeField] bool _isGameStarted = false;
  [SerializeField] bool _isGameFinished = false;
  [SerializeField] UnityEvent _gameFinish;
  [SerializeField] UnityEvent _gameStart;
  [SerializeField] UnityEvent<int> _machIncrease;

  // Statistics
  [SerializeField] List<Typewriter.TypingStatistics> _stats;
  [SerializeField] int _perfects;
  [SerializeField] int _perfectStreak;
  [SerializeField] int _longestPerfectStreak;

  TMPro.TextMeshProUGUI _timerLabel;
  RoundCompleteDisplay _roundCompleteDisplay;
  GameObject _gameStartScreen;
  GameObject _gameEndScreen;

  void Awake() {
    var canvas = transform.GetChild(0);
    _timerLabel = canvas.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
    _roundCompleteDisplay = canvas.GetChild(1).GetComponent<RoundCompleteDisplay>();
    _gameEndScreen = canvas.GetChild(1).gameObject;
    _gameStartScreen = canvas.GetChild(2).gameObject;

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
    if (_isGameStarted) {
      if (_timeRemaining > 0)
        _timeRemaining -= Time.deltaTime;
      else if (!_isGameFinished) {
        _timeRemaining = 0;
        GameFinish();
      }
    }

    _timerLabel.text = $"{((int)_timeRemaining) / 60}:{_timeRemaining % 60:00}";
  }

  // Start game
  void GameStart() {
    if (_wordLists.Length != _typewriter.PaperCount) {
      _typewriter.SetSentences(_fixedSentences);
      return;
    }

    LeanTween.moveLocalY(_gameStartScreen, 540, 0.5f);
    _isGameStarted = true;
    _typewriter.SetSentences(GetRandomSentences());
    _gameStart.Invoke();
  }

  // Finish game
  void GameFinish() {
    _gameFinish.Invoke();
    var sumLetters = 0;
    double sumLPM = 0;
    int sumMistakes = 0;
    int sumPerfects = 0;
    int maxHypeLevel = 0;
    int toneScore = 0;
    foreach (var stat in _stats) {
      maxHypeLevel = Mathf.Max(maxHypeLevel, Mathf.Min(stat.punctuations, (int)stat.rating));
      sumLetters += stat.sentenceSRO.sentence.Length;
      sumLPM += stat.lpm;
      sumPerfects += stat.mistakes == 0 ? 1 : 0;
      sumMistakes += stat.mistakes;
      toneScore += stat.sentenceSRO.tone == SentenceSRO.Tone.AGGRESIVE ? 1 : 0;
    }

    _roundCompleteDisplay.Init(new RoundCompleteDisplay.DisplayStats() {
      LongestPerfectStreak = _longestPerfectStreak,
      HighestHypeLevel = maxHypeLevel,
      LPM = sumLPM / sumLetters,
      Perfects = sumPerfects,
      Mistakes = sumMistakes,
      Style = toneScore < 0 ? SentenceSRO.Tone.PERSUASIVE : SentenceSRO.Tone.AGGRESIVE
    });

    _isGameFinished = true;
    LeanTween.moveLocalY(_gameEndScreen, 0, 0.5f);
    _typewriter.enabled = false;
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
    if (stats.mistakes == 0) {
      _perfects++;
      _perfectStreak++;
      _longestPerfectStreak = Mathf.Max(_perfectStreak, _longestPerfectStreak);
    } else {
      _perfectStreak = 0;
    }
  }

  void OnTypo() {

  }
}