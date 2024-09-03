using UnityEngine;
using PixelCrushers.DialogueSystem;

public class StatsManager : MonoBehaviour
{
    public PlayerStats playerStats;

    private void Awake()
    {
        playerStats = new PlayerStats(); // Inicializa las estadísticas del jugador
        RegisterFunctions();
    }

    private void OnDestroy()
    {
        UnregisterFunctions();
    }

    #region Lua Registration

    private void RegisterFunctions()
    {
        Lua.RegisterFunction("ModifyEnergy", this, SymbolExtensions.GetMethodInfo(() => ModifyEnergy(0)));
        Lua.RegisterFunction("ModifyKarma", this, SymbolExtensions.GetMethodInfo(() => ModifyKarma(0)));
        Lua.RegisterFunction("ModifySpirituality", this, SymbolExtensions.GetMethodInfo(() => ModifySpirituality(0)));
        Lua.RegisterFunction("ModifyExperience", this, SymbolExtensions.GetMethodInfo(() => ModifyExperience(0)));
        Lua.RegisterFunction("SetEnergy", this, SymbolExtensions.GetMethodInfo(() => SetEnergy(0)));
        Lua.RegisterFunction("SetKarma", this, SymbolExtensions.GetMethodInfo(() => SetKarma(0)));
        Lua.RegisterFunction("SetSpirituality", this, SymbolExtensions.GetMethodInfo(() => SetSpirituality(0)));
        Lua.RegisterFunction("SetExperience", this, SymbolExtensions.GetMethodInfo(() => SetExperience(0)));
        Lua.RegisterFunction("GetEnergy", this, SymbolExtensions.GetMethodInfo(() => GetEnergy()));
        Lua.RegisterFunction("GetKarma", this, SymbolExtensions.GetMethodInfo(() => GetKarma()));
        Lua.RegisterFunction("GetSpirituality", this, SymbolExtensions.GetMethodInfo(() => GetSpirituality()));
        Lua.RegisterFunction("GetExperience", this, SymbolExtensions.GetMethodInfo(() => GetExperience()));
    }

    private void UnregisterFunctions()
    {
        Lua.UnregisterFunction("ModifyEnergy");
        Lua.UnregisterFunction("ModifyKarma");
        Lua.UnregisterFunction("ModifySpirituality");
        Lua.UnregisterFunction("ModifyExperience");
        Lua.UnregisterFunction("SetEnergy");
        Lua.UnregisterFunction("SetKarma");
        Lua.UnregisterFunction("SetSpirituality");
        Lua.UnregisterFunction("SetExperience");
        Lua.UnregisterFunction("GetEnergy");
        Lua.UnregisterFunction("GetKarma");
        Lua.UnregisterFunction("GetSpirituality");
        Lua.UnregisterFunction("GetExperience");
    }

    #endregion

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
    public int GetEnergy()
    {
        return playerStats.energy;
    }

    public int GetKarma()
    {
        return playerStats.karma;
    }

    public int GetSpirituality()
    {
        return playerStats.spirituality;
    }

    public int GetExperience()
    {
        return playerStats.experience;
    }
}
