using System;

namespace TarjetaSube
{
    public class TarjetaFranquiciaCompleta : Tarjeta
    {
        public TarjetaFranquiciaCompleta() : base()
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