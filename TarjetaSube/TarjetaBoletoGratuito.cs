using System;

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