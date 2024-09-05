[System.Serializable]
public class PlayerStats
{
    public double energy;
    public double karma;
    public double spirituality;
    public double experience;

    public double minEnergy = 0;
    public double minKarma = 0;
    public double minSpirituality = 0;
    public double minExperience = 0;

    public double maxEnergy = 100;
    public double maxKarma = 100;
    public double maxSpirituality = 100;
    public double maxExperience = 100;
}
