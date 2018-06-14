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
                    var test = new Label {Text = "" + list[count]};
                    grid.Children.Add(test, j, i);
                    count++;
                }
            }

            return grid;

        }

        //Gera a lista com todos os valores de distribuição normal calculados com precisão de 6 casas decimais
        public List<decimal> CalculateNormalDistribuitionValues()
        {
            double zScore;
            var list = new List<decimal>();
            var oldzscore = 0.0;
            for (zScore = 0; zScore < 3*Math.PI; zScore += 0.01*Math.PI)
            {
                var result = CalculateNormalDistribuition(oldzscore,zScore);
                list.Add((decimal)Math.Round(result, 6));
                oldzscore = zScore;
            }

            return list;
        }

        //Calcula um valor especifico de distribuição normal multiplicando a constante pela integral da série
        private decimal CalculateNormalDistribuition(double init,double z)
        {

            //var firstPart = (1 / Math.Sqrt(2 * Math.PI));
            var integrate = SimpsonRule.IntegrateComposite(x => CalcSomatorio(x,15), init, z, 10000);
            //var result = firstPart * integrate;
            decimal result = (decimal)integrate;
            result = decimal.Parse(result.ToString(), NumberStyles.Float);
          
            return result;
        }


        //Função que calcula o somatório com o numero de termos estabelecido
        private double CalcSomatorio(double x, double terms)
        {
            double somatorio = 0;

            for (var n = 0; n <= terms; n++)
            {
                var alternateValue = Math.Pow(-1, n);
                var numerador = (Math.Pow(x, 2 * n)) * alternateValue;
                //var firstPartDivision = (Math.Pow(2, n));
                //var secondPartDivision = SpecialFunctions.Factorial(n);
                //var denominador = firstPartDivision * secondPartDivision;
                var denominador = SpecialFunctions.Factorial(2 * n + 1);
                var ztf = numerador / denominador;
                
                somatorio += ztf;
            }


            return somatorio;
        }

    }
}
