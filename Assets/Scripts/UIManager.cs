using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Player
    public Text hpText;
    public Text attackDelayText;
    public Image hpBar;
    public Image skilIcon;
    public Image skillBar;

    private static UIManager instance = null;


    public void SetPlayerHpImage(int maxHp, int currentHp)
    {
        hpBar.fillAmount = (currentHp / (float)maxHp);
    }

    public void ChangePlayerSkillImage(CurrentSkill currentSkill)
    {
        switch (currentSkill)
        {
            case CurrentSkill.timeStop:
                skilIcon.sprite = ResourceManager.Instance.SkillIconResources[2];
                break;

            case CurrentSkill.timeSlow:
                if(ResourceManager.Instance.SkillIconResources[1] != null)

                skilIcon.sprite = ResourceManager.Instance.SkillIconResources[1];
                break;

            case CurrentSkill.timeFast:
                skilIcon.sprite = ResourceManager.Instance.SkillIconResources[0];
                break;
        }
    }

    public void SetPlayerSkillImage(int maxSkillCool, int currentSkillCool)
    {
        skillBar.fillAmount = (currentSkillCool / (float)maxSkillCool);
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static UIManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {
    }
}
