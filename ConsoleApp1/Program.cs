using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static private int[,] matrixA = {
                { 0, 0, 0, 0 },
                { 0, 1, 2, 3 },
                { 0, 2, 4, 6 },
                { 0, 3, 6, 9 }
            };


        static private int[,] matrixB = {
                { 0, 1, 2, 3 },
                { 1, 2, 3, 4 },
                { 2, 3, 4, 5 },
                { 3, 4, 5, 6 }
            };

        static private int[,] matrixC = new int[4, 4];

        static void Main(string[] args)
        {


            Console.WriteLine("Matrix A");
            printMatrix(matrixA);


            Console.WriteLine("Matrix B");
            printMatrix(matrixB);


            RunThreads();

            Console.ReadLine();

        }

        public static void RunThreads()
        {
            Action<object> action = (object obj) =>
            {
                int Id = (int)Task.CurrentId - 1;
                Console.WriteLine($"Thread {Id} starting");
                for (int i = 0; i < 4; i++)
                {
                    int product = matrixC[Id, i] = matrixA[Id, i] * matrixB[Id, i];
                    Console.WriteLine($"Thread {Id} calculating element C[{Id}, {i}]={product}");
                    matrixC[Id, i] = product;
                }
            };

            var tasks = new List<Task>();

            for (int i = 0; i < 4; i++)
            {
                tasks.Add(Task.Factory.StartNew(action, i));
            }

            Task.WhenAll(tasks).ContinueWith(e => FinalConsoleWriteLine());
        }

        static void FinalConsoleWriteLine()
        {
            Console.WriteLine("\nMatrix C");
            printMatrix(matrixC);
        }

        static void printMatrix(int[,] matrix)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write($"{matrix[i, j]}  ");
                }
                Console.Write(Environment.NewLine);
            }
            Console.Write(Environment.NewLine);
        }
    }
}
