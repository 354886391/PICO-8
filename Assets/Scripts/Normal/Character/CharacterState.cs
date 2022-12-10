using Sirenix.OdinValidator.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class CharacterState
{
    public const int StNormal = 0;
    public const int StClimb = 1;
    public const int StDash = 2;
    public const int StSwim = 3;
    public const int StBoost = 4;
    public const int StRedDash = 5;
    public const int StHitSquash = 6;
    public const int StLaunch = 7;
    public const int StPickup = 8;
    public const int StDreamDash = 9;
    public const int StSummitLaunch = 10;
    public const int StDummy = 11;
    public const int StBirdDashTutorial = 12;
    public const int StFrozen = 13;
    public const int StReflectionFall = 14;
    public const int StStarFly = 15;
    public const int StTempleFall = 16;
    public const int StCassetteFly = 17;
    public const int StAttract = 18;
    public const int StFlingBird = 19;


    private StateMachine _machine;

    public void Init()
    {
        _machine = new StateMachine(20);
        _machine.SetCallbacks(StNormal, onUpdate: NormalUpdate, begin: NormalBegin);

    }

    public void Update()
    {

    }

    private void NormalBegin()
    {

    }

    private int NormalUpdate()
    {
        return -1;
    }


}

