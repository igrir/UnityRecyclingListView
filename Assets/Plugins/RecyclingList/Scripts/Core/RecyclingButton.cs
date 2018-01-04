using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RecyclingButton : MonoBehaviour
{

    public Text CharText;

    public RecyclingModel Model;

    Button _Button;
    public delegate void _OnClick();
    public _OnClick OnClick;

    // Use this for initialization
    void Start()
    {
        _Button = GetComponent<Button>();

        _Button.onClick.AddListener(() => {
            if (OnClick != null)
            {
                OnClick();
            }
        });
    }

    public void Disable()
    {
        _Button.enabled = false;
    }

    public void Enable()
    {
        _Button.enabled = true;
    }
    

    public void SetText(string text)
    {
        CharText.text = text;
    }
}

