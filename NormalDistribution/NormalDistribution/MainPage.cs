using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MathNet.Numerics;
using Xamarin.Forms;

namespace NormalDistribution
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            Main();
            Test();
            Content = BuildGrid(list);
        }

        static List<double> list = new List<double>();

        private static void TestApproximationMethods(DefiniteIntegral integral, int subdomainCount)
        {

           list.Add(integral.Approximate(DefiniteIntegral.ApproximationMethod.RectangleMidpoint, subdomainCount));
        }

        public static void Main()
        {
            TestApproximationMethods(new DefiniteIntegral(x => x * x * x, new Interval(0, 1)), 10000);
            TestApproximationMethods(new DefiniteIntegral(x => 1 / x, new Interval(1, 99)), 1000);
            TestApproximationMethods(new DefiniteIntegral(x => x, new Interval(0, 5000)), 500000);
            TestApproximationMethods(new DefiniteIntegral(x => x, new Interval(0, 6000)), 6000000);
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
                    if (list.Count == count) return grid;
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
                list.Add(Math.Round(sumDistribuition,6));
                var zResult = -(Math.Pow(zScore,2)) / 2;
                var firstPart = (1 / Math.Sqrt(2 * Math.PI));
                var secondPart = (Math.Exp(zResult));
                var normDist =  (firstPart * secondPart)/100;
                sumDistribuition += normDist;
            }

            return list;
        }


        private void Test()
        {
            var firstPart = (1 / Math.Sqrt(2 * Math.PI));

            var sum = 0.0;
            var n = 6;
            var r1 = Math.Pow(-1.0, n);
            var r2 = 2 * n;
            var r3 = 2 * n * (fatorial(n));

            var integral = new DefiniteIntegral(x => r1 * (Math.Pow(x,r2)/r3), new Interval(0.0, 0.01));
            var aproximatte = integral.Approximate(DefiniteIntegral.ApproximationMethod.RectangleMidpoint, 10000);
            var result = firstPart * aproximatte;
            var result2 = result;

        }

        public static Int32 fatorial(Int32 num)
        {
            if (num == 1)
            {
                return 1;
            }
            // Poderia colocar um else, porem como eh usado o return, ele nao eh mais necessario
            return num * fatorial(num - 1);
        }


    }
    public class Interval
    {
        public Interval(double leftEndpoint, double size)
        {
            LeftEndpoint = leftEndpoint;
            RightEndpoint = leftEndpoint + size;
        }

        public double LeftEndpoint
        {
            get;
            set;
        }

        public double RightEndpoint
        {
            get;
            set;
        }

        public double Size
        {
            get
            {
                return RightEndpoint - LeftEndpoint;
            }
        }

        public double Center
        {
            get
            {
                return (LeftEndpoint + RightEndpoint) / 2;
            }
        }

        public IEnumerable<Interval> Subdivide(int subintervalCount)
        {
            double subintervalSize = Size / subintervalCount;
            return Enumerable.Range(0, subintervalCount).Select(index => new Interval(LeftEndpoint + index * subintervalSize, subintervalSize));
        }
    }

    public class DefiniteIntegral
    {
        public DefiniteIntegral(Func<double, double> integrand, Interval domain)
        {
            Integrand = integrand;
            Domain = domain;
        }

        public Func<double, double> Integrand
        {
            get;
            set;
        }

        public Interval Domain
        {
            get;
            set;
        }

        public double SampleIntegrand(ApproximationMethod approximationMethod, Interval subdomain)
        {
            switch (approximationMethod)
            {
                case ApproximationMethod.RectangleLeft:
                    return Integrand(subdomain.LeftEndpoint);
                case ApproximationMethod.RectangleMidpoint:
                    return Integrand(subdomain.Center);
                case ApproximationMethod.RectangleRight:
                    return Integrand(subdomain.RightEndpoint);
                case ApproximationMethod.Trapezium:
                    return (Integrand(subdomain.LeftEndpoint) + Integrand(subdomain.RightEndpoint)) / 2;
                case ApproximationMethod.Simpson:
                    return (Integrand(subdomain.LeftEndpoint) + 4 * Integrand(subdomain.Center) + Integrand(subdomain.RightEndpoint)) / 6;
                default:
                    throw new NotImplementedException();
            }
        }

        public double Approximate(ApproximationMethod approximationMethod, int subdomainCount)
        {
            return Domain.Size * Domain.Subdivide(subdomainCount).Sum(subdomain => SampleIntegrand(approximationMethod, subdomain)) / subdomainCount;
        }

        public enum ApproximationMethod
        {
            RectangleLeft,
            RectangleMidpoint,
            RectangleRight,
            Trapezium,
            Simpson
        }
    }

}

