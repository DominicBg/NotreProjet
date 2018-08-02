using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInGameCanvas : UIState, IUIManager {

    UICharCard[] charCards;
    PlayersManager playersManager;

	public void Enable(){

	}


	public void Disable(){

	}

    public void Initialize()
    {
        playersManager = PlayersManager.playersManager;
        UICharCard card = (UICharCard)Resources.Load("UICharCard", typeof(UICharCard));
        charCards = new UICharCard[playersManager.playersNumber];

        //Get Width from canvas 
        Canvas can = this.GetComponent<Canvas>();
        //RectTransform rectParent = can.rootCanvas.GetComponent<RectTransform>();
        RectTransform rectParent = can.GetComponent<RectTransform>();

        Debug.Log(rectParent.rect.width);
        float widthForCard = (rectParent.rect.width / 5f);
        float heighttForCard = rectParent.rect.height;
        Debug.Log(widthForCard);

        for(int i = 0; i < playersManager.playersNumber; i++)
        { 
            UICharCard zzzChar = Instantiate(card, gameObject.transform, false);

            RectTransform rectCard = zzzChar.GetComponent<RectTransform>();

            Vector3 newPos = new Vector3(widthForCard * (i + 1) - (rectParent.rect.width/2), 180f, 0f);

            rectCard.position = Vector3.zero;
            rectCard.localPosition = newPos;

            zzzChar.Initialize(playersManager.charactersPlayedNow[i]);
        }
    }

    // Use this for initialization
    void Start () {
        Debug.Log(this + " Start ZZZ");
        Debug.Log(this + " Enabled ZZZ");
        StartCoroutine(WaitForScene());
        //Initialize();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public IEnumerator WaitForScene()
    {

        yield return new WaitForSeconds(1);
        Initialize();
    }
}
