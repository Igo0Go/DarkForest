using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUICenter : MonoBehaviour
{
    [SerializeField]
    private Slider magicSlider;
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private GameObject deadPanel;
    [SerializeField]
    private Image damagePanel;
    [SerializeField]
    private Slider rageSlider;
    [SerializeField]
    private TMP_Text rageMultiplicatorText;
    [SerializeField]
    private GameObject pausePanel;

    private bool pause;

    public void Init()
    {
        PlayerMagic playerMagic = FindObjectOfType<PlayerMagic>();
        playerMagic.MagicMaxValueChanget += SetMaxValueForMagicSlider;
        playerMagic.MagicValueChanget+= SetValueForMagicSlider;

        PlayerInteraction playerInteraction = FindObjectOfType<PlayerInteraction>();
        playerInteraction.HPMaxValueChanget += SetMaxValueForHPSlider;
        playerInteraction.HPValueChanget += SetValueForHPSlider;
        playerInteraction.DeadEvent += OnDead;
        playerInteraction.DamageValueChanged += SetAlphaForDamagePanel;

        MusicRageSystem musicRageSystem = FindObjectOfType<MusicRageSystem>();
        musicRageSystem.RageInfoChanged += OnRageInfoChanged;
        musicRageSystem.Init();

        deadPanel.SetActive(false);
        rageSlider.value = 0;

        FindObjectOfType<SettingsPanel>().Init();

        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            pause = !pause;
            SetPause(pause);
        }
    }

    public void SetPause(bool value)
    {
        pause = value;
        if (value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
    }

    private void SetMaxValueForMagicSlider(float value)
    {
        magicSlider.maxValue = value;
    }
    private void SetValueForMagicSlider(float value)
    {
        magicSlider.value = value;
    }

    private void SetMaxValueForHPSlider(float value)
    {
        hpSlider.maxValue = value;
    }
    private void SetValueForHPSlider(float value)
    {
        hpSlider.value = value;
    }

    private void OnDead()
    {
        deadPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void SetAlphaForDamagePanel(float alpha)
    {
        damagePanel.color = new Color(damagePanel.color.r, damagePanel.color.g, damagePanel.color.b, alpha);
    }


    private void OnRageInfoChanged(float value, float min, float max, int multiplicator)
    {
        rageMultiplicatorText.text = "X" + multiplicator.ToString();
        rageSlider.maxValue = max;
        rageSlider.minValue = min;
        rageSlider.value = value;
    }
}
