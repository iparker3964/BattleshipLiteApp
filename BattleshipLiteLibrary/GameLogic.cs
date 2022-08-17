using BattleshipLiteLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipLiteLibrary
{
    //Class is static because it does not hold or store data
    public static class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel model)
        {
            List<string> letters = new List<string>
            {
                "A",
                "B",
                "C",
                "D",
                "E"
            };

            List<int> nums = new List<int>
            {
                1,
                2,
                3,
                4,
                5
            };

            foreach (var letter in letters)
            {
                foreach (var num in nums) {
                    AddGridSpot(model,letter,num);
                }
            }
        }
        private static void AddGridSpot(PlayerInfoModel model, string letter, int num)
        {
            GridSpotModel spot = new GridSpotModel
            {
                SpotLetter = letter,
                SpotNum = num,
                Status = GridSpotStatus.Empty
            };
            model.ShotGrid.Add(spot);
        }

        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            bool isActive = false;

            foreach (var ship in player.ShipLocations)
            {
                if (ship.Status != GridSpotStatus.Sunk)
                {
                    isActive = true;
                }
            }

            return isActive;
        }

        public static bool PlaceShip(PlayerInfoModel ShipLocationList, string location)
        {
            bool output = false;

            (string row, int col) = SplitShotIntoRowAndColumn(location);

            bool isVaidLocation = ValidGridLocation(ShipLocationList, row,col);
            bool isSpotOpen = ValidateShipLocation(ShipLocationList, row,col);

            if (isVaidLocation && isSpotOpen)
            {
                ShipLocationList.ShipLocations.Add(
                    new GridSpotModel
                    { 
                        SpotLetter = row.ToUpper(),
                        SpotNum = col,
                        Status = GridSpotStatus.Ship
                    }
                );
                output = true;
            }

            return output;
        }

        private static bool ValidateShipLocation(PlayerInfoModel model, string row, int col)
        {
            bool isValidLocation = true;

            foreach (var ship in model.ShipLocations)
            {
                if (ship.SpotLetter.Equals(row.ToUpper()) && ship.SpotNum == col)
                {
                    isValidLocation = false;
                }
            }

            return isValidLocation;
        }

        private static bool ValidGridLocation(PlayerInfoModel model, string row, int col)
        {
            bool isValidLocation = false;

            foreach (var ship in model.ShotGrid)
            {
                if (ship.SpotLetter.Equals(row.ToUpper()) && ship.SpotNum == col)
                {
                    isValidLocation = true;
                }
            }

            return isValidLocation;
        }

        public static int GetShotCount(PlayerInfoModel player)
        {
            int count = 0;

            foreach (var shot in player.ShotGrid)
            {
                if (shot.Status != GridSpotStatus.Empty)
                {
                    count++;
                }
            }

            return count;
        }

        public static (string row, int col) SplitShotIntoRowAndColumn(string shot)
        {
            string row;
            int col;

            if (shot.Length != 2)
            {
                throw new ArgumentException("This was an invalid shot type.","shot");
            }

            char[] shotArray = shot.ToArray();

            row = shotArray[0].ToString();
            col = int.Parse(shotArray[1].ToString());

            return (row,col);
        }

        public static bool ValidShot(string row, int col, PlayerInfoModel playerShotGridList)
        {
            bool isValid = false;

            foreach (var item in playerShotGridList.ShotGrid)
            {
                if (item.SpotLetter.Equals(row.ToUpper()) && item.SpotNum == col)
                {
                    if (item.Status == GridSpotStatus.Empty)
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int col)
        {
            bool isAhit = false;

            foreach (var ship in opponent.ShipLocations)
            {
                if (ship.SpotLetter.Equals(row.ToUpper()) && ship.SpotNum == col)
                {
                    isAhit = true;
                    ship.Status = GridSpotStatus.Sunk;
                }
            }

            return isAhit;

        }

        public static void MarkShotResult(PlayerInfoModel player, string row, int col, bool isAHit)
        {
            foreach (var gridSpot in player.ShotGrid)
            {
                if (gridSpot.SpotLetter == row.ToUpper() && gridSpot.SpotNum == col)
                {
                    if (isAHit)
                    {
                        gridSpot.Status = GridSpotStatus.Hit;
                    }
                    else
                    {
                        gridSpot.Status = GridSpotStatus.Miss;
                    }
                }
            }
        }
    }
}
