using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlockSkill : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string idSkill;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.instance.skillTreeController.Unlock(idSkill);
        GameManager.instance.uiManager.ToggleSkillTreeUI();
    }
}
