using KaelsToolbox.GameStuff.FNA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KaelsToolbox.GameStuff
{
    public class Grid
    {
        int width, height;
        bool loops;
        List<List<Data>> grid;
        public List<Data> AllData;

        public Grid(int width, int height, bool loops)
        {
            this.width = width;
            this.height = height;
            this.loops = loops;

            grid = new List<List<Data>>();
            AllData = new List<Data>();
            Setup();
        }

        void Setup()
        {
            for (int y = 0; y < height; y++)
            {
                List<Data> temp = new List<Data>();
                grid.Add(temp);
                for (int x = 0; x < width; x++)
                {
                    Data tmp = new Data(x, y);
                    temp.Add(tmp);
                    AllData.Add(tmp);
                }
            }
        }

        public Data GetTile(int x, int y)
        {
            try
            {
                return grid[y][x];
            }
            catch (IndexOutOfRangeException)
            {
                return grid[0][0];
                throw;
            }
        }
        public Data GetTile(int[] XY)
        {
            try
            {
                return grid[XY[0]][XY[1]];
            }
            catch (IndexOutOfRangeException)
            {
                return grid[0][0];
                throw;
            }
        }
        public void SetTile(int x, int y, Data data)
        {
            grid[y][x] = data;
        } 
        public Data Move(Data data, int x_movement, int y_movement)
        {
            if (!loops)
            {
                if (
                    x_movement + data.x < width && 
                    y_movement + data.y < height && 
                    x_movement + data.x >= 0 && 
                    y_movement + data.y >= 0)
                {
                    return grid[data.y + y_movement][data.x + x_movement];
                }
                else
                {
                    return grid[data.y][data.x];
                }
            }
            return new Data(0, 0);
        }
    }

    public class Data
    {
        public string Name;
        public bool Occupied;
        public int OccupiedCount;
        public FNA_Tile Tile;
        public int x { get; private set; }
        public int y { get; private set; }
        public Data(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Ocupy()
        {
            Occupied = true;
            OccupiedCount++;
        }
        public void DeOcupy()
        {
            Occupied = false;
            OccupiedCount--;
        }
    }
}
