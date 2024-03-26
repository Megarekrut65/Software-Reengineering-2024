using System;
using UnityEngine;
using Random = System.Random;

namespace Fight
{
    public class GameResult
    {
        public int newPoints;
        public int coins;

        public string GetString()
        {
            string line = "";
            if(newPoints < 0)
            {
                line += "You lose ";
            }
            else
            {
                line += "You win ";
            }
            line += Math.Abs(newPoints).ToString();
            line += " points and you get " + coins.ToString() + " coins.";

            return line;
        }
        public void WriteResult()
        {
            PlayerPrefs.SetInt("ResultCoins", coins);
            PlayerPrefs.SetInt("ResultPoints", newPoints);
        }
        public void ReadResult()
        {
            coins = PlayerPrefs.GetInt("ResultCoins", 0);
            newPoints = PlayerPrefs.GetInt("ResultPoints", 0);
        }
        public GameResult(int newPoints = 0, int coins= 0) 
        {
            this.newPoints = newPoints;
            this.coins = coins;
        }
        public static int CountCoins(bool isWin, int coefficient, int points)
        {
            Random rnd = new Random();
            if(isWin) 
            {
                return (75 + coefficient * rnd.Next(0, 2*points + 1))/4;
            }
            return (35 + coefficient * rnd.Next(0, 2*points + 1))/8;
        }
        public static GameResult CountWin(int pointsWin, int points, int level = 0)
        {
            int coefficient = 0;
            if(level <= 8) coefficient = 3;
            else coefficient = 2;
            int newPoints = 15;
            if(pointsWin <= points)
            {
                newPoints += (points - pointsWin)/2 + coefficient * 4;
            }
            else
            {
                newPoints += (pointsWin - points)/3 + coefficient * 4;
            }
            return new GameResult(newPoints, CountCoins(true, coefficient, Math.Abs(newPoints)));
        }
        public static GameResult CountLose(int pointsLose, int points, int level = 0)
        {
            int coefficient = 0;
            if(level > 8) coefficient = 2;
            else coefficient = 1;
            int newPoints = -10;
            if(pointsLose <= points)
            {
                newPoints -= ((points - pointsLose)/3 + 6/coefficient);
            }
            else
            {
                newPoints -= ((pointsLose - points)/2 + 8/coefficient);
            }
            return new GameResult(newPoints, CountCoins(false, coefficient, Math.Abs(newPoints)));
        }
    }
}
