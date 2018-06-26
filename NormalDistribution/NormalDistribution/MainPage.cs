using System;
using System.Collections.Generic;
using System.Globalization;
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

        public Grid BuildGrid(List<decimal> list)
        {
            var grid = new Grid() { };

            for (var i = 0; i < 32; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(1, GridUnitType.Star)});
            }

            for (var i = 0; i < 11; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(1, GridUnitType.Star)});
                if (i <= 0) continue;
                var topName = new Label {Text = "" + (i - 1)+ "π" };
                grid.Children.Add(topName, i, 0);
            }

            var pos = 0.0;
            for (var i = 1; i < 32; i++)
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
                    Label label = new Label { Text = "" + list[count] };
                    
                   
                    grid.Children.Add(label, j, i);
                    count++;
                }
            }

            return grid;

        }

        //Gera a lista com todos os valores da integral de sin(x)/x calculados com precisão de 6 casas decimais
        public List<decimal> CalculateNormalDistribuitionValues()
        {
            double point;
            decimal result;
            double sum=0;
            var list = new List<decimal>();
            
            for (point = 0.0 * Math.PI; point < 3*Math.PI; point += 0.01*Math.PI)
            {
                sum = CalcSomatorio(point,20);
                result = decimal.Parse(sum.ToString(), NumberStyles.Float);
                list.Add(Math.Round(result, 6));                
            }

            return list;
        }

        private double CalcSomatorio(double x, double terms)
        {
            double somatorio = 0;

            for (var n = 0; n <= terms; n++)
            {
                var alternateValue = Math.Pow(-1, n);
                var numerador = (Math.Pow(x, 2 * n+1)) * alternateValue;
                var denominador = (2 * n + 1) * SpecialFunctions.Factorial(2 * n + 1);
                var result = numerador / denominador;
                
                somatorio += result;
            }


            return somatorio;
        }

    }
}
