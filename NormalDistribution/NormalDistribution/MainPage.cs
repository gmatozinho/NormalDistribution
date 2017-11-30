using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NormalDistribution
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            Content = BuildGrid(CalculateNormalDistribuition());
        }

        public Grid BuildGrid(List<double> list)
        {
            var grid = new Grid() { };

            for (var i = 0; i < 41; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (var i = 0; i < 11; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                if (i <= 0) continue;
                var topName = new Label {Text = "" + (i-1)};
                grid.Children.Add(topName, i, 0);
            }

            var pos = 0.0;
            for (var i = 1; i < 41; i++)
            {
                var leftName = new Label { Text = "" + pos};
                pos += 0.1;
                grid.Children.Add(leftName, 0, i);
            }

            var count = 0;

            for (var i = 1; i < grid.RowDefinitions.Count; i++)
            {
                for (var j = 1; j < grid.ColumnDefinitions.Count; j++)
                {
                    var test = new Label { Text = ""+list[count]};
                    grid.Children.Add(test,j,i);
                    count++;
                }
                
            }

            return grid;

        }



        public List<double> CalculateNormalDistribuition()
        {
            double zScore;
            var sumDistribuition = 0.0;
            var list = new List<double>();
            
            for (zScore=0; zScore < 3.99; zScore+=0.01)
            {
                list.Add(Math.Round(sumDistribuition,4));
                var zResult = -(Math.Pow(zScore,2)) / 2;
                var firstPart = (1 / Math.Sqrt(2 * Math.PI));
                var secondPart = (Math.Exp(zResult));
                var normDist =  (firstPart * secondPart)/100;
                sumDistribuition += normDist;
            }

            return list;
        }


    }

}

