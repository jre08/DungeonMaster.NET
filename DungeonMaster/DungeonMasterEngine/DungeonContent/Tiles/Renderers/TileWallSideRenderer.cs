using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonMasterEngine.DungeonContent.Tiles
{
    public class TileWallSideRenderer<TSide> : TileSideRenderer where TSide : TileSide
    {
        public Texture2D WallTexture { get; }
        public Texture2D DecorationTexture { get; }
        public TSide TileSide { get; }

        protected Matrix transformation;
        private readonly DecorationRenderer<object> decorationRenderer;

        public WallResource Resource => WallResource.Instance;

        public TileWallSideRenderer(TSide tileSide, Texture2D wallTexture, Texture2D decorationTexture)
        {
            WallTexture = wallTexture;
            DecorationTexture = decorationTexture;
            TileSide = tileSide;

            decorationRenderer = new DecorationRenderer<object>(decorationTexture, null);

            Matrix rotation;
            GetTransformation(tileSide.Face, out rotation);
            Matrix translation;
            var translationVector = new Vector3(0.5f);
            Matrix.CreateTranslation(ref translationVector, out translation);
            Matrix.Multiply(ref rotation, ref translation, out transformation);
        }

        public override Matrix Render(ref Matrix currentTransformation, BasicEffect effect, object parameter)
        {
            Matrix finalTransformation;
            Matrix.Multiply(ref transformation, ref currentTransformation, out finalTransformation);

            RenderWall(effect, ref finalTransformation);

            if (TileSide.RandomDecoration)
                decorationRenderer.Render(ref finalTransformation, effect, parameter);

            return finalTransformation;
        }

        public override bool Interact(ILeader leader, ref Matrix currentTransformation, object param)
        {
            return false;
        }

        private void RenderWall(BasicEffect effect, ref Matrix finalTransformation)
        {
            Color color = Color.Black;

            if(TileSide.Face == MapDirection.North)
                color = Color.Red;
            if(TileSide.Face == MapDirection.West)
                color = Color.Green;
            if(TileSide.Face == MapDirection.South)
                color = Color.Blue;
            if(TileSide.Face == MapDirection.East)
                color = Color.Yellow;

            if (Highlighted)
            {
                color = Color.Orange;
            }


            effect.DiffuseColor = color.ToVector3();
            effect.World = finalTransformation;
            effect.Texture = WallTexture;
            effect.GraphicsDevice.Indices = Resource.IndexBuffer;
            effect.GraphicsDevice.SetVertexBuffer(Resource.VertexBuffer);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                effect.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Resource.VertexBuffer.VertexCount, 0, 2);
            }
        }
    }
}