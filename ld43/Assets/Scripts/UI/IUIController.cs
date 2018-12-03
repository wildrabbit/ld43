using UnityEngine;
using System.Collections;

public interface IUIController
{
   Transform UIRoot { get; }

    void ShowAltarView(ItemConfig[] choices, int startIdx);

    UIAltarView GetAltarView();
    void HideAltarView(bool destroy= false);
    void SelectAltarChoice(int idx);
    void Setup(GameConfig gameConfig);
}
