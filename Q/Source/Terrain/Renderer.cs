using System.Collections.Generic;
using QE;
using QE.Engine;

namespace Q {

    partial class Terrain {

        public class Renderer {

            public Terrain Terrain { get; private set; }

            public Renderer(Terrain terrain) {
                Terrain = terrain;
                terrain.OnUpdate += (i, j, v) => Update(i, j);
            }

            IEnumerable<ResourcedTexture> ChunkTextures(int cx, int cy) {
                SortedSet<ResourcedTexture> textures = new SortedSet<ResourcedTexture>();
                for (int i = 0; i <= CHUNK_SIZE; i++)
                    for (int j = 0; j <= CHUNK_SIZE; j++) {
                        textures.Add(Terrain[cx * CHUNK_SIZE + i, cy * CHUNK_SIZE + j].Texture);
                    }
                return textures;
            }

            Dictionary<Vec2i, Dictionary<ResourcedTexture, Texture>> alphaTexture = new Dictionary<Vec2i, Dictionary<ResourcedTexture, Texture>>();
            Dictionary<Vec2i, Model> model = new Dictionary<Vec2i, Model>();

            Model MakeModel(int cx, int cy) {
                List<Vec3> shape = new List<Vec3>();
                for (int i = 0; i < CHUNK_SIZE; i++)
                    for (int j = 0; j < CHUNK_SIZE; j++) {
                        shape.Add(Terrain.GetVec(cx * CHUNK_SIZE + i, cy * CHUNK_SIZE + j));
                        shape.Add(Terrain.GetVec(cx * CHUNK_SIZE + i, cy * CHUNK_SIZE + j + 1));
                        shape.Add(Terrain.GetVec(cx * CHUNK_SIZE + i + 1, cy * CHUNK_SIZE + j));

                        shape.Add(Terrain.GetVec(cx * CHUNK_SIZE + i + 1, cy * CHUNK_SIZE + j + 1));
                        shape.Add(Terrain.GetVec(cx * CHUNK_SIZE + i, cy * CHUNK_SIZE + j + 1));
                        shape.Add(Terrain.GetVec(cx * CHUNK_SIZE + i + 1, cy * CHUNK_SIZE + j));
                    }
                return new Model(shape, Shader);
            }

            Model GetModel(int cx, int cy) {
                Vec2i c = new Vec2i(cx, cy);
                if (!model.ContainsKey(c))
                    model[c] = MakeModel(cx, cy);
                return model[c];
            }

            static Texture Empty = new Texture(CHUNK_SIZE + 1, CHUNK_SIZE + 1);
            static Renderer() {
                for (int i = 0; i <= CHUNK_SIZE; i++)
                    for (int j = 0; j <= CHUNK_SIZE; j++)
                        Empty[i, j] = new Color(1, 1, 1, 1);
                Empty.Wrap = Texture.WrapMode.Clamp;
                Empty.Smooth = true;
            }

            void Update(int i, int j) {
                Vec2i c = new Vec2i(GMath.DivDown(i, CHUNK_SIZE), GMath.DivDown(j, CHUNK_SIZE));
                List<Vec2i> todel = new List<Vec2i>();
                todel.Add(c);
                if (i % CHUNK_SIZE == 0)
                    todel.Add(c - Vec2i.OrtX);
                if (j % CHUNK_SIZE == 0)
                    todel.Add(c - Vec2i.OrtY);
                foreach (var ch in todel) {
                    alphaTexture.Remove(ch);
                    model.Remove(ch);
                }
            }

            Texture GetAlpha(int cx, int cy, ResourcedTexture texture) {
                Vec2i c = new Vec2i(cx, cy);
                if (!alphaTexture.ContainsKey(c))
                    alphaTexture[c] = new Dictionary<ResourcedTexture, Texture>();
                if (!alphaTexture[c].ContainsKey(texture)) {
                    var tex = Empty.Copy();
                    for (int i = 0; i <= CHUNK_SIZE; i++)
                        for (int j = 0; j <= CHUNK_SIZE; j++)
                            tex[i, j] = new Color(1, 1, 1, texture.CompareTo(Terrain[cx * CHUNK_SIZE + i, cy * CHUNK_SIZE + j].Texture) > 0 ? 0 : 1);
                    alphaTexture[c][texture] = tex;
                }
                return alphaTexture[c][texture];
            }

            void RenderChunk(int cx, int cy) {
                RenderState.Push();

                RenderState.Set("chunkSize", CHUNK_SIZE);
                RenderState.Set("chunk", new Vec2(cx, cy));

                foreach (var texture in ChunkTextures(cx, cy)) {
                    RenderState.Set("texture", texture.Texture);
                    RenderState.Set("alphaTexture", GetAlpha(cx, cy, texture));
                    GetModel(cx, cy).Render();
                }

                RenderState.Pop();
            }

            public void Render(int fromX, int fromY, int toX, int toY) {
                RenderState.Push();
                //RenderState.DepthTest = true;
                for (int i = GMath.DivDown(fromX, CHUNK_SIZE); i < GMath.DivUp(toX, CHUNK_SIZE); i++)
                    for (int j = GMath.DivDown(fromY, CHUNK_SIZE); j < GMath.DivUp(toY, CHUNK_SIZE); j++)
                        RenderChunk(i, j);
            }

            static Font Font = new Font("Courier New", 20, Font.Style.Bold);
            static ResourcedTexture WaterTexture = new ResourcedTexture("Terrain/Water.png");
            static Shader Shader = new Shader(Resource.String("Shaders/Terrain.glsl"), "texture", "alphaTexture", "chunkSize", "chunk");

        }

    }

}