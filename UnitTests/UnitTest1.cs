using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SieveOfEratosthenes.ViewModel;
using System.Collections.ObjectModel;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            PrimesViewModel pViewModel = new PrimesViewModel();
            pViewModel.TxtNumberBox = "2";
            pViewModel.BtnFindPrimesCommand.Execute(pViewModel.TxtNumberBox);
            Assert.IsTrue(pViewModel.LstBxPrimeNumbers.Count == 0);
        }
    }
}
