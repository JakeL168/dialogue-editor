using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DEUI_DialogueLink : MonoBehaviour, IPointerClickHandler
{
	public Text contentText;

	public void UpdateContent ( string _content )
	{
		contentText.text = _content;
	}

	public System.Action<DEUI_DialogueLink> OnClickLink;

	public void OnPointerClick ( PointerEventData _event )
	{
		if ( OnClickLink != null )
			OnClickLink ( this );
	}

}
