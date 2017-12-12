using System;
using System.Collections.Generic;
using MathNet.Numerics;
using MathNet.Numerics.Integration;
using Xamarin.Forms;

namespace NormalDistribution
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            Content = BuildGrid(CalculateNormalDistribuitionValues());
        }

        public Grid BuildGrid(List<double> list)
        {
            var grid = new Grid() { };

            for (var i = 0; i < 41; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
            }

            for (var i = 0; i < 11; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(1, GridUnitType.Star)});
                if (i <= 0) continue;
                var topName = new Label {Text = "" + (i - 1)};
                grid.Children.Add(topName, i, 0);
            }

            var pos = 0.0;
            for (var i = 1; i < 41; i++)
            {
                var leftName = new Label {Text = "" + pos};
                pos += 0.1;
                grid.Children.Add(leftName, 0, i);
            }

            var count = 0;

            for (var i = 1; i < grid.RowDefinitions.Count; i++)
            {
                for (var j = 1; j < grid.ColumnDefinitions.Count; j++)
                {
                    if (list.Count == count) return grid;
                    var test = new Label {Text = "" + list[count]};
                    grid.Children.Add(test, j, i);
                    count++;
                }
            }

            return grid;

        }


        public List<double> CalculateNormalDistribuitionValues()
        {
            double zScore;
            var list = new List<double>();
            for (zScore = 0; zScore < 3.99; zScore += 0.01)
            {
                var result = CalculateNormalDistribuition(zScore);
                list.Add(Math.Round(result, 6));
            }

            return list;
        }

        private double CalculateNormalDistribuition(double z)
        {

            var firstPart = (1 / Math.Sqrt(2 * Math.PI));
            var aproximatte = SimpsonRule.IntegrateComposite(x => CalcSomatorio(x,27), 0, z, 10000);
            var result = firstPart * aproximatte;

            return result;
        }

        private double CalcSomatorio(double x, double terms)
        {
            double somatorio = 0;

            for (var n = 0; n <= terms; n++)
            {
                var alternateValue = Math.Pow(-1, n);
                var numerador = (Math.Pow(x, 2 * n)) * alternateValue;
                var firstPartDivision = (Math.Pow(2, n));
                var secondPartDivision = SpecialFunctions.Factorial(n);
                var denominador = firstPartDivision * secondPartDivision;
                var ztf = numerador / denominador;
                somatorio += ztf;
            }

            return somatorio;
        }

    }
}
