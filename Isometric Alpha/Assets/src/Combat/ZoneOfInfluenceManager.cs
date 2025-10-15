using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ZoneOfInfluenceManager: MonoBehaviour
{
	private ActivatedPassiveTraitManager activatedPassiveTraitManager;
	private static ZoneOfInfluenceManager instance;
	
	
	public static ZoneOfInfluenceManager getInstance()
	{
		return instance;
	}
	
	private void Awake()
	{
		if(instance != null)
		{
			throw new IOException("Instance already exists for ZoneOfInfluenceManager");
		}
		
		instance = this;
	}
	
    // Start is called before the first frame update
    void Start()
    {
		activatedPassiveTraitManager = ActivatedPassiveTraitManager.getInstance();
		
        activatedPassiveTraitManager.addEquippedPassiveTraits();
		
		applyAllZOITraits();
    }

	public void applyAllZOITraits()
	{
		ArrayList listOfAllies = CombatGrid.getAllAliveAllyCombatants();
		
		foreach(Stats ally in listOfAllies)
		{
			Trait[] zoiTraits = getAllZOITraitsAtCoords(ally.position);
			
			ally.removeAllZoneOfInfluenceTraits();
			
			ally.addTraits(zoiTraits);
		}
	}

	
	private Trait[] getAllZOITraitsAtCoords(GridCoords coords)
	{
		ArrayList ZOITargets = CombatGrid.getAllZOITargets(coords); //used to get all creatures both within someone's ZOI
		Trait[] traits = new Trait[0];								//and also can be used to get all creatures providing ZOI
																	//benefits back to the target
		foreach(Stats target in ZOITargets)
		{
			if(target.getZoneOfInfluenceTrait() != null && !(target.getZoneOfInfluenceTrait() is null))
			{
				traits = Helpers.appendArray<Trait>(traits, target.getZoneOfInfluenceTrait());
			}
		}
		
		return traits;
	}


}
