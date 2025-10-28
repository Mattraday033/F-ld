using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IButtonEvaluationScript
{
	
	public void evaluate(FloorButtonTrueFalse[] buttons);
	
}

public class ButtonEvaluator : MonoBehaviour
{
	//[SerializeField]
	public IButtonEvaluationScript script;
	public FloorButtonTrueFalse[] buttons;
	
	void Start()
	{
		script = GetComponent<IButtonEvaluationScript>();
		declareButton();
	}
	
	private void declareButton()
	{
		// AreaManager.getMovementManager().buttonEvaluators.Add(script,buttons);
	}
}
