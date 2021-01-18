/*
 * AltFormatter.
 * Licensed under MIT License.
 * Copyright © 2017 Pavel Chaimardanov.
 */
using System;
using System.IO;

namespace AltFormatter.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Examples:");
            Console.WriteLine();

            TextWriter console = Console.Out;

            SerializationReport<TestClassWithDoubleArray>.GenerateReport(
                "Small array (double, 100 items)",
                () => new TestClassWithDoubleArray(100),
                console);

            SerializationReport<TestClassWithDoubleArray>.GenerateReport(
                "Middle array (double, 1000 items)",
                () => new TestClassWithDoubleArray(1000),
                console);

            SerializationReport<TestClassWithDoubleArray>.GenerateReport(
                "Large array (double, 10000 items)",
                () => new TestClassWithDoubleArray(10000),
                console);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}