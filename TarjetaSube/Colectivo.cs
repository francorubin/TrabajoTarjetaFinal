using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSube
{
    public class Colectivo
    {
        private const decimal TARIFA_BASICA = 1580m;
        private string linea;

        public string Linea
        {
            get { return linea; }
        }

        public Colectivo(string linea)
        {
            this.linea = linea;
        }

        public Boleto? PagarCon(Tarjeta tarjeta)
{
    
    decimal tarifaAPagar = tarjeta.ObtenerTarifa(TARIFA_BASICA);
    
    if (tarjeta.DescontarSaldo(tarifaAPagar))
    {
        return new Boleto(tarifaAPagar, tarjeta.Saldo, this.linea);
    }
    return null;
}
        {
            if (tarjeta.DescontarSaldo(TARIFA_BASICA))
            {
                return new Boleto(TARIFA_BASICA, tarjeta.Saldo, this.linea);
            }
            return null;
        }
    }
}