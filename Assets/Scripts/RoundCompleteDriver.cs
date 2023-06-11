using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoundCompleteDisplay : MonoBehaviour {

  // TODO: Isinya value yang mau didisplay.
  public struct DisplayStats {

  }

  TMPro.TextMeshProUGUI _displayLabel;


  // To be called by GameMaster
  public void Init(DisplayStats ds) {
    // TODO: Ini string formatnya sekian value terus ada newlinenya
    // Pas panggil init pake String.Format untuk replacein nilainya
    string displayFormat = "{0}\n ";
  }

  void Awake() {
    transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
  }

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }
}
