using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSube
{
    public class TarjetaBoletoGratuito : Tarjeta
    {
        public TarjetaBoletoGratuito() : base()
        {
        }

        public override bool DescontarSaldo(decimal monto)
        {
            return true;
        }

        public override decimal ObtenerTarifa(decimal tarifaBase)
        {

            return 0;
        }
    }
}