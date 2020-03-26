using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MenuCtrl : MonoBehaviour
{
	public PlayerButton buttonTmp;
	public TextMeshProUGUI titleTmp;
	public SliderCtrl sliderTmp;

	public Transform buttonPanel;

	void Awake()
	{
		buttonTmp.gameObject.SetActive(false);
		titleTmp.gameObject.SetActive(false);
		sliderTmp.gameObject.SetActive(false);
	}

    // Start is called before the first frame update
    void Start()
    {
        createTitle("Scene configuration");

		createButton(EF.img.iconClose, "", () => { EF.efCtrl.CloseConfigMenu(); });
    }

	internal void Hide()
	{
		buttonPanel.gameObject.SetActive(false);
	}

	private void createButton(Sprite iconPlay, string s, System.Action act)
	{
		var button = Instantiate(buttonTmp);
		button.icon.sprite = iconPlay;
		button.text.text = s;
		button.act = act;
		addMenuObject(button.gameObject);
	}

	private void createTitle(string s)
	{
		var title = Instantiate(titleTmp);
		title.text = s;
		addMenuObject(title.gameObject);
	}

	private void addMenuObject(GameObject go)
	{
		go.gameObject.SetActive(true);
		go.transform.SetParent(buttonPanel);
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
