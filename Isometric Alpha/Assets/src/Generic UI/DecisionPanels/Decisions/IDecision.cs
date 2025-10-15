using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IDecision
{
	public string getMessage();
 
	public void execute();
 
	public void backOut();
}
