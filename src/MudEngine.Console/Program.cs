using System;
using MudEngine;

namespace MudEngine.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.TreatControlCAsInput = true;

        var game = new Game();
        var player = game.CreatePlayer("Player1");

        Console.WriteLine($"{AnsiColors.Blue}Welcome to the MUD!{AnsiColors.Reset}");
        Console.WriteLine(game.ProcessCommand(player, "look"));

        while (true)
        {
            Console.Write($"{AnsiColors.Yellow}> {AnsiColors.Reset}");
            var command = Console.ReadLine();
            if (command?.ToLower() == "quit")
                break;

            var result = game.ProcessCommand(player, command);
            Console.WriteLine(result);
        }
    }
} 