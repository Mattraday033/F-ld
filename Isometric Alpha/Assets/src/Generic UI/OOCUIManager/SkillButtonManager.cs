using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonManager : MonoBehaviour
{
    private static SkillButtonManager instance;

    public static SkillButtonManager getInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    public Button skillButton;

    public Image skillButtonOutline;

    private void OnEnable()
    {
        PlayerOOCStateManager.OnStateChangeToSkill.AddListener(highlightSkillOutline);
        PlayerOOCStateManager.OnStateChangeFromSkill.AddListener(unhighlightSkillOutline);
    }

    private void OnDisable()
    {
        PlayerOOCStateManager.OnStateChangeToSkill.RemoveListener(highlightSkillOutline);
        PlayerOOCStateManager.OnStateChangeFromSkill.RemoveListener(unhighlightSkillOutline);
    }

    public static void highlightSkillOutline()
    {
        if(instance == null)
        {
            return;
        }

        instance.skillButtonOutline.color = ColorList.skillButtonOutlineHighlight;
    }

    public static void unhighlightSkillOutline()
    {
        if(instance == null)
        {
            return;
        }

        instance.skillButtonOutline.color = ColorList.grey25;
    }

    public void changeCurrentSkill(bool descending)
    {
        changeSkill(descending);
    }

    public static void changeSkill(bool descending)
    {
        switch(State.currentSkillType)
        {
            case SkillType.Cunning:
                if(descending)
                {
                    State.currentSkillType = SkillType.Observation;
                } else
                {
                    State.currentSkillType = SkillType.Intimidate;
                }
                break;
            case SkillType.Observation:
                if(descending)
                {
                    State.currentSkillType = SkillType.Leadership;
                } else
                {
                    State.currentSkillType = SkillType.Cunning;
                }
                break;
            case SkillType.Leadership:
                if(descending)
                {
                    State.currentSkillType = SkillType.Intimidate;
                } else
                {
                    State.currentSkillType = SkillType.Observation;
                }
                break;
            default:
                if(descending)
                {
                    State.currentSkillType = SkillType.Cunning;
                } else
                {
                    State.currentSkillType = SkillType.Leadership;
                }
                break;
        }

        if(!hasSkill(State.currentSkillType))
        {
            changeSkill(descending);
        } else
        {
            SkillManager.OnSkillUse.Invoke();
        }
    }

    public static void setToSkill(SkillType skillType)
    {
        State.currentSkillType = skillType;

        SkillManager.OnSkillUse.Invoke();
    }

    private static bool hasSkill(SkillType skillType)
    {
        switch(State.currentSkillType)
        {
            case SkillType.Cunning:
                return PartyStats.getMaxCunningCount() > 0;
            case SkillType.Observation:
                return PartyStats.getObservationLevel() > 0;
            case SkillType.Leadership:
                return PartyStats.getMaxPlacablePartyMembers() > 0;
            default:
                return PartyStats.getMaxIntimidateCount() > 0;
        }
    }

    public void setSkillButtonInteractability()
    {
        switch(State.currentSkillType)
        {
            case SkillType.Cunning:
                skillButton.interactable = CunningManager.getCunningsRemaining() > 0;
                break;
            case SkillType.Observation:
                skillButton.interactable = true;
                break;
            case SkillType.Leadership:
                skillButton.interactable = PartyMemberPlacer.getPlacedPartyMemberCount() < PartyStats.getMaxPlacablePartyMembers();
                break;
            default:
                skillButton.interactable = IntimidateManager.getIntimidatesRemaining() > 0;
                break;
        }
    }

    public void useCurrentSkill()
    {
        useSkill();
    }

    public static void useSkill()
    {
        switch(State.currentSkillType)
        {
            case SkillType.Cunning:
                useCunning();
                return;
            case SkillType.Observation:
                useObservation();
                return;
            case SkillType.Leadership:
                useLeadership();
                return;
            default:
                useIntimidate();
                return;
        }
    }


    private static void useIntimidate()
    {
        if(!hasSkill(SkillType.Intimidate))
        {
            return;
        }

        if (PlayerOOCStateManager.currentActivity != OOCActivity.intimidating)
        {
            IntimidateManager.enterIntimidateMode();
        }
        else
        {
            IntimidateManager.leaveIntimidateMode();            
        }
    }

    private static void useCunning()
    {
        if(!hasSkill(SkillType.Cunning))
        {
            return;
        }


        if (PlayerOOCStateManager.currentActivity != OOCActivity.cunning)
        {
            CunningManager.enterCunningMode(); 
        }
        else
        {
            CunningManager.leaveCunningMode();            
        }
    }

    private static void useObservation()
    {
        if(!hasSkill(SkillType.Observation))
        {
            return;
        }

        if (PlayerOOCStateManager.currentActivity != OOCActivity.observing)
        {
            ObservationManager.enterObservationMode();            
        }
        else
        {
            ObservationManager.leaveObservationMode();            
        }
    }


    private static void useLeadership()
    {
        if(!hasSkill(SkillType.Leadership))
        {
            return;
        }

        if (PartyMemberPlacer.getPlacedPartyMemberCount() < PartyStats.getMaxPlacablePartyMembers())
		{
			PartyMemberPlacer.placeNextPartyMember();
		}
    }
}
