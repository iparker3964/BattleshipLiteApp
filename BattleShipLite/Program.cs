using BattleshipLiteLibrary;
using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipLite
{
    public class Program
    {
        /*UI-specific code should not live in a class library unless the library is for that UI type.*/
        static void Main(string[] args)
        {
            Welcome();

            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;

            do
            {
                //Display grid from activePlayer on where they fired
                DisplayShotGrid(activePlayer);

                //Ask activePlayer for a shot
                //Determine if it is a valid shot
                //Determine shot results
                RecordPlayerShot(activePlayer,opponent);

                //Determine if the game should continue
                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);
                
                //If over set activePlayer as the winner
                //else swap positions to (active player becomes opponent) tuple - allows for two types together
                if(doesGameContinue == true)
                {
                    //Swap using a temp variable
                    //PlayerInfoModel tempHolder = activePlayer;
                    //activePlayer = opponent;
                    //opponent = tempHolder;
                    
                    //Swap using a tuple - happens at the sametime
                    (activePlayer,opponent) = (opponent,activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }
            } while (winner == null);

            IdentifyWinner(winner);
            Console.ReadLine();
        }

        private static void IdentifyWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations to {winner.UserName} for winning!");
            Console.WriteLine($"{winner.UserName} took {GameLogic.GetShotCount(winner)} shots.");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            string row = "";
            int col = 0;
            //Asks for shot
            //? B2 or B then 2
            //Determine what row and column that is - split it apart substring
            //Determine if that is a valid shot
            //Go back to the beginning if not a valid shot

            bool isValidShot = false;

            do
            {
                string shot = AskForShot(activePlayer);
                try
                {
                    (row, col) = GameLogic.SplitShotIntoRowAndColumn(shot);
                    isValidShot = GameLogic.ValidShot(row, col, activePlayer);
                }
                catch (Exception ex)
                {

                    isValidShot = false;
                }


                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid shot selection. Please try again.");
                }
            } while (isValidShot == false);
            //Determine shot results - hit or no
            bool isAHit = GameLogic.IdentifyShotResult(opponent,row,col);

            //Record results
            GameLogic.MarkShotResult(activePlayer,row,col,isAHit);

            DisplayShotResults(row,col,isAHit);
        }

        private static void DisplayShotResults(string row, int col, bool isAHit)
        {
            if (isAHit)
            {
                Console.WriteLine($"{row}{col} is a hit!");
            }
            else
            {
                Console.WriteLine($"{row}{col} is a miss!");
            }

            Console.WriteLine();
        }

        private static string AskForShot(PlayerInfoModel player)
        {
            Console.WriteLine();
            Console.WriteLine(new string('-',10));
            Console.Write($"{player.UserName} please enter your shot selection: ");
            string output = Console.ReadLine();

            return output;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currChar = activePlayer.ShotGrid[0].SpotLetter;

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (!currChar.Equals(gridSpot.SpotLetter))
                {
                    currChar = gridSpot.SpotLetter;
                    Console.WriteLine();
                }

                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($"{gridSpot.SpotLetter}{gridSpot.SpotNum}");
                }else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write("X ");
                }
                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write("O ");
                }
                else
                {
                    Console.Write("? ");
                }
            }
        }

        private static void Welcome()
        {
            Console.WriteLine("Welcome to Battleship Lite");
            Console.WriteLine("Created by Isaiah Parker");
            Console.WriteLine();
        }
        private static PlayerInfoModel CreatePlayer(string title)
        {
            Console.WriteLine($"Player information for {title}");
            //Ask user for their name
            //Load up the shot grid
            //Ask the user for their 5 ship placements
            //clear
            PlayerInfoModel player = new PlayerInfoModel();
            player.UserName = ConsoleMsgResult("What is the player name? ");

            GameLogic.InitializeGrid(player);

            PlaceShips(player);

            Console.Clear();

            return player;
        }

        private static void PlaceShips(PlayerInfoModel player)
        {
            do
            {
                string location = ConsoleMsgResult($"Where do you want to place ship {player.ShipLocations.Count + 1} ");
                bool isValid = false;

                try
                {
                    isValid = GameLogic.PlaceShip(player, location);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error: " + ex);
                }

                if (isValid == false)
                {
                    Console.WriteLine($"Location {location} is not valid. Please try again.");
                }
            } while (player.ShipLocations.Count < 5);
        }

        private static string ConsoleMsgResult(string msg)
        {
            string output = "";

            Console.Write(msg);

            return output = Console.ReadLine();
        }
    }
}
