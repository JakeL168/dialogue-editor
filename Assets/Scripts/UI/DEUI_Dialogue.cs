using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DEUI_Dialogue : MonoBehaviour
{
	public Text speakerText;
	public Text contentText;
	public Transform linkUIParent;

	private List<DEUI_DialogueLink> linkUIs;
	private float timer;

	private DE_DialogueDataHandler _activeData;
	public DE_DialogueDataHandler activeData
	{
		get { return _activeData; }
		set
		{
//			if ( _activeData == value )
//				return;

			_activeData = value;
			if ( _activeData != null )
			{
				enabled = true;
				curDialogue = activeData.firstDialogue;
			}
		}
	}

	private DE_DialogueController _curDialogue;
	private DE_DialogueController curDialogue
	{
		get { return _curDialogue; }
		set
		{
			if ( _curDialogue == value )
				return;
			
			_curDialogue = value;
			if ( _curDialogue != null )
			{
				speakerText.text = _curDialogue.dialogueData.speaker;
				contentText.text = _curDialogue.dialogueData.content;
				linksDisplayed = false;
				// code : reset UI here
				if ( linkUIs != null )
				{
					foreach ( DEUI_DialogueLink _link in linkUIs )
						_link.gameObject.SetActive ( false );
				}
				_curDialogue.OnDialogueTrigger = OnDialogueTrigger;

				timer = 0.05f;
			} 
			else
				enabled = false;
		}
	}
	private bool linksDisplayed = false;
	private bool dHasLinks
	{
		get { return ( curDialogue.links != null && curDialogue.links.Length > 1 ); }
	}

	//--------------------------------------------------
	// public
	//--------------------------------------------------

	//--------------------------------------------------
	// listener
	//--------------------------------------------------
	private void OnClickLink ( DEUI_DialogueLink _link )
	{
		curDialogue = curDialogue.Next ( linkUIs.IndexOf ( _link ) );
	}

	private void OnDialogueTrigger ( string _trigger )
	{
		Debug.Log ( _trigger );
	}

	//--------------------------------------------------
	// private
	//--------------------------------------------------
	private void DisplayLinks ()
	{
		if ( linksDisplayed )
			return;

		linksDisplayed = true;

		if ( linkUIs == null )
			linkUIs = new List<DEUI_DialogueLink> ();

		DEUI_DialogueLink _linkBtn = ( DEUI_DialogueLink ) Resources.Load ( "link_button", typeof ( DEUI_DialogueLink ) );
		while ( linkUIs.Count < curDialogue.links.Length )
		{
			DEUI_DialogueLink _new = ( DEUI_DialogueLink ) Instantiate ( _linkBtn );
			_new.transform.SetParent ( linkUIParent );
			_new.transform.localScale = Vector3.one;
			linkUIs.Add ( _new );
		}

		for ( int a = 0; a < linkUIs.Count; ++a )
		{
			bool _canUse = ( a < curDialogue.links.Length );
			linkUIs [ a ].gameObject.SetActive ( _canUse );
			linkUIs [ a ].OnClickLink -= OnClickLink;
			if ( _canUse )
			{
				linkUIs [ a ].UpdateContent ( curDialogue.dialogueData.links [ a ].content );
				linkUIs [ a ].OnClickLink += OnClickLink;
			}
		}
	}

	//--------------------------------------------------
	// protected mono
	//--------------------------------------------------
	protected void Awake ()
	{
		enabled = false;
	}

	protected void OnDisable ()
	{
		foreach ( Transform _c in transform )
			_c.gameObject.SetActive ( false );
		
		activeData = null;
	}

	protected void OnEnable ()
	{
		foreach ( Transform _c in transform )
			_c.gameObject.SetActive ( true );
	}
	
	protected void Update ()
	{
		timer -= Time.deltaTime;
		if ( timer > 0f )
			return;

		if ( Input.GetKeyUp ( KeyCode.Mouse0 ) )
		{
			if ( dHasLinks )
				DisplayLinks ();
			else
				curDialogue = curDialogue.Next ();
		}
	}

}
