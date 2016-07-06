using System.Linq;
using DungeonMasterEngine.DungeonContent.Entity.GroupSupport.Base;
using DungeonMasterEngine.DungeonContent.Tiles.Initializers;
using DungeonMasterEngine.DungeonContent.Tiles.Support;
using DungeonMasterEngine.Interfaces;
using Microsoft.Xna.Framework;

namespace DungeonMasterEngine.DungeonContent.Tiles
{

    public class TeleportTile : TeleportTile<Message>
    {
        public TeleportTile(TeleprotInitializer initializer) : base(initializer) { }
    }

    public class TeleportTile<TMessage> : FloorTile<TMessage>, ILevelConnector where TMessage : Message
    {
        private ITile nextLevelEnter;
        public IConstrain ScopeConstrain { get; private set; }
        public bool Visible { get; private set; }
        public int NextLevelIndex { get; private set; }
        public Point TargetTilePosition { get; private set; }
        public MapDirection Direction { get; private set; }
        public bool AbsoluteDirection { get; private set; }

        public ITile NextLevelEnter
        {
            get { return nextLevelEnter; }
            set
            {
                nextLevelEnter = value;
            }
        }

        public bool Open { get; private set; }

        public sealed override bool ContentActivated
        {
            get { return Open; }

            protected set
            {
                Open = value;
                TryTeleportAll();
            }
        }


        private void TryTeleportAll()
        {
            if (Open)
            {
                foreach (var i in SubItems.ToArray())
                    TeleportItem(i);
            }
        }

        public TeleportTile(TeleprotInitializer initializer) : base(initializer)
        {
            initializer.Initializing += Initialize;
        }

        private void Initialize(TeleprotInitializer initializer)
        {
            Direction = initializer.Direction;
            AbsoluteDirection = initializer.AbsoluteDirection;
            NextLevelIndex = initializer.NextLevelIndex;
            ScopeConstrain = initializer.ScopeConstrain;
            TargetTilePosition = initializer.TargetTilePosition;
            Visible = initializer.Visible;
            Open = initializer.Open;

            FloorSide.SubItemsChanged += (sender, args) => TryTeleportAll();

            initializer.Initializing -= Initialize;
        }

        private void TeleportItem(object obj)
        {
            var localizable = obj as ILocalizable<ISpaceRouteElement>;
            if (localizable != null && Open && ScopeConstrain.IsAcceptable(obj) && NextLevelEnter != null)//TODO how to set taget location creatures
            {
                if (AbsoluteDirection)
                {
                    localizable.MapDirection = Direction;
                }
                else
                {
                    localizable.MapDirection = localizable.MapDirection.GetRotated(Direction.Index + 1);
                }
                localizable.Location = localizable.Location.GetNew(NextLevelEnter);
            }
        }

    }
}