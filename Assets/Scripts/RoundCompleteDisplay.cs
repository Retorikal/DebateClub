using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoundCompleteDisplay : MonoBehaviour {

  // TODO: Isinya value yang mau didisplay.
  [System.Serializable]
  public struct DisplayStats {
    public int LongestPerfectStreak;
    public int HighestHypeLevel;
    public double LPM;
    public int Perfects;
    public int Mistakes;
    public SentenceSRO.Tone Style;
  }

  TMPro.TextMeshProUGUI _displayLabel;


  void Awake() {
    _displayLabel = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
  }

  // To be called by GameMaster
  public void Init(DisplayStats ds) {
    // TODO: Ini string formatnya sekian value terus ada newlinenya
    // Pas panggil init pake String.Format untuk replacein nilainya
    string displayFormat = $"{ds.LongestPerfectStreak}\n{ds.HighestHypeLevel}\n{ds.LPM}\n{ds.Perfects}\n{ds.Mistakes}\n{ds.Style.ToString()}";
    _displayLabel.text = displayFormat;
  }


  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }
}
