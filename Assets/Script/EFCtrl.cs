using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFCtrl : MonoBehaviour
{
	[HideInInspector]
	public Population population;
	public MenuCtrl menuCtrl;

    // Start is called before the first frame update
    void Awake()
    {
		EF.img = FindObjectOfType<ImageCollection>();
		menuCtrl = FindObjectOfType<MenuCtrl>();

		population = new Population();
		population.Create();
    }

    // Update is called once per frame
    void Update()
    {
        population.Step(5 * Time.deltaTime);
    }

	internal void CloseConfigMenu()
	{
		menuCtrl.Hide();
	}
}
