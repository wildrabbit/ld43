
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAltarView: MonoBehaviour
{
    public RectTransform _itemViewPrefab;
    ItemConfig[] _configs;
    public int SelectedIndex;
    public int NumItems => _configs.Length;

    RectTransform[] _items;
    RectTransform _itemContainer;
    Image[] _images;
    TextMeshProUGUI[] _title;

    Image _selected;
    TextMeshProUGUI _selectedDescription;
    Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _itemContainer = transform.Find("ItemContainer") as RectTransform;
    }
    public void Show(ItemConfig[] items, int defaultIndex)
    {
        gameObject.SetActive(true);
        _canvas.worldCamera = Camera.current;
        Select(defaultIndex);

    }

    public void Hide(bool destroy)
    {
        if (destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void Select(int idx)
    {
        SelectedIndex = idx;
        if(idx < 0)
        {
            _selected?.gameObject.SetActive(false);
        }
        else
        {
            _selected?.gameObject.SetActive(true);
            _selected?.transform.SetParent(_items[idx]);
            if(_selectedDescription != null)
                _selectedDescription.text = _configs[idx].Desc;
        }
        // add effects to descriptions?
    }
}