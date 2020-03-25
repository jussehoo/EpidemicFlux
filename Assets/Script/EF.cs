using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EF : MonoBehaviour
{
	internal static ImageCollection img;

	public static EFCtrl efCtrl;
	
	public const float unitDistance = .5f;

	private void Awake()
	{
		efCtrl = FindObjectOfType<EFCtrl>();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
