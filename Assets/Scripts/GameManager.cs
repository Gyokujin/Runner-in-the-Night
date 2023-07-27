using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum StageType
    {
        run, battle
    }

    public static GameManager instance = null;
    StageType stageType;

    [Header("AnimatorType")]
    [SerializeField]
    private AnimatorController runAnimator;
    [SerializeField]
    private AnimatorController battleAnimator;

    [Header("Component")]
    [SerializeField]
    private PlayerController player;

    void Awake()
    {
        if (instance == null)
        {
            instance = null;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        stageType = StageType.run;
        ChangeAnimator();
    }

    void ChangeAnimator()
    {
        Animator playerAnimator = player.GetComponent<Animator>();

        switch (stageType)
        {
            case StageType.run:
                playerAnimator.runtimeAnimatorController  = runAnimator;
                break;
            case StageType.battle:
                playerAnimator.runtimeAnimatorController = battleAnimator;
                break;
        }
    }
}