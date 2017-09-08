using System;
using System.Collections.Generic;
using EW.Graphics;
using EW.Primitives;
using EW.Activities;
using EW.Xna.Platforms;
namespace EW.Traits
{
    public enum PipType
    {
        Transparent,
        Green,
        Yellow,
        Red,
        Gray,
        Blue,Ammo,
        AmmoEmpty,
    }

    public enum Stance
    {
        None = 0,
        Enemy = 1,      //敌对
        Neutral = 2,    //中立国
        Ally = 4,       //同盟国
    }

    public enum TargetModifiers
    {
        None = 0,
        ForceAttack = 1,
        ForceQueue = 2,
        ForceMove = 4,
    }

    public interface IExplodeModifier { bool ShouldExplode(Actor self); }

    [Flags]
    public enum DamageState
    {
        Undamaged = 1,
        Light = 2,
        Medium = 4,
        Heavy = 8,
        Critical = 16,
        Dead = 32
    }

    public class Damage
    {
        public readonly int Value;

        public readonly HashSet<string> DamageTypes;
    }

    public interface IGameOver
    {
        void GameOver(World world);
    }

    public interface IHealth
    {
        DamageState DamageState { get; }

        int HP { get; }

        int MaxHP { get; }

        int DisplayHP { get; }

        bool IsDead { get; }

        void InflictDamage(Actor self, Actor attacker, int damage,IWarHead warhead, bool ignoreModifiers);

        void Kill(Actor self, Actor attacker);

    }
    /// <summary>
    /// 弹头接口
    /// </summary>
    public interface IWarHead
    {
        int Delay { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="victim">受害方</param>
        /// <param name="firedBy">发射方</param>
        /// <returns></returns>
        bool IsValidAgainst(Actor victim, Actor firedBy);

        bool IsValidAgainst(FrozenActor victim,Actor firedBy);

        void DoImpact(Target target, Actor firedBy, IEnumerable<int> damageModifiers);
    }

    #region Notify Interface

    public interface INotifyIdle { void TickIdle(Actor self); }


    
    public interface INotifyCreated { void Created(Actor self); }

    public interface INotifyAddToWorld { void AddedToWorld(Actor self); }

    public interface INotifyActorDisposing { void Disposing(Actor self); }

    public interface INotifyRemovedFromWorld { void RemovedFromWorld(Actor self); }

    public interface INotifySold { void Selling(Actor self); void Sold(Actor self); }

    public interface INotifyBlockingMove { void OnNotifyBlockingMove(Actor self, Actor blocking); }

    public interface INotifyCrushed
    {
        void OnCrush(Actor self, Actor crusher, HashSet<string> crushClasses);

        void WarnCrush(Actor self, Actor crusher, HashSet<string> crushClasses);
    }

    public interface INotifyDiscovered { void OnDiscovered(Actor self, Player discoverer, bool playNotification); }

    public interface INotifySelected { void Selected(Actor self); }

    public interface INotifyBecomingIdle { void OnBecomingIdle(Actor self); }

    public interface INotifyOwnerChanged { void OnOwnerChanged(Actor self, Player oldOwner, Player newOwner); }
    #endregion

    #region Render Interface
    public interface IRenderAboveShroudWhenSelected
    {
        IEnumerable<IRenderable> RenderAboveShroud(Actor self, WorldRenderer wr);
    }
    public interface IRenderAboveWorld { void RenderAboveWorld(Actor self, WorldRenderer wr); }

    public interface IRenderOverlay
    {
        void Render(WorldRenderer wr);
    }

    public interface IRender
    {
        IEnumerable<IRenderable> Render(Actor self, WorldRenderer wr);
    }

    public interface IRenderModifier
    {
        IEnumerable<IRenderable> ModifyRender(Actor self, WorldRenderer wr, IEnumerable<IRenderable> r);
    }

    public interface IPostRenderSelection
    {
        IEnumerable<IRenderable> RenderAfterWorld(WorldRenderer wr);
    }
    #endregion

    #region Modifier Interface


    public interface IDefaultVisibility { bool IsVisible(Actor self, Player byPlayer); }
    public interface IVisibilityModifier { bool IsVisible(Actor self, Player byPlayer); }

    public interface IDamageModifier { int GetDamageModifier(Actor attacker, IWarHead warhead); }

    public interface ISpeedModifier { int GetSpeedModifier(); }

    public interface IReloadModifier { int GetReloadModifier(); }

    public interface IFogVisibilityModifier
    {
        bool IsVisible(Actor actor);
        bool HasFogVisibility();
    }
    #endregion

    public interface IAutoSelectionSize { Vector2 SelectionSize(Actor self); }

    /// <summary>
    /// 占领地
    /// </summary>
    public interface IOccupySpace
    {
        WPos CenterPosition { get; }

        CPos TopLeft { get; }

