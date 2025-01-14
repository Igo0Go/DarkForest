using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField]
    private List<UpgradeItem> upgradeItems;

    [SerializeField]
    private GameObject upgradePanel;

    private void Awake()
    {
        ReturnAll();
    }

    public void ReturnAll()
    {
        SetActiveForCursor(false);
        Time.timeScale = 1.0f;
        upgradePanel.SetActive(false);
    }

    public void ActivatePanel()
    {
        SetActiveForCursor(true);
        Time.timeScale = 0.0f;

        upgradePanel.SetActive(true);

        foreach (var upgradeItem in upgradeItems)
        {
            upgradeItem.UpgradePanel.SetActive(false);
            if (MagicStats.availableSpells.Contains(upgradeItem.SpellType) && 
                MagicStats.GetOpportunityToUpgrade(upgradeItem.SpellType))
            {
                upgradeItem.UpgradePanel.SetActive(true);
            }
        }
    }

    public void UpgradeKinetick()
    {
        MagicStats.UpgradeKineticSpeed();
        ReturnAll();
    }

    public void UnlockSpell(int index)
    {
        MagicSpellType type = (MagicSpellType)index;


        if(!MagicStats.availableSpells.Contains(type))
        {
            MagicStats.availableSpells.Add(type);
        }
    }

    public void UpgradeIce()
    {
        MagicStats.UpgradeIceCount();
        ReturnAll();
    }

    public void UpgradeFire()
    {
        MagicStats.UpgradeFireSpeed();
        ReturnAll();
    }

    public void UpgradeSparks()
    {
        MagicStats.UpgradeSparksSpeed();
        ReturnAll();
    }

    private void SetActiveForCursor(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value? CursorLockMode.None : CursorLockMode.Locked;
    }
}

[System.Serializable]
public class UpgradeItem
{
    public MagicSpellType SpellType;
    public GameObject UpgradePanel;
}
