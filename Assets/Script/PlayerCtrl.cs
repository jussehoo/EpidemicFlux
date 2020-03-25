using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
	public PlayerButton buttonTmp;
	public Transform buttonPanel;

	void Awake()
	{
		buttonTmp.gameObject.SetActive(false);
	}

    // Start is called before the first frame update
    void Start()
    {
        createButton(EF.img.iconPlay, "Play");
        createButton(EF.img.iconPause, "Pause");
        createButton(EF.img.iconReset, "Reset");
    }

	private void createButton(Sprite iconPlay, string s)
	{
		var button = Instantiate(buttonTmp);
		button.gameObject.SetActive(true);
		button.transform.SetParent(buttonPanel);
		button.icon.sprite = iconPlay;
		button.text.text = s;
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
