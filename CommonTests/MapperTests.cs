using Common.Infs;
using Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Common.Tests
{
    [TestClass()]
    public class MapperTests
    {
        [TestMethod()]
        public void MapTest()
        {
            var fatura = new Fatura { Date = DateTime.Today,FaturaNo = "123"};

            var faturaDto = Mapper.Map<FaturaDto, Fatura>(fatura);
            Assert.IsNotNull(faturaDto);
            Assert.AreEqual(faturaDto.Date, fatura.Date);
        }
    }
}