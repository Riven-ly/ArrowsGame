using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MultilingualSpriteInfo
{
    public MultilingualType Key; 
    public Sprite Value;
    public UnityEngine.Vector3 vector3;
}

public class MultilingualSprite : MonoBehaviour
{
    public List<MultilingualSpriteInfo> spriteList;
    private Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = transform.GetComponent<Image>();
        UpdateText();
    }

    public void UpdateText()
    {
        if (img == null)
        {
            return;
        }
        foreach (var info in spriteList)
        {
            if(info.Key == LanguageManager.Instance.type && info.Value != null)
            {
                img.sprite = info.Value;
                img.SetNativeSize();
                img.transform.localPosition = info.vector3;
                break;
            }
        }
    }
}
