using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMaster : MonoBehaviour {

  [SerializeField] double _maxHypeLevel = 10;
  [SerializeField] double _hypeDecayRate = 0.12; // Hype decrease by time
  [SerializeField] double _typoHypePenalty = 0.5; // Hype decrease when typo occurs
  [SerializeField] double _submitHypeMultiplier = 0.3; // Submission rating multiplier when submitting
  [SerializeField] double _punctuationBonusMultiplier = 0.08; // Extra multiplier for each punctuation if correct
  [SerializeField] double _punctuationOverMultiplier = 0.8; // Fix multiplier if too much punctuation
  [SerializeField] double _roundDuration;
  [SerializeField] Typewriter _typewriter;
  [SerializeField] SentenceList[] _wordLists;
  [SerializeField] SentenceSRO[] _fixedSentences; // for if no wordlist

  [SerializeField] double _timeRemaining;
  [SerializeField] double _hypeLevel;
  [SerializeField] int _machLevel;
  [SerializeField] double[] _machLevelThresholds;
  [SerializeField] bool _isGameStarted = false;
  [SerializeField] bool _isGameFinished = false;
  [SerializeField] UnityEvent _gameFinish;
  [SerializeField] UnityEvent _gameStart;
  [SerializeField] UnityEvent _sentenceSubmitted;
  [SerializeField] UnityEvent<int> _machLevelChange;

  // Statistics
  [SerializeField] List<Typewriter.TypingStatistics> _stats;
  [SerializeField] int _perfects;
  [SerializeField] int _perfectStreak;
  [SerializeField] int _longestPerfectStreak;

  [SerializeField] TMPro.TextMeshProUGUI _timerLabel;
  RoundCompleteDisplay _roundCompleteDisplay;
  GameObject _gameStartScreen;
  GameObject _gameEndScreen;

  double HypeLevel {
    get { return _hypeLevel; }
    set {
      var newMachLevel = 0;
      for (newMachLevel = 0; newMachLevel < _machLevelThresholds.Length; newMachLevel++)
        if (value < _machLevelThresholds[newMachLevel])
          break;

      if (newMachLevel != _machLevel) {
        _machLevelChange.Invoke(newMachLevel);
      }

      _machLevel = newMachLevel;
      _hypeLevel = Mathf.Clamp(0, (float)value, (float)_maxHypeLevel);
    }
  }

  void Awake() {
    var canvas = transform.GetChild(0);
    //_timerLabel = canvas.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
    _roundCompleteDisplay = canvas.GetChild(1).GetComponent<RoundCompleteDisplay>();
    _gameEndScreen = canvas.GetChild(1).gameObject;
    _gameStartScreen = canvas.GetChild(2).gameObject;

    _gameFinish ??= new UnityEvent();
    _gameStart ??= new UnityEvent();
    _sentenceSubmitted ??= new UnityEvent();
    _machLevelChange ??= new UnityEvent<int>();
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
      HypeLevel -= _hypeDecayRate * Time.deltaTime;

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

    LeanTween.moveLocalY(_gameStartScreen, 1000, 0.5f);
    _isGameStarted = true;
    _typewriter.AcceptingInput = true;
    _typewriter.SetSentences(GetRandomSentences());
    _gameStart.Invoke();
  }

  // Finish game
  void GameFinish() {
    _isGameFinished = true;
    _gameFinish.Invoke();
    _typewriter.AcceptingInput = false;
    _roundCompleteDisplay.Init(RoundCompleteDisplay.DisplayStats.Compile(_stats));

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
     AudioManager.instance.PlayOneShot(FmodEvents.instance.SubmitSound, this.transform.position);
    _stats.Add(stats);
    _typewriter.SetSentences(GetRandomSentences());
    if (stats.mistakes == 0) {
      _perfects++;
      _perfectStreak++;
      _longestPerfectStreak = Mathf.Max(_perfectStreak, _longestPerfectStreak);
    } else {
      _perfectStreak = 0;
    }
    double multiplier = stats.punctuations <= stats.rating ?
      stats.punctuations * _punctuationBonusMultiplier : // Multiply by punctuations if guessed correctly
      _punctuationOverMultiplier; // penalty if wrong guess
    double hypeLevelIncrease = stats.rating * multiplier * _submitHypeMultiplier;

    HypeLevel += hypeLevelIncrease;
    _sentenceSubmitted.Invoke();
  }

  void OnTypo() {
    HypeLevel -= _typoHypePenalty;
  }
}
