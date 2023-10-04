using Swashbuckle.Examples;
using System;
using System.Collections.Generic;

namespace ApiERP.Services
{
    public class ArticleExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<Object> {
                new { Codigo = "01 01 01 0001", Descripcion = "Agroquímico", UnidadMedida = "KGS", Tipo = "01 01", CostoUSD = 123.45 ,  FechaEstandar  = "2023-09-30 00:00:00" }
            };
        }
    }

    public class ArticleTypeExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<Object> {
                new { Codigo = "01 01", Descripcion = "Agroquímicos" },
                new { Codigo = "01 02", Descripcion = "Equipos Informáticos"}
            };
        }
    }

    public class ArticleStockExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<Object> {
                new { Codigo = "01 01 01 0001", CodBodega = 1, UnidadMedida = "KGS", StockActual = 1235.25, StockDisponible = 1235.25 ,CostoUSD = 123.45 , FechaEstandar  = "2023-09-30 00:00:00" }
            };
        }
    }

    public class ArticleStockCExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<Object> {
                new { Codigo = "01 01 01 0001", CodBodega = 1 , UnidadMedida = "KGS", StockActual = 12.25, StockDisponible = 12.25 , CostoUSD = 1234.5 , FechaEstandar  = "2023-09-30 00:00:00" }
            };
        }
    }

    public class CustomerExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<Object> {
                new { Codigo = 1, Nombre = "Azucarera de Nicaragua, S.A.", Ruc = "J000000000025", Activo = 1},
                new { Codigo = 2, Nombre = "Compañía Azucarera, S.A.", Ruc = "J000000000015", Activo = 1},
            };
        }
    }

    public class CCExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<Object> {
                new { Codigo = "1.1.1.1", Nombre = "Departamento de Informática" },
                new { Codigo = "1.1.1.2", Nombre = "Departamento de Compras"},
            };
        }
    }

    public class ProviderExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<Object> {
                new { Compania = 3, Codigo = 1, Nombre = "SINSA", Ruc = "J0230000000025", Tipo = "01", Estado = 1, TipoPropietario = 3 },
                new { Compania = 3, Codigo = 2, Nombre = "Comtech", Ruc = "J0280000000027", Tipo = "01", Estado = 1, TipoPropietario = 3 },
            };
        }
    }

    public class ProviderTypeExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<Object> {
                new { Compania = 3, Codigo = "01", Descripcion = "Ferreterías" },
                new { Compania = 3, Codigo = "02", Descripcion = "Tecnologías" },
            };
        }
    }

    public class EmployeeExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<Object> {
                new { Compania = 3, Codigo = "2538", Nombre = "Benito Jesús Valverde Torres", Dni = "0012104860014A", Activo = 1, FechaNacimiento = "21/04/1986", Sexo=1,Profesion=1, Funcion = "1", Planta = "01" ,TURMA = 72578 },
                new { Compania = 3, Codigo = "01", Nombre = "Norman Felipe Cruz", Dni = "0012104860014A", Activo = 1, FechaNacimiento = "21/04/1986", Sexo=1,Profesion=1, Funcion = "1", Planta = "01",TURMA = 72578 },
            };
        }
    }

    public class EmployeeProofExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<Object> {
                new { Compania = 3, Codigo = 1, Descripcion = "Benito Jesús Valverde Torres"},
                new { Compania = 3, Codigo = 2, Descripcion = "Norman Felipe Cruz"},
            };
        }
    }

    public class RequisaExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<Object> {
                new {
                    REQCOMPANYID = 1,
                    REQUISAID = 50,
                    REQBODEGAID = 1,
                    REQEMPID = 0,
                    REQTOPMNGRID = 3,
                    REQCOMPSCTID = 1,
                    REQCLASSID = 52,
                    REQLOT = "03000",
                    REQACTIVITY = "1",
                    REQOT = "50",
                    REQUISASTATUS="B",
                    REQUISADATE = DateTime.Now,
                    REQFORDATE = DateTime.Now,
                    REQMEMO = "Prueba de Requisa",
                    REQADDWHO = "marias",
                    REQADDIP = "127.0.0.1",
                    REQADDDATE= DateTime.Now,
                    REQUPDWHO = "marias",
                    REQUPDDATE= DateTime.Now,
                    REQUPDIP = "127.0.0.1",
                    REQAPROBY = "marias",
                    REQAPRODATE= DateTime.Now,
                    REQAPROIP = "127.0.0.1",
                    REQCLOSEBY = "marias",
                    REQCLOSEDATE= DateTime.Now,
                    REQCLOSEIP = "127.0.0.1",
                    Detalle = new List<Object>{
                                    new {
                                        REQDETID = 1,
                                        REQDETIDPRODMCS = "01 02 00 0019",
                                        REQDETPRODDESC = "TUBO ACERO INOXIDABLE 304 C/COSTURA ",
                                        REQDETUMMCS = "UND",
                                        REQDETMEMO = "",
                                        REQDETSTOCKORD = 10,
                                        REQDETSTOCKGET = 10,
                                        REQDETCOSTTRAN = 10,
                                        REQDETADDWHO = "marias",
                                        REQDETDDDATE = "2022-09-19 15:27:59",
                                        REQDETADDIP = "127.0.0.1"
                                    },
                                    new {
                                        REQDETID = 1,
                                        REQDETIDPRODMCS = "01 02 09 0011",
                                        REQDETPRODDESC = "CABLE 8 AWG 600V,THHN VERDE",
                                        REQDETUMMCS = "UND",
                                        REQDETMEMO = "",
                                        REQDETSTOCKORD = 15,
                                        REQDETSTOCKGET = 10,
                                        REQDETCOSTTRAN = 10,
                                        REQDETADDWHO = "marias",
                                        REQDETDDDATE = "2022-09-19 15:27:59",
                                        REQDETADDIP = "127.0.0.1"
                                    },
                                }
                },
            };
        }
    }


    public class BiomasaExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<object>
            {
                new
                {
                    NUMDOC         = 3,
                    DATETIMEIN     =  "2022-09-20 16:52:30",
                    DATETIMEOUT    = "2022-09-20 16:52:30",
                    CONDUC         = "Jose Alberto Perez Lopez",
                    PLACAB         = "M20209",
                    BRUTO          = 40.00,
                    TARA           = 20.00,
                    NETO           = 20.00,
                    CODPROD        = 1
                },
            };
        }
    }

    public class NominaExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new List<object>
            {
                new
                {
                    NombreArchivo   = "ARCHIVO NOMINA",
                    File            = ""
        },
            };
        }
    }
}