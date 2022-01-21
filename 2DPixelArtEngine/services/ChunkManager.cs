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
        public PixelEngine Parent;
        private Dictionary<ChunkPosition, List<Object>> _chunks;
        private int _chunkSize;

        public ChunkManager(int chunkSize = 320)
        {
            _chunkSize = chunkSize;
            Set(new List<Object>());
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
            ChunkPosition chunk = GetChunkPosition(obj.Position);
            obj.Parent = this;
            obj.Chunk = chunk;
            if (!_chunks.ContainsKey(chunk))
            {
                _chunks.Add(chunk, new List<Object>());
            }
            _chunks[chunk].Add(obj);
        }

        public void Remove(Object obj)
        {
            ChunkPosition chunk = obj.Chunk;
            if (_chunks[chunk].Contains(obj))
                _chunks[chunk].Remove(obj);
        }

        public void Reindex(Object obj)
        {
            ChunkPosition objChunk = GetChunkPosition(obj.Position);
            if (obj.Chunk.X != objChunk.X || obj.Chunk.Y != objChunk.Y)
            {
                Remove(obj);
                Add(obj);
            }
        }

        public ChunkPosition GetChunkPosition(Vector2 position)
        {
            return GetChunkPosition(position.X, position.Y);
        }

        public ChunkPosition GetChunkPosition(float x, float y)
        {
            return new ChunkPosition((int)(x / _chunkSize), (int)(y / _chunkSize));
        }

        public List<Object> GetChunk(ChunkPosition chunk)
        {
            if (!_chunks.ContainsKey(chunk)) return new List<Object>();
            return _chunks[chunk];
        }

        public List<Object> GetChunk(int chunkX, int chunkY)
        {
            ChunkPosition chunk = new ChunkPosition(chunkX, chunkY);
            return GetChunk(chunk);
        }

        public List<Object> GetChunksInRange(int x, int y, int width, int height)
        {
            List<Object> objects = new List<Object>();
            for (int y1 = y; y1 <= height; y1++)
            {
                for (int x1 = x; x1 <= width; x1++)
                {
                    objects = objects.Concat(GetChunk(x1, y1)).ToList();
                }
            }
            return objects;
        }

        public List<Object> GetNearbyChunks(ChunkPosition chunk, int chunkRadius = 1)
        {
            return GetChunksInRange(chunk.X - chunkRadius, chunk.Y - chunkRadius, chunkRadius * 2, chunkRadius * 2);
        }
    }
}
