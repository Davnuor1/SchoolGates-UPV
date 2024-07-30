using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public PlayerStats playerStats;

    private void Awake()
    {
        playerStats = new PlayerStats(); // Inicializa las estadísticas del jugador
    }

    public void ModifyEnergy(int amount)
    {
        playerStats.energy = Mathf.Clamp(playerStats.energy + amount, playerStats.minEnergy, playerStats.maxEnergy);
    }

    public void ModifyKarma(int amount)
    {
        playerStats.karma = Mathf.Clamp(playerStats.karma + amount, playerStats.minKarma, playerStats.maxKarma);
    }

    public void ModifySpirituality(int amount)
    {
        playerStats.spirituality = Mathf.Clamp(playerStats.spirituality + amount, playerStats.minSpirituality, playerStats.maxSpirituality);
    }

    public void ModifyExperience(int amount)
    {
        playerStats.experience = Mathf.Clamp(playerStats.experience + amount, playerStats.minExperience, playerStats.maxExperience);
    }

    public void SetEnergy(int amount)
    {
        playerStats.energy = Mathf.Clamp(amount, playerStats.minEnergy, playerStats.maxEnergy);
    }

    public void SetKarma(int amount)
    {
        playerStats.karma = Mathf.Clamp(amount, playerStats.minKarma, playerStats.maxKarma);
    }

    public void SetSpirituality(int amount)
    {
        playerStats.spirituality = Mathf.Clamp(amount, playerStats.minSpirituality, playerStats.maxSpirituality);
    }

    public void SetExperience(int amount)
    {
        playerStats.experience = Mathf.Clamp(amount, playerStats.minExperience, playerStats.maxExperience);
    }
}
