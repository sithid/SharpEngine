using System;
using System.Threading;
using MudEngine;

namespace MudEngine.ConsoleApp;

class Program
{
    static ManualResetEvent _exitEvent = new ManualResetEvent(false);

    static void Main(string[] args)
    {
        Console.CancelKeyPress += (sender, e) => 
        {
            e.Cancel = true; // Prevent the process from terminating immediately
            _exitEvent.Set(); // Signal the main thread to exit
        };

        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine($"{AnsiColors.Blue}╔══════════════════════════════════════╗{AnsiColors.Reset}");
        Console.WriteLine($"{AnsiColors.Blue}║         Welcome to SharpMUD          ║{AnsiColors.Reset}");
        Console.WriteLine($"{AnsiColors.Blue}║           Version 0.1.0              ║{AnsiColors.Reset}");
        Console.WriteLine($"{AnsiColors.Blue}╚══════════════════════════════════════╝{AnsiColors.Reset}");
        Console.WriteLine();

        var game = new Game();
        var player = game.CreatePlayer("Player1");

        Console.WriteLine(game.ProcessCommand(player, "look"));
        

        while (!_exitEvent.WaitOne(0)) // Check if exit has been signaled
        {
            Console.Write($"{AnsiColors.Yellow}> {AnsiColors.Reset}");
            var command = Console.ReadLine();
            if (command?.ToLower() == "quit")
                break;

            var result = game.ProcessCommand(player, command);
            Console.WriteLine(result);
        }

        Console.WriteLine("Shutting down...");
    }
} 