using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using MathNet.Numerics.Distributions;
using System.Data;
using Newtonsoft.Json;


namespace WebApplication1.Pages
{
    public class Index4Model : PageModel
    {
        public bool HasValue = false;

        [BindProperty(SupportsGet = true)]

        public double underlyingPrice { get; set; }
        [BindProperty(SupportsGet = true)]


        public double strikePrice { get; set; }
        [BindProperty(SupportsGet = true)]

        public double riskFreeRate { get; set; }
        [BindProperty(SupportsGet = true)]

        public double timeToMaturity { get; set; }

        [BindProperty(SupportsGet = true)]


        public double volatility { get; set; }

        public double d1 { get; set; }
        public double d2 { get; set; }
        public double delta { get; set; }

        public double gamma { get; set; }

        public double vega { get; set; }
        public double theta { get; set; }
        public double rho { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Option { get; set; }

        public double callPrice { get; set; }
        public double putPrice { get; set; }

        public void OnGet()
        {
            //var option = Request.Form["option"];

            Normal normalDistribution = new Normal();

            d1 = (Math.Log(underlyingPrice / strikePrice) + (riskFreeRate + Math.Pow(volatility, 2) / 2) * timeToMaturity) / (volatility * Math.Sqrt(timeToMaturity));
            d2 = d1 - volatility * Math.Sqrt(timeToMaturity);
            d1 = Math.Round(d1, 3);
            d2 = Math.Round(d2, 3);

            if (Option == "call")
            {
                delta = normalDistribution.CumulativeDistribution(d1);
                delta = Math.Round(delta, 3);
                rho = strikePrice * timeToMaturity * Math.Exp(-riskFreeRate * timeToMaturity) * normalDistribution.CumulativeDistribution(d2);
                rho = Math.Round(rho, 3);
            }
            else if (Option == "put")
            {
                delta = normalDistribution.CumulativeDistribution(d1) - 1;
                delta = Math.Round(delta, 3);
                rho = -strikePrice * timeToMaturity * Math.Exp(-riskFreeRate * timeToMaturity) * normalDistribution.CumulativeDistribution(d2);
                rho = Math.Round(rho, 3);

            }

            gamma = normalDistribution.Density(d1) / (underlyingPrice * volatility * Math.Sqrt(timeToMaturity));
            gamma = Math.Round(gamma, 3);
            vega = underlyingPrice * normalDistribution.Density(d1) * Math.Sqrt(timeToMaturity);
            vega = Math.Round(vega, 3);
            theta = -(underlyingPrice * normalDistribution.Density(d1) * volatility) / (2 * Math.Sqrt(timeToMaturity)) - riskFreeRate * strikePrice * Math.Exp(-riskFreeRate * timeToMaturity) * normalDistribution.CumulativeDistribution(d2);
            theta = Math.Round(theta, 3);
            callPrice = underlyingPrice * normalDistribution.CumulativeDistribution(d1) - strikePrice * Math.Exp(-riskFreeRate * timeToMaturity) * normalDistribution.CumulativeDistribution(d2);
            callPrice = Math.Round(callPrice, 3);

            putPrice = strikePrice * Math.Exp(-riskFreeRate * timeToMaturity) * normalDistribution.CumulativeDistribution(-d2) - underlyingPrice * normalDistribution.CumulativeDistribution(-d1);
            putPrice = Math.Round(putPrice, 3);
            double[] prices = new double[] { callPrice, putPrice };

            if (d1 != 0 && d2 != 0)
            {

                HasValue = true;
            }

            //Creating chart data
            var chartData = new
            {
                labels = new[] { "Call Option", "Put Option" },
                datasets = new[]
                {
        new
        {
            data = prices,
            backgroundColor = new[] { "#3e95cd", "#8e5ea2" }
        }
    }
            };

            //Creating chart options
            var chartOptions = new
            {
                title = new
                {
                    display = true,
                    text = "Option Prices"
                },
                legend = new
                {
                    display = false
                }
            };

            //Creating chart object
            var chart = new
            {
                type = "bar",
                data = chartData,
                options = chartOptions
            };

            //Converting chart object to JSON format
            var chartJson = JsonConvert.SerializeObject(chart);

            //Passing chartJson to the view
            ViewData["chartData"] = chartJson;
        }
    }
}





