using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flood
{
    class Program
    {
        static int countOfFloodField = 0;

        static void Main(string[] args)
        {
            File inputFile = new File("input.txt");
            File outputFile = new File("output.txt");

            string[] linesFromFile = inputFile.ReadFromFile().Split('\n');

            int numberOfPoints = Convert.ToInt32(linesFromFile[0]);
            int numberOfWalls = Convert.ToInt32(linesFromFile[numberOfPoints + 1]);

            Point[] arPoint = new Point[numberOfPoints];
            List<Wall> lstWall = new List<Wall>();


            try
            {
                if (numberOfPoints < 2 || numberOfPoints > 100000)
                    throw new Exception("Error: 2 <= Number of Points <= 100 000");

                if (numberOfWalls < 1 || numberOfWalls > numberOfPoints * 2)
                    throw new Exception("Error: 1 <= Number of Wall <= " + Convert.ToString(numberOfPoints * 2));

                // Make Point array and Wall List
                arPoint = makePointArray(linesFromFile);
                lstWall = makeWallList(linesFromFile, arPoint);

                // Find max coordinate and create Plane
                Point maxCoord = maxCoordinate(arPoint);

                // Create the main plane one cell more than needed
                Cell[,] mainPlane = new Cell[maxCoord.X + 1, maxCoord.Y + 1];

                // Basic fill the main plane
                makePlane(mainPlane, maxCoord);
                buildWalls(lstWall, mainPlane);


                int countOfField = mainPlane.Length;
                countOfFloodField = 2 * (maxCoord.X + 1) + 2 * (maxCoord.Y - 1);

                // First flood
                floodStep(mainPlane, maxCoord);

                while (countOfFloodField < countOfField)
                {
                    breakWalls(lstWall, mainPlane);
                    floodStep(mainPlane, maxCoord);
                }


                int countOfSaveWalls = lstWall.Count;
                string[] arWall = new string[countOfSaveWalls+1];
                arWall[0] = Convert.ToString(countOfSaveWalls);

                Console.WriteLine(countOfSaveWalls);

                for (int i = 1; i < arWall.Length; i++)
	            {
                    arWall[i] = Convert.ToString(lstWall.ToArray()[i - 1].index);
                    Console.WriteLine(arWall[i]);
                }

                outputFile.WriteToFile(arWall);

            }
            catch(Exception e){
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();

        }


        // Method for calculate maximum value of coordinate
        static Point maxCoordinate(Point[] arPoint)
        {
            Point tmp = new Point(0,0);

            for (int i = 0; i < arPoint.Length; i++)
            {
                tmp.X = (tmp.X < arPoint[i].X) ? arPoint[i].X : tmp.X;
                tmp.Y = (tmp.Y < arPoint[i].Y) ? arPoint[i].Y : tmp.Y;
            }

            return tmp;
        }


        // Method for basic fill the plane and created border
        static void makePlane(Cell[,] mainPlane, Point maxCoord)
        {
            for (int i = 0; i < maxCoord.X + 1; i++)
            {
                for (int j = 0; j < maxCoord.Y + 1; j++)
                {
                    mainPlane[i, j] = new Cell();

                    // Fill bordered cell
                    if (i == maxCoord.X || i == 0 || j == maxCoord.Y || j == 0)
                    {
                        mainPlane[i, j].flooded = true;
                    }
                }
            }
        }


        // Method for make Point's array of string array in the input file
        static Point[] makePointArray(string[] linesFromFile)
        {
            int numberOfPoint = Convert.ToInt32(linesFromFile[0]);
            Point[] arPoint = new Point[numberOfPoint];

            for (int i = 1; i < arPoint.Length + 1; i++)
            {
                string[] line = linesFromFile[i].Split(' ');
                arPoint[i - 1] = new Point(Convert.ToInt32(line[0]), Convert.ToInt32(line[1]));
            }

            return arPoint;
        }


        // Method for make Wall's array of string array in the input file
        static List<Wall> makeWallList(string[] linesFromFile, Point[] arPoint)
        {
            int numberOfPoint = Convert.ToInt32(linesFromFile[0]);
            int numberOfWalls = Convert.ToInt32(linesFromFile[numberOfPoint + 1]);

            List<Wall> lstWall = new List<Wall>();

            for (int i = arPoint.Length + 2, j = 1; i < linesFromFile.Length; i++, j++)
            {
                string[] line = linesFromFile[i].Split(' ');
                lstWall.Add(new Wall(arPoint[Convert.ToInt32(line[0]) - 1], arPoint[Convert.ToInt32(line[1]) - 1]));
            }
            return lstWall;
        }


        // Method for building all of walls in the plane
        static void buildWalls(List<Wall> lstWall, Cell[,] mainPlane)
        {
            foreach (Wall wall in lstWall)
            {
                if (wall.horizontal){
                    for (int i = wall.start.Y; i < wall.end.Y; i++)
                    {
                        mainPlane[wall.start.X, i].horizontalWall = true;
                    }
                }
                else
                {
                    for (int i = wall.start.X; i < wall.end.X; i++)
                    {
                        mainPlane[i, wall.start.Y].verticalWall = true;
                    }
                }
            }
        }


        // Method for break wall after flood step
        static void breakWalls(List<Wall> lstWall, Cell[,] mainPlane)
        {
            int i = 0;
            while (i < lstWall.Count)
                if (lstWall[i].horizontal)
                {
                    if (mainPlane[lstWall[i].start.X - 1, lstWall[i].start.Y].flooded != mainPlane[lstWall[i].start.X, lstWall[i].start.Y].flooded)
                    {
                        for (int j = lstWall[i].start.Y; j < lstWall[i].end.Y; j++)
                        {
                            mainPlane[lstWall[i].start.X, j].horizontalWall = false;
                        }
                        lstWall.RemoveAt(i);
                    }
                    else i++;
                }
                else
                {
                    if (mainPlane[lstWall[i].start.X, lstWall[i].start.Y - 1].flooded != mainPlane[lstWall[i].start.X, lstWall[i].start.Y].flooded)
                    {
                        for (int j = lstWall[i].start.X; j < lstWall[i].end.X; j++)
                        {
                            mainPlane[j, lstWall[i].start.Y].verticalWall = false;
                        }
                        lstWall.RemoveAt(i);
                    }
                    else i++;
                }
                    
        }


        // 
        static void floodStep(Cell[,] mainPlane, Point maxCoord)
        {
            for (int i = 0; i < maxCoord.X + 1; i++)
            {
                for (int j = 0; j < maxCoord.Y + 1; j++)
                {
                    if (mainPlane[i, j].flooded == true)
                    {
                        Stack<Point> floodField = new Stack<Point>();
                        floodField.Push(new Point(i, j));
                        while (floodField.Count != 0)
                        {
                            Point point = floodField.Pop();

                            // Check down cell
                            if (point.X != maxCoord.X && !mainPlane[point.X + 1, point.Y].flooded && !mainPlane[point.X + 1, point.Y].horizontalWall)
                            {
                                mainPlane[point.X + 1, point.Y].flooded = true;
                                countOfFloodField++;
                                floodField.Push(new Point(point.X + 1, point.Y));
                            }

                            // Check up cell
                            if (point.X != 0 && !mainPlane[point.X - 1, point.Y].flooded && !mainPlane[point.X, point.Y].horizontalWall)
                            {
                                mainPlane[point.X - 1, point.Y].flooded = true;
                                countOfFloodField++;
                                floodField.Push(new Point(point.X - 1, point.Y));
                            }

                            // Check right cell
                            if (point.Y != maxCoord.Y && !mainPlane[point.X, point.Y + 1].flooded && !mainPlane[point.X, point.Y + 1].verticalWall)
                            {
                                mainPlane[point.X, point.Y + 1].flooded = true;
                                countOfFloodField++;
                                floodField.Push(new Point(point.X, point.Y + 1));
                            }

                            // Check left cell
                            if (point.Y != 0 && !mainPlane[point.X, point.Y - 1].flooded && !mainPlane[point.X, point.Y].verticalWall)
                            {
                                mainPlane[point.X, point.Y - 1].flooded = true;
                                countOfFloodField++;
                                floodField.Push(new Point(point.X, point.Y - 1));
                            }

                        }
                    }
                }
            }
        }


    }
}