        IEnumerable<Pair<CPos, SubCell>> OccupiedCells();
    }
    public interface IPositionableInfo : IOccupySpaceInfo
    {
        bool CanEnterCell(World world, Actor self, CPos cell, Actor ignoreActor = null, bool checkTransientActors = true);
    }
    public interface IPositionable : IOccupySpace
    {
        bool IsLeavingCell(CPos location, SubCell subCell = SubCell.Any);

        bool CanEnterCell(CPos location, Actor ignoreActor = null, bool checkTransientActors = true);

        void SetPosition(Actor self, CPos cPos, SubCell subCell = SubCell.Any);

        void SetPosition(Actor self, WPos wPos);

        void SetVisualPosition(Actor self, WPos wPos);
    }
    #region Order Interface
        public interface IOrderTargeter
        {
            string OrderID { get; }

            int OrderPriority { get; }

            bool CanTarget(Actor self, Target target, List<Actor> othersAtTarget, ref TargetModifiers modifiers, ref string cursor);

            bool IsQueued { get; }

            bool TargetOverridesSelection(TargetModifiers modifiers);
        }
        public interface IIssueOrder
        {
            IEnumerable<IOrderTargeter> Orders { get; }

            Order IssueOrder(Actor self, IOrderTargeter order, Target target, bool queued);
        }

    public interface IResolveOrder
    {
        void ResolveOrder(Actor self, Order order);
    }

    public interface IOrderVoice { string VoicePhraseForOrder(Actor self, Order order); }

    #endregion
    

    public interface IDisable { bool Disabled { get; } }

    public interface IDisabledTrait { bool IsTraitDisabled { get; } }

    /// <summary>
    /// 
    /// </summary>
    public interface IUpgradable
    {
        IEnumerable<string> UpgradeTypes { get; }

        bool AcceptsUpgradeLevel(Actor self, string type, int level);

        void UpgradeLevelChanged(Actor self, string type, int oldLevel, int newLevel);
    }


    public interface ITraitInfoInterface { }

    public interface Requires<T> where T : class, ITraitInfoInterface { }

    public interface ITraitInfo : ITraitInfoInterface
    {
        object Create(ActorInitializer init);
    }

    public class TraitInfo<T>:ITraitInfo where T : new()
    {
        public virtual object Create(ActorInitializer init)
        {
            return new T();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public interface IUpgradableInfo : ITraitInfo { }
    public interface IMove
    {
        Activity MoveTo(CPos cell, int nearEnough);

        Activity MoveTo(CPos cell, Actor ignoredActor);

        Activity MoveWithinRange(Target target, WDist range);

        Activity MoveWithinRange(Target target, WDist minRange, WDist maxRange);

        Activity MoveFollow(Actor self, Target target, WDist minRange, WDist maxRange);

        Activity MoveIntoWorld(Actor self, CPos cell, SubCell subCell = SubCell.Any);

        Activity MoveToTarget(Actor self, Target target);

        Activity MoveIntoTarget(Actor self, Target target);

        Activity VisualMove(Actor self, WPos fromPos, WPos toPos);

        CPos NearestMoveableCell(CPos target);

        bool IsMoving { get; set; }

        bool CanEnterTargetNow(Actor self, Target target);
    }
    public interface IMoveInfo : ITraitInfoInterface { }
    
    public interface IOccupySpaceInfo : ITraitInfoInterface
    {
        IReadOnlyDictionary<CPos, SubCell> OccupiedCells(ActorInfo info, CPos location, SubCell subCell = SubCell.Any);

        bool SharesCell { get; }
    }

    public interface IFacingInfo : ITraitInfoInterface
    {
        int GetInitialFacing();
    }

    public interface IFacing
    {
        int TurnSpeed { get; }
        int Facing { get; set; }
    }

    public interface IRulesetLoaded<TInfo> { void RulesetLoaded(Ruleset rules, TInfo info); }

    public interface IRulesetLoaded : IRulesetLoaded<ActorInfo>, ITraitInfoInterface { }


    public interface ITick { void Tick(Actor self); }

    public interface ITickRender { void TickRender(WorldRenderer wr, Actor self); }

    public interface IWorldLoaded { void WorldLoaded(World w,WorldRenderer render); }

    public interface UsesInit<T>:ITraitInfo where T : IActorInit { }

    public interface ILoadsPalettes
    {
        void LoadPalettes(WorldRenderer wr);
    }

    public interface IPaletteModifier
    {
        void AdjustPalette(IReadOnlyDictionary<string, MutablePalette> b);
    }

    public interface IProvidesAssetBrowserPalettes
    {
        IEnumerable<string> PaletteNames { get; }
    }

    public interface ICreatePlayers
    {
        void CreatePlayers(World w);
    }

    public interface IEffectiveOwner
    {
        bool Disguised { get; }

        Player Owner { get; }
    }

    public interface ILintRulesPass
    {
        void Run(Action<string> emitError, Action<string> emitWarning, Ruleset rules);
    }

    public interface ILintMapPass
    {
        void Run(Action<string> emitError, Action<string> emitWarning, Map map);
        
    }

    public interface ILintPass
    {
        void Run(Action<string> emitError, Action<string> emitWarning, ModData modData);
    }
}