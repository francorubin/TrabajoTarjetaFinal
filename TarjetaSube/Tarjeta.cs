using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TarjetaSube
{
    public class Tarjeta
    {
        protected decimal saldo;
        private const decimal LIMITE_SALDO_MAXIMO = 40000m;
        protected const decimal LIMITE_SALDO_NEGATIVO = -1200m;
        private static readonly decimal[] CargasPermitidas = { 2000, 3000, 4000, 5000, 8000, 10000, 15000, 20000, 25000, 30000 };

        public decimal Saldo
        {
            get { return saldo; }
        }

        public Tarjeta()
        {
            saldo = 0;
        }

        public bool Cargar(decimal monto)
        {
            // Validar que el monto esté en la lista de cargas permitidas
            bool montoValido = false;
            foreach (decimal carga in CargasPermitidas)
            {
                if (carga == monto)
                {
                    montoValido = true;
                    break;
                }
            }

            if (!montoValido)
            {
                return false;
            }

            // Calcular el nuevo saldo
            decimal nuevoSaldo = saldo + monto;

            // Verificar que no supere el límite máximo
            if (nuevoSaldo > LIMITE_SALDO_MAXIMO)
            {
                return false;
            }

            // Acreditar la carga
            saldo = nuevoSaldo;
            return true;
        }

        public virtual bool DescontarSaldo(decimal monto)
        {
            decimal nuevoSaldo = saldo - monto;

            // Verificar que no supere el límite de saldo negativo
            if (nuevoSaldo < LIMITE_SALDO_NEGATIVO)
            {
                return false;
            }

            saldo = nuevoSaldo;
            return true;
        }

        public virtual decimal ObtenerTarifa(decimal tarifaBase)
        {
            return tarifaBase;
        }
    }
}