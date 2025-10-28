using System;

namespace TarjetaSube
{
    public class TarjetaMedioBoleto : Tarjeta
    {
        public TarjetaMedioBoleto() : base()
        {
        }

        public override decimal ObtenerTarifa(decimal tarifaBase)
        {
            return tarifaBase / 2;
        }
    }
}