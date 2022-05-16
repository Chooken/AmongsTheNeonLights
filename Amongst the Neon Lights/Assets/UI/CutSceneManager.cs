using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aarthificial.Reanimation;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;

public class CutSceneManager : MonoBehaviour
{
    #region VAR_DECLORATION

    [Serializable]
    public class DialogueSpeaker
    {
        public string name;
        public Sprite sprite;
        public bool isPlayer;
    }

    [Serializable]
    public class Dialogue
    {
        public string dialogue;
        public int speaker;
    }

    [Header("Player")]
    [SerializeField] private Transform playerCutscenePos;
    [SerializeField] private Reanimator playerAnimation;
    [SerializeField] private Image playerDialogueSprite;
    [SerializeField] private Text playerName;
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private Sprite playerIdle;

	[Header("Npc")]
	[SerializeField] private bool isNPC;
    [SerializeField] private GameObject npc;
    [SerializeField] private Image npcDialogueSprite;
    [SerializeField] private Text npcName;
    [SerializeField] private Transform npcCutscenePos;
    [SerializeField] private Reanimator npcAnimation;
    [SerializeField] private SpriteRenderer npcRenderer;

    [Header("General")]
    [SerializeField] private GameScript game;
    [SerializeField] private DialogueSpeaker[] speakers;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Text dialogueText;
    [SerializeField] private float typeSpeed = 0.2f;
    [SerializeField] private Button firstButton;

    [Header("Foreground")]
    [SerializeField] private VelocityScript[] foregroundObjects;

    [Header("Dialogue")]
    [SerializeField] private Dialogue[] startDialogueLines;
    [SerializeField] private Dialogue[] endDialogueLines;

    private int activeLine;
    private Dialogue[] sentences;
    private bool typingDialogue = false;
    private bool skip = false;
    private bool firstFrame;
    private bool isFinish = true;

    #endregion

    private void Start()
    {
        dialogueText.text = "";
        playerCollider.enabled = false;
    }

    // Sets firstupdate to false after update loop runs
    private void LateUpdate()
    {
        firstFrame = false;
    }

    public void StartCutscene(bool isStart)
    {
        isFinish = !isStart;

        if (isStart) StartCoroutine(MoveToStart());
        else StartCoroutine(MoveToFinish());
    }

    public void SkipToStart()
    {
        player.gameObject.transform.position = playerCutscenePos.position;
        playerCollider.enabled = true;
    }

    #region MOVETOCUTSCENE

    private IEnumerator MoveToStart()
    {
        player.enabled = false;
        float elapsedTime = 0f;
        float waitTime = 2f;

        Vector3 target = playerCutscenePos.position;
        Vector3 current = player.gameObject.transform.position;

        foreach (VelocityScript tree in foregroundObjects)
        {
            tree.enabled = false;
        }

        while (elapsedTime < waitTime)
        {
            player.gameObject.transform.position = Vector3.Lerp(current, target, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }

        // Make sure we got there
        player.gameObject.transform.position = target;
        playerAnimation.enabled = false;
        playerRenderer.sprite = playerIdle;
        DisplayDialogue();
    }

    private IEnumerator MoveToFinish()
    {
        player.enabled = false;
        playerCollider.enabled = false;
        if (isNPC) npc.SetActive(true);

        foreach (VelocityScript tree in foregroundObjects)
        {
            tree.enabled = false;
        }

        float elapsedTime = 0f;
        float waitTime = 2f;

        Vector3 target = playerCutscenePos.position;
        Vector3 current = player.gameObject.transform.position;

        Vector3 bossTarget = npcCutscenePos.position;
        Vector3 bossCurrent = npc.transform.position;


        while (elapsedTime < waitTime)
        {
            player.gameObject.transform.position = Vector3.Lerp(current, target, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

			if (isNPC)
			{
				npc.transform.position = Vector3.Lerp(bossCurrent, bossTarget, (elapsedTime / waitTime));
				elapsedTime += Time.deltaTime;
			}

            // Yield here
            yield return null;
        }

        // Make sure we got there
        player.gameObject.transform.position = target;
        npc.transform.position = bossTarget;
        playerAnimation.enabled = false;
        playerRenderer.sprite = playerIdle;
        DisplayDialogue();
    }

	private IEnumerator ExitScene()
	{
		float elapsedTime = 0f;
		float waitTime = 2f;

		Vector3 target = Vector3.up * 7;
		Vector3 current = player.gameObject.transform.position;

		if (isNPC)
		{
			Vector3 bossTarget = Vector3.up * 10;
			Vector3 bossCurrent = npc.transform.position;


			while (elapsedTime < waitTime)
			{
				npc.transform.position = Vector3.Lerp(bossCurrent, bossTarget, (elapsedTime / waitTime));
				elapsedTime += Time.deltaTime;

				// Yield here
				yield return null;
			}

			npc.transform.position = bossTarget;

			elapsedTime = 0f;
		}

		while (elapsedTime < waitTime)
		{
			player.gameObject.transform.position = Vector3.Lerp(current, target, (elapsedTime / waitTime));
			elapsedTime += Time.deltaTime;

			// Yield here
			yield return null;
		}

		// Make sure we got there
		player.gameObject.transform.position = target;

		game.GoToScene(4);
	}

	#endregion

	#region DIALOGUE

	public void DisplayDialogue()
    {
        firstFrame = true;

        dialogueBox.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);

        activeLine = 0;
        sentences = (!isFinish) ? startDialogueLines : endDialogueLines;

        StartCoroutine(WriteSentence());
    }

    public void NextSentence()
    {
        if (firstFrame || !dialogueBox.activeInHierarchy) return;

        if (typingDialogue) { skip = true; return; }

        dialogueText.text = "";

        if (speakers[sentences[activeLine].speaker].isPlayer)
        {
            playerDialogueSprite.enabled = false;
            playerName.enabled = false;
        }
        else
        {
            npcDialogueSprite.enabled = false;
            npcName.enabled = false;
        }

        if (activeLine == sentences.Length - 1)
        {
            dialogueBox.SetActive(false);
            player.enabled = true;
            playerAnimation.enabled = true;
            playerCollider.enabled = true;
            if (isFinish) StartCoroutine(ExitScene());
            else
            {
                foreach (VelocityScript tree in foregroundObjects)
                {
                    tree.enabled = true;
                }
                game.StartGame();
            }
            return;
        }

        activeLine++;

        StartCoroutine(WriteSentence());
    }

    private IEnumerator WriteSentence()
    {
        typingDialogue = true;

        if (speakers[sentences[activeLine].speaker].isPlayer)
        {
            playerDialogueSprite.enabled = true;
            playerName.enabled = true;
            playerName.text = speakers[sentences[activeLine].speaker].name;
            playerDialogueSprite.sprite = speakers[sentences[activeLine].speaker].sprite;
        }
        else
        {
            npcDialogueSprite.enabled = true;
            npcName.enabled = true;
            npcName.text = speakers[sentences[activeLine].speaker].name;
            npcDialogueSprite.sprite = speakers[sentences[activeLine].speaker].sprite;
        }

        foreach (char character in sentences[activeLine].dialogue.ToCharArray())
        {
            if (skip)
            {
                dialogueText.text = sentences[activeLine].dialogue;
                skip = false;
                typingDialogue = false;
                yield break;
            }
            dialogueText.text += character;
            yield return new WaitForSeconds(typeSpeed);
        }

        typingDialogue = false;
    }

    #endregion
}
