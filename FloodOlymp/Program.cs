using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloodOlymp
{
    class Program
    {

        static void Main(string[] args)
        {
            File inputFile = new File("input.txt");
            File outputFile = new File("output.txt");

            string[] linesFromFile = inputFile.ReadFromFile().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            int numberOfPoints = Convert.ToInt32(linesFromFile[0]);
            int numberOfWalls = Convert.ToInt32(linesFromFile[numberOfPoints + 1]);

            Point[] arPoint = new Point[numberOfPoints];
            List<Wall> lstWall = new List<Wall>();


            try
            {
                Validate.CheckBorder(numberOfPoints, 2, 100000, "Error: 2 <= Number of Points <= 100 000");

                Validate.CheckBorder(numberOfWalls, 1, numberOfPoints * 2, "Error: 1 <= Number of Wall <= " + Convert.ToString(numberOfPoints * 2));

                // Make Point array and Wall List
                arPoint = MakePointArray(linesFromFile);
                lstWall = MakeWallList(linesFromFile, arPoint);

                // Find max coordinate and create Plane
                Point maxCoord = MaxCoordinate(arPoint);

                // Create the main plane one cell more than needed
                Cell[,] mainPlane = new Cell[maxCoord.X + 1, maxCoord.Y + 1];

                // Basic fill the main plane
                Plane.MakePlane(mainPlane, maxCoord);
                Plane.BuildWalls(lstWall, mainPlane);

                int countOfField = mainPlane.Length;

                // Count of bordered cells
                Plane.CountOfFloodField = 2 * (maxCoord.X + 1) + 2 * (maxCoord.Y - 1);

                // First step of flood
                Plane.FloodStep(mainPlane, maxCoord);


                while (Plane.CountOfFloodField < countOfField)
                {
                    Plane.BreakWalls(lstWall, mainPlane);
                    Plane.FloodStep(mainPlane, maxCoord);
                }

                // Preparation of output (console and file)
                int countOfSaveWalls = lstWall.Count;
                string[] outputArray = new string[countOfSaveWalls+1];


                // Add in array number of saved walls
                outputArray[0] = Convert.ToString(countOfSaveWalls);
                Console.WriteLine(outputArray[0]);

                for (int i = 1; i < outputArray.Length; i++)
	            {
                    outputArray[i] = Convert.ToString(lstWall.ToArray()[i - 1].Index);
                    Console.WriteLine(outputArray[i]);
                }


                outputFile.WriteToFile(outputArray);

            }
            catch(Exception e){
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();

        }


        // Method for calculate maximum value of coordinate
        private static Point MaxCoordinate(Point[] arPoint)
        {
            Point tmp = new Point(0,0);

            for (int i = 0; i < arPoint.Length; i++)
            {
                tmp.X = (tmp.X < arPoint[i].X) ? arPoint[i].X : tmp.X;
                tmp.Y = (tmp.Y < arPoint[i].Y) ? arPoint[i].Y : tmp.Y;
            }

            return tmp;
        }


        // Method for make Point's array of string array in the input file
        private static Point[] MakePointArray(string[] linesFromFile)
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
        private static List<Wall> MakeWallList(string[] linesFromFile, Point[] arPoint)
        {
            int numberOfPoint = Convert.ToInt32(linesFromFile[0]);
            int numberOfWalls = Convert.ToInt32(linesFromFile[numberOfPoint + 1]);

            List<Wall> lstWall = new List<Wall>();

            // Index of first wall in the file
            int firstWall = arPoint.Length + 2;

            for (int i = firstWall; i < linesFromFile.Length; i++)
            {
                string[] line = linesFromFile[i].Split(' ');
                lstWall.Add(new Wall(arPoint[Convert.ToInt32(line[0]) - 1], arPoint[Convert.ToInt32(line[1]) - 1]));
            }
            return lstWall;
        }


    }
}
