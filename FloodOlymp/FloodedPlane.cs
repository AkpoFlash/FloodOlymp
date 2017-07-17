using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodOlymp
{
    static class FloodedPlane
    {
        public static int CountOfFloodField { get; set; }


        // Method for basic fill the plane and created border
        public static void MakePlane(Cell[,] mainPlane, Point maxCoord)
        {
            for (int i = 0; i < maxCoord.X + 1; i++)
            {
                for (int j = 0; j < maxCoord.Y + 1; j++)
                {
                    mainPlane[i, j] = new Cell();

                    // Fill bordered cell
                    if (i == maxCoord.X || i == 0 || j == maxCoord.Y || j == 0)
                    {
                        mainPlane[i, j].Flooded = true;
                    }
                }
            }
        }


        // Method for building all of walls in the plane
        public static void BuildWalls(List<Wall> lstWall, Cell[,] mainPlane)
        {
            foreach (Wall wall in lstWall)
            {
                if (wall.Horizontal)
                {
                    for (int i = wall.Start.Y; i < wall.End.Y; i++)
                    {
                        mainPlane[wall.Start.X, i].HorizontalWall = true;
                    }
                }
                else
                {
                    for (int i = wall.Start.X; i < wall.End.X; i++)
                    {
                        mainPlane[i, wall.Start.Y].VerticalWall = true;
                    }
                }
            }
        }


        // Method for break wall after flood step
        public static void BreakWalls(List<Wall> lstWall, Cell[,] mainPlane)
        {
            int i = 0;
            while (i < lstWall.Count)
            {
                if (lstWall[i].Horizontal)
                {
                    if (IsBothSideFlooded(mainPlane, lstWall[i]))
                    {
                        for (int j = lstWall[i].Start.Y; j < lstWall[i].End.Y; j++)
                        {
                            mainPlane[lstWall[i].Start.X, j].HorizontalWall = false;
                        }
                        lstWall.RemoveAt(i);
                    }
                    else i++;
                }
                else
                {
                    if (IsBothSideFlooded(mainPlane, lstWall[i]))
                    {
                        for (int j = lstWall[i].Start.X; j < lstWall[i].End.X; j++)
                        {
                            mainPlane[j, lstWall[i].Start.Y].VerticalWall = false;
                        }
                        lstWall.RemoveAt(i);
                    }
                    else i++;
                }
            }
        }


        // Execution the one step of the flood 
        public static void FloodStep(Cell[,] mainPlane, Point maxCoord)
        {
            for (int i = 0; i < maxCoord.X + 1; i++)
            {
                for (int j = 0; j < maxCoord.Y + 1; j++)
                {
                    if (mainPlane[i, j].Flooded == true)
                    {
                        Stack<Point> floodField = new Stack<Point>();
                        floodField.Push(new Point(i, j));
                        while (floodField.Count != 0)
                        {
                            Point point = floodField.Pop();

                            // Check down cell
                            if (point.X != maxCoord.X
                                && !mainPlane[point.X + 1, point.Y].Flooded
                                && !mainPlane[point.X + 1, point.Y].HorizontalWall)
                            {
                                FloodingField(mainPlane, new Point(point.X + 1, point.Y), floodField);
                            }

                            // Check up cell
                            if (point.X != 0
                                && !mainPlane[point.X - 1, point.Y].Flooded
                                && !mainPlane[point.X, point.Y].HorizontalWall)
                            {
                                FloodingField(mainPlane, new Point(point.X - 1, point.Y), floodField);
                            }

                            // Check right cell
                            if (point.Y != maxCoord.Y
                                && !mainPlane[point.X, point.Y + 1].Flooded
                                && !mainPlane[point.X, point.Y + 1].VerticalWall)
                            {
                                FloodingField(mainPlane, new Point(point.X, point.Y + 1), floodField);
                            }

                            // Check left cell
                            if (point.Y != 0
                                && !mainPlane[point.X, point.Y - 1].Flooded
                                && !mainPlane[point.X, point.Y].VerticalWall)
                            {
                                FloodingField(mainPlane, new Point(point.X, point.Y - 1), floodField);
                            }

                        }
                    }
                }
            }
        }


        // Check flooding of the wall on both side
        private static bool IsBothSideFlooded(Cell[,] mainPlane, Wall wall)
        {
            if (wall.Horizontal)
                return mainPlane[wall.Start.X - 1, wall.Start.Y].Flooded != mainPlane[wall.Start.X, wall.Start.Y].Flooded;
            else
                return mainPlane[wall.Start.X, wall.Start.Y - 1].Flooded != mainPlane[wall.Start.X, wall.Start.Y].Flooded;
        }


        // Flooding one field of the plane
        private static void FloodingField(Cell[,] mainPlane, Point point, Stack<Point> floodField)
        {
            mainPlane[point.X, point.Y].Flooded = true;
            CountOfFloodField++;
            floodField.Push(new Point(point.X, point.Y));
        }


    }
}
