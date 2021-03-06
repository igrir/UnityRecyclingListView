﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecyclingListController : MonoBehaviour
{

    ScrollRect _ScrollRect;

    RecyclingButton PrefabButton;
    RectTransform PrefabRT;

    RectTransform _ScrollAreaRT;
    RectTransform _ContainerRT;

    private int _DataCount = 0;
    private int _PrefabCount = 0;
    private List<RecyclingButton> Buttons = new List<RecyclingButton>();
    private List<RecyclingModel> _LabelData = new List<RecyclingModel>();

    private RectTransform _RTContainerTopPos;
    private RectTransform _RTContainerBottomPos;

    private int _TopIndex;
    private int _PrefabTopIndex;
    private Coroutine _UpdatingRoutine;
    private bool initialized = false;

    public delegate void _OnRecyclingButton(RecyclingButton button);
    public _OnRecyclingButton OnButtonClicked;
    public _OnRecyclingButton OnDataSet;


    public void SetLabelData(List<RecyclingModel> labelData)
    {
        this._LabelData = labelData;
        if (labelData != null)
            this._DataCount = labelData.Count;

        Init();
    }

    public void Init()
    {
        if (!initialized)
        {
            GetReferences();
            GetPrefab();
            CalculateShownPrefabs();
            FillPrefabs();
        }

        ResizeContainer();

        if (_UpdatingRoutine != null)
            StopCoroutine(_UpdatingRoutine);
        _UpdatingRoutine = StartCoroutine(UpdatingButtonPosition());

        initialized = true;
    }

    //TODO: Masih belum dibuat, heheh.
    public void Reset()
    {
        Buttons.Clear();
    }

    IEnumerator UpdatingButtonPosition()
    {
        while (true)
        {
            UpdateButtonPositions();
            yield return null;
        }
    }

    private void UpdateButtonPositions()
    {
        if (_DataCount <= 0)
            RestackButtons(0, 0);

        _TopIndex = (int)(_ContainerRT.localPosition.y / PrefabRT.sizeDelta.y);
        if (_TopIndex >= 0 && _TopIndex < _DataCount)
        {
            _PrefabTopIndex = _TopIndex % _PrefabCount;

            RestackButtons(_PrefabTopIndex, _TopIndex);
        }

    }

    public void StopUpdatingButtonPosition()
    {
        StopCoroutine(_UpdatingRoutine);
        _UpdatingRoutine = null;
    }

    private void RestackButtons(int index, int dataIndex)
    {
        int itButtons = 0;
        int currentButtonIndex = index;
        while (itButtons < Buttons.Count)
        {

            if (currentButtonIndex > Buttons.Count - 1)
            {
                currentButtonIndex = 0;
            }
            RecyclingButton currentButton = Buttons[currentButtonIndex];

            // set visible
            if (dataIndex < _DataCount)
            {
                currentButton.gameObject.SetActive(true);

                // set position
                RectTransform currentButtonRT = currentButton.GetComponent<RectTransform>();
                currentButtonRT.localPosition = new Vector3(
                    currentButtonRT.localPosition.x,
                    (-itButtons * PrefabRT.sizeDelta.y) + (-_TopIndex * PrefabRT.sizeDelta.y),
                    currentButtonRT.localPosition.z);

                // set content
                SetButtonContent(currentButton, dataIndex);

            }
            else
            {
                currentButton.gameObject.SetActive(false);
            }

            currentButtonIndex++;
            dataIndex++;
            itButtons++;
        }
    }

    private void SetButtonContent(RecyclingButton currentButton, int dataIndex)
    {
        if (_LabelData == null || _LabelData.Count <= dataIndex)
            return;

        // set data
        currentButton.Model = _LabelData[dataIndex];

        // set text
        string buttonLabel = _LabelData[dataIndex].Label;
        currentButton.SetText(buttonLabel);

        if (OnDataSet != null)
        {
            if (OnDataSet != null)
                OnDataSet(currentButton);
        }

        currentButton.OnClick = delegate
        {
            if (OnButtonClicked != null)
                OnButtonClicked(currentButton);
        };

    }

    private void GetReferences()
    {
        _ScrollRect = GetComponent<ScrollRect>();
        _ContainerRT = _ScrollRect.content;
        _ScrollAreaRT = GetComponent<RectTransform>();
    }

    GameObject GetWorldPosInRect(float x, float y)
    {
        // Steal world top position
        GameObject ruler = new GameObject();
        RectTransform RTRuler = ruler.AddComponent<RectTransform>();
        RTRuler.transform.SetParent(this.transform.parent);

        RTRuler.sizeDelta = new Vector2(0, 0);
        RTRuler.pivot = new Vector2(0, 0);
        RTRuler.anchorMin = new Vector2(0, 1);
        RTRuler.anchorMax = new Vector2(1, 1);

        RTRuler.anchoredPosition = new Vector2(x, y);

        return ruler;

    }

    private void GetPrefab()
    {
        PrefabButton = GetComponentInChildren<RecyclingButton>();
        PrefabRT = PrefabButton.GetComponent<RectTransform>();
        Buttons.Add(PrefabButton);
    }

    private void FillPrefabs()
    {
        for (int i = 0; i < _PrefabCount - 1; i++)
        {
            RecyclingButton newTextButton = Instantiate(PrefabButton, _ContainerRT);
            RectTransform rt = newTextButton.GetComponent<RectTransform>();

            rt.localScale = PrefabRT.localScale;
            rt.sizeDelta = PrefabRT.sizeDelta;
            rt.transform.position = PrefabRT.transform.position;
            rt.anchoredPosition = new Vector2(0, (i + 1) * -PrefabRT.sizeDelta.y);

            Buttons.Add(newTextButton);
        }

    }

    private void CalculateShownPrefabs()
    {
        _PrefabCount = (int)Mathf.Ceil(_ScrollAreaRT.sizeDelta.y / PrefabRT.sizeDelta.y) + 1;
    }

    private void ResizeContainer()
    {
        _ContainerRT.sizeDelta = new Vector2(_ContainerRT.sizeDelta.x, (_DataCount * PrefabRT.sizeDelta.y) - (_ScrollAreaRT.sizeDelta.y));
    }

    public void SetTexts(string[] texts)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            string currentText = texts[i];
            if (Buttons.Count > i)
            {
                Buttons[i].SetText(currentText);
            }
        }
    }

    public void JumpToIndex(int index)
    {
        _ContainerRT.localPosition = new Vector3(
            _ContainerRT.localPosition.x,
            index * PrefabRT.sizeDelta.y,
            _ContainerRT.localPosition.z
        );
    }

    public void Disable()
    {
        foreach (RecyclingButton btn in Buttons)
        {
            btn.Disable();
        }
    }

    public void Enable()
    {
        foreach (RecyclingButton btn in Buttons)
        {
            btn.Enable();
        }
    }
}
