using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharCard : MonoBehaviour {

    public Text characterName;

    public Text RunningSpeed;
    public Image RunningSpeedImage;

    public Text JumpingHeight;
    public Image JumpingHeightImage;

    public Image bombImage;
    public Image miniBomb;

    public Color cardColor;

	// Use this for initialization
	void Start () {
		
	}

    public void Initialize()
    {
        Debug.Log("NoChar");
    }

    public void Initialize(Character charZ)
    {
        characterName.text = charZ.characterName;

    }
    // Update is called once per frame
    void Update () {
		
	}
}
