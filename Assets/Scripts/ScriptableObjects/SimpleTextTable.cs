using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Localization/SimpleTextTable")]
public class SimpleTextTable : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public string key;
        public string es;
        public string en;
        public string de;
        public string it;
        public string fi;
    }

    public List<Entry> entries = new List<Entry>();

    public string Get(string key, string lang)
    {
        var e = entries.Find(x => x.key == key);
        if (e == null) return key;

        switch (lang)
        {
            case "en": return string.IsNullOrEmpty(e.en) ? Fallback(e) : e.en;
            case "de": return string.IsNullOrEmpty(e.de) ? Fallback(e) : e.de;
            case "it": return string.IsNullOrEmpty(e.it) ? Fallback(e) : e.it;
            case "fi": return string.IsNullOrEmpty(e.fi) ? Fallback(e) : e.fi;
            default: return string.IsNullOrEmpty(e.es) ? key : e.es;
        }
    }

    private string Fallback(Entry e)
    {
        return string.IsNullOrEmpty(e.es) ? e.key : e.es;
    }
}
