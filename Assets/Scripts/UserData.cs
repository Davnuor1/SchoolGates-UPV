using System;

[Serializable]
public class UserData
{
    public string tan;
    public string password;

    public float totalPlayTime;     // tiempo total de juego
    public int timesGameOpened;     // veces que se ha iniciado sesión/juego

    [Serializable]
    public class GateTimeEntry
    {
        public string gateId;
        public float seconds;       // segundos acumulados en ese gate
    }

    public GateTimeEntry[] gateTimes = new GateTimeEntry[0]; // tiempos por gate
    public string[] completedGates = new string[0];          // gates completados (sin duplicados)
    public string[] finalsChosen = new string[0];            // finales elegidos (histórico)

    public int miniquestsCompletedCache = 0;                 // copia opcional desde DS

    // Stats para los finales (se rellenarán cuando montemos el stats system)
    public int experiencePoints = 0;
    public int integrityPoints = 0;
    public int positivePresencePoints = 0;
    public string languageCode = "es"; // valor por defecto que quieras (es/it/en/de/fi)
    // MUY IMPORTANTE: conservamos el snapshot del Dialogue System
    public string dialogueSystemSaveData;
    public string[] unlockedSkills = new string[0]; // ids de skills desbloqueadas

}
