using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultithreadingHomework
{
    internal class Program
    {
        private static readonly int[,] MatrixA = {
                { 0, 0, 0, 0 },
                { 0, 1, 2, 3 },
                { 0, 2, 4, 6 },
                { 0, 3, 6, 9 }
            };


        private static readonly int[,] MatrixB = {
                { 0, 1, 2, 3 },
                { 1, 2, 3, 4 },
                { 2, 3, 4, 5 },
                { 3, 4, 5, 6 }
            };

        private static readonly int[,] MatrixC = new int[4, 4];

        private static void Main(string[] args)
        {
            Console.WriteLine("Matrix A");
            PrintMatrix(MatrixA);

            Console.WriteLine("Matrix B");
            PrintMatrix(MatrixB);

            // Run concurrent processes
            RunThreads();

            // Keeps window open until ENTER key is pressed
            Console.ReadLine();
        }

        public static void RunThreads()
        {
            void CalculateRow(object obj)
            {
                var i = (int)obj;

                if (Task.CurrentId == null)
                {
                    return;
                }

                // Thread ID 
                var id = (int)Task.CurrentId - 1;

                Console.WriteLine($"Thread {id} starting.");

                for (var j = 0; j < 4; j++)
                {
                    for (var k = 0; k < 4; k++)
                    {
                        MatrixC[i, j] += MatrixA[i, k] * MatrixB[k, j];
                    }

                    var product = MatrixC[id, j];
                    Console.WriteLine($"Thread {id} calculating element C[{id}, {j}]={product}.");
                }
            }

            // Create four Tasks to calculate the four rows of Matrix C
            var tasks = new List<Task>();

            for (var i = 0; i < 4; i++)
            {
                tasks.Add(Task.Factory.StartNew(CalculateRow, i));
            }

            // Once all of the four concurrent processes finish, print Matrix C
            Task.WhenAll(tasks).ContinueWith(e => FinalConsoleWriteLine());
        }

        private static void FinalConsoleWriteLine()
        {
            Console.WriteLine("\nMultiplication of A and B");
            PrintMatrix(MatrixC);
        }

        private static void PrintMatrix(int[,] matrix)
        {
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    Console.Write($"{matrix[i, j]}  ");
                }
                Console.Write(Environment.NewLine);
            }
            Console.Write(Environment.NewLine);
        }
    }
}
