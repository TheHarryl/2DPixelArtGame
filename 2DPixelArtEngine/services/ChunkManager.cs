using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace _2DPixelArtEngine
{
    public struct ChunkPosition
    {
        public int X;
        public int Y;
        public ChunkPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public class ChunkManager
    {
        private Dictionary<ChunkPosition, List<Object>> _chunks;
        private int _chunkSize;

        public ChunkManager(int chunkSize = 320)
        {
            _chunkSize = chunkSize;
            Set(new List<Object>());
        }

        public void ReindexChunks()
        {
            foreach (KeyValuePair<ChunkPosition, List<Object>> chunk in _chunks)
            {
                for (int i = 0; i < chunk.Value.Count; i++)
                {
                    Object obj = chunk.Value[i];
                    if (obj.Direction == Vector2.Zero || obj.Speed == 0f) continue;
                    ChunkPosition objChunk = GetChunk(obj.Position);
                    if (chunk.Key.X != objChunk.X || chunk.Key.Y != objChunk.Y)
                    {
                        chunk.Value.Remove(obj);
                        _chunks[objChunk].Add(obj);
                    }
                }
            }
        }

        public void UpdateChunk(GameTime gameTime, ChunkPosition chunk)
        {
            if (!_chunks.ContainsKey(chunk)) return;
            for (int i = 0; i < _chunks[chunk].Count; i++)
            {
                _chunks[chunk][i].Update(gameTime);
            }
        }

        public void DrawChunk(SpriteBatch spriteBatch, RectangleF screenBounds, ChunkPosition chunk, Vector2 offset = new Vector2(), bool sort = false)
        {
            if (!_chunks.ContainsKey(chunk)) return;
            if (sort)
                _chunks[chunk].OrderByDescending(o => o.Position.Y).ToList();
            for (int i = 0; i < _chunks[chunk].Count; i++)
            {
                if (screenBounds.Contains(_chunks[chunk][i].GetBounds()))
                    _chunks[chunk][i].Draw(spriteBatch, offset);
            }
        }

        public void Set(List<Object> objects)
        {
            _chunks = new Dictionary<ChunkPosition, List<Object>>();
            Add(objects);
        }

        public void Add(List<Object> objects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                Add(objects[i]);
            }
        }

        public void Add(Object obj)
        {
            ChunkPosition chunk = GetChunk(obj.Position);
            if (!_chunks.ContainsKey(chunk))
            {
                _chunks.Add(chunk, new List<Object>());
            }
            _chunks[chunk].Add(obj);
        }

        public ChunkPosition GetChunk(Vector2 position)
        {
            return new ChunkPosition((int)Math.Ceiling(position.X / _chunkSize), (int)Math.Ceiling(position.Y / _chunkSize));
        }
    }
}
