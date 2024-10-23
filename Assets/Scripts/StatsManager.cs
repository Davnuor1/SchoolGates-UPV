using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;

public class StatsManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public StatFeedbackManager statFeedbackManager;


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
        Lua.RegisterFunction("ModifyEnergy", this, SymbolExtensions.GetMethodInfo(() => ModifyEnergy(0.0)));
        Lua.RegisterFunction("ModifyKarma", this, SymbolExtensions.GetMethodInfo(() => ModifyKarma(0.0)));
        Lua.RegisterFunction("ModifySpirituality", this, SymbolExtensions.GetMethodInfo(() => ModifySpirituality(0.0)));
        Lua.RegisterFunction("ModifyExperience", this, SymbolExtensions.GetMethodInfo(() => ModifyExperience(0.0)));
        Lua.RegisterFunction("SetEnergy", this, SymbolExtensions.GetMethodInfo(() => SetEnergy(0.0)));
        Lua.RegisterFunction("SetKarma", this, SymbolExtensions.GetMethodInfo(() => SetKarma(0.0)));
        Lua.RegisterFunction("SetSpirituality", this, SymbolExtensions.GetMethodInfo(() => SetSpirituality(0.0)));
        Lua.RegisterFunction("SetExperience", this, SymbolExtensions.GetMethodInfo(() => SetExperience(0.0)));
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
    private double Clamp(double value, double min, double max)
    {
        return Math.Max(min, Math.Min(max, value));
    }
    public void ModifyEnergy(double amount)
    {
        double oldValue = playerStats.energy;
        playerStats.energy = Clamp(playerStats.energy + amount, playerStats.minEnergy, playerStats.maxEnergy);

        // Mostrar el feedback si hubo un cambio
        if (amount != 0 && statFeedbackManager != null)
        {
            statFeedbackManager.ShowFeedback("Energy", amount);
        }
    }

    public void ModifyKarma(double amount)
    {
        double oldValue = playerStats.karma;
        playerStats.karma = Clamp(playerStats.karma + amount, playerStats.minKarma, playerStats.maxKarma);

        // Mostrar el feedback si hubo un cambio
        if (amount != 0 && statFeedbackManager != null)
        {
            statFeedbackManager.ShowFeedback("Karma", amount);
        }
    }

    public void ModifySpirituality(double amount)
    {
        double oldValue = playerStats.spirituality;
        playerStats.spirituality = Clamp(playerStats.spirituality + amount, playerStats.minSpirituality, playerStats.maxSpirituality);

        // Mostrar el feedback si hubo un cambio
        if (amount != 0 && statFeedbackManager != null)
        {
            statFeedbackManager.ShowFeedback("Spirituality", amount);
        }
    }

    public void ModifyExperience(double amount)
    {
        double oldValue = playerStats.experience;
        playerStats.experience = Clamp(playerStats.experience + amount, playerStats.minExperience, playerStats.maxExperience);
        // Mostrar el feedback si hubo un cambio
        if (amount != 0 && statFeedbackManager != null)
        {
            statFeedbackManager.ShowFeedback("Experience", amount);
        }
    }

    public void SetEnergy(double amount)
    {
        playerStats.energy = Clamp(amount, playerStats.minEnergy, playerStats.maxEnergy);
    }

    public void SetKarma(double amount)
    {
        playerStats.karma = Clamp(amount, playerStats.minKarma, playerStats.maxKarma);
    }

    public void SetSpirituality(double amount)
    {
        playerStats.spirituality = Clamp(amount, playerStats.minSpirituality, playerStats.maxSpirituality);
    }

    public void SetExperience(double amount)
    {
        playerStats.experience = Clamp(amount, playerStats.minExperience, playerStats.maxExperience);
    }
    public double GetEnergy()
    {
        return playerStats.energy;
    }

    public double GetKarma()
    {
        return playerStats.karma;
    }

    public double GetSpirituality()
    {
        return playerStats.spirituality;
    }

    public double GetExperience()
    {
        return playerStats.experience;
    }
}
