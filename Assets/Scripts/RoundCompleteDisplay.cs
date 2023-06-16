using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoundCompleteDisplay : MonoBehaviour {

  // TODO: Isinya value yang mau didisplay.
  [System.Serializable]
  public struct DisplayStats {
    public int CharactersTyped;
    public int LongestPerfectStreak;
    public int HighestHypeLevel;
    public double LPM;
    public int Perfects;
    public int Mistakes;
    public SentenceSRO.Tone Style;

    public static DisplayStats Compile(IEnumerable<Typewriter.TypingStatistics> stats) {
      var sumLetters = 0;
      double sumLPM = 0;
      int sumMistakes = 0;
      int sumPerfects = 0;
      int maxHypeLevel = 0;
      int toneScore = 0;
      int charactersTyped = 0;
      int perfectStreak = 0;
      int longestPerfectStreak = 0;
      foreach (var stat in stats) {
        charactersTyped +=
        maxHypeLevel = Mathf.Max(maxHypeLevel, Mathf.Min(stat.punctuations, (int)stat.rating));
        sumLetters += stat.sentenceSRO.sentence.Length;
        sumLPM += stat.lpm;
        sumPerfects += stat.mistakes == 0 ? 1 : 0;
        sumMistakes += stat.mistakes;
        toneScore += stat.sentenceSRO.tone == SentenceSRO.Tone.AGGRESIVE ? 1 : 0;
        perfectStreak = stat.mistakes == 0 ? perfectStreak + 1 : 0;
        longestPerfectStreak = Mathf.Max(longestPerfectStreak, perfectStreak);
      }

      return new RoundCompleteDisplay.DisplayStats() {
        LongestPerfectStreak = longestPerfectStreak,
        HighestHypeLevel = maxHypeLevel,
        LPM = sumLPM / sumLetters,
        Perfects = sumPerfects,
        Mistakes = sumMistakes,
        Style = toneScore < 0 ? SentenceSRO.Tone.PERSUASIVE : SentenceSRO.Tone.AGGRESIVE
      };
    }
  }

  TMPro.TextMeshProUGUI _displayLabel;


  void Awake() {
    _displayLabel = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
  }

  // To be called by GameMaster
  public void Init(DisplayStats ds) {
    // Pas panggil init pake String.Format untuk replacein nilainya
    string displayFormat = $"{ds.CharactersTyped}\n{ds.LongestPerfectStreak}\n{ds.HighestHypeLevel}\n{ds.LPM}\n{ds.Perfects}\n{ds.Mistakes}\n{ds.Style.ToString()}";
    _displayLabel.text = displayFormat;
  }


  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }
}
