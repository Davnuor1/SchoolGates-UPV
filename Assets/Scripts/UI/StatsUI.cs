using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI : MonoBehaviour
{
    public StatsManager statsManager;

    public Image energyIcon;
    public Slider energyBar;
    public TextMeshProUGUI energyValueTMP;

    public Image karmaIcon;
    public Slider karmaBar;
    public TextMeshProUGUI karmaValueTMP;

    public Image spiritualityIcon;
    public Slider spiritualityBar;
    public TextMeshProUGUI spiritualityValueTMP;

    public Image experienceIcon;
    public Slider experienceBar;
    public TextMeshProUGUI experienceValueTMP;

    private void Start()
    {
        // Asegúrate de que el StatsManager esté referenciado correctamente
        if (statsManager == null)
        {
            statsManager = GameManager.instance.statsManager;
        }

        // Configurar los valores máximos de las barras según el máximo de cada estadística
        energyBar.maxValue = (float)statsManager.playerStats.maxEnergy;
        karmaBar.maxValue = (float)statsManager.playerStats.maxKarma;
        spiritualityBar.maxValue = (float)statsManager.playerStats.maxSpirituality;
        experienceBar.maxValue = (float)statsManager.playerStats.maxExperience;
    }

    private void Update()
    {
        // Actualizar los valores de las barras y los textos en tiempo real
        energyBar.value = (float)statsManager.GetEnergy();
        energyValueTMP.text = statsManager.GetEnergy().ToString();

        karmaBar.value = (float)statsManager.GetKarma();
        karmaValueTMP.text = statsManager.GetKarma().ToString();

        spiritualityBar.value = (float)statsManager.GetSpirituality();
        spiritualityValueTMP.text = statsManager.GetSpirituality().ToString();

        experienceBar.value = (float)statsManager.GetExperience();
        experienceValueTMP.text = statsManager.GetExperience().ToString();
    }
}
