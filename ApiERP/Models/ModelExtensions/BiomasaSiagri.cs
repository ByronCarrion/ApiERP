using System;

namespace ApiERP.Models.ModelExtensions
{
    public class BiomasaSiagri
    {
        public int NUMDOC { get; set; }
        public DateTime DATETIMEIN { get; set; }
        public DateTime DATETIMEOUT { get; set; }
        public string CONDUC { get; set; }
        public string PLACAB { get; set; }
        public Decimal BRUTO { get; set; }
        public Decimal TARA { get; set; }
        public Decimal NETO { get; set; }
        public int CODPROD { get; set; }

    }

}