using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour, IUIController
{
    public Transform UIRoot { get { return transform; } }

    UIAltarView _altarView;
    GameConfig _config;

    public void Setup(GameConfig config)
    {
        _config = config;
        _altarView = null;
    }

    public UIAltarView GetAltarView()
    {
        return _altarView;
    }

    public void ShowAltarView(ItemConfig[] choices, int startIdx)
    {
        if(_altarView == null)
        {
            _altarView = Instantiate<UIAltarView>(_config.AltarUIPrefab);
            _altarView.transform.SetParent(UIRoot);
        }
        _altarView.Show(choices, startIdx);
    }

    public void HideAltarView(bool destroy = false)
    {
        if(_altarView != null)
        {
            _altarView.Hide(destroy);
            if(destroy)
            {
                _altarView = null;
            }
        }
    }

    public void SelectAltarChoice(int idx)
    {
        if(_altarView)
            _altarView.Select(idx);
    }
}
