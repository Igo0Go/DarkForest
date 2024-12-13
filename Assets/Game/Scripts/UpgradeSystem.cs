using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject upgradePanel;
    [SerializeField]
    private GameObject iceUpgradeButton;
    [SerializeField]
    private GameObject fireUpgradeButton;
    [SerializeField]
    private GameObject sparksUpgradeButton;

    private void Awake()
    {
        ReturnAll();
    }

    public void ReturnAll()
    {
        SetActiveForCursor(false);
        Time.timeScale = 1.0f;

        sparksUpgradeButton.SetActive(false);
        fireUpgradeButton.SetActive(false);
        iceUpgradeButton.SetActive(false);

        upgradePanel.SetActive(false);
    }

    public void ActivatePanel()
    {
        if(!MagicStats.opportunityToFireUpgrade && 
            !MagicStats.opportunityToIceUpgrade && 
            !MagicStats.opportunityToSparksUpgrade)
        {
            return;
        }


        SetActiveForCursor(true);
        Time.timeScale = 0.0f;

        upgradePanel.SetActive(true);

        if (MagicStats.opportunityToIceUpgrade)
        {
            iceUpgradeButton.SetActive(true);
        }

        if (MagicStats.opportunityToFireUpgrade)
        {
            fireUpgradeButton.SetActive(true);
        }

        if (MagicStats.opportunityToSparksUpgrade)
        {
            sparksUpgradeButton.SetActive(true);
        }
    }

    public void UpgradeIce()
    {
        MagicStats.UpgradeIceSpeed();
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
