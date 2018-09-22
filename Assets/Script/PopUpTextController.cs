using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpTextController : MonoBehaviour {

    private static PopUpText popUpText;
    public static GameObject canvas;

    public static void Initialize()
    {
        if (!popUpText)
            popUpText = Resources.Load<PopUpText>("Prefabs/Visual Effect/DamagePopUpPosition");
        canvas = GameObject.Find("DamagePopUpCanvas");
    }

	public static void CreatePopUpText(string text, Transform location)
    {
        PopUpText instance = Instantiate(popUpText);
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = instance.transform.position + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
        instance.SetText(text);
    }
}
