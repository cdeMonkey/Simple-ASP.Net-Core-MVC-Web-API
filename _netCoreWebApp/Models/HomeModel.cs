﻿namespace _netCoreWebApp.Models
{
    public class HomeModel
    {
        public int RateVisitors { get; set; }
        public int RatePageViews { get; set; }
        public int RateUsers { get; set; }

        public int RateOrders { get; set; }

        public List<Tuple<int, string, string, string>> Users;

        public List<Tuple<int, string, string, string>> Orders;

        public List<Tuple<int, string, string, string>> Clients;

        public List<Tuple<int, DateTime, Double>> Invoices;

    }
}
