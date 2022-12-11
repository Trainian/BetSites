using NUnit.Framework;
using System;
using WPF.Converters;
using Moq;
using WPF.Interfaces;
using ApplicationCore.Repositories;
using ApplicationCore.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WPF.Services;

namespace Test
{
    public class StatisticTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void GetPercentChangedWiners_InputDbContex_ReturnEquel()
        {
            
        }

        private List<Bet> GetBets (int count)
        {
            return new List<Bet>();
        }

    }
}