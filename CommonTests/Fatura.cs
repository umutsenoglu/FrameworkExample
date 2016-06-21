using Common;
using System;

namespace Dto
{
    public class Fatura : IEntity
    {
        public Fatura()
        {
        }

        public Guid Id
        {
            get;

            set;
        }

        public string FaturaNo { get; set; }
        public DateTime Date { get; set; }
    }
}
